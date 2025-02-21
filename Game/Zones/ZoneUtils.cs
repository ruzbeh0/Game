// Decompiled with JetBrains decompiler
// Type: Game.Zones.ZoneUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Unity.Mathematics;

#nullable disable
namespace Game.Zones
{
  public static class ZoneUtils
  {
    public const float CELL_SIZE = 8f;
    public const float CELL_AREA = 64f;
    public const int MAX_ZONE_WIDTH = 10;
    public const int MAX_ZONE_DEPTH = 6;
    public const int MAX_ZONE_TYPES = 339;

    public static float3 GetPosition(Block block, int2 min, int2 max)
    {
      float2 float2 = (float2) (block.m_Size - min - max) * 4f;
      float3 position = block.m_Position;
      position.xz += block.m_Direction * float2.y;
      position.xz += MathUtils.Right(block.m_Direction) * float2.x;
      return position;
    }

    public static quaternion GetRotation(Block block)
    {
      return quaternion.LookRotation(new float3(block.m_Direction.x, 0.0f, block.m_Direction.y), math.up());
    }

    public static Quad2 CalculateCorners(Block block)
    {
      float2 float2_1 = (float2) block.m_Size * 4f;
      float2 float2_2 = block.m_Direction * float2_1.y;
      float2 float2_3 = MathUtils.Right(block.m_Direction) * float2_1.x;
      float2 float2_4 = block.m_Position.xz + float2_2;
      float2 float2_5 = block.m_Position.xz - float2_2;
      return new Quad2(float2_4 + float2_3, float2_4 - float2_3, float2_5 - float2_3, float2_5 + float2_3);
    }

    public static Quad2 CalculateCorners(Block block, ValidArea validArea)
    {
      float4 float4_1 = (float4) (block.m_Size.xxyy - (validArea.m_Area << 1)) * 4f;
      float4 float4_2 = block.m_Direction.xyxy * float4_1.zzww;
      float4 float4_3 = MathUtils.Right(block.m_Direction).xyxy * float4_1.xxyy;
      float4 float4_4 = block.m_Position.xzxz + float4_2;
      float4 float4_5 = float4_4.xyxy + float4_3;
      float4 float4_6 = float4_4.zwzw + float4_3;
      return new Quad2(float4_5.xy, float4_5.zw, float4_6.zw, float4_6.xy);
    }

    public static Bounds2 CalculateBounds(Block block)
    {
      float2 float2_1 = (float2) block.m_Size * 4f;
      float2 float2_2 = math.abs(block.m_Direction * float2_1.y) + math.abs(MathUtils.Right(block.m_Direction) * float2_1.x);
      return new Bounds2(block.m_Position.xz - float2_2, block.m_Position.xz + float2_2);
    }

    public static int2 GetCellIndex(Block block, float2 position)
    {
      float2 y = MathUtils.Right(block.m_Direction);
      float2 x = block.m_Position.xz - position;
      return (int2) math.floor((new float2(math.dot(x, y), math.dot(x, block.m_Direction)) + (float2) block.m_Size * 4f) / 8f);
    }

    public static float3 GetCellPosition(Block block, int2 cellIndex)
    {
      float2 float2 = (float2) (block.m_Size - (cellIndex << 1) - 1) * 4f;
      float3 position = block.m_Position;
      position.xz += block.m_Direction * float2.y;
      position.xz += MathUtils.Right(block.m_Direction) * float2.x;
      return position;
    }

    public static bool CanShareCells(
      Block block1,
      Block block2,
      BuildOrder buildOrder1,
      BuildOrder buildOrder2)
    {
      if (buildOrder1.m_Order < buildOrder2.m_Order)
        return ZoneUtils.CanShareCells(block1, block2);
      return buildOrder2.m_Order < buildOrder1.m_Order && ZoneUtils.CanShareCells(block2, block1);
    }

