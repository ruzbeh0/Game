// Decompiled with JetBrains decompiler
// Type: Game.Objects.ObjectUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Buildings;
using Game.Common;
using Game.Economy;
using Game.Pathfind;
using Game.Prefabs;
using Game.Rendering;
using Game.Simulation;
using Game.Tools;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Objects
{
  public static class ObjectUtils
  {
    public const float MAX_SPAWN_LOCATION_CONNECTION_DISTANCE = 32f;
    public const float MIN_TREE_WOOD_RESOURCE = 1f;
    public const float MAX_TREE_AGE = 40f;
    public const float TREE_AGE_PHASE_CHILD = 0.1f;
    public const float TREE_AGE_PHASE_TEEN = 0.15f;
    public const float TREE_AGE_PHASE_ADULT = 0.35f;
    public const float TREE_AGE_PHASE_ELDERLY = 0.35f;
    public const float TREE_AGE_PHASE_DEAD = 0.05f;
    public const float TREE_WOOD_GROWTH_CHILD = 0.2f;
    public const float TREE_WOOD_GROWTH_TEEN = 0.5f;
    public const float TREE_WOOD_GROWTH_ADULT = 0.3f;

    public static float3 GetSize(Bounds3 bounds)
    {
      return new float3()
      {
        xz = math.max(-bounds.min, bounds.max).xz * 2f,
        y = bounds.max.y
      };
    }

    public static Bounds3 CalculateBounds(
      float3 position,
      quaternion rotation,
      ObjectGeometryData geometryData)
    {
      if ((geometryData.m_Flags & GeometryFlags.Circular) == GeometryFlags.None)
        return ObjectUtils.CalculateBounds(position, rotation, geometryData.m_Bounds);
      float num = geometryData.m_Size.x * 0.5f;
      return new Bounds3(position + new float3(-num, geometryData.m_Bounds.min.y, -num), position + new float3(num, geometryData.m_Bounds.max.y, num));
    }

    public static Bounds3 GetBounds(ObjectGeometryData geometryData)
    {
      Bounds3 bounds = geometryData.m_Bounds;
      if ((geometryData.m_Flags & GeometryFlags.Circular) != GeometryFlags.None)
      {
        bounds.min.xz = geometryData.m_Size.xz * -0.5f;
        bounds.max.xz = geometryData.m_Size.xz * 0.5f;
      }
      return bounds;
    }

    public static Bounds3 GetBounds(
      Stack stack,
      ObjectGeometryData geometryData,
      StackData stackData)
    {
      Bounds3 bounds = ObjectUtils.GetBounds(geometryData);
      switch (stackData.m_Direction)
      {
        case StackDirection.Right:
          bounds.x = stack.m_Range;
          break;
        case StackDirection.Up:
          bounds.y = stack.m_Range;
          break;
        case StackDirection.Forward:
          bounds.z = stack.m_Range;
          break;
      }
      return bounds;
    }

    public static Bounds3 CalculateBounds(
      float3 position,
      quaternion rotation,
      Stack stack,
      ObjectGeometryData geometryData,
      StackData stackData)
    {
      Line3.Segment segment;
      switch (stackData.m_Direction)
      {
        case StackDirection.Right:
          segment.a = ObjectUtils.LocalToWorld(position, rotation, new float3(stack.m_Range.min, 0.0f, 0.0f));
          segment.b = ObjectUtils.LocalToWorld(position, rotation, new float3(stack.m_Range.max, 0.0f, 0.0f));
          break;
        case StackDirection.Up:
          segment.a = ObjectUtils.LocalToWorld(position, rotation, new float3(0.0f, stack.m_Range.min, 0.0f));
          segment.b = ObjectUtils.LocalToWorld(position, rotation, new float3(0.0f, stack.m_Range.max, 0.0f));
          break;
        case StackDirection.Forward:
          segment.a = ObjectUtils.LocalToWorld(position, rotation, new float3(0.0f, 0.0f, stack.m_Range.min));
          segment.b = ObjectUtils.LocalToWorld(position, rotation, new float3(0.0f, 0.0f, stack.m_Range.max));
          break;
        default:
          return ObjectUtils.CalculateBounds(position, rotation, geometryData);
      }
      if ((geometryData.m_Flags & GeometryFlags.Circular) == GeometryFlags.None)
        return ObjectUtils.CalculateBounds(segment, rotation, geometryData.m_Bounds);
      float num = geometryData.m_Size.x * 0.5f;
      return new Bounds3(MathUtils.Min(segment) + new float3(-num, geometryData.m_Bounds.min.y, -num), MathUtils.Max(segment) + new float3(num, geometryData.m_Bounds.max.y, num));
    }

    public static Bounds3 CalculateBounds(float3 position, quaternion rotation, Bounds3 bounds)
    {
      float3 float3_1 = math.mul(rotation, new float3(1f, 0.0f, 0.0f));
      float3 float3_2 = math.mul(rotation, new float3(0.0f, 1f, 0.0f));
      float3 float3_3 = math.mul(rotation, new float3(0.0f, 0.0f, 1f));
      float3 x1 = float3_1 * bounds.min.x;
      float3 y1 = float3_1 * bounds.max.x;
      float3 x2 = float3_2 * bounds.min.y;
      float3 y2 = float3_2 * bounds.max.y;
      float3 x3 = float3_3 * bounds.min.z;
      float3 y3 = float3_3 * bounds.max.z;
      return new Bounds3()
      {
        min = position + math.min(x1, y1) + math.min(x2, y2) + math.min(x3, y3),
        max = position + math.max(x1, y1) + math.max(x2, y2) + math.max(x3, y3)
      };
    }

    public static Bounds3 CalculateBounds(
      Line3.Segment positionRange,
      quaternion rotation,
      Bounds3 bounds)
    {
      float3 float3_1 = math.mul(rotation, new float3(1f, 0.0f, 0.0f));
      float3 float3_2 = math.mul(rotation, new float3(0.0f, 1f, 0.0f));
      float3 float3_3 = math.mul(rotation, new float3(0.0f, 0.0f, 1f));
      float3 x1 = float3_1 * bounds.min.x;
      float3 y1 = float3_1 * bounds.max.x;
      float3 x2 = float3_2 * bounds.min.y;
      float3 y2 = float3_2 * bounds.max.y;
      float3 x3 = float3_3 * bounds.min.z;
      float3 y3 = float3_3 * bounds.max.z;
      return new Bounds3()
      {
        min = MathUtils.Min(positionRange) + math.min(x1, y1) + math.min(x2, y2) + math.min(x3, y3),
        max = MathUtils.Max(positionRange) + math.max(x1, y1) + math.max(x2, y2) + math.max(x3, y3)
      };
    }

    public static Quad3 CalculateBaseCorners(float3 position, quaternion rotation, Bounds3 bounds)
    {
      float3 float3_1 = math.mul(rotation, new float3(0.0f, 0.0f, 1f));
      float3 float3_2 = math.mul(rotation, new float3(1f, 0.0f, 0.0f));
      float3 float3_3 = position + float3_1 * bounds.max.z;
      float3 float3_4 = position + float3_1 * bounds.min.z;
      float3 float3_5 = float3_2 * bounds.max.x;
      float3 float3_6 = float3_2 * bounds.min.x;
      return new Quad3(float3_3 + float3_6, float3_3 + float3_5, float3_4 + float3_5, float3_4 + float3_6);
    }

    public static Quad3 CalculateBaseCorners(float3 position, quaternion rotation, float2 size)
    {
      size *= 0.5f;
      float3 float3_1 = math.mul(rotation, new float3(0.0f, 0.0f, 1f)) * size.y;
      float3 float3_2 = math.mul(rotation, new float3(1f, 0.0f, 0.0f)) * size.x;
      float3 float3_3 = position + float3_1;
      float3 float3_4 = position - float3_1;
      return new Quad3(float3_3 - float3_2, float3_3 + float3_2, float3_4 + float3_2, float3_4 - float3_2);
    }

    public static float3 CalculatePointVelocity(float3 offset, Moving moving)
    {
      return moving.m_Velocity + math.cross(moving.m_AngularVelocity, offset);
    }

    public static float3 CalculateMomentOfInertia(quaternion rotation, float3 size)
    {
      size *= 0.5f;
      size *= size;
      float3 float3_1 = math.abs(math.rotate(rotation, new float3(size.x, 0.0f, 0.0f)));
      float3 float3_2 = math.abs(math.rotate(rotation, new float3(0.0f, size.y, 0.0f)));
      float3 float3_3 = math.abs(math.rotate(rotation, new float3(0.0f, 0.0f, size.z)));
      float3 float3_4 = float3_2;
      float3 float3_5 = float3_1 + float3_4 + float3_3;
      return float3_5.yzx + float3_5.zxy;
    }

    public static Transform InverseTransform(Transform transform)
    {
      Transform transform1;
      transform1.m_Position = -transform.m_Position;
      transform1.m_Rotation = math.inverse(transform.m_Rotation);
      return transform1;
    }

    public static float3 LocalToWorld(Transform transform, float3 position)
    {
      return transform.m_Position + math.mul(transform.m_Rotation, position);
    }

    public static float3 LocalToWorld(
      float3 transformPosition,
      quaternion transformRotation,
      float3 position)
    {
      return transformPosition + math.mul(transformRotation, position);
    }

    public static Bezier4x3 LocalToWorld(
      float3 transformPosition,
      quaternion transformRotation,
      Bezier4x3 curve)
    {
      Bezier4x3 world;
      world.a = ObjectUtils.LocalToWorld(transformPosition, transformRotation, curve.a);
      world.b = ObjectUtils.LocalToWorld(transformPosition, transformRotation, curve.b);
      world.c = ObjectUtils.LocalToWorld(transformPosition, transformRotation, curve.c);
      world.d = ObjectUtils.LocalToWorld(transformPosition, transformRotation, curve.d);
      return world;
    }

    public static Transform LocalToWorld(Transform transform, float3 position, quaternion rotation)
    {
      Transform world;
      world.m_Position = transform.m_Position + math.mul(transform.m_Rotation, position);
      world.m_Rotation = math.mul(transform.m_Rotation, rotation);
      return world;
    }

    public static InterpolatedTransform LocalToWorld(
      InterpolatedTransform transform,
      float3 position,
      quaternion rotation)
    {
      return transform with
      {
        m_Position = transform.m_Position + math.mul(transform.m_Rotation, position),
        m_Rotation = math.mul(transform.m_Rotation, rotation)
      };
    }

    public static Transform LocalToWorld(Transform parentTransform, Transform transform)
    {
      Transform world;
      world.m_Position = parentTransform.m_Position + math.mul(parentTransform.m_Rotation, transform.m_Position);
      world.m_Rotation = math.mul(parentTransform.m_Rotation, transform.m_Rotation);
      return world;
    }

    public static Transform WorldToLocal(Transform inverseParentTransform, Transform transform)
    {
      Transform local;
      local.m_Position = math.mul(inverseParentTransform.m_Rotation, transform.m_Position + inverseParentTransform.m_Position);
      local.m_Rotation = math.mul(inverseParentTransform.m_Rotation, transform.m_Rotation);
      return local;
    }

    public static float3 WorldToLocal(Transform inverseParentTransform, float3 position)
    {
      return math.mul(inverseParentTransform.m_Rotation, position + inverseParentTransform.m_Position);
    }

    public static CollisionMask GetCollisionMask(
      ObjectGeometryData geometryData,
      Elevation elevation,
      bool ignoreMarkers)
    {
      if ((geometryData.m_Flags & GeometryFlags.Marker) != 0 & ignoreMarkers)
        return (CollisionMask) 0;
      CollisionMask collisionMask1 = (CollisionMask) 0;
      if ((geometryData.m_Flags & GeometryFlags.ExclusiveGround) != GeometryFlags.None)
        collisionMask1 |= CollisionMask.OnGround | CollisionMask.ExclusiveGround;
      CollisionMask collisionMask2;
      if ((double) elevation.m_Elevation < 0.0)
      {
        collisionMask2 = collisionMask1 | CollisionMask.Underground;
        if ((elevation.m_Flags & ElevationFlags.Lowered) != (ElevationFlags) 0)
          collisionMask2 |= CollisionMask.Overground;
      }
      else
        collisionMask2 = collisionMask1 | CollisionMask.Overground;
      return collisionMask2;
    }

    public static CollisionMask GetCollisionMask(
      ObjectGeometryData geometryData,
      bool ignoreMarkers)
    {
      if ((geometryData.m_Flags & GeometryFlags.Marker) != 0 & ignoreMarkers)
        return (CollisionMask) 0;
      CollisionMask collisionMask = (CollisionMask) 0;
      if ((geometryData.m_Flags & (GeometryFlags.ExclusiveGround | GeometryFlags.BaseCollision)) != GeometryFlags.None)
        collisionMask |= CollisionMask.ExclusiveGround;
      return collisionMask | CollisionMask.OnGround | CollisionMask.Overground;
    }

    public static int GetContructionCost(
      int constructionCost,
      Tree tree,
      in EconomyParameterData economyParameterData)
    {
      switch (tree.m_State & (TreeState.Teen | TreeState.Adult | TreeState.Elderly | TreeState.Dead | TreeState.Stump))
      {
        case TreeState.Teen:
          return constructionCost * economyParameterData.m_TreeCostMultipliers.x;
        case TreeState.Adult:
          return constructionCost * economyParameterData.m_TreeCostMultipliers.y;
        case TreeState.Elderly:
          return constructionCost * economyParameterData.m_TreeCostMultipliers.z;
        default:
          return constructionCost;
      }
    }

    public static int GetRelocationCost(int constructionCost)
    {
      int a = (constructionCost + 1000) / 2000 * 500;
      return math.min(math.select(a, 500, a == 0 & constructionCost > 0), constructionCost);
    }

    public static int GetRelocationCost(
      int constructionCost,
      Recent recent,
      uint simulationFrame,
      EconomyParameterData economyParameterData)
    {
      int refundAmount = ObjectUtils.GetRefundAmount(recent, simulationFrame, economyParameterData);
      constructionCost = math.max(constructionCost / 4, constructionCost - refundAmount);
      return ObjectUtils.GetRelocationCost(constructionCost);
    }

    public static int GetRebuildCost(int constructionCost)
    {
      int a = (constructionCost + 500) / 1000 * 500;
      return math.min(math.select(a, 500, a == 0 & constructionCost > 0), constructionCost);
    }

    public static int GetRebuildCost(
      int constructionCost,
      Recent recent,
      uint simulationFrame,
      EconomyParameterData economyParameterData)
    {
      int refundAmount = ObjectUtils.GetRefundAmount(recent, simulationFrame, economyParameterData);
      constructionCost = math.max(constructionCost / 4, constructionCost - refundAmount);
      return ObjectUtils.GetRebuildCost(constructionCost);
    }

    public static int GetUpgradeCost(int constructionCost, int originalCost)
    {
      return math.max(0, constructionCost - originalCost);
    }

    public static int GetUpgradeCost(
      int constructionCost,
      int originalCost,
      Recent recent,
      uint simulationFrame,
      EconomyParameterData economyParameterData)
    {
      if (constructionCost >= originalCost)
        return ObjectUtils.GetUpgradeCost(constructionCost, originalCost);
      recent.m_ModificationCost = math.min(recent.m_ModificationCost, originalCost - constructionCost);
      return -ObjectUtils.GetRefundAmount(recent, simulationFrame, economyParameterData);
    }

    public static int GetRefundAmount(
      Recent recent,
      uint simulationFrame,
      EconomyParameterData economyParameterData)
    {
      if ((double) simulationFrame < (double) recent.m_ModificationFrame + 262144.0 * (double) economyParameterData.m_BuildRefundTimeRange.x)
        return (int) ((double) recent.m_ModificationCost * (double) economyParameterData.m_BuildRefundPercentage.x);
      if ((double) simulationFrame < (double) recent.m_ModificationFrame + 262144.0 * (double) economyParameterData.m_BuildRefundTimeRange.y)
        return (int) ((double) recent.m_ModificationCost * (double) economyParameterData.m_BuildRefundPercentage.y);
      return (double) simulationFrame < (double) recent.m_ModificationFrame + 262144.0 * (double) economyParameterData.m_BuildRefundTimeRange.z ? (int) ((double) recent.m_ModificationCost * (double) economyParameterData.m_BuildRefundPercentage.z) : 0;
    }

    public static float CalculateWoodAmount(
      Tree tree,
      Plant plant,
      Damaged damaged,
      TreeData treeData)
    {
      float num;
      switch (tree.m_State & (TreeState.Teen | TreeState.Adult | TreeState.Elderly | TreeState.Dead | TreeState.Stump))
      {
        case TreeState.Teen:
          num = math.lerp(0.2f, 0.7f, (float) tree.m_Growth * (1f / 256f)) * treeData.m_WoodAmount;
          break;
        case TreeState.Adult:
          num = math.lerp(0.7f, 1f, (float) tree.m_Growth * (1f / 256f)) * treeData.m_WoodAmount;
          break;
        case TreeState.Elderly:
          num = treeData.m_WoodAmount;
          break;
        case TreeState.Dead:
        case TreeState.Stump:
          return 0.0f;
        default:
          num = math.lerp(0.0f, 0.2f, (float) tree.m_Growth * (1f / 256f)) * treeData.m_WoodAmount;
          break;
      }
      return (float) ((double) num * (1.0 - (double) plant.m_Pollution) * (1.0 - (double) ObjectUtils.GetTotalDamage(damaged)));
    }

    public static float CalculateGrowthRate(Tree tree, Plant plant, TreeData treeData)
    {
      float num;
      switch (tree.m_State & (TreeState.Teen | TreeState.Adult | TreeState.Elderly | TreeState.Dead | TreeState.Stump))
      {
        case TreeState.Teen:
          num = 0.025f * treeData.m_WoodAmount;
          break;
        case TreeState.Adult:
          num = 0.021428572f * treeData.m_WoodAmount;
          break;
        case TreeState.Elderly:
        case TreeState.Dead:
        case TreeState.Stump:
          return 0.0f;
        default:
          num = 0.05f * treeData.m_WoodAmount;
          break;
      }
      return num * (1f - plant.m_Pollution);
    }

    public static Tree InitializeTreeState(float age)
    {
      Tree tree = new Tree();
      if ((double) age < 0.10000000149011612)
        tree.m_Growth = (byte) math.clamp(Mathf.FloorToInt(age * 2560f), 0, (int) byte.MaxValue);
      else if ((double) age < 0.25)
      {
        tree.m_State = TreeState.Teen;
        tree.m_Growth = (byte) math.clamp(Mathf.FloorToInt((float) (((double) age - 0.10000000149011612) * 1706.6666259765625)), 0, (int) byte.MaxValue);
      }
      else if ((double) age < 0.60000002384185791)
      {
        tree.m_State = TreeState.Adult;
        tree.m_Growth = (byte) math.clamp(Mathf.FloorToInt((float) (((double) age - 0.25) * 731.4285888671875)), 0, (int) byte.MaxValue);
      }
      else if ((double) age < 0.95000004768371582)
      {
        tree.m_State = TreeState.Elderly;
        tree.m_Growth = (byte) math.clamp(Mathf.FloorToInt((float) (((double) age - 0.60000002384185791) * 731.4285888671875)), 0, (int) byte.MaxValue);
      }
      else
      {
        tree.m_State = TreeState.Dead;
        tree.m_Growth = (byte) math.clamp(Mathf.FloorToInt((float) (((double) age - 0.949999988079071) * 5120.0)), 0, (int) byte.MaxValue);
      }
      return tree;
    }

    public static void UpdateAnimation(
      Entity prefab,
      float timeStep,
      DynamicBuffer<MeshGroup> meshGroups,
      ref BufferLookup<SubMeshGroup> subMeshGroupBuffers,
      ref BufferLookup<CharacterElement> characterElementBuffers,
      ref BufferLookup<SubMesh> subMeshBuffers,
      ref BufferLookup<Game.Prefabs.AnimationClip> animationClipBuffers,
      ref BufferLookup<AnimationMotion> animationMotionBuffers,
      AnimatedPropID oldPropID,
      AnimatedPropID newPropID,
      ActivityCondition conditions,
      ref float maxSpeed,
      ref byte activity,
      ref float3 targetPosition,
      ref float3 targetDirection,
      ref Transform transform,
      ref TransformFrame oldFrameData,
      ref TransformFrame newFrameData)
    {
      bool flag1 = newFrameData.m_Activity == (byte) 0;
      bool flag2 = oldFrameData.m_Activity == (byte) 0;
      if ((int) oldFrameData.m_Activity != (int) newFrameData.m_Activity)
      {
        byte parentActivity1 = ObjectUtils.GetParentActivity(oldFrameData.m_Activity);
        byte parentActivity2 = ObjectUtils.GetParentActivity(newFrameData.m_Activity);
        if (parentActivity1 != (byte) 0)
        {
          if ((int) parentActivity1 != (int) newFrameData.m_Activity)
            newFrameData.m_Activity = parentActivity1;
          else
            flag1 = true;
        }
        else if (parentActivity2 != (byte) 0)
        {
          if ((int) parentActivity2 != (int) oldFrameData.m_Activity)
            newFrameData.m_Activity = parentActivity2;
          else
            flag2 = true;
        }
      }
      CharacterElement characterElement;
      Game.Prefabs.AnimationClip animationClip;
      bool crossFade1;
      switch (oldFrameData.m_State)
      {
        case TransformState.Default:
          flag2 = true;
          break;
        case TransformState.Idle:
          if (newFrameData.m_State == TransformState.Idle && (int) newFrameData.m_Activity == (int) oldFrameData.m_Activity)
          {
            newFrameData.m_StateTimer = (ushort) ((uint) oldFrameData.m_StateTimer + 1U);
            return;
          }
          float stateDuration1 = ObjectUtils.GetStateDuration(prefab, TransformState.Idle, oldFrameData.m_Activity, oldPropID, conditions, meshGroups, ref subMeshGroupBuffers, ref characterElementBuffers, ref subMeshBuffers, ref animationClipBuffers, out characterElement, out animationClip, out crossFade1);
          if (animationClip.m_Playback != AnimationPlayback.RandomLoop && (double) stateDuration1 > 0.0)
          {
            float2 float2_1;
            float2_1.x = (float) oldFrameData.m_StateTimer * timeStep;
            float2_1.y = float2_1.x + timeStep;
            float2 float2_2 = math.floor(float2_1 / math.select(stateDuration1, stateDuration1 * 0.5f, animationClip.m_Playback == AnimationPlayback.HalfLoop));
            if ((double) float2_2.x > (double) float2_2.y - 0.5)
            {
              newFrameData.m_State = TransformState.Idle;
              newFrameData.m_Activity = oldFrameData.m_Activity;
              newFrameData.m_StateTimer = (ushort) ((uint) oldFrameData.m_StateTimer + 1U);
              maxSpeed = 0.0f;
              return;
            }
            break;
          }
          break;
        case TransformState.Move:
          if (newFrameData.m_State == TransformState.Move)
          {
            newFrameData.m_StateTimer = (ushort) ((uint) oldFrameData.m_StateTimer + 1U);
            return;
          }
          break;
        case TransformState.Start:
          float stateDuration2 = ObjectUtils.GetStateDuration(prefab, TransformState.Start, oldFrameData.m_Activity, oldPropID, conditions, meshGroups, ref subMeshGroupBuffers, ref characterElementBuffers, ref subMeshBuffers, ref animationClipBuffers, out characterElement, out animationClip, out crossFade1);
          float2 x1;
          x1.x = (float) oldFrameData.m_StateTimer * timeStep;
          x1.y = x1.x + timeStep;
          float2 xy1 = math.min(x1, (float2) stateDuration2);
          if (animationClip.m_MotionRange.y != animationClip.m_MotionRange.x && (double) xy1.y > (double) xy1.x && (double) stateDuration2 > 0.0)
          {
            DynamicBuffer<AnimationMotion> motions = animationMotionBuffers[characterElement.m_Style];
            ObjectUtils.ApplyRootMotion(ref transform, ref newFrameData, motions, characterElement.m_ShapeWeights, animationClip.m_MotionRange, new float3(xy1, 1f) / stateDuration2);
            targetPosition = transform.m_Position;
            targetDirection = math.forward(transform.m_Rotation);
          }
          if ((double) xy1.y < (double) stateDuration2)
          {
            newFrameData.m_State = TransformState.Start;
            newFrameData.m_Activity = oldFrameData.m_Activity;
            newFrameData.m_StateTimer = (ushort) ((uint) oldFrameData.m_StateTimer + 1U);
            maxSpeed = 0.0f;
            return;
          }
          if (animationClip.m_MotionRange.y == animationClip.m_MotionRange.x)
            ObjectUtils.ApplyRootMotion(ref transform, ref targetPosition, ref targetDirection, animationClip.m_RootOffset, animationClip.m_RootRotation);
          if ((int) newFrameData.m_Activity == (int) oldFrameData.m_Activity)
            return;
          break;
        case TransformState.End:
          float stateDuration3 = ObjectUtils.GetStateDuration(prefab, TransformState.End, oldFrameData.m_Activity, oldPropID, conditions, meshGroups, ref subMeshGroupBuffers, ref characterElementBuffers, ref subMeshBuffers, ref animationClipBuffers, out characterElement, out animationClip, out crossFade1);
          float2 x2;
          x2.x = (float) oldFrameData.m_StateTimer * timeStep;
          x2.y = x2.x + timeStep;
          float2 xy2 = math.min(x2, (float2) stateDuration3);
          if (animationClip.m_MotionRange.y != animationClip.m_MotionRange.x && (double) xy2.y > (double) xy2.x && (double) stateDuration3 > 0.0)
          {
            DynamicBuffer<AnimationMotion> motions = animationMotionBuffers[characterElement.m_Style];
            ObjectUtils.ApplyRootMotion(ref transform, ref newFrameData, motions, characterElement.m_ShapeWeights, animationClip.m_MotionRange, new float3(xy2, 1f) / stateDuration3);
            targetPosition = transform.m_Position;
            targetDirection = math.forward(transform.m_Rotation);
          }
          if ((double) xy2.y < (double) stateDuration3)
          {
            newFrameData.m_State = TransformState.End;
            newFrameData.m_Activity = oldFrameData.m_Activity;
            newFrameData.m_StateTimer = (ushort) ((uint) oldFrameData.m_StateTimer + 1U);
            maxSpeed = 0.0f;
            return;
          }
          if (animationClip.m_MotionRange.y == animationClip.m_MotionRange.x)
            ObjectUtils.ApplyRootMotion(ref transform, ref targetPosition, ref targetDirection, animationClip.m_RootOffset, animationClip.m_RootRotation);
          flag2 = true;
          break;
        case TransformState.Action:
        case TransformState.Done:
          float stateDuration4 = ObjectUtils.GetStateDuration(prefab, TransformState.Action, oldFrameData.m_Activity, oldPropID, conditions, meshGroups, ref subMeshGroupBuffers, ref characterElementBuffers, ref subMeshBuffers, ref animationClipBuffers, out characterElement, out animationClip, out crossFade1);
          float2 x3;
          x3.x = (float) oldFrameData.m_StateTimer * timeStep;
          x3.y = x3.x + timeStep;
          float2 xy3 = math.min(x3, (float2) stateDuration4);
          if (animationClip.m_MotionRange.y != animationClip.m_MotionRange.x && (double) xy3.y > (double) xy3.x && (double) stateDuration4 > 0.0)
          {
            DynamicBuffer<AnimationMotion> motions = animationMotionBuffers[characterElement.m_Style];
            ObjectUtils.ApplyRootMotion(ref transform, ref newFrameData, motions, characterElement.m_ShapeWeights, animationClip.m_MotionRange, new float3(xy3, 1f) / stateDuration4);
            if (animationClip.m_Playback != AnimationPlayback.OptionalOnce)
            {
              targetPosition = transform.m_Position;
              targetDirection = math.forward(transform.m_Rotation);
            }
          }
          if (animationClip.m_Playback != AnimationPlayback.OptionalOnce || (double) maxSpeed < 0.10000000149011612)
          {
            if ((double) xy3.y < (double) stateDuration4)
            {
              newFrameData.m_State = TransformState.Action;
              newFrameData.m_Activity = oldFrameData.m_Activity;
              newFrameData.m_StateTimer = (ushort) ((uint) oldFrameData.m_StateTimer + 1U);
              maxSpeed = 0.0f;
              return;
            }
            if ((int) newFrameData.m_Activity == (int) oldFrameData.m_Activity && newFrameData.m_Activity == (byte) 10)
            {
              newFrameData.m_State = TransformState.Done;
              newFrameData.m_Activity = oldFrameData.m_Activity;
              newFrameData.m_StateTimer = (ushort) math.min((int) ushort.MaxValue, (int) oldFrameData.m_StateTimer + 1);
              maxSpeed = 0.0f;
              return;
            }
          }
          if ((int) newFrameData.m_Activity == (int) oldFrameData.m_Activity)
          {
            targetDirection = new float3();
            activity = (byte) 0;
            newFrameData.m_Activity = activity;
          }
          if (animationClip.m_MotionRange.y == animationClip.m_MotionRange.x)
            ObjectUtils.ApplyRootMotion(ref transform, ref targetPosition, ref targetDirection, animationClip.m_RootOffset, animationClip.m_RootRotation);
          flag2 = true;
          break;
      }
      bool crossFade2;
      float stateDuration5;
      if (!flag2 && (double) (stateDuration5 = ObjectUtils.GetStateDuration(prefab, TransformState.End, oldFrameData.m_Activity, oldPropID, conditions, meshGroups, ref subMeshGroupBuffers, ref characterElementBuffers, ref subMeshBuffers, ref animationClipBuffers, out characterElement, out animationClip, out crossFade2)) > 0.0)
      {
        newFrameData.m_State = TransformState.End;
        newFrameData.m_Activity = oldFrameData.m_Activity;
      }
      else if (!flag1 && (double) (stateDuration5 = ObjectUtils.GetStateDuration(prefab, TransformState.Start, newFrameData.m_Activity, newPropID, conditions, meshGroups, ref subMeshGroupBuffers, ref characterElementBuffers, ref subMeshBuffers, ref animationClipBuffers, out characterElement, out animationClip, out crossFade2)) > 0.0)
      {
        newFrameData.m_State = TransformState.Start;
      }
      else
      {
        if ((double) (stateDuration5 = ObjectUtils.GetStateDuration(prefab, TransformState.Action, newFrameData.m_Activity, newPropID, conditions, meshGroups, ref subMeshGroupBuffers, ref characterElementBuffers, ref subMeshBuffers, ref animationClipBuffers, out characterElement, out animationClip, out crossFade2)) <= 0.0)
          return;
        newFrameData.m_State = TransformState.Action;
      }
      newFrameData.m_StateTimer = (ushort) math.select(0, 1, crossFade2);
      maxSpeed = 0.0f;
      if (!crossFade2 || animationClip.m_MotionRange.y == animationClip.m_MotionRange.x || (double) stateDuration5 <= 0.0)
        return;
      DynamicBuffer<AnimationMotion> motions1 = animationMotionBuffers[characterElement.m_Style];
      ObjectUtils.ApplyRootMotion(ref transform, ref newFrameData, motions1, characterElement.m_ShapeWeights, animationClip.m_MotionRange, new float3(0.0f, timeStep, 1f) / stateDuration5);
      if (newFrameData.m_State == TransformState.Action && animationClip.m_Playback == AnimationPlayback.OptionalOnce)
        return;
      targetPosition = transform.m_Position;
      targetDirection = math.forward(transform.m_Rotation);
    }

    public static Transform GetActivityStartPosition(
      Entity prefab,
      DynamicBuffer<MeshGroup> meshGroups,
      Transform activityTransform,
      TransformState state,
      ActivityType activityType,
      AnimatedPropID propID,
      ActivityCondition conditions,
      ref BufferLookup<SubMeshGroup> subMeshGroupBuffers,
      ref BufferLookup<CharacterElement> characterElementBuffers,
      ref BufferLookup<SubMesh> subMeshBuffers,
      ref BufferLookup<Game.Prefabs.AnimationClip> animationClipBuffers,
      ref BufferLookup<AnimationMotion> animationMotionBuffers,
      ref ObjectUtils.ActivityStartPositionCache cache)
    {
      if (activityType != cache.m_ActivityType)
      {
        cache.m_ActivityType = activityType;
        CharacterElement characterElement;
        Game.Prefabs.AnimationClip animationClip;
        float stateDuration = ObjectUtils.GetStateDuration(prefab, state, (byte) activityType, propID, conditions, meshGroups, ref subMeshGroupBuffers, ref characterElementBuffers, ref subMeshBuffers, ref animationClipBuffers, out characterElement, out animationClip, out bool _);
        if (animationClip.m_MotionRange.y != animationClip.m_MotionRange.x && (double) stateDuration > 0.0)
        {
          DynamicBuffer<AnimationMotion> motions = animationMotionBuffers[characterElement.m_Style];
          float3 rootOffset1;
          float3 rootVelocity;
          quaternion rootRotation1;
          ObjectUtils.GetRootMotion(motions, animationClip.m_MotionRange, characterElement.m_ShapeWeights, 0.0f, out rootOffset1, out rootVelocity, out rootRotation1);
          float3 rootOffset2;
          quaternion rootRotation2;
          ObjectUtils.GetRootMotion(motions, animationClip.m_MotionRange, characterElement.m_ShapeWeights, 1f, out rootOffset2, out rootVelocity, out rootRotation2);
          cache.m_RotationOffset = math.inverse(rootRotation2);
          cache.m_PositionOffset = math.mul(cache.m_RotationOffset, rootOffset1 - rootOffset2);
          cache.m_RotationOffset = math.mul(cache.m_RotationOffset, rootRotation1);
        }
        else
        {
          cache.m_PositionOffset = new float3();
          cache.m_RotationOffset = quaternion.identity;
        }
      }
      return cache.m_ActivityType != ActivityType.None ? ObjectUtils.LocalToWorld(activityTransform, cache.m_PositionOffset, cache.m_RotationOffset) : activityTransform;
    }

    private static byte GetParentActivity(byte activity)
    {
      return activity == (byte) 6 ? (byte) 5 : (byte) 0;
    }

    private static void ApplyRootMotion(
      ref Transform transform,
      ref TransformFrame newFrameData,
      DynamicBuffer<AnimationMotion> motions,
      Game.Rendering.BlendWeights weights,
      int2 motionRange,
      float3 deltaRange)
    {
      float3 rootOffset1;
      quaternion rootRotation1;
      ObjectUtils.GetRootMotion(motions, motionRange, weights, deltaRange.x, out rootOffset1, out float3 _, out rootRotation1);
      float3 rootOffset2;
      float3 rootVelocity;
      quaternion rootRotation2;
      ObjectUtils.GetRootMotion(motions, motionRange, weights, deltaRange.y, out rootOffset2, out rootVelocity, out rootRotation2);
      transform.m_Rotation = math.mul(transform.m_Rotation, math.inverse(rootRotation1));
      transform.m_Position += math.mul(transform.m_Rotation, rootOffset2 - rootOffset1);
      newFrameData.m_Velocity += math.mul(transform.m_Rotation, rootVelocity * deltaRange.z);
      transform.m_Rotation = math.normalize(math.mul(transform.m_Rotation, rootRotation2));
    }

    private static void ApplyRootMotion(
      ref Transform transform,
      ref float3 targetPosition,
      ref float3 targetDirection,
      float3 rootOffset,
      quaternion rootRotation)
    {
      if (!rootOffset.Equals(new float3()))
      {
        rootOffset = math.mul(transform.m_Rotation, rootOffset);
        transform.m_Position += rootOffset;
        targetPosition = transform.m_Position;
      }
      if (rootRotation.Equals(new quaternion()))
        return;
      transform.m_Rotation = math.mul(transform.m_Rotation, rootRotation);
      targetDirection = math.forward(transform.m_Rotation);
    }

    private static float GetStateDuration(
      Entity prefab,
      TransformState state,
      byte activity,
      AnimatedPropID propID,
      ActivityCondition conditions,
      DynamicBuffer<MeshGroup> meshGroups,
      ref BufferLookup<SubMeshGroup> subMeshGroupBuffers,
      ref BufferLookup<CharacterElement> characterElementBuffers,
      ref BufferLookup<SubMesh> subMeshBuffers,
      ref BufferLookup<Game.Prefabs.AnimationClip> animationClipBuffers,
      out CharacterElement characterElement,
      out Game.Prefabs.AnimationClip animationClip,
      out bool crossFade)
    {
      characterElement = new CharacterElement();
      animationClip = new Game.Prefabs.AnimationClip();
      crossFade = false;
      ActivityType activityType = (ActivityType) activity;
      AnimationType animationType;
      switch (state)
      {
        case TransformState.Idle:
          animationType = AnimationType.Idle;
          break;
        case TransformState.Start:
          animationType = AnimationType.Start;
          break;
        case TransformState.End:
          animationType = AnimationType.End;
          break;
        case TransformState.Action:
          animationType = AnimationType.Action;
          break;
        case TransformState.Done:
          animationType = AnimationType.Action;
          break;
        default:
          return 0.0f;
      }
      float x1 = 0.0f;
      int num1 = 0;
      DynamicBuffer<CharacterElement> bufferData1 = new DynamicBuffer<CharacterElement>();
      DynamicBuffer<SubMesh> dynamicBuffer = new DynamicBuffer<SubMesh>();
      DynamicBuffer<SubMeshGroup> bufferData2;
      if (subMeshGroupBuffers.TryGetBuffer(prefab, out bufferData2))
      {
        if (meshGroups.IsCreated)
          num1 = meshGroups.Length;
        crossFade = characterElementBuffers.TryGetBuffer(prefab, out bufferData1);
      }
      else
      {
        dynamicBuffer = subMeshBuffers[prefab];
        num1 = dynamicBuffer.Length;
      }
      for (int index1 = 0; index1 < num1; ++index1)
      {
        CharacterElement characterElement1 = new CharacterElement();
        if (bufferData1.IsCreated)
        {
          MeshGroup meshGroup;
          CollectionUtils.TryGet<MeshGroup>(meshGroups, index1, out meshGroup);
          characterElement1 = bufferData1[(int) meshGroup.m_SubMeshGroup];
        }
        else
        {
          int index2 = index1;
          if (bufferData2.IsCreated)
          {
            MeshGroup meshGroup;
            CollectionUtils.TryGet<MeshGroup>(meshGroups, index1, out meshGroup);
            index2 = bufferData2[(int) meshGroup.m_SubMeshGroup].m_SubMeshRange.x;
          }
          SubMesh subMesh = dynamicBuffer[index2];
          characterElement1.m_Style = subMesh.m_SubMesh;
        }
        DynamicBuffer<Game.Prefabs.AnimationClip> bufferData3;
        if (animationClipBuffers.TryGetBuffer(characterElement1.m_Style, out bufferData3))
        {
          int num2 = int.MaxValue;
          float y = 0.0f;
          for (int index3 = 0; index3 < bufferData3.Length; ++index3)
          {
            Game.Prefabs.AnimationClip animationClip1 = bufferData3[index3];
            if (animationClip1.m_Type == animationType && animationClip1.m_Activity == activityType && animationClip1.m_Layer == AnimationLayer.Body && animationClip1.m_PropID == propID)
            {
              ActivityCondition x2 = animationClip1.m_Conditions ^ conditions;
              if (x2 == (ActivityCondition) 0)
              {
                y = animationClip1.m_AnimationLength;
                characterElement = characterElement1;
                animationClip = animationClip1;
                break;
              }
              int num3 = math.countbits((uint) x2);
              if (num3 < num2)
              {
                num2 = num3;
                y = animationClip1.m_AnimationLength;
                characterElement = characterElement1;
                animationClip = animationClip1;
              }
            }
          }
          x1 = math.max(x1, y);
        }
      }
      return x1;
    }

    public static void GetRootMotion(
      DynamicBuffer<AnimationMotion> motions,
      int2 range,
      Game.Rendering.BlendWeights weights,
      float t,
      out float3 rootOffset,
      out float3 rootVelocity,
      out quaternion rootRotation)
    {
      if (range.y == range.x + 1)
      {
        ObjectUtils.GetRootMotion(motions[range.x], t, out rootOffset, out rootVelocity, out rootRotation);
      }
      else
      {
        float3 rootOffset1;
        float3 rootVelocity1;
        quaternion rootRotation1;
        ObjectUtils.GetRootMotion(motions[range.x], t, out rootOffset1, out rootVelocity1, out rootRotation1);
        float3 rootOffset2;
        float3 rootVelocity2;
        quaternion rootRotation2;
        ObjectUtils.GetRootMotion(motions[range.x + weights.m_Weight0.m_Index + 1], t, out rootOffset2, out rootVelocity2, out rootRotation2);
        float3 rootOffset3;
        float3 rootVelocity3;
        quaternion rootRotation3;
        ObjectUtils.GetRootMotion(motions[range.x + weights.m_Weight1.m_Index + 1], t, out rootOffset3, out rootVelocity3, out rootRotation3);
        float3 rootOffset4;
        float3 rootVelocity4;
        quaternion rootRotation4;
        ObjectUtils.GetRootMotion(motions[range.x + weights.m_Weight2.m_Index + 1], t, out rootOffset4, out rootVelocity4, out rootRotation4);
        float3 rootOffset5;
        float3 rootVelocity5;
        quaternion rootRotation5;
        ObjectUtils.GetRootMotion(motions[range.x + weights.m_Weight3.m_Index + 1], t, out rootOffset5, out rootVelocity5, out rootRotation5);
        float3 rootOffset6;
        float3 rootVelocity6;
        quaternion rootRotation6;
        ObjectUtils.GetRootMotion(motions[range.x + weights.m_Weight4.m_Index + 1], t, out rootOffset6, out rootVelocity6, out rootRotation6);
        float3 rootOffset7;
        float3 rootVelocity7;
        quaternion rootRotation7;
        ObjectUtils.GetRootMotion(motions[range.x + weights.m_Weight5.m_Index + 1], t, out rootOffset7, out rootVelocity7, out rootRotation7);
        float3 rootOffset8;
        float3 rootVelocity8;
        quaternion rootRotation8;
        ObjectUtils.GetRootMotion(motions[range.x + weights.m_Weight6.m_Index + 1], t, out rootOffset8, out rootVelocity8, out rootRotation8);
        float3 rootOffset9;
        float3 rootVelocity9;
        quaternion rootRotation9;
        ObjectUtils.GetRootMotion(motions[range.x + weights.m_Weight7.m_Index + 1], t, out rootOffset9, out rootVelocity9, out rootRotation9);
        float3 float3_1 = rootOffset2 * weights.m_Weight0.m_Weight;
        float3 float3_2 = rootOffset3 * weights.m_Weight1.m_Weight;
        float3 float3_3 = rootOffset4 * weights.m_Weight2.m_Weight;
        float3 float3_4 = rootOffset5 * weights.m_Weight3.m_Weight;
        float3 float3_5 = rootOffset6 * weights.m_Weight4.m_Weight;
        float3 float3_6 = rootOffset7 * weights.m_Weight5.m_Weight;
        float3 float3_7 = rootOffset8 * weights.m_Weight6.m_Weight;
        float3 float3_8 = rootOffset9 * weights.m_Weight7.m_Weight;
        float3 float3_9 = rootVelocity2 * weights.m_Weight0.m_Weight;
        float3 float3_10 = rootVelocity3 * weights.m_Weight1.m_Weight;
        float3 float3_11 = rootVelocity4 * weights.m_Weight2.m_Weight;
        float3 float3_12 = rootVelocity5 * weights.m_Weight3.m_Weight;
        float3 float3_13 = rootVelocity6 * weights.m_Weight4.m_Weight;
        float3 float3_14 = rootVelocity7 * weights.m_Weight5.m_Weight;
        float3 float3_15 = rootVelocity8 * weights.m_Weight6.m_Weight;
        float3 float3_16 = rootVelocity9 * weights.m_Weight7.m_Weight;
        rootOffset = rootOffset1 + float3_1 + float3_2 + float3_3 + float3_4 + float3_5 + float3_6 + float3_7 + float3_8;
        rootVelocity = rootVelocity1 + float3_9 + float3_10 + float3_11 + float3_12 + float3_13 + float3_14 + float3_15 + float3_16;
        quaternion a1 = math.slerp(quaternion.identity, rootRotation2, weights.m_Weight0.m_Weight);
        quaternion a2 = math.slerp(quaternion.identity, rootRotation3, weights.m_Weight1.m_Weight);
        quaternion a3 = math.slerp(quaternion.identity, rootRotation4, weights.m_Weight2.m_Weight);
        quaternion a4 = math.slerp(quaternion.identity, rootRotation5, weights.m_Weight3.m_Weight);
        quaternion a5 = math.slerp(quaternion.identity, rootRotation6, weights.m_Weight4.m_Weight);
        quaternion a6 = math.slerp(quaternion.identity, rootRotation7, weights.m_Weight5.m_Weight);
        quaternion a7 = math.slerp(quaternion.identity, rootRotation8, weights.m_Weight6.m_Weight);
        quaternion a8 = math.slerp(quaternion.identity, rootRotation9, weights.m_Weight7.m_Weight);
        rootRotation = math.mul(a8, math.mul(a7, math.mul(a6, math.mul(a5, math.mul(a4, math.mul(a3, math.mul(a2, math.mul(a1, rootRotation1))))))));
      }
    }

    private static void GetRootMotion(
      AnimationMotion motion,
      float t,
      out float3 rootOffset,
      out float3 rootVelocity,
      out quaternion rootRotation)
    {
      Bezier4x3 curve = new Bezier4x3(motion.m_StartOffset, motion.m_StartOffset, motion.m_EndOffset, motion.m_EndOffset);
      rootOffset = MathUtils.Position(curve, t);
      rootVelocity = MathUtils.Tangent(curve, t);
      rootRotation = math.slerp(motion.m_StartRotation, motion.m_EndRotation, t);
    }

    public static float GetTotalDamage(Damaged damaged)
    {
      float3 damage = damaged.m_Damage;
      damage.z = math.max(0.0f, damage.z - math.min(0.5f, math.csum(damage.xy)));
      return math.min(1f, math.csum(damage));
    }

    public static void UpdateResourcesDamage(
      Entity entity,
      float totalDamage,
      ref BufferLookup<Renter> renterData,
      ref BufferLookup<Game.Economy.Resources> resourcesData)
    {
      DynamicBuffer<Renter> bufferData1;
      if (!renterData.TryGetBuffer(entity, out bufferData1))
        return;
      for (int index1 = 0; index1 < bufferData1.Length; ++index1)
      {
        Renter renter = bufferData1[index1];
        DynamicBuffer<Game.Economy.Resources> bufferData2;
        if (resourcesData.TryGetBuffer(renter.m_Renter, out bufferData2))
        {
          for (int index2 = 0; index2 < bufferData2.Length; ++index2)
          {
            Game.Economy.Resources resources = bufferData2[index2];
            if (resources.m_Resource != Resource.Money)
              resources.m_Amount = (int) ((double) resources.m_Amount * (1.0 - (double) totalDamage));
            bufferData2[index2] = resources;
          }
        }
      }
    }

    public static Transform AdjustPosition(
      Transform transform,
      Elevation elevation,
      Entity prefab,
      out bool angledSample,
      ref TerrainHeightData terrainHeightData,
      ref WaterSurfaceData waterSurfaceData,
      ref ComponentLookup<PlaceableObjectData> placeableObjectDatas,
      ref ComponentLookup<ObjectGeometryData> objectGeometryDatas)
    {
      Transform transform1 = transform;
      float num = 0.0f;
      angledSample = true;
      PlaceableObjectData componentData1;
      if (placeableObjectDatas.TryGetComponent(prefab, out componentData1))
      {
        if ((componentData1.m_Flags & PlacementFlags.Hovering) != PlacementFlags.None)
        {
          transform1.m_Position.y = WaterUtils.SampleHeight(ref waterSurfaceData, ref terrainHeightData, transform.m_Position);
          transform1.m_Position.y += componentData1.m_PlacementOffset.y;
          angledSample = false;
        }
        else if ((componentData1.m_Flags & (PlacementFlags.Shoreline | PlacementFlags.Floating)) != PlacementFlags.None)
        {
          float waterHeight;
          float waterDepth;
          WaterUtils.SampleHeight(ref waterSurfaceData, ref terrainHeightData, transform.m_Position, out transform1.m_Position.y, out waterHeight, out waterDepth);
          if ((double) waterDepth >= 0.20000000298023224)
            transform1.m_Position.y = math.max(transform1.m_Position.y, waterHeight + componentData1.m_PlacementOffset.y);
          angledSample = false;
        }
        else
          num = componentData1.m_PlacementOffset.y;
      }
      if (angledSample)
      {
        ObjectGeometryData componentData2;
        if (objectGeometryDatas.TryGetComponent(prefab, out componentData2) && (componentData2.m_Flags & (GeometryFlags.Standing | GeometryFlags.HasBase)) != GeometryFlags.Standing)
        {
          float3 float3 = math.normalizesafe(math.forward(transform.m_Rotation) with
          {
            y = 0.0f
          }, math.forward());
          float3 y = new float3();
          y.xz = MathUtils.Right(float3.xz);
          float4 x;
          x.x = TerrainUtils.SampleHeight(ref terrainHeightData, transform.m_Position + y * componentData2.m_Bounds.min.x + float3 * componentData2.m_Bounds.min.z);
          x.y = TerrainUtils.SampleHeight(ref terrainHeightData, transform.m_Position + y * componentData2.m_Bounds.min.x + float3 * componentData2.m_Bounds.max.z);
          x.z = TerrainUtils.SampleHeight(ref terrainHeightData, transform.m_Position + y * componentData2.m_Bounds.max.x + float3 * componentData2.m_Bounds.max.z);
          x.w = TerrainUtils.SampleHeight(ref terrainHeightData, transform.m_Position + y * componentData2.m_Bounds.max.x + float3 * componentData2.m_Bounds.min.z);
          if ((componentData2.m_Flags & GeometryFlags.HasBase) != GeometryFlags.None)
          {
            transform1.m_Position.y = math.cmax(x);
          }
          else
          {
            float4 float4 = x.wzyz - x.xyxw;
            float4.xy = (float4.xz + float4.yw) / (2f * math.max((float2) 0.01f, MathUtils.Size(componentData2.m_Bounds.xz)));
            y.y = float4.x;
            float3.y = float4.y;
            float3 = math.normalizesafe(float3, math.forward());
            float3 up = math.normalizesafe(math.cross(float3, y), math.up());
            transform1.m_Rotation = quaternion.LookRotationSafe(float3, up);
            transform1.m_Position.y = math.csum(x) * 0.25f;
          }
        }
        else
        {
          transform1.m_Position.y = TerrainUtils.SampleHeight(ref terrainHeightData, transform.m_Position);
          angledSample = false;
        }
        transform1.m_Position.y += num;
      }
      transform1.m_Position.y += elevation.m_Elevation;
      return transform1;
    }

    public static int GetSubParentMesh(ElevationFlags elevationFlags)
    {
      switch (elevationFlags & (ElevationFlags.Stacked | ElevationFlags.OnGround))
      {
        case ElevationFlags.Stacked:
          return 1000;
        case ElevationFlags.OnGround:
          return -2;
        case ElevationFlags.Stacked | ElevationFlags.OnGround:
          return -1001;
        default:
          return 0;
      }
    }

    public static float GetTerrainSmoothingWidth(ObjectGeometryData objectGeometryData)
    {
      return ObjectUtils.GetTerrainSmoothingWidth(MathUtils.Size(objectGeometryData.m_Bounds.xz));
    }

    public static float GetTerrainSmoothingWidth(float2 size)
    {
      return math.max(8f, math.length(size) * 0.0833333358f);
    }

    public static uint GetRemainingConstructionFrames(UnderConstruction underConstruction)
    {
      return (uint) (math.clamp(100 - (int) underConstruction.m_Progress, 0, 100) * (int) (8192U / (uint) math.max(1, (int) underConstruction.m_Speed)) + 64);
    }

    public static uint GetTripDelayFrames(
      UnderConstruction underConstruction,
      PathInformation pathInformation)
    {
      uint constructionFrames = ObjectUtils.GetRemainingConstructionFrames(underConstruction);
      uint num = (uint) ((double) pathInformation.m_Duration * 60.0 + 0.5);
      return math.select(constructionFrames - num, 0U, num > constructionFrames);
    }

    public struct ActivityStartPositionCache
    {
      public ActivityType m_ActivityType;
      public float3 m_PositionOffset;
      public quaternion m_RotationOffset;
    }
  }
}
