// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TelecomCoverage
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public struct TelecomCoverage : IStrideSerializable, ISerializable
  {
    public byte m_SignalStrength;
    public byte m_NetworkLoad;

    public int networkQuality
    {
      get
      {
        return (int) this.m_SignalStrength * 510 / ((int) byte.MaxValue + ((int) this.m_NetworkLoad << 1));
      }
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_SignalStrength);
      writer.Write(this.m_NetworkLoad);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_SignalStrength);
      reader.Read(out this.m_NetworkLoad);
    }

    public int GetStride(Context context) => 2;

    public static float SampleNetworkQuality(CellMapData<TelecomCoverage> coverage, float3 position)
    {
      float2 x = position.xz / coverage.m_CellSize + (float2) coverage.m_TextureSize * 0.5f - 0.5f;
      int4 xyxy = ((int2) math.floor(x)).xyxy;
      xyxy.zw += 1;
      int4 int4_1 = math.clamp(xyxy, (int4) 0, coverage.m_TextureSize.xyxy - 1);
      int4 int4_2 = int4_1.xzxz + coverage.m_TextureSize.x * int4_1.yyww;
      TelecomCoverage telecomCoverage1 = coverage.m_Buffer[int4_2.x];
      TelecomCoverage telecomCoverage2 = coverage.m_Buffer[int4_2.y];
      TelecomCoverage telecomCoverage3 = coverage.m_Buffer[int4_2.z];
      TelecomCoverage telecomCoverage4 = coverage.m_Buffer[int4_2.w];
      float4 float4 = math.min((float4) 1f, new float4((float) telecomCoverage1.m_SignalStrength, (float) telecomCoverage2.m_SignalStrength, (float) telecomCoverage3.m_SignalStrength, (float) telecomCoverage4.m_SignalStrength) / (127.5f + new float4((float) telecomCoverage1.m_NetworkLoad, (float) telecomCoverage2.m_NetworkLoad, (float) telecomCoverage3.m_NetworkLoad, (float) telecomCoverage4.m_NetworkLoad)));
      float2 float2_1 = math.saturate(x - (float2) int4_1.xy);
      float2 float2_2 = math.lerp(float4.xz, float4.yw, float2_1.x);
      return math.lerp(float2_2.x, float2_2.y, float2_1.y);
    }
  }
}
