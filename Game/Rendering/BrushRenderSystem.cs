// Decompiled with JetBrains decompiler
// Type: Game.Rendering.BrushRenderSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Mathematics;
using Game.Common;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class BrushRenderSystem : GameSystemBase
  {
    private EntityQuery m_BrushQuery;
    private EntityQuery m_SettingsQuery;
    private ToolSystem m_ToolSystem;
    private TerrainSystem m_TerrainSystem;
    private PrefabSystem m_PrefabSystem;
    private Mesh m_Mesh;
    private MaterialPropertyBlock m_Properties;
    private int m_BrushTexture;
    private int m_BrushOpacity;
    private BrushRenderSystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BrushQuery = this.GetEntityQuery(ComponentType.ReadOnly<Brush>(), ComponentType.Exclude<Hidden>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_SettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<OverlayConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_BrushTexture = Shader.PropertyToID("_BrushTexture");
      // ISSUE: reference to a compiler-generated field
      this.m_BrushOpacity = Shader.PropertyToID("_BrushOpacity");
      RenderPipelineManager.beginContextRendering += (Action<ScriptableRenderContext, List<Camera>>) ((context, cameras) =>
      {
        try
        {
          OverlayConfigurationPrefab prefab1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (!this.m_PrefabSystem.TryGetSingletonPrefab<OverlayConfigurationPrefab>(this.m_SettingsQuery, out prefab1))
            return;
          // ISSUE: reference to a compiler-generated field
          float y1 = this.m_TerrainSystem.heightScaleOffset.y - 50f;
          // ISSUE: reference to a compiler-generated field
          float y2 = this.m_TerrainSystem.heightScaleOffset.x + 100f;
          // ISSUE: reference to a compiler-generated method
          Mesh mesh = this.GetMesh();
          // ISSUE: reference to a compiler-generated method
          MaterialPropertyBlock properties = this.GetProperties();
          // ISSUE: reference to a compiler-generated field
          NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_BrushQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
          this.CompleteDependency();
          try
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Tools_Brush_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ComponentTypeHandle<Brush> componentTypeHandle1 = this.__TypeHandle.__Game_Tools_Brush_RO_ComponentTypeHandle;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ComponentTypeHandle<PrefabRef> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
            for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
            {
              ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
              NativeArray<Brush> nativeArray1 = archetypeChunk.GetNativeArray<Brush>(ref componentTypeHandle1);
              NativeArray<PrefabRef> nativeArray2 = archetypeChunk.GetNativeArray<PrefabRef>(ref componentTypeHandle2);
              for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
              {
                Brush brush = nativeArray1[index2];
                PrefabRef refData = nativeArray2[index2];
                float3 pos = new float3(brush.m_Position.x, y1, brush.m_Position.z);
                quaternion q = quaternion.RotateY(brush.m_Angle);
                float3 s = new float3(brush.m_Size * 0.5f, y2, brush.m_Size * 0.5f);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                PrefabBase prefab2 = this.m_PrefabSystem.GetPrefab<PrefabBase>(brush.m_Tool);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                BrushPrefab prefab3 = this.m_PrefabSystem.GetPrefab<BrushPrefab>(refData);
                properties.Clear();
                // ISSUE: reference to a compiler-generated field
                properties.SetTexture(this.m_BrushTexture, (Texture) prefab3.m_Texture);
                // ISSUE: reference to a compiler-generated field
                properties.SetFloat(this.m_BrushOpacity, brush.m_Opacity);
                Material material = (Material) null;
                switch (prefab2)
                {
                  case TerraformingPrefab terraformingPrefab2:
                    material = terraformingPrefab2.m_BrushMaterial;
                    break;
                  case ObjectPrefab _:
                    material = prefab1.m_ObjectBrushMaterial;
                    break;
                }
                Matrix4x4 matrix = Matrix4x4.TRS((Vector3) pos, (Quaternion) q, (Vector3) s);
                foreach (Camera camera in cameras)
                {
                  if (camera.cameraType == CameraType.Game || camera.cameraType == CameraType.SceneView)
                    Graphics.DrawMesh(mesh, matrix, material, 0, camera, 0, properties, ShadowCastingMode.Off, false);
                }
                TerraformingData component;
                if (this.EntityManager.TryGetComponent<TerraformingData>(brush.m_Tool, out component))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.PreviewHeight(brush, prefab3, component.m_Type);
                }
              }
            }
          }
          finally
          {
            archetypeChunkArray.Dispose();
          }
        }
        finally
        {
        }
      });
    }

    [Preserve]
    protected override void OnDestroy()
    {
      RenderPipelineManager.beginContextRendering -= (Action<ScriptableRenderContext, List<Camera>>) ((context, cameras) =>
      {
        try
        {
          OverlayConfigurationPrefab prefab1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (!this.m_PrefabSystem.TryGetSingletonPrefab<OverlayConfigurationPrefab>(this.m_SettingsQuery, out prefab1))
            return;
          // ISSUE: reference to a compiler-generated field
          float y1 = this.m_TerrainSystem.heightScaleOffset.y - 50f;
          // ISSUE: reference to a compiler-generated field
          float y2 = this.m_TerrainSystem.heightScaleOffset.x + 100f;
          // ISSUE: reference to a compiler-generated method
          Mesh mesh = this.GetMesh();
          // ISSUE: reference to a compiler-generated method
          MaterialPropertyBlock properties = this.GetProperties();
          // ISSUE: reference to a compiler-generated field
          NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_BrushQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
          this.CompleteDependency();
          try
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Tools_Brush_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ComponentTypeHandle<Brush> componentTypeHandle1 = this.__TypeHandle.__Game_Tools_Brush_RO_ComponentTypeHandle;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ComponentTypeHandle<PrefabRef> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
            for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
            {
              ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
              NativeArray<Brush> nativeArray1 = archetypeChunk.GetNativeArray<Brush>(ref componentTypeHandle1);
              NativeArray<PrefabRef> nativeArray2 = archetypeChunk.GetNativeArray<PrefabRef>(ref componentTypeHandle2);
              for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
              {
                Brush brush = nativeArray1[index2];
                PrefabRef refData = nativeArray2[index2];
                float3 pos = new float3(brush.m_Position.x, y1, brush.m_Position.z);
                quaternion q = quaternion.RotateY(brush.m_Angle);
                float3 s = new float3(brush.m_Size * 0.5f, y2, brush.m_Size * 0.5f);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                PrefabBase prefab2 = this.m_PrefabSystem.GetPrefab<PrefabBase>(brush.m_Tool);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                BrushPrefab prefab3 = this.m_PrefabSystem.GetPrefab<BrushPrefab>(refData);
                properties.Clear();
                // ISSUE: reference to a compiler-generated field
                properties.SetTexture(this.m_BrushTexture, (Texture) prefab3.m_Texture);
                // ISSUE: reference to a compiler-generated field
                properties.SetFloat(this.m_BrushOpacity, brush.m_Opacity);
                Material material = (Material) null;
                switch (prefab2)
                {
                  case TerraformingPrefab terraformingPrefab2:
                    material = terraformingPrefab2.m_BrushMaterial;
                    break;
                  case ObjectPrefab _:
                    material = prefab1.m_ObjectBrushMaterial;
                    break;
                }
                Matrix4x4 matrix = Matrix4x4.TRS((Vector3) pos, (Quaternion) q, (Vector3) s);
                foreach (Camera camera in cameras)
                {
                  if (camera.cameraType == CameraType.Game || camera.cameraType == CameraType.SceneView)
                    Graphics.DrawMesh(mesh, matrix, material, 0, camera, 0, properties, ShadowCastingMode.Off, false);
                }
                TerraformingData component;
                if (this.EntityManager.TryGetComponent<TerraformingData>(brush.m_Tool, out component))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.PreviewHeight(brush, prefab3, component.m_Type);
                }
              }
            }
          }
          finally
          {
            archetypeChunkArray.Dispose();
          }
        }
        finally
        {
        }
      });
      // ISSUE: reference to a compiler-generated field
      CoreUtils.Destroy((UnityEngine.Object) this.m_Mesh);
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
        OverlayConfigurationPrefab prefab1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (!this.m_PrefabSystem.TryGetSingletonPrefab<OverlayConfigurationPrefab>(this.m_SettingsQuery, out prefab1))
          return;
        // ISSUE: reference to a compiler-generated field
        float y1 = this.m_TerrainSystem.heightScaleOffset.y - 50f;
        // ISSUE: reference to a compiler-generated field
        float y2 = this.m_TerrainSystem.heightScaleOffset.x + 100f;
        // ISSUE: reference to a compiler-generated method
        Mesh mesh = this.GetMesh();
        // ISSUE: reference to a compiler-generated method
        MaterialPropertyBlock properties = this.GetProperties();
        // ISSUE: reference to a compiler-generated field
        NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_BrushQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        this.CompleteDependency();
        try
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Tools_Brush_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ComponentTypeHandle<Brush> componentTypeHandle1 = this.__TypeHandle.__Game_Tools_Brush_RO_ComponentTypeHandle;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ComponentTypeHandle<PrefabRef> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
          for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
          {
            ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
            NativeArray<Brush> nativeArray1 = archetypeChunk.GetNativeArray<Brush>(ref componentTypeHandle1);
            NativeArray<PrefabRef> nativeArray2 = archetypeChunk.GetNativeArray<PrefabRef>(ref componentTypeHandle2);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              Brush brush = nativeArray1[index2];
              PrefabRef refData = nativeArray2[index2];
              float3 pos = new float3(brush.m_Position.x, y1, brush.m_Position.z);
              quaternion q = quaternion.RotateY(brush.m_Angle);
              float3 s = new float3(brush.m_Size * 0.5f, y2, brush.m_Size * 0.5f);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              PrefabBase prefab2 = this.m_PrefabSystem.GetPrefab<PrefabBase>(brush.m_Tool);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              BrushPrefab prefab3 = this.m_PrefabSystem.GetPrefab<BrushPrefab>(refData);
              properties.Clear();
              // ISSUE: reference to a compiler-generated field
              properties.SetTexture(this.m_BrushTexture, (Texture) prefab3.m_Texture);
              // ISSUE: reference to a compiler-generated field
              properties.SetFloat(this.m_BrushOpacity, brush.m_Opacity);
              Material material = (Material) null;
              switch (prefab2)
              {
                case TerraformingPrefab terraformingPrefab:
                  material = terraformingPrefab.m_BrushMaterial;
                  break;
                case ObjectPrefab _:
                  material = prefab1.m_ObjectBrushMaterial;
                  break;
              }
              Matrix4x4 matrix = Matrix4x4.TRS((Vector3) pos, (Quaternion) q, (Vector3) s);
              foreach (Camera camera in cameras)
              {
                if (camera.cameraType == CameraType.Game || camera.cameraType == CameraType.SceneView)
                  Graphics.DrawMesh(mesh, matrix, material, 0, camera, 0, properties, ShadowCastingMode.Off, false);
              }
              TerraformingData component;
              if (this.EntityManager.TryGetComponent<TerraformingData>(brush.m_Tool, out component))
              {
                // ISSUE: reference to a compiler-generated method
                this.PreviewHeight(brush, prefab3, component.m_Type);
              }
            }
          }
        }
        finally
        {
          archetypeChunkArray.Dispose();
        }
      }
      finally
      {
      }
    }

    private void PreviewHeight(Brush brush, BrushPrefab prefab, TerraformingType terraformingType)
    {
      Bounds2 bounds = ToolUtils.GetBounds(brush);
      if ((terraformingType == TerraformingType.Level || terraformingType == TerraformingType.Slope) && (double) brush.m_Strength < 0.0)
        brush.m_Strength = terraformingType != TerraformingType.Level ? 0.0f : math.abs(brush.m_Strength);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.PreviewBrush(terraformingType, bounds, brush, (Texture) prefab.m_Texture);
    }

    private Mesh GetMesh()
    {
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_Mesh == (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Mesh = new Mesh();
        // ISSUE: reference to a compiler-generated field
        this.m_Mesh.name = "Brush";
        // ISSUE: reference to a compiler-generated field
        this.m_Mesh.vertices = new Vector3[8]
        {
          new Vector3(-1f, 0.0f, -1f),
          new Vector3(-1f, 0.0f, 1f),
          new Vector3(1f, 0.0f, 1f),
          new Vector3(1f, 0.0f, -1f),
          new Vector3(-1f, 1f, -1f),
          new Vector3(-1f, 1f, 1f),
          new Vector3(1f, 1f, 1f),
          new Vector3(1f, 1f, -1f)
        };
        // ISSUE: reference to a compiler-generated field
        this.m_Mesh.triangles = new int[36]
        {
          0,
          1,
          5,
          5,
          4,
          0,
          3,
          7,
          6,
          6,
          2,
          3,
          0,
          3,
          2,
          2,
          1,
          0,
          4,
          5,
          6,
          6,
          7,
          4,
          0,
          4,
          7,
          7,
          3,
          0,
          1,
          2,
          6,
          6,
          5,
          1
        };
      }
      // ISSUE: reference to a compiler-generated field
      return this.m_Mesh;
    }

    private MaterialPropertyBlock GetProperties()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_Properties == null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Properties = new MaterialPropertyBlock();
      }
      // ISSUE: reference to a compiler-generated field
      return this.m_Properties;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
    }

    protected override void OnCreateForCompiler()
    {
      base.OnCreateForCompiler();
      // ISSUE: reference to a compiler-generated method
      this.__AssignQueries(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.__TypeHandle.__AssignHandles(ref this.CheckedStateRef);
    }

    [Preserve]
    public BrushRenderSystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Brush> __Game_Tools_Brush_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Brush_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Brush>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
      }
    }
  }
}
