// Decompiled with JetBrains decompiler
// Type: Game.Net.RaycastJobs
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Buildings;
using Game.Common;
using Game.Objects;
using Game.Prefabs;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  public static class RaycastJobs
  {
    public static float GetMinLaneRadius(float fovTan, float cameraDistance)
    {
      return (float) ((double) cameraDistance * (double) fovTan * 0.0099999997764825821);
    }

    [BurstCompile]
    public struct RaycastEdgesJob : IJobParallelForDefer
    {
      [ReadOnly]
      public float m_FovTan;
      [ReadOnly]
      public NativeArray<RaycastInput> m_Input;
      [ReadOnly]
      public NativeArray<RaycastSystem.EntityResult> m_Edges;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<Orphan> m_OrphanData;
      [ReadOnly]
      public ComponentLookup<NodeGeometry> m_NodeGeometryData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<Placeholder> m_PlaceholderData;
      [ReadOnly]
      public ComponentLookup<Attachment> m_AttachmentData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.ServiceUpgrade> m_ServiceUpgradeData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetData> m_PrefabNetData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [NativeDisableContainerSafetyRestriction]
      public NativeAccumulator<RaycastResult>.ParallelWriter m_Results;

      public void Execute(int index)
      {
        // ISSUE: variable of a compiler-generated type
        RaycastSystem.EntityResult edge = this.m_Edges[index];
        // ISSUE: reference to a compiler-generated field
        RaycastInput input = this.m_Input[edge.m_RaycastIndex];
        if ((input.m_TypeMask & TypeMask.Net) == TypeMask.None)
          return;
        // ISSUE: reference to a compiler-generated field
        if (this.m_EdgeData.HasComponent(edge.m_Entity))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.CheckEdge(edge.m_Entity, edge.m_RaycastIndex, input);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_NodeData.HasComponent(edge.m_Entity))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.CheckNode(edge.m_Entity, edge.m_RaycastIndex, input);
        }
      }

      private void CheckNode(Entity entity, int raycastIndex, RaycastInput input)
      {
        if (this.m_NodeGeometryData.HasComponent(entity))
        {
          if (!MathUtils.Intersect(this.m_NodeGeometryData[entity].m_Bounds, input.m_Line, out float2 _) || !this.m_OrphanData.HasComponent(entity))
            return;
          Node node = this.m_NodeData[entity];
          NetCompositionData compositionData = this.m_PrefabCompositionData[this.m_OrphanData[entity].m_Composition];
          if ((compositionData.m_State & CompositionState.Marker) != (CompositionState) 0)
          {
            if ((input.m_Flags & RaycastFlags.Markers) == (RaycastFlags) 0)
              return;
          }
          else if ((NetUtils.GetCollisionMask(compositionData, true) & input.m_CollisionMask) == (CollisionMask) 0)
            return;
          float3 position = node.m_Position;
          if ((input.m_Flags & RaycastFlags.ElevateOffset) != (RaycastFlags) 0)
          {
            float maxElevation = -input.m_Offset.y - compositionData.m_SurfaceHeight.max;
            this.SetElevationOffset(ref position, entity, maxElevation);
          }
          Line3.Segment line = input.m_Line + input.m_Offset;
          if ((input.m_Flags & RaycastFlags.ElevateOffset) == (RaycastFlags) 0)
            position.y += compositionData.m_SurfaceHeight.max;
          float t;
          if (!MathUtils.Intersect(line.y, position.y, out t))
            return;
          float3 float3 = MathUtils.Position(line, t);
          if ((double) math.distance(float3.xz, position.xz) > (double) compositionData.m_Width * 0.5)
            return;
          RaycastResult result = new RaycastResult();
          result.m_Owner = entity;
          result.m_Hit.m_HitEntity = entity;
          result.m_Hit.m_Position = node.m_Position;
          result.m_Hit.m_HitPosition = float3;
          result.m_Hit.m_NormalizedDistance = t + 0.5f / math.max(1f, MathUtils.Length(line));
          if (!this.ValidateResult(input, ref result))
            return;
          this.m_Results.Accumulate(raycastIndex, result);
        }
        else
        {
          if ((input.m_Flags & RaycastFlags.Markers) == (RaycastFlags) 0)
            return;
          Node node = this.m_NodeData[entity];
          float3 position = node.m_Position;
          if ((input.m_Flags & RaycastFlags.ElevateOffset) != (RaycastFlags) 0)
          {
            float maxElevation = -input.m_Offset.y;
            this.SetElevationOffset(ref position, entity, maxElevation);
          }
          Line3.Segment line = input.m_Line + input.m_Offset;
          float t;
          float num = MathUtils.Distance(line, position, out t);
          if ((double) num >= 1.0)
            return;
          RaycastResult result = new RaycastResult();
          result.m_Owner = entity;
          result.m_Hit.m_HitEntity = entity;
          result.m_Hit.m_Position = node.m_Position;
          result.m_Hit.m_HitPosition = MathUtils.Position(line, t);
          result.m_Hit.m_NormalizedDistance = t - (1f - num) / math.max(1f, MathUtils.Length(line));
          if (!this.ValidateResult(input, ref result))
            return;
          this.m_Results.Accumulate(raycastIndex, result);
        }
      }

      private void CheckEdge(Entity entity, int raycastIndex, RaycastInput input)
      {
        if (this.m_EdgeGeometryData.HasComponent(entity))
        {
          EdgeGeometry geometry1 = this.m_EdgeGeometryData[entity];
          EdgeNodeGeometry geometry2 = this.m_StartNodeGeometryData[entity].m_Geometry;
          EdgeNodeGeometry geometry3 = this.m_EndNodeGeometryData[entity].m_Geometry;
          geometry1.m_Bounds |= geometry1.m_Bounds - input.m_Offset;
          geometry2.m_Bounds |= geometry2.m_Bounds - input.m_Offset;
          geometry3.m_Bounds |= geometry3.m_Bounds - input.m_Offset;
          bool3 x;
          float2 t;
          x.x = MathUtils.Intersect(geometry1.m_Bounds, input.m_Line, out t);
          x.y = MathUtils.Intersect(geometry2.m_Bounds, input.m_Line, out t);
          x.z = MathUtils.Intersect(geometry3.m_Bounds, input.m_Line, out t);
          if (!math.any(x))
            return;
          Composition composition = this.m_CompositionData[entity];
          Edge edge = this.m_EdgeData[entity];
          Curve curve = this.m_CurveData[entity];
          if (x.x)
          {
            NetCompositionData netCompositionData = this.m_PrefabCompositionData[composition.m_Edge];
            if ((netCompositionData.m_State & CompositionState.Marker) == (CompositionState) 0 ? (NetUtils.GetCollisionMask(netCompositionData, true) & input.m_CollisionMask) != 0 : (input.m_Flags & RaycastFlags.Markers) > (RaycastFlags) 0)
            {
              if ((input.m_Flags & RaycastFlags.ElevateOffset) != (RaycastFlags) 0)
              {
                float maxElevation = -input.m_Offset.y - netCompositionData.m_SurfaceHeight.max;
                this.SetElevationOffset(ref geometry1, entity, edge.m_Start, edge.m_End, maxElevation);
              }
              this.CheckSegment(input, raycastIndex, entity, entity, geometry1.m_Start, curve.m_Bezier, netCompositionData);
              this.CheckSegment(input, raycastIndex, entity, entity, geometry1.m_End, curve.m_Bezier, netCompositionData);
            }
          }
          if (x.y)
          {
            NetCompositionData netCompositionData = this.m_PrefabCompositionData[composition.m_StartNode];
            if ((netCompositionData.m_State & CompositionState.Marker) == (CompositionState) 0 ? (NetUtils.GetCollisionMask(netCompositionData, true) & input.m_CollisionMask) != 0 : (input.m_Flags & RaycastFlags.Markers) > (RaycastFlags) 0)
            {
              if ((input.m_Flags & RaycastFlags.ElevateOffset) != (RaycastFlags) 0)
              {
                float maxElevation = -input.m_Offset.y - netCompositionData.m_SurfaceHeight.max;
                this.SetElevationOffset(ref geometry2, edge.m_Start, maxElevation);
              }
              if ((double) geometry2.m_MiddleRadius > 0.0)
              {
                this.CheckSegment(input, raycastIndex, edge.m_Start, entity, geometry2.m_Left, curve.m_Bezier, netCompositionData);
                Segment right1 = geometry2.m_Right;
                Segment right2 = geometry2.m_Right;
                right1.m_Right = MathUtils.Lerp(geometry2.m_Right.m_Left, geometry2.m_Right.m_Right, 0.5f);
                right2.m_Left = MathUtils.Lerp(geometry2.m_Right.m_Left, geometry2.m_Right.m_Right, 0.5f);
                right1.m_Right.d = geometry2.m_Middle.d;
                right2.m_Left.d = geometry2.m_Middle.d;
                this.CheckSegment(input, raycastIndex, edge.m_Start, entity, right1, curve.m_Bezier, netCompositionData);
                this.CheckSegment(input, raycastIndex, edge.m_Start, entity, right2, curve.m_Bezier, netCompositionData);
              }
              else
              {
                Segment left = geometry2.m_Left;
                Segment right = geometry2.m_Right;
                this.CheckSegment(input, raycastIndex, edge.m_Start, entity, left, curve.m_Bezier, netCompositionData);
                this.CheckSegment(input, raycastIndex, edge.m_Start, entity, right, curve.m_Bezier, netCompositionData);
                left.m_Right = geometry2.m_Middle;
                right.m_Left = geometry2.m_Middle;
                this.CheckSegment(input, raycastIndex, edge.m_Start, entity, left, curve.m_Bezier, netCompositionData);
                this.CheckSegment(input, raycastIndex, edge.m_Start, entity, right, curve.m_Bezier, netCompositionData);
              }
            }
          }
          if (!x.z)
            return;
          NetCompositionData netCompositionData1 = this.m_PrefabCompositionData[composition.m_EndNode];
          if (!((netCompositionData1.m_State & CompositionState.Marker) == (CompositionState) 0 ? (NetUtils.GetCollisionMask(netCompositionData1, true) & input.m_CollisionMask) != 0 : (input.m_Flags & RaycastFlags.Markers) > (RaycastFlags) 0))
            return;
          if ((input.m_Flags & RaycastFlags.ElevateOffset) != (RaycastFlags) 0)
          {
            float maxElevation = -input.m_Offset.y - netCompositionData1.m_SurfaceHeight.max;
            this.SetElevationOffset(ref geometry3, edge.m_End, maxElevation);
          }
          if ((double) geometry3.m_MiddleRadius > 0.0)
          {
            this.CheckSegment(input, raycastIndex, edge.m_End, entity, geometry3.m_Left, curve.m_Bezier, netCompositionData1);
            Segment right3 = geometry3.m_Right;
            Segment right4 = geometry3.m_Right;
            right3.m_Right = MathUtils.Lerp(geometry3.m_Right.m_Left, geometry3.m_Right.m_Right, 0.5f);
            right3.m_Right.d = geometry3.m_Middle.d;
            right4.m_Left = right3.m_Right;
            this.CheckSegment(input, raycastIndex, edge.m_End, entity, right3, curve.m_Bezier, netCompositionData1);
            this.CheckSegment(input, raycastIndex, edge.m_End, entity, right4, curve.m_Bezier, netCompositionData1);
          }
          else
          {
            Segment left = geometry3.m_Left;
            Segment right = geometry3.m_Right;
            this.CheckSegment(input, raycastIndex, edge.m_End, entity, left, curve.m_Bezier, netCompositionData1);
            this.CheckSegment(input, raycastIndex, edge.m_End, entity, right, curve.m_Bezier, netCompositionData1);
            left.m_Right = geometry3.m_Middle;
            right.m_Left = geometry3.m_Middle;
            this.CheckSegment(input, raycastIndex, edge.m_End, entity, left, curve.m_Bezier, netCompositionData1);
            this.CheckSegment(input, raycastIndex, edge.m_End, entity, right, curve.m_Bezier, netCompositionData1);
          }
        }
        else
        {
          if ((input.m_Flags & RaycastFlags.Markers) == (RaycastFlags) 0)
            return;
          Edge edge = this.m_EdgeData[entity];
          Curve curve = this.m_CurveData[entity];
          Bezier4x3 bezier = curve.m_Bezier;
          if ((input.m_Flags & RaycastFlags.ElevateOffset) != (RaycastFlags) 0)
          {
            float maxElevation = -input.m_Offset.y;
            this.SetElevationOffset(ref bezier, entity, edge.m_Start, edge.m_End, maxElevation);
          }
          Line3.Segment line = input.m_Line + input.m_Offset;
          float2 t;
          float num = MathUtils.Distance(bezier, line, out t);
          if ((double) num >= 0.5)
            return;
          RaycastResult result = new RaycastResult();
          result.m_Owner = entity;
          result.m_Hit.m_HitEntity = entity;
          result.m_Hit.m_Position = MathUtils.Position(curve.m_Bezier, t.x);
          result.m_Hit.m_HitPosition = MathUtils.Position(line, t.y);
          result.m_Hit.m_NormalizedDistance = t.y - (0.5f - num) / math.max(1f, MathUtils.Length(line));
          result.m_Hit.m_CurvePosition = t.x;
          if (!this.ValidateResult(input, ref result))
            return;
          this.m_Results.Accumulate(raycastIndex, result);
        }
      }

      private void SetElevationOffset(ref float3 position, Entity node, float maxElevation)
      {
        if (!this.m_ElevationData.HasComponent(node))
          return;
        Elevation elevation = this.m_ElevationData[node];
        float x = math.lerp(elevation.m_Elevation.x, elevation.m_Elevation.y, 0.5f);
        position.y -= math.min(x, maxElevation);
      }

      private void SetElevationOffset(
        ref Bezier4x3 curve,
        Entity edge,
        Entity startNode,
        Entity endNode,
        float maxElevation)
      {
        float3 float3 = new float3();
        if (this.m_ElevationData.HasComponent(startNode))
        {
          Elevation elevation = this.m_ElevationData[startNode];
          float3.x = math.lerp(elevation.m_Elevation.x, elevation.m_Elevation.y, 0.5f);
        }
        if (this.m_ElevationData.HasComponent(edge))
        {
          Elevation elevation = this.m_ElevationData[edge];
          float3.y = math.lerp(elevation.m_Elevation.x, elevation.m_Elevation.y, 0.5f);
        }
        if (this.m_ElevationData.HasComponent(endNode))
        {
          Elevation elevation = this.m_ElevationData[endNode];
          float3.z = math.lerp(elevation.m_Elevation.x, elevation.m_Elevation.y, 0.5f);
        }
        if (!math.any(float3 != 0.0f))
          return;
        this.SetElevationOffset(ref curve, float3.xy, maxElevation);
        this.SetElevationOffset(ref curve, float3.yz, maxElevation);
      }

      private void SetElevationOffset(
        ref EdgeGeometry geometry,
        Entity edge,
        Entity startNode,
        Entity endNode,
        float maxElevation)
      {
        float3 float3 = new float3();
        if (this.m_ElevationData.HasComponent(startNode))
        {
          Elevation elevation = this.m_ElevationData[startNode];
          float3.x = math.lerp(elevation.m_Elevation.x, elevation.m_Elevation.y, 0.5f);
        }
        if (this.m_ElevationData.HasComponent(edge))
        {
          Elevation elevation = this.m_ElevationData[edge];
          float3.y = math.lerp(elevation.m_Elevation.x, elevation.m_Elevation.y, 0.5f);
        }
        if (this.m_ElevationData.HasComponent(endNode))
        {
          Elevation elevation = this.m_ElevationData[endNode];
          float3.z = math.lerp(elevation.m_Elevation.x, elevation.m_Elevation.y, 0.5f);
        }
        if (!math.any(float3 != 0.0f))
          return;
        this.SetElevationOffset(ref geometry.m_Start.m_Left, float3.xy, maxElevation);
        this.SetElevationOffset(ref geometry.m_Start.m_Right, float3.xy, maxElevation);
        this.SetElevationOffset(ref geometry.m_End.m_Left, float3.yz, maxElevation);
        this.SetElevationOffset(ref geometry.m_End.m_Right, float3.yz, maxElevation);
      }

      private void SetElevationOffset(
        ref EdgeNodeGeometry geometry,
        Entity node,
        float maxElevation)
      {
        if (!this.m_ElevationData.HasComponent(node))
          return;
        Elevation elevation = this.m_ElevationData[node];
        float offset = math.lerp(elevation.m_Elevation.x, elevation.m_Elevation.y, 0.5f);
        this.SetElevationOffset(ref geometry.m_Left.m_Left, offset, maxElevation);
        this.SetElevationOffset(ref geometry.m_Left.m_Right, offset, maxElevation);
        this.SetElevationOffset(ref geometry.m_Right.m_Left, offset, maxElevation);
        this.SetElevationOffset(ref geometry.m_Right.m_Right, offset, maxElevation);
        this.SetElevationOffset(ref geometry.m_Middle, offset, maxElevation);
      }

      private void SetElevationOffset(ref Bezier4x3 curve, float offset, float maxElevation)
      {
        curve.a.y -= math.min(offset, maxElevation);
        curve.b.y -= math.min(offset, maxElevation);
        curve.c.y -= math.min(offset, maxElevation);
        curve.d.y -= math.min(offset, maxElevation);
      }

      private void SetElevationOffset(ref Bezier4x3 curve, float2 offset, float maxElevation)
      {
        curve.a.y -= math.min(offset.x, maxElevation);
        curve.b.y -= math.min(math.lerp(offset.x, offset.y, 0.333333343f), maxElevation);
        curve.c.y -= math.min(math.lerp(offset.x, offset.y, 0.6666667f), maxElevation);
        curve.d.y -= math.min(offset.y, maxElevation);
      }

      private void CheckSegment(
        RaycastInput input,
        int raycastIndex,
        Entity owner,
        Entity hitEntity,
        Segment segment,
        Bezier4x3 curve,
        NetCompositionData prefabCompositionData)
      {
        Line3.Segment line1 = input.m_Line + input.m_Offset;
        float3 _a1 = segment.m_Left.a;
        float3 float3_1 = segment.m_Right.a;
        if ((input.m_Flags & RaycastFlags.ElevateOffset) == (RaycastFlags) 0)
        {
          _a1.y += prefabCompositionData.m_SurfaceHeight.max;
          float3_1.y += prefabCompositionData.m_SurfaceHeight.max;
        }
        for (int index = 1; index <= 8; ++index)
        {
          float t1 = (float) index / 8f;
          float3 float3_2 = MathUtils.Position(segment.m_Left, t1);
          float3 _a2 = MathUtils.Position(segment.m_Right, t1);
          if ((input.m_Flags & RaycastFlags.ElevateOffset) == (RaycastFlags) 0)
          {
            float3_2.y += prefabCompositionData.m_SurfaceHeight.max;
            _a2.y += prefabCompositionData.m_SurfaceHeight.max;
          }
          Triangle3 triangle1 = new Triangle3(_a1, float3_1, float3_2);
          Triangle3 triangle2 = new Triangle3(_a2, float3_2, float3_1);
          Line3.Segment line2 = line1;
          float3 t2;
          ref float3 local = ref t2;
          if (MathUtils.Intersect(triangle1, line2, out local))
          {
            float3 float3_3 = MathUtils.Position(line1, t2.z);
            float t3;
            double num = (double) MathUtils.Distance(curve.xz, float3_3.xz, out t3);
            RaycastResult result = new RaycastResult();
            result.m_Owner = owner;
            result.m_Hit.m_HitEntity = hitEntity;
            result.m_Hit.m_Position = MathUtils.Position(curve, t3);
            result.m_Hit.m_HitPosition = float3_3;
            result.m_Hit.m_NormalizedDistance = t2.z + 0.5f / math.max(1f, MathUtils.Length(line1));
            result.m_Hit.m_CurvePosition = t3;
            if (this.ValidateResult(input, ref result))
              this.m_Results.Accumulate(raycastIndex, result);
          }
          else if (MathUtils.Intersect(triangle2, line1, out t2))
          {
            float3 float3_4 = MathUtils.Position(line1, t2.z);
            float t4;
            double num = (double) MathUtils.Distance(curve.xz, float3_4.xz, out t4);
            RaycastResult result = new RaycastResult();
            result.m_Owner = owner;
            result.m_Hit.m_HitEntity = hitEntity;
            result.m_Hit.m_Position = MathUtils.Position(curve, t4);
            result.m_Hit.m_HitPosition = float3_4;
            result.m_Hit.m_NormalizedDistance = t2.z + 0.5f / math.max(1f, MathUtils.Length(line1));
            result.m_Hit.m_CurvePosition = t4;
            if (this.ValidateResult(input, ref result))
              this.m_Results.Accumulate(raycastIndex, result);
          }
          _a1 = float3_2;
          float3_1 = _a2;
        }
      }

      private bool ValidateResult(RaycastInput input, ref RaycastResult result)
      {
        TypeMask typeMask1 = TypeMask.Net;
        Entity owner = Entity.Null;
        TypeMask typeMask2 = TypeMask.None;
        DynamicBuffer<InstalledUpgrade> bufferData;
        while (true)
        {
          if ((input.m_Flags & RaycastFlags.UpgradeIsMain) != (RaycastFlags) 0)
          {
            if (!this.m_ServiceUpgradeData.HasComponent(result.m_Owner))
            {
              if (this.m_InstalledUpgrades.TryGetBuffer(result.m_Owner, out bufferData) && bufferData.Length != 0)
                break;
            }
            else
              goto label_10;
          }
          else if ((input.m_Flags & RaycastFlags.SubBuildings) != (RaycastFlags) 0 && this.m_BuildingData.HasComponent(result.m_Owner) && this.m_ServiceUpgradeData.HasComponent(result.m_Owner))
            goto label_10;
          if (this.m_OwnerData.HasComponent(result.m_Owner))
          {
            if ((input.m_TypeMask & typeMask1) != TypeMask.None)
            {
              owner = result.m_Owner;
              typeMask2 = typeMask1;
            }
            result.m_Owner = this.m_OwnerData[result.m_Owner].m_Owner;
            typeMask1 = TypeMask.StaticObjects;
          }
          else
            goto label_10;
        }
        owner = Entity.Null;
        typeMask2 = TypeMask.None;
        typeMask1 = TypeMask.StaticObjects;
        result.m_Owner = bufferData[0].m_Upgrade;
label_10:
        if ((input.m_Flags & RaycastFlags.SubElements) != (RaycastFlags) 0 && (input.m_TypeMask & typeMask2) != TypeMask.None)
        {
          result.m_Owner = owner;
          typeMask1 = typeMask2;
        }
        else if ((input.m_Flags & RaycastFlags.NoMainElements) != (RaycastFlags) 0)
          return false;
        if ((input.m_TypeMask & typeMask1) == TypeMask.None)
          return false;
        if (typeMask1 == TypeMask.StaticObjects)
          return this.CheckPlaceholder(input, ref result.m_Owner);
        return typeMask1 != TypeMask.Net || (this.m_PrefabNetData[this.m_PrefabRefData[result.m_Owner].m_Prefab].m_ConnectLayers & input.m_NetLayerMask) > Layer.None;
      }

      private bool CheckPlaceholder(RaycastInput input, ref Entity entity)
      {
        if ((input.m_Flags & RaycastFlags.Placeholders) != (RaycastFlags) 0 || !this.m_PlaceholderData.HasComponent(entity))
          return true;
        if (this.m_AttachmentData.HasComponent(entity))
        {
          Attachment attachment = this.m_AttachmentData[entity];
          if (this.m_PrefabRefData.HasComponent(attachment.m_Attached))
          {
            entity = attachment.m_Attached;
            return true;
          }
        }
        return false;
      }
    }

    [BurstCompile]
    public struct RaycastLabelsJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<RaycastInput> m_Input;
      [ReadOnly]
      public float3 m_CameraRight;
      [ReadOnly]
      public NativeArray<RaycastSystem.EntityResult> m_Edges;
      [ReadOnly]
      public ComponentLookup<Aggregated> m_AggregatedData;
      [ReadOnly]
      public ComponentLookup<LabelExtents> m_LabelExtentsData;
      [ReadOnly]
      public BufferLookup<LabelPosition> m_LabelPositions;
      [NativeDisableContainerSafetyRestriction]
      public NativeAccumulator<RaycastResult>.ParallelWriter m_Results;

      public void Execute(int index)
      {
        // ISSUE: variable of a compiler-generated type
        RaycastSystem.EntityResult edge = this.m_Edges[index];
        // ISSUE: reference to a compiler-generated field
        RaycastInput input = this.m_Input[edge.m_RaycastIndex];
        Aggregated componentData;
        // ISSUE: reference to a compiler-generated field
        if ((input.m_TypeMask & TypeMask.Labels) == TypeMask.None || !this.m_AggregatedData.TryGetComponent(edge.m_Entity, out componentData))
          return;
        // ISSUE: reference to a compiler-generated field
        this.CheckAggregate(edge.m_RaycastIndex, input, componentData.m_Aggregate);
      }

      private void CheckAggregate(int raycastIndex, RaycastInput input, Entity aggregate)
      {
        LabelExtents componentData;
        if (!this.m_LabelExtentsData.TryGetComponent(aggregate, out componentData))
          return;
        DynamicBuffer<LabelPosition> labelPosition1 = this.m_LabelPositions[aggregate];
        for (int index1 = 0; index1 < labelPosition1.Length; ++index1)
        {
          LabelPosition labelPosition2 = labelPosition1[index1];
          if ((NetUtils.GetCollisionMask(labelPosition2) & input.m_CollisionMask) != (CollisionMask) 0)
          {
            float3 y = MathUtils.Position(labelPosition2.m_Curve, 0.5f);
            float num1 = math.max(math.sqrt(math.distance(input.m_Line.a, y) * 0.0001f), 0.01f);
            if ((double) num1 < (double) labelPosition2.m_MaxScale * 0.949999988079071)
            {
              Bounds2 bounds2 = ((double) math.dot(this.m_CameraRight, MathUtils.Tangent(labelPosition2.m_Curve, 0.5f)) < 0.0 ? new Bounds2(-componentData.m_Bounds.max, -componentData.m_Bounds.min) : componentData.m_Bounds) * (float2) num1;
              Bounds1 t1 = new Bounds1(0.0f, 1f);
              Bounds1 t2 = new Bounds1(0.0f, 1f);
              float num2 = -bounds2.min.x - labelPosition2.m_HalfLength;
              float num3 = bounds2.max.x - labelPosition2.m_HalfLength;
              if ((double) num2 < 0.0)
                MathUtils.ClampLength(labelPosition2.m_Curve, ref t1, -num2);
              else
                t1.max = 0.0f;
              if ((double) num3 < 0.0)
                MathUtils.ClampLengthInverse(labelPosition2.m_Curve, ref t2, -num3);
              else
                t2.min = 1f;
              Quad3 quad;
              if ((double) num2 > 0.0)
              {
                float3 float3_1 = math.normalizesafe(MathUtils.StartTangent(labelPosition2.m_Curve));
                float3 float3_2 = new float3(-float3_1.z, 0.0f, float3_1.x);
                quad.a = labelPosition2.m_Curve.a - float3_1 * num2 + float3_2 * bounds2.min.y;
                quad.b = labelPosition2.m_Curve.a - float3_1 * num2 + float3_2 * bounds2.max.y;
                quad.c = labelPosition2.m_Curve.a + float3_2 * bounds2.max.y;
                quad.d = labelPosition2.m_Curve.a + float3_2 * bounds2.min.y;
                float t3;
                if (MathUtils.Intersect(quad, input.m_Line, out t3))
                {
                  float num4 = MathUtils.Size(bounds2.y);
                  RaycastResult raycastResult = new RaycastResult()
                  {
                    m_Owner = aggregate
                  };
                  raycastResult.m_Hit.m_HitEntity = raycastResult.m_Owner;
                  raycastResult.m_Hit.m_Position = y;
                  raycastResult.m_Hit.m_HitPosition = MathUtils.Position(input.m_Line, t3);
                  raycastResult.m_Hit.m_NormalizedDistance = t3 - num4 / math.max(1f, MathUtils.Length(input.m_Line));
                  raycastResult.m_Hit.m_CellIndex = new int2(labelPosition2.m_ElementIndex, -1);
                  this.m_Results.Accumulate(raycastIndex, raycastResult);
                }
              }
              else
              {
                float3 float3_3 = MathUtils.Position(labelPosition2.m_Curve, t1.max);
                float3 float3_4 = math.normalizesafe(MathUtils.Tangent(labelPosition2.m_Curve, t1.max));
                float3 float3_5 = new float3(-float3_4.z, 0.0f, float3_4.x);
                quad.c = float3_3 + float3_5 * bounds2.max.y;
                quad.d = float3_3 + float3_5 * bounds2.min.y;
              }
              for (int index2 = 1; index2 <= 16; ++index2)
              {
                float t4 = math.lerp(t1.max, t2.min, (float) index2 * (1f / 16f));
                float3 float3_6 = MathUtils.Position(labelPosition2.m_Curve, t4);
                float3 float3_7 = math.normalizesafe(MathUtils.Tangent(labelPosition2.m_Curve, t4));
                float3 float3_8 = new float3(-float3_7.z, 0.0f, float3_7.x);
                quad.a = quad.d;
                quad.b = quad.c;
                quad.c = float3_6 + float3_8 * bounds2.max.y;
                quad.d = float3_6 + float3_8 * bounds2.min.y;
                float t5;
                if (MathUtils.Intersect(quad, input.m_Line, out t5))
                {
                  float num5 = MathUtils.Size(bounds2.y);
                  RaycastResult raycastResult = new RaycastResult()
                  {
                    m_Owner = aggregate
                  };
                  raycastResult.m_Hit.m_HitEntity = raycastResult.m_Owner;
                  raycastResult.m_Hit.m_Position = y;
                  raycastResult.m_Hit.m_HitPosition = MathUtils.Position(input.m_Line, t5);
                  raycastResult.m_Hit.m_NormalizedDistance = t5 - num5 / math.max(1f, MathUtils.Length(input.m_Line));
                  raycastResult.m_Hit.m_CellIndex = new int2(labelPosition2.m_ElementIndex, -1);
                  this.m_Results.Accumulate(raycastIndex, raycastResult);
                }
              }
              if ((double) num3 > 0.0)
              {
                float3 float3_9 = math.normalizesafe(MathUtils.EndTangent(labelPosition2.m_Curve));
                float3 float3_10 = new float3(-float3_9.z, 0.0f, float3_9.x);
                quad.a = quad.d;
                quad.b = quad.c;
                quad.c = labelPosition2.m_Curve.d + float3_9 * num3 + float3_10 * bounds2.min.y;
                quad.d = labelPosition2.m_Curve.d + float3_9 * num3 + float3_10 * bounds2.max.y;
                float t6;
                if (MathUtils.Intersect(quad, input.m_Line, out t6))
                {
                  float num6 = MathUtils.Size(bounds2.y);
                  RaycastResult raycastResult = new RaycastResult()
                  {
                    m_Owner = aggregate
                  };
                  raycastResult.m_Hit.m_HitEntity = raycastResult.m_Owner;
                  raycastResult.m_Hit.m_Position = y;
                  raycastResult.m_Hit.m_HitPosition = MathUtils.Position(input.m_Line, t6);
                  raycastResult.m_Hit.m_NormalizedDistance = t6 - num6 / math.max(1f, MathUtils.Length(input.m_Line));
                  raycastResult.m_Hit.m_CellIndex = new int2(labelPosition2.m_ElementIndex, -1);
                  this.m_Results.Accumulate(raycastIndex, raycastResult);
                }
              }
            }
          }
        }
      }
    }

    [BurstCompile]
    public struct RaycastLanesJob : IJobParallelForDefer
    {
      [ReadOnly]
      public float m_FovTan;
      [ReadOnly]
      public NativeArray<RaycastInput> m_Input;
      [ReadOnly]
      public NativeArray<RaycastSystem.EntityResult> m_Lanes;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<UtilityLaneData> m_PrefabUtilityLaneData;
      [ReadOnly]
      public ComponentLookup<NetLaneGeometryData> m_PrefabLaneGeometryData;
      [NativeDisableContainerSafetyRestriction]
      public NativeAccumulator<RaycastResult>.ParallelWriter m_Results;

      public void Execute(int index)
      {
        // ISSUE: variable of a compiler-generated type
        RaycastSystem.EntityResult lane = this.m_Lanes[index];
        // ISSUE: reference to a compiler-generated field
        RaycastInput raycastInput = this.m_Input[lane.m_RaycastIndex];
        if ((raycastInput.m_TypeMask & TypeMask.Lanes) == TypeMask.None)
          return;
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[lane.m_Entity];
        UtilityLaneData componentData1;
        if (!this.m_PrefabUtilityLaneData.TryGetComponent(prefabRef.m_Prefab, out componentData1) || (componentData1.m_UtilityTypes & raycastInput.m_UtilityTypeMask) == UtilityTypes.None)
          return;
        // ISSUE: reference to a compiler-generated field
        Curve curve = this.m_CurveData[lane.m_Entity];
        float2 t;
        float num = MathUtils.Distance(curve.m_Bezier, raycastInput.m_Line, out t);
        float3 x1 = MathUtils.Position(raycastInput.m_Line, t.y);
        float x2 = RaycastJobs.GetMinLaneRadius(this.m_FovTan, math.distance(x1, raycastInput.m_Line.a));
        NetLaneGeometryData componentData2;
        if (this.m_PrefabLaneGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData2))
          x2 = math.max(x2, componentData2.m_Size.x * 0.5f);
        if ((double) num >= (double) x2)
          return;
        // ISSUE: reference to a compiler-generated field
        RaycastResult raycastResult = new RaycastResult()
        {
          m_Owner = lane.m_Entity
        };
        raycastResult.m_Hit.m_HitEntity = raycastResult.m_Owner;
        raycastResult.m_Hit.m_Position = MathUtils.Position(curve.m_Bezier, t.x);
        raycastResult.m_Hit.m_HitPosition = x1;
        raycastResult.m_Hit.m_CurvePosition = t.x;
        raycastResult.m_Hit.m_NormalizedDistance = t.y - (x2 - num) / math.max(1f, MathUtils.Length(raycastInput.m_Line));
        // ISSUE: reference to a compiler-generated field
        this.m_Results.Accumulate(lane.m_RaycastIndex, raycastResult);
      }
    }
  }
}
