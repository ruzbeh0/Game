// Decompiled with JetBrains decompiler
// Type: Game.Rendering.NotificationIconRenderSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs;
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
  public class NotificationIconRenderSystem : GameSystemBase
  {
    private PrefabSystem m_PrefabSystem;
    private NotificationIconBufferSystem m_BufferSystem;
    private RenderingSystem m_RenderingSystem;
    private Mesh m_Mesh;
    private Material m_Material;
    private ComputeBuffer m_ArgsBuffer;
    private ComputeBuffer m_InstanceBuffer;
    private Texture2DArray m_TextureArray;
    private uint[] m_ArgsArray;
    private EntityQuery m_ConfigurationQuery;
    private EntityQuery m_PrefabQuery;
    private int m_InstanceBufferID;
    private bool m_UpdateBuffer;
    private NotificationIconRenderSystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BufferSystem = this.World.GetOrCreateSystemManaged<NotificationIconBufferSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ConfigurationQuery = this.GetEntityQuery(ComponentType.ReadOnly<IconConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<NotificationIconData>(), ComponentType.ReadOnly<PrefabData>());
      // ISSUE: reference to a compiler-generated field
      this.m_InstanceBufferID = Shader.PropertyToID("instanceBuffer");
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ConfigurationQuery);
      RenderPipelineManager.beginContextRendering += (Action<ScriptableRenderContext, List<Camera>>) ((context, cameras) =>
      {
        try
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_RenderingSystem.hideOverlay)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: variable of a compiler-generated type
          NotificationIconBufferSystem.IconData iconData = this.m_BufferSystem.GetIconData();
          // ISSUE: reference to a compiler-generated field
          if (!iconData.m_InstanceData.IsCreated)
            return;
          // ISSUE: reference to a compiler-generated field
          int length = iconData.m_InstanceData.Length;
          if (length == 0)
            return;
          // ISSUE: reference to a compiler-generated field
          Bounds bounds = RenderingUtils.ToBounds(iconData.m_IconBounds.value);
          // ISSUE: reference to a compiler-generated method
          Mesh mesh = this.GetMesh();
          // ISSUE: reference to a compiler-generated method
          Material material = this.GetMaterial();
          // ISSUE: reference to a compiler-generated method
          ComputeBuffer argsBuffer = this.GetArgsBuffer();
          // ISSUE: reference to a compiler-generated field
          this.m_ArgsArray[0] = mesh.GetIndexCount(0);
          // ISSUE: reference to a compiler-generated field
          this.m_ArgsArray[1] = (uint) length;
          // ISSUE: reference to a compiler-generated field
          this.m_ArgsArray[2] = mesh.GetIndexStart(0);
          // ISSUE: reference to a compiler-generated field
          this.m_ArgsArray[3] = mesh.GetBaseVertex(0);
          // ISSUE: reference to a compiler-generated field
          this.m_ArgsArray[4] = 0U;
          // ISSUE: reference to a compiler-generated field
          argsBuffer.SetData((System.Array) this.m_ArgsArray);
          // ISSUE: reference to a compiler-generated field
          if (this.m_UpdateBuffer)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_UpdateBuffer = false;
            // ISSUE: reference to a compiler-generated method
            ComputeBuffer instanceBuffer = this.GetInstanceBuffer(length);
            // ISSUE: reference to a compiler-generated field
            instanceBuffer.SetData<NotificationIconBufferSystem.InstanceData>(iconData.m_InstanceData, 0, 0, length);
            // ISSUE: reference to a compiler-generated field
            material.SetBuffer(this.m_InstanceBufferID, instanceBuffer);
          }
          foreach (Camera camera in cameras)
          {
            if (camera.cameraType == CameraType.Game || camera.cameraType == CameraType.SceneView)
              Graphics.DrawMeshInstancedIndirect(mesh, 0, material, bounds, argsBuffer, castShadows: ShadowCastingMode.Off, receiveShadows: false, camera: camera);
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
          // ISSUE: reference to a compiler-generated field
          if (this.m_RenderingSystem.hideOverlay)
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: variable of a compiler-generated type
          NotificationIconBufferSystem.IconData iconData = this.m_BufferSystem.GetIconData();
          // ISSUE: reference to a compiler-generated field
          if (!iconData.m_InstanceData.IsCreated)
            return;
          // ISSUE: reference to a compiler-generated field
          int length = iconData.m_InstanceData.Length;
          if (length == 0)
            return;
          // ISSUE: reference to a compiler-generated field
          Bounds bounds = RenderingUtils.ToBounds(iconData.m_IconBounds.value);
          // ISSUE: reference to a compiler-generated method
          Mesh mesh = this.GetMesh();
          // ISSUE: reference to a compiler-generated method
          Material material = this.GetMaterial();
          // ISSUE: reference to a compiler-generated method
          ComputeBuffer argsBuffer = this.GetArgsBuffer();
          // ISSUE: reference to a compiler-generated field
          this.m_ArgsArray[0] = mesh.GetIndexCount(0);
          // ISSUE: reference to a compiler-generated field
          this.m_ArgsArray[1] = (uint) length;
          // ISSUE: reference to a compiler-generated field
          this.m_ArgsArray[2] = mesh.GetIndexStart(0);
          // ISSUE: reference to a compiler-generated field
          this.m_ArgsArray[3] = mesh.GetBaseVertex(0);
          // ISSUE: reference to a compiler-generated field
          this.m_ArgsArray[4] = 0U;
          // ISSUE: reference to a compiler-generated field
          argsBuffer.SetData((System.Array) this.m_ArgsArray);
          // ISSUE: reference to a compiler-generated field
          if (this.m_UpdateBuffer)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_UpdateBuffer = false;
            // ISSUE: reference to a compiler-generated method
            ComputeBuffer instanceBuffer = this.GetInstanceBuffer(length);
            // ISSUE: reference to a compiler-generated field
            instanceBuffer.SetData<NotificationIconBufferSystem.InstanceData>(iconData.m_InstanceData, 0, 0, length);
            // ISSUE: reference to a compiler-generated field
            material.SetBuffer(this.m_InstanceBufferID, instanceBuffer);
          }
          foreach (Camera camera in cameras)
          {
            if (camera.cameraType == CameraType.Game || camera.cameraType == CameraType.SceneView)
              Graphics.DrawMeshInstancedIndirect(mesh, 0, material, bounds, argsBuffer, castShadows: ShadowCastingMode.Off, receiveShadows: false, camera: camera);
          }
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
      if ((UnityEngine.Object) this.m_Material != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_Material);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_ArgsBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ArgsBuffer.Release();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_InstanceBuffer != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_InstanceBuffer.Release();
      }
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_TextureArray != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_TextureArray);
      }
      base.OnDestroy();
    }

    [Preserve]
    protected override void OnUpdate() => this.m_UpdateBuffer = true;

    public void DisplayDataUpdated()
    {
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_Material != (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        UnityEngine.Object.Destroy((UnityEngine.Object) this.m_Material);
        // ISSUE: reference to a compiler-generated field
        this.m_Material = (Material) null;
      }
      // ISSUE: reference to a compiler-generated field
      if (!((UnityEngine.Object) this.m_TextureArray != (UnityEngine.Object) null))
        return;
      // ISSUE: reference to a compiler-generated field
      UnityEngine.Object.Destroy((UnityEngine.Object) this.m_TextureArray);
      // ISSUE: reference to a compiler-generated field
      this.m_TextureArray = (Texture2DArray) null;
    }

    private void Render(ScriptableRenderContext context, List<Camera> cameras)
    {
      try
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_RenderingSystem.hideOverlay)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: variable of a compiler-generated type
        NotificationIconBufferSystem.IconData iconData = this.m_BufferSystem.GetIconData();
        // ISSUE: reference to a compiler-generated field
        if (!iconData.m_InstanceData.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        int length = iconData.m_InstanceData.Length;
        if (length == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        Bounds bounds = RenderingUtils.ToBounds(iconData.m_IconBounds.value);
        // ISSUE: reference to a compiler-generated method
        Mesh mesh = this.GetMesh();
        // ISSUE: reference to a compiler-generated method
        Material material = this.GetMaterial();
        // ISSUE: reference to a compiler-generated method
        ComputeBuffer argsBuffer = this.GetArgsBuffer();
        // ISSUE: reference to a compiler-generated field
        this.m_ArgsArray[0] = mesh.GetIndexCount(0);
        // ISSUE: reference to a compiler-generated field
        this.m_ArgsArray[1] = (uint) length;
        // ISSUE: reference to a compiler-generated field
        this.m_ArgsArray[2] = mesh.GetIndexStart(0);
        // ISSUE: reference to a compiler-generated field
        this.m_ArgsArray[3] = mesh.GetBaseVertex(0);
        // ISSUE: reference to a compiler-generated field
        this.m_ArgsArray[4] = 0U;
        // ISSUE: reference to a compiler-generated field
        argsBuffer.SetData((System.Array) this.m_ArgsArray);
        // ISSUE: reference to a compiler-generated field
        if (this.m_UpdateBuffer)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_UpdateBuffer = false;
          // ISSUE: reference to a compiler-generated method
          ComputeBuffer instanceBuffer = this.GetInstanceBuffer(length);
          // ISSUE: reference to a compiler-generated field
          instanceBuffer.SetData<NotificationIconBufferSystem.InstanceData>(iconData.m_InstanceData, 0, 0, length);
          // ISSUE: reference to a compiler-generated field
          material.SetBuffer(this.m_InstanceBufferID, instanceBuffer);
        }
        foreach (Camera camera in cameras)
        {
          if (camera.cameraType == CameraType.Game || camera.cameraType == CameraType.SceneView)
            Graphics.DrawMeshInstancedIndirect(mesh, 0, material, bounds, argsBuffer, castShadows: ShadowCastingMode.Off, receiveShadows: false, camera: camera);
        }
      }
      finally
      {
      }
    }

    private Mesh GetMesh()
    {
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_Mesh == (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Mesh = new Mesh();
        // ISSUE: reference to a compiler-generated field
        this.m_Mesh.name = "Notification icon";
        // ISSUE: reference to a compiler-generated field
        this.m_Mesh.vertices = new Vector3[4]
        {
          new Vector3(-1f, -1f, 0.0f),
          new Vector3(-1f, 1f, 0.0f),
          new Vector3(1f, 1f, 0.0f),
          new Vector3(1f, -1f, 0.0f)
        };
        // ISSUE: reference to a compiler-generated field
        this.m_Mesh.uv = new Vector2[4]
        {
          new Vector2(0.0f, 0.0f),
          new Vector2(0.0f, 1f),
          new Vector2(1f, 1f),
          new Vector2(1f, 0.0f)
        };
        // ISSUE: reference to a compiler-generated field
        this.m_Mesh.triangles = new int[6]
        {
          0,
          1,
          2,
          2,
          3,
          0
        };
      }
      // ISSUE: reference to a compiler-generated field
      return this.m_Mesh;
    }

    private Material GetMaterial()
    {
      // ISSUE: reference to a compiler-generated field
      if ((UnityEngine.Object) this.m_Material == (UnityEngine.Object) null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        IconConfigurationPrefab prefab1 = this.m_PrefabSystem.GetPrefab<IconConfigurationPrefab>(this.m_ConfigurationQuery.GetSingletonEntity());
        // ISSUE: reference to a compiler-generated field
        this.m_Material = new Material(prefab1.m_Material);
        // ISSUE: reference to a compiler-generated field
        this.m_Material.name = "Notification icons";
        // ISSUE: reference to a compiler-generated field
        NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_PrefabQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PrefabData> componentTypeHandle1 = this.__TypeHandle.__Game_Prefabs_PrefabData_RW_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NotificationIconDisplayData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<NotificationIconDisplayData> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_NotificationIconDisplayData_RW_ComponentTypeHandle;
        try
        {
          int num = 1;
          int2 x = new int2(prefab1.m_MissingIcon.width, prefab1.m_MissingIcon.height);
          TextureFormat format = prefab1.m_MissingIcon.format;
          for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
          {
            ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
            NativeArray<PrefabData> nativeArray1 = archetypeChunk.GetNativeArray<PrefabData>(ref componentTypeHandle1);
            NativeArray<NotificationIconDisplayData> nativeArray2 = archetypeChunk.GetNativeArray<NotificationIconDisplayData>(ref componentTypeHandle2);
            for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              NotificationIconPrefab prefab2 = this.m_PrefabSystem.GetPrefab<NotificationIconPrefab>(nativeArray1[index2]);
              NotificationIconDisplayData notificationIconDisplayData = nativeArray2[index2];
              num = math.max(num, notificationIconDisplayData.m_IconIndex + 1);
              x = math.max(x, new int2(prefab2.m_Icon.width, prefab2.m_Icon.height));
              format = prefab2.m_Icon.format;
            }
          }
          Texture2DArray texture2Darray = new Texture2DArray(x.x, x.y, num, format, true);
          texture2Darray.name = "NotificationIcons";
          // ISSUE: reference to a compiler-generated field
          this.m_TextureArray = texture2Darray;
          // ISSUE: reference to a compiler-generated field
          Graphics.CopyTexture((Texture) prefab1.m_MissingIcon, 0, (Texture) this.m_TextureArray, 0);
          for (int index3 = 0; index3 < archetypeChunkArray.Length; ++index3)
          {
            ArchetypeChunk archetypeChunk = archetypeChunkArray[index3];
            NativeArray<PrefabData> nativeArray3 = archetypeChunk.GetNativeArray<PrefabData>(ref componentTypeHandle1);
            NativeArray<NotificationIconDisplayData> nativeArray4 = archetypeChunk.GetNativeArray<NotificationIconDisplayData>(ref componentTypeHandle2);
            for (int index4 = 0; index4 < nativeArray3.Length; ++index4)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated field
              Graphics.CopyTexture((Texture) this.m_PrefabSystem.GetPrefab<NotificationIconPrefab>(nativeArray3[index4]).m_Icon, 0, (Texture) this.m_TextureArray, nativeArray4[index4].m_IconIndex);
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Material.mainTexture = (Texture) this.m_TextureArray;
        }
        finally
        {
          archetypeChunkArray.Dispose();
        }
      }
      // ISSUE: reference to a compiler-generated field
      return this.m_Material;
    }

    private ComputeBuffer GetArgsBuffer()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_ArgsBuffer == null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ArgsArray = new uint[5];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ArgsBuffer = new ComputeBuffer(1, this.m_ArgsArray.Length * 4, ComputeBufferType.DrawIndirect);
        // ISSUE: reference to a compiler-generated field
        this.m_ArgsBuffer.name = "Notification args buffer";
      }
      // ISSUE: reference to a compiler-generated field
      return this.m_ArgsBuffer;
    }

    private ComputeBuffer GetInstanceBuffer(int count)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_InstanceBuffer != null && this.m_InstanceBuffer.count < count)
      {
        // ISSUE: reference to a compiler-generated field
        count = math.max(this.m_InstanceBuffer.count * 2, count);
        // ISSUE: reference to a compiler-generated field
        this.m_InstanceBuffer.Release();
        // ISSUE: reference to a compiler-generated field
        this.m_InstanceBuffer = (ComputeBuffer) null;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_InstanceBuffer == null)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_InstanceBuffer = new ComputeBuffer(math.max(64, count), sizeof (NotificationIconBufferSystem.InstanceData));
        // ISSUE: reference to a compiler-generated field
        this.m_InstanceBuffer.name = "Notification instance buffer";
      }
      // ISSUE: reference to a compiler-generated field
      return this.m_InstanceBuffer;
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
    public NotificationIconRenderSystem()
    {
    }

    private struct TypeHandle
    {
      public ComponentTypeHandle<PrefabData> __Game_Prefabs_PrefabData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<NotificationIconDisplayData> __Game_Prefabs_NotificationIconDisplayData_RW_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NotificationIconDisplayData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<NotificationIconDisplayData>();
      }
    }
  }
}
