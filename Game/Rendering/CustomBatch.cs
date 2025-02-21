// Decompiled with JetBrains decompiler
// Type: Game.Rendering.CustomBatch
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using Colossal.Rendering;
using Game.Prefabs;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Rendering
{
  public class CustomBatch : ManagedBatch
  {
    public SurfaceAsset sourceSurface { get; private set; }

    public Material sourceMaterial { get; private set; }

    public Material defaultMaterial { get; private set; }

    public Material loadedMaterial { get; private set; }

    public int sourceSubMeshIndex { get; private set; }

    public Entity sourceMeshEntity { get; private set; }

    public Entity sharedMeshEntity { get; private set; }

    public BatchFlags sourceFlags { get; private set; }

    public GeneratedType generatedType { get; private set; }

    public MeshType sourceType { get; private set; }

    public CustomBatch(
      int groupIndex,
      int batchIndex,
      SurfaceAsset sourceSurface,
      Material sourceMaterial,
      Material defaultMaterial,
      Material loadedMaterial,
      Mesh mesh,
      Entity meshEntity,
      Entity sharedEntity,
      BatchFlags flags,
      GeneratedType generatedType,
      MeshType type,
      int subMeshIndex,
      MaterialPropertyBlock customProps)
      : base(groupIndex, batchIndex, defaultMaterial, mesh, 0, customProps)
    {
      this.sourceSurface = sourceSurface;
      this.sourceMaterial = sourceMaterial;
      this.defaultMaterial = defaultMaterial;
      this.loadedMaterial = loadedMaterial;
      this.sourceSubMeshIndex = subMeshIndex;
      this.sourceMeshEntity = meshEntity;
      this.sharedMeshEntity = sharedEntity;
      this.sourceFlags = flags;
      this.generatedType = generatedType;
      this.sourceType = type;
    }

    public void ReplaceMesh(Entity oldMesh, Entity newMesh)
    {
      if (this.sourceMeshEntity == oldMesh)
        this.sourceMeshEntity = newMesh;
      if (!(this.sharedMeshEntity == oldMesh))
        return;
      this.sharedMeshEntity = newMesh;
    }

    public override void Dispose() => base.Dispose();
  }
}
