// Decompiled with JetBrains decompiler
// Type: Game.Routes.RaycastJobs
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Prefabs;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Routes
{
  public static class RaycastJobs
  {
    [BurstCompile]
    public struct FindRoutesFromTreeJob : IJob
    {
      [ReadOnly]
      public NativeArray<RaycastInput> m_Input;
      [ReadOnly]
      public NativeQuadTree<RouteSearchItem, QuadTreeBoundsXZ> m_SearchTree;
      [WriteOnly]
      public NativeList<RaycastJobs.RouteItem> m_RouteList;

      public void Execute()
      {
        for (int index = 0; index < this.m_Input.Length; ++index)
        {
          RaycastInput raycastInput = this.m_Input[index];
          if ((raycastInput.m_TypeMask & (TypeMask.RouteWaypoints | TypeMask.RouteSegments)) != TypeMask.None)
          {
            RaycastJobs.FindRoutesFromTreeJob.FindRoutesIterator iterator = new RaycastJobs.FindRoutesFromTreeJob.FindRoutesIterator()
            {
              m_RaycastIndex = index,
              m_Line = raycastInput.m_Line,
              m_RouteList = this.m_RouteList
            };
            this.m_SearchTree.Iterate<RaycastJobs.FindRoutesFromTreeJob.FindRoutesIterator>(ref iterator);
          }
        }
      }

      private struct FindRoutesIterator : 
        INativeQuadTreeIterator<RouteSearchItem, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<RouteSearchItem, QuadTreeBoundsXZ>
      {
        public int m_RaycastIndex;
        public Line3.Segment m_Line;
        public NativeList<RaycastJobs.RouteItem> m_RouteList;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          return MathUtils.Intersect(bounds.m_Bounds, this.m_Line, out float2 _);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, RouteSearchItem item)
        {
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Line, out float2 _))
            return;
          this.m_RouteList.Add(new RaycastJobs.RouteItem()
          {
            m_Entity = item.m_Entity,
            m_Element = item.m_Element,
            m_RaycastIndex = this.m_RaycastIndex
          });
        }
      }
    }

    public struct RouteItem
    {
      public Entity m_Entity;
      public int m_Element;
      public int m_RaycastIndex;
    }

    [BurstCompile]
    public struct RaycastRoutesJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<RaycastInput> m_Input;
      [ReadOnly]
      public NativeArray<RaycastJobs.RouteItem> m_Routes;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<RouteData> m_PrefabRouteData;
      [ReadOnly]
      public ComponentLookup<TransportLineData> m_PrefabTransportLineData;
      [ReadOnly]
      public ComponentLookup<Waypoint> m_WaypointData;
      [ReadOnly]
      public ComponentLookup<Segment> m_SegmentData;
      [ReadOnly]
      public ComponentLookup<Position> m_PositionData;
      [ReadOnly]
      public ComponentLookup<HiddenRoute> m_HiddenRouteData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public BufferLookup<CurveElement> m_CurveElements;
      [NativeDisableContainerSafetyRestriction]
      public NativeAccumulator<RaycastResult>.ParallelWriter m_Results;

      public void Execute(int index)
      {
        RaycastJobs.RouteItem route = this.m_Routes[index];
        RaycastInput raycastInput = this.m_Input[route.m_RaycastIndex];
        Waypoint componentData1;
        if ((raycastInput.m_TypeMask & TypeMask.RouteWaypoints) != TypeMask.None && this.m_WaypointData.TryGetComponent(route.m_Entity, out componentData1))
        {
          PrefabRef prefabRef = this.m_PrefabRefData[route.m_Entity];
          Position position = this.m_PositionData[route.m_Entity];
          Owner owner = this.m_OwnerData[route.m_Entity];
          if (this.m_HiddenRouteData.HasComponent(owner.m_Owner))
            return;
          RouteData routeData = this.m_PrefabRouteData[prefabRef.m_Prefab];
          TransportLineData componentData2;
          if (raycastInput.m_RouteType != RouteType.None && raycastInput.m_RouteType != routeData.m_Type || this.m_PrefabTransportLineData.TryGetComponent(prefabRef.m_Prefab, out componentData2) && (raycastInput.m_TransportType != TransportType.None && raycastInput.m_TransportType != componentData2.m_TransportType || (raycastInput.m_Flags & (RaycastFlags.Cargo | RaycastFlags.Passenger)) != (RaycastFlags) 0 && ((raycastInput.m_Flags & RaycastFlags.Passenger) == (RaycastFlags) 0 || !componentData2.m_PassengerTransport) && ((raycastInput.m_Flags & RaycastFlags.Cargo) == (RaycastFlags) 0 || !componentData2.m_CargoTransport)))
            return;
          float t;
          float num = MathUtils.Distance(raycastInput.m_Line, position.m_Position, out t) - routeData.m_SnapDistance;
          if ((double) num < 0.0)
          {
            RaycastResult raycastResult = new RaycastResult()
            {
              m_Owner = owner.m_Owner
            };
            raycastResult.m_Hit.m_HitEntity = raycastResult.m_Owner;
            raycastResult.m_Hit.m_Position = position.m_Position;
            raycastResult.m_Hit.m_HitPosition = MathUtils.Position(raycastInput.m_Line, t);
            raycastResult.m_Hit.m_NormalizedDistance = t;
            raycastResult.m_Hit.m_CellIndex = new int2(componentData1.m_Index, -1);
            raycastResult.m_Hit.m_NormalizedDistance -= 100f / math.max(1f, MathUtils.Length(raycastInput.m_Line));
            raycastResult.m_Hit.m_NormalizedDistance += num * 1E-06f / math.max(1f, routeData.m_SnapDistance);
            this.m_Results.Accumulate(route.m_RaycastIndex, raycastResult);
          }
        }
        Segment componentData3;
        if ((raycastInput.m_TypeMask & TypeMask.RouteSegments) == TypeMask.None || !this.m_SegmentData.TryGetComponent(route.m_Entity, out componentData3))
          return;
        PrefabRef prefabRef1 = this.m_PrefabRefData[route.m_Entity];
        DynamicBuffer<CurveElement> curveElement1 = this.m_CurveElements[route.m_Entity];
        Owner owner1 = this.m_OwnerData[route.m_Entity];
        if (this.m_HiddenRouteData.HasComponent(owner1.m_Owner))
          return;
        RouteData routeData1 = this.m_PrefabRouteData[prefabRef1.m_Prefab];
        TransportLineData componentData4;
        if (raycastInput.m_RouteType != RouteType.None && raycastInput.m_RouteType != routeData1.m_Type || this.m_PrefabTransportLineData.TryGetComponent(prefabRef1.m_Prefab, out componentData4) && (raycastInput.m_TransportType != TransportType.None && raycastInput.m_TransportType != componentData4.m_TransportType || (raycastInput.m_Flags & (RaycastFlags.Cargo | RaycastFlags.Passenger)) != (RaycastFlags) 0 && ((raycastInput.m_Flags & RaycastFlags.Passenger) == (RaycastFlags) 0 || !componentData4.m_PassengerTransport) && ((raycastInput.m_Flags & RaycastFlags.Cargo) == (RaycastFlags) 0 || !componentData4.m_CargoTransport)) || curveElement1.Length <= route.m_Element)
          return;
        CurveElement curveElement2 = curveElement1[route.m_Element];
        float2 t1;
        float num1 = MathUtils.Distance(curveElement2.m_Curve, raycastInput.m_Line, out t1) - routeData1.m_SnapDistance * 0.5f;
        if ((double) num1 >= 0.0)
          return;
        RaycastResult raycastResult1 = new RaycastResult()
        {
          m_Owner = owner1.m_Owner
        };
        raycastResult1.m_Hit.m_HitEntity = raycastResult1.m_Owner;
        raycastResult1.m_Hit.m_Position = MathUtils.Position(curveElement2.m_Curve, t1.x);
        raycastResult1.m_Hit.m_HitPosition = MathUtils.Position(raycastInput.m_Line, t1.y);
        raycastResult1.m_Hit.m_NormalizedDistance = t1.y;
        raycastResult1.m_Hit.m_CellIndex = new int2(-1, componentData3.m_Index);
        raycastResult1.m_Hit.m_NormalizedDistance -= 100f / math.max(1f, MathUtils.Length(raycastInput.m_Line));
        raycastResult1.m_Hit.m_NormalizedDistance += num1 * 1E-06f / math.max(1f, routeData1.m_SnapDistance);
        this.m_Results.Accumulate(route.m_RaycastIndex, raycastResult1);
      }
    }
  }
}
