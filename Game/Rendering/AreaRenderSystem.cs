// Decompiled with JetBrains decompiler
// Type: Game.Rendering.AreaRenderSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Tools;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Rendering
{
  public class AreaRenderSystem : GameSystemBase
  {
    private RenderingSystem m_RenderingSystem;
    private AreaBufferSystem m_AreaBufferSystem;
    private AreaBatchSystem m_AreaBatchSystem;
    private CityBoundaryMeshSystem m_CityBoundaryMeshSystem;
    private ToolSystem m_ToolSystem;
    private int m_AreaTriangleBuffer;
    private int m_AreaBatchBuffer;
    private int m_AreaBatchColors;
    private int m_VisibleIndices;
    private Mesh m_AreaMesh;
    private GraphicsBuffer m_ArgsBuffer;
    private List<GraphicsBuffer.IndirectDrawIndexedArgs> m_ArgsArray;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      this.m_AreaBufferSystem = this.World.GetOrCreateSystemManaged<AreaBufferSystem>();
      this.m_AreaBatchSystem = this.World.GetOrCreateSystemManaged<AreaBatchSystem>();
      this.m_CityBoundaryMeshSystem = this.World.GetOrCreateSystemManaged<CityBoundaryMeshSystem>();
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      this.m_AreaTriangleBuffer = Shader.PropertyToID("colossal_AreaTriangleBuffer");
      this.m_AreaBatchBuffer = Shader.PropertyToID("colossal_AreaBatchBuffer");
      this.m_AreaBatchColors = Shader.PropertyToID("colossal_AreaBatchColors");
      this.m_VisibleIndices = Shader.PropertyToID("colossal_VisibleIndices");
      RenderPipelineManager.beginContextRendering += new Action<ScriptableRenderContext, List<Camera>>(this.Render);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      RenderPipelineManager.beginContextRendering -= new Action<ScriptableRenderContext, List<Camera>>(this.Render);
      if ((UnityEngine.Object) this.m_AreaMesh != (UnityEngine.Object) null)
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_AreaMesh);
      if (this.m_ArgsBuffer != null)
        this.m_ArgsBuffer.Release();
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
        int count1 = 0;
        // ISSUE: reference to a compiler-generated method
        int batchCount = this.m_AreaBatchSystem.GetBatchCount();
        ComputeBuffer buffer1;
        Material material1;
        Bounds bounds1;
        if (!this.m_RenderingSystem.hideOverlay)
        {
          for (AreaType type = AreaType.Lot; type < AreaType.Count; ++type)
          {
            Mesh mesh;
            int subMeshCount;
            // ISSUE: reference to a compiler-generated method
            if (this.m_AreaBufferSystem.GetNameMesh(type, out mesh, out subMeshCount))
            {
              for (int index = 0; index < subMeshCount; ++index)
              {
                Material material2;
                // ISSUE: reference to a compiler-generated method
                if (this.m_AreaBufferSystem.GetNameMaterial(type, index, out material2))
                {
                  foreach (Camera camera in cameras)
                  {
                    if (camera.cameraType == CameraType.Game || camera.cameraType == CameraType.SceneView)
                      Graphics.DrawMesh(mesh, Matrix4x4.identity, material2, 0, camera, index, (MaterialPropertyBlock) null, false, false);
                  }
                }
              }
            }
          }
          Mesh mesh1;
          Material material3;
          // ISSUE: reference to a compiler-generated method
          if (this.m_CityBoundaryMeshSystem.GetBoundaryMesh(out mesh1, out material3))
          {
            foreach (Camera camera in cameras)
            {
              if (camera.cameraType == CameraType.Game || camera.cameraType == CameraType.SceneView)
                Graphics.DrawMesh(mesh1, Matrix4x4.identity, material3, 0, camera, 0, (MaterialPropertyBlock) null, false, false);
            }
          }
          if (this.m_ToolSystem.activeTool != null && this.m_ToolSystem.activeTool.requireAreas != AreaTypeMask.None)
          {
            for (int type = 0; type < 5; ++type)
            {
              // ISSUE: reference to a compiler-generated method
              if ((this.m_ToolSystem.activeTool.requireAreas & (AreaTypeMask) (1 << type)) != AreaTypeMask.None && this.m_AreaBufferSystem.GetAreaBuffer((AreaType) type, out buffer1, out material1, out bounds1))
                ++count1;
            }
          }
        }
        for (int index = 0; index < batchCount; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          if (this.m_AreaBatchSystem.GetAreaBatch(index, out buffer1, out ComputeBuffer _, out GraphicsBuffer _, out material1, out bounds1, out int _, out int _))
            ++count1;
        }
        if (count1 == 0)
          return;
        if (this.m_ArgsBuffer != null && this.m_ArgsBuffer.count < count1)
        {
          this.m_ArgsBuffer.Release();
          this.m_ArgsBuffer = (GraphicsBuffer) null;
        }
        if (this.m_ArgsBuffer == null)
          this.m_ArgsBuffer = new GraphicsBuffer(GraphicsBuffer.Target.IndirectArguments, count1, sizeof (GraphicsBuffer.IndirectDrawIndexedArgs));
        if (this.m_ArgsArray == null)
          this.m_ArgsArray = new List<GraphicsBuffer.IndirectDrawIndexedArgs>();
        this.m_ArgsArray.Clear();
        if (!this.m_RenderingSystem.hideOverlay && this.m_ToolSystem.activeTool != null && this.m_ToolSystem.activeTool.requireAreas != AreaTypeMask.None)
        {
          for (int type = 0; type < 5; ++type)
          {
            ComputeBuffer buffer2;
            Material material4;
            Bounds bounds2;
            // ISSUE: reference to a compiler-generated method
            if ((this.m_ToolSystem.activeTool.requireAreas & (AreaTypeMask) (1 << type)) != AreaTypeMask.None && this.m_AreaBufferSystem.GetAreaBuffer((AreaType) type, out buffer2, out material4, out bounds2))
            {
              if ((UnityEngine.Object) this.m_AreaMesh == (UnityEngine.Object) null)
                this.m_AreaMesh = AreaRenderSystem.CreateMesh();
              GraphicsBuffer.IndirectDrawIndexedArgs indirectDrawIndexedArgs = new GraphicsBuffer.IndirectDrawIndexedArgs();
              indirectDrawIndexedArgs.indexCountPerInstance = this.m_AreaMesh.GetIndexCount(0);
              indirectDrawIndexedArgs.instanceCount = (uint) buffer2.count;
              indirectDrawIndexedArgs.startIndex = this.m_AreaMesh.GetIndexStart(0);
              indirectDrawIndexedArgs.baseVertexIndex = this.m_AreaMesh.GetBaseVertex(0);
              int count2 = this.m_ArgsArray.Count;
              this.m_ArgsArray.Add(indirectDrawIndexedArgs);
              material4.SetBuffer(this.m_AreaTriangleBuffer, buffer2);
              foreach (Camera camera in cameras)
              {
                if (camera.cameraType == CameraType.Game || camera.cameraType == CameraType.SceneView)
                  Graphics.RenderMeshIndirect(new RenderParams(material4)
                  {
                    worldBounds = bounds2,
                    camera = camera
                  }, this.m_AreaMesh, this.m_ArgsBuffer, startCommand: count2);
              }
            }
          }
        }
        for (int index = 0; index < batchCount; ++index)
        {
          ComputeBuffer buffer3;
          ComputeBuffer colors;
          GraphicsBuffer indices;
          Material material5;
          Bounds bounds3;
          int count3;
          int rendererPriority;
          // ISSUE: reference to a compiler-generated method
          if (this.m_AreaBatchSystem.GetAreaBatch(index, out buffer3, out colors, out indices, out material5, out bounds3, out count3, out rendererPriority))
          {
            if ((UnityEngine.Object) this.m_AreaMesh == (UnityEngine.Object) null)
              this.m_AreaMesh = AreaRenderSystem.CreateMesh();
            GraphicsBuffer.IndirectDrawIndexedArgs indirectDrawIndexedArgs = new GraphicsBuffer.IndirectDrawIndexedArgs();
            indirectDrawIndexedArgs.indexCountPerInstance = this.m_AreaMesh.GetIndexCount(0);
            indirectDrawIndexedArgs.instanceCount = (uint) count3;
            indirectDrawIndexedArgs.startIndex = this.m_AreaMesh.GetIndexStart(0);
            indirectDrawIndexedArgs.baseVertexIndex = this.m_AreaMesh.GetBaseVertex(0);
            int count4 = this.m_ArgsArray.Count;
            this.m_ArgsArray.Add(indirectDrawIndexedArgs);
            material5.SetBuffer(this.m_AreaBatchBuffer, buffer3);
            material5.SetBuffer(this.m_AreaBatchColors, colors);
            material5.SetBuffer(this.m_VisibleIndices, indices);
            foreach (Camera camera in cameras)
            {
              if (camera.cameraType != CameraType.Preview)
                Graphics.RenderMeshIndirect(new RenderParams(material5)
                {
                  worldBounds = bounds3,
                  camera = camera,
                  rendererPriority = rendererPriority
                }, this.m_AreaMesh, this.m_ArgsBuffer, startCommand: count4);
            }
          }
        }
        this.m_ArgsBuffer.SetData<GraphicsBuffer.IndirectDrawIndexedArgs>(this.m_ArgsArray, 0, 0, this.m_ArgsArray.Count);
      }
      finally
      {
      }
    }

    private static Mesh CreateMesh()
    {
      Vector3[] vector3Array1 = new Vector3[6];
      int[] numArray1 = new int[24];
      int num1 = 0;
      int num2 = 0;
      Vector3[] vector3Array2 = vector3Array1;
      int index1 = num1;
      int num3 = index1 + 1;
      Vector3 vector3_1 = new Vector3(0.0f, 0.0f, 0.0f);
      vector3Array2[index1] = vector3_1;
      Vector3[] vector3Array3 = vector3Array1;
      int index2 = num3;
      int num4 = index2 + 1;
      Vector3 vector3_2 = new Vector3(0.0f, 0.0f, 1f);
      vector3Array3[index2] = vector3_2;
      Vector3[] vector3Array4 = vector3Array1;
      int index3 = num4;
      int num5 = index3 + 1;
      Vector3 vector3_3 = new Vector3(1f, 0.0f, 0.0f);
      vector3Array4[index3] = vector3_3;
      Vector3[] vector3Array5 = vector3Array1;
      int index4 = num5;
      int num6 = index4 + 1;
      Vector3 vector3_4 = new Vector3(0.0f, 1f, 0.0f);
      vector3Array5[index4] = vector3_4;
      Vector3[] vector3Array6 = vector3Array1;
      int index5 = num6;
      int num7 = index5 + 1;
      Vector3 vector3_5 = new Vector3(0.0f, 1f, 1f);
      vector3Array6[index5] = vector3_5;
      Vector3[] vector3Array7 = vector3Array1;
      int index6 = num7;
      int num8 = index6 + 1;
      Vector3 vector3_6 = new Vector3(1f, 1f, 0.0f);
      vector3Array7[index6] = vector3_6;
      int[] numArray2 = numArray1;
      int index7 = num2;
      int num9 = index7 + 1;
      numArray2[index7] = 0;
      int[] numArray3 = numArray1;
      int index8 = num9;
      int num10 = index8 + 1;
      numArray3[index8] = 2;
      int[] numArray4 = numArray1;
      int index9 = num10;
      int num11 = index9 + 1;
      numArray4[index9] = 1;
      int[] numArray5 = numArray1;
      int index10 = num11;
      int num12 = index10 + 1;
      numArray5[index10] = 3;
      int[] numArray6 = numArray1;
      int index11 = num12;
      int num13 = index11 + 1;
      numArray6[index11] = 4;
      int[] numArray7 = numArray1;
      int index12 = num13;
      int num14 = index12 + 1;
      numArray7[index12] = 5;
      int[] numArray8 = numArray1;
      int index13 = num14;
      int num15 = index13 + 1;
      numArray8[index13] = 3;
      int[] numArray9 = numArray1;
      int index14 = num15;
      int num16 = index14 + 1;
      numArray9[index14] = 0;
      int[] numArray10 = numArray1;
      int index15 = num16;
      int num17 = index15 + 1;
      numArray10[index15] = 4;
      int[] numArray11 = numArray1;
      int index16 = num17;
      int num18 = index16 + 1;
      numArray11[index16] = 4;
      int[] numArray12 = numArray1;
      int index17 = num18;
      int num19 = index17 + 1;
      numArray12[index17] = 0;
      int[] numArray13 = numArray1;
      int index18 = num19;
      int num20 = index18 + 1;
      numArray13[index18] = 1;
      int[] numArray14 = numArray1;
      int index19 = num20;
      int num21 = index19 + 1;
      numArray14[index19] = 4;
      int[] numArray15 = numArray1;
      int index20 = num21;
      int num22 = index20 + 1;
      numArray15[index20] = 1;
      int[] numArray16 = numArray1;
      int index21 = num22;
      int num23 = index21 + 1;
      numArray16[index21] = 5;
      int[] numArray17 = numArray1;
      int index22 = num23;
      int num24 = index22 + 1;
      numArray17[index22] = 5;
      int[] numArray18 = numArray1;
      int index23 = num24;
      int num25 = index23 + 1;
      numArray18[index23] = 1;
      int[] numArray19 = numArray1;
      int index24 = num25;
      int num26 = index24 + 1;
      numArray19[index24] = 2;
      int[] numArray20 = numArray1;
      int index25 = num26;
      int num27 = index25 + 1;
      numArray20[index25] = 5;
      int[] numArray21 = numArray1;
      int index26 = num27;
      int num28 = index26 + 1;
      numArray21[index26] = 2;
      int[] numArray22 = numArray1;
      int index27 = num28;
      int num29 = index27 + 1;
      numArray22[index27] = 3;
      int[] numArray23 = numArray1;
      int index28 = num29;
      int num30 = index28 + 1;
      numArray23[index28] = 3;
      int[] numArray24 = numArray1;
      int index29 = num30;
      int num31 = index29 + 1;
      numArray24[index29] = 2;
      int[] numArray25 = numArray1;
      int index30 = num31;
      int num32 = index30 + 1;
      numArray25[index30] = 0;
      Mesh mesh = new Mesh();
      mesh.name = "Area triangle volume";
      mesh.vertices = vector3Array1;
      mesh.triangles = numArray1;
      return mesh;
    }

    [Preserve]
    public AreaRenderSystem()
    {
    }
  }
}
