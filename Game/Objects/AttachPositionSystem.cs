// Decompiled with JetBrains decompiler
// Type: Game.Objects.AttachPositionSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Objects
{
  [CompilerGenerated]
  public class AttachPositionSystem : GameSystemBase
  {
    private EntityQuery m_UpdateQuery;
    private AttachPositionSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateQuery = this.GetEntityQuery(ComponentType.ReadOnly<Updated>(), ComponentType.ReadOnly<Attached>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_UpdateQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PillarData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AttachPositionSystem.AttachPositionJob jobData = new AttachPositionSystem.AttachPositionJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_AttachedType = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_UpdatedData = this.__TypeHandle.__Game_Common_Updated_RO_ComponentLookup,
        m_AttachedData = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RO_ComponentLookup,
        m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabPlaceableObjectData = this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup,
        m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_PrefabRouteConnectionData = this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup,
        m_PrefabCarLaneData = this.__TypeHandle.__Game_Prefabs_CarLaneData_RO_ComponentLookup,
        m_PrefabTrackLaneData = this.__TypeHandle.__Game_Prefabs_TrackLaneData_RO_ComponentLookup,
        m_PrefabPillarData = this.__TypeHandle.__Game_Prefabs_PillarData_RO_ComponentLookup,
        m_PrefabLaneData = this.__TypeHandle.__Game_Prefabs_NetLaneData_RO_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_NetCompositionAreas = this.__TypeHandle.__Game_Prefabs_NetCompositionArea_RO_BufferLookup,
        m_NetCompositionLanes = this.__TypeHandle.__Game_Prefabs_NetCompositionLane_RO_BufferLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RW_ComponentLookup
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<AttachPositionSystem.AttachPositionJob>(this.m_UpdateQuery, this.Dependency);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
    }

    protected override void OnCreateForCompiler()
    {
      base.OnCreateForCompiler();
      // ISSUE: reference to a compiler-generated method
      this.__AssignQueries(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.__TypeHandle.__AssignHandles(ref this.CheckedStateRef);
    }

    [UnityEngine.Scripting.Preserve]
    public AttachPositionSystem()
    {
    }

    [BurstCompile]
    private struct AttachPositionJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Attached> m_AttachedType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Updated> m_UpdatedData;
      [ReadOnly]
      public ComponentLookup<Attached> m_AttachedData;
      [ReadOnly]
      public ComponentLookup<Curve> m_CurveData;
      [ReadOnly]
      public ComponentLookup<Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> m_PrefabPlaceableObjectData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public ComponentLookup<RouteConnectionData> m_PrefabRouteConnectionData;
      [ReadOnly]
      public ComponentLookup<CarLaneData> m_PrefabCarLaneData;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> m_PrefabTrackLaneData;
      [ReadOnly]
      public ComponentLookup<PillarData> m_PrefabPillarData;
      [ReadOnly]
      public ComponentLookup<NetLaneData> m_PrefabLaneData;
      [ReadOnly]
      public BufferLookup<SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<NetCompositionArea> m_NetCompositionAreas;
      [ReadOnly]
      public BufferLookup<NetCompositionLane> m_NetCompositionLanes;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Transform> m_TransformData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Attached> nativeArray2 = chunk.GetNativeArray<Attached>(ref this.m_AttachedType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Attached attached = nativeArray2[index];
          PrefabRef prefabRef = nativeArray3[index];
          // ISSUE: reference to a compiler-generated field
          Transform transform1 = this.m_TransformData[entity];
          Transform transform2 = transform1;
          // ISSUE: reference to a compiler-generated method
          this.FixAttachedPosition(attached, prefabRef, ref transform2);
          if (!transform2.Equals(transform1))
          {
            // ISSUE: reference to a compiler-generated method
            this.MoveObject(entity, transform1, transform2);
          }
        }
      }

      private void MoveObject(Entity entity, Transform oldTransform, Transform newTransform)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_TransformData[entity] = newTransform;
        DynamicBuffer<SubObject> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.TryGetBuffer(entity, out bufferData))
          return;
        Transform inverseParentTransform = ObjectUtils.InverseTransform(oldTransform);
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Entity subObject = bufferData[index].m_SubObject;
          Owner componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerData.TryGetComponent(subObject, out componentData) && !(componentData.m_Owner != entity) && this.m_UpdatedData.HasComponent(subObject) && !this.m_AttachedData.HasComponent(subObject))
          {
            // ISSUE: reference to a compiler-generated field
            Transform transform = this.m_TransformData[subObject];
            Transform world = ObjectUtils.LocalToWorld(newTransform, ObjectUtils.WorldToLocal(inverseParentTransform, transform));
            if (!world.Equals(transform))
            {
              // ISSUE: reference to a compiler-generated method
              this.MoveObject(subObject, transform, world);
            }
          }
        }
      }

      private void FixAttachedPosition(
        Attached attached,
        PrefabRef prefabRef,
        ref Transform transform)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabPlaceableObjectData.HasComponent(prefabRef.m_Prefab) || (this.m_PrefabPlaceableObjectData[prefabRef.m_Prefab].m_Flags & PlacementFlags.CanOverlap) != PlacementFlags.None)
          return;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabPillarData.HasComponent(prefabRef.m_Prefab))
        {
          // ISSUE: reference to a compiler-generated field
          PillarData pillarData = this.m_PrefabPillarData[prefabRef.m_Prefab];
          if (pillarData.m_Type != PillarType.Vertical && pillarData.m_Type != PillarType.Base)
            return;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_NodeData.HasComponent(attached.m_Parent))
        {
          // ISSUE: reference to a compiler-generated field
          Node node = this.m_NodeData[attached.m_Parent];
          transform.m_Position = node.m_Position;
          transform.m_Rotation = node.m_Rotation;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_CompositionData.HasComponent(attached.m_Parent))
            return;
          // ISSUE: reference to a compiler-generated field
          Composition composition = this.m_CompositionData[attached.m_Parent];
          // ISSUE: reference to a compiler-generated field
          EdgeGeometry edgeGeometry = this.m_EdgeGeometryData[attached.m_Parent];
          // ISSUE: reference to a compiler-generated field
          Curve curve = this.m_CurveData[attached.m_Parent];
          // ISSUE: reference to a compiler-generated field
          if (!this.m_NetCompositionAreas.HasBuffer(composition.m_Edge))
            return;
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<NetCompositionArea> netCompositionArea = this.m_NetCompositionAreas[composition.m_Edge];
          // ISSUE: reference to a compiler-generated field
          NetCompositionData prefabCompositionData = this.m_PrefabCompositionData[composition.m_Edge];
          float num = 0.0f;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabObjectGeometryData.HasComponent(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
            if ((objectGeometryData.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
            {
              num = objectGeometryData.m_LegSize.z * 0.5f;
              if ((double) objectGeometryData.m_LegSize.y <= (double) prefabCompositionData.m_HeightRange.max)
                num = math.max(num, objectGeometryData.m_Size.z * 0.5f);
            }
            else
              num = objectGeometryData.m_Size.z * 0.5f;
          }
          Transform bestTransform1 = transform;
          Transform bestTransform2 = transform;
          float3 curvePosition = MathUtils.Position(curve.m_Bezier, attached.m_CurvePosition);
          bool snapEdge = false;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRouteConnectionData.HasComponent(prefabRef.m_Prefab) && this.m_NetCompositionLanes.HasBuffer(composition.m_Edge))
          {
            // ISSUE: reference to a compiler-generated field
            RouteConnectionData routeConnectionData = this.m_PrefabRouteConnectionData[prefabRef.m_Prefab];
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<NetCompositionLane> netCompositionLane = this.m_NetCompositionLanes[composition.m_Edge];
            if (routeConnectionData.m_RouteConnectionType != RouteConnectionType.None)
            {
              float3 curveTangent = MathUtils.Tangent(curve.m_Bezier, attached.m_CurvePosition);
              // ISSUE: reference to a compiler-generated method
              this.SnapRouteLane(transform, ref bestTransform1, ref snapEdge, curvePosition, curveTangent, routeConnectionData.m_RouteConnectionType, routeConnectionData.m_RouteTrackType, routeConnectionData.m_RouteRoadType, prefabCompositionData, netCompositionLane);
            }
          }
          if ((double) attached.m_CurvePosition < 0.5)
          {
            // ISSUE: reference to a compiler-generated method
            this.SnapSegmentAreas(bestTransform1, ref bestTransform2, snapEdge, num, curvePosition, edgeGeometry.m_Start, prefabCompositionData, netCompositionArea);
          }
          else
          {
            // ISSUE: reference to a compiler-generated method
            this.SnapSegmentAreas(bestTransform1, ref bestTransform2, snapEdge, num, curvePosition, edgeGeometry.m_End, prefabCompositionData, netCompositionArea);
          }
          transform = bestTransform2;
        }
      }

      private void SnapRouteLane(
        Transform transform,
        ref Transform bestTransform,
        ref bool snapEdge,
        float3 curvePosition,
        float3 curveTangent,
        RouteConnectionType connectionType,
        TrackTypes trackType,
        RoadTypes carType,
        NetCompositionData prefabCompositionData,
        DynamicBuffer<NetCompositionLane> lanes)
      {
        LaneFlags laneFlags;
        switch (connectionType)
        {
          case RouteConnectionType.Road:
            laneFlags = LaneFlags.Road;
            break;
          case RouteConnectionType.Pedestrian:
            laneFlags = LaneFlags.Pedestrian;
            break;
          case RouteConnectionType.Track:
            laneFlags = LaneFlags.Track;
            break;
          default:
            return;
        }
        float2 float2 = MathUtils.Right(math.normalizesafe(curveTangent.xz));
        float3 float3 = transform.m_Position - curvePosition;
        float2 y = new float2(math.dot(float2, float3.xz), float3.y);
        float num1 = float.MaxValue;
        for (int index = 0; index < lanes.Length; ++index)
        {
          NetCompositionLane lane = lanes[index];
          if ((lane.m_Flags & laneFlags) == laneFlags)
          {
            float num2 = math.distancesq(lane.m_Position.xy, y);
            if ((double) num2 < (double) num1)
            {
              switch (connectionType)
              {
                case RouteConnectionType.Road:
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (!this.m_PrefabCarLaneData.HasComponent(lane.m_Lane) || (this.m_PrefabCarLaneData[lane.m_Lane].m_RoadTypes & carType) == RoadTypes.None)
                    continue;
                  break;
                case RouteConnectionType.Track:
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (!this.m_PrefabTrackLaneData.HasComponent(lane.m_Lane) || (this.m_PrefabTrackLaneData[lane.m_Lane].m_TrackTypes & trackType) == TrackTypes.None)
                    continue;
                  break;
              }
              num1 = num2;
              float2 direction = float2;
              if ((lane.m_Flags & LaneFlags.Invert) == (LaneFlags) 0)
                direction = -float2;
              float x = lane.m_Position.x;
              if ((laneFlags & (LaneFlags.Road | LaneFlags.Track)) != (LaneFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                NetLaneData netLaneData = this.m_PrefabLaneData[lane.m_Lane];
                x += netLaneData.m_Width * math.select(0.25f, -0.25f, (double) math.dot(transform.m_Position.xz - curvePosition.xz, float2) < 0.0);
              }
              bestTransform.m_Position = curvePosition;
              bestTransform.m_Position.xz += float2 * x;
              bestTransform.m_Position.y += lane.m_Position.y;
              bestTransform.m_Rotation = ToolUtils.CalculateRotation(direction);
              snapEdge = true;
            }
          }
        }
      }

      private void SnapSegmentAreas(
        Transform transform,
        ref Transform bestTransform,
        bool snapEdge,
        float radius,
        float3 curvePosition,
        Game.Net.Segment segment,
        NetCompositionData prefabCompositionData,
        DynamicBuffer<NetCompositionArea> areas)
      {
        float num1 = float.MaxValue;
        for (int index = 0; index < areas.Length; ++index)
        {
          NetCompositionArea area = areas[index];
          if ((area.m_Flags & NetAreaFlags.Buildable) != (NetAreaFlags) 0)
          {
            float num2 = area.m_Width * 0.51f;
            if ((double) radius < (double) num2)
            {
              Bezier4x3 curve = MathUtils.Lerp(segment.m_Left, segment.m_Right, (float) ((double) area.m_Position.x / (double) prefabCompositionData.m_Width + 0.5));
              float t;
              double num3 = (double) MathUtils.Distance(curve, curvePosition, out t);
              float3 x = MathUtils.Position(curve, t);
              float num4 = math.distancesq(x, transform.m_Position);
              if ((double) num4 < (double) num1)
              {
                num1 = num4;
                float2 forward = math.normalizesafe(MathUtils.Tangent(curve, t).xz);
                float2 direction = (area.m_Flags & NetAreaFlags.Invert) == (NetAreaFlags) 0 ? MathUtils.Left(forward) : MathUtils.Right(forward);
                float3 float3 = MathUtils.Position(MathUtils.Lerp(segment.m_Left, segment.m_Right, (float) ((double) area.m_SnapPosition.x / (double) prefabCompositionData.m_Width + 0.5)), t);
                float maxLength1 = math.max(0.0f, math.min(area.m_Width * 0.5f, math.abs(area.m_SnapPosition.x - area.m_Position.x) + area.m_SnapWidth * 0.5f) - radius);
                x.xz += MathUtils.ClampLength(float3.xz - x.xz, maxLength1);
                if (snapEdge)
                {
                  float maxLength2 = math.max(0.0f, area.m_SnapWidth * 0.5f - radius);
                  x.xz += MathUtils.ClampLength(transform.m_Position.xz - x.xz, maxLength2);
                }
                x.y += area.m_Position.y;
                bestTransform.m_Position = x;
                bestTransform.m_Rotation = ToolUtils.CalculateRotation(direction);
              }
            }
          }
        }
      }

      void IJobChunk.Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated method
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Attached> __Game_Objects_Attached_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Updated> __Game_Common_Updated_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attached> __Game_Objects_Attached_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Curve> __Game_Net_Curve_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> __Game_Prefabs_PlaceableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RouteConnectionData> __Game_Prefabs_RouteConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarLaneData> __Game_Prefabs_CarLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrackLaneData> __Game_Prefabs_TrackLaneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PillarData> __Game_Prefabs_PillarData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetLaneData> __Game_Prefabs_NetLaneData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<NetCompositionArea> __Game_Prefabs_NetCompositionArea_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<NetCompositionLane> __Game_Prefabs_NetCompositionLane_RO_BufferLookup;
      public ComponentLookup<Transform> __Game_Objects_Transform_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Updated_RO_ComponentLookup = state.GetComponentLookup<Updated>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentLookup = state.GetComponentLookup<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentLookup = state.GetComponentLookup<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup = state.GetComponentLookup<PlaceableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup = state.GetComponentLookup<RouteConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarLaneData_RO_ComponentLookup = state.GetComponentLookup<CarLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TrackLaneData_RO_ComponentLookup = state.GetComponentLookup<TrackLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PillarData_RO_ComponentLookup = state.GetComponentLookup<PillarData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetLaneData_RO_ComponentLookup = state.GetComponentLookup<NetLaneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionArea_RO_BufferLookup = state.GetBufferLookup<NetCompositionArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionLane_RO_BufferLookup = state.GetBufferLookup<NetCompositionLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RW_ComponentLookup = state.GetComponentLookup<Transform>();
      }
    }
  }
}
