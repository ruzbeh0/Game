// Decompiled with JetBrains decompiler
// Type: Game.Rendering.RouteRenderSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Prefabs;
using Game.Routes;
using Game.Tools;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class RouteRenderSystem : GameSystemBase
  {
    private ToolSystem m_ToolSystem;
    private RouteBufferSystem m_RouteBufferSystem;
    private EntityQuery m_RouteQuery;
    private EntityQuery m_LivePathQuery;
    private EntityQuery m_InfomodeQuery;
    private Mesh m_Mesh;
    private ComputeBuffer m_ArgsBuffer;
    private List<uint> m_ArgsArray;
    private int m_RouteSegmentBuffer;
    private int m_RouteColor;
    private int m_RouteSize;
    private RouteRenderSystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RouteBufferSystem = this.World.GetOrCreateSystemManaged<RouteBufferSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RouteQuery = this.GetEntityQuery(ComponentType.ReadOnly<Route>(), ComponentType.ReadOnly<RouteWaypoint>(), ComponentType.ReadOnly<RouteSegment>(), ComponentType.Exclude<HiddenRoute>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Hidden>());
      // ISSUE: reference to a compiler-generated field
      this.m_LivePathQuery = this.GetEntityQuery(ComponentType.ReadOnly<LivePath>(), ComponentType.ReadOnly<RouteWaypoint>(), ComponentType.ReadOnly<RouteSegment>(), ComponentType.Exclude<HiddenRoute>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Hidden>());
      // ISSUE: reference to a compiler-generated field
      this.m_InfomodeQuery = this.GetEntityQuery(ComponentType.ReadOnly<InfomodeActive>(), ComponentType.ReadOnly<InfoviewRouteData>());
      // ISSUE: reference to a compiler-generated field
      this.m_RouteSegmentBuffer = Shader.PropertyToID("colossal_RouteSegmentBuffer");
      // ISSUE: reference to a compiler-generated field
      this.m_RouteColor = Shader.PropertyToID("colossal_RouteColor");
      // ISSUE: reference to a compiler-generated field
      this.m_RouteSize = Shader.PropertyToID("colossal_RouteSize");
      RenderPipelineManager.beginContextRendering += (Action<ScriptableRenderContext, List<Camera>>) ((context, cameras) =>
      {
        try
        {
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          EntityQuery entityQuery = this.ShouldRenderRoutes() ? this.m_RouteQuery : this.m_LivePathQuery;
          if (entityQuery.IsEmptyIgnoreFilter)
            return;
          using (NativeArray<ArchetypeChunk> archetypeChunkArray = entityQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
          {
            if (archetypeChunkArray.Length == 0)
              return;
            // ISSUE: reference to a compiler-generated method
            this.EnsureMesh();
            // ISSUE: reference to a compiler-generated field
            if (this.m_ArgsArray == null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_ArgsArray = new List<uint>();
            }
            // ISSUE: reference to a compiler-generated field
            this.m_ArgsArray.Clear();
            // ISSUE: reference to a compiler-generated field
            uint indexCount = this.m_Mesh.GetIndexCount(0);
            // ISSUE: reference to a compiler-generated field
            uint indexStart = this.m_Mesh.GetIndexStart(0);
            // ISSUE: reference to a compiler-generated field
            uint baseVertex = this.m_Mesh.GetBaseVertex(0);
            this.CompleteDependency();
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Rendering_RouteBufferIndex_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ComponentTypeHandle<RouteBufferIndex> componentTypeHandle1 = this.__TypeHandle.__Game_Rendering_RouteBufferIndex_RO_ComponentTypeHandle;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Routes_Color_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ComponentTypeHandle<Game.Routes.Color> componentTypeHandle2 = this.__TypeHandle.__Game_Routes_Color_RO_ComponentTypeHandle;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Tools_Highlighted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ComponentTypeHandle<Highlighted> componentTypeHandle3 = this.__TypeHandle.__Game_Tools_Highlighted_RO_ComponentTypeHandle;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ComponentTypeHandle<Temp> componentTypeHandle4 = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle;
            int count1 = 0;
            for (int index = 0; index < archetypeChunkArray.Length; ++index)
            {
              ArchetypeChunk archetypeChunk = archetypeChunkArray[index];
              count1 += archetypeChunk.Count * 5;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ArgsBuffer != null && this.m_ArgsBuffer.count < count1)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_ArgsBuffer.Release();
              // ISSUE: reference to a compiler-generated field
              this.m_ArgsBuffer = (ComputeBuffer) null;
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_ArgsBuffer == null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_ArgsBuffer = new ComputeBuffer(count1, 4, ComputeBufferType.DrawIndirect);
              // ISSUE: reference to a compiler-generated field
              this.m_ArgsBuffer.name = "Route args buffer";
            }
            // ISSUE: reference to a compiler-generated field
            Entity selected = this.m_ToolSystem.selected;
            for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
            {
              ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
              NativeArray<Entity> nativeArray1 = archetypeChunk.GetNativeArray(entityTypeHandle);
              NativeArray<RouteBufferIndex> nativeArray2 = archetypeChunk.GetNativeArray<RouteBufferIndex>(ref componentTypeHandle1);
              NativeArray<Game.Routes.Color> nativeArray3 = archetypeChunk.GetNativeArray<Game.Routes.Color>(ref componentTypeHandle2);
              NativeArray<Temp> nativeArray4 = archetypeChunk.GetNativeArray<Temp>(ref componentTypeHandle4);
              bool flag = archetypeChunk.Has<Highlighted>(ref componentTypeHandle3);
              for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
              {
                Material material;
                ComputeBuffer segmentBuffer;
                int originalRenderQueue;
                Bounds bounds;
                Vector4 size;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.m_RouteBufferSystem.GetBuffer(nativeArray2[index2].m_Index, out material, out segmentBuffer, out originalRenderQueue, out bounds, out size);
                if (!((UnityEngine.Object) material == (UnityEngine.Object) null) && segmentBuffer != null)
                {
                  // ISSUE: reference to a compiler-generated field
                  int count2 = this.m_ArgsArray.Count;
                  // ISSUE: reference to a compiler-generated field
                  this.m_ArgsArray.Add(indexCount);
                  // ISSUE: reference to a compiler-generated field
                  this.m_ArgsArray.Add((uint) segmentBuffer.count);
                  // ISSUE: reference to a compiler-generated field
                  this.m_ArgsArray.Add(indexStart);
                  // ISSUE: reference to a compiler-generated field
                  this.m_ArgsArray.Add(baseVertex);
                  // ISSUE: reference to a compiler-generated field
                  this.m_ArgsArray.Add(0U);
                  Game.Routes.Color color = nativeArray3[index2];
                  if (nativeArray1[index2] == selected | flag || nativeArray4.Length != 0 && (nativeArray4[index2].m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify)) != (TempFlags) 0)
                  {
                    color.m_Color.a = byte.MaxValue;
                    size.x *= 1.33333337f;
                    material.renderQueue = originalRenderQueue + 1;
                  }
                  else
                  {
                    color.m_Color.a = (byte) 128;
                    material.renderQueue = originalRenderQueue;
                  }
                  // ISSUE: reference to a compiler-generated field
                  material.SetBuffer(this.m_RouteSegmentBuffer, segmentBuffer);
                  // ISSUE: reference to a compiler-generated field
                  material.SetColor(this.m_RouteColor, (UnityEngine.Color) color.m_Color);
                  // ISSUE: reference to a compiler-generated field
                  material.SetVector(this.m_RouteSize, size);
                  bounds.Expand(size.x);
                  foreach (Camera camera in cameras)
                  {
                    if (camera.cameraType == CameraType.Game || camera.cameraType == CameraType.SceneView)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      Graphics.DrawMeshInstancedIndirect(this.m_Mesh, 0, material, bounds, this.m_ArgsBuffer, count2 * 4, castShadows: ShadowCastingMode.Off, receiveShadows: false, camera: camera);
                    }
                  }
                }
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_ArgsArray.Count <= 0)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ArgsBuffer.SetData<uint>(this.m_ArgsArray, 0, 0, this.m_ArgsArray.Count);
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
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          EntityQuery entityQuery = this.ShouldRenderRoutes() ? this.m_RouteQuery : this.m_LivePathQuery;
          if (entityQuery.IsEmptyIgnoreFilter)
            return;
          using (NativeArray<ArchetypeChunk> archetypeChunkArray = entityQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
          {
            if (archetypeChunkArray.Length == 0)
              return;
            // ISSUE: reference to a compiler-generated method
            this.EnsureMesh();
            // ISSUE: reference to a compiler-generated field
            if (this.m_ArgsArray == null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_ArgsArray = new List<uint>();
            }
            // ISSUE: reference to a compiler-generated field
            this.m_ArgsArray.Clear();
            // ISSUE: reference to a compiler-generated field
            uint indexCount = this.m_Mesh.GetIndexCount(0);
            // ISSUE: reference to a compiler-generated field
            uint indexStart = this.m_Mesh.GetIndexStart(0);
            // ISSUE: reference to a compiler-generated field
            uint baseVertex = this.m_Mesh.GetBaseVertex(0);
            this.CompleteDependency();
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Rendering_RouteBufferIndex_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ComponentTypeHandle<RouteBufferIndex> componentTypeHandle1 = this.__TypeHandle.__Game_Rendering_RouteBufferIndex_RO_ComponentTypeHandle;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Routes_Color_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ComponentTypeHandle<Game.Routes.Color> componentTypeHandle2 = this.__TypeHandle.__Game_Routes_Color_RO_ComponentTypeHandle;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Tools_Highlighted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ComponentTypeHandle<Highlighted> componentTypeHandle3 = this.__TypeHandle.__Game_Tools_Highlighted_RO_ComponentTypeHandle;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ComponentTypeHandle<Temp> componentTypeHandle4 = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle;
            int count1 = 0;
            for (int index = 0; index < archetypeChunkArray.Length; ++index)
            {
              ArchetypeChunk archetypeChunk = archetypeChunkArray[index];
              count1 += archetypeChunk.Count * 5;
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ArgsBuffer != null && this.m_ArgsBuffer.count < count1)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_ArgsBuffer.Release();
              // ISSUE: reference to a compiler-generated field
              this.m_ArgsBuffer = (ComputeBuffer) null;
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_ArgsBuffer == null)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_ArgsBuffer = new ComputeBuffer(count1, 4, ComputeBufferType.DrawIndirect);
              // ISSUE: reference to a compiler-generated field
              this.m_ArgsBuffer.name = "Route args buffer";
            }
            // ISSUE: reference to a compiler-generated field
            Entity selected = this.m_ToolSystem.selected;
            for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
            {
              ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
              NativeArray<Entity> nativeArray1 = archetypeChunk.GetNativeArray(entityTypeHandle);
              NativeArray<RouteBufferIndex> nativeArray2 = archetypeChunk.GetNativeArray<RouteBufferIndex>(ref componentTypeHandle1);
              NativeArray<Game.Routes.Color> nativeArray3 = archetypeChunk.GetNativeArray<Game.Routes.Color>(ref componentTypeHandle2);
              NativeArray<Temp> nativeArray4 = archetypeChunk.GetNativeArray<Temp>(ref componentTypeHandle4);
              bool flag = archetypeChunk.Has<Highlighted>(ref componentTypeHandle3);
              for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
              {
                Material material;
                ComputeBuffer segmentBuffer;
                int originalRenderQueue;
                Bounds bounds;
                Vector4 size;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.m_RouteBufferSystem.GetBuffer(nativeArray2[index2].m_Index, out material, out segmentBuffer, out originalRenderQueue, out bounds, out size);
                if (!((UnityEngine.Object) material == (UnityEngine.Object) null) && segmentBuffer != null)
                {
                  // ISSUE: reference to a compiler-generated field
                  int count2 = this.m_ArgsArray.Count;
                  // ISSUE: reference to a compiler-generated field
                  this.m_ArgsArray.Add(indexCount);
                  // ISSUE: reference to a compiler-generated field
                  this.m_ArgsArray.Add((uint) segmentBuffer.count);
                  // ISSUE: reference to a compiler-generated field
                  this.m_ArgsArray.Add(indexStart);
                  // ISSUE: reference to a compiler-generated field
                  this.m_ArgsArray.Add(baseVertex);
                  // ISSUE: reference to a compiler-generated field
                  this.m_ArgsArray.Add(0U);
                  Game.Routes.Color color = nativeArray3[index2];
                  if (nativeArray1[index2] == selected | flag || nativeArray4.Length != 0 && (nativeArray4[index2].m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify)) != (TempFlags) 0)
                  {
                    color.m_Color.a = byte.MaxValue;
                    size.x *= 1.33333337f;
                    material.renderQueue = originalRenderQueue + 1;
                  }
                  else
                  {
                    color.m_Color.a = (byte) 128;
                    material.renderQueue = originalRenderQueue;
                  }
                  // ISSUE: reference to a compiler-generated field
                  material.SetBuffer(this.m_RouteSegmentBuffer, segmentBuffer);
                  // ISSUE: reference to a compiler-generated field
                  material.SetColor(this.m_RouteColor, (UnityEngine.Color) color.m_Color);
                  // ISSUE: reference to a compiler-generated field
                  material.SetVector(this.m_RouteSize, size);
                  bounds.Expand(size.x);
                  foreach (Camera camera in cameras)
                  {
                    if (camera.cameraType == CameraType.Game || camera.cameraType == CameraType.SceneView)
                    {
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      Graphics.DrawMeshInstancedIndirect(this.m_Mesh, 0, material, bounds, this.m_ArgsBuffer, count2 * 4, castShadows: ShadowCastingMode.Off, receiveShadows: false, camera: camera);
                    }
                  }
                }
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_ArgsArray.Count <= 0)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ArgsBuffer.SetData<uint>(this.m_ArgsArray, 0, 0, this.m_ArgsArray.Count);
        }
        finally
        {
        }
      });
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_Mesh != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_Mesh);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_ArgsBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ArgsBuffer.Release();
      }
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
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EntityQuery entityQuery = this.ShouldRenderRoutes() ? this.m_RouteQuery : this.m_LivePathQuery;
        if (entityQuery.IsEmptyIgnoreFilter)
          return;
        using (NativeArray<ArchetypeChunk> archetypeChunkArray = entityQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
        {
          if (archetypeChunkArray.Length == 0)
            return;
          // ISSUE: reference to a compiler-generated method
          this.EnsureMesh();
          // ISSUE: reference to a compiler-generated field
          if (this.m_ArgsArray == null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ArgsArray = new List<uint>();
          }
          // ISSUE: reference to a compiler-generated field
          this.m_ArgsArray.Clear();
          // ISSUE: reference to a compiler-generated field
          uint indexCount = this.m_Mesh.GetIndexCount(0);
          // ISSUE: reference to a compiler-generated field
          uint indexStart = this.m_Mesh.GetIndexStart(0);
          // ISSUE: reference to a compiler-generated field
          uint baseVertex = this.m_Mesh.GetBaseVertex(0);
          this.CompleteDependency();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Rendering_RouteBufferIndex_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ComponentTypeHandle<RouteBufferIndex> componentTypeHandle1 = this.__TypeHandle.__Game_Rendering_RouteBufferIndex_RO_ComponentTypeHandle;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Routes_Color_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ComponentTypeHandle<Game.Routes.Color> componentTypeHandle2 = this.__TypeHandle.__Game_Routes_Color_RO_ComponentTypeHandle;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Tools_Highlighted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ComponentTypeHandle<Highlighted> componentTypeHandle3 = this.__TypeHandle.__Game_Tools_Highlighted_RO_ComponentTypeHandle;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ComponentTypeHandle<Temp> componentTypeHandle4 = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle;
          int count1 = 0;
          for (int index = 0; index < archetypeChunkArray.Length; ++index)
          {
            ArchetypeChunk archetypeChunk = archetypeChunkArray[index];
            count1 += archetypeChunk.Count * 5;
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_ArgsBuffer != null && this.m_ArgsBuffer.count < count1)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ArgsBuffer.Release();
            // ISSUE: reference to a compiler-generated field
            this.m_ArgsBuffer = (ComputeBuffer) null;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_ArgsBuffer == null)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_ArgsBuffer = new ComputeBuffer(count1, 4, ComputeBufferType.DrawIndirect);
            // ISSUE: reference to a compiler-generated field
            this.m_ArgsBuffer.name = "Route args buffer";
          }
          // ISSUE: reference to a compiler-generated field
          Entity selected = this.m_ToolSystem.selected;
          for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
          {
            ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
            NativeArray<Entity> nativeArray1 = archetypeChunk.GetNativeArray(entityTypeHandle);
            NativeArray<RouteBufferIndex> nativeArray2 = archetypeChunk.GetNativeArray<RouteBufferIndex>(ref componentTypeHandle1);
            NativeArray<Game.Routes.Color> nativeArray3 = archetypeChunk.GetNativeArray<Game.Routes.Color>(ref componentTypeHandle2);
            NativeArray<Temp> nativeArray4 = archetypeChunk.GetNativeArray<Temp>(ref componentTypeHandle4);
            bool flag = archetypeChunk.Has<Highlighted>(ref componentTypeHandle3);
            for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
            {
              Material material;
              ComputeBuffer segmentBuffer;
              int originalRenderQueue;
              Bounds bounds;
              Vector4 size;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_RouteBufferSystem.GetBuffer(nativeArray2[index2].m_Index, out material, out segmentBuffer, out originalRenderQueue, out bounds, out size);
              if (!((UnityEngine.Object) material == (UnityEngine.Object) null) && segmentBuffer != null)
              {
                // ISSUE: reference to a compiler-generated field
                int count2 = this.m_ArgsArray.Count;
                // ISSUE: reference to a compiler-generated field
                this.m_ArgsArray.Add(indexCount);
                // ISSUE: reference to a compiler-generated field
                this.m_ArgsArray.Add((uint) segmentBuffer.count);
                // ISSUE: reference to a compiler-generated field
                this.m_ArgsArray.Add(indexStart);
                // ISSUE: reference to a compiler-generated field
                this.m_ArgsArray.Add(baseVertex);
                // ISSUE: reference to a compiler-generated field
                this.m_ArgsArray.Add(0U);
                Game.Routes.Color color = nativeArray3[index2];
                if (nativeArray1[index2] == selected | flag || nativeArray4.Length != 0 && (nativeArray4[index2].m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Select | TempFlags.Modify)) != (TempFlags) 0)
                {
                  color.m_Color.a = byte.MaxValue;
                  size.x *= 1.33333337f;
                  material.renderQueue = originalRenderQueue + 1;
                }
                else
                {
                  color.m_Color.a = (byte) 128;
                  material.renderQueue = originalRenderQueue;
                }
                // ISSUE: reference to a compiler-generated field
                material.SetBuffer(this.m_RouteSegmentBuffer, segmentBuffer);
                // ISSUE: reference to a compiler-generated field
                material.SetColor(this.m_RouteColor, (UnityEngine.Color) color.m_Color);
                // ISSUE: reference to a compiler-generated field
                material.SetVector(this.m_RouteSize, size);
                bounds.Expand(size.x);
                foreach (Camera camera in cameras)
                {
                  if (camera.cameraType == CameraType.Game || camera.cameraType == CameraType.SceneView)
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    Graphics.DrawMeshInstancedIndirect(this.m_Mesh, 0, material, bounds, this.m_ArgsBuffer, count2 * 4, castShadows: ShadowCastingMode.Off, receiveShadows: false, camera: camera);
                  }
                }
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ArgsArray.Count <= 0)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ArgsBuffer.SetData<uint>(this.m_ArgsArray, 0, 0, this.m_ArgsArray.Count);
      }
      finally
      {
      }
    }

    private bool ShouldRenderRoutes()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return this.m_ToolSystem.activeTool != null && this.m_ToolSystem.activeTool.requireRoutes != RouteType.None || !this.m_InfomodeQuery.IsEmptyIgnoreFilter;
    }

    private void EnsureMesh()
    {
      // ISSUE: reference to a compiler-generated field
      if (!((UnityEngine.Object) this.m_Mesh == (UnityEngine.Object) null))
        return;
      Vector3[] vector3Array = new Vector3[68];
      Vector2[] vector2Array = new Vector2[vector3Array.Length];
      int[] numArray1 = new int[192];
      int index1 = 0;
      int num1 = 0;
      for (int index2 = 0; index2 <= 16; ++index2)
      {
        float num2 = (float) index2 / 16f;
        vector3Array[index1] = new Vector3(-1f, 0.0f, num2);
        vector2Array[index1] = new Vector2(0.0f, num2);
        int index3 = index1 + 1;
        vector3Array[index3] = new Vector3(1f, 0.0f, num2);
        vector2Array[index3] = new Vector2(0.0f, num2);
        index1 = index3 + 1;
        if (index2 != 0)
        {
          int[] numArray2 = numArray1;
          int index4 = num1;
          int num3 = index4 + 1;
          int num4 = index1 - 4;
          numArray2[index4] = num4;
          int[] numArray3 = numArray1;
          int index5 = num3;
          int num5 = index5 + 1;
          int num6 = index1 - 3;
          numArray3[index5] = num6;
          int[] numArray4 = numArray1;
          int index6 = num5;
          int num7 = index6 + 1;
          int num8 = index1 - 2;
          numArray4[index6] = num8;
          int[] numArray5 = numArray1;
          int index7 = num7;
          int num9 = index7 + 1;
          int num10 = index1 - 2;
          numArray5[index7] = num10;
          int[] numArray6 = numArray1;
          int index8 = num9;
          int num11 = index8 + 1;
          int num12 = index1 - 3;
          numArray6[index8] = num12;
          int[] numArray7 = numArray1;
          int index9 = num11;
          num1 = index9 + 1;
          int num13 = index1 - 1;
          numArray7[index9] = num13;
        }
      }
      for (int index10 = 0; index10 <= 16; ++index10)
      {
        float num14 = (float) index10 / 16f;
        vector3Array[index1] = new Vector3(0.0f, -1f, num14);
        vector2Array[index1] = new Vector2(1f, num14);
        int index11 = index1 + 1;
        vector3Array[index11] = new Vector3(0.0f, 1f, num14);
        vector2Array[index11] = new Vector2(1f, num14);
        index1 = index11 + 1;
        if (index10 != 0)
        {
          int[] numArray8 = numArray1;
          int index12 = num1;
          int num15 = index12 + 1;
          int num16 = index1 - 4;
          numArray8[index12] = num16;
          int[] numArray9 = numArray1;
          int index13 = num15;
          int num17 = index13 + 1;
          int num18 = index1 - 3;
          numArray9[index13] = num18;
          int[] numArray10 = numArray1;
          int index14 = num17;
          int num19 = index14 + 1;
          int num20 = index1 - 2;
          numArray10[index14] = num20;
          int[] numArray11 = numArray1;
          int index15 = num19;
          int num21 = index15 + 1;
          int num22 = index1 - 2;
          numArray11[index15] = num22;
          int[] numArray12 = numArray1;
          int index16 = num21;
          int num23 = index16 + 1;
          int num24 = index1 - 3;
          numArray12[index16] = num24;
          int[] numArray13 = numArray1;
          int index17 = num23;
          num1 = index17 + 1;
          int num25 = index1 - 1;
          numArray13[index17] = num25;
        }
      }
      // ISSUE: reference to a compiler-generated field
      this.m_Mesh = new Mesh();
      // ISSUE: reference to a compiler-generated field
      this.m_Mesh.name = "Route segment";
      // ISSUE: reference to a compiler-generated field
      this.m_Mesh.vertices = vector3Array;
      // ISSUE: reference to a compiler-generated field
      this.m_Mesh.uv = vector2Array;
      // ISSUE: reference to a compiler-generated field
      this.m_Mesh.triangles = numArray1;
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
    public RouteRenderSystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<RouteBufferIndex> __Game_Rendering_RouteBufferIndex_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Routes.Color> __Game_Routes_Color_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Highlighted> __Game_Tools_Highlighted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_RouteBufferIndex_RO_ComponentTypeHandle = state.GetComponentTypeHandle<RouteBufferIndex>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Color_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Routes.Color>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Highlighted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Highlighted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
      }
    }
  }
}
