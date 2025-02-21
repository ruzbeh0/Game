// Decompiled with JetBrains decompiler
// Type: Game.Rendering.NetMeshHelpers
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.IO.AssetDatabase;
using Game.Prefabs;
using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

#nullable disable
namespace Game.Rendering
{
  public static class NetMeshHelpers
  {
    private static readonly float3 v_left = new float3(-1f, 0.0f, 0.0f);
    private static readonly float3 v_up = new float3(0.0f, 1f, 0.0f);
    private static readonly float3 v_right = new float3(1f, 0.0f, 0.0f);
    private static readonly float3 v_down = new float3(0.0f, -1f, 0.0f);
    private static readonly float3 v_forward = new float3(0.0f, 0.0f, 1f);
    private static readonly float3 v_backward = new float3(0.0f, 0.0f, -1f);

    public static Mesh CreateDefaultLaneMesh()
    {
      int num = 4;
      int length1 = num * 8 + 16;
      int length2 = num * 24 + 12;
      Vector3[] vertices = new Vector3[length1];
      Vector3[] normals = new Vector3[length1];
      Vector4[] tangents = new Vector4[length1];
      Vector2[] uvs = new Vector2[length1];
      int[] indices = new int[length2];
      int vertexIndex = 0;
      int indexIndex = 0;
      NetMeshHelpers.AddVertex(vertices, normals, tangents, uvs, ref vertexIndex, new float3(-1f, -1f, -1f), NetMeshHelpers.v_backward, NetMeshHelpers.v_right, new float2(0.0f, 0.0f));
      NetMeshHelpers.AddVertex(vertices, normals, tangents, uvs, ref vertexIndex, new float3(-1f, 1f, -1f), NetMeshHelpers.v_backward, NetMeshHelpers.v_right, new float2(0.0f, 1f));
      NetMeshHelpers.AddVertex(vertices, normals, tangents, uvs, ref vertexIndex, new float3(1f, 1f, -1f), NetMeshHelpers.v_backward, NetMeshHelpers.v_right, new float2(1f, 1f));
      NetMeshHelpers.AddVertex(vertices, normals, tangents, uvs, ref vertexIndex, new float3(1f, -1f, -1f), NetMeshHelpers.v_backward, NetMeshHelpers.v_right, new float2(1f, 0.0f));
      NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 4, vertexIndex - 3, vertexIndex - 2, vertexIndex - 1);
      for (int index = 0; index <= num; ++index)
      {
        float y = (float) index / (float) num;
        float z = (float) ((double) y * 2.0 - 1.0);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, uvs, ref vertexIndex, new float3(-1f, -1f, z), NetMeshHelpers.v_left, NetMeshHelpers.v_up, new float2(0.0f, y));
        NetMeshHelpers.AddVertex(vertices, normals, tangents, uvs, ref vertexIndex, new float3(-1f, 1f, z), NetMeshHelpers.v_left, NetMeshHelpers.v_up, new float2(1f, y));
        NetMeshHelpers.AddVertex(vertices, normals, tangents, uvs, ref vertexIndex, new float3(-1f, 1f, z), NetMeshHelpers.v_up, NetMeshHelpers.v_right, new float2(0.0f, y));
        NetMeshHelpers.AddVertex(vertices, normals, tangents, uvs, ref vertexIndex, new float3(1f, 1f, z), NetMeshHelpers.v_up, NetMeshHelpers.v_right, new float2(1f, y));
        NetMeshHelpers.AddVertex(vertices, normals, tangents, uvs, ref vertexIndex, new float3(1f, 1f, z), NetMeshHelpers.v_right, NetMeshHelpers.v_down, new float2(0.0f, y));
        NetMeshHelpers.AddVertex(vertices, normals, tangents, uvs, ref vertexIndex, new float3(1f, -1f, z), NetMeshHelpers.v_right, NetMeshHelpers.v_down, new float2(1f, y));
        NetMeshHelpers.AddVertex(vertices, normals, tangents, uvs, ref vertexIndex, new float3(1f, -1f, z), NetMeshHelpers.v_down, NetMeshHelpers.v_left, new float2(0.0f, y));
        NetMeshHelpers.AddVertex(vertices, normals, tangents, uvs, ref vertexIndex, new float3(-1f, -1f, z), NetMeshHelpers.v_down, NetMeshHelpers.v_left, new float2(1f, y));
        if (index != 0)
        {
          NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 16, vertexIndex - 8, vertexIndex - 7, vertexIndex - 15);
          NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 14, vertexIndex - 6, vertexIndex - 5, vertexIndex - 13);
          NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 12, vertexIndex - 4, vertexIndex - 3, vertexIndex - 11);
          NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 10, vertexIndex - 2, vertexIndex - 1, vertexIndex - 9);
        }
      }
      NetMeshHelpers.AddVertex(vertices, normals, tangents, uvs, ref vertexIndex, new float3(1f, -1f, 1f), NetMeshHelpers.v_forward, NetMeshHelpers.v_left, new float2(0.0f, 0.0f));
      NetMeshHelpers.AddVertex(vertices, normals, tangents, uvs, ref vertexIndex, new float3(1f, 1f, 1f), NetMeshHelpers.v_forward, NetMeshHelpers.v_left, new float2(0.0f, 1f));
      NetMeshHelpers.AddVertex(vertices, normals, tangents, uvs, ref vertexIndex, new float3(-1f, 1f, 1f), NetMeshHelpers.v_forward, NetMeshHelpers.v_left, new float2(1f, 1f));
      NetMeshHelpers.AddVertex(vertices, normals, tangents, uvs, ref vertexIndex, new float3(-1f, -1f, 1f), NetMeshHelpers.v_forward, NetMeshHelpers.v_left, new float2(1f, 0.0f));
      NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 4, vertexIndex - 3, vertexIndex - 2, vertexIndex - 1);
      Mesh defaultLaneMesh = new Mesh();
      defaultLaneMesh.name = "Default lane";
      defaultLaneMesh.vertices = vertices;
      defaultLaneMesh.normals = normals;
      defaultLaneMesh.tangents = tangents;
      defaultLaneMesh.uv = uvs;
      defaultLaneMesh.triangles = indices;
      return defaultLaneMesh;
    }

    public static Mesh CreateDefaultRoundaboutMesh()
    {
      int num = 4;
      int length1 = num * 10 + 22;
      int length2 = num * 36 + 24;
      Vector3[] vertices = new Vector3[length1];
      Vector3[] normals = new Vector3[length1];
      Vector4[] tangents = new Vector4[length1];
      Color32[] colors = new Color32[length1];
      Vector4[] uvs = new Vector4[length1];
      int[] indices = new int[length2];
      int vertexIndex = 0;
      int indexIndex = 0;
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_backward, NetMeshHelpers.v_right, new float2(0.0f, -2f), new int2(0, 4), 0.0f, -2f, 0.0f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_backward, NetMeshHelpers.v_right, new float2(0.0f, -1f), new int2(0, 4), 0.0f, 0.0f, 0.0f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_backward, NetMeshHelpers.v_right, new float2(0.5f, -1f), new int2(4, 2), 0.0f, 0.0f, 0.0f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_backward, NetMeshHelpers.v_right, new float2(1f, -1f), new int2(4, 2), 1f, 0.0f, 0.0f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_backward, NetMeshHelpers.v_right, new float2(1f, -2f), new int2(4, 2), 1f, -2f, 0.0f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_backward, NetMeshHelpers.v_right, new float2(0.5f, -2f), new int2(4, 2), 0.0f, -2f, 0.0f);
      NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 6, vertexIndex - 5, vertexIndex - 4, vertexIndex - 1);
      NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 1, vertexIndex - 4, vertexIndex - 3, vertexIndex - 2);
      for (int index = 0; index <= num; ++index)
      {
        int3 int3 = new int3(0, 4, 2);
        float tz = (float) index / ((float) num * 0.5f);
        float y = tz - 3f;
        if (index >= num >> 1)
        {
          int3 += 1;
          --tz;
        }
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_left, NetMeshHelpers.v_up, new float2(0.0f, y), int3.xy, 0.0f, -2f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_left, NetMeshHelpers.v_up, new float2(1f, y), int3.xy, 0.0f, 0.0f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_up, NetMeshHelpers.v_right, new float2(0.0f, y), int3.xy, 0.0f, 0.0f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_up, NetMeshHelpers.v_right, new float2(0.5f, y), int3.yz, 0.0f, 0.0f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_up, NetMeshHelpers.v_right, new float2(1f, y), int3.yz, 1f, 0.0f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_right, NetMeshHelpers.v_down, new float2(0.0f, y), int3.yz, 1f, 0.0f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_right, NetMeshHelpers.v_down, new float2(1f, y), int3.yz, 1f, -2f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_down, NetMeshHelpers.v_left, new float2(0.0f, y), int3.yz, 1f, -2f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_down, NetMeshHelpers.v_left, new float2(0.5f, y), int3.yz, 0.0f, -2f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_down, NetMeshHelpers.v_left, new float2(1f, y), int3.xy, 0.0f, -2f, tz);
        if (index != 0)
        {
          NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 20, vertexIndex - 10, vertexIndex - 9, vertexIndex - 19);
          NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 18, vertexIndex - 8, vertexIndex - 7, vertexIndex - 17);
          NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 17, vertexIndex - 7, vertexIndex - 6, vertexIndex - 16);
          NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 15, vertexIndex - 5, vertexIndex - 4, vertexIndex - 14);
          NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 13, vertexIndex - 3, vertexIndex - 2, vertexIndex - 12);
          NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 12, vertexIndex - 2, vertexIndex - 1, vertexIndex - 11);
        }
      }
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_forward, NetMeshHelpers.v_left, new float2(0.0f, -2f), new int2(5, 3), 1f, -2f, 1f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_forward, NetMeshHelpers.v_left, new float2(0.0f, -1f), new int2(5, 3), 1f, 0.0f, 1f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_forward, NetMeshHelpers.v_left, new float2(0.5f, -1f), new int2(5, 3), 0.0f, 0.0f, 1f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_forward, NetMeshHelpers.v_left, new float2(1f, -1f), new int2(1, 5), 0.0f, 0.0f, 1f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_forward, NetMeshHelpers.v_left, new float2(1f, -2f), new int2(1, 5), 0.0f, -2f, 1f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_forward, NetMeshHelpers.v_left, new float2(0.5f, -2f), new int2(5, 3), 0.0f, -2f, 1f);
      NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 6, vertexIndex - 5, vertexIndex - 4, vertexIndex - 1);
      NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 1, vertexIndex - 4, vertexIndex - 3, vertexIndex - 2);
      return NetMeshHelpers.CreateMesh("Default roundabout", vertices, normals, tangents, colors, uvs, indices);
    }

    public static Mesh CreateDefaultNodeMesh()
    {
      int num = 2;
      int length1 = num * 14 + 34;
      int length2 = num * 60 + 48;
      Vector3[] vertices = new Vector3[length1];
      Vector3[] normals = new Vector3[length1];
      Vector4[] tangents = new Vector4[length1];
      Color32[] colors = new Color32[length1];
      Vector4[] uvs = new Vector4[length1];
      int[] indices = new int[length2];
      int vertexIndex = 0;
      int indexIndex = 0;
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_backward, NetMeshHelpers.v_right, new float2(0.0f, -2f), new int2(0, 2), 0.0f, -2f, 0.0f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_backward, NetMeshHelpers.v_right, new float2(0.0f, -1f), new int2(0, 2), 0.0f, 0.0f, 0.0f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_backward, NetMeshHelpers.v_right, new float2(0.25f, -1f), new int2(0, 2), 1f, 0.0f, 0.0f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_backward, NetMeshHelpers.v_right, new float2(0.5f, -1f), new int2(4, 0), 0.0f, 0.0f, 0.0f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_backward, NetMeshHelpers.v_right, new float2(0.75f, -1f), new int2(1, 3), 0.0f, 0.0f, 0.0f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_backward, NetMeshHelpers.v_right, new float2(1f, -1f), new int2(1, 3), 1f, 0.0f, 0.0f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_backward, NetMeshHelpers.v_right, new float2(1f, -2f), new int2(1, 3), 1f, -2f, 0.0f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_backward, NetMeshHelpers.v_right, new float2(0.75f, -2f), new int2(1, 3), 0.0f, -2f, 0.0f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_backward, NetMeshHelpers.v_right, new float2(0.5f, -2f), new int2(4, 0), 0.0f, -2f, 0.0f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_backward, NetMeshHelpers.v_right, new float2(0.25f, -2f), new int2(0, 2), 1f, -2f, 0.0f);
      NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 10, vertexIndex - 9, vertexIndex - 8, vertexIndex - 1);
      NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 1, vertexIndex - 8, vertexIndex - 7, vertexIndex - 2);
      NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 2, vertexIndex - 7, vertexIndex - 6, vertexIndex - 3);
      NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 3, vertexIndex - 6, vertexIndex - 5, vertexIndex - 4);
      for (int index = 0; index <= num; ++index)
      {
        float tz = (float) index / (float) num;
        float y = tz - 2f;
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_left, NetMeshHelpers.v_up, new float2(0.0f, y), new int2(0, 2), 0.0f, -2f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_left, NetMeshHelpers.v_up, new float2(1f, y), new int2(0, 2), 0.0f, 0.0f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_up, NetMeshHelpers.v_right, new float2(0.0f, y), new int2(0, 2), 0.0f, 0.0f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_up, NetMeshHelpers.v_right, new float2(0.25f, y), new int2(0, 2), 1f, 0.0f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_up, NetMeshHelpers.v_right, new float2(0.5f, y), new int2(4, 0), 0.0f, 0.0f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_up, NetMeshHelpers.v_right, new float2(0.75f, y), new int2(1, 3), 0.0f, 0.0f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_up, NetMeshHelpers.v_right, new float2(1f, y), new int2(1, 3), 1f, 0.0f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_right, NetMeshHelpers.v_down, new float2(0.0f, y), new int2(1, 3), 1f, 0.0f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_right, NetMeshHelpers.v_down, new float2(1f, y), new int2(1, 3), 1f, -2f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_down, NetMeshHelpers.v_left, new float2(0.0f, y), new int2(1, 3), 1f, -2f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_down, NetMeshHelpers.v_left, new float2(0.25f, y), new int2(1, 3), 0.0f, -2f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_down, NetMeshHelpers.v_left, new float2(0.5f, y), new int2(4, 0), 0.0f, -2f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_down, NetMeshHelpers.v_left, new float2(0.75f, y), new int2(0, 2), 1f, -2f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_down, NetMeshHelpers.v_left, new float2(1f, y), new int2(0, 2), 0.0f, -2f, tz);
        if (index != 0)
        {
          NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 28, vertexIndex - 14, vertexIndex - 13, vertexIndex - 27);
          NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 26, vertexIndex - 12, vertexIndex - 11, vertexIndex - 25);
          NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 25, vertexIndex - 11, vertexIndex - 10, vertexIndex - 24);
          NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 24, vertexIndex - 10, vertexIndex - 9, vertexIndex - 23);
          NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 23, vertexIndex - 9, vertexIndex - 8, vertexIndex - 22);
          NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 21, vertexIndex - 7, vertexIndex - 6, vertexIndex - 20);
          NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 19, vertexIndex - 5, vertexIndex - 4, vertexIndex - 18);
          NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 18, vertexIndex - 4, vertexIndex - 3, vertexIndex - 17);
          NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 17, vertexIndex - 3, vertexIndex - 2, vertexIndex - 16);
          NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 16, vertexIndex - 2, vertexIndex - 1, vertexIndex - 15);
        }
      }
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_forward, NetMeshHelpers.v_left, new float2(0.0f, -2f), new int2(1, 3), 1f, -2f, 1f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_forward, NetMeshHelpers.v_left, new float2(0.0f, -1f), new int2(1, 3), 1f, 0.0f, 1f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_forward, NetMeshHelpers.v_left, new float2(0.25f, -1f), new int2(1, 3), 0.0f, 0.0f, 1f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_forward, NetMeshHelpers.v_left, new float2(0.5f, -1f), new int2(4, 0), 0.0f, 0.0f, 1f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_forward, NetMeshHelpers.v_left, new float2(0.75f, -1f), new int2(0, 2), 1f, 0.0f, 1f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_forward, NetMeshHelpers.v_left, new float2(1f, -1f), new int2(0, 2), 0.0f, 0.0f, 1f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_forward, NetMeshHelpers.v_left, new float2(1f, -2f), new int2(0, 2), 0.0f, -2f, 1f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_forward, NetMeshHelpers.v_left, new float2(0.75f, -2f), new int2(0, 2), 1f, -2f, 1f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_forward, NetMeshHelpers.v_left, new float2(0.5f, -2f), new int2(4, 0), 0.0f, -2f, 1f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_forward, NetMeshHelpers.v_left, new float2(0.25f, -2f), new int2(1, 3), 0.0f, -2f, 1f);
      NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 10, vertexIndex - 9, vertexIndex - 8, vertexIndex - 1);
      NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 1, vertexIndex - 8, vertexIndex - 7, vertexIndex - 2);
      NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 2, vertexIndex - 7, vertexIndex - 6, vertexIndex - 3);
      NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 3, vertexIndex - 6, vertexIndex - 5, vertexIndex - 4);
      return NetMeshHelpers.CreateMesh("Default node", vertices, normals, tangents, colors, uvs, indices);
    }

    public static Mesh CreateDefaultEdgeMesh()
    {
      int num = 4;
      int length1 = num * 8 + 16;
      int length2 = num * 24 + 12;
      Vector3[] vertices = new Vector3[length1];
      Vector3[] normals = new Vector3[length1];
      Vector4[] tangents = new Vector4[length1];
      Color32[] colors = new Color32[length1];
      Vector4[] uvs = new Vector4[length1];
      int[] indices = new int[length2];
      int vertexIndex = 0;
      int indexIndex = 0;
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_backward, NetMeshHelpers.v_right, new float2(0.0f, -2f), new int2(0, 2), 0.0f, -2f, 0.0f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_backward, NetMeshHelpers.v_right, new float2(0.0f, -1f), new int2(0, 2), 0.0f, 0.0f, 0.0f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_backward, NetMeshHelpers.v_right, new float2(1f, -1f), new int2(0, 2), 1f, 0.0f, 0.0f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_backward, NetMeshHelpers.v_right, new float2(1f, -2f), new int2(0, 2), 1f, -2f, 0.0f);
      NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 4, vertexIndex - 3, vertexIndex - 2, vertexIndex - 1);
      for (int index = 0; index <= num; ++index)
      {
        int2 m = new int2(0, 2);
        float tz = (float) index / ((float) num * 0.5f);
        float y = tz - 3f;
        if (index >= num >> 1)
        {
          m += 1;
          --tz;
        }
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_left, NetMeshHelpers.v_up, new float2(0.0f, y), m, 0.0f, -2f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_left, NetMeshHelpers.v_up, new float2(1f, y), m, 0.0f, 0.0f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_up, NetMeshHelpers.v_right, new float2(0.0f, y), m, 0.0f, 0.0f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_up, NetMeshHelpers.v_right, new float2(1f, y), m, 1f, 0.0f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_right, NetMeshHelpers.v_down, new float2(0.0f, y), m, 1f, 0.0f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_right, NetMeshHelpers.v_down, new float2(1f, y), m, 1f, -2f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_down, NetMeshHelpers.v_left, new float2(0.0f, y), m, 1f, -2f, tz);
        NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_down, NetMeshHelpers.v_left, new float2(1f, y), m, 0.0f, -2f, tz);
        if (index != 0)
        {
          NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 16, vertexIndex - 8, vertexIndex - 7, vertexIndex - 15);
          NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 14, vertexIndex - 6, vertexIndex - 5, vertexIndex - 13);
          NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 12, vertexIndex - 4, vertexIndex - 3, vertexIndex - 11);
          NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 10, vertexIndex - 2, vertexIndex - 1, vertexIndex - 9);
        }
      }
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_forward, NetMeshHelpers.v_left, new float2(0.0f, -2f), new int2(1, 3), 1f, -2f, 1f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_forward, NetMeshHelpers.v_left, new float2(0.0f, -1f), new int2(1, 3), 1f, 0.0f, 1f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_forward, NetMeshHelpers.v_left, new float2(1f, -1f), new int2(1, 3), 0.0f, 0.0f, 1f);
      NetMeshHelpers.AddVertex(vertices, normals, tangents, colors, uvs, ref vertexIndex, NetMeshHelpers.v_forward, NetMeshHelpers.v_left, new float2(1f, -2f), new int2(1, 3), 0.0f, -2f, 1f);
      NetMeshHelpers.AddQuad(indices, ref indexIndex, vertexIndex - 4, vertexIndex - 3, vertexIndex - 2, vertexIndex - 1);
      return NetMeshHelpers.CreateMesh("Default edge", vertices, normals, tangents, colors, uvs, indices);
    }

    public static JobHandle CacheMeshData(
      GeometryAsset meshData,
      Entity entity,
      EntityManager entityManager,
      EntityCommandBuffer commandBuffer)
    {
      DynamicBuffer<MeshMaterial> buffer = entityManager.GetBuffer<MeshMaterial>(entity);
      int num1 = 0;
      int num2 = 0;
      int num3 = 0;
      for (int meshIndex = 0; meshIndex < meshData.meshCount; ++meshIndex)
      {
        int subMeshCount = meshData.GetSubMeshCount(meshIndex);
        for (int subMeshIndex = 0; subMeshIndex < subMeshCount; ++subMeshIndex)
        {
          SubMeshDescriptor subMeshDesc = meshData.GetSubMeshDesc(meshIndex, subMeshIndex);
          ref MeshMaterial local = ref buffer.ElementAt(num1++);
          local.m_StartIndex = num2 + subMeshDesc.indexStart;
          local.m_IndexCount = subMeshDesc.indexCount;
          local.m_StartVertex = num3 + subMeshDesc.firstVertex;
          local.m_VertexCount = subMeshDesc.vertexCount;
        }
        num2 += meshData.GetIndicesCount(meshIndex);
        num3 += meshData.GetVertexCount(meshIndex);
      }
      return new NetMeshHelpers.CacheMeshDataJob()
      {
        m_Data = meshData.data,
        m_Entity = entity,
        m_CommandBuffer = commandBuffer
      }.Schedule<NetMeshHelpers.CacheMeshDataJob>();
    }

    public static void CacheMeshData(
      Mesh mesh,
      Entity entity,
      EntityManager entityManager,
      EntityCommandBuffer commandBuffer)
    {
      DynamicBuffer<MeshVertex> dynamicBuffer1 = commandBuffer.AddBuffer<MeshVertex>(entity);
      DynamicBuffer<MeshNormal> dynamicBuffer2 = commandBuffer.AddBuffer<MeshNormal>(entity);
      DynamicBuffer<MeshTangent> dynamicBuffer3 = commandBuffer.AddBuffer<MeshTangent>(entity);
      DynamicBuffer<MeshUV0> dynamicBuffer4 = commandBuffer.AddBuffer<MeshUV0>(entity);
      DynamicBuffer<MeshIndex> dynamicBuffer5 = commandBuffer.AddBuffer<MeshIndex>(entity);
      DynamicBuffer<MeshMaterial> buffer = entityManager.GetBuffer<MeshMaterial>(entity);
      Mesh.MeshDataArray meshDataArray = Mesh.AcquireReadOnlyMeshData(mesh);
      Mesh.MeshData meshData = meshDataArray[0];
      int length = 0;
      int subMeshCount = meshData.subMeshCount;
      for (int index = 0; index < subMeshCount; ++index)
        length += meshData.GetSubMesh(index).indexCount;
      dynamicBuffer1.ResizeUninitialized(meshData.vertexCount);
      dynamicBuffer2.ResizeUninitialized(meshData.vertexCount);
      dynamicBuffer3.ResizeUninitialized(meshData.vertexCount);
      dynamicBuffer4.ResizeUninitialized(meshData.vertexCount);
      dynamicBuffer5.ResizeUninitialized(length);
      meshData.GetVertices(dynamicBuffer1.AsNativeArray().Reinterpret<Vector3>());
      meshData.GetNormals(dynamicBuffer2.AsNativeArray().Reinterpret<Vector3>());
      meshData.GetTangents(dynamicBuffer3.AsNativeArray().Reinterpret<Vector4>());
      meshData.GetUVs(0, dynamicBuffer4.AsNativeArray().Reinterpret<Vector2>());
      int start = 0;
      for (int index = 0; index < subMeshCount; ++index)
      {
        SubMeshDescriptor subMesh = meshData.GetSubMesh(index);
        meshData.GetIndices(dynamicBuffer5.AsNativeArray().GetSubArray(start, subMesh.indexCount).Reinterpret<int>(), index);
        MeshMaterial meshMaterial = buffer[index] with
        {
          m_StartIndex = subMesh.indexStart,
          m_IndexCount = subMesh.indexCount,
          m_StartVertex = subMesh.firstVertex,
          m_VertexCount = subMesh.vertexCount
        };
        buffer[index] = meshMaterial;
        start += subMesh.indexCount;
      }
      meshDataArray.Dispose();
    }

    public static void UncacheMeshData(Entity entity, EntityCommandBuffer commandBuffer)
    {
      commandBuffer.RemoveComponent<MeshVertex>(entity);
      commandBuffer.RemoveComponent<MeshNormal>(entity);
      commandBuffer.RemoveComponent<MeshTangent>(entity);
      commandBuffer.RemoveComponent<MeshUV0>(entity);
      commandBuffer.RemoveComponent<MeshIndex>(entity);
    }

    private static void AddVertex(
      Vector3[] vertices,
      Vector3[] normals,
      Vector4[] tangents,
      Vector2[] uvs,
      ref int vertexIndex,
      float3 position,
      float3 normal,
      float3 tangent,
      float2 uv)
    {
      vertices[vertexIndex] = (Vector3) position;
      normals[vertexIndex] = (Vector3) normal;
      tangents[vertexIndex] = (Vector4) new float4(tangent, -1f);
      uvs[vertexIndex] = (Vector2) uv;
      ++vertexIndex;
    }

    private static void AddQuad(int[] indices, ref int indexIndex, int a, int b, int c, int d)
    {
      indices[indexIndex++] = a;
      indices[indexIndex++] = b;
      indices[indexIndex++] = c;
      indices[indexIndex++] = c;
      indices[indexIndex++] = d;
      indices[indexIndex++] = a;
    }

    private static Mesh CreateMesh(
      string name,
      Vector3[] vertices,
      Vector3[] normals,
      Vector4[] tangents,
      Color32[] colors,
      Vector4[] uvs,
      int[] indices)
    {
      Mesh mesh = new Mesh();
      mesh.name = name;
      mesh.vertices = vertices;
      mesh.normals = normals;
      mesh.tangents = tangents;
      mesh.colors32 = colors;
      mesh.SetUVs(0, uvs);
      mesh.triangles = indices;
      mesh.bounds = new Bounds(Vector3.zero, new Vector3(1000f, 1000f, 1000f));
      return mesh;
    }

    private static void AddVertex(
      Vector3[] vertices,
      Vector3[] normals,
      Vector4[] tangents,
      Color32[] colors,
      Vector4[] uvs,
      ref int vertexIndex,
      float3 normal,
      float3 tangent,
      float2 uv,
      int2 m,
      float tx,
      float y,
      float tz)
    {
      vertices[vertexIndex] = new Vector3(tx, y, tz);
      normals[vertexIndex] = (Vector3) normal;
      tangents[vertexIndex] = (Vector4) new float4(tangent, -1f);
      colors[vertexIndex] = new Color32((byte) m.x, (byte) m.y, (byte) 0, (byte) 0);
      uvs[vertexIndex] = new Vector4(uv.x, uv.y, 0.5f, 0.0f);
      ++vertexIndex;
    }

    [BurstCompile]
    private struct CacheMeshDataJob : IJob
    {
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeSlice<byte> m_Positions;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeSlice<byte> m_Normals;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeSlice<byte> m_Tangents;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeSlice<byte> m_TexCoords0;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeArray<byte> m_Indices;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public GeometryAsset.Data m_Data;
      [ReadOnly]
      public Entity m_Entity;
      [ReadOnly]
      public IndexFormat m_IndexFormat;
      [ReadOnly]
      public int m_VertexCount;
      [ReadOnly]
      public int m_IndexCount;
      [ReadOnly]
      public VertexAttributeFormat m_PositionsFormat;
      [ReadOnly]
      public VertexAttributeFormat m_NormalsFormat;
      [ReadOnly]
      public VertexAttributeFormat m_TangentsFormat;
      [ReadOnly]
      public VertexAttributeFormat m_TexCoords0Format;
      [ReadOnly]
      public int m_PositionsDim;
      [ReadOnly]
      public int m_NormalsDim;
      [ReadOnly]
      public int m_TangentsDim;
      [ReadOnly]
      public int m_TexCoords0Dim;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute()
      {
        DynamicBuffer<MeshVertex> dst1 = this.m_CommandBuffer.AddBuffer<MeshVertex>(this.m_Entity);
        DynamicBuffer<MeshNormal> dst2 = this.m_CommandBuffer.AddBuffer<MeshNormal>(this.m_Entity);
        DynamicBuffer<MeshTangent> dst3 = this.m_CommandBuffer.AddBuffer<MeshTangent>(this.m_Entity);
        DynamicBuffer<MeshUV0> dst4 = this.m_CommandBuffer.AddBuffer<MeshUV0>(this.m_Entity);
        DynamicBuffer<MeshIndex> dst5 = this.m_CommandBuffer.AddBuffer<MeshIndex>(this.m_Entity);
        if (this.m_Data.IsValid)
        {
          int allVertexCount = GeometryAsset.GetAllVertexCount(ref this.m_Data);
          int allIndicesCount = GeometryAsset.GetAllIndicesCount(ref this.m_Data);
          dst1.ResizeUninitialized(allVertexCount);
          dst2.ResizeUninitialized(allVertexCount);
          dst3.ResizeUninitialized(allVertexCount);
          dst4.ResizeUninitialized(allVertexCount);
          dst5.ResizeUninitialized(allIndicesCount);
          int num = 0;
          int start = 0;
          for (int meshIndex = 0; meshIndex < this.m_Data.meshCount; ++meshIndex)
          {
            int vertexCount = GeometryAsset.GetVertexCount(ref this.m_Data, meshIndex);
            int indicesCount = GeometryAsset.GetIndicesCount(ref this.m_Data, meshIndex);
            VertexAttributeFormat format1;
            int dimension1;
            GeometryAsset.GetAttributeFormat(ref this.m_Data, meshIndex, VertexAttribute.Position, out format1, out dimension1);
            VertexAttributeFormat format2;
            int dimension2;
            GeometryAsset.GetAttributeFormat(ref this.m_Data, meshIndex, VertexAttribute.Normal, out format2, out dimension2);
            VertexAttributeFormat format3;
            int dimension3;
            GeometryAsset.GetAttributeFormat(ref this.m_Data, meshIndex, VertexAttribute.Tangent, out format3, out dimension3);
            VertexAttributeFormat format4;
            int dimension4;
            GeometryAsset.GetAttributeFormat(ref this.m_Data, meshIndex, VertexAttribute.TexCoord0, out format4, out dimension4);
            IndexFormat indexFormat = GeometryAsset.GetIndexFormat(ref this.m_Data, meshIndex);
            if (dimension1 == 0)
              throw new Exception("Cannot cache geometry asset: mesh do not have a position");
            if (dimension2 == 0)
              throw new Exception("Cannot cache geometry asset: mesh do not have a normal");
            if (dimension3 == 0)
              throw new Exception("Cannot cache geometry asset: mesh do not have a tangent");
            if (dimension4 == 0)
              throw new Exception("Cannot cache geometry asset: mesh do not have a UV0");
            NativeSlice<byte> attributeData1 = GeometryAsset.GetAttributeData(ref this.m_Data, meshIndex, VertexAttribute.Position);
            NativeSlice<byte> attributeData2 = GeometryAsset.GetAttributeData(ref this.m_Data, meshIndex, VertexAttribute.Normal);
            NativeSlice<byte> attributeData3 = GeometryAsset.GetAttributeData(ref this.m_Data, meshIndex, VertexAttribute.Tangent);
            NativeSlice<byte> attributeData4 = GeometryAsset.GetAttributeData(ref this.m_Data, meshIndex, VertexAttribute.TexCoord0);
            NativeArray<byte> indices = GeometryAsset.GetIndices(ref this.m_Data, meshIndex);
            NativeArray<MeshVertex> subArray1 = dst1.AsNativeArray().GetSubArray(num, vertexCount);
            NativeArray<MeshNormal> subArray2 = dst2.AsNativeArray().GetSubArray(num, vertexCount);
            NativeArray<MeshTangent> subArray3 = dst3.AsNativeArray().GetSubArray(num, vertexCount);
            NativeArray<MeshUV0> subArray4 = dst4.AsNativeArray().GetSubArray(num, vertexCount);
            NativeArray<MeshIndex> subArray5 = dst5.AsNativeArray().GetSubArray(start, indicesCount);
            MeshVertex.Unpack(attributeData1, subArray1, vertexCount, format1, dimension1);
            MeshNormal.Unpack(attributeData2, subArray2, vertexCount, format2, dimension2);
            MeshTangent.Unpack(attributeData3, subArray3, vertexCount, format3, dimension3);
            NativeArray<MeshUV0> dst6 = subArray4;
            int count = vertexCount;
            int format5 = (int) format4;
            int dimension5 = dimension4;
            MeshUV0.Unpack(attributeData4, dst6, count, (VertexAttributeFormat) format5, dimension5);
            MeshIndex.Unpack(indices, subArray5, indicesCount, indexFormat, num);
            num += vertexCount;
            start += indicesCount;
          }
        }
        else
        {
          MeshVertex.Unpack(this.m_Positions, dst1, this.m_VertexCount, this.m_PositionsFormat, this.m_PositionsDim);
          MeshNormal.Unpack(this.m_Normals, dst2, this.m_VertexCount, this.m_NormalsFormat, this.m_NormalsDim);
          MeshTangent.Unpack(this.m_Tangents, dst3, this.m_VertexCount, this.m_TangentsFormat, this.m_TangentsDim);
          MeshUV0.Unpack(this.m_TexCoords0, dst4, this.m_VertexCount, this.m_TexCoords0Format, this.m_TexCoords0Dim);
          MeshIndex.Unpack(this.m_Indices, dst5, this.m_IndexCount, this.m_IndexFormat);
        }
      }
    }
  }
}
