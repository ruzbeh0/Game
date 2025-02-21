// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CreatureTargetIterator
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Creatures;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Vehicles;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public struct CreatureTargetIterator
  {
    public ComponentLookup<Moving> m_MovingData;
    public ComponentLookup<Curve> m_CurveData;
    public ComponentLookup<Game.Net.LaneReservation> m_LaneReservationData;
    public BufferLookup<LaneOverlap> m_LaneOverlaps;
    public BufferLookup<LaneObject> m_LaneObjects;
    public ObjectGeometryData m_PrefabObjectGeometry;
    public Entity m_Blocker;
    public BlockerType m_BlockerType;
    public Entity m_QueueEntity;
    public Sphere3 m_QueueArea;
    private float m_TargetDelta;

    public bool IterateLane(Entity currentLane, ref float curveDelta, float targetDelta)
    {
      this.m_TargetDelta = targetDelta;
      DynamicBuffer<LaneOverlap> bufferData;
      if (this.m_LaneOverlaps.TryGetBuffer(currentLane, out bufferData))
      {
        for (int index = 0; index < bufferData.Length; ++index)
        {
          LaneOverlap laneOverlap = bufferData[index];
          if ((laneOverlap.m_Flags & (OverlapFlags.MergeStart | OverlapFlags.MergeEnd | OverlapFlags.MergeMiddleStart | OverlapFlags.MergeMiddleEnd)) == (OverlapFlags) 0 && (laneOverlap.m_Flags & (OverlapFlags.Road | OverlapFlags.Track)) != (OverlapFlags) 0)
          {
            float4 float4 = new float4((float) laneOverlap.m_ThisStart, (float) laneOverlap.m_ThisEnd, (float) laneOverlap.m_OtherStart, (float) laneOverlap.m_OtherEnd) * 0.003921569f;
            if ((double) curveDelta <= (double) float4.x & (double) this.m_TargetDelta > (double) float4.x)
              this.CheckOverlapLane(currentLane, laneOverlap.m_Other, float4.x, targetDelta, float4.zw);
            else if ((double) curveDelta >= (double) float4.y & (double) this.m_TargetDelta < (double) float4.y)
              this.CheckOverlapLane(currentLane, laneOverlap.m_Other, float4.y, targetDelta, float4.zw);
          }
        }
      }
      curveDelta = this.m_TargetDelta;
      return (double) this.m_TargetDelta == (double) targetDelta;
    }

    private void CheckOverlapLane(
      Entity currentLane,
      Entity overlapLane,
      float limitDelta,
      float targetDelta,
      float2 overlapRange)
    {
      Game.Net.LaneReservation componentData;
      if (this.m_LaneReservationData.TryGetComponent(overlapLane, out componentData))
      {
        double offset = (double) componentData.GetOffset();
        int priority = componentData.GetPriority();
        double x = (double) overlapRange.x;
        if (offset > x | priority >= 108)
        {
          this.m_TargetDelta = limitDelta;
          this.m_Blocker = Entity.Null;
          this.m_BlockerType = BlockerType.Crossing;
          Curve curve = this.m_CurveData[currentLane];
          float3 position1 = MathUtils.Position(curve.m_Bezier, limitDelta);
          float3 position2 = math.select(curve.m_Bezier.a, curve.m_Bezier.d, (double) targetDelta > (double) limitDelta);
          this.m_QueueEntity = currentLane;
          this.m_QueueArea = CreatureUtils.GetQueueArea(this.m_PrefabObjectGeometry, position1, position2);
          return;
        }
      }
      DynamicBuffer<LaneObject> bufferData;
      if (!this.m_LaneObjects.TryGetBuffer(overlapLane, out bufferData))
        return;
      for (int index = 0; index < bufferData.Length; ++index)
      {
        LaneObject laneObject = bufferData[index];
        double num1 = (double) math.min(laneObject.m_CurvePosition.x, laneObject.m_CurvePosition.y);
        float num2 = math.max(laneObject.m_CurvePosition.x, laneObject.m_CurvePosition.y);
        double y = (double) overlapRange.y;
        if (num1 <= y & (double) num2 >= (double) overlapRange.x && this.m_MovingData.HasComponent(laneObject.m_LaneObject))
        {
          this.m_TargetDelta = limitDelta;
          this.m_Blocker = laneObject.m_LaneObject;
          this.m_BlockerType = BlockerType.Crossing;
          Curve curve = this.m_CurveData[currentLane];
          float3 position1 = MathUtils.Position(curve.m_Bezier, limitDelta);
          float3 position2 = math.select(curve.m_Bezier.a, curve.m_Bezier.d, (double) targetDelta > (double) limitDelta);
          this.m_QueueEntity = currentLane;
          this.m_QueueArea = CreatureUtils.GetQueueArea(this.m_PrefabObjectGeometry, position1, position2);
          break;
        }
      }
    }
  }
}
