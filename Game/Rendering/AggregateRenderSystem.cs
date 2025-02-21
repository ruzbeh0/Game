// Decompiled with JetBrains decompiler
// Type: Game.Rendering.AggregateRenderSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Rendering
{
  public class AggregateRenderSystem : GameSystemBase
  {
    private AggregateMeshSystem m_AggregateMeshSystem;
    private RenderingSystem m_RenderingSystem;
    private ToolSystem m_ToolSystem;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_AggregateMeshSystem = this.World.GetOrCreateSystemManaged<AggregateMeshSystem>();
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      RenderPipelineManager.beginContextRendering += new Action<ScriptableRenderContext, List<Camera>>(this.Render);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      RenderPipelineManager.beginContextRendering -= new Action<ScriptableRenderContext, List<Camera>>(this.Render);
      base.OnDestroy();
    }

    [Preserve]
    protected override void OnUpdate()
    {
    }

    private void Render(ScriptableRenderContext context, List<Camera> cameras)
    {
      try
      {
        if (this.m_RenderingSystem.hideOverlay)
          return;
        if (this.m_ToolSystem.activeTool != null && this.m_ToolSystem.activeTool.requireNetArrows)
        {
          // ISSUE: reference to a compiler-generated method
          int arrowMaterialCount = this.m_AggregateMeshSystem.GetArrowMaterialCount();
          for (int index1 = 0; index1 < arrowMaterialCount; ++index1)
          {
            Mesh mesh;
            int subMeshCount;
            // ISSUE: reference to a compiler-generated method
            if (this.m_AggregateMeshSystem.GetArrowMesh(index1, out mesh, out subMeshCount))
            {
              for (int index2 = 0; index2 < subMeshCount; ++index2)
              {
                Material material;
                // ISSUE: reference to a compiler-generated method
                if (this.m_AggregateMeshSystem.GetArrowMaterial(index1, index2, out material))
                {
                  foreach (Camera camera in cameras)
                  {
                    if (camera.cameraType == CameraType.Game || camera.cameraType == CameraType.SceneView)
                      Graphics.DrawMesh(mesh, Matrix4x4.identity, material, 0, camera, index2, (MaterialPropertyBlock) null, false, false);
                  }
                }
              }
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          int nameMaterialCount = this.m_AggregateMeshSystem.GetNameMaterialCount();
          for (int index3 = 0; index3 < nameMaterialCount; ++index3)
          {
            Mesh mesh;
            int subMeshCount;
            // ISSUE: reference to a compiler-generated method
            if (this.m_AggregateMeshSystem.GetNameMesh(index3, out mesh, out subMeshCount))
            {
              for (int index4 = 0; index4 < subMeshCount; ++index4)
              {
                Material material;
                // ISSUE: reference to a compiler-generated method
                if (this.m_AggregateMeshSystem.GetNameMaterial(index3, index4, out material))
                {
                  foreach (Camera camera in cameras)
                  {
                    if (camera.cameraType == CameraType.Game || camera.cameraType == CameraType.SceneView)
                      Graphics.DrawMesh(mesh, Matrix4x4.identity, material, 0, camera, index4, (MaterialPropertyBlock) null, false, false);
                  }
                }
              }
            }
          }
        }
      }
      finally
      {
      }
    }

    [Preserve]
    public AggregateRenderSystem()
    {
    }
  }
}
