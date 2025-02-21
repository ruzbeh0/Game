// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.RenderPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using Colossal.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Rendering/", new System.Type[] {})]
  public class RenderPrefab : RenderPrefabBase
  {
    [SerializeField]
    private AssetReference<GeometryAsset> m_GeometryAsset;
    [SerializeField]
    private AssetReference<SurfaceAsset>[] m_SurfaceAssets;
    [SerializeField]
    private Bounds3 m_Bounds;
    [SerializeField]
    private float m_SurfaceArea;
    [SerializeField]
    private int m_IndexCount;
    [SerializeField]
    private int m_VertexCount;
    [SerializeField]
    private int m_MeshCount;
    [SerializeField]
    private bool m_IsImpostor;
    [SerializeField]
    private bool m_ManualVTRequired;
    private Material[] m_MaterialsContainer;

    public bool hasGeometryAsset => this.m_GeometryAsset != (AssetReference<GeometryAsset>) null;

    public GeometryAsset geometryAsset
    {
      get => (GeometryAsset) this.m_GeometryAsset;
      set => this.m_GeometryAsset = (AssetReference<GeometryAsset>) value;
    }

    public IEnumerable<SurfaceAsset> surfaceAssets
    {
      get
      {
        return ((IEnumerable<AssetReference<SurfaceAsset>>) this.m_SurfaceAssets).Select<AssetReference<SurfaceAsset>, SurfaceAsset>((Func<AssetReference<SurfaceAsset>, SurfaceAsset>) (x => (SurfaceAsset) x));
      }
      set
      {
        this.m_SurfaceAssets = value.Select<SurfaceAsset, AssetReference<SurfaceAsset>>((Func<SurfaceAsset, AssetReference<SurfaceAsset>>) (x => new AssetReference<SurfaceAsset>(x.guid))).ToArray<AssetReference<SurfaceAsset>>();
      }
    }

    public Bounds3 bounds
    {
      get => this.m_Bounds;
      set => this.m_Bounds = value;
    }

    public float surfaceArea
    {
      get => this.m_SurfaceArea;
      set => this.m_SurfaceArea = value;
    }

    public int indexCount
    {
      get => this.m_IndexCount;
      set => this.m_IndexCount = value;
    }

    public int vertexCount
    {
      get => this.m_VertexCount;
      set => this.m_VertexCount = value;
    }

    public int meshCount
    {
      get => this.m_MeshCount;
      set => this.m_MeshCount = value;
    }

    public bool isImpostor
    {
      get => this.m_IsImpostor;
      set => this.m_IsImpostor = value;
    }

    public bool manualVTRequired
    {
      get => this.m_ManualVTRequired;
      set => this.m_ManualVTRequired = value;
    }

    public int materialCount => this.m_SurfaceAssets.Length;

    public SurfaceAsset GetSurfaceAsset(int index) => (SurfaceAsset) this.m_SurfaceAssets[index];

    public Mesh[] ObtainMeshes()
    {
      ComponentBase.baseLog.TraceFormat((UnityEngine.Object) this, "ObtainMeshes {0}", (object) this.name);
      return this.geometryAsset?.ObtainMeshes();
    }

    public Mesh ObtainMesh(int materialIndex, out int subMeshIndex)
    {
      ComponentBase.baseLog.TraceFormat((UnityEngine.Object) this, "ObtainMesh {0}", (object) this.name);
      subMeshIndex = materialIndex;
      if (this.hasGeometryAsset)
      {
        Mesh[] meshes = this.geometryAsset?.ObtainMeshes();
        if (meshes != null)
        {
          foreach (Mesh mesh in meshes)
          {
            if (materialIndex < mesh.subMeshCount)
            {
              subMeshIndex = materialIndex;
              return mesh;
            }
            materialIndex -= mesh.subMeshCount;
          }
        }
      }
      return (Mesh) null;
    }

    public void ReleaseMeshes()
    {
      ComponentBase.baseLog.TraceFormat((UnityEngine.Object) this, "ReleaseMeshes {0}", (object) this.name);
      this.geometryAsset?.ReleaseMeshes();
    }

    public Material[] ObtainMaterials(bool useVT = true)
    {
      ComponentBase.baseLog.TraceFormat((UnityEngine.Object) this, "ObtainMaterials {0}", (object) this.name);
      if (this.m_MaterialsContainer == null || this.m_MaterialsContainer.Length != this.materialCount)
        this.m_MaterialsContainer = new Material[this.materialCount];
      for (int index = 0; index < this.materialCount; ++index)
      {
        SurfaceAsset surfaceAsset = (SurfaceAsset) this.m_SurfaceAssets[index];
        this.m_MaterialsContainer[index] = surfaceAsset.Load(useVT: useVT);
      }
      return this.m_MaterialsContainer;
    }

    public Material ObtainMaterial(int i, bool useVT = true)
    {
      Material[] materials = this.ObtainMaterials(useVT);
      if (i < 0 || i >= materials.Length)
        throw new IndexOutOfRangeException(string.Format("i {0} is out of material range (length: {1}) in {2}", (object) i, (object) materials.Length, (object) this.name));
      return materials[i];
    }

    public void ReleaseMaterials()
    {
      ComponentBase.baseLog.TraceFormat((UnityEngine.Object) this, "ReleaseMaterials {0}", (object) this.name);
    }

    public void Release()
    {
      this.ReleaseMeshes();
      this.ReleaseMaterials();
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<MeshData>());
      components.Add(ComponentType.ReadWrite<SharedMeshData>());
      components.Add(ComponentType.ReadWrite<BatchGroup>());
      if (!this.isImpostor)
        return;
      components.Add(ComponentType.ReadWrite<ImpostorData>());
    }
  }
}
