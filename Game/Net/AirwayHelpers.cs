// Decompiled with JetBrains decompiler
// Type: Game.Net.AirwayHelpers
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Game.Pathfind;
using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  public static class AirwayHelpers
  {
    public struct AirwayMap : IDisposable
    {
      private int2 m_GridSize;
      private float m_CellSize;
      private float m_PathHeight;
      private NativeArray<Entity> m_Entities;

      public NativeArray<Entity> entities => this.m_Entities;

      public AirwayMap(int2 gridSize, float cellSize, float pathHeight, Allocator allocator)
      {
        this.m_GridSize = gridSize;
        this.m_CellSize = cellSize;
        this.m_PathHeight = pathHeight;
        this.m_Entities = new NativeArray<Entity>((this.m_GridSize.x * 3 + 1) * this.m_GridSize.y + this.m_GridSize.x, allocator);
      }

      public void Dispose()
      {
        if (!this.m_Entities.IsCreated)
          return;
        this.m_Entities.Dispose();
      }

      public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
      {
        writer.Write(this.m_GridSize);
        writer.Write(this.m_CellSize);
        writer.Write(this.m_PathHeight);
        writer.Write(this.m_Entities.Length);
        writer.Write(this.m_Entities);
      }

      public void Deserialize<TReader>(TReader reader) where TReader : IReader
      {
        reader.Read(out this.m_GridSize);
        reader.Read(out this.m_CellSize);
        if (reader.context.version >= Game.Version.airplaneAirways)
          reader.Read(out this.m_PathHeight);
        reader.Read(out int _);
        reader.Read(this.m_Entities);
      }

      public void SetDefaults(Colossal.Serialization.Entities.Context context)
      {
        for (int index = 0; index < this.m_Entities.Length; ++index)
          this.m_Entities[index] = Entity.Null;
      }

      public int2 GetCellIndex(int entityIndex, out AirwayHelpers.LaneDirection direction)
      {
        int num = this.m_GridSize.x * 3 + 1;
        int2 cellIndex;
        cellIndex.y = entityIndex / num;
        cellIndex.x = entityIndex - cellIndex.y * num;
        direction = AirwayHelpers.LaneDirection.HorizontalX;
        if (cellIndex.y < this.m_GridSize.y)
        {
          cellIndex.x /= 3;
          direction = (AirwayHelpers.LaneDirection) (entityIndex - cellIndex.x * 3 - cellIndex.y * num);
          direction += (AirwayHelpers.LaneDirection) ((int) direction >> 1 & cellIndex.x + cellIndex.y);
        }
        return cellIndex;
      }

      public int GetEntityIndex(int2 nodeIndex, AirwayHelpers.LaneDirection direction)
      {
        int num1 = this.m_GridSize.x * 3 + 1;
        int num2 = nodeIndex.y * num1;
        return nodeIndex.y >= this.m_GridSize.y ? num2 + nodeIndex.x : num2 + (nodeIndex.x * 3 + math.min(2, (int) direction));
      }

      public PathNode GetPathNode(int2 index)
      {
        int num = this.m_GridSize.x * 3 + 1;
        if (index.y < this.m_GridSize.y)
          return new PathNode(this.m_Entities[index.y * num + index.x * 3], (ushort) 0);
        return index.x < this.m_GridSize.x ? new PathNode(this.m_Entities[this.m_GridSize.y * num + index.x], (ushort) 0) : new PathNode(this.m_Entities[this.m_GridSize.y * num + this.m_GridSize.x - 1], (ushort) 2);
      }

      public float3 GetNodePosition(int2 nodeIndex)
      {
        float2 float2 = ((float2) nodeIndex - (float2) this.m_GridSize * 0.5f) * this.m_CellSize;
        return new float3(float2.x, this.m_PathHeight, float2.y);
      }

      public int2 GetCellIndex(float3 position)
      {
        return math.clamp((int2) (position.xz / this.m_CellSize + (float2) this.m_GridSize * 0.5f), (int2) 0, this.m_GridSize - 1);
      }

      public void FindClosestLane(
        float3 position,
        ComponentLookup<Curve> curveData,
        ref Entity lane,
        ref float curvePos,
        ref float distance)
      {
        int2 cellIndex = this.GetCellIndex(position);
        this.FindClosestLaneImpl(position, curveData, ref lane, ref curvePos, ref distance, this.GetEntityIndex(cellIndex, AirwayHelpers.LaneDirection.HorizontalZ));
        this.FindClosestLaneImpl(position, curveData, ref lane, ref curvePos, ref distance, this.GetEntityIndex(cellIndex, AirwayHelpers.LaneDirection.HorizontalX));
        this.FindClosestLaneImpl(position, curveData, ref lane, ref curvePos, ref distance, this.GetEntityIndex(cellIndex, AirwayHelpers.LaneDirection.Diagonal));
        this.FindClosestLaneImpl(position, curveData, ref lane, ref curvePos, ref distance, this.GetEntityIndex(new int2(cellIndex.x + 1, cellIndex.y), AirwayHelpers.LaneDirection.HorizontalZ));
        this.FindClosestLaneImpl(position, curveData, ref lane, ref curvePos, ref distance, this.GetEntityIndex(new int2(cellIndex.x, cellIndex.y + 1), AirwayHelpers.LaneDirection.HorizontalX));
      }

      private void FindClosestLaneImpl(
        float3 position,
        ComponentLookup<Curve> curveData,
        ref Entity bestLane,
        ref float bestCurvePos,
        ref float bestDistance,
        int entityIndex)
      {
        Entity entity = this.m_Entities[entityIndex];
        if (!curveData.HasComponent(entity))
          return;
        float t;
        float num = MathUtils.Distance(curveData[entity].m_Bezier, position, out t);
        if ((double) num >= (double) bestDistance)
          return;
        bestLane = entity;
        bestCurvePos = t;
        bestDistance = num;
      }
    }

    public struct AirwayData : IDisposable
    {
      public AirwayHelpers.AirwayMap helicopterMap { get; private set; }

      public AirwayHelpers.AirwayMap airplaneMap { get; private set; }

      public AirwayData(
        AirwayHelpers.AirwayMap _helicopterMap,
        AirwayHelpers.AirwayMap _airplaneMap)
      {
        this.helicopterMap = _helicopterMap;
        this.airplaneMap = _airplaneMap;
      }

      public void Dispose()
      {
        this.helicopterMap.Dispose();
        this.airplaneMap.Dispose();
      }
    }

    public enum LaneDirection
    {
      HorizontalZ,
      HorizontalX,
      Diagonal,
      DiagonalCross,
    }
  }
}
