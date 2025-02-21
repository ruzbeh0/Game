// Decompiled with JetBrains decompiler
// Type: Game.Objects.ValidationHelpers
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Objects
{
  public static class ValidationHelpers
  {
    public const float COLLISION_TOLERANCE = 0.01f;

    public static void ValidateObject(
      Entity entity,
      Temp temp,
      Owner owner,
      Transform transform,
      PrefabRef prefabRef,
      Attached attached,
      bool isOutsideConnection,
      bool editorMode,
      ValidationSystem.EntityData data,
      NativeList<ValidationSystem.BoundsData> edgeList,
      NativeList<ValidationSystem.BoundsData> objectList,
      NativeQuadTree<Entity, QuadTreeBoundsXZ> objectSearchTree,
      NativeQuadTree<Entity, QuadTreeBoundsXZ> netSearchTree,
      NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> areaSearchTree,
      NativeParallelHashMap<Entity, int> instanceCounts,
      WaterSurfaceData waterSurfaceData,
      TerrainHeightData terrainHeightData,
      NativeQueue<ErrorData>.ParallelWriter errorQueue)
    {
      ObjectGeometryData componentData1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!data.m_PrefabObjectGeometry.TryGetComponent(prefabRef.m_Prefab, out componentData1) || (componentData1.m_Flags & GeometryFlags.IgnoreSecondaryCollision) != GeometryFlags.None && data.m_Secondary.HasComponent(entity))
        return;
      StackData componentData2 = new StackData();
      Stack componentData3;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Bounds3 bounds3 = !data.m_Stack.TryGetComponent(entity, out componentData3) || !data.m_PrefabStackData.TryGetComponent(prefabRef.m_Prefab, out componentData2) ? ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, componentData1) : ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, componentData3, componentData1, componentData2);
      PlaceableObjectData componentData4;
      // ISSUE: reference to a compiler-generated field
      data.m_PlaceableObject.TryGetComponent(prefabRef.m_Prefab, out componentData4);
      bool flag1 = false;
      if ((componentData1.m_Flags & (GeometryFlags.Overridable | GeometryFlags.DeleteOverridden)) == GeometryFlags.Overridable)
        flag1 = (temp.m_Flags & TempFlags.Essential) == (TempFlags) 0;
      Elevation componentData5;
      CollisionMask collisionMask;
      bool flag2;
      // ISSUE: reference to a compiler-generated field
      if (data.m_ObjectElevation.TryGetComponent(entity, out componentData5))
      {
        collisionMask = ObjectUtils.GetCollisionMask(componentData1, componentData5, !editorMode || owner.m_Owner != Entity.Null);
        flag2 = (componentData5.m_Flags & ElevationFlags.OnGround) != (ElevationFlags) 0 && flag1;
        Owner componentData6 = owner;
        while (flag1 && !flag2 && componentData6.m_Owner != Entity.Null)
        {
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef1 = data.m_PrefabRef[componentData6.m_Owner];
          ObjectGeometryData componentData7;
          Temp componentData8;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (data.m_PrefabObjectGeometry.TryGetComponent(prefabRef1.m_Prefab, out componentData7) && (componentData7.m_Flags & (GeometryFlags.Overridable | GeometryFlags.DeleteOverridden)) == GeometryFlags.Overridable && data.m_Temp.TryGetComponent(componentData6.m_Owner, out componentData8) && (componentData8.m_Flags & TempFlags.Essential) == (TempFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            if (!data.m_ObjectElevation.TryGetComponent(componentData6.m_Owner, out componentData5) || (componentData5.m_Flags & ElevationFlags.OnGround) != (ElevationFlags) 0)
            {
              flag2 = true;
              break;
            }
            // ISSUE: reference to a compiler-generated field
            if (!data.m_Owner.TryGetComponent(componentData6.m_Owner, out componentData6))
              break;
          }
          else
            break;
        }
      }
      else
      {
        collisionMask = ObjectUtils.GetCollisionMask(componentData1, !editorMode || owner.m_Owner != Entity.Null);
        flag2 = flag1;
      }
      Entity parent1 = Entity.Null;
      Entity entity1 = Entity.Null;
      if ((componentData4.m_Flags & PlacementFlags.RoadNode) != PlacementFlags.None)
      {
        // ISSUE: reference to a compiler-generated field
        if (data.m_Node.HasComponent(attached.m_Parent))
          parent1 = attached.m_Parent;
        // ISSUE: reference to a compiler-generated field
        if (data.m_Temp.HasComponent(attached.m_Parent))
        {
          // ISSUE: reference to a compiler-generated field
          Entity original = data.m_Temp[attached.m_Parent].m_Original;
          // ISSUE: reference to a compiler-generated field
          if (data.m_Node.HasComponent(original))
            entity1 = original;
        }
        else
        {
          entity1 = parent1;
          parent1 = Entity.Null;
        }
      }
      if (temp.m_Original == Entity.Null && (componentData4.m_Flags & PlacementFlags.Unique) != PlacementFlags.None && instanceCounts.ContainsKey(prefabRef.m_Prefab))
        errorQueue.Enqueue(new ErrorData()
        {
          m_ErrorType = ErrorType.AlreadyExists,
          m_ErrorSeverity = ErrorSeverity.Error,
          m_TempEntity = entity,
          m_Position = (float3) float.NaN
        });
      ValidationHelpers.ObjectIterator iterator1 = new ValidationHelpers.ObjectIterator();
      Entity attachedParent = new Entity();
      Edge tempNodes = new Edge();
      Edge ownerNodes = new Edge();
      if ((temp.m_Flags & TempFlags.Delete) == (TempFlags) 0)
      {
        Entity assetStamp;
        Entity owner1 = ValidationHelpers.GetOwner(entity, temp, data, out tempNodes, out ownerNodes, out attachedParent, out assetStamp);
        iterator1 = new ValidationHelpers.ObjectIterator()
        {
          m_ObjectEntity = entity,
          m_TopLevelEntity = owner1,
          m_AssetStampEntity = assetStamp,
          m_ObjectBounds = bounds3,
          m_Transform = transform,
          m_ObjectStack = componentData3,
          m_CollisionMask = collisionMask,
          m_PrefabObjectGeometryData = componentData1,
          m_ObjectStackData = componentData2,
          m_CanOverride = flag1,
          m_Optional = (temp.m_Flags & TempFlags.Optional) > (TempFlags) 0,
          m_EditorMode = editorMode,
          m_Data = data,
          m_ErrorQueue = errorQueue
        };
        objectSearchTree.Iterate<ValidationHelpers.ObjectIterator>(ref iterator1);
      }
      ValidationHelpers.NetIterator iterator2 = new ValidationHelpers.NetIterator();
      if ((temp.m_Flags & TempFlags.Delete) == (TempFlags) 0)
      {
        iterator2 = new ValidationHelpers.NetIterator()
        {
          m_ObjectEntity = entity,
          m_AttachedParent = attachedParent,
          m_TopLevelEntity = iterator1.m_TopLevelEntity,
          m_IgnoreNode = entity1,
          m_OwnerNodes = ownerNodes,
          m_ObjectBounds = bounds3,
          m_Transform = transform,
          m_ObjectStack = componentData3,
          m_CollisionMask = collisionMask,
          m_PrefabObjectGeometryData = componentData1,
          m_ObjectStackData = componentData2,
          m_Optional = flag1,
          m_EditorMode = editorMode,
          m_Data = data,
          m_ErrorQueue = errorQueue
        };
        netSearchTree.Iterate<ValidationHelpers.NetIterator>(ref iterator2);
      }
      ValidationHelpers.AreaIterator iterator3 = new ValidationHelpers.AreaIterator()
      {
        m_ObjectEntity = entity,
        m_ObjectBounds = bounds3,
        m_IgnoreCollisions = (temp.m_Flags & TempFlags.Delete) > (TempFlags) 0,
        m_IgnoreProtectedAreas = (temp.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Modify | TempFlags.Replace | TempFlags.Upgrade)) == (TempFlags) 0,
        m_Optional = flag1,
        m_EditorMode = editorMode,
        m_TransformData = transform,
        m_CollisionMask = collisionMask,
        m_PrefabObjectGeometryData = componentData1,
        m_Data = data,
        m_ErrorQueue = errorQueue
      };
      areaSearchTree.Iterate<ValidationHelpers.AreaIterator>(ref iterator3);
      if ((temp.m_Flags & TempFlags.Delete) == (TempFlags) 0 && (edgeList.Length != 0 || objectList.Length != 0))
      {
        Entity entity2 = entity;
        Entity entity3 = Entity.Null;
        Entity parent2 = Entity.Null;
        // ISSUE: reference to a compiler-generated field
        if (owner.m_Owner != Entity.Null && !data.m_Building.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          if (data.m_AssetStamp.HasComponent(owner.m_Owner))
          {
            entity3 = owner.m_Owner;
          }
          else
          {
            Attached componentData9;
            // ISSUE: reference to a compiler-generated field
            if (data.m_Attached.TryGetComponent(owner.m_Owner, out componentData9))
              parent2 = componentData9.m_Parent;
            Owner componentData10;
            Entity owner2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            for (entity2 = owner.m_Owner; data.m_Owner.TryGetComponent(entity2, out componentData10) && !data.m_Building.HasComponent(entity2); entity2 = owner2)
            {
              owner2 = componentData10.m_Owner;
              // ISSUE: reference to a compiler-generated field
              if (data.m_AssetStamp.HasComponent(owner2))
              {
                entity3 = owner2;
                break;
              }
              // ISSUE: reference to a compiler-generated field
              if (data.m_Attached.TryGetComponent(componentData10.m_Owner, out componentData9))
                parent2 = componentData9.m_Parent;
            }
          }
        }
        DynamicBuffer<ConnectedEdge> dynamicBuffer1 = new DynamicBuffer<ConnectedEdge>();
        DynamicBuffer<ConnectedNode> dynamicBuffer2 = new DynamicBuffer<ConnectedNode>();
        Edge edge1 = new Edge();
        // ISSUE: reference to a compiler-generated field
        if (data.m_ConnectedEdges.HasBuffer(entity2))
        {
          // ISSUE: reference to a compiler-generated field
          dynamicBuffer1 = data.m_ConnectedEdges[entity2];
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (data.m_ConnectedNodes.HasBuffer(entity2))
          {
            // ISSUE: reference to a compiler-generated field
            dynamicBuffer2 = data.m_ConnectedNodes[entity2];
            // ISSUE: reference to a compiler-generated field
            edge1 = data.m_Edge[entity2];
          }
        }
        iterator2.m_TopLevelEntity = entity2;
        if (edgeList.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float3 float3 = edgeList[edgeList.Length - 1].m_Bounds.max - edgeList[0].m_Bounds.min;
          bool flag3 = (double) float3.z > (double) float3.x;
label_78:
          for (int index = 0; index < edgeList.Length; ++index)
          {
            // ISSUE: variable of a compiler-generated type
            ValidationSystem.BoundsData edge2 = edgeList[index];
            // ISSUE: reference to a compiler-generated field
            bool2 bool2 = edge2.m_Bounds.min.xz > bounds3.max.xz;
            if ((flag3 ? (bool2.y ? 1 : 0) : (bool2.x ? 1 : 0)) == 0)
            {
              if ((collisionMask & CollisionMask.OnGround) != (CollisionMask) 0)
              {
                // ISSUE: reference to a compiler-generated field
                if (!MathUtils.Intersect(bounds3.xz, edge2.m_Bounds.xz))
                  continue;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (!MathUtils.Intersect(bounds3, edge2.m_Bounds))
                  continue;
              }
              // ISSUE: reference to a compiler-generated field
              Entity entity4 = edge2.m_Entity;
              Owner componentData11;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (data.m_Owner.TryGetComponent(edge2.m_Entity, out componentData11))
              {
                Entity owner3 = componentData11.m_Owner;
                Entity owner4;
                // ISSUE: reference to a compiler-generated field
                if (data.m_AssetStamp.HasComponent(owner3))
                {
                  if (owner3 == entity)
                    continue;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  for (entity4 = owner3; data.m_Owner.HasComponent(entity4) && !data.m_Building.HasComponent(entity4); entity4 = owner4)
                  {
                    // ISSUE: reference to a compiler-generated field
                    owner4 = data.m_Owner[entity4].m_Owner;
                    // ISSUE: reference to a compiler-generated field
                    if (data.m_AssetStamp.HasComponent(owner4))
                    {
                      if (!(owner4 == entity))
                        break;
                      goto label_78;
                    }
                  }
                }
              }
              if (!(entity2 == entity4))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Edge edgeData2 = data.m_Edge[edge2.m_Entity];
                // ISSUE: reference to a compiler-generated field
                if (!(edge2.m_Entity == parent2) && !(edgeData2.m_Start == parent2) && !(edgeData2.m_End == parent2))
                {
                  if (owner.m_Owner != Entity.Null)
                  {
                    Entity owner5 = owner.m_Owner;
                    // ISSUE: reference to a compiler-generated field
                    while (data.m_Owner.HasComponent(owner5) && owner5 != edgeData2.m_Start && owner5 != edgeData2.m_End)
                    {
                      // ISSUE: reference to a compiler-generated field
                      owner5 = data.m_Owner[owner5].m_Owner;
                    }
                    if (owner5 == edgeData2.m_Start || owner5 == edgeData2.m_End)
                      continue;
                  }
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  EdgeGeometry edgeGeometryData2 = data.m_EdgeGeometry[edge2.m_Entity];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  StartNodeGeometry startNodeGeometryData2 = data.m_StartNodeGeometry[edge2.m_Entity];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  EndNodeGeometry endNodeGeometryData2 = data.m_EndNodeGeometry[edge2.m_Entity];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  Composition compositionData2 = data.m_Composition[edge2.m_Entity];
                  Entity entity5 = edgeData2.m_Start;
                  Entity entity6 = edgeData2.m_End;
                  Entity owner6;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  for (; data.m_Owner.HasComponent(entity5) && !data.m_Building.HasComponent(entity5); entity5 = owner6)
                  {
                    // ISSUE: reference to a compiler-generated field
                    owner6 = data.m_Owner[entity5].m_Owner;
                    // ISSUE: reference to a compiler-generated field
                    if (data.m_AssetStamp.HasComponent(owner6))
                    {
                      if (!(owner6 == entity))
                        break;
                      goto label_78;
                    }
                  }
                  Entity owner7;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  for (; data.m_Owner.HasComponent(entity6) && !data.m_Building.HasComponent(entity6); entity6 = owner7)
                  {
                    // ISSUE: reference to a compiler-generated field
                    owner7 = data.m_Owner[entity6].m_Owner;
                    // ISSUE: reference to a compiler-generated field
                    if (data.m_AssetStamp.HasComponent(owner7))
                    {
                      if (!(owner7 == entity))
                        break;
                      goto label_78;
                    }
                  }
                  bool checkStartNode = entity5 != entity2 && edgeData2.m_Start != tempNodes.m_Start && edgeData2.m_Start != tempNodes.m_End;
                  bool checkEndNode = entity6 != entity2 && edgeData2.m_End != tempNodes.m_Start && edgeData2.m_End != tempNodes.m_End;
                  if (checkStartNode && edgeData2.m_Start == parent1)
                  {
                    // ISSUE: reference to a compiler-generated field
                    NetCompositionData netCompositionData = data.m_PrefabComposition[compositionData2.m_StartNode];
                    checkStartNode &= (netCompositionData.m_Flags.m_General & CompositionFlags.General.Roundabout) == (CompositionFlags.General) 0;
                  }
                  if (checkEndNode && edgeData2.m_End == parent1)
                  {
                    // ISSUE: reference to a compiler-generated field
                    NetCompositionData netCompositionData = data.m_PrefabComposition[compositionData2.m_EndNode];
                    checkEndNode &= (netCompositionData.m_Flags.m_General & CompositionFlags.General.Roundabout) == (CompositionFlags.General) 0;
                  }
                  edgeData2.m_Start = entity5;
                  edgeData2.m_End = entity6;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  Temp temp1 = data.m_Temp[edge2.m_Entity];
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  iterator2.CheckOverlap(entity4, edge2.m_Entity, edge2.m_Bounds, edgeData2, compositionData2, edgeGeometryData2, startNodeGeometryData2, endNodeGeometryData2, transform.m_Position, checkStartNode, checkEndNode, (temp1.m_Flags & TempFlags.Essential) > (TempFlags) 0, componentData11.m_Owner != Entity.Null);
                }
              }
            }
            else
              break;
          }
        }
        if (objectList.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float3 float3 = objectList[objectList.Length - 1].m_Bounds.max - objectList[0].m_Bounds.min;
          bool flag4 = (double) float3.z > (double) float3.x;
          int num1 = 0;
          int num2 = objectList.Length;
          while (num1 < num2)
          {
            int index = num1 + num2 >> 1;
            // ISSUE: variable of a compiler-generated type
            ValidationSystem.BoundsData boundsData = objectList[index];
            // ISSUE: reference to a compiler-generated field
            bool2 bool2 = boundsData.m_Bounds.min.xz < bounds3.min.xz;
            if ((flag4 ? (bool2.y ? 1 : 0) : (bool2.x ? 1 : 0)) != 0)
              num1 = index + 1;
            else
              num2 = index;
          }
label_115:
          for (int index1 = num1; index1 < objectList.Length; ++index1)
          {
            // ISSUE: variable of a compiler-generated type
            ValidationSystem.BoundsData boundsData = objectList[index1];
            // ISSUE: reference to a compiler-generated field
            bool2 bool2 = boundsData.m_Bounds.min.xz > bounds3.max.xz;
            if ((flag4 ? (bool2.y ? 1 : 0) : (bool2.x ? 1 : 0)) == 0)
            {
              if ((collisionMask & CollisionMask.OnGround) != (CollisionMask) 0)
              {
                // ISSUE: reference to a compiler-generated field
                if (!MathUtils.Intersect(bounds3.xz, boundsData.m_Bounds.xz))
                  continue;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (!MathUtils.Intersect(bounds3, boundsData.m_Bounds))
                  continue;
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!(boundsData.m_Entity == entity) && !(boundsData.m_Entity == entity3) && ((double) boundsData.m_Bounds.min.x != (double) bounds3.min.x || boundsData.m_Entity.Index >= entity.Index))
              {
                // ISSUE: reference to a compiler-generated field
                Entity entity7 = boundsData.m_Entity;
                Owner componentData12;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (data.m_Owner.TryGetComponent(boundsData.m_Entity, out componentData12) && !data.m_Building.HasComponent(entity7))
                {
                  Entity owner8 = componentData12.m_Owner;
                  Entity owner9;
                  // ISSUE: reference to a compiler-generated field
                  if (data.m_AssetStamp.HasComponent(owner8))
                  {
                    if (owner8 == entity)
                      continue;
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    for (entity7 = owner8; data.m_Owner.HasComponent(entity7) && !data.m_Building.HasComponent(entity7); entity7 = owner9)
                    {
                      // ISSUE: reference to a compiler-generated field
                      owner9 = data.m_Owner[entity7].m_Owner;
                      // ISSUE: reference to a compiler-generated field
                      if (data.m_AssetStamp.HasComponent(owner9))
                      {
                        if (!(owner9 == entity))
                          break;
                        goto label_115;
                      }
                    }
                  }
                }
                if (!(entity2 == entity7))
                {
                  if (dynamicBuffer1.IsCreated)
                  {
                    for (int index2 = 0; index2 < dynamicBuffer1.Length; ++index2)
                    {
                      if (dynamicBuffer1[index2].m_Edge == entity7)
                        goto label_115;
                    }
                  }
                  else if (dynamicBuffer2.IsCreated)
                  {
                    for (int index3 = 0; index3 < dynamicBuffer2.Length; ++index3)
                    {
                      if (dynamicBuffer2[index3].m_Node == entity7)
                        goto label_115;
                    }
                    if (edge1.m_Start == entity7 || edge1.m_End == entity7)
                      continue;
                  }
                  Attached componentData13;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (!(attached.m_Parent == boundsData.m_Entity) && (!data.m_Attached.TryGetComponent(boundsData.m_Entity, out componentData13) || !(componentData13.m_Parent == entity)))
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    Temp temp2 = data.m_Temp[boundsData.m_Entity];
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    iterator1.CheckOverlap(entity7, boundsData.m_Entity, boundsData.m_Bounds, (temp2.m_Flags & TempFlags.Essential) > (TempFlags) 0, componentData12.m_Owner != Entity.Null);
                  }
                }
              }
            }
            else
              break;
          }
        }
      }
      if ((temp.m_Flags & (TempFlags.Create | TempFlags.Modify)) != (TempFlags) 0 && componentData4.m_Flags != PlacementFlags.None && !flag2)
        ValidationHelpers.CheckSurface(entity, transform, collisionMask, componentData1, componentData4, data, waterSurfaceData, terrainHeightData, errorQueue);
      if ((temp.m_Flags & TempFlags.Essential) != (TempFlags) 0 && (temp.m_Flags & (TempFlags.Create | TempFlags.Modify)) != (TempFlags) 0 && owner.m_Owner != Entity.Null)
        ValidationHelpers.ValidateSubPlacement(entity, owner, transform, prefabRef, componentData1, data, errorQueue);
      if ((temp.m_Flags & (TempFlags.Create | TempFlags.Modify | TempFlags.Replace | TempFlags.Upgrade)) == (TempFlags) 0 || isOutsideConnection)
        return;
      ValidationHelpers.ValidateWorldBounds(entity, owner, bounds3, data, terrainHeightData, errorQueue);
    }

    public static void ValidateWorldBounds(
      Entity entity,
      Owner owner,
      Bounds3 bounds,
      ValidationSystem.EntityData data,
      TerrainHeightData terrainHeightData,
      NativeQueue<ErrorData>.ParallelWriter errorQueue)
    {
      Bounds3 bounds3 = MathUtils.Expand(TerrainUtils.GetBounds(ref terrainHeightData), (float3) 0.1f);
      if (bounds.xz.Equals(bounds.xz & bounds3.xz))
        return;
      Owner componentData;
      for (; owner.m_Owner != Entity.Null; owner = componentData)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (data.m_Node.HasComponent(owner.m_Owner) || data.m_Edge.HasComponent(owner.m_Owner))
          return;
        // ISSUE: reference to a compiler-generated field
        data.m_Owner.TryGetComponent(owner.m_Owner, out componentData);
      }
      Bounds3 bounds1 = bounds;
      bounds1.min.xz = math.select(bounds.min.xz, bounds3.max.xz, bounds3.max.xz > bounds.min.xz & bounds.min.xz >= bounds3.min.xz & bounds.max.xz > bounds3.max.xz);
      bounds1.max.xz = math.select(bounds.max.xz, bounds3.min.xz, bounds3.min.xz < bounds.max.xz & bounds.max.xz <= bounds3.max.xz & bounds.min.xz < bounds3.min.xz);
      errorQueue.Enqueue(new ErrorData()
      {
        m_Position = MathUtils.Center(bounds1),
        m_ErrorType = ErrorType.ExceedsCityLimits,
        m_ErrorSeverity = ErrorSeverity.Error,
        m_TempEntity = entity
      });
    }

    public static void ValidateSubPlacement(
      Entity entity,
      Owner owner,
      Transform transform,
      PrefabRef prefabRef,
      ObjectGeometryData prefabObjectGeometryData,
      ValidationSystem.EntityData data,
      NativeQueue<ErrorData>.ParallelWriter errorQueue)
    {
      // ISSUE: reference to a compiler-generated field
      if (!data.m_Building.HasComponent(owner.m_Owner))
        return;
      // ISSUE: reference to a compiler-generated field
      Transform transform1 = data.m_Transform[owner.m_Owner];
      // ISSUE: reference to a compiler-generated field
      PrefabRef prefabRef1 = data.m_PrefabRef[owner.m_Owner];
      // ISSUE: reference to a compiler-generated field
      Game.Prefabs.BuildingData ownerBuildingData = data.m_PrefabBuilding[prefabRef1.m_Prefab];
      // ISSUE: reference to a compiler-generated field
      if (data.m_Building.HasComponent(entity))
      {
        Game.Prefabs.BuildingData componentData1;
        ServiceUpgradeData componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!data.m_PrefabBuilding.TryGetComponent(prefabRef.m_Prefab, out componentData1) || !data.m_ServiceUpgradeData.TryGetComponent(prefabRef.m_Prefab, out componentData2) || (double) componentData2.m_MaxPlacementDistance == 0.0)
          return;
        float3 forward;
        float width;
        float length;
        float roundness;
        bool circular;
        BuildingUtils.CalculateUpgradeRangeValues(transform1.m_Rotation, ownerBuildingData, componentData1, componentData2, out forward, out width, out length, out roundness, out circular);
        float2 halfLotSize = (float2) componentData1.m_LotSize * 4f - 0.4f;
        Quad3 corners = BuildingUtils.CalculateCorners(transform.m_Position, transform.m_Rotation, halfLotSize);
        float4 float4 = new float4();
        if (ValidationHelpers.ExceedRange(transform1.m_Position, forward, width, length, roundness, circular, corners.a.xz))
          float4 += new float4(corners.a, 1f);
        if (ValidationHelpers.ExceedRange(transform1.m_Position, forward, width, length, roundness, circular, corners.b.xz))
          float4 += new float4(corners.b, 1f);
        if (ValidationHelpers.ExceedRange(transform1.m_Position, forward, width, length, roundness, circular, corners.c.xz))
          float4 += new float4(corners.c, 1f);
        if (ValidationHelpers.ExceedRange(transform1.m_Position, forward, width, length, roundness, circular, corners.d.xz))
          float4 += new float4(corners.d, 1f);
        if ((double) float4.w == 0.0)
          return;
        errorQueue.Enqueue(new ErrorData()
        {
          m_ErrorType = ErrorType.LongDistance,
          m_ErrorSeverity = ErrorSeverity.Error,
          m_TempEntity = entity,
          m_PermanentEntity = owner.m_Owner,
          m_Position = float4.xyz / float4.w
        });
      }
      else
      {
        float2 _max = (float2) ownerBuildingData.m_LotSize * 4f;
        Bounds2 bounds2 = new Bounds2(-_max, _max);
        Transform local = ObjectUtils.WorldToLocal(ObjectUtils.InverseTransform(transform1), transform);
        Bounds3 bounds = ObjectUtils.CalculateBounds(local.m_Position, local.m_Rotation, prefabObjectGeometryData);
        if (bounds.xz.Equals(bounds.xz & bounds2))
          return;
        errorQueue.Enqueue(new ErrorData()
        {
          m_ErrorType = ErrorType.ExceedsLotLimits,
          m_ErrorSeverity = ErrorSeverity.Warning,
          m_TempEntity = entity,
          m_Position = ObjectUtils.LocalToWorld(transform1, new float3()
          {
            xz = (math.select(bounds.min.xz, bounds2.max, bounds.min.xz >= bounds2.min & bounds.max.xz > bounds2.max) + math.select(bounds.max.xz, bounds2.min, bounds.max.xz <= bounds2.max & bounds.min.xz < bounds2.min)) * 0.5f,
            y = MathUtils.Center(bounds.y)
          })
        });
      }
    }

    private static bool ExceedRange(
      float3 position,
      float3 forward,
      float width,
      float length,
      float roundness,
      bool circular,
      float2 checkPosition)
    {
      float2 x = checkPosition - position.xz;
      if (!circular)
      {
        roundness -= 8f;
        x = math.max((float2) 0.0f, math.abs(new float2(math.dot(x, MathUtils.Right(forward.xz)), math.dot(x, forward.xz))) - new float2(width * 0.5f, length * 0.5f) + roundness);
      }
      return (double) math.length(x) > (double) roundness;
    }

    public static void ValidateNetObject(
      Entity entity,
      NetObject netObject,
      Transform transform,
      PrefabRef prefabRef,
      Attached attached,
      ValidationSystem.EntityData data,
      NativeQueue<ErrorData>.ParallelWriter errorQueue)
    {
      RoadTypes roadTypes = RoadTypes.None;
      NetObjectData componentData1;
      // ISSUE: reference to a compiler-generated field
      if (data.m_PrefabNetObject.TryGetComponent(prefabRef.m_Prefab, out componentData1) && componentData1.m_RequireRoad != RoadTypes.None)
      {
        roadTypes = componentData1.m_RequireRoad;
        DynamicBuffer<Game.Net.SubLane> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (data.m_Lanes.TryGetBuffer(attached.m_Parent, out bufferData))
        {
          for (int index = 0; index < bufferData.Length; ++index)
          {
            Game.Net.SubLane subLane = bufferData[index];
            // ISSUE: reference to a compiler-generated field
            if (data.m_CarLane.HasComponent(subLane.m_SubLane))
            {
              // ISSUE: reference to a compiler-generated field
              PrefabRef prefabRef1 = data.m_PrefabRef[subLane.m_SubLane];
              CarLaneData componentData2;
              // ISSUE: reference to a compiler-generated field
              if (data.m_CarLaneData.TryGetComponent(prefabRef1.m_Prefab, out componentData2))
              {
                roadTypes &= ~componentData2.m_RoadTypes;
                if (roadTypes == RoadTypes.None)
                  break;
              }
            }
          }
        }
      }
      if (roadTypes != RoadTypes.None)
      {
        errorQueue.Enqueue(new ErrorData()
        {
          m_ErrorType = ErrorType.NoRoadAccess,
          m_ErrorSeverity = ErrorSeverity.Error,
          m_TempEntity = entity,
          m_Position = transform.m_Position
        });
      }
      else
      {
        if ((netObject.m_Flags & (NetObjectFlags.IsClear | NetObjectFlags.TrackPassThrough)) != (NetObjectFlags) 0)
          return;
        errorQueue.Enqueue(new ErrorData()
        {
          m_ErrorType = ErrorType.OverlapExisting,
          m_ErrorSeverity = ErrorSeverity.Error,
          m_TempEntity = entity,
          m_Position = transform.m_Position
        });
      }
    }

    public static void ValidateOutsideConnection(
      Entity entity,
      Transform transform,
      TerrainHeightData terrainHeightData,
      NativeQueue<ErrorData>.ParallelWriter errorQueue)
    {
      if (!MathUtils.Intersect(MathUtils.Expand(TerrainUtils.GetBounds(ref terrainHeightData), (float3) -0.1f).xz, transform.m_Position.xz))
        return;
      errorQueue.Enqueue(new ErrorData()
      {
        m_ErrorType = ErrorType.NotOnBorder,
        m_ErrorSeverity = ErrorSeverity.Error,
        m_TempEntity = entity,
        m_Position = transform.m_Position
      });
    }

    public static void ValidateWaterSource(
      Entity entity,
      Transform transform,
      Game.Simulation.WaterSourceData waterSourceData,
      TerrainHeightData terrainHeightData,
      NativeQueue<ErrorData>.ParallelWriter errorQueue)
    {
      Bounds3 bounds = TerrainUtils.GetBounds(ref terrainHeightData);
      Bounds3 bounds3_1 = MathUtils.Expand(bounds, (float3) -waterSourceData.m_Radius);
      Bounds3 bounds3_2 = MathUtils.Expand(bounds, (float3) waterSourceData.m_Radius);
      if (waterSourceData.m_ConstantDepth < 2)
      {
        if (MathUtils.Intersect(bounds3_1.xz, transform.m_Position.xz))
          return;
        errorQueue.Enqueue(new ErrorData()
        {
          m_ErrorType = ErrorType.ExceedsCityLimits,
          m_ErrorSeverity = ErrorSeverity.Error,
          m_TempEntity = entity,
          m_Position = transform.m_Position
        });
      }
      else
      {
        if (MathUtils.Intersect(bounds3_2.xz, transform.m_Position.xz) && !MathUtils.Intersect(bounds3_1.xz, transform.m_Position.xz))
          return;
        errorQueue.Enqueue(new ErrorData()
        {
          m_ErrorType = ErrorType.NotOnBorder,
          m_ErrorSeverity = ErrorSeverity.Error,
          m_TempEntity = entity,
          m_Position = transform.m_Position
        });
      }
    }

    private static Entity GetOwner(
      Entity entity,
      Temp temp,
      ValidationSystem.EntityData data,
      out Edge tempNodes,
      out Edge ownerNodes,
      out Entity attachedParent,
      out Entity assetStamp)
    {
      tempNodes = new Edge();
      ownerNodes = new Edge();
      attachedParent = Entity.Null;
      assetStamp = Entity.Null;
      Owner componentData1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!data.m_Owner.TryGetComponent(entity, out componentData1) || data.m_Building.HasComponent(entity))
      {
        entity = temp.m_Original;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        while (!data.m_AssetStamp.HasComponent(componentData1.m_Owner))
        {
          entity = componentData1.m_Owner;
          Edge componentData2;
          // ISSUE: reference to a compiler-generated field
          if (data.m_Edge.TryGetComponent(entity, out componentData2))
          {
            ownerNodes = componentData2;
            // ISSUE: reference to a compiler-generated field
            if (data.m_Temp.TryGetComponent(ownerNodes.m_Start, out temp))
            {
              tempNodes.m_Start = ownerNodes.m_Start;
              ownerNodes.m_Start = temp.m_Original;
            }
            // ISSUE: reference to a compiler-generated field
            if (data.m_Temp.TryGetComponent(ownerNodes.m_End, out temp))
            {
              tempNodes.m_End = ownerNodes.m_End;
              ownerNodes.m_End = temp.m_Original;
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (data.m_Temp.TryGetComponent(entity, out temp))
            entity = temp.m_Original;
          Attached componentData3;
          // ISSUE: reference to a compiler-generated field
          if (data.m_Attached.TryGetComponent(entity, out componentData3))
            attachedParent = componentData3.m_Parent;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!data.m_Owner.TryGetComponent(entity, out componentData1) || data.m_Building.HasComponent(entity))
            goto label_14;
        }
        assetStamp = componentData1.m_Owner;
      }
label_14:
      return entity;
    }

    private static void CheckSurface(
      Entity entity,
      Transform transform,
      CollisionMask collisionMask,
      ObjectGeometryData prefabObjectGeometryData,
      PlaceableObjectData placeableObjectData,
      ValidationSystem.EntityData data,
      WaterSurfaceData waterSurfaceData,
      TerrainHeightData terrainHeightData,
      NativeQueue<ErrorData>.ParallelWriter errorQueue)
    {
      float sampleInterval = WaterUtils.GetSampleInterval(ref waterSurfaceData);
      int2 int2 = (int2) math.ceil((prefabObjectGeometryData.m_Bounds.max.xz - prefabObjectGeometryData.m_Bounds.min.xz) / sampleInterval);
      Quad3 baseCorners = ObjectUtils.CalculateBaseCorners(transform.m_Position, transform.m_Rotation, prefabObjectGeometryData.m_Bounds);
      Bounds3 bounds1;
      bounds1.min = (float3) float.MaxValue;
      bounds1.max = (float3) float.MinValue;
      Bounds3 bounds2;
      bounds2.min = (float3) float.MaxValue;
      bounds2.max = (float3) float.MinValue;
      bool flag1 = false;
      bool flag2 = false;
      for (int index1 = 0; index1 < int2.x; ++index1)
      {
        float s1 = ((float) index1 + 0.5f) / (float) int2.x;
        float3 float3_1 = math.lerp(baseCorners.a, baseCorners.b, s1);
        float3 float3_2 = math.lerp(baseCorners.d, baseCorners.c, s1);
        if ((placeableObjectData.m_Flags & PlacementFlags.Shoreline) != PlacementFlags.None)
        {
          double num1 = (double) WaterUtils.SampleDepth(ref waterSurfaceData, float3_1);
          float num2 = WaterUtils.SampleDepth(ref waterSurfaceData, float3_2);
          if (num1 >= 0.20000000298023224)
          {
            bounds1 |= float3_1;
            flag1 = true;
          }
          if ((double) num2 < 0.20000000298023224)
          {
            bounds2 |= float3_2;
            flag2 = true;
          }
        }
        else if ((placeableObjectData.m_Flags & (PlacementFlags.Floating | PlacementFlags.Underwater)) != PlacementFlags.None)
        {
          if ((placeableObjectData.m_Flags & PlacementFlags.OnGround) == PlacementFlags.None)
          {
            for (int index2 = 0; index2 < int2.y; ++index2)
            {
              float s2 = ((float) index2 + 0.5f) / (float) int2.y;
              float3 worldPosition = math.lerp(float3_1, float3_2, s2);
              if ((double) WaterUtils.SampleDepth(ref waterSurfaceData, worldPosition) < 0.20000000298023224)
              {
                bounds2 |= worldPosition;
                flag2 = true;
              }
            }
          }
        }
        else if ((prefabObjectGeometryData.m_Flags & GeometryFlags.CanSubmerge) == GeometryFlags.None)
        {
          for (int index3 = 0; index3 < int2.y; ++index3)
          {
            float s3 = ((float) index3 + 0.5f) / (float) int2.y;
            float3 worldPosition = math.lerp(float3_1, float3_2, s3);
            float waterDepth;
            if ((collisionMask & CollisionMask.ExclusiveGround) != (CollisionMask) 0)
            {
              waterDepth = WaterUtils.SampleDepth(ref waterSurfaceData, worldPosition);
            }
            else
            {
              float num = WaterUtils.SampleHeight(ref waterSurfaceData, ref terrainHeightData, worldPosition, out waterDepth);
              waterDepth = math.min(waterDepth, num - transform.m_Position.y);
            }
            if ((double) waterDepth >= 0.20000000298023224)
            {
              bounds1 |= worldPosition;
              flag1 = true;
            }
          }
        }
      }
      if (flag1)
        errorQueue.Enqueue(new ErrorData()
        {
          m_ErrorType = ErrorType.InWater,
          m_ErrorSeverity = ErrorSeverity.Error,
          m_TempEntity = entity,
          m_Position = MathUtils.Center(bounds1)
        });
      if (!flag2)
        return;
      errorQueue.Enqueue(new ErrorData()
      {
        m_ErrorType = (placeableObjectData.m_Flags & (PlacementFlags.OnGround | PlacementFlags.Shoreline)) != (PlacementFlags.OnGround | PlacementFlags.Shoreline) ? ErrorType.NoWater : ErrorType.NotOnShoreline,
        m_ErrorSeverity = (placeableObjectData.m_Flags & PlacementFlags.OnGround) != PlacementFlags.None ? ErrorSeverity.Warning : ErrorSeverity.Error,
        m_TempEntity = entity,
        m_Position = MathUtils.Center(bounds2)
      });
    }

    public static bool Intersect(Cylinder3 cylinder1, Cylinder3 cylinder2, ref float3 pos)
    {
      quaternion q = math.mul(cylinder2.rotation, math.inverse(cylinder1.rotation));
      cylinder2.circle.position = math.mul(q, new float3(cylinder2.circle.position.x, 0.0f, cylinder2.circle.position.y)).xz;
      cylinder2.height.min = math.mul(q, new float3(0.0f, cylinder2.height.min, 0.0f)).y;
      cylinder2.height.max = math.mul(q, new float3(0.0f, cylinder2.height.max, 0.0f)).y;
      float2 x1 = cylinder1.circle.position - cylinder2.circle.position;
      float num = cylinder1.circle.radius + cylinder2.circle.radius;
      if ((double) math.lengthsq(x1) >= (double) num * (double) num || !MathUtils.Intersect(cylinder1.height, cylinder2.height))
        return false;
      MathUtils.TryNormalize(ref x1);
      float2 x2 = cylinder1.circle.position + x1 * cylinder1.circle.radius;
      float2 y = cylinder2.circle.position - x1 * cylinder2.circle.radius;
      pos.y = MathUtils.Center(cylinder1.height & cylinder2.height);
      pos.xz = math.lerp(x2, y, 0.5f);
      pos = math.mul(cylinder1.rotation, pos);
      return true;
    }

    private struct ObjectIterator : 
      INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
    {
      public Entity m_ObjectEntity;
      public Entity m_TopLevelEntity;
      public Entity m_AssetStampEntity;
      public Bounds3 m_ObjectBounds;
      public Transform m_Transform;
      public Stack m_ObjectStack;
      public CollisionMask m_CollisionMask;
      public ObjectGeometryData m_PrefabObjectGeometryData;
      public StackData m_ObjectStackData;
      public bool m_CanOverride;
      public bool m_Optional;
      public bool m_EditorMode;
      public ValidationSystem.EntityData m_Data;
      public NativeQueue<ErrorData>.ParallelWriter m_ErrorQueue;

      public bool Intersect(QuadTreeBoundsXZ bounds)
      {
        if ((bounds.m_Mask & BoundsMask.NotOverridden) == (BoundsMask) 0)
          return false;
        return (this.m_CollisionMask & CollisionMask.OnGround) != (CollisionMask) 0 ? MathUtils.Intersect(bounds.m_Bounds.xz, this.m_ObjectBounds.xz) : MathUtils.Intersect(bounds.m_Bounds, this.m_ObjectBounds);
      }

      public void Iterate(QuadTreeBoundsXZ bounds, Entity objectEntity2)
      {
        if ((bounds.m_Mask & BoundsMask.NotOverridden) == (BoundsMask) 0)
          return;
        if ((this.m_CollisionMask & CollisionMask.OnGround) != (CollisionMask) 0)
        {
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_ObjectBounds.xz))
            return;
        }
        else if (!MathUtils.Intersect(bounds.m_Bounds, this.m_ObjectBounds))
          return;
        // ISSUE: reference to a compiler-generated field
        if (this.m_Data.m_Hidden.HasComponent(objectEntity2) || objectEntity2 == this.m_AssetStampEntity)
          return;
        Entity entity = objectEntity2;
        bool hasOwner = false;
        Owner componentData;
        Entity owner;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        for (; this.m_Data.m_Owner.TryGetComponent(entity, out componentData) && !this.m_Data.m_Building.HasComponent(entity); entity = owner)
        {
          owner = componentData.m_Owner;
          hasOwner = true;
          // ISSUE: reference to a compiler-generated field
          if (this.m_Data.m_AssetStamp.HasComponent(owner))
          {
            if (owner == this.m_ObjectEntity)
              return;
            break;
          }
        }
        if (this.m_TopLevelEntity == entity)
          return;
        this.CheckOverlap(entity, objectEntity2, bounds.m_Bounds, false, hasOwner);
      }

      public void CheckOverlap(
        Entity topLevelEntity2,
        Entity objectEntity2,
        Bounds3 bounds2,
        bool essential,
        bool hasOwner)
      {
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef1 = this.m_Data.m_PrefabRef[objectEntity2];
        // ISSUE: reference to a compiler-generated field
        Transform transform = this.m_Data.m_Transform[objectEntity2];
        // ISSUE: reference to a compiler-generated field
        if (!this.m_Data.m_PrefabObjectGeometry.HasComponent(prefabRef1.m_Prefab))
          return;
        // ISSUE: reference to a compiler-generated field
        ObjectGeometryData objectGeometryData = this.m_Data.m_PrefabObjectGeometry[prefabRef1.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        if ((objectGeometryData.m_Flags & GeometryFlags.IgnoreSecondaryCollision) != GeometryFlags.None && this.m_Data.m_Secondary.HasComponent(objectEntity2))
          return;
        Elevation componentData1;
        // ISSUE: reference to a compiler-generated field
        CollisionMask mask2 = !this.m_Data.m_ObjectElevation.TryGetComponent(objectEntity2, out componentData1) ? ObjectUtils.GetCollisionMask(objectGeometryData, !this.m_EditorMode | hasOwner) : ObjectUtils.GetCollisionMask(objectGeometryData, componentData1, !this.m_EditorMode | hasOwner);
        if ((this.m_CollisionMask & mask2) == (CollisionMask) 0)
          return;
        ErrorData error = new ErrorData();
        error.m_ErrorSeverity = ErrorSeverity.Error;
        error.m_TempEntity = this.m_ObjectEntity;
        error.m_PermanentEntity = objectEntity2;
        if (this.m_CanOverride)
        {
          error.m_ErrorSeverity = ErrorSeverity.Override;
          error.m_PermanentEntity = Entity.Null;
        }
        else if (!essential)
        {
          if (topLevelEntity2 != objectEntity2)
          {
            if (topLevelEntity2 != Entity.Null)
            {
              if ((objectGeometryData.m_Flags & (GeometryFlags.Overridable | GeometryFlags.DeleteOverridden)) == GeometryFlags.Overridable)
              {
                if ((this.m_PrefabObjectGeometryData.m_Flags & (GeometryFlags.Overridable | GeometryFlags.DeleteOverridden)) == GeometryFlags.Overridable)
                {
                  if (this.m_Optional)
                    error.m_ErrorSeverity = ErrorSeverity.Warning;
                }
                else
                  error.m_ErrorSeverity = ErrorSeverity.Override;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                PrefabRef prefabRef2 = this.m_Data.m_PrefabRef[topLevelEntity2];
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_Data.m_PrefabObjectGeometry.HasComponent(prefabRef2.m_Prefab) && (this.m_Data.m_PrefabObjectGeometry[prefabRef2.m_Prefab].m_Flags & (GeometryFlags.Overridable | GeometryFlags.DeleteOverridden)) == (GeometryFlags.Overridable | GeometryFlags.DeleteOverridden) && !this.m_Data.m_Attached.HasComponent(topLevelEntity2) && (!this.m_Data.m_Temp.HasComponent(topLevelEntity2) || (this.m_Data.m_Temp[topLevelEntity2].m_Flags & TempFlags.Essential) == (TempFlags) 0) && (this.m_Optional || (this.m_PrefabObjectGeometryData.m_Flags & (GeometryFlags.Overridable | GeometryFlags.DeleteOverridden)) != GeometryFlags.Overridable))
                {
                  error.m_ErrorSeverity = ErrorSeverity.Warning;
                  error.m_PermanentEntity = topLevelEntity2;
                }
              }
            }
          }
          else if ((objectGeometryData.m_Flags & GeometryFlags.Overridable) != GeometryFlags.None)
          {
            if ((objectGeometryData.m_Flags & GeometryFlags.DeleteOverridden) != GeometryFlags.None)
            {
              // ISSUE: reference to a compiler-generated field
              if (!this.m_Data.m_Attached.HasComponent(objectEntity2) && (this.m_Optional || (this.m_PrefabObjectGeometryData.m_Flags & (GeometryFlags.Overridable | GeometryFlags.DeleteOverridden)) != GeometryFlags.Overridable))
                error.m_ErrorSeverity = ErrorSeverity.Warning;
            }
            else if ((this.m_PrefabObjectGeometryData.m_Flags & (GeometryFlags.Overridable | GeometryFlags.DeleteOverridden)) == GeometryFlags.Overridable)
            {
              if (this.m_Optional)
                error.m_ErrorSeverity = ErrorSeverity.Warning;
            }
            else
              error.m_ErrorSeverity = ErrorSeverity.Override;
          }
        }
        float3 origin = MathUtils.Center(bounds2);
        StackData componentData2 = new StackData();
        Stack componentData3;
        // ISSUE: reference to a compiler-generated field
        if (this.m_Data.m_Stack.TryGetComponent(objectEntity2, out componentData3))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Data.m_PrefabStackData.TryGetComponent(prefabRef1.m_Prefab, out componentData2);
        }
        if ((this.m_CollisionMask & CollisionMask.OnGround) == (CollisionMask) 0 || MathUtils.Intersect(bounds2, this.m_ObjectBounds))
          this.CheckOverlap3D(ref error, transform, componentData3, objectGeometryData, componentData2, origin);
        if (error.m_ErrorType == ErrorType.None && CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, mask2))
          this.CheckOverlap2D(ref error, transform, objectGeometryData, bounds2, origin);
        if (error.m_ErrorType == ErrorType.None)
          return;
        // ISSUE: reference to a compiler-generated field
        if ((error.m_ErrorSeverity == ErrorSeverity.Override || error.m_ErrorSeverity == ErrorSeverity.Warning) && error.m_ErrorType == ErrorType.OverlapExisting && this.m_Data.m_OnFire.HasComponent(error.m_PermanentEntity))
        {
          error.m_ErrorType = ErrorType.OnFire;
          error.m_ErrorSeverity = ErrorSeverity.Error;
        }
        this.m_ErrorQueue.Enqueue(error);
      }

      private void CheckOverlap3D(
        ref ErrorData error,
        Transform transform2,
        Stack stack2,
        ObjectGeometryData prefabObjectGeometryData2,
        StackData stackData2,
        float3 origin)
      {
        quaternion q1 = math.inverse(this.m_Transform.m_Rotation);
        quaternion q2 = math.inverse(transform2.m_Rotation);
        float3 v = this.m_Transform.m_Position - origin;
        float3 float3_1 = math.mul(q1, v);
        float3 float3_2 = math.mul(q2, transform2.m_Position - origin);
        Bounds3 bounds1 = ObjectUtils.GetBounds(this.m_ObjectStack, this.m_PrefabObjectGeometryData, this.m_ObjectStackData);
        if ((this.m_PrefabObjectGeometryData.m_Flags & GeometryFlags.IgnoreBottomCollision) != GeometryFlags.None)
          bounds1.min.y = math.max(bounds1.min.y, 0.0f);
        if ((this.m_PrefabObjectGeometryData.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
        {
          if ((this.m_PrefabObjectGeometryData.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
          {
            Cylinder3 cylinder3 = new Cylinder3();
            cylinder3.circle = new Circle2((float) ((double) this.m_PrefabObjectGeometryData.m_LegSize.x * 0.5 - 0.0099999997764825821), float3_1.xz);
            cylinder3.height = new Bounds1(bounds1.min.y + 0.01f, this.m_PrefabObjectGeometryData.m_LegSize.y + 0.01f) + float3_1.y;
            cylinder3.rotation = this.m_Transform.m_Rotation;
            Bounds3 bounds2 = ObjectUtils.GetBounds(stack2, prefabObjectGeometryData2, stackData2);
            if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.IgnoreBottomCollision) != GeometryFlags.None)
              bounds2.min.y = math.max(bounds2.min.y, 0.0f);
            if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
            {
              if ((prefabObjectGeometryData2.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
              {
                if (ValidationHelpers.Intersect(cylinder3, new Cylinder3()
                {
                  circle = new Circle2((float) ((double) prefabObjectGeometryData2.m_LegSize.x * 0.5 - 0.0099999997764825821), float3_2.xz),
                  height = new Bounds1(bounds2.min.y + 0.01f, prefabObjectGeometryData2.m_LegSize.y + 0.01f) + float3_2.y,
                  rotation = transform2.m_Rotation
                }, ref error.m_Position))
                {
                  error.m_Position += origin;
                  error.m_ErrorType = ErrorType.OverlapExisting;
                }
              }
              else if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.IgnoreLegCollision) == GeometryFlags.None)
              {
                Box3 box = new Box3();
                box.bounds.min.y = bounds2.min.y + 0.01f;
                box.bounds.min.xz = prefabObjectGeometryData2.m_LegSize.xz * -0.5f + 0.01f;
                box.bounds.max.y = prefabObjectGeometryData2.m_LegSize.y + 0.01f;
                box.bounds.max.xz = prefabObjectGeometryData2.m_LegSize.xz * 0.5f - 0.01f;
                box.bounds += float3_2;
                box.rotation = transform2.m_Rotation;
                Bounds3 cylinderIntersection;
                Bounds3 boxIntersection;
                if (MathUtils.Intersect(cylinder3, box, out cylinderIntersection, out boxIntersection))
                {
                  float3 x = math.mul(cylinder3.rotation, MathUtils.Center(cylinderIntersection));
                  float3 y = math.mul(box.rotation, MathUtils.Center(boxIntersection));
                  error.m_Position = origin + math.lerp(x, y, 0.5f);
                  error.m_ErrorType = ErrorType.OverlapExisting;
                }
              }
              bounds2.min.y = prefabObjectGeometryData2.m_LegSize.y;
            }
            if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
            {
              if (ValidationHelpers.Intersect(cylinder3, new Cylinder3()
              {
                circle = new Circle2((float) ((double) prefabObjectGeometryData2.m_Size.x * 0.5 - 0.0099999997764825821), float3_2.xz),
                height = new Bounds1(bounds2.min.y + 0.01f, bounds2.max.y - 0.01f) + float3_2.y,
                rotation = transform2.m_Rotation
              }, ref error.m_Position))
              {
                error.m_Position += origin;
                error.m_ErrorType = ErrorType.OverlapExisting;
              }
            }
            else
            {
              Box3 box = new Box3()
              {
                bounds = bounds2 + float3_2
              };
              box.bounds = MathUtils.Expand(box.bounds, (float3) -0.01f);
              box.rotation = transform2.m_Rotation;
              Bounds3 cylinderIntersection;
              Bounds3 boxIntersection;
              if (MathUtils.Intersect(cylinder3, box, out cylinderIntersection, out boxIntersection))
              {
                float3 x = math.mul(cylinder3.rotation, MathUtils.Center(cylinderIntersection));
                float3 y = math.mul(box.rotation, MathUtils.Center(boxIntersection));
                error.m_Position = origin + math.lerp(x, y, 0.5f);
                error.m_ErrorType = ErrorType.OverlapExisting;
              }
            }
          }
          else if ((this.m_PrefabObjectGeometryData.m_Flags & GeometryFlags.IgnoreLegCollision) == GeometryFlags.None)
          {
            Box3 box3 = new Box3();
            box3.bounds.min.y = bounds1.min.y + 0.01f;
            box3.bounds.min.xz = this.m_PrefabObjectGeometryData.m_LegSize.xz * -0.5f + 0.01f;
            box3.bounds.max.y = this.m_PrefabObjectGeometryData.m_LegSize.y + 0.01f;
            box3.bounds.max.xz = this.m_PrefabObjectGeometryData.m_LegSize.xz * 0.5f - 0.01f;
            box3.bounds += float3_1;
            box3.rotation = this.m_Transform.m_Rotation;
            Bounds3 bounds3 = ObjectUtils.GetBounds(stack2, prefabObjectGeometryData2, stackData2);
            if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.IgnoreBottomCollision) != GeometryFlags.None)
              bounds3.min.y = math.max(bounds3.min.y, 0.0f);
            if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
            {
              if ((prefabObjectGeometryData2.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
              {
                Cylinder3 cylinder = new Cylinder3();
                cylinder.circle = new Circle2((float) ((double) prefabObjectGeometryData2.m_LegSize.x * 0.5 - 0.0099999997764825821), float3_2.xz);
                cylinder.height = new Bounds1(bounds3.min.y + 0.01f, prefabObjectGeometryData2.m_LegSize.y + 0.01f) + float3_2.y;
                cylinder.rotation = transform2.m_Rotation;
                Bounds3 cylinderIntersection;
                Bounds3 boxIntersection;
                if (MathUtils.Intersect(cylinder, box3, out cylinderIntersection, out boxIntersection))
                {
                  float3 x = math.mul(box3.rotation, MathUtils.Center(boxIntersection));
                  float3 y = math.mul(cylinder.rotation, MathUtils.Center(cylinderIntersection));
                  error.m_Position = origin + math.lerp(x, y, 0.5f);
                  error.m_ErrorType = ErrorType.OverlapExisting;
                }
              }
              else if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.IgnoreLegCollision) == GeometryFlags.None)
              {
                Box3 box2 = new Box3();
                box2.bounds.min.y = bounds3.min.y + 0.01f;
                box2.bounds.min.xz = prefabObjectGeometryData2.m_LegSize.xz * -0.5f + 0.01f;
                box2.bounds.max.y = prefabObjectGeometryData2.m_LegSize.y + 0.01f;
                box2.bounds.max.xz = prefabObjectGeometryData2.m_LegSize.xz * 0.5f - 0.01f;
                box2.bounds += float3_2;
                box2.rotation = transform2.m_Rotation;
                Bounds3 intersection1;
                Bounds3 intersection2;
                if (MathUtils.Intersect(box3, box2, out intersection1, out intersection2))
                {
                  float3 x = math.mul(box3.rotation, MathUtils.Center(intersection1));
                  float3 y = math.mul(box2.rotation, MathUtils.Center(intersection2));
                  error.m_Position = origin + math.lerp(x, y, 0.5f);
                  error.m_ErrorType = ErrorType.OverlapExisting;
                }
              }
              bounds3.min.y = prefabObjectGeometryData2.m_LegSize.y;
            }
            if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
            {
              Cylinder3 cylinder = new Cylinder3();
              cylinder.circle = new Circle2((float) ((double) prefabObjectGeometryData2.m_Size.x * 0.5 - 0.0099999997764825821), float3_2.xz);
              cylinder.height = new Bounds1(bounds3.min.y + 0.01f, bounds3.max.y - 0.01f) + float3_2.y;
              cylinder.rotation = transform2.m_Rotation;
              Bounds3 cylinderIntersection;
              Bounds3 boxIntersection;
              if (MathUtils.Intersect(cylinder, box3, out cylinderIntersection, out boxIntersection))
              {
                float3 x = math.mul(box3.rotation, MathUtils.Center(boxIntersection));
                float3 y = math.mul(cylinder.rotation, MathUtils.Center(cylinderIntersection));
                error.m_Position = origin + math.lerp(x, y, 0.5f);
                error.m_ErrorType = ErrorType.OverlapExisting;
              }
            }
            else
            {
              Box3 box2 = new Box3()
              {
                bounds = bounds3 + float3_2
              };
              box2.bounds = MathUtils.Expand(box2.bounds, (float3) -0.01f);
              box2.rotation = transform2.m_Rotation;
              Bounds3 intersection1;
              Bounds3 intersection2;
              if (MathUtils.Intersect(box3, box2, out intersection1, out intersection2))
              {
                float3 x = math.mul(box3.rotation, MathUtils.Center(intersection1));
                float3 y = math.mul(box2.rotation, MathUtils.Center(intersection2));
                error.m_Position = origin + math.lerp(x, y, 0.5f);
                error.m_ErrorType = ErrorType.OverlapExisting;
              }
            }
          }
          bounds1.min.y = this.m_PrefabObjectGeometryData.m_LegSize.y;
        }
        if ((this.m_PrefabObjectGeometryData.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
        {
          Cylinder3 cylinder3 = new Cylinder3();
          cylinder3.circle = new Circle2((float) ((double) this.m_PrefabObjectGeometryData.m_Size.x * 0.5 - 0.0099999997764825821), float3_1.xz);
          cylinder3.height = new Bounds1(bounds1.min.y + 0.01f, bounds1.max.y - 0.01f) + float3_1.y;
          cylinder3.rotation = this.m_Transform.m_Rotation;
          Bounds3 bounds4 = ObjectUtils.GetBounds(stack2, prefabObjectGeometryData2, stackData2);
          if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.IgnoreBottomCollision) != GeometryFlags.None)
            bounds4.min.y = math.max(bounds4.min.y, 0.0f);
          if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
          {
            if ((prefabObjectGeometryData2.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
            {
              if (ValidationHelpers.Intersect(cylinder3, new Cylinder3()
              {
                circle = new Circle2((float) ((double) prefabObjectGeometryData2.m_LegSize.x * 0.5 - 0.0099999997764825821), float3_2.xz),
                height = new Bounds1(bounds4.min.y + 0.01f, prefabObjectGeometryData2.m_LegSize.y + 0.01f) + float3_2.y,
                rotation = transform2.m_Rotation
              }, ref error.m_Position))
              {
                error.m_Position += origin;
                error.m_ErrorType = ErrorType.OverlapExisting;
              }
            }
            else if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.IgnoreLegCollision) == GeometryFlags.None)
            {
              Box3 box = new Box3();
              box.bounds.min.y = bounds4.min.y + 0.01f;
              box.bounds.min.xz = prefabObjectGeometryData2.m_LegSize.xz * -0.5f + 0.01f;
              box.bounds.max.y = prefabObjectGeometryData2.m_LegSize.y + 0.01f;
              box.bounds.max.xz = prefabObjectGeometryData2.m_LegSize.xz * 0.5f - 0.01f;
              box.bounds += float3_2;
              box.rotation = transform2.m_Rotation;
              Bounds3 cylinderIntersection;
              Bounds3 boxIntersection;
              if (MathUtils.Intersect(cylinder3, box, out cylinderIntersection, out boxIntersection))
              {
                float3 x = math.mul(cylinder3.rotation, MathUtils.Center(cylinderIntersection));
                float3 y = math.mul(box.rotation, MathUtils.Center(boxIntersection));
                error.m_Position = origin + math.lerp(x, y, 0.5f);
                error.m_ErrorType = ErrorType.OverlapExisting;
              }
            }
            bounds4.min.y = prefabObjectGeometryData2.m_LegSize.y;
          }
          if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
          {
            if (!ValidationHelpers.Intersect(cylinder3, new Cylinder3()
            {
              circle = new Circle2((float) ((double) prefabObjectGeometryData2.m_Size.x * 0.5 - 0.0099999997764825821), float3_2.xz),
              height = new Bounds1(bounds4.min.y + 0.01f, bounds4.max.y - 0.01f) + float3_2.y,
              rotation = transform2.m_Rotation
            }, ref error.m_Position))
              return;
            error.m_Position += origin;
            error.m_ErrorType = ErrorType.OverlapExisting;
          }
          else
          {
            Box3 box = new Box3()
            {
              bounds = bounds4 + float3_2
            };
            box.bounds = MathUtils.Expand(box.bounds, (float3) -0.01f);
            box.rotation = transform2.m_Rotation;
            Bounds3 cylinderIntersection;
            Bounds3 boxIntersection;
            if (!MathUtils.Intersect(cylinder3, box, out cylinderIntersection, out boxIntersection))
              return;
            float3 x = math.mul(cylinder3.rotation, MathUtils.Center(cylinderIntersection));
            float3 y = math.mul(box.rotation, MathUtils.Center(boxIntersection));
            error.m_Position = origin + math.lerp(x, y, 0.5f);
            error.m_ErrorType = ErrorType.OverlapExisting;
          }
        }
        else
        {
          Box3 box3 = new Box3()
          {
            bounds = bounds1 + float3_1
          };
          box3.bounds = MathUtils.Expand(box3.bounds, (float3) -0.01f);
          box3.rotation = this.m_Transform.m_Rotation;
          Bounds3 bounds5 = ObjectUtils.GetBounds(stack2, prefabObjectGeometryData2, stackData2);
          if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.IgnoreBottomCollision) != GeometryFlags.None)
            bounds5.min.y = math.max(bounds5.min.y, 0.0f);
          if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
          {
            if ((prefabObjectGeometryData2.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
            {
              Cylinder3 cylinder = new Cylinder3();
              cylinder.circle = new Circle2((float) ((double) prefabObjectGeometryData2.m_LegSize.x * 0.5 - 0.0099999997764825821), float3_2.xz);
              cylinder.height = new Bounds1(bounds5.min.y + 0.01f, prefabObjectGeometryData2.m_LegSize.y + 0.01f) + float3_2.y;
              cylinder.rotation = transform2.m_Rotation;
              Bounds3 cylinderIntersection;
              Bounds3 boxIntersection;
              if (MathUtils.Intersect(cylinder, box3, out cylinderIntersection, out boxIntersection))
              {
                float3 x = math.mul(box3.rotation, MathUtils.Center(boxIntersection));
                float3 y = math.mul(cylinder.rotation, MathUtils.Center(cylinderIntersection));
                error.m_Position = origin + math.lerp(x, y, 0.5f);
                error.m_ErrorType = ErrorType.OverlapExisting;
              }
            }
            else if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.IgnoreLegCollision) == GeometryFlags.None)
            {
              Box3 box2 = new Box3();
              box2.bounds.min.y = bounds5.min.y + 0.01f;
              box2.bounds.min.xz = prefabObjectGeometryData2.m_LegSize.xz * -0.5f + 0.01f;
              box2.bounds.max.y = prefabObjectGeometryData2.m_LegSize.y + 0.01f;
              box2.bounds.max.xz = prefabObjectGeometryData2.m_LegSize.xz * 0.5f - 0.01f;
              box2.bounds += float3_2;
              box2.rotation = transform2.m_Rotation;
              Bounds3 intersection1;
              Bounds3 intersection2;
              if (MathUtils.Intersect(box3, box2, out intersection1, out intersection2))
              {
                float3 x = math.mul(box3.rotation, MathUtils.Center(intersection1));
                float3 y = math.mul(box2.rotation, MathUtils.Center(intersection2));
                error.m_Position = origin + math.lerp(x, y, 0.5f);
                error.m_ErrorType = ErrorType.OverlapExisting;
              }
            }
            bounds5.min.y = prefabObjectGeometryData2.m_LegSize.y;
          }
          if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
          {
            Cylinder3 cylinder = new Cylinder3();
            cylinder.circle = new Circle2((float) ((double) prefabObjectGeometryData2.m_Size.x * 0.5 - 0.0099999997764825821), float3_2.xz);
            cylinder.height = new Bounds1(bounds5.min.y + 0.01f, bounds5.max.y - 0.01f) + float3_2.y;
            cylinder.rotation = transform2.m_Rotation;
            Bounds3 cylinderIntersection;
            Bounds3 boxIntersection;
            if (!MathUtils.Intersect(cylinder, box3, out cylinderIntersection, out boxIntersection))
              return;
            float3 x = math.mul(box3.rotation, MathUtils.Center(boxIntersection));
            float3 y = math.mul(cylinder.rotation, MathUtils.Center(cylinderIntersection));
            error.m_Position = origin + math.lerp(x, y, 0.5f);
            error.m_ErrorType = ErrorType.OverlapExisting;
          }
          else
          {
            Box3 box2 = new Box3()
            {
              bounds = bounds5 + float3_2
            };
            box2.bounds = MathUtils.Expand(box2.bounds, (float3) -0.01f);
            box2.rotation = transform2.m_Rotation;
            Bounds3 intersection1;
            Bounds3 intersection2;
            if (!MathUtils.Intersect(box3, box2, out intersection1, out intersection2))
              return;
            float3 x = math.mul(box3.rotation, MathUtils.Center(intersection1));
            float3 y = math.mul(box2.rotation, MathUtils.Center(intersection2));
            error.m_Position = origin + math.lerp(x, y, 0.5f);
            error.m_ErrorType = ErrorType.OverlapExisting;
          }
        }
      }

      private void CheckOverlap2D(
        ref ErrorData error,
        Transform transformData2,
        ObjectGeometryData prefabObjectGeometryData2,
        Bounds3 bounds2,
        float3 origin)
      {
        if ((this.m_PrefabObjectGeometryData.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
        {
          if ((this.m_PrefabObjectGeometryData.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
          {
            Circle2 circle2_1;
            ref Circle2 local1 = ref circle2_1;
            double _radius1 = (double) this.m_PrefabObjectGeometryData.m_LegSize.x * 0.5 - 0.0099999997764825821;
            float3 float3 = this.m_Transform.m_Position - origin;
            float2 xz1 = float3.xz;
            local1 = new Circle2((float) _radius1, xz1);
            if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
            {
              if ((prefabObjectGeometryData2.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
              {
                Circle2 circle2_2;
                ref Circle2 local2 = ref circle2_2;
                double _radius2 = (double) prefabObjectGeometryData2.m_LegSize.x * 0.5 - 0.0099999997764825821;
                float3 = transformData2.m_Position - origin;
                float2 xz2 = float3.xz;
                local2 = new Circle2((float) _radius2, xz2);
                if (!MathUtils.Intersect(circle2_1, circle2_2))
                  return;
                error.m_Position.xz = origin.xz + MathUtils.Center(MathUtils.Bounds(circle2_1) & MathUtils.Bounds(circle2_2));
                error.m_Position.y = MathUtils.Center(bounds2.y & this.m_ObjectBounds.y);
                error.m_ErrorType = ErrorType.OverlapExisting;
              }
              else
              {
                if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.IgnoreLegCollision) != GeometryFlags.None)
                  return;
                Bounds2 intersection;
                if (!MathUtils.Intersect(ObjectUtils.CalculateBaseCorners(transformData2.m_Position - origin, transformData2.m_Rotation, MathUtils.Expand(new Bounds3()
                {
                  min = {
                    xz = prefabObjectGeometryData2.m_LegSize.xz * -0.5f
                  },
                  max = {
                    xz = prefabObjectGeometryData2.m_LegSize.xz * 0.5f
                  }
                }, (float3) -0.01f)).xz, circle2_1, out intersection))
                  return;
                error.m_Position.xz = origin.xz + MathUtils.Center(intersection);
                error.m_Position.y = MathUtils.Center(bounds2.y & this.m_ObjectBounds.y);
                error.m_ErrorType = ErrorType.OverlapExisting;
              }
            }
            else if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
            {
              Circle2 circle2_3;
              ref Circle2 local3 = ref circle2_3;
              double _radius3 = (double) prefabObjectGeometryData2.m_Size.x * 0.5 - 0.0099999997764825821;
              float3 = transformData2.m_Position - origin;
              float2 xz3 = float3.xz;
              local3 = new Circle2((float) _radius3, xz3);
              if (!MathUtils.Intersect(circle2_1, circle2_3))
                return;
              error.m_Position.xz = origin.xz + MathUtils.Center(MathUtils.Bounds(circle2_1) & MathUtils.Bounds(circle2_3));
              error.m_Position.y = MathUtils.Center(bounds2.y & this.m_ObjectBounds.y);
              error.m_ErrorType = ErrorType.OverlapExisting;
            }
            else
            {
              Bounds2 intersection;
              if (!MathUtils.Intersect(ObjectUtils.CalculateBaseCorners(transformData2.m_Position - origin, transformData2.m_Rotation, MathUtils.Expand(prefabObjectGeometryData2.m_Bounds, (float3) -0.01f)).xz, circle2_1, out intersection))
                return;
              error.m_Position.xz = origin.xz + MathUtils.Center(intersection);
              error.m_Position.y = MathUtils.Center(bounds2.y & this.m_ObjectBounds.y);
              error.m_ErrorType = ErrorType.OverlapExisting;
            }
          }
          else
          {
            if ((this.m_PrefabObjectGeometryData.m_Flags & GeometryFlags.IgnoreLegCollision) != GeometryFlags.None)
              return;
            Quad2 xz4 = ObjectUtils.CalculateBaseCorners(this.m_Transform.m_Position - origin, this.m_Transform.m_Rotation, MathUtils.Expand(new Bounds3()
            {
              min = {
                xz = this.m_PrefabObjectGeometryData.m_LegSize.xz * -0.5f
              },
              max = {
                xz = this.m_PrefabObjectGeometryData.m_LegSize.xz * 0.5f
              }
            }, (float3) -0.01f)).xz;
            if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
            {
              if ((prefabObjectGeometryData2.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
              {
                Circle2 circle = new Circle2((float) ((double) prefabObjectGeometryData2.m_LegSize.x * 0.5 - 0.0099999997764825821), (transformData2.m_Position - origin).xz);
                Bounds2 intersection;
                if (!MathUtils.Intersect(xz4, circle, out intersection))
                  return;
                error.m_Position.xz = origin.xz + MathUtils.Center(intersection);
                error.m_Position.y = MathUtils.Center(bounds2.y & this.m_ObjectBounds.y);
                error.m_ErrorType = ErrorType.OverlapExisting;
              }
              else
              {
                if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.IgnoreLegCollision) != GeometryFlags.None)
                  return;
                Quad2 xz5 = ObjectUtils.CalculateBaseCorners(transformData2.m_Position - origin, transformData2.m_Rotation, MathUtils.Expand(new Bounds3()
                {
                  min = {
                    xz = prefabObjectGeometryData2.m_LegSize.xz * -0.5f
                  },
                  max = {
                    xz = prefabObjectGeometryData2.m_LegSize.xz * 0.5f
                  }
                }, (float3) -0.01f)).xz;
                Bounds2 intersection;
                if (!MathUtils.Intersect(xz4, xz5, out intersection))
                  return;
                error.m_Position.xz = origin.xz + MathUtils.Center(intersection);
                error.m_Position.y = MathUtils.Center(bounds2.y & this.m_ObjectBounds.y);
                error.m_ErrorType = ErrorType.OverlapExisting;
              }
            }
            else if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
            {
              Circle2 circle = new Circle2((float) ((double) prefabObjectGeometryData2.m_Size.x * 0.5 - 0.0099999997764825821), (transformData2.m_Position - origin).xz);
              Bounds2 intersection;
              if (!MathUtils.Intersect(xz4, circle, out intersection))
                return;
              error.m_Position.xz = origin.xz + MathUtils.Center(intersection);
              error.m_Position.y = MathUtils.Center(bounds2.y & this.m_ObjectBounds.y);
              error.m_ErrorType = ErrorType.OverlapExisting;
            }
            else
            {
              Quad2 xz6 = ObjectUtils.CalculateBaseCorners(transformData2.m_Position - origin, transformData2.m_Rotation, MathUtils.Expand(prefabObjectGeometryData2.m_Bounds, (float3) -0.01f)).xz;
              Bounds2 intersection;
              if (!MathUtils.Intersect(xz4, xz6, out intersection))
                return;
              error.m_Position.xz = origin.xz + MathUtils.Center(intersection);
              error.m_Position.y = MathUtils.Center(bounds2.y & this.m_ObjectBounds.y);
              error.m_ErrorType = ErrorType.OverlapExisting;
            }
          }
        }
        else if ((this.m_PrefabObjectGeometryData.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
        {
          Circle2 circle2_4;
          ref Circle2 local4 = ref circle2_4;
          double _radius4 = (double) this.m_PrefabObjectGeometryData.m_Size.x * 0.5 - 0.0099999997764825821;
          float3 float3 = this.m_Transform.m_Position - origin;
          float2 xz7 = float3.xz;
          local4 = new Circle2((float) _radius4, xz7);
          if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
          {
            if ((prefabObjectGeometryData2.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
            {
              Circle2 circle2_5;
              ref Circle2 local5 = ref circle2_5;
              double _radius5 = (double) prefabObjectGeometryData2.m_LegSize.x * 0.5 - 0.0099999997764825821;
              float3 = transformData2.m_Position - origin;
              float2 xz8 = float3.xz;
              local5 = new Circle2((float) _radius5, xz8);
              if (!MathUtils.Intersect(circle2_4, circle2_5))
                return;
              error.m_Position.xz = origin.xz + MathUtils.Center(MathUtils.Bounds(circle2_4) & MathUtils.Bounds(circle2_5));
              error.m_Position.y = MathUtils.Center(bounds2.y & this.m_ObjectBounds.y);
              error.m_ErrorType = ErrorType.OverlapExisting;
            }
            else
            {
              if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.IgnoreLegCollision) != GeometryFlags.None)
                return;
              Bounds2 intersection;
              if (!MathUtils.Intersect(ObjectUtils.CalculateBaseCorners(transformData2.m_Position - origin, transformData2.m_Rotation, MathUtils.Expand(new Bounds3()
              {
                min = {
                  xz = prefabObjectGeometryData2.m_LegSize.xz * -0.5f
                },
                max = {
                  xz = prefabObjectGeometryData2.m_LegSize.xz * 0.5f
                }
              }, (float3) -0.01f)).xz, circle2_4, out intersection))
                return;
              error.m_Position.xz = origin.xz + MathUtils.Center(intersection);
              error.m_Position.y = MathUtils.Center(bounds2.y & this.m_ObjectBounds.y);
              error.m_ErrorType = ErrorType.OverlapExisting;
            }
          }
          else if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
          {
            Circle2 circle2_6;
            ref Circle2 local6 = ref circle2_6;
            double _radius6 = (double) prefabObjectGeometryData2.m_Size.x * 0.5 - 0.0099999997764825821;
            float3 = transformData2.m_Position - origin;
            float2 xz9 = float3.xz;
            local6 = new Circle2((float) _radius6, xz9);
            if (!MathUtils.Intersect(circle2_4, circle2_6))
              return;
            error.m_Position.xz = origin.xz + MathUtils.Center(MathUtils.Bounds(circle2_4) & MathUtils.Bounds(circle2_6));
            error.m_Position.y = MathUtils.Center(bounds2.y & this.m_ObjectBounds.y);
            error.m_ErrorType = ErrorType.OverlapExisting;
          }
          else
          {
            Bounds2 intersection;
            if (!MathUtils.Intersect(ObjectUtils.CalculateBaseCorners(transformData2.m_Position - origin, transformData2.m_Rotation, MathUtils.Expand(prefabObjectGeometryData2.m_Bounds, (float3) -0.01f)).xz, circle2_4, out intersection))
              return;
            error.m_Position.xz = origin.xz + MathUtils.Center(intersection);
            error.m_Position.y = MathUtils.Center(bounds2.y & this.m_ObjectBounds.y);
            error.m_ErrorType = ErrorType.OverlapExisting;
          }
        }
        else
        {
          Quad3 baseCorners = ObjectUtils.CalculateBaseCorners(this.m_Transform.m_Position - origin, this.m_Transform.m_Rotation, MathUtils.Expand(this.m_PrefabObjectGeometryData.m_Bounds, (float3) -0.01f));
          Quad2 xz10 = baseCorners.xz;
          if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
          {
            if ((prefabObjectGeometryData2.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
            {
              Circle2 circle = new Circle2((float) ((double) prefabObjectGeometryData2.m_LegSize.x * 0.5 - 0.0099999997764825821), (transformData2.m_Position - origin).xz);
              Bounds2 intersection;
              if (!MathUtils.Intersect(xz10, circle, out intersection))
                return;
              error.m_Position.xz = origin.xz + MathUtils.Center(intersection);
              error.m_Position.y = MathUtils.Center(bounds2.y & this.m_ObjectBounds.y);
              error.m_ErrorType = ErrorType.OverlapExisting;
            }
            else
            {
              if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.IgnoreLegCollision) != GeometryFlags.None)
                return;
              baseCorners = ObjectUtils.CalculateBaseCorners(transformData2.m_Position - origin, transformData2.m_Rotation, MathUtils.Expand(new Bounds3()
              {
                min = {
                  xz = prefabObjectGeometryData2.m_LegSize.xz * -0.5f
                },
                max = {
                  xz = prefabObjectGeometryData2.m_LegSize.xz * 0.5f
                }
              }, (float3) -0.01f));
              Quad2 xz11 = baseCorners.xz;
              Bounds2 intersection;
              if (!MathUtils.Intersect(xz10, xz11, out intersection))
                return;
              error.m_Position.xz = origin.xz + MathUtils.Center(intersection);
              error.m_Position.y = MathUtils.Center(bounds2.y & this.m_ObjectBounds.y);
              error.m_ErrorType = ErrorType.OverlapExisting;
            }
          }
          else if ((prefabObjectGeometryData2.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
          {
            Circle2 circle = new Circle2((float) ((double) prefabObjectGeometryData2.m_Size.x * 0.5 - 0.0099999997764825821), (transformData2.m_Position - origin).xz);
            Bounds2 intersection;
            if (!MathUtils.Intersect(xz10, circle, out intersection))
              return;
            error.m_Position.xz = origin.xz + MathUtils.Center(intersection);
            error.m_Position.y = MathUtils.Center(bounds2.y & this.m_ObjectBounds.y);
            error.m_ErrorType = ErrorType.OverlapExisting;
          }
          else
          {
            baseCorners = ObjectUtils.CalculateBaseCorners(transformData2.m_Position - origin, transformData2.m_Rotation, MathUtils.Expand(prefabObjectGeometryData2.m_Bounds, (float3) -0.01f));
            Quad2 xz12 = baseCorners.xz;
            Bounds2 intersection;
            if (!MathUtils.Intersect(xz10, xz12, out intersection))
              return;
            error.m_Position.xz = origin.xz + MathUtils.Center(intersection);
            error.m_Position.y = MathUtils.Center(bounds2.y & this.m_ObjectBounds.y);
            error.m_ErrorType = ErrorType.OverlapExisting;
          }
        }
      }
    }

    private struct NetIterator : 
      INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
    {
      public Entity m_ObjectEntity;
      public Entity m_AttachedParent;
      public Entity m_TopLevelEntity;
      public Entity m_IgnoreNode;
      public Edge m_OwnerNodes;
      public Bounds3 m_ObjectBounds;
      public Transform m_Transform;
      public Stack m_ObjectStack;
      public CollisionMask m_CollisionMask;
      public ObjectGeometryData m_PrefabObjectGeometryData;
      public StackData m_ObjectStackData;
      public bool m_Optional;
      public bool m_EditorMode;
      public ValidationSystem.EntityData m_Data;
      public NativeQueue<ErrorData>.ParallelWriter m_ErrorQueue;

      public bool Intersect(QuadTreeBoundsXZ bounds)
      {
        return (this.m_CollisionMask & CollisionMask.OnGround) != (CollisionMask) 0 ? MathUtils.Intersect(bounds.m_Bounds.xz, this.m_ObjectBounds.xz) : MathUtils.Intersect(bounds.m_Bounds, this.m_ObjectBounds);
      }

      public void Iterate(QuadTreeBoundsXZ bounds, Entity edgeEntity2)
      {
        if ((this.m_CollisionMask & CollisionMask.OnGround) != (CollisionMask) 0)
        {
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_ObjectBounds.xz))
            return;
        }
        else if (!MathUtils.Intersect(bounds.m_Bounds, this.m_ObjectBounds))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Data.m_Hidden.HasComponent(edgeEntity2) || !this.m_Data.m_EdgeGeometry.HasComponent(edgeEntity2))
          return;
        // ISSUE: reference to a compiler-generated field
        Edge edgeData2 = this.m_Data.m_Edge[edgeEntity2];
        bool flag1 = true;
        bool flag2 = true;
        if (edgeEntity2 == this.m_AttachedParent || edgeData2.m_Start == this.m_AttachedParent || edgeData2.m_End == this.m_AttachedParent)
          return;
        Entity entity1 = this.m_ObjectEntity;
        // ISSUE: reference to a compiler-generated field
        while (this.m_Data.m_Owner.HasComponent(entity1))
        {
          // ISSUE: reference to a compiler-generated field
          entity1 = this.m_Data.m_Owner[entity1].m_Owner;
          // ISSUE: reference to a compiler-generated field
          if (this.m_Data.m_Temp.HasComponent(entity1))
          {
            // ISSUE: reference to a compiler-generated field
            Temp temp = this.m_Data.m_Temp[entity1];
            if (temp.m_Original != Entity.Null)
              entity1 = temp.m_Original;
          }
          if (edgeEntity2 == entity1 || edgeData2.m_Start == entity1 || edgeData2.m_End == entity1)
            return;
        }
        Entity entity2 = edgeEntity2;
        bool hasOwner = false;
        Owner componentData;
        Entity owner;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        for (; this.m_Data.m_Owner.TryGetComponent(entity2, out componentData) && !this.m_Data.m_Building.HasComponent(entity2); entity2 = owner)
        {
          owner = componentData.m_Owner;
          hasOwner = true;
          // ISSUE: reference to a compiler-generated field
          if (this.m_Data.m_AssetStamp.HasComponent(owner))
          {
            if (owner == this.m_ObjectEntity)
              return;
            break;
          }
        }
        if (this.m_TopLevelEntity == entity2)
          return;
        // ISSUE: reference to a compiler-generated field
        Composition compositionData2 = this.m_Data.m_Composition[edgeEntity2];
        // ISSUE: reference to a compiler-generated field
        EdgeGeometry edgeGeometryData2 = this.m_Data.m_EdgeGeometry[edgeEntity2];
        // ISSUE: reference to a compiler-generated field
        StartNodeGeometry startNodeGeometryData2 = this.m_Data.m_StartNodeGeometry[edgeEntity2];
        // ISSUE: reference to a compiler-generated field
        EndNodeGeometry endNodeGeometryData2 = this.m_Data.m_EndNodeGeometry[edgeEntity2];
        float3 origin = MathUtils.Center(bounds.m_Bounds);
        bool checkStartNode = ((flag1 ? 1 : 0) & (!(edgeData2.m_Start != this.m_OwnerNodes.m_Start) ? 0 : (edgeData2.m_Start != this.m_OwnerNodes.m_End ? 1 : 0))) != 0;
        bool checkEndNode = ((flag2 ? 1 : 0) & (!(edgeData2.m_End != this.m_OwnerNodes.m_Start) ? 0 : (edgeData2.m_End != this.m_OwnerNodes.m_End ? 1 : 0))) != 0;
        if (edgeData2.m_Start == this.m_IgnoreNode)
        {
          // ISSUE: reference to a compiler-generated field
          NetCompositionData netCompositionData = this.m_Data.m_PrefabComposition[compositionData2.m_StartNode];
          checkStartNode &= (netCompositionData.m_Flags.m_General & CompositionFlags.General.Roundabout) == (CompositionFlags.General) 0;
        }
        if (edgeData2.m_End == this.m_IgnoreNode)
        {
          // ISSUE: reference to a compiler-generated field
          NetCompositionData netCompositionData = this.m_Data.m_PrefabComposition[compositionData2.m_EndNode];
          checkEndNode &= (netCompositionData.m_Flags.m_General & CompositionFlags.General.Roundabout) == (CompositionFlags.General) 0;
        }
        this.CheckOverlap(entity2, edgeEntity2, bounds.m_Bounds, edgeData2, compositionData2, edgeGeometryData2, startNodeGeometryData2, endNodeGeometryData2, origin, checkStartNode, checkEndNode, false, hasOwner);
      }

      public void CheckOverlap(
        Entity topLevelEntity2,
        Entity edgeEntity2,
        Bounds3 bounds2,
        Edge edgeData2,
        Composition compositionData2,
        EdgeGeometry edgeGeometryData2,
        StartNodeGeometry startNodeGeometryData2,
        EndNodeGeometry endNodeGeometryData2,
        float3 origin,
        bool checkStartNode,
        bool checkEndNode,
        bool essential,
        bool hasOwner)
      {
        // ISSUE: reference to a compiler-generated field
        NetCompositionData netCompositionData1 = this.m_Data.m_PrefabComposition[compositionData2.m_Edge];
        // ISSUE: reference to a compiler-generated field
        NetCompositionData netCompositionData2 = this.m_Data.m_PrefabComposition[compositionData2.m_StartNode];
        // ISSUE: reference to a compiler-generated field
        NetCompositionData netCompositionData3 = this.m_Data.m_PrefabComposition[compositionData2.m_EndNode];
        CollisionMask collisionMask = NetUtils.GetCollisionMask(netCompositionData1, !this.m_EditorMode | hasOwner);
        CollisionMask startCollisionMask2 = NetUtils.GetCollisionMask(netCompositionData2, !this.m_EditorMode | hasOwner);
        CollisionMask endCollisionMask2 = NetUtils.GetCollisionMask(netCompositionData3, !this.m_EditorMode | hasOwner);
        if (!checkStartNode)
          startCollisionMask2 = (CollisionMask) 0;
        if (!checkEndNode)
          endCollisionMask2 = (CollisionMask) 0;
        CollisionMask mask2 = collisionMask | startCollisionMask2 | endCollisionMask2;
        if ((this.m_CollisionMask & mask2) == (CollisionMask) 0)
          return;
        DynamicBuffer<NetCompositionArea> edgeCompositionAreas2 = new DynamicBuffer<NetCompositionArea>();
        DynamicBuffer<NetCompositionArea> startCompositionAreas2 = new DynamicBuffer<NetCompositionArea>();
        DynamicBuffer<NetCompositionArea> endCompositionAreas2 = new DynamicBuffer<NetCompositionArea>();
        if ((this.m_PrefabObjectGeometryData.m_Flags & (GeometryFlags.Overridable | GeometryFlags.DeleteOverridden)) != GeometryFlags.Overridable)
        {
          // ISSUE: reference to a compiler-generated field
          edgeCompositionAreas2 = this.m_Data.m_PrefabCompositionAreas[compositionData2.m_Edge];
          // ISSUE: reference to a compiler-generated field
          startCompositionAreas2 = this.m_Data.m_PrefabCompositionAreas[compositionData2.m_StartNode];
          // ISSUE: reference to a compiler-generated field
          endCompositionAreas2 = this.m_Data.m_PrefabCompositionAreas[compositionData2.m_EndNode];
        }
        ErrorData error = new ErrorData();
        if ((this.m_CollisionMask & CollisionMask.OnGround) == (CollisionMask) 0 || MathUtils.Intersect(bounds2, this.m_ObjectBounds))
          this.CheckOverlap3D(ref error, collisionMask, startCollisionMask2, endCollisionMask2, edgeData2, edgeGeometryData2, startNodeGeometryData2, endNodeGeometryData2, netCompositionData1, netCompositionData2, netCompositionData3, edgeCompositionAreas2, startCompositionAreas2, endCompositionAreas2, origin);
        if (error.m_ErrorType == ErrorType.None && CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, mask2))
          this.CheckOverlap2D(ref error, collisionMask, startCollisionMask2, endCollisionMask2, edgeData2, edgeGeometryData2, startNodeGeometryData2, endNodeGeometryData2, netCompositionData1, netCompositionData2, netCompositionData3, edgeCompositionAreas2, startCompositionAreas2, endCompositionAreas2, origin);
        if (error.m_ErrorType == ErrorType.None)
          return;
        if (this.m_Optional)
        {
          error.m_ErrorSeverity = ErrorSeverity.Override;
          error.m_TempEntity = this.m_ObjectEntity;
        }
        else
        {
          error.m_ErrorSeverity = ErrorSeverity.Error;
          error.m_TempEntity = this.m_ObjectEntity;
          error.m_PermanentEntity = edgeEntity2;
          if (!essential && topLevelEntity2 != edgeEntity2 && topLevelEntity2 != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_Data.m_PrefabRef[topLevelEntity2];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_Data.m_PrefabObjectGeometry.HasComponent(prefabRef.m_Prefab) && (this.m_Data.m_PrefabObjectGeometry[prefabRef.m_Prefab].m_Flags & (GeometryFlags.Overridable | GeometryFlags.DeleteOverridden)) == (GeometryFlags.Overridable | GeometryFlags.DeleteOverridden) && !this.m_Data.m_Attached.HasComponent(topLevelEntity2) && (!this.m_Data.m_Temp.HasComponent(topLevelEntity2) || (this.m_Data.m_Temp[topLevelEntity2].m_Flags & TempFlags.Essential) == (TempFlags) 0))
            {
              error.m_ErrorSeverity = ErrorSeverity.Warning;
              error.m_PermanentEntity = topLevelEntity2;
            }
          }
        }
        this.m_ErrorQueue.Enqueue(error);
      }

      private void CheckOverlap3D(
        ref ErrorData error,
        CollisionMask edgeCollisionMask2,
        CollisionMask startCollisionMask2,
        CollisionMask endCollisionMask2,
        Edge edgeData2,
        EdgeGeometry edgeGeometryData2,
        StartNodeGeometry startNodeGeometryData2,
        EndNodeGeometry endNodeGeometryData2,
        NetCompositionData edgeCompositionData2,
        NetCompositionData startCompositionData2,
        NetCompositionData endCompositionData2,
        DynamicBuffer<NetCompositionArea> edgeCompositionAreas2,
        DynamicBuffer<NetCompositionArea> startCompositionAreas2,
        DynamicBuffer<NetCompositionArea> endCompositionAreas2,
        float3 origin)
      {
        Bounds3 intersection;
        intersection.min = (float3) float.MaxValue;
        intersection.max = (float3) float.MinValue;
        float3 float3 = math.mul(math.inverse(this.m_Transform.m_Rotation), this.m_Transform.m_Position - origin);
        Bounds3 bounds = ObjectUtils.GetBounds(this.m_ObjectStack, this.m_PrefabObjectGeometryData, this.m_ObjectStackData);
        if ((this.m_PrefabObjectGeometryData.m_Flags & GeometryFlags.IgnoreBottomCollision) != GeometryFlags.None)
          bounds.min.y = math.max(bounds.min.y, 0.0f);
        if ((this.m_PrefabObjectGeometryData.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
        {
          if ((this.m_PrefabObjectGeometryData.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
          {
            Cylinder3 cylinder2 = new Cylinder3();
            cylinder2.circle = new Circle2(this.m_PrefabObjectGeometryData.m_LegSize.x * 0.5f, float3.xz);
            cylinder2.height = new Bounds1(bounds.min.y, this.m_PrefabObjectGeometryData.m_LegSize.y) + float3.y;
            cylinder2.rotation = this.m_Transform.m_Rotation;
            if ((edgeCollisionMask2 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edgeData2, this.m_TopLevelEntity, edgeGeometryData2, -origin, cylinder2, this.m_ObjectBounds, edgeCompositionData2, edgeCompositionAreas2, ref intersection))
              error.m_ErrorType = ErrorType.OverlapExisting;
            if ((startCollisionMask2 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edgeData2.m_Start, this.m_TopLevelEntity, startNodeGeometryData2.m_Geometry, -origin, cylinder2, this.m_ObjectBounds, startCompositionData2, startCompositionAreas2, ref intersection))
              error.m_ErrorType = ErrorType.OverlapExisting;
            if ((endCollisionMask2 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edgeData2.m_End, this.m_TopLevelEntity, endNodeGeometryData2.m_Geometry, -origin, cylinder2, this.m_ObjectBounds, endCompositionData2, endCompositionAreas2, ref intersection))
              error.m_ErrorType = ErrorType.OverlapExisting;
          }
          else if ((this.m_PrefabObjectGeometryData.m_Flags & GeometryFlags.IgnoreLegCollision) == GeometryFlags.None)
          {
            Box3 box2 = new Box3();
            box2.bounds.min.y = bounds.min.y;
            box2.bounds.min.xz = this.m_PrefabObjectGeometryData.m_LegSize.xz * -0.5f;
            box2.bounds.max.y = this.m_PrefabObjectGeometryData.m_LegSize.y;
            box2.bounds.max.xz = this.m_PrefabObjectGeometryData.m_LegSize.xz * 0.5f;
            box2.bounds += float3;
            box2.rotation = this.m_Transform.m_Rotation;
            if ((edgeCollisionMask2 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edgeData2, this.m_TopLevelEntity, edgeGeometryData2, -origin, box2, this.m_ObjectBounds, edgeCompositionData2, edgeCompositionAreas2, ref intersection))
              error.m_ErrorType = ErrorType.OverlapExisting;
            if ((startCollisionMask2 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edgeData2.m_Start, this.m_TopLevelEntity, startNodeGeometryData2.m_Geometry, -origin, box2, this.m_ObjectBounds, startCompositionData2, startCompositionAreas2, ref intersection))
              error.m_ErrorType = ErrorType.OverlapExisting;
            if ((endCollisionMask2 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edgeData2.m_End, this.m_TopLevelEntity, endNodeGeometryData2.m_Geometry, -origin, box2, this.m_ObjectBounds, endCompositionData2, endCompositionAreas2, ref intersection))
              error.m_ErrorType = ErrorType.OverlapExisting;
          }
          bounds.min.y = this.m_PrefabObjectGeometryData.m_LegSize.y;
        }
        if ((this.m_PrefabObjectGeometryData.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
        {
          Cylinder3 cylinder2 = new Cylinder3();
          cylinder2.circle = new Circle2(this.m_PrefabObjectGeometryData.m_Size.x * 0.5f, float3.xz);
          cylinder2.height = new Bounds1(bounds.min.y, bounds.max.y) + float3.y;
          cylinder2.rotation = this.m_Transform.m_Rotation;
          if ((edgeCollisionMask2 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edgeData2, this.m_TopLevelEntity, edgeGeometryData2, -origin, cylinder2, this.m_ObjectBounds, edgeCompositionData2, edgeCompositionAreas2, ref intersection))
            error.m_ErrorType = ErrorType.OverlapExisting;
          if ((startCollisionMask2 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edgeData2.m_Start, this.m_TopLevelEntity, startNodeGeometryData2.m_Geometry, -origin, cylinder2, this.m_ObjectBounds, startCompositionData2, startCompositionAreas2, ref intersection))
            error.m_ErrorType = ErrorType.OverlapExisting;
          if ((endCollisionMask2 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edgeData2.m_End, this.m_TopLevelEntity, endNodeGeometryData2.m_Geometry, -origin, cylinder2, this.m_ObjectBounds, endCompositionData2, endCompositionAreas2, ref intersection))
            error.m_ErrorType = ErrorType.OverlapExisting;
        }
        else
        {
          Box3 box2 = new Box3();
          box2.bounds = bounds + float3;
          box2.rotation = this.m_Transform.m_Rotation;
          if ((edgeCollisionMask2 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edgeData2, this.m_TopLevelEntity, edgeGeometryData2, -origin, box2, this.m_ObjectBounds, edgeCompositionData2, edgeCompositionAreas2, ref intersection))
            error.m_ErrorType = ErrorType.OverlapExisting;
          if ((startCollisionMask2 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edgeData2.m_Start, this.m_TopLevelEntity, startNodeGeometryData2.m_Geometry, -origin, box2, this.m_ObjectBounds, startCompositionData2, startCompositionAreas2, ref intersection))
            error.m_ErrorType = ErrorType.OverlapExisting;
          if ((endCollisionMask2 & this.m_CollisionMask) != (CollisionMask) 0 && Game.Net.ValidationHelpers.Intersect(edgeData2.m_End, this.m_TopLevelEntity, endNodeGeometryData2.m_Geometry, -origin, box2, this.m_ObjectBounds, endCompositionData2, endCompositionAreas2, ref intersection))
            error.m_ErrorType = ErrorType.OverlapExisting;
        }
        if (error.m_ErrorType == ErrorType.None)
          return;
        error.m_Position = origin + MathUtils.Center(intersection);
      }

      private void CheckOverlap2D(
        ref ErrorData error,
        CollisionMask edgeCollisionMask2,
        CollisionMask startCollisionMask2,
        CollisionMask endCollisionMask2,
        Edge edgeData2,
        EdgeGeometry edgeGeometryData2,
        StartNodeGeometry startNodeGeometryData2,
        EndNodeGeometry endNodeGeometryData2,
        NetCompositionData edgeCompositionData2,
        NetCompositionData startCompositionData2,
        NetCompositionData endCompositionData2,
        DynamicBuffer<NetCompositionArea> edgeCompositionAreas2,
        DynamicBuffer<NetCompositionArea> startCompositionAreas2,
        DynamicBuffer<NetCompositionArea> endCompositionAreas2,
        float3 origin)
      {
        Bounds2 intersection;
        intersection.min = (float2) float.MaxValue;
        intersection.max = (float2) float.MinValue;
        Bounds1 bounds;
        bounds.min = float.MaxValue;
        bounds.max = float.MinValue;
        if ((this.m_PrefabObjectGeometryData.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
        {
          if ((this.m_PrefabObjectGeometryData.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
          {
            Circle2 circle2 = new Circle2(this.m_PrefabObjectGeometryData.m_LegSize.x * 0.5f, (this.m_Transform.m_Position - origin).xz);
            if (CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, edgeCollisionMask2) && Game.Net.ValidationHelpers.Intersect(edgeData2, this.m_TopLevelEntity, edgeGeometryData2, -origin.xz, circle2, this.m_ObjectBounds.xz, edgeCompositionData2, edgeCompositionAreas2, ref intersection))
            {
              error.m_ErrorType = ErrorType.OverlapExisting;
              bounds |= MathUtils.Center(edgeGeometryData2.m_Bounds.y & this.m_ObjectBounds.y);
            }
            if (CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, startCollisionMask2) && Game.Net.ValidationHelpers.Intersect(edgeData2.m_Start, this.m_TopLevelEntity, startNodeGeometryData2.m_Geometry, -origin.xz, circle2, this.m_ObjectBounds.xz, startCompositionData2, startCompositionAreas2, ref intersection))
            {
              error.m_ErrorType = ErrorType.OverlapExisting;
              bounds |= MathUtils.Center(startNodeGeometryData2.m_Geometry.m_Bounds.y & this.m_ObjectBounds.y);
            }
            if (CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, endCollisionMask2) && Game.Net.ValidationHelpers.Intersect(edgeData2.m_End, this.m_TopLevelEntity, endNodeGeometryData2.m_Geometry, -origin.xz, circle2, this.m_ObjectBounds.xz, endCompositionData2, endCompositionAreas2, ref intersection))
            {
              error.m_ErrorType = ErrorType.OverlapExisting;
              bounds |= MathUtils.Center(endNodeGeometryData2.m_Geometry.m_Bounds.y & this.m_ObjectBounds.y);
            }
          }
          else if ((this.m_PrefabObjectGeometryData.m_Flags & GeometryFlags.IgnoreLegCollision) == GeometryFlags.None)
          {
            Quad2 xz = ObjectUtils.CalculateBaseCorners(this.m_Transform.m_Position - origin, this.m_Transform.m_Rotation, new Bounds3()
            {
              min = {
                xz = this.m_PrefabObjectGeometryData.m_LegSize.xz * -0.5f
              },
              max = {
                xz = this.m_PrefabObjectGeometryData.m_LegSize.xz * 0.5f
              }
            }).xz;
            if (CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, edgeCollisionMask2) && Game.Net.ValidationHelpers.Intersect(edgeData2, this.m_TopLevelEntity, edgeGeometryData2, -origin.xz, xz, this.m_ObjectBounds.xz, edgeCompositionData2, edgeCompositionAreas2, ref intersection))
            {
              error.m_ErrorType = ErrorType.OverlapExisting;
              bounds |= MathUtils.Center(edgeGeometryData2.m_Bounds.y & this.m_ObjectBounds.y);
            }
            if (CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, startCollisionMask2) && Game.Net.ValidationHelpers.Intersect(edgeData2.m_Start, this.m_TopLevelEntity, startNodeGeometryData2.m_Geometry, -origin.xz, xz, this.m_ObjectBounds.xz, startCompositionData2, startCompositionAreas2, ref intersection))
            {
              error.m_ErrorType = ErrorType.OverlapExisting;
              bounds |= MathUtils.Center(startNodeGeometryData2.m_Geometry.m_Bounds.y & this.m_ObjectBounds.y);
            }
            if (CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, endCollisionMask2) && Game.Net.ValidationHelpers.Intersect(edgeData2.m_End, this.m_TopLevelEntity, endNodeGeometryData2.m_Geometry, -origin.xz, xz, this.m_ObjectBounds.xz, endCompositionData2, endCompositionAreas2, ref intersection))
            {
              error.m_ErrorType = ErrorType.OverlapExisting;
              bounds |= MathUtils.Center(endNodeGeometryData2.m_Geometry.m_Bounds.y & this.m_ObjectBounds.y);
            }
          }
        }
        else if ((this.m_PrefabObjectGeometryData.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
        {
          Circle2 circle2 = new Circle2(this.m_PrefabObjectGeometryData.m_Size.x * 0.5f, (this.m_Transform.m_Position - origin).xz);
          if (CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, edgeCollisionMask2) && Game.Net.ValidationHelpers.Intersect(edgeData2, this.m_TopLevelEntity, edgeGeometryData2, -origin.xz, circle2, this.m_ObjectBounds.xz, edgeCompositionData2, edgeCompositionAreas2, ref intersection))
          {
            error.m_ErrorType = ErrorType.OverlapExisting;
            bounds |= MathUtils.Center(edgeGeometryData2.m_Bounds.y & this.m_ObjectBounds.y);
          }
          if (CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, startCollisionMask2) && Game.Net.ValidationHelpers.Intersect(edgeData2.m_Start, this.m_TopLevelEntity, startNodeGeometryData2.m_Geometry, -origin.xz, circle2, this.m_ObjectBounds.xz, startCompositionData2, startCompositionAreas2, ref intersection))
          {
            error.m_ErrorType = ErrorType.OverlapExisting;
            bounds |= MathUtils.Center(startNodeGeometryData2.m_Geometry.m_Bounds.y & this.m_ObjectBounds.y);
          }
          if (CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, endCollisionMask2) && Game.Net.ValidationHelpers.Intersect(edgeData2.m_End, this.m_TopLevelEntity, endNodeGeometryData2.m_Geometry, -origin.xz, circle2, this.m_ObjectBounds.xz, endCompositionData2, endCompositionAreas2, ref intersection))
          {
            error.m_ErrorType = ErrorType.OverlapExisting;
            bounds |= MathUtils.Center(endNodeGeometryData2.m_Geometry.m_Bounds.y & this.m_ObjectBounds.y);
          }
        }
        else
        {
          Quad2 xz = ObjectUtils.CalculateBaseCorners(this.m_Transform.m_Position - origin, this.m_Transform.m_Rotation, this.m_PrefabObjectGeometryData.m_Bounds).xz;
          if (CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, edgeCollisionMask2) && Game.Net.ValidationHelpers.Intersect(edgeData2, this.m_TopLevelEntity, edgeGeometryData2, -origin.xz, xz, this.m_ObjectBounds.xz, edgeCompositionData2, edgeCompositionAreas2, ref intersection))
          {
            error.m_ErrorType = ErrorType.OverlapExisting;
            bounds |= MathUtils.Center(edgeGeometryData2.m_Bounds.y & this.m_ObjectBounds.y);
          }
          if (CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, startCollisionMask2) && Game.Net.ValidationHelpers.Intersect(edgeData2.m_Start, this.m_TopLevelEntity, startNodeGeometryData2.m_Geometry, -origin.xz, xz, this.m_ObjectBounds.xz, startCompositionData2, startCompositionAreas2, ref intersection))
          {
            error.m_ErrorType = ErrorType.OverlapExisting;
            bounds |= MathUtils.Center(startNodeGeometryData2.m_Geometry.m_Bounds.y & this.m_ObjectBounds.y);
          }
          if (CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, endCollisionMask2) && Game.Net.ValidationHelpers.Intersect(edgeData2.m_End, this.m_TopLevelEntity, endNodeGeometryData2.m_Geometry, -origin.xz, xz, this.m_ObjectBounds.xz, endCompositionData2, endCompositionAreas2, ref intersection))
          {
            error.m_ErrorType = ErrorType.OverlapExisting;
            bounds |= MathUtils.Center(endNodeGeometryData2.m_Geometry.m_Bounds.y & this.m_ObjectBounds.y);
          }
        }
        if (error.m_ErrorType == ErrorType.None)
          return;
        error.m_Position.xz = origin.xz + MathUtils.Center(intersection);
        error.m_Position.y = MathUtils.Center(bounds);
      }
    }

    private struct AreaIterator : 
      INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
    {
      public Entity m_ObjectEntity;
      public Bounds3 m_ObjectBounds;
      public bool m_IgnoreCollisions;
      public bool m_IgnoreProtectedAreas;
      public bool m_Optional;
      public bool m_EditorMode;
      public Transform m_TransformData;
      public CollisionMask m_CollisionMask;
      public ObjectGeometryData m_PrefabObjectGeometryData;
      public ValidationSystem.EntityData m_Data;
      public NativeQueue<ErrorData>.ParallelWriter m_ErrorQueue;

      public bool Intersect(QuadTreeBoundsXZ bounds)
      {
        return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_ObjectBounds.xz);
      }

      public void Iterate(QuadTreeBoundsXZ bounds, AreaSearchItem areaItem2)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_ObjectBounds.xz) || this.m_Data.m_Hidden.HasComponent(areaItem2.m_Area) || (this.m_Data.m_Area[areaItem2.m_Area].m_Flags & AreaFlags.Slave) != (AreaFlags) 0)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        AreaGeometryData areaGeometryData = this.m_Data.m_PrefabAreaGeometry[this.m_Data.m_PrefabRef[areaItem2.m_Area].m_Prefab];
        // ISSUE: reference to a compiler-generated field
        AreaUtils.SetCollisionFlags(ref areaGeometryData, !this.m_EditorMode || this.m_Data.m_Owner.HasComponent(areaItem2.m_Area));
        if ((areaGeometryData.m_Flags & (Game.Areas.GeometryFlags.PhysicalGeometry | Game.Areas.GeometryFlags.ProtectedArea)) == (Game.Areas.GeometryFlags) 0)
          return;
        if ((areaGeometryData.m_Flags & Game.Areas.GeometryFlags.ProtectedArea) != (Game.Areas.GeometryFlags) 0)
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_Data.m_Native.HasComponent(areaItem2.m_Area) || this.m_IgnoreProtectedAreas)
            return;
        }
        else if (this.m_IgnoreCollisions)
          return;
        CollisionMask collisionMask = AreaUtils.GetCollisionMask(areaGeometryData);
        if ((this.m_CollisionMask & collisionMask) == (CollisionMask) 0)
          return;
        ErrorType errorType = areaGeometryData.m_Type == AreaType.MapTile ? ErrorType.ExceedsCityLimits : ErrorType.OverlapExisting;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<Game.Areas.Node> areaNode = this.m_Data.m_AreaNodes[areaItem2.m_Area];
        // ISSUE: reference to a compiler-generated field
        Triangle triangle1 = this.m_Data.m_AreaTriangles[areaItem2.m_Area][areaItem2.m_Triangle];
        Triangle triangle2 = triangle1;
        Triangle3 triangle3 = AreaUtils.GetTriangle3(areaNode, triangle2);
        ErrorData errorData = new ErrorData();
        if (areaGeometryData.m_Type != AreaType.MapTile && ((this.m_CollisionMask & CollisionMask.OnGround) == (CollisionMask) 0 || MathUtils.Intersect(bounds.m_Bounds, this.m_ObjectBounds)))
        {
          Bounds1 heightRange = triangle1.m_HeightRange;
          heightRange.max += areaGeometryData.m_MaxHeight;
          float3 float3 = math.mul(math.inverse(this.m_TransformData.m_Rotation), this.m_TransformData.m_Position);
          Bounds3 bounds1 = this.m_PrefabObjectGeometryData.m_Bounds;
          if ((this.m_PrefabObjectGeometryData.m_Flags & GeometryFlags.IgnoreBottomCollision) != GeometryFlags.None)
            bounds1.min.y = math.max(bounds1.min.y, 0.0f);
          if ((this.m_PrefabObjectGeometryData.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
          {
            if ((this.m_PrefabObjectGeometryData.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
            {
              Bounds3 intersection1;
              Bounds3 intersection2;
              if (Game.Net.ValidationHelpers.TriangleCylinderIntersect(triangle3, new Cylinder3()
              {
                circle = new Circle2(this.m_PrefabObjectGeometryData.m_LegSize.x * 0.5f, float3.xz),
                height = new Bounds1(bounds1.min.y, this.m_PrefabObjectGeometryData.m_LegSize.y) + float3.y,
                rotation = this.m_TransformData.m_Rotation
              }, out intersection1, out intersection2))
              {
                Bounds3 bounds2 = Game.Net.ValidationHelpers.SetHeightRange(intersection1, heightRange);
                Bounds3 intersection;
                if (MathUtils.Intersect(intersection2, bounds2, out intersection))
                {
                  errorData.m_Position = MathUtils.Center(intersection);
                  errorData.m_ErrorType = errorType;
                }
              }
            }
            else if ((this.m_PrefabObjectGeometryData.m_Flags & GeometryFlags.IgnoreLegCollision) == GeometryFlags.None)
            {
              bounds1.min.xz = this.m_PrefabObjectGeometryData.m_LegSize.xz * -0.5f;
              bounds1.max.xz = this.m_PrefabObjectGeometryData.m_LegSize.xz * 0.5f;
              Bounds3 intersection1;
              Bounds3 intersection2;
              if (Game.Net.ValidationHelpers.QuadTriangleIntersect(ObjectUtils.CalculateBaseCorners(this.m_TransformData.m_Position, this.m_TransformData.m_Rotation, bounds1), triangle3, out intersection1, out intersection2))
              {
                intersection1 = Game.Net.ValidationHelpers.SetHeightRange(intersection1, bounds1.y);
                intersection2 = Game.Net.ValidationHelpers.SetHeightRange(intersection2, heightRange);
                Bounds3 intersection;
                if (MathUtils.Intersect(intersection1, intersection2, out intersection))
                {
                  errorData.m_Position = MathUtils.Center(intersection);
                  errorData.m_ErrorType = errorType;
                }
              }
            }
            bounds1.min.y = this.m_PrefabObjectGeometryData.m_LegSize.y;
          }
          if ((this.m_PrefabObjectGeometryData.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
          {
            Bounds3 intersection1;
            Bounds3 intersection2;
            if (Game.Net.ValidationHelpers.TriangleCylinderIntersect(triangle3, new Cylinder3()
            {
              circle = new Circle2(this.m_PrefabObjectGeometryData.m_Size.x * 0.5f, float3.xz),
              height = new Bounds1(bounds1.min.y, bounds1.max.y) + float3.y,
              rotation = this.m_TransformData.m_Rotation
            }, out intersection1, out intersection2))
            {
              Bounds3 bounds2 = Game.Net.ValidationHelpers.SetHeightRange(intersection1, heightRange);
              Bounds3 intersection;
              if (MathUtils.Intersect(intersection2, bounds2, out intersection))
              {
                errorData.m_Position = MathUtils.Center(intersection);
                errorData.m_ErrorType = errorType;
              }
            }
          }
          else
          {
            Bounds3 intersection1;
            Bounds3 intersection2;
            if (Game.Net.ValidationHelpers.QuadTriangleIntersect(ObjectUtils.CalculateBaseCorners(this.m_TransformData.m_Position, this.m_TransformData.m_Rotation, this.m_PrefabObjectGeometryData.m_Bounds), triangle3, out intersection1, out intersection2))
            {
              intersection1 = Game.Net.ValidationHelpers.SetHeightRange(intersection1, bounds1.y);
              Bounds3 bounds2 = Game.Net.ValidationHelpers.SetHeightRange(intersection2, heightRange);
              Bounds3 intersection;
              if (MathUtils.Intersect(intersection1, bounds2, out intersection))
              {
                errorData.m_Position = MathUtils.Center(intersection);
                errorData.m_ErrorType = errorType;
              }
            }
          }
        }
        if (areaGeometryData.m_Type == AreaType.MapTile || errorData.m_ErrorType == ErrorType.None && CommonUtils.ExclusiveGroundCollision(this.m_CollisionMask, collisionMask))
        {
          if (areaGeometryData.m_Type != AreaType.MapTile && (this.m_PrefabObjectGeometryData.m_Flags & GeometryFlags.Standing) != GeometryFlags.None)
          {
            if ((this.m_PrefabObjectGeometryData.m_Flags & (GeometryFlags.CircularLeg | GeometryFlags.IgnoreLegCollision)) == GeometryFlags.CircularLeg)
            {
              Circle2 circle = new Circle2(this.m_PrefabObjectGeometryData.m_LegSize.x * 0.5f, this.m_TransformData.m_Position.xz);
              if (MathUtils.Intersect(triangle3.xz, circle))
              {
                errorData.m_Position = MathUtils.Center(this.m_ObjectBounds & bounds.m_Bounds);
                errorData.m_ErrorType = errorType;
              }
            }
            else if ((this.m_PrefabObjectGeometryData.m_Flags & GeometryFlags.IgnoreLegCollision) == GeometryFlags.None)
            {
              if (MathUtils.Intersect(ObjectUtils.CalculateBaseCorners(this.m_TransformData.m_Position, this.m_TransformData.m_Rotation, new Bounds3()
              {
                min = {
                  xz = this.m_PrefabObjectGeometryData.m_LegSize.xz * -0.5f
                },
                max = {
                  xz = this.m_PrefabObjectGeometryData.m_LegSize.xz * 0.5f
                }
              }).xz, triangle3.xz))
              {
                errorData.m_Position = MathUtils.Center(this.m_ObjectBounds & bounds.m_Bounds);
                errorData.m_ErrorType = errorType;
              }
            }
          }
          else if ((this.m_PrefabObjectGeometryData.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
          {
            Circle2 circle = new Circle2(this.m_PrefabObjectGeometryData.m_Size.x * 0.5f, this.m_TransformData.m_Position.xz);
            if (MathUtils.Intersect(triangle3.xz, circle))
            {
              errorData.m_Position = MathUtils.Center(this.m_ObjectBounds & bounds.m_Bounds);
              errorData.m_ErrorType = errorType;
            }
          }
          else if (MathUtils.Intersect(ObjectUtils.CalculateBaseCorners(this.m_TransformData.m_Position, this.m_TransformData.m_Rotation, this.m_PrefabObjectGeometryData.m_Bounds).xz, triangle3.xz))
          {
            errorData.m_Position = MathUtils.Center(this.m_ObjectBounds & bounds.m_Bounds);
            errorData.m_ErrorType = errorType;
          }
        }
        if (errorData.m_ErrorType == ErrorType.None)
          return;
        errorData.m_Position.y = MathUtils.Clamp(errorData.m_Position.y, this.m_ObjectBounds.y);
        if (this.m_Optional && errorType == ErrorType.OverlapExisting)
        {
          errorData.m_ErrorSeverity = ErrorSeverity.Override;
          errorData.m_TempEntity = this.m_ObjectEntity;
        }
        else
        {
          errorData.m_ErrorSeverity = ErrorSeverity.Error;
          errorData.m_TempEntity = this.m_ObjectEntity;
          errorData.m_PermanentEntity = areaItem2.m_Area;
        }
        this.m_ErrorQueue.Enqueue(errorData);
      }
    }
  }
}
