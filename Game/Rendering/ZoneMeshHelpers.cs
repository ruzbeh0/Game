// Decompiled with JetBrains decompiler
// Type: Game.Rendering.ZoneMeshHelpers
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Rendering
{
  public static class ZoneMeshHelpers
  {
    public static Mesh CreateMesh(int2 resolution, int2 factor)
    {
      int length = (resolution.x + 1) * (resolution.y + 1) + resolution.x * resolution.y;
      int indexCount = ZoneMeshHelpers.GetIndexCount(resolution);
      Vector3[] vector3Array = new Vector3[length];
      Vector2[] vector2Array = new Vector2[length];
      int[] numArray1 = new int[indexCount];
      int index1 = 0;
      int num1 = 0;
      for (int index2 = 0; index2 <= resolution.y; ++index2)
      {
        float z = (float) (((double) index2 - (double) resolution.y * 0.5) * (double) factor.y * 8.0);
        float y = (float) ((resolution.y - index2) * factor.y);
        for (int index3 = 0; index3 <= resolution.x; ++index3)
        {
          float x1 = (float) (((double) index3 - (double) resolution.x * 0.5) * (double) factor.x * 8.0);
          float x2 = (float) ((resolution.x - index3) * factor.x);
          vector3Array[index1] = new Vector3(x1, 0.0f, z);
          vector2Array[index1] = new Vector2(x2, y);
          ++index1;
        }
      }
      for (int index4 = 0; index4 < resolution.y; ++index4)
      {
        float z = (float) (((double) index4 + (0.5 - (double) resolution.y * 0.5)) * (double) factor.y * 8.0);
        float y = ((float) resolution.y - 0.5f - (float) index4) * (float) factor.y;
        for (int index5 = 0; index5 < resolution.x; ++index5)
        {
          float x3 = (float) (((double) index5 + (0.5 - (double) resolution.x * 0.5)) * (double) factor.x * 8.0);
          float x4 = ((float) resolution.x - 0.5f - (float) index5) * (float) factor.x;
          int num2 = index4 * (resolution.x + 1) + index5;
          int num3 = num2 + 1;
          int num4 = num2 + (resolution.x + 1);
          int num5 = num2 + (resolution.x + 2);
          int[] numArray2 = numArray1;
          int index6 = num1;
          int num6 = index6 + 1;
          int num7 = index1;
          numArray2[index6] = num7;
          int[] numArray3 = numArray1;
          int index7 = num6;
          int num8 = index7 + 1;
          int num9 = num3;
          numArray3[index7] = num9;
          int[] numArray4 = numArray1;
          int index8 = num8;
          int num10 = index8 + 1;
          int num11 = num2;
          numArray4[index8] = num11;
          int[] numArray5 = numArray1;
          int index9 = num10;
          int num12 = index9 + 1;
          int num13 = index1;
          numArray5[index9] = num13;
          int[] numArray6 = numArray1;
          int index10 = num12;
          int num14 = index10 + 1;
          int num15 = num5;
          numArray6[index10] = num15;
          int[] numArray7 = numArray1;
          int index11 = num14;
          int num16 = index11 + 1;
          int num17 = num3;
          numArray7[index11] = num17;
          int[] numArray8 = numArray1;
          int index12 = num16;
          int num18 = index12 + 1;
          int num19 = index1;
          numArray8[index12] = num19;
          int[] numArray9 = numArray1;
          int index13 = num18;
          int num20 = index13 + 1;
          int num21 = num4;
          numArray9[index13] = num21;
          int[] numArray10 = numArray1;
          int index14 = num20;
          int num22 = index14 + 1;
          int num23 = num5;
          numArray10[index14] = num23;
          int[] numArray11 = numArray1;
          int index15 = num22;
          int num24 = index15 + 1;
          int num25 = index1;
          numArray11[index15] = num25;
          int[] numArray12 = numArray1;
          int index16 = num24;
          int num26 = index16 + 1;
          int num27 = num2;
          numArray12[index16] = num27;
          int[] numArray13 = numArray1;
          int index17 = num26;
          num1 = index17 + 1;
          int num28 = num4;
          numArray13[index17] = num28;
          vector3Array[index1] = new Vector3(x3, 0.0f, z);
          vector2Array[index1] = new Vector2(x4, y);
          ++index1;
        }
      }
      Mesh mesh = new Mesh();
      mesh.name = string.Format("Zone {0}x{1}", (object) resolution.x, (object) resolution.y);
      mesh.vertices = vector3Array;
      mesh.uv = vector2Array;
      mesh.triangles = numArray1;
      return mesh;
    }

    public static int GetIndexCount(int2 resolution) => resolution.x * resolution.y * 4 * 3;

    public static Bounds3 GetBounds(int2 resolution)
    {
      float3 _max = new float3((float) resolution.x * 4f, 0.0f, (float) resolution.y * 4f);
      _max.y = math.cmax(_max.xz);
      return new Bounds3(-_max, _max);
    }
  }
}
