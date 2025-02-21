// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.PathUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Routes;
using Game.Vehicles;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Pathfind
{
  public static class PathUtils
  {
    public const float MIN_DENSITY = 0.01f;

    public static float CalculateCost(
      ref Random random,
      in PathSpecification pathSpecification,
      in PathfindParameters pathfindParameters)
    {
      float speed = PathUtils.CalculateSpeed(in pathSpecification, in pathfindParameters);
      float4 x = pathSpecification.m_Costs.m_Value;
      x.x += pathSpecification.m_Length / speed;
      float b = math.dot(x, pathfindParameters.m_Weights.m_Value);
      return math.select(b * random.NextFloat(0.5f, 1f), b, (pathfindParameters.m_PathfindFlags & PathfindFlags.Stable) != 0);
    }

    public static float CalculateCost(
      in PathSpecification pathSpecification,
      in CoverageParameters coverageParameters,
      float2 delta)
    {
      return pathSpecification.m_Length * pathSpecification.m_Density * math.abs(delta.y - delta.x);
    }

    public static float CalculateCost(
      in PathSpecification pathSpecification,
      in AvailabilityParameters availabilityParameters,
      float2 delta)
    {
      float num = math.lerp(1f, pathSpecification.m_Density, availabilityParameters.m_DensityWeight);
      return pathSpecification.m_Length * num * math.abs(delta.y - delta.x) * availabilityParameters.m_CostFactor / pathSpecification.m_MaxSpeed;
    }

    public static float CalculateLength(in PathSpecification pathSpecification, float2 delta)
    {
      return pathSpecification.m_Length * math.abs(delta.y - delta.x);
    }

    public static float CalculateSpeed(
      in PathSpecification pathSpecification,
      in PathfindParameters pathfindParameters)
    {
      PathMethod pathMethod = pathSpecification.m_Methods & pathfindParameters.m_Methods;
      float2 float2 = math.select(pathfindParameters.m_MaxSpeed, pathfindParameters.m_WalkSpeed, (pathMethod & PathMethod.Pedestrian) != 0);
      bool c = (pathSpecification.m_Flags & EdgeFlags.Secondary) != 0;
      float b1 = math.min(math.select(float2.x, float2.y, c), pathSpecification.m_MaxSpeed);
      float b2 = math.select(pathSpecification.m_MaxSpeed, b1, (pathMethod & (PathMethod.Pedestrian | PathMethod.Road | PathMethod.Track | PathMethod.Flying)) != 0);
      return math.select(b2 - (float) pathSpecification.m_FlowOffset * (1f / 256f) * b2, b2, (pathfindParameters.m_PathfindFlags & PathfindFlags.IgnoreFlow) != 0);
    }

    public static void CombinePaths(
      DynamicBuffer<PathElement> sourceElements1,
      DynamicBuffer<PathElement> sourceElements2,
      DynamicBuffer<PathElement> targetElements)
    {
      targetElements.ResizeUninitialized(sourceElements1.Length + sourceElements2.Length);
      int num = 0;
      for (int index = 0; index < sourceElements1.Length; ++index)
        targetElements[num++] = sourceElements1[index];
      for (int index = 0; index < sourceElements2.Length; ++index)
        targetElements[num++] = sourceElements2[index];
    }

    public static PathInformation CombinePaths(
      PathInformation pathInformation1,
      PathInformation pathInformation2)
    {
      pathInformation1.m_Distance += pathInformation2.m_Distance;
      pathInformation1.m_Duration += pathInformation2.m_Duration;
      pathInformation1.m_TotalCost += pathInformation2.m_TotalCost;
      pathInformation1.m_Destination = pathInformation2.m_Destination;
      return pathInformation1;
    }

    public static void CopyPath(
      DynamicBuffer<PathElement> sourceElements,
      PathOwner sourceOwner,
      int skipCount,
      DynamicBuffer<PathElement> targetElements)
    {
      PathUtils.CopyPath(sourceElements, sourceOwner, skipCount, sourceElements.Length, targetElements);
    }

    public static void CopyPath(
      DynamicBuffer<PathElement> sourceElements,
      PathOwner sourceOwner,
      int skipCount,
      int endIndex,
      DynamicBuffer<PathElement> targetElements)
    {
      endIndex = math.min(endIndex, sourceElements.Length);
      int start = sourceOwner.m_ElementIndex + skipCount;
      int length = endIndex - start;
      if (length > 0)
      {
        targetElements.ResizeUninitialized(length);
        sourceElements.AsNativeArray().GetSubArray(start, length).CopyTo(targetElements.AsNativeArray());
      }
      else
        targetElements.Clear();
    }

    public static void TrimPath(DynamicBuffer<PathElement> pathElements, ref PathOwner pathOwner)
    {
      PathUtils.TrimPath(pathElements, ref pathOwner, pathOwner.m_ElementIndex);
    }

    public static void TrimPath(
      DynamicBuffer<PathElement> pathElements,
      ref PathOwner pathOwner,
      int startIndex)
    {
      if (startIndex > 0)
      {
        if (startIndex >= pathElements.Length)
          pathElements.Clear();
        else
          pathElements.RemoveRange(0, startIndex);
      }
      pathOwner.m_ElementIndex = 0;
    }

    public static int FindFirstLane(
      DynamicBuffer<PathElement> pathElements,
      PathOwner pathOwner,
      int skipCount,
      ComponentLookup<Game.Net.ParkingLane> parkingLaneData)
    {
      for (int index = pathOwner.m_ElementIndex + skipCount; index < pathElements.Length; ++index)
      {
        if (parkingLaneData.HasComponent(pathElements[index].m_Target))
          return index;
      }
      return pathElements.Length;
    }

    public static bool GetStartDirection(
      DynamicBuffer<PathElement> path,
      PathOwner pathOwner,
      ref ComponentLookup<Curve> curveData,
      out int startOffset,
      out bool forward)
    {
      startOffset = 0;
      forward = true;
      if (pathOwner.m_ElementIndex >= path.Length)
        return false;
      PathElement pathElement1;
      int index1;
      for (pathElement1 = path[pathOwner.m_ElementIndex]; !curveData.HasComponent(pathElement1.m_Target); pathElement1 = path[index1])
      {
        index1 = pathOwner.m_ElementIndex + ++startOffset;
        if (index1 >= path.Length)
          return false;
      }
      if ((double) pathElement1.m_TargetDelta.x != (double) pathElement1.m_TargetDelta.y)
      {
        forward = (double) pathElement1.m_TargetDelta.y > (double) pathElement1.m_TargetDelta.x;
        return true;
      }
      forward = (double) pathElement1.m_TargetDelta.y == 1.0;
      bool startDirection = forward || (double) pathElement1.m_TargetDelta.y == 0.0;
      for (int index2 = pathOwner.m_ElementIndex + startOffset + 1; index2 < path.Length; ++index2)
      {
        PathElement pathElement2 = path[index2];
        if (pathElement2.m_Target != pathElement1.m_Target)
          return startDirection;
        if ((double) pathElement2.m_TargetDelta.x != (double) pathElement2.m_TargetDelta.y)
        {
          forward = (double) pathElement2.m_TargetDelta.y > (double) pathElement2.m_TargetDelta.x;
          return true;
        }
      }
      return startDirection;
    }

    public static bool GetEndDirection(
      DynamicBuffer<PathElement> path,
      PathOwner pathOwner,
      ref ComponentLookup<Curve> curveData,
      out int endOffset,
      out bool forward)
    {
      endOffset = 0;
      forward = true;
      if (pathOwner.m_ElementIndex >= path.Length)
        return false;
      PathElement pathElement1;
      int index1;
      for (pathElement1 = path[path.Length - 1]; !curveData.HasComponent(pathElement1.m_Target); pathElement1 = path[index1])
      {
        index1 = path.Length - 1 - ++endOffset;
        if (index1 < pathOwner.m_ElementIndex)
          return false;
      }
      if ((double) pathElement1.m_TargetDelta.x != (double) pathElement1.m_TargetDelta.y)
      {
        forward = (double) pathElement1.m_TargetDelta.y > (double) pathElement1.m_TargetDelta.x;
        return true;
      }
      forward = (double) pathElement1.m_TargetDelta.x == 0.0;
      bool endDirection = forward || (double) pathElement1.m_TargetDelta.x == 1.0;
      for (int index2 = path.Length - endOffset - 2; index2 >= pathOwner.m_ElementIndex; --index2)
      {
        PathElement pathElement2 = path[index2];
        if (pathElement2.m_Target != pathElement1.m_Target)
          return endDirection;
        if ((double) pathElement2.m_TargetDelta.x != (double) pathElement2.m_TargetDelta.y)
        {
          forward = (double) pathElement2.m_TargetDelta.y > (double) pathElement2.m_TargetDelta.x;
          return true;
        }
      }
      return endDirection;
    }

    public static void ExtendPath(
      DynamicBuffer<PathElement> path,
      PathOwner pathOwner,
      ref float distance,
      ref ComponentLookup<Curve> curveData,
      ref ComponentLookup<Lane> laneData,
      ref ComponentLookup<EdgeLane> edgeLaneData,
      ref ComponentLookup<Owner> ownerData,
      ref ComponentLookup<Game.Net.Edge> edgeData,
      ref BufferLookup<ConnectedEdge> connectedEdges,
      ref BufferLookup<Game.Net.SubLane> subLanes)
    {
      float num1 = distance;
      distance = 0.0f;
      int endOffset;
      bool forward;
      if (!PathUtils.GetEndDirection(path, pathOwner, ref curveData, out endOffset, out forward))
        return;
      PathElement elem = path[path.Length - endOffset - 1];
      elem.m_TargetDelta = new float2(elem.m_TargetDelta.y, math.select(0.0f, 1f, forward));
      for (int index = 0; index < 10000 && curveData.HasComponent(elem.m_Target); ++index)
      {
        if ((double) elem.m_TargetDelta.x != (double) elem.m_TargetDelta.y)
        {
          float num2 = curveData[elem.m_Target].m_Length * math.abs(elem.m_TargetDelta.y - elem.m_TargetDelta.x);
          if ((double) num2 >= (double) num1)
          {
            float s = math.select(num1 / num2, 1f, (double) num2 == 0.0);
            elem.m_TargetDelta.y = math.lerp(elem.m_TargetDelta.x, elem.m_TargetDelta.y, s);
            path.Insert(path.Length - endOffset, elem);
            distance += num1;
            break;
          }
          path.Insert(path.Length - endOffset, elem);
          distance += num2;
          num1 -= num2;
        }
        if (!NetUtils.FindConnectedLane(ref elem.m_Target, ref forward, ref laneData, ref edgeLaneData, ref ownerData, ref edgeData, ref connectedEdges, ref subLanes))
          break;
        elem.m_TargetDelta = math.select(new float2(1f, 0.0f), new float2(0.0f, 1f), forward);
        elem.m_Flags = (PathElementFlags) 0;
      }
    }

    public static void ExtendReverseLocations(
      PathElement prevElement,
      DynamicBuffer<PathElement> path,
      PathOwner pathOwner,
      float distance,
      ComponentLookup<Curve> curveData,
      ComponentLookup<Lane> laneData,
      ComponentLookup<EdgeLane> edgeLaneData,
      ComponentLookup<Owner> ownerData,
      ComponentLookup<Game.Net.Edge> edgeData,
      BufferLookup<ConnectedEdge> connectedEdges,
      BufferLookup<Game.Net.SubLane> subLanes)
    {
      if (pathOwner.m_ElementIndex >= path.Length)
        return;
      int index1 = pathOwner.m_ElementIndex - 1;
      if (prevElement.m_Target == Entity.Null)
        prevElement = path[++index1];
      Entity entity = Entity.Null;
      if ((prevElement.m_Flags & PathElementFlags.Return) == (PathElementFlags) 0 && (double) prevElement.m_TargetDelta.x != (double) prevElement.m_TargetDelta.y && ownerData.HasComponent(prevElement.m_Target))
        entity = ownerData[prevElement.m_Target].m_Owner;
      while (++index1 < path.Length)
      {
        PathElement pathElement = path[index1];
        if ((double) pathElement.m_TargetDelta.x == (double) pathElement.m_TargetDelta.y)
        {
          if ((pathElement.m_Flags & PathElementFlags.Return) != (PathElementFlags) 0)
            entity = Entity.Null;
        }
        else
        {
          Entity owner = Entity.Null;
          if ((double) pathElement.m_TargetDelta.x != (double) pathElement.m_TargetDelta.y && ownerData.HasComponent(pathElement.m_Target))
            owner = ownerData[pathElement.m_Target].m_Owner;
          if (owner != Entity.Null && owner == entity && curveData.HasComponent(prevElement.m_Target) && curveData.HasComponent(pathElement.m_Target))
          {
            Curve curve1 = curveData[prevElement.m_Target];
            Curve curve2 = curveData[pathElement.m_Target];
            float3 x = MathUtils.Tangent(curve1.m_Bezier, prevElement.m_TargetDelta.y);
            float3 float3 = MathUtils.Tangent(curve2.m_Bezier, pathElement.m_TargetDelta.x);
            bool forward = (double) prevElement.m_TargetDelta.y > (double) prevElement.m_TargetDelta.x;
            bool flag = (double) pathElement.m_TargetDelta.y > (double) pathElement.m_TargetDelta.x;
            float3 y = float3;
            if ((double) math.dot(x, y) * (double) math.select(1f, -1f, forward != flag) < 0.0)
            {
              float num1 = distance;
              prevElement.m_TargetDelta = new float2(prevElement.m_TargetDelta.y, math.select(0.0f, 1f, forward));
              for (int index2 = 0; index2 < 10000; ++index2)
              {
                if ((double) prevElement.m_TargetDelta.x != (double) prevElement.m_TargetDelta.y)
                {
                  float num2 = curveData[prevElement.m_Target].m_Length * math.abs(prevElement.m_TargetDelta.y - prevElement.m_TargetDelta.x);
                  if ((double) num2 >= (double) num1)
                  {
                    float s = math.select(num1 / num2, 1f, (double) num2 == 0.0);
                    prevElement.m_TargetDelta.y = math.lerp(prevElement.m_TargetDelta.x, prevElement.m_TargetDelta.y, s);
                    path.Insert(index1++, prevElement);
                    num1 = 0.0f;
                    break;
                  }
                  path.Insert(index1++, prevElement);
                  num1 -= num2;
                }
                if (NetUtils.FindConnectedLane(ref prevElement.m_Target, ref forward, ref laneData, ref edgeLaneData, ref ownerData, ref edgeData, ref connectedEdges, ref subLanes))
                {
                  prevElement.m_TargetDelta = math.select(new float2(1f, 0.0f), new float2(0.0f, 1f), forward);
                  prevElement.m_Flags = PathElementFlags.Reverse;
                }
                else
                  break;
              }
              if (index1 > 0)
              {
                prevElement = path[index1 - 1];
                prevElement.m_Flags |= PathElementFlags.Return;
                path[index1 - 1] = prevElement;
              }
              else
              {
                prevElement.m_TargetDelta.x = prevElement.m_TargetDelta.y;
                prevElement.m_Flags |= PathElementFlags.Return;
                path.Insert(index1++, prevElement);
              }
              if ((double) num1 > 0.0)
              {
                while (index1 < path.Length)
                {
                  pathElement = path[index1];
                  if (curveData.HasComponent(pathElement.m_Target))
                  {
                    float num3 = curveData[pathElement.m_Target].m_Length * math.abs(pathElement.m_TargetDelta.y - pathElement.m_TargetDelta.x);
                    if ((double) num3 >= (double) num1)
                    {
                      float s = math.select(num1 / num3, 1f, (double) num3 == 0.0);
                      pathElement.m_TargetDelta.x = math.lerp(pathElement.m_TargetDelta.x, pathElement.m_TargetDelta.y, s);
                      path[index1] = pathElement;
                      break;
                    }
                    path.RemoveAt(index1);
                    num1 -= num3;
                  }
                  else
                    break;
                }
              }
            }
          }
          prevElement = pathElement;
          entity = (pathElement.m_Flags & PathElementFlags.Return) == (PathElementFlags) 0 ? owner : Entity.Null;
        }
      }
    }

    public static void InitializeSpawnPath(
      DynamicBuffer<PathElement> path,
      NativeList<PathElement> laneBuffer,
      Entity parkingLocation,
      ref PathOwner pathOwner,
      float length,
      ref ComponentLookup<Curve> curveData,
      ref ComponentLookup<Lane> laneData,
      ref ComponentLookup<EdgeLane> edgeLaneData,
      ref ComponentLookup<Owner> ownerData,
      ref ComponentLookup<Game.Net.Edge> edgeData,
      ref ComponentLookup<Game.Objects.SpawnLocation> spawnLocationData,
      ref BufferLookup<ConnectedEdge> connectedEdges,
      ref BufferLookup<Game.Net.SubLane> subLanes)
    {
      Entity laneEntity1;
      Entity laneEntity2;
      bool forward1;
      bool forward2;
      float2 targetDelta1;
      float2 targetDelta2;
      if (path.IsCreated)
      {
        int startOffset;
        bool forward3;
        if (!PathUtils.GetStartDirection(path, pathOwner, ref curveData, out startOffset, out forward3))
          return;
        pathOwner.m_ElementIndex += startOffset;
        PathElement pathElement = path[pathOwner.m_ElementIndex];
        laneEntity1 = pathElement.m_Target;
        laneEntity2 = pathElement.m_Target;
        forward1 = !forward3;
        forward2 = forward3;
        targetDelta1 = forward1 ? new float2(1f, pathElement.m_TargetDelta.x) : new float2(0.0f, pathElement.m_TargetDelta.x);
        targetDelta2 = forward2 ? new float2(pathElement.m_TargetDelta.x, 1f) : new float2(pathElement.m_TargetDelta.x, 0.0f);
      }
      else
      {
        Game.Objects.SpawnLocation componentData;
        if (!spawnLocationData.TryGetComponent(parkingLocation, out componentData) || !curveData.HasComponent(componentData.m_ConnectedLane1))
          return;
        laneEntity1 = componentData.m_ConnectedLane1;
        laneEntity2 = componentData.m_ConnectedLane1;
        forward1 = false;
        forward2 = true;
        targetDelta1 = new float2(0.0f, componentData.m_CurvePosition1);
        targetDelta2 = new float2(componentData.m_CurvePosition1, 1f);
      }
      float num1 = length * 0.5f;
      float num2 = 0.0f;
      PathElement pathElement1;
      for (int index = 0; index < 10000; ++index)
      {
        float num3 = curveData[laneEntity1].m_Length * math.abs(targetDelta1.y - targetDelta1.x);
        if ((double) num3 >= (double) num1)
        {
          float s = math.select(num1 / num3, 1f, (double) num3 == 0.0);
          targetDelta1.x = math.lerp(targetDelta1.y, targetDelta1.x, s);
          ref NativeList<PathElement> local1 = ref laneBuffer;
          pathElement1 = new PathElement(laneEntity1, targetDelta1);
          ref PathElement local2 = ref pathElement1;
          local1.Add(in local2);
          num2 = num1;
          num1 = 0.0f;
          break;
        }
        laneBuffer.Add(new PathElement(laneEntity1, targetDelta1));
        num1 -= num3;
        if (NetUtils.FindConnectedLane(ref laneEntity1, ref forward1, ref laneData, ref edgeLaneData, ref ownerData, ref edgeData, ref connectedEdges, ref subLanes))
          targetDelta1 = forward1 ? new float2(1f, 0.0f) : new float2(0.0f, 1f);
        else
          break;
      }
      CollectionUtils.Reverse<PathElement>(laneBuffer.AsArray());
      float num4 = num1 + length * 0.5f;
      if (path.IsCreated)
      {
        while (pathOwner.m_ElementIndex < path.Length)
        {
          PathElement pathElement2 = path[pathOwner.m_ElementIndex];
          Curve componentData;
          if (curveData.TryGetComponent(pathElement2.m_Target, out componentData))
          {
            float num5 = componentData.m_Length * math.abs(pathElement2.m_TargetDelta.y - pathElement2.m_TargetDelta.x);
            if ((double) num5 >= (double) num4)
            {
              float s = math.select(num4 / num5, 1f, (double) num5 == 0.0);
              float y = math.lerp(pathElement2.m_TargetDelta.x, pathElement2.m_TargetDelta.y, s);
              ref NativeList<PathElement> local3 = ref laneBuffer;
              pathElement1 = new PathElement(pathElement2.m_Target, new float2(pathElement2.m_TargetDelta.x, y));
              ref PathElement local4 = ref pathElement1;
              local3.Add(in local4);
              num4 = 0.0f;
              pathElement2.m_TargetDelta.x = y;
              path[pathOwner.m_ElementIndex] = pathElement2;
              break;
            }
            laneEntity2 = pathElement2.m_Target;
            if ((double) pathElement2.m_TargetDelta.y != (double) pathElement2.m_TargetDelta.x)
              forward2 = (double) pathElement2.m_TargetDelta.y > (double) pathElement2.m_TargetDelta.x;
            else if ((double) pathElement2.m_TargetDelta.y == 0.0)
              forward2 = false;
            else if ((double) pathElement2.m_TargetDelta.y == 1.0)
              forward2 = true;
            targetDelta2 = forward2 ? new float2(pathElement2.m_TargetDelta.y, 1f) : new float2(pathElement2.m_TargetDelta.y, 0.0f);
            laneBuffer.Add(in pathElement2);
            ++pathOwner.m_ElementIndex;
            num4 -= num5;
          }
          else
            break;
        }
      }
      if ((double) num4 > 0.0)
      {
        for (int index = 0; index < 10000; ++index)
        {
          float num6 = curveData[laneEntity2].m_Length * math.abs(targetDelta2.y - targetDelta2.x);
          if ((double) num6 >= (double) num4)
          {
            float s = math.select(num4 / num6, 1f, (double) num6 == 0.0);
            targetDelta2.y = math.lerp(targetDelta2.x, targetDelta2.y, s);
            ref NativeList<PathElement> local5 = ref laneBuffer;
            pathElement1 = new PathElement(laneEntity2, targetDelta2);
            ref PathElement local6 = ref pathElement1;
            local5.Add(in local6);
            break;
          }
          ref NativeList<PathElement> local7 = ref laneBuffer;
          pathElement1 = new PathElement(laneEntity2, targetDelta2);
          ref PathElement local8 = ref pathElement1;
          local7.Add(in local8);
          num4 -= num6;
          if (NetUtils.FindConnectedLane(ref laneEntity2, ref forward2, ref laneData, ref edgeLaneData, ref ownerData, ref edgeData, ref connectedEdges, ref subLanes))
            targetDelta2 = forward2 ? new float2(0.0f, 1f) : new float2(1f, 0.0f);
          else
            break;
        }
      }
      CollectionUtils.Reverse<PathElement>(laneBuffer.AsArray());
      if ((double) num4 <= 0.0 || (double) num2 <= 0.0)
        return;
      laneBuffer.RemoveAt(laneBuffer.Length - 1);
      float num7 = num4 + num2;
      targetDelta1 = forward1 ? new float2(1f, 0.0f) : new float2(0.0f, 1f);
      for (int index = 0; index < 10000; ++index)
      {
        float num8 = curveData[laneEntity1].m_Length * math.abs(targetDelta1.y - targetDelta1.x);
        if ((double) num8 >= (double) num7)
        {
          float s = math.select(num7 / num8, 1f, (double) num8 == 0.0);
          targetDelta1.x = math.lerp(targetDelta1.y, targetDelta1.x, s);
          ref NativeList<PathElement> local9 = ref laneBuffer;
          pathElement1 = new PathElement(laneEntity1, targetDelta1);
          ref PathElement local10 = ref pathElement1;
          local9.Add(in local10);
          break;
        }
        ref NativeList<PathElement> local11 = ref laneBuffer;
        pathElement1 = new PathElement(laneEntity1, targetDelta1);
        ref PathElement local12 = ref pathElement1;
        local11.Add(in local12);
        num7 -= num8;
        if (!NetUtils.FindConnectedLane(ref laneEntity1, ref forward1, ref laneData, ref edgeLaneData, ref ownerData, ref edgeData, ref connectedEdges, ref subLanes))
          break;
        targetDelta1 = forward1 ? new float2(1f, 0.0f) : new float2(0.0f, 1f);
      }
    }

    public static void ResetPath(
      ref CarCurrentLane currentLane,
      DynamicBuffer<PathElement> path,
      ComponentLookup<SlaveLane> slaveLaneData,
      ComponentLookup<Owner> ownerData,
      BufferLookup<Game.Net.SubLane> subLanes)
    {
    }

    public static void ResetPath(
      ref WatercraftCurrentLane currentLane,
      DynamicBuffer<PathElement> path,
      ComponentLookup<SlaveLane> slaveLaneData,
      ComponentLookup<Owner> ownerData,
      BufferLookup<Game.Net.SubLane> subLanes)
    {
      if (currentLane.m_Lane != Entity.Null && path.Length > 0)
      {
        Entity masterLane = PathUtils.GetMasterLane(currentLane.m_Lane, slaveLaneData, ownerData, subLanes);
        PathElement pathElement = path[0];
        if (pathElement.m_Target == masterLane)
        {
          currentLane.m_CurvePosition.z = pathElement.m_TargetDelta.x;
          return;
        }
      }
      currentLane.m_CurvePosition.z = currentLane.m_CurvePosition.y;
    }

    public static void ResetPath(
      ref AircraftCurrentLane currentLane,
      DynamicBuffer<PathElement> path)
    {
      if (currentLane.m_Lane != Entity.Null && path.Length > 0)
      {
        PathElement pathElement = path[0];
        if (pathElement.m_Target == currentLane.m_Lane)
        {
          currentLane.m_CurvePosition.z = pathElement.m_TargetDelta.x;
          return;
        }
      }
      if ((currentLane.m_LaneFlags & (AircraftLaneFlags.Airway | AircraftLaneFlags.Flying)) == (AircraftLaneFlags.Airway | AircraftLaneFlags.Flying))
        currentLane.m_LaneFlags |= AircraftLaneFlags.SkipLane;
      currentLane.m_CurvePosition.z = currentLane.m_CurvePosition.y;
    }

    public static bool TryAppendPath(
      ref CarCurrentLane currentLane,
      DynamicBuffer<CarNavigationLane> navigationLanes,
      DynamicBuffer<PathElement> path,
      DynamicBuffer<PathElement> appendPath,
      ComponentLookup<SlaveLane> slaveLaneData,
      ComponentLookup<Owner> ownerData,
      BufferLookup<Game.Net.SubLane> subLanes)
    {
      return PathUtils.TryAppendPath(ref currentLane, navigationLanes, path, appendPath, slaveLaneData, ownerData, subLanes, out int _);
    }

    public static bool TryAppendPath(
      ref CarCurrentLane currentLane,
      DynamicBuffer<CarNavigationLane> navigationLanes,
      DynamicBuffer<PathElement> path,
      DynamicBuffer<PathElement> appendPath,
      ComponentLookup<SlaveLane> slaveLaneData,
      ComponentLookup<Owner> ownerData,
      BufferLookup<Game.Net.SubLane> subLanes,
      out int appendedCount)
    {
      NativeParallelHashMap<Entity, PathUtils.AppendPathValue> nativeParallelHashMap = new NativeParallelHashMap<Entity, PathUtils.AppendPathValue>(navigationLanes.Length + path.Length + 1, (AllocatorManager.AllocatorHandle) Allocator.Temp);
      if (currentLane.m_Lane != Entity.Null && (navigationLanes.Length == 0 || (navigationLanes[0].m_Flags & Game.Vehicles.CarLaneFlags.Reserved) == (Game.Vehicles.CarLaneFlags) 0))
      {
        Entity masterLane = PathUtils.GetMasterLane(currentLane.m_Lane, slaveLaneData, ownerData, subLanes);
        nativeParallelHashMap.TryAdd(masterLane, new PathUtils.AppendPathValue()
        {
          m_Index = 0,
          m_TargetDelta = currentLane.m_CurvePosition.y
        });
      }
      PathUtils.AppendPathValue appendPathValue1;
      for (int index = 1; index <= navigationLanes.Length; ++index)
      {
        if (navigationLanes.Length == index || (navigationLanes[index].m_Flags & Game.Vehicles.CarLaneFlags.Reserved) == (Game.Vehicles.CarLaneFlags) 0)
        {
          CarNavigationLane navigationLane = navigationLanes[index - 1];
          Entity masterLane = PathUtils.GetMasterLane(navigationLane.m_Lane, slaveLaneData, ownerData, subLanes);
          float num = math.select(navigationLane.m_CurvePosition.x, navigationLane.m_CurvePosition.y, (navigationLane.m_Flags & Game.Vehicles.CarLaneFlags.Reserved) > (Game.Vehicles.CarLaneFlags) 0);
          ref NativeParallelHashMap<Entity, PathUtils.AppendPathValue> local = ref nativeParallelHashMap;
          Entity key = masterLane;
          appendPathValue1 = new PathUtils.AppendPathValue();
          appendPathValue1.m_Index = index;
          appendPathValue1.m_TargetDelta = num;
          PathUtils.AppendPathValue appendPathValue2 = appendPathValue1;
          local.TryAdd(key, appendPathValue2);
        }
      }
      int num1 = navigationLanes.Length + 1;
      for (int index = 0; index < path.Length; ++index)
      {
        PathElement pathElement = path[index];
        ref NativeParallelHashMap<Entity, PathUtils.AppendPathValue> local = ref nativeParallelHashMap;
        Entity target = pathElement.m_Target;
        appendPathValue1 = new PathUtils.AppendPathValue();
        appendPathValue1.m_Index = num1 + index;
        appendPathValue1.m_TargetDelta = pathElement.m_TargetDelta.x;
        PathUtils.AppendPathValue appendPathValue3 = appendPathValue1;
        local.TryAdd(target, appendPathValue3);
      }
      for (int index1 = 0; appendPath.Length > index1; ++index1)
      {
        PathElement elem = appendPath[index1];
        PathUtils.AppendPathValue appendPathValue4;
        if (nativeParallelHashMap.TryGetValue(elem.m_Target, out appendPathValue4))
        {
          if (appendPathValue4.m_Index == 0)
          {
            currentLane.m_CurvePosition.z = appendPathValue4.m_TargetDelta;
            navigationLanes.Clear();
            path.Clear();
          }
          else if (appendPathValue4.m_Index <= navigationLanes.Length)
          {
            CarNavigationLane navigationLane = navigationLanes[appendPathValue4.m_Index - 1];
            navigationLane.m_CurvePosition.y = appendPathValue4.m_TargetDelta;
            navigationLanes[appendPathValue4.m_Index - 1] = navigationLane;
            if (appendPathValue4.m_Index < navigationLanes.Length)
              navigationLanes.RemoveRange(appendPathValue4.m_Index, navigationLanes.Length - appendPathValue4.m_Index);
            path.Clear();
          }
          else if (appendPathValue4.m_Index < num1 + path.Length)
            path.RemoveRange(appendPathValue4.m_Index - num1, num1 + path.Length - appendPathValue4.m_Index);
          path.EnsureCapacity(path.Length + appendPath.Length - index1);
          elem.m_TargetDelta.x = appendPathValue4.m_TargetDelta;
          path.Add(elem);
          for (int index2 = index1 + 1; index2 < appendPath.Length; ++index2)
            path.Add(appendPath[index2]);
          nativeParallelHashMap.Dispose();
          appendedCount = appendPath.Length - index1;
          return true;
        }
      }
      nativeParallelHashMap.Dispose();
      appendedCount = 0;
      return false;
    }

    public static bool TryAppendPath(
      ref WatercraftCurrentLane currentLane,
      DynamicBuffer<WatercraftNavigationLane> navigationLanes,
      DynamicBuffer<PathElement> path,
      DynamicBuffer<PathElement> appendPath,
      ComponentLookup<SlaveLane> slaveLaneData,
      ComponentLookup<Owner> ownerData,
      BufferLookup<Game.Net.SubLane> subLanes)
    {
      return PathUtils.TryAppendPath(ref currentLane, navigationLanes, path, appendPath, slaveLaneData, ownerData, subLanes, out int _);
    }

    public static bool TryAppendPath(
      ref WatercraftCurrentLane currentLane,
      DynamicBuffer<WatercraftNavigationLane> navigationLanes,
      DynamicBuffer<PathElement> path,
      DynamicBuffer<PathElement> appendPath,
      ComponentLookup<SlaveLane> slaveLaneData,
      ComponentLookup<Owner> ownerData,
      BufferLookup<Game.Net.SubLane> subLanes,
      out int appendedCount)
    {
      NativeParallelHashMap<Entity, PathUtils.AppendPathValue> nativeParallelHashMap = new NativeParallelHashMap<Entity, PathUtils.AppendPathValue>(navigationLanes.Length + path.Length + 1, (AllocatorManager.AllocatorHandle) Allocator.Temp);
      if (currentLane.m_Lane != Entity.Null && (navigationLanes.Length == 0 || (navigationLanes[0].m_Flags & WatercraftLaneFlags.Reserved) == (WatercraftLaneFlags) 0))
      {
        Entity masterLane = PathUtils.GetMasterLane(currentLane.m_Lane, slaveLaneData, ownerData, subLanes);
        nativeParallelHashMap.TryAdd(masterLane, new PathUtils.AppendPathValue()
        {
          m_Index = 0,
          m_TargetDelta = currentLane.m_CurvePosition.y
        });
      }
      PathUtils.AppendPathValue appendPathValue1;
      for (int index = 1; index <= navigationLanes.Length; ++index)
      {
        if (navigationLanes.Length == index || (navigationLanes[index].m_Flags & WatercraftLaneFlags.Reserved) == (WatercraftLaneFlags) 0)
        {
          WatercraftNavigationLane navigationLane = navigationLanes[index - 1];
          Entity masterLane = PathUtils.GetMasterLane(navigationLane.m_Lane, slaveLaneData, ownerData, subLanes);
          float num = math.select(navigationLane.m_CurvePosition.x, navigationLane.m_CurvePosition.y, (navigationLane.m_Flags & WatercraftLaneFlags.Reserved) > (WatercraftLaneFlags) 0);
          ref NativeParallelHashMap<Entity, PathUtils.AppendPathValue> local = ref nativeParallelHashMap;
          Entity key = masterLane;
          appendPathValue1 = new PathUtils.AppendPathValue();
          appendPathValue1.m_Index = index;
          appendPathValue1.m_TargetDelta = num;
          PathUtils.AppendPathValue appendPathValue2 = appendPathValue1;
          local.TryAdd(key, appendPathValue2);
        }
      }
      int num1 = navigationLanes.Length + 1;
      for (int index = 0; index < path.Length; ++index)
      {
        PathElement pathElement = path[index];
        ref NativeParallelHashMap<Entity, PathUtils.AppendPathValue> local = ref nativeParallelHashMap;
        Entity target = pathElement.m_Target;
        appendPathValue1 = new PathUtils.AppendPathValue();
        appendPathValue1.m_Index = num1 + index;
        appendPathValue1.m_TargetDelta = pathElement.m_TargetDelta.x;
        PathUtils.AppendPathValue appendPathValue3 = appendPathValue1;
        local.TryAdd(target, appendPathValue3);
      }
      for (int index1 = 0; appendPath.Length > index1; ++index1)
      {
        PathElement elem = appendPath[index1];
        PathUtils.AppendPathValue appendPathValue4;
        if (nativeParallelHashMap.TryGetValue(elem.m_Target, out appendPathValue4))
        {
          if (appendPathValue4.m_Index == 0)
          {
            currentLane.m_CurvePosition.z = appendPathValue4.m_TargetDelta;
            navigationLanes.Clear();
            path.Clear();
          }
          else if (appendPathValue4.m_Index <= navigationLanes.Length)
          {
            WatercraftNavigationLane navigationLane = navigationLanes[appendPathValue4.m_Index - 1];
            navigationLane.m_CurvePosition.y = appendPathValue4.m_TargetDelta;
            navigationLanes[appendPathValue4.m_Index - 1] = navigationLane;
            if (appendPathValue4.m_Index < navigationLanes.Length)
              navigationLanes.RemoveRange(appendPathValue4.m_Index, navigationLanes.Length - appendPathValue4.m_Index);
            path.Clear();
          }
          else if (appendPathValue4.m_Index < num1 + path.Length)
            path.RemoveRange(appendPathValue4.m_Index - num1, num1 + path.Length - appendPathValue4.m_Index);
          path.EnsureCapacity(path.Length + appendPath.Length - index1);
          elem.m_TargetDelta.x = appendPathValue4.m_TargetDelta;
          path.Add(elem);
          for (int index2 = index1 + 1; index2 < appendPath.Length; ++index2)
            path.Add(appendPath[index2]);
          nativeParallelHashMap.Dispose();
          appendedCount = appendPath.Length - index1;
          return true;
        }
      }
      nativeParallelHashMap.Dispose();
      appendedCount = 0;
      return false;
    }

    public static bool TryAppendPath(
      ref AircraftCurrentLane currentLane,
      DynamicBuffer<AircraftNavigationLane> navigationLanes,
      DynamicBuffer<PathElement> path,
      DynamicBuffer<PathElement> appendPath)
    {
      NativeParallelHashMap<Entity, PathUtils.AppendPathValue> nativeParallelHashMap = new NativeParallelHashMap<Entity, PathUtils.AppendPathValue>(navigationLanes.Length + path.Length + 1, (AllocatorManager.AllocatorHandle) Allocator.Temp);
      if (currentLane.m_Lane != Entity.Null && (navigationLanes.Length == 0 || (navigationLanes[0].m_Flags & AircraftLaneFlags.Reserved) == (AircraftLaneFlags) 0))
        nativeParallelHashMap.TryAdd(currentLane.m_Lane, new PathUtils.AppendPathValue()
        {
          m_Index = 0,
          m_TargetDelta = currentLane.m_CurvePosition.y
        });
      PathUtils.AppendPathValue appendPathValue1;
      for (int index = 1; index <= navigationLanes.Length; ++index)
      {
        if (navigationLanes.Length == index || (navigationLanes[index].m_Flags & AircraftLaneFlags.Reserved) == (AircraftLaneFlags) 0)
        {
          AircraftNavigationLane navigationLane = navigationLanes[index - 1];
          float num = math.select(navigationLane.m_CurvePosition.x, navigationLane.m_CurvePosition.y, (navigationLane.m_Flags & AircraftLaneFlags.Reserved) > (AircraftLaneFlags) 0);
          ref NativeParallelHashMap<Entity, PathUtils.AppendPathValue> local = ref nativeParallelHashMap;
          Entity lane = navigationLane.m_Lane;
          appendPathValue1 = new PathUtils.AppendPathValue();
          appendPathValue1.m_Index = index;
          appendPathValue1.m_TargetDelta = num;
          PathUtils.AppendPathValue appendPathValue2 = appendPathValue1;
          local.TryAdd(lane, appendPathValue2);
        }
      }
      int num1 = navigationLanes.Length + 1;
      for (int index = 0; index < path.Length; ++index)
      {
        PathElement pathElement = path[index];
        ref NativeParallelHashMap<Entity, PathUtils.AppendPathValue> local = ref nativeParallelHashMap;
        Entity target = pathElement.m_Target;
        appendPathValue1 = new PathUtils.AppendPathValue();
        appendPathValue1.m_Index = num1 + index;
        appendPathValue1.m_TargetDelta = pathElement.m_TargetDelta.x;
        PathUtils.AppendPathValue appendPathValue3 = appendPathValue1;
        local.TryAdd(target, appendPathValue3);
      }
      for (int index1 = 0; appendPath.Length > index1; ++index1)
      {
        PathElement elem = appendPath[index1];
        PathUtils.AppendPathValue appendPathValue4;
        if (nativeParallelHashMap.TryGetValue(elem.m_Target, out appendPathValue4))
        {
          if (appendPathValue4.m_Index == 0)
          {
            currentLane.m_CurvePosition.z = appendPathValue4.m_TargetDelta;
            navigationLanes.Clear();
            path.Clear();
          }
          else if (appendPathValue4.m_Index <= navigationLanes.Length)
          {
            AircraftNavigationLane navigationLane = navigationLanes[appendPathValue4.m_Index - 1];
            navigationLane.m_CurvePosition.y = appendPathValue4.m_TargetDelta;
            navigationLanes[appendPathValue4.m_Index - 1] = navigationLane;
            if (appendPathValue4.m_Index < navigationLanes.Length)
              navigationLanes.RemoveRange(appendPathValue4.m_Index, navigationLanes.Length - appendPathValue4.m_Index);
            path.Clear();
          }
          else if (appendPathValue4.m_Index < num1 + path.Length)
            path.RemoveRange(appendPathValue4.m_Index - num1, num1 + path.Length - appendPathValue4.m_Index);
          path.EnsureCapacity(path.Length + appendPath.Length - index1);
          elem.m_TargetDelta.x = appendPathValue4.m_TargetDelta;
          path.Add(elem);
          for (int index2 = index1 + 1; index2 < appendPath.Length; ++index2)
            path.Add(appendPath[index2]);
          nativeParallelHashMap.Dispose();
          return true;
        }
      }
      nativeParallelHashMap.Dispose();
      return false;
    }

    public static bool TryAppendPath(
      ref TrainCurrentLane currentLane,
      DynamicBuffer<TrainNavigationLane> navigationLanes,
      DynamicBuffer<PathElement> path,
      DynamicBuffer<PathElement> appendPath)
    {
      NativeParallelHashMap<Entity, PathUtils.AppendPathValue> nativeParallelHashMap = new NativeParallelHashMap<Entity, PathUtils.AppendPathValue>(navigationLanes.Length + path.Length + 1, (AllocatorManager.AllocatorHandle) Allocator.Temp);
      if (currentLane.m_Front.m_Lane != Entity.Null && (navigationLanes.Length == 0 || (navigationLanes[0].m_Flags & TrainLaneFlags.Reserved) == (TrainLaneFlags) 0))
        nativeParallelHashMap.TryAdd(currentLane.m_Front.m_Lane, new PathUtils.AppendPathValue()
        {
          m_Index = 0,
          m_TargetDelta = currentLane.m_Front.m_CurvePosition.z
        });
      PathUtils.AppendPathValue appendPathValue1;
      for (int index = 1; index <= navigationLanes.Length; ++index)
      {
        if (navigationLanes.Length == index || (navigationLanes[index].m_Flags & TrainLaneFlags.Reserved) == (TrainLaneFlags) 0)
        {
          TrainNavigationLane navigationLane = navigationLanes[index - 1];
          float num = math.select(navigationLane.m_CurvePosition.x, navigationLane.m_CurvePosition.y, (navigationLane.m_Flags & TrainLaneFlags.Reserved) > (TrainLaneFlags) 0);
          ref NativeParallelHashMap<Entity, PathUtils.AppendPathValue> local = ref nativeParallelHashMap;
          Entity lane = navigationLane.m_Lane;
          appendPathValue1 = new PathUtils.AppendPathValue();
          appendPathValue1.m_Index = index;
          appendPathValue1.m_TargetDelta = num;
          PathUtils.AppendPathValue appendPathValue2 = appendPathValue1;
          local.TryAdd(lane, appendPathValue2);
        }
      }
      int num1 = navigationLanes.Length + 1;
      for (int index = 0; index < path.Length; ++index)
      {
        PathElement pathElement = path[index];
        ref NativeParallelHashMap<Entity, PathUtils.AppendPathValue> local = ref nativeParallelHashMap;
        Entity target = pathElement.m_Target;
        appendPathValue1 = new PathUtils.AppendPathValue();
        appendPathValue1.m_Index = num1 + index;
        appendPathValue1.m_TargetDelta = pathElement.m_TargetDelta.x;
        PathUtils.AppendPathValue appendPathValue3 = appendPathValue1;
        local.TryAdd(target, appendPathValue3);
      }
      for (int index1 = 0; appendPath.Length > index1; ++index1)
      {
        PathElement elem = appendPath[index1];
        PathUtils.AppendPathValue appendPathValue4;
        if (nativeParallelHashMap.TryGetValue(elem.m_Target, out appendPathValue4))
        {
          if (appendPathValue4.m_Index == 0)
          {
            currentLane.m_Front.m_CurvePosition.w = appendPathValue4.m_TargetDelta;
            navigationLanes.Clear();
            path.Clear();
          }
          else if (appendPathValue4.m_Index <= navigationLanes.Length)
          {
            TrainNavigationLane navigationLane = navigationLanes[appendPathValue4.m_Index - 1];
            navigationLane.m_CurvePosition.y = appendPathValue4.m_TargetDelta;
            navigationLanes[appendPathValue4.m_Index - 1] = navigationLane;
            if (appendPathValue4.m_Index < navigationLanes.Length)
              navigationLanes.RemoveRange(appendPathValue4.m_Index, navigationLanes.Length - appendPathValue4.m_Index);
            path.Clear();
          }
          else if (appendPathValue4.m_Index < num1 + path.Length)
            path.RemoveRange(appendPathValue4.m_Index - num1, num1 + path.Length - appendPathValue4.m_Index);
          path.EnsureCapacity(path.Length + appendPath.Length - index1);
          elem.m_TargetDelta.x = appendPathValue4.m_TargetDelta;
          path.Add(elem);
          for (int index2 = index1 + 1; index2 < appendPath.Length; ++index2)
            path.Add(appendPath[index2]);
          nativeParallelHashMap.Dispose();
          return true;
        }
      }
      nativeParallelHashMap.Dispose();
      return false;
    }

    public static Entity GetMasterLane(
      Entity lane,
      ComponentLookup<SlaveLane> slaveLaneData,
      ComponentLookup<Owner> ownerData,
      BufferLookup<Game.Net.SubLane> subLanes)
    {
      if (slaveLaneData.HasComponent(lane) && ownerData.HasComponent(lane))
      {
        SlaveLane slaveLane = slaveLaneData[lane];
        Owner owner = ownerData[lane];
        if (subLanes.HasBuffer(owner.m_Owner))
          return subLanes[owner.m_Owner][(int) slaveLane.m_MasterIndex].m_SubLane;
      }
      return lane;
    }

    public static void TryAddCosts(ref PathfindCosts costs, PathfindCosts add)
    {
      costs.m_Value += add.m_Value;
    }

    public static void TryAddCosts(ref PathfindCosts costs, PathfindCosts add, float distance)
    {
      costs.m_Value += add.m_Value * distance;
    }

    public static void TryAddCosts(ref PathfindCosts costs, PathfindCosts add, Bezier4x3 curve)
    {
      float2 xz1 = MathUtils.StartTangent(curve).xz;
      float2 xz2 = MathUtils.EndTangent(curve).xz;
      if (!MathUtils.TryNormalize(ref xz1) || !MathUtils.TryNormalize(ref xz2))
        return;
      float distance = math.acos(math.clamp(math.dot(xz1, xz2), -1f, 1f));
      PathUtils.TryAddCosts(ref costs, add, distance);
    }

    public static void TryAddCosts(ref PathfindCosts costs, PathfindCosts add, bool doIt)
    {
      costs.m_Value = math.select(costs.m_Value, costs.m_Value + add.m_Value, doIt);
    }

    public static void TryAddCosts(
      ref PathfindCosts costs,
      PathfindCosts add,
      float distance,
      bool doIt)
    {
      costs.m_Value = math.select(costs.m_Value, costs.m_Value + add.m_Value * distance, doIt);
    }

    public static PathSpecification GetCarDriveSpecification(
      Curve curveData,
      Game.Net.CarLane carLaneData,
      PathfindCarData carPathfindData,
      float density)
    {
      PathSpecification driveSpecification = new PathSpecification();
      driveSpecification.m_Flags = EdgeFlags.Forward;
      driveSpecification.m_Methods = PathMethod.Road;
      driveSpecification.m_Length = curveData.m_Length;
      driveSpecification.m_MaxSpeed = carLaneData.m_SpeedLimit;
      driveSpecification.m_Density = math.sqrt(density);
      driveSpecification.m_FlowOffset = carLaneData.m_FlowOffset;
      driveSpecification.m_AccessRequirement = math.select(-1, carLaneData.m_AccessRestriction.Index, carLaneData.m_AccessRestriction != Entity.Null);
      PathUtils.TryAddCosts(ref driveSpecification.m_Costs, carPathfindData.m_DrivingCost, driveSpecification.m_Length);
      PathUtils.TryAddCosts(ref driveSpecification.m_Costs, carPathfindData.m_CurveAngleCost, curveData.m_Bezier);
      PathUtils.TryAddCosts(ref driveSpecification.m_Costs, carPathfindData.m_LaneCrossCost, (float) carLaneData.m_LaneCrossCount);
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.Approach) == ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
      {
        bool flag1 = (carLaneData.m_Flags & Game.Net.CarLaneFlags.Forbidden) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
        bool flag2 = (carLaneData.m_Flags & Game.Net.CarLaneFlags.Unsafe) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
        bool flag3 = (carLaneData.m_Flags & Game.Net.CarLaneFlags.Highway) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
        bool doIt1 = (carLaneData.m_Flags & (Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight)) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
        bool doIt2 = (carLaneData.m_Flags & (Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.UTurnRight)) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
        PathUtils.TryAddCosts(ref driveSpecification.m_Costs, carPathfindData.m_ForbiddenCost, flag1 || flag2 & flag3 & doIt2);
        PathUtils.TryAddCosts(ref driveSpecification.m_Costs, carPathfindData.m_TurningCost, doIt1);
        if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.Unsafe) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
          PathUtils.TryAddCosts(ref driveSpecification.m_Costs, carPathfindData.m_UnsafeUTurnCost, doIt2);
        else
          PathUtils.TryAddCosts(ref driveSpecification.m_Costs, carPathfindData.m_UTurnCost, doIt2);
      }
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.Unsafe) == ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        driveSpecification.m_Flags |= EdgeFlags.AllowMiddle;
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.Twoway) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        driveSpecification.m_Flags |= EdgeFlags.Backward;
      if ((int) carLaneData.m_BlockageEnd >= (int) carLaneData.m_BlockageStart)
      {
        driveSpecification.m_Rules |= RuleFlags.HasBlockage;
        driveSpecification.m_BlockageStart = carLaneData.m_BlockageStart;
        driveSpecification.m_BlockageEnd = carLaneData.m_BlockageEnd;
      }
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.ForbidCombustionEngines) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        driveSpecification.m_Rules |= RuleFlags.ForbidCombustionEngines;
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.ForbidTransitTraffic) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        driveSpecification.m_Rules |= RuleFlags.ForbidTransitTraffic;
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.ForbidHeavyTraffic) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        driveSpecification.m_Rules |= RuleFlags.ForbidHeavyTraffic;
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.PublicOnly) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        driveSpecification.m_Rules |= RuleFlags.ForbidPrivateTraffic;
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.Highway) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        driveSpecification.m_Rules |= RuleFlags.ForbidSlowTraffic;
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.SideConnection) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        driveSpecification.m_Flags |= EdgeFlags.SingleOnly;
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.AllowEnter) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        driveSpecification.m_Flags |= EdgeFlags.AllowEnter;
      return driveSpecification;
    }

    public static PathSpecification GetCarDriveSpecification(
      Curve curveData,
      Game.Net.CarLane carLaneData,
      Game.Net.TrackLane trackLaneData,
      PathfindCarData carPathfindData,
      float density)
    {
      PathSpecification driveSpecification = new PathSpecification();
      driveSpecification.m_Flags = EdgeFlags.Forward;
      driveSpecification.m_Methods = PathMethod.Road | PathMethod.Track;
      driveSpecification.m_Length = curveData.m_Length;
      driveSpecification.m_MaxSpeed = carLaneData.m_SpeedLimit;
      driveSpecification.m_Density = math.sqrt(density);
      driveSpecification.m_FlowOffset = carLaneData.m_FlowOffset;
      driveSpecification.m_AccessRequirement = math.select(-1, carLaneData.m_AccessRestriction.Index, carLaneData.m_AccessRestriction != Entity.Null);
      PathUtils.TryAddCosts(ref driveSpecification.m_Costs, carPathfindData.m_DrivingCost, driveSpecification.m_Length);
      PathUtils.TryAddCosts(ref driveSpecification.m_Costs, carPathfindData.m_CurveAngleCost, curveData.m_Bezier);
      PathUtils.TryAddCosts(ref driveSpecification.m_Costs, carPathfindData.m_LaneCrossCost, (float) carLaneData.m_LaneCrossCount);
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.Approach) == ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
      {
        bool flag1 = (carLaneData.m_Flags & Game.Net.CarLaneFlags.Forbidden) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
        bool flag2 = (carLaneData.m_Flags & Game.Net.CarLaneFlags.Unsafe) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
        bool flag3 = (carLaneData.m_Flags & Game.Net.CarLaneFlags.Highway) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
        bool doIt1 = (carLaneData.m_Flags & (Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight)) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
        bool doIt2 = (carLaneData.m_Flags & (Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.UTurnRight)) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
        PathUtils.TryAddCosts(ref driveSpecification.m_Costs, carPathfindData.m_ForbiddenCost, flag1 || flag2 & flag3 & doIt2);
        PathUtils.TryAddCosts(ref driveSpecification.m_Costs, carPathfindData.m_TurningCost, doIt1);
        if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.Unsafe) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
          PathUtils.TryAddCosts(ref driveSpecification.m_Costs, carPathfindData.m_UnsafeUTurnCost, doIt2);
        else
          PathUtils.TryAddCosts(ref driveSpecification.m_Costs, carPathfindData.m_UTurnCost, doIt2);
      }
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.Unsafe) == ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter) || (trackLaneData.m_Flags & TrackLaneFlags.AllowMiddle) != (TrackLaneFlags) 0)
        driveSpecification.m_Flags |= EdgeFlags.AllowMiddle;
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.Twoway) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter) || (trackLaneData.m_Flags & TrackLaneFlags.Twoway) != (TrackLaneFlags) 0)
        driveSpecification.m_Flags |= EdgeFlags.Backward;
      if ((int) carLaneData.m_BlockageEnd >= (int) carLaneData.m_BlockageStart)
      {
        driveSpecification.m_Rules |= RuleFlags.HasBlockage;
        driveSpecification.m_BlockageStart = carLaneData.m_BlockageStart;
        driveSpecification.m_BlockageEnd = carLaneData.m_BlockageEnd;
      }
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.ForbidCombustionEngines) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        driveSpecification.m_Rules |= RuleFlags.ForbidCombustionEngines;
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.ForbidTransitTraffic) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        driveSpecification.m_Rules |= RuleFlags.ForbidTransitTraffic;
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.ForbidHeavyTraffic) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        driveSpecification.m_Rules |= RuleFlags.ForbidHeavyTraffic;
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.PublicOnly) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        driveSpecification.m_Rules |= RuleFlags.ForbidPrivateTraffic;
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.Highway) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        driveSpecification.m_Rules |= RuleFlags.ForbidSlowTraffic;
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.SideConnection) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        driveSpecification.m_Flags |= EdgeFlags.SingleOnly;
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.AllowEnter) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        driveSpecification.m_Flags |= EdgeFlags.AllowEnter;
      return driveSpecification;
    }

    public static PathSpecification GetTaxiDriveSpecification(
      Curve curveData,
      Game.Net.CarLane carLaneData,
      PathfindCarData carPathfindData,
      PathfindTransportData transportPathfindData,
      float density)
    {
      PathSpecification driveSpecification = new PathSpecification();
      driveSpecification.m_Flags = EdgeFlags.Forward | EdgeFlags.SecondaryStart | EdgeFlags.SecondaryEnd;
      driveSpecification.m_Methods = PathMethod.Taxi;
      driveSpecification.m_Length = curveData.m_Length;
      driveSpecification.m_MaxSpeed = carLaneData.m_SpeedLimit;
      driveSpecification.m_Density = math.sqrt(density);
      driveSpecification.m_FlowOffset = carLaneData.m_FlowOffset;
      driveSpecification.m_AccessRequirement = math.select(-1, carLaneData.m_AccessRestriction.Index, carLaneData.m_AccessRestriction != Entity.Null);
      transportPathfindData.m_TravelCost.m_Value.z *= 0.03f;
      PathUtils.TryAddCosts(ref driveSpecification.m_Costs, transportPathfindData.m_TravelCost, driveSpecification.m_Length);
      PathUtils.TryAddCosts(ref driveSpecification.m_Costs, carPathfindData.m_CurveAngleCost, curveData.m_Bezier);
      PathUtils.TryAddCosts(ref driveSpecification.m_Costs, carPathfindData.m_LaneCrossCost, (float) carLaneData.m_LaneCrossCount);
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.Approach) == ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
      {
        bool flag1 = (carLaneData.m_Flags & Game.Net.CarLaneFlags.Forbidden) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
        bool flag2 = (carLaneData.m_Flags & Game.Net.CarLaneFlags.Unsafe) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
        bool flag3 = (carLaneData.m_Flags & Game.Net.CarLaneFlags.Highway) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
        bool doIt1 = (carLaneData.m_Flags & (Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight)) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
        bool doIt2 = (carLaneData.m_Flags & (Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.UTurnRight)) > ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter);
        PathUtils.TryAddCosts(ref driveSpecification.m_Costs, carPathfindData.m_ForbiddenCost, flag1 || flag2 & flag3 & doIt2);
        PathUtils.TryAddCosts(ref driveSpecification.m_Costs, carPathfindData.m_TurningCost, doIt1);
        if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.Unsafe) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
          PathUtils.TryAddCosts(ref driveSpecification.m_Costs, carPathfindData.m_UnsafeUTurnCost, doIt2);
        else
          PathUtils.TryAddCosts(ref driveSpecification.m_Costs, carPathfindData.m_UTurnCost, doIt2);
      }
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.Unsafe) == ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        driveSpecification.m_Flags |= EdgeFlags.AllowMiddle;
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.Twoway) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        driveSpecification.m_Flags |= EdgeFlags.Backward;
      if ((int) carLaneData.m_BlockageEnd >= (int) carLaneData.m_BlockageStart)
      {
        driveSpecification.m_Rules |= RuleFlags.HasBlockage;
        driveSpecification.m_BlockageStart = carLaneData.m_BlockageStart;
        driveSpecification.m_BlockageEnd = carLaneData.m_BlockageEnd;
      }
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.ForbidCombustionEngines) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        driveSpecification.m_Rules |= RuleFlags.ForbidCombustionEngines;
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.ForbidTransitTraffic) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        driveSpecification.m_Rules |= RuleFlags.ForbidTransitTraffic;
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.ForbidHeavyTraffic) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        driveSpecification.m_Rules |= RuleFlags.ForbidHeavyTraffic;
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.PublicOnly) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        driveSpecification.m_Rules |= RuleFlags.ForbidPrivateTraffic;
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.Highway) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        driveSpecification.m_Rules |= RuleFlags.ForbidSlowTraffic;
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.SideConnection) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        driveSpecification.m_Flags |= EdgeFlags.SingleOnly;
      if ((carLaneData.m_Flags & Game.Net.CarLaneFlags.AllowEnter) != ~(Game.Net.CarLaneFlags.Unsafe | Game.Net.CarLaneFlags.UTurnLeft | Game.Net.CarLaneFlags.Invert | Game.Net.CarLaneFlags.SideConnection | Game.Net.CarLaneFlags.TurnLeft | Game.Net.CarLaneFlags.TurnRight | Game.Net.CarLaneFlags.LevelCrossing | Game.Net.CarLaneFlags.Twoway | Game.Net.CarLaneFlags.IsSecured | Game.Net.CarLaneFlags.Runway | Game.Net.CarLaneFlags.Yield | Game.Net.CarLaneFlags.Stop | Game.Net.CarLaneFlags.ForbidCombustionEngines | Game.Net.CarLaneFlags.ForbidTransitTraffic | Game.Net.CarLaneFlags.ForbidHeavyTraffic | Game.Net.CarLaneFlags.PublicOnly | Game.Net.CarLaneFlags.Highway | Game.Net.CarLaneFlags.UTurnRight | Game.Net.CarLaneFlags.GentleTurnLeft | Game.Net.CarLaneFlags.GentleTurnRight | Game.Net.CarLaneFlags.Forward | Game.Net.CarLaneFlags.Approach | Game.Net.CarLaneFlags.Roundabout | Game.Net.CarLaneFlags.RightLimit | Game.Net.CarLaneFlags.LeftLimit | Game.Net.CarLaneFlags.ForbidPassing | Game.Net.CarLaneFlags.RightOfWay | Game.Net.CarLaneFlags.TrafficLights | Game.Net.CarLaneFlags.ParkingLeft | Game.Net.CarLaneFlags.ParkingRight | Game.Net.CarLaneFlags.Forbidden | Game.Net.CarLaneFlags.AllowEnter))
        driveSpecification.m_Flags |= EdgeFlags.AllowEnter;
      return driveSpecification;
    }

    public static PathSpecification GetTrackDriveSpecification(
      Curve curveData,
      Game.Net.TrackLane trackLaneData,
      PathfindTrackData trackPathfindData)
    {
      PathSpecification driveSpecification = new PathSpecification();
      driveSpecification.m_Flags = (trackLaneData.m_Flags & TrackLaneFlags.Twoway) != (TrackLaneFlags) 0 ? EdgeFlags.Forward | EdgeFlags.Backward : EdgeFlags.Forward;
      driveSpecification.m_Methods = PathMethod.Track;
      driveSpecification.m_Length = curveData.m_Length;
      driveSpecification.m_MaxSpeed = trackLaneData.m_SpeedLimit;
      driveSpecification.m_Density = 0.0f;
      driveSpecification.m_AccessRequirement = math.select(-1, trackLaneData.m_AccessRestriction.Index, trackLaneData.m_AccessRestriction != Entity.Null);
      if ((trackLaneData.m_Flags & TrackLaneFlags.AllowMiddle) != (TrackLaneFlags) 0)
        driveSpecification.m_Flags |= EdgeFlags.AllowMiddle;
      PathUtils.TryAddCosts(ref driveSpecification.m_Costs, trackPathfindData.m_DrivingCost, driveSpecification.m_Length);
      PathUtils.TryAddCosts(ref driveSpecification.m_Costs, trackPathfindData.m_CurveAngleCost, curveData.m_Bezier);
      PathUtils.TryAddCosts(ref driveSpecification.m_Costs, trackPathfindData.m_TwowayCost, (trackLaneData.m_Flags & TrackLaneFlags.Twoway) != 0);
      PathUtils.TryAddCosts(ref driveSpecification.m_Costs, trackPathfindData.m_SwitchCost, (trackLaneData.m_Flags & TrackLaneFlags.Switch) != 0);
      PathUtils.TryAddCosts(ref driveSpecification.m_Costs, trackPathfindData.m_SwitchCost, (trackLaneData.m_Flags & TrackLaneFlags.DoubleSwitch) != 0);
      PathUtils.TryAddCosts(ref driveSpecification.m_Costs, trackPathfindData.m_DiamondCrossingCost, (trackLaneData.m_Flags & TrackLaneFlags.DiamondCrossing) != 0);
      return driveSpecification;
    }

    public static PathSpecification GetParkingSpaceSpecification(
      Game.Net.ParkingLane parkingLane,
      ParkingLaneData parkingLaneData,
      PathfindCarData carPathfindData)
    {
      PathSpecification spaceSpecification = new PathSpecification();
      spaceSpecification.m_Flags = EdgeFlags.Forward | EdgeFlags.Backward | EdgeFlags.FreeBackward;
      spaceSpecification.m_Methods = PathMethod.Boarding;
      spaceSpecification.m_Length = 0.0f;
      spaceSpecification.m_MaxSpeed = math.max(1f, parkingLane.m_FreeSpace);
      spaceSpecification.m_Density = VehicleUtils.GetParkingSize(parkingLaneData).x;
      spaceSpecification.m_AccessRequirement = math.select(-1, parkingLane.m_AccessRestriction.Index, parkingLane.m_AccessRestriction != Entity.Null);
      if ((parkingLane.m_Flags & ParkingLaneFlags.VirtualLane) == (ParkingLaneFlags) 0)
      {
        spaceSpecification.m_Methods |= (parkingLane.m_Flags & ParkingLaneFlags.SpecialVehicles) != (ParkingLaneFlags) 0 ? PathMethod.SpecialParking : PathMethod.Parking;
        spaceSpecification.m_MaxSpeed = math.select(spaceSpecification.m_MaxSpeed, 1f, (parkingLane.m_Flags & ParkingLaneFlags.ParkingDisabled) != 0);
      }
      if ((parkingLane.m_Flags & ParkingLaneFlags.AllowEnter) != (ParkingLaneFlags) 0)
        spaceSpecification.m_Flags |= EdgeFlags.AllowEnter;
      if ((parkingLane.m_Flags & ParkingLaneFlags.AllowExit) != (ParkingLaneFlags) 0)
        spaceSpecification.m_Flags |= EdgeFlags.AllowExit;
      carPathfindData.m_ParkingCost.m_Value.z *= (float) parkingLane.m_ParkingFee;
      carPathfindData.m_ParkingCost.m_Value.w *= (float) ((int) ushort.MaxValue - (int) parkingLane.m_ComfortFactor) * 1.52590219E-05f;
      PathUtils.TryAddCosts(ref spaceSpecification.m_Costs, carPathfindData.m_ParkingCost);
      return spaceSpecification;
    }

    public static float GetTaxiAvailabilityDelay(Game.Net.ParkingLane parkingLaneData)
    {
      return (float) (100.0 / (0.25 + (double) parkingLaneData.m_TaxiAvailability * 1.5259021893143654E-05) - 80.0);
    }

    public static PathSpecification GetTaxiAccessSpecification(
      Game.Net.ParkingLane parkingLaneData,
      PathfindCarData carPathfindData,
      PathfindTransportData transportPathfindData)
    {
      PathSpecification accessSpecification = new PathSpecification();
      accessSpecification.m_Flags = EdgeFlags.Forward | EdgeFlags.SecondaryStart | EdgeFlags.FreeForward;
      accessSpecification.m_Methods = PathMethod.Taxi;
      accessSpecification.m_Length = 0.0f;
      accessSpecification.m_MaxSpeed = 1f;
      accessSpecification.m_Density = 0.0f;
      accessSpecification.m_AccessRequirement = math.select(-1, parkingLaneData.m_AccessRestriction.Index, parkingLaneData.m_AccessRestriction != Entity.Null);
      if ((parkingLaneData.m_Flags & ParkingLaneFlags.AllowEnter) != (ParkingLaneFlags) 0)
        accessSpecification.m_Flags |= EdgeFlags.AllowEnter;
      if ((parkingLaneData.m_Flags & ParkingLaneFlags.AllowExit) != (ParkingLaneFlags) 0)
        accessSpecification.m_Flags |= EdgeFlags.AllowExit;
      if (parkingLaneData.m_TaxiAvailability != (ushort) 0)
      {
        accessSpecification.m_Flags |= EdgeFlags.Backward;
        transportPathfindData.m_OrderingCost.m_Value.x += PathUtils.GetTaxiAvailabilityDelay(parkingLaneData);
        transportPathfindData.m_StartingCost.m_Value.z *= (float) parkingLaneData.m_TaxiFee;
        PathUtils.TryAddCosts(ref accessSpecification.m_Costs, transportPathfindData.m_OrderingCost);
        PathUtils.TryAddCosts(ref accessSpecification.m_Costs, transportPathfindData.m_StartingCost);
      }
      return accessSpecification;
    }

    public static PathSpecification GetSpecification(
      Curve curveData,
      Game.Net.PedestrianLane pedestrianLaneData,
      PathfindPedestrianData pedestrianPathfindData)
    {
      PathSpecification specification = new PathSpecification();
      specification.m_Flags = EdgeFlags.Forward | EdgeFlags.Backward;
      specification.m_Methods = PathMethod.Pedestrian;
      specification.m_Length = curveData.m_Length;
      specification.m_MaxSpeed = 5.555556f;
      specification.m_Density = 1f;
      specification.m_AccessRequirement = math.select(-1, pedestrianLaneData.m_AccessRestriction.Index, pedestrianLaneData.m_AccessRestriction != Entity.Null);
      if ((pedestrianLaneData.m_Flags & PedestrianLaneFlags.AllowMiddle) != (PedestrianLaneFlags) 0)
        specification.m_Flags |= EdgeFlags.AllowMiddle;
      if ((pedestrianLaneData.m_Flags & PedestrianLaneFlags.AllowEnter) != (PedestrianLaneFlags) 0)
        specification.m_Flags |= EdgeFlags.AllowEnter;
      if ((pedestrianLaneData.m_Flags & PedestrianLaneFlags.ForbidTransitTraffic) != (PedestrianLaneFlags) 0)
        specification.m_Rules |= RuleFlags.ForbidTransitTraffic;
      if ((pedestrianLaneData.m_Flags & PedestrianLaneFlags.OnWater) != (PedestrianLaneFlags) 0)
        specification.m_Flags &= ~(EdgeFlags.Forward | EdgeFlags.Backward);
      PathUtils.TryAddCosts(ref specification.m_Costs, pedestrianPathfindData.m_WalkingCost, specification.m_Length);
      PathUtils.TryAddCosts(ref specification.m_Costs, pedestrianPathfindData.m_CrosswalkCost, (pedestrianLaneData.m_Flags & (PedestrianLaneFlags.Unsafe | PedestrianLaneFlags.Crosswalk)) == PedestrianLaneFlags.Crosswalk);
      PathUtils.TryAddCosts(ref specification.m_Costs, pedestrianPathfindData.m_UnsafeCrosswalkCost, (pedestrianLaneData.m_Flags & (PedestrianLaneFlags.Unsafe | PedestrianLaneFlags.Crosswalk)) == (PedestrianLaneFlags.Unsafe | PedestrianLaneFlags.Crosswalk));
      return specification;
    }

    public static PathSpecification GetSpecification(
      Curve curveData,
      Game.Net.ConnectionLane connectionLaneData,
      GarageLane garageLane,
      Game.Net.OutsideConnection outsideConnection,
      PathfindConnectionData connectionPathfindData)
    {
      PathSpecification specification = new PathSpecification();
      specification.m_Flags = EdgeFlags.Forward | EdgeFlags.Backward;
      specification.m_AccessRequirement = math.select(-1, connectionLaneData.m_AccessRestriction.Index, connectionLaneData.m_AccessRestriction != Entity.Null);
      specification.m_Costs.m_Value.x = outsideConnection.m_Delay;
      if ((connectionLaneData.m_Flags & ConnectionLaneFlags.Parking) != (ConnectionLaneFlags) 0)
      {
        specification.m_Methods |= PathMethod.Parking;
        if ((connectionLaneData.m_Flags & ConnectionLaneFlags.Outside) != (ConnectionLaneFlags) 0)
        {
          specification.m_Methods |= PathMethod.Boarding;
          specification.m_MaxSpeed = 1000000f;
          specification.m_Density = 1000000f;
        }
        else
        {
          bool c = (int) garageLane.m_VehicleCount < (int) garageLane.m_VehicleCapacity && (connectionLaneData.m_Flags & ConnectionLaneFlags.Disabled) == (ConnectionLaneFlags) 0;
          specification.m_Flags |= EdgeFlags.FreeBackward;
          specification.m_MaxSpeed = math.select(1f, 1000000f, c);
          specification.m_Density = math.select(0.0f, 1000000f, c);
          connectionPathfindData.m_ParkingCost.m_Value.z *= (float) garageLane.m_ParkingFee;
          connectionPathfindData.m_ParkingCost.m_Value.w *= (float) ((int) ushort.MaxValue - (int) garageLane.m_ComfortFactor) * 1.52590219E-05f;
          PathUtils.TryAddCosts(ref specification.m_Costs, connectionPathfindData.m_ParkingCost);
        }
      }
      else if ((connectionLaneData.m_Flags & ConnectionLaneFlags.Airway) != (ConnectionLaneFlags) 0)
      {
        specification.m_Length = curveData.m_Length;
        specification.m_MaxSpeed = math.select(83.3333359f, 277.777771f, (connectionLaneData.m_RoadTypes & RoadTypes.Airplane) != 0);
      }
      else if ((connectionLaneData.m_Flags & (ConnectionLaneFlags.Inside | ConnectionLaneFlags.Area)) != (ConnectionLaneFlags) 0)
      {
        specification.m_Length = curveData.m_Length;
        specification.m_MaxSpeed = math.select(3f, 5.555556f, (connectionLaneData.m_Flags & ConnectionLaneFlags.Pedestrian) != 0);
      }
      else
        specification.m_MaxSpeed = 1f;
      if ((connectionLaneData.m_Flags & ConnectionLaneFlags.Road) != (ConnectionLaneFlags) 0)
      {
        if ((connectionLaneData.m_Flags & ConnectionLaneFlags.Area) != (ConnectionLaneFlags) 0)
          specification.m_Methods |= PathMethod.Offroad;
        else
          specification.m_Methods |= PathMethod.Road;
      }
      if ((connectionLaneData.m_Flags & ConnectionLaneFlags.Track) != (ConnectionLaneFlags) 0)
        specification.m_Methods |= PathMethod.Track;
      if ((connectionLaneData.m_Flags & ConnectionLaneFlags.Pedestrian) != (ConnectionLaneFlags) 0)
        specification.m_Methods |= PathMethod.Pedestrian;
      if ((connectionLaneData.m_Flags & ConnectionLaneFlags.AllowMiddle) != (ConnectionLaneFlags) 0)
        specification.m_Flags |= EdgeFlags.AllowMiddle;
      if ((connectionLaneData.m_Flags & ConnectionLaneFlags.AllowCargo) != (ConnectionLaneFlags) 0)
        specification.m_Methods |= PathMethod.CargoLoading;
      if ((connectionLaneData.m_Flags & ConnectionLaneFlags.Airway) != (ConnectionLaneFlags) 0)
        specification.m_Methods |= PathMethod.Flying;
      if ((connectionLaneData.m_Flags & ConnectionLaneFlags.Outside) != (ConnectionLaneFlags) 0)
        specification.m_Flags |= EdgeFlags.OutsideConnection;
      if ((connectionLaneData.m_Flags & ConnectionLaneFlags.AllowEnter) != (ConnectionLaneFlags) 0)
        specification.m_Flags |= EdgeFlags.AllowEnter;
      if ((connectionLaneData.m_Flags & ConnectionLaneFlags.AllowExit) != (ConnectionLaneFlags) 0)
        specification.m_Flags |= EdgeFlags.AllowExit;
      if ((connectionLaneData.m_Flags & ConnectionLaneFlags.Parking) == (ConnectionLaneFlags) 0)
      {
        PathUtils.TryAddCosts(ref specification.m_Costs, (connectionLaneData.m_Flags & ConnectionLaneFlags.Pedestrian) != (ConnectionLaneFlags) 0 ? connectionPathfindData.m_PedestrianBorderCost : connectionPathfindData.m_BorderCost, (connectionLaneData.m_Flags & ConnectionLaneFlags.Start) != 0);
        PathUtils.TryAddCosts(ref specification.m_Costs, connectionPathfindData.m_DistanceCost, math.select(0.0f, curveData.m_Length, (connectionLaneData.m_Flags & ConnectionLaneFlags.Distance) != 0));
      }
      PathUtils.TryAddCosts(ref specification.m_Costs, connectionPathfindData.m_AirwayCost, specification.m_Length, (connectionLaneData.m_Flags & ConnectionLaneFlags.Airway) != 0);
      PathUtils.TryAddCosts(ref specification.m_Costs, connectionPathfindData.m_InsideCost, specification.m_Length, (connectionLaneData.m_Flags & ConnectionLaneFlags.Inside) != 0);
      PathUtils.TryAddCosts(ref specification.m_Costs, connectionPathfindData.m_AreaCost, specification.m_Length, (connectionLaneData.m_Flags & ConnectionLaneFlags.Area) != 0);
      return specification;
    }

    public static PathSpecification GetSecondarySpecification(
      Curve curveData,
      Game.Net.ConnectionLane connectionLaneData,
      Game.Net.OutsideConnection outsideConnection,
      PathfindConnectionData connectionPathfindData)
    {
      PathSpecification secondarySpecification = new PathSpecification();
      secondarySpecification.m_Flags = EdgeFlags.Forward | EdgeFlags.Backward;
      secondarySpecification.m_Length = 0.0f;
      secondarySpecification.m_MaxSpeed = 1f;
      secondarySpecification.m_Density = 0.0f;
      secondarySpecification.m_AccessRequirement = math.select(-1, connectionLaneData.m_AccessRestriction.Index, connectionLaneData.m_AccessRestriction != Entity.Null);
      secondarySpecification.m_Costs.m_Value.x = outsideConnection.m_Delay;
      if ((connectionLaneData.m_Flags & ConnectionLaneFlags.Road) != (ConnectionLaneFlags) 0)
        secondarySpecification.m_Methods |= PathMethod.Taxi;
      if ((connectionLaneData.m_Flags & ConnectionLaneFlags.Track) != (ConnectionLaneFlags) 0)
        secondarySpecification.m_Methods |= PathMethod.Track;
      if ((connectionLaneData.m_Flags & ConnectionLaneFlags.Pedestrian) != (ConnectionLaneFlags) 0)
        secondarySpecification.m_Methods |= PathMethod.Pedestrian;
      if ((connectionLaneData.m_Flags & ConnectionLaneFlags.AllowMiddle) != (ConnectionLaneFlags) 0)
        secondarySpecification.m_Flags |= EdgeFlags.AllowMiddle;
      if ((connectionLaneData.m_Flags & ConnectionLaneFlags.SecondaryStart) != (ConnectionLaneFlags) 0)
        secondarySpecification.m_Flags |= EdgeFlags.SecondaryStart;
      if ((connectionLaneData.m_Flags & ConnectionLaneFlags.SecondaryEnd) != (ConnectionLaneFlags) 0)
        secondarySpecification.m_Flags |= EdgeFlags.SecondaryEnd;
      if ((connectionLaneData.m_Flags & ConnectionLaneFlags.Outside) != (ConnectionLaneFlags) 0)
        secondarySpecification.m_Flags |= EdgeFlags.OutsideConnection;
      if ((connectionLaneData.m_Flags & ConnectionLaneFlags.AllowEnter) != (ConnectionLaneFlags) 0)
        secondarySpecification.m_Flags |= EdgeFlags.AllowEnter;
      if ((connectionLaneData.m_Flags & ConnectionLaneFlags.AllowExit) != (ConnectionLaneFlags) 0)
        secondarySpecification.m_Flags |= EdgeFlags.AllowExit;
      if ((connectionLaneData.m_Flags & ConnectionLaneFlags.Parking) != (ConnectionLaneFlags) 0)
      {
        secondarySpecification.m_Methods |= PathMethod.Taxi;
        secondarySpecification.m_Flags |= EdgeFlags.FreeForward;
        PathUtils.TryAddCosts(ref secondarySpecification.m_Costs, connectionPathfindData.m_TaxiStartCost);
      }
      else
      {
        PathUtils.TryAddCosts(ref secondarySpecification.m_Costs, (connectionLaneData.m_Flags & ConnectionLaneFlags.Pedestrian) != (ConnectionLaneFlags) 0 ? connectionPathfindData.m_PedestrianBorderCost : connectionPathfindData.m_BorderCost, (connectionLaneData.m_Flags & ConnectionLaneFlags.Start) != 0);
        PathUtils.TryAddCosts(ref secondarySpecification.m_Costs, connectionPathfindData.m_DistanceCost, math.select(0.0f, curveData.m_Length, (connectionLaneData.m_Flags & ConnectionLaneFlags.Distance) != 0));
      }
      return secondarySpecification;
    }

    public static PathSpecification GetTransportStopSpecification(
      Game.Routes.TransportStop transportStop,
      TransportLine transportLine,
      WaitingPassengers waitingPassengers,
      TransportLineData transportLineData,
      PathfindTransportData transportPathfindData,
      bool isWaypoint)
    {
      PathSpecification stopSpecification = new PathSpecification();
      stopSpecification.m_Flags = EdgeFlags.FreeBackward;
      stopSpecification.m_Length = 0.0f;
      stopSpecification.m_MaxSpeed = 1f;
      stopSpecification.m_Density = 0.0f;
      stopSpecification.m_AccessRequirement = math.select(-1, transportStop.m_AccessRestriction.Index, transportStop.m_AccessRestriction != Entity.Null);
      if ((transportStop.m_Flags & StopFlags.Active) != (StopFlags) 0)
        stopSpecification.m_Flags |= EdgeFlags.Forward;
      if ((transportStop.m_Flags & StopFlags.AllowEnter) != (StopFlags) 0)
        stopSpecification.m_Flags |= EdgeFlags.AllowEnter;
      if (!isWaypoint)
        stopSpecification.m_Flags |= EdgeFlags.Backward;
      if (transportStop.m_AccessRestriction != Entity.Null)
        stopSpecification.m_Flags |= EdgeFlags.AllowExit;
      if (transportLineData.m_PassengerTransport)
        stopSpecification.m_Methods |= PathMethod.PublicTransportDay | PathMethod.PublicTransportNight;
      if (transportLineData.m_CargoTransport)
        stopSpecification.m_Methods |= PathMethod.CargoTransport;
      float stopDuration = RouteUtils.GetStopDuration(transportLineData, transportStop);
      float num = math.max(transportLine.m_VehicleInterval * 0.5f, (float) waitingPassengers.m_AverageWaitingTime) - stopDuration;
      transportPathfindData.m_StartingCost.m_Value.x = math.max(0.0f, transportPathfindData.m_StartingCost.m_Value.x + num);
      transportPathfindData.m_StartingCost.m_Value.z *= (float) transportLine.m_TicketPrice;
      transportPathfindData.m_StartingCost.m_Value.w *= 1f - transportStop.m_ComfortFactor;
      PathUtils.TryAddCosts(ref stopSpecification.m_Costs, transportPathfindData.m_StartingCost);
      return stopSpecification;
    }

    public static PathSpecification GetTaxiStopSpecification(
      Game.Routes.TransportStop transportStop,
      TaxiStand taxiStand,
      WaitingPassengers waitingPassengers,
      PathfindTransportData transportPathfindData)
    {
      PathSpecification stopSpecification = new PathSpecification();
      stopSpecification.m_Flags = EdgeFlags.SecondaryEnd;
      stopSpecification.m_Methods = PathMethod.Taxi;
      stopSpecification.m_Length = 0.0f;
      stopSpecification.m_MaxSpeed = 1f;
      stopSpecification.m_Density = 0.0f;
      stopSpecification.m_AccessRequirement = math.select(-1, transportStop.m_AccessRestriction.Index, transportStop.m_AccessRestriction != Entity.Null);
      if ((transportStop.m_Flags & StopFlags.Active) != (StopFlags) 0)
        stopSpecification.m_Flags |= EdgeFlags.Forward;
      if ((transportStop.m_Flags & StopFlags.AllowEnter) != (StopFlags) 0)
        stopSpecification.m_Flags |= EdgeFlags.AllowEnter;
      if (transportStop.m_AccessRestriction != Entity.Null)
        stopSpecification.m_Flags |= EdgeFlags.AllowExit;
      transportPathfindData.m_StartingCost.m_Value.x += (float) waitingPassengers.m_AverageWaitingTime;
      transportPathfindData.m_StartingCost.m_Value.z *= (float) taxiStand.m_StartingFee;
      transportPathfindData.m_StartingCost.m_Value.w *= 1f - transportStop.m_ComfortFactor;
      PathUtils.TryAddCosts(ref stopSpecification.m_Costs, transportPathfindData.m_StartingCost);
      return stopSpecification;
    }

    public static PathSpecification GetSpawnLocationSpecification(
      PathfindPedestrianData pedestrianPathfindData,
      float distance,
      Entity accessRestriction,
      bool requireAuthorization,
      bool allowEnter)
    {
      PathSpecification locationSpecification = new PathSpecification();
      locationSpecification.m_Flags = EdgeFlags.Forward | EdgeFlags.Backward;
      locationSpecification.m_Methods = PathMethod.Pedestrian;
      locationSpecification.m_Length = distance;
      locationSpecification.m_MaxSpeed = 5.555556f;
      locationSpecification.m_AccessRequirement = math.select(-1, accessRestriction.Index, accessRestriction != Entity.Null);
      if (requireAuthorization)
        locationSpecification.m_Flags |= EdgeFlags.RequireAuthorization;
      if (allowEnter)
        locationSpecification.m_Flags |= EdgeFlags.AllowEnter;
      PathUtils.TryAddCosts(ref locationSpecification.m_Costs, pedestrianPathfindData.m_SpawnCost);
      return locationSpecification;
    }

    public static PathSpecification GetSpawnLocationSpecification(
      RouteConnectionType connectionType,
      PathfindCarData carPathfindData,
      Game.Net.CarLane carLane,
      float distance,
      int laneCrossCount,
      Entity accessRestriction,
      bool requireAuthorization,
      bool allowEnter)
    {
      PathSpecification locationSpecification = new PathSpecification();
      locationSpecification.m_Flags = EdgeFlags.Forward | EdgeFlags.Backward;
      locationSpecification.m_Length = distance;
      locationSpecification.m_MaxSpeed = carLane.m_SpeedLimit;
      locationSpecification.m_AccessRequirement = math.select(-1, accessRestriction.Index, accessRestriction != Entity.Null);
      if (requireAuthorization)
        locationSpecification.m_Flags |= EdgeFlags.RequireAuthorization;
      if (allowEnter)
        locationSpecification.m_Flags |= EdgeFlags.AllowEnter;
      switch (connectionType)
      {
        case RouteConnectionType.Road:
        case RouteConnectionType.Parking:
          locationSpecification.m_Methods = PathMethod.Road;
          locationSpecification.m_Flags |= EdgeFlags.SingleOnly;
          break;
        case RouteConnectionType.Cargo:
          locationSpecification.m_Methods = PathMethod.CargoLoading;
          locationSpecification.m_Flags |= EdgeFlags.SingleOnly;
          break;
      }
      PathUtils.TryAddCosts(ref locationSpecification.m_Costs, carPathfindData.m_SpawnCost);
      PathUtils.TryAddCosts(ref locationSpecification.m_Costs, carPathfindData.m_DrivingCost, distance);
      PathUtils.TryAddCosts(ref locationSpecification.m_Costs, carPathfindData.m_LaneCrossCost, (float) laneCrossCount);
      return locationSpecification;
    }

    public static PathSpecification GetSpawnLocationSpecification(
      PathfindTrackData trackPathfindData,
      Entity accessRestriction)
    {
      PathSpecification locationSpecification = new PathSpecification();
      locationSpecification.m_Flags = EdgeFlags.Forward | EdgeFlags.Backward;
      locationSpecification.m_Methods = PathMethod.Track;
      locationSpecification.m_MaxSpeed = 1f;
      locationSpecification.m_AccessRequirement = math.select(-1, accessRestriction.Index, accessRestriction != Entity.Null);
      PathUtils.TryAddCosts(ref locationSpecification.m_Costs, trackPathfindData.m_SpawnCost);
      return locationSpecification;
    }

    public static PathSpecification GetSpawnLocationSpecification(
      RouteConnectionType connectionType,
      PathfindConnectionData connectionPathfindData,
      RoadTypes roadType,
      float distance,
      Entity accessRestriction,
      bool requireAuthorization,
      bool allowEnter)
    {
      PathSpecification locationSpecification = new PathSpecification();
      locationSpecification.m_Flags = EdgeFlags.Forward | EdgeFlags.Backward;
      locationSpecification.m_AccessRequirement = math.select(-1, accessRestriction.Index, accessRestriction != Entity.Null);
      if (requireAuthorization)
        locationSpecification.m_Flags |= EdgeFlags.RequireAuthorization;
      if (allowEnter)
        locationSpecification.m_Flags |= EdgeFlags.AllowEnter;
      switch (connectionType)
      {
        case RouteConnectionType.Road:
          locationSpecification.m_Methods = PathMethod.Road;
          locationSpecification.m_Flags |= EdgeFlags.SingleOnly;
          if (roadType == RoadTypes.Car)
          {
            locationSpecification.m_Length = distance;
            locationSpecification.m_MaxSpeed = 3f;
            PathUtils.TryAddCosts(ref locationSpecification.m_Costs, connectionPathfindData.m_CarSpawnCost);
            break;
          }
          locationSpecification.m_MaxSpeed = 1f;
          break;
        case RouteConnectionType.Pedestrian:
          locationSpecification.m_Methods = PathMethod.Pedestrian;
          locationSpecification.m_Length = distance;
          locationSpecification.m_MaxSpeed = 5.555556f;
          PathUtils.TryAddCosts(ref locationSpecification.m_Costs, connectionPathfindData.m_PedestrianSpawnCost);
          break;
        case RouteConnectionType.Track:
          locationSpecification.m_Methods = PathMethod.Track;
          locationSpecification.m_MaxSpeed = 1f;
          break;
        case RouteConnectionType.Air:
          locationSpecification.m_Methods = PathMethod.Flying;
          switch (roadType)
          {
            case RoadTypes.Helicopter:
              locationSpecification.m_Length = 750f;
              locationSpecification.m_MaxSpeed = 83.3333359f;
              PathUtils.TryAddCosts(ref locationSpecification.m_Costs, connectionPathfindData.m_HelicopterTakeoffCost);
              break;
            case RoadTypes.Airplane:
              locationSpecification.m_Length = 1500f;
              locationSpecification.m_MaxSpeed = 277.777771f;
              PathUtils.TryAddCosts(ref locationSpecification.m_Costs, connectionPathfindData.m_AirplaneTakeoffCost);
              break;
            default:
              locationSpecification.m_MaxSpeed = 1f;
              break;
          }
          break;
      }
      return locationSpecification;
    }

    public static PathSpecification GetTransportLineSpecification(
      TransportLineData transportLineData,
      PathfindTransportData transportPathfindData,
      RouteInfo routeInfo)
    {
      PathSpecification lineSpecification = new PathSpecification();
      lineSpecification.m_Length = routeInfo.m_Distance;
      lineSpecification.m_MaxSpeed = math.max(1f, routeInfo.m_Distance) / math.max(1f, routeInfo.m_Duration);
      lineSpecification.m_Density = 1f;
      lineSpecification.m_AccessRequirement = -1;
      if ((double) routeInfo.m_Distance > 0.0 && (double) routeInfo.m_Duration > 0.0)
        lineSpecification.m_Flags |= EdgeFlags.Forward;
      if (transportLineData.m_PassengerTransport)
      {
        if ((routeInfo.m_Flags & RouteInfoFlags.InactiveDay) == (RouteInfoFlags) 0)
          lineSpecification.m_Methods |= PathMethod.PublicTransportDay;
        if ((routeInfo.m_Flags & RouteInfoFlags.InactiveNight) == (RouteInfoFlags) 0)
          lineSpecification.m_Methods |= PathMethod.PublicTransportNight;
      }
      if (transportLineData.m_CargoTransport && (routeInfo.m_Flags & (RouteInfoFlags.InactiveDay | RouteInfoFlags.InactiveNight)) != (RouteInfoFlags.InactiveDay | RouteInfoFlags.InactiveNight))
        lineSpecification.m_Methods |= PathMethod.CargoTransport;
      PathUtils.TryAddCosts(ref lineSpecification.m_Costs, transportPathfindData.m_TravelCost, routeInfo.m_Distance);
      return lineSpecification;
    }

    public static LocationSpecification GetLocationSpecification(Curve curveData)
    {
      return new LocationSpecification()
      {
        m_Line = new Line3.Segment(curveData.m_Bezier.a, curveData.m_Bezier.d)
      };
    }

    public static LocationSpecification GetLocationSpecification(
      Curve curveData,
      Game.Net.ParkingLane parkingLaneData)
    {
      LocationSpecification locationSpecification = new LocationSpecification();
      float3 float3 = MathUtils.Position(curveData.m_Bezier, 0.5f);
      locationSpecification.m_Line = new Line3.Segment(float3, float3);
      return locationSpecification;
    }

    public static LocationSpecification GetLocationSpecification(float3 position)
    {
      return new LocationSpecification()
      {
        m_Line = new Line3.Segment(position, position)
      };
    }

    public static LocationSpecification GetLocationSpecification(float3 position1, float3 position2)
    {
      return new LocationSpecification()
      {
        m_Line = new Line3.Segment(position1, position2)
      };
    }

    public static void UpdateOwnedVehicleMethods(
      Entity householdEntity,
      ref BufferLookup<OwnedVehicle> ownedVehicleBuffs,
      ref PathfindParameters parameters,
      ref SetupQueueTarget origin,
      ref SetupQueueTarget destination)
    {
      DynamicBuffer<OwnedVehicle> bufferData;
      if (!ownedVehicleBuffs.TryGetBuffer(householdEntity, out bufferData) || bufferData.Length == 0)
        return;
      parameters.m_Methods |= PathMethod.Road | PathMethod.Parking;
      parameters.m_ParkingSize = (float2) float.MinValue;
      parameters.m_IgnoredRules |= RuleFlags.ForbidCombustionEngines | RuleFlags.ForbidHeavyTraffic | RuleFlags.ForbidSlowTraffic;
      origin.m_Methods |= PathMethod.Road;
      origin.m_RoadTypes |= RoadTypes.Car;
      destination.m_Methods |= PathMethod.Road;
      destination.m_RoadTypes |= RoadTypes.Car;
    }

    public static bool IsPathfindingPurpose(Game.Citizens.Purpose purpose)
    {
      if ((uint) purpose <= 12U)
      {
        if (purpose != Game.Citizens.Purpose.GoingHome && purpose != Game.Citizens.Purpose.Hospital)
          goto label_4;
      }
      else
      {
        switch (purpose)
        {
          case Game.Citizens.Purpose.Safety:
          case Game.Citizens.Purpose.EmergencyShelter:
          case Game.Citizens.Purpose.Crime:
          case Game.Citizens.Purpose.Escape:
          case Game.Citizens.Purpose.Sightseeing:
          case Game.Citizens.Purpose.VisitAttractions:
            break;
          default:
            goto label_4;
        }
      }
      return true;
label_4:
      return false;
    }

    private struct AppendPathValue
    {
      public float m_TargetDelta;
      public int m_Index;
    }
  }
}