    public static bool CanShareCells(Block block1, Block block2)
    {
      float num = math.abs(math.dot(block1.m_Direction, block2.m_Direction));
      if ((double) num > 0.017452405765652657 & (double) num < 0.99984771013259888)
        return false;
      float2 float2_1 = MathUtils.Right(block1.m_Direction);
      float2 float2_2 = MathUtils.Right(block2.m_Direction);
      float2 a1 = block2.m_Position.xz - block1.m_Position.xz;
      bool2 bool2_1 = (block1.m_Size & 1) != 0;
      bool2 bool2_2 = (block2.m_Size & 1) != 0;
      float2 a2 = math.select(a1, a1 - block1.m_Direction * 4f, bool2_1.y);
      float2 a3 = math.select(a2, a2 + block2.m_Direction * 4f, bool2_2.y);
      float2 a4 = math.select(a3, a3 - float2_1 * 4f, bool2_1.x);
      float2 x1 = math.select(a4, a4 + float2_2 * 4f, bool2_2.x);
      float2 x2;
      x2.y = math.dot(x1, block1.m_Direction);
      x2.x = math.dot(x1, MathUtils.Right(block1.m_Direction));
      x2 = math.abs(x2 / 8f);
      return math.all(math.abs(math.frac(x2) - 0.5f) >= 0.48f);
    }

    public static bool IsNeighbor(
      Block block1,
      Block block2,
      BuildOrder buildOrder1,
      BuildOrder buildOrder2)
    {
      if (buildOrder1.m_Order < buildOrder2.m_Order)
        return ZoneUtils.IsNeighbor(block1, block2);
      return buildOrder2.m_Order < buildOrder1.m_Order && ZoneUtils.IsNeighbor(block2, block1);
    }

    public static bool IsNeighbor(Block block1, Block block2)
    {
      if ((double) math.dot(block1.m_Direction, block2.m_Direction) < 0.99984771013259888)
        return false;
      float2 x = block2.m_Position.xz - block1.m_Position.xz - block1.m_Direction * ((float) block1.m_Size.y * 4f) + block2.m_Direction * ((float) block2.m_Size.y * 4f);
      float2 float2_1;
      float2_1.y = math.dot(x, block1.m_Direction);
      float2_1.x = math.dot(x, MathUtils.Right(block1.m_Direction));
      float2 float2_2 = math.abs(float2_1 / 8f);
      return math.all(new float2(math.abs(float2_2.x - (float) (block1.m_Size.x + block2.m_Size.x) * 0.5f), float2_2.y) <= 0.02f);
    }

    public static int GetColorIndex(CellFlags state, ZoneType type)
    {
      return math.select(0, 3 + (int) type.m_Index * 3, (state & (CellFlags.Shared | CellFlags.Visible)) == CellFlags.Visible) + math.select(math.select(0, 1, (state & CellFlags.Occupied) != 0), 2, (state & CellFlags.Selected) != 0);
    }

    public static int GetCellWidth(float roadWidth)
    {
      return (int) math.ceil((float) ((double) roadWidth / 8.0 - 0.0099999997764825821));
    }

    public static CellFlags GetRoadDirection(Block target, Block source)
    {
      float2 x = new float2(math.dot(target.m_Direction, source.m_Direction), math.dot(MathUtils.Right(target.m_Direction), source.m_Direction));
      int2 int2 = math.select(new int2(4, 512), new int2(2048, 1024), x < 0.0f);
      float2 float2 = math.abs(x);
      return (CellFlags) math.select(int2.x, int2.y, (double) float2.y > (double) float2.x);
    }

    public static CellFlags GetRoadDirection(Block target, Block source, CellFlags directionFlags)
    {
      float2 x = new float2(math.dot(target.m_Direction, source.m_Direction), math.dot(MathUtils.Right(target.m_Direction), source.m_Direction));
      int4 int4 = new int4(4, 512, 2048, 1024);
      int4 a = math.select(int4.xyzw, int4.zwxy, (double) x.x < 0.0);
      int4 b = math.select(int4.yzwx, int4.wxyz, (double) x.y < 0.0);
      float2 float2 = math.abs(x);
      return (CellFlags) math.csum(math.select((int4) 0, math.select(a, b, (double) float2.y > (double) float2.x), ((int) directionFlags & int4) != 0));
    }
  }
}
