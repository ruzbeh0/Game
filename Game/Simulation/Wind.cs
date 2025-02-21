// Decompiled with JetBrains decompiler
// Type: Game.Simulation.Wind
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public struct Wind : IStrideSerializable, ISerializable
  {
    public float2 m_Wind;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Wind);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Wind);
    }

    public int GetStride(Context context) => UnsafeUtility.SizeOf<float2>();

    public static float2 SampleWind(CellMapData<Wind> wind, float3 position)
    {
      float2 x1 = position.xz / wind.m_CellSize + (float2) wind.m_TextureSize * 0.5f - 0.5f;
      int4 xyxy = ((int2) math.floor(x1)).xyxy;
      xyxy.zw += 1;
      int4 int4_1 = math.clamp(xyxy, (int4) 0, wind.m_TextureSize.xyxy - 1);
      int4 int4_2 = int4_1.xzxz + wind.m_TextureSize.x * int4_1.yyww;
      float4 x2 = new float4(wind.m_Buffer[int4_2.x].m_Wind, wind.m_Buffer[int4_2.z].m_Wind);
      float4 float4_1 = new float4(wind.m_Buffer[int4_2.y].m_Wind, wind.m_Buffer[int4_2.w].m_Wind);
      float2 float2 = math.saturate(x1 - (float2) int4_1.xy);
      float4 y = float4_1;
      double x3 = (double) float2.x;
      float4 float4_2 = math.lerp(x2, y, (float) x3);
      return math.lerp(float4_2.xy, float4_2.zw, float2.y);
    }
  }
}
