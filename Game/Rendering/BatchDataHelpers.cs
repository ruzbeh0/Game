// Decompiled with JetBrains decompiler
// Type: Game.Rendering.BatchDataHelpers
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.Rendering;
using Game.Buildings;
using Game.Common;
using Game.Events;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Rendering
{
  public static class BatchDataHelpers
  {
    public static float4 GetBuildingState(
      PseudoRandomSeed pseudoRandomSeed,
      CitizenPresence citizenPresence,
      float lightFactor,
      bool abandoned,
      bool electricity)
    {
      Unity.Mathematics.Random random = pseudoRandomSeed.GetRandom((uint) PseudoRandomSeed.kBuildingState);
      float y = math.select(0.0f, math.select((float) (0.20000000298023224 + (double) citizenPresence.m_Presence * 0.0031372548546642065), 0.0f, abandoned) * lightFactor, electricity);
      float z = math.select(0.09f, 0.0f, abandoned | !electricity);
      return new float4(random.NextFloat(1f), y, z, 0.0f);
    }

    public static float4 GetBuildingState(
      PseudoRandomSeed pseudoRandomSeed,
      int passengersCount,
      int passengerCapacity,
      float lightFactor,
      bool destroyed)
    {
      Unity.Mathematics.Random random = pseudoRandomSeed.GetRandom((uint) PseudoRandomSeed.kBuildingState);
      float y = math.select((float) (0.20000000298023224 + (double) ((float) passengersCount / (float) math.max(1, passengerCapacity)) * 0.0031372548546642065), 0.0f, destroyed) * lightFactor;
      float z = math.select(0.09f, 0.0f, destroyed);
      return new float4(random.NextFloat(1f), y, z, 0.0f);
    }

    public static float3 GetAnimationCoordinate(Game.Prefabs.AnimationClip clip, float time, float previousTime)
    {
      float2 x = new float2(time, previousTime) / clip.m_AnimationLength;
      return new float3((math.select(math.saturate(x), math.frac(x), clip.m_Playback != 0) * clip.m_TextureRange + clip.m_TextureOffset) * clip.m_TextureWidth + 0.5f, clip.m_OneOverTextureWidth);
    }

    public static float2 GetBoneParameters(Skeleton skeleton)
    {
      uint x = math.select(skeleton.m_BufferAllocation.End - 1U, 0U, skeleton.m_BufferAllocation.End == 0U);
      return new float2(math.asfloat(skeleton.m_BufferAllocation.Begin), math.asfloat(x));
    }

    public static float2 GetBoneParameters(Animated animated)
    {
      uint x = math.select(animated.m_BoneAllocation.End - 1U, 0U, animated.m_BoneAllocation.End == 0U);
      return new float2(math.asfloat(animated.m_BoneAllocation.Begin), math.asfloat(x));
    }

    public static float2 GetLightParameters(Emissive emissive)
    {
      uint x = math.select(emissive.m_BufferAllocation.End - 1U, 0U, emissive.m_BufferAllocation.End == 0U);
      return new float2(math.asfloat(emissive.m_BufferAllocation.Begin), math.asfloat(x));
    }

    public static float4 GetWetness(Surface surface)
    {
      return new float4((float) surface.m_Wetness, (float) surface.m_SnowAmount, (float) surface.m_AccumulatedWetness, (float) surface.m_AccumulatedSnow) * 0.003921569f;
    }

    public static float4 GetDamage(Surface surface, Damaged damaged, OnFire onFire)
    {
      float2 float2 = 1f - new float2((float) surface.m_Dirtyness * 0.003921569f, damaged.m_Damage.x);
      float4 damage;
      damage.x = (float) (1.0 - (double) float2.x * (double) float2.y);
      damage.y = damaged.m_Damage.y;
      damage.z = damaged.m_Damage.z;
      damage.w = onFire.m_Intensity * 0.01f;
      return damage;
    }

    public static SubMeshFlags CalculateTreeSubMeshData(
      Tree tree,
      GrowthScaleData growthScaleData,
      out float3 scale)
    {
      switch (tree.m_State & (TreeState.Teen | TreeState.Adult | TreeState.Elderly | TreeState.Dead | TreeState.Stump))
      {
        case TreeState.Teen:
          scale = tree.m_Growth >= (byte) 128 ? math.lerp((float3) 1f, math.sqrt(growthScaleData.m_AdultSize / growthScaleData.m_TeenSize), (float) ((int) tree.m_Growth - 128) * (1f / 128f)) : math.lerp(math.sqrt(growthScaleData.m_ChildSize / growthScaleData.m_TeenSize), (float3) 1f, (float) tree.m_Growth * (1f / 128f));
          return SubMeshFlags.RequireTeen;
        case TreeState.Adult:
          scale = tree.m_Growth >= (byte) 128 ? math.lerp((float3) 1f, math.sqrt(growthScaleData.m_ElderlySize / growthScaleData.m_AdultSize), (float) ((int) tree.m_Growth - 128) * (1f / 128f)) : math.lerp(math.sqrt(growthScaleData.m_TeenSize / growthScaleData.m_AdultSize), (float3) 1f, (float) tree.m_Growth * (1f / 128f));
          return SubMeshFlags.RequireAdult;
        case TreeState.Elderly:
          scale = tree.m_Growth >= (byte) 128 ? math.lerp((float3) 1f, math.sqrt(growthScaleData.m_DeadSize / growthScaleData.m_ElderlySize), (float) ((int) tree.m_Growth - 128) * (1f / 128f)) : math.lerp(math.sqrt(growthScaleData.m_AdultSize / growthScaleData.m_ElderlySize), (float3) 1f, (float) tree.m_Growth * (1f / 128f));
          return SubMeshFlags.RequireElderly;
        case TreeState.Dead:
          scale = tree.m_Growth >= (byte) 128 ? (float3) 1f : math.lerp(math.sqrt(growthScaleData.m_ElderlySize / growthScaleData.m_DeadSize), (float3) 1f, (float) tree.m_Growth * (1f / 128f));
          return SubMeshFlags.RequireDead;
        case TreeState.Stump:
          scale = (float3) 1f;
          return SubMeshFlags.RequireStump;
        default:
          scale = tree.m_Growth >= (byte) 128 ? math.lerp((float3) 1f, math.sqrt(growthScaleData.m_TeenSize / growthScaleData.m_ChildSize), (float) ((int) tree.m_Growth - 128) * (1f / 128f)) : math.lerp(math.sqrt(growthScaleData.m_ChildSize / growthScaleData.m_TeenSize), (float3) 1f, (float) tree.m_Growth * (1f / 128f));
          return SubMeshFlags.RequireChild;
      }
    }

    public static SubMeshFlags CalculateNetObjectSubMeshData(Game.Objects.NetObject netObject)
    {
      return (SubMeshFlags) (((netObject.m_Flags & NetObjectFlags.TrackPassThrough) != (NetObjectFlags) 0 ? 4096 : 2048) | ((netObject.m_Flags & NetObjectFlags.Backward) != (NetObjectFlags) 0 ? 8388608 : 4194304));
    }

    public static SubMeshFlags CalculateQuantitySubMeshData(
      Quantity quantity,
      QuantityObjectData quantityObjectData,
      bool editorMode)
    {
      if (editorMode)
        return ((int) quantityObjectData.m_StepMask & 1) == 0 ? SubMeshFlags.RequireFull : SubMeshFlags.RequireEmpty;
      switch (quantityObjectData.m_StepMask & 6U)
      {
        case 2:
          if (quantity.m_Fullness > (byte) 50)
            return SubMeshFlags.RequireFull;
          return quantity.m_Fullness > (byte) 0 ? SubMeshFlags.RequirePartial1 : SubMeshFlags.RequireEmpty;
        case 4:
          if (quantity.m_Fullness > (byte) 50)
            return SubMeshFlags.RequireFull;
          return quantity.m_Fullness > (byte) 0 ? SubMeshFlags.RequirePartial2 : SubMeshFlags.RequireEmpty;
        case 6:
          if (quantity.m_Fullness > (byte) 66)
            return SubMeshFlags.RequireFull;
          if (quantity.m_Fullness > (byte) 33)
            return SubMeshFlags.RequirePartial2;
          return quantity.m_Fullness > (byte) 0 ? SubMeshFlags.RequirePartial1 : SubMeshFlags.RequireEmpty;
        default:
          return quantity.m_Fullness == (byte) 0 ? SubMeshFlags.RequireEmpty : SubMeshFlags.RequireFull;
      }
    }

    public static SubMeshFlags CalculateStackSubMeshData(
      Stack stack,
      StackData stackData,
      out int3 tileCounts,
      out float3 offsets,
      out float3 scale)
    {
      float a1 = MathUtils.Size(stackData.m_FirstBounds);
      float num1 = MathUtils.Size(stackData.m_MiddleBounds);
      float a2 = MathUtils.Size(stackData.m_LastBounds);
      float num2 = MathUtils.Size(stack.m_Range);
      float num3 = num2 - a1 - a2;
      int num4 = math.select(0, Mathf.RoundToInt(num3 / num1), (double) num3 > 0.0 && (double) num1 > 0.0);
      float num5 = math.select(a1, (float) ((double) num2 * (double) a1 / ((double) a1 + (double) a2)), num4 == 0 && (double) a1 > 0.0);
      float num6 = math.select(a2, (float) ((double) num2 * (double) a2 / ((double) a1 + (double) a2)), num4 == 0 && (double) a2 > 0.0);
      tileCounts.x = math.select(0, 1, (double) num5 > 0.0);
      tileCounts.y = num4;
      tileCounts.z = math.select(0, 1, (double) num6 > 0.0);
      scale.x = math.select(1f, num5 / a1, (double) a1 > 0.0);
      scale.y = math.select(1f, num3 / ((float) num4 * num1), num4 > 0 && (double) num1 > 0.0);
      scale.z = math.select(1f, num6 / a2, (double) a2 > 0.0);
      offsets.x = stack.m_Range.min - stackData.m_FirstBounds.min * scale.x;
      offsets.y = (float) ((double) stack.m_Range.min + (double) num5 - (double) stackData.m_MiddleBounds.min * (double) scale.y);
      offsets.z = stack.m_Range.max - stackData.m_LastBounds.max * scale.z;
      return SubMeshFlags.IsStackStart | SubMeshFlags.IsStackMiddle | SubMeshFlags.IsStackEnd;
    }

    public static void CalculateStackSubMeshData(
      StackData stackData,
      float3 offsets,
      float3 scales,
      int tileIndex,
      SubMeshFlags subMeshFlags,
      ref float3 subMeshPosition,
      ref float3 subMeshScale)
    {
      float num1;
      float num2;
      if ((subMeshFlags & SubMeshFlags.IsStackStart) != (SubMeshFlags) 0)
      {
        num1 = offsets.x + scales.x * MathUtils.Size(stackData.m_FirstBounds) * (float) tileIndex;
        num2 = scales.x;
      }
      else if ((subMeshFlags & SubMeshFlags.IsStackMiddle) != (SubMeshFlags) 0)
      {
        num1 = offsets.y + scales.y * MathUtils.Size(stackData.m_MiddleBounds) * (float) tileIndex;
        num2 = scales.y;
      }
      else
      {
        num1 = offsets.z + scales.z * MathUtils.Size(stackData.m_LastBounds) * (float) tileIndex;
        num2 = scales.z;
      }
      switch (stackData.m_Direction)
      {
        case StackDirection.Right:
          subMeshPosition.x += num1;
          subMeshScale.x = num2;
          break;
        case StackDirection.Up:
          subMeshPosition.y += num1;
          subMeshScale.y = num2;
          break;
        case StackDirection.Forward:
          subMeshPosition.z += num1;
          subMeshScale.z = num2;
          break;
      }
    }

    public static void CalculateEdgeParameters(
      EdgeGeometry edgeGeometry,
      bool isRotated,
      out BatchDataHelpers.CompositionParameters compositionParameters)
    {
      float3 float3 = MathUtils.Center(edgeGeometry.m_Bounds);
      float2 float2 = edgeGeometry.m_Start.m_Length + edgeGeometry.m_End.m_Length;
      float4 leftOffsets1;
      float4 rightOffsets1;
      BatchDataHelpers.CalculateMappingOffsets(edgeGeometry.m_Start, out leftOffsets1, out rightOffsets1, new float2(0.0f, edgeGeometry.m_Start.m_Length.x), new float2(0.0f, edgeGeometry.m_Start.m_Length.y));
      float4 leftOffsets2;
      float4 rightOffsets2;
      BatchDataHelpers.CalculateMappingOffsets(edgeGeometry.m_End, out leftOffsets2, out rightOffsets2, new float2(edgeGeometry.m_Start.m_Length.x, float2.x), new float2(edgeGeometry.m_Start.m_Length.y, float2.y));
      compositionParameters.m_TransformMatrix = TransformHelper.Translate(float3);
      if (isRotated)
      {
        compositionParameters.m_CompositionMatrix0 = BatchDataHelpers.BuildEdgeMatrix(MathUtils.Invert(edgeGeometry.m_End.m_Right), float3, rightOffsets2.wzyx);
        compositionParameters.m_CompositionMatrix1 = BatchDataHelpers.BuildEdgeMatrix(MathUtils.Invert(edgeGeometry.m_Start.m_Right), float3, rightOffsets1.wzyx);
        compositionParameters.m_CompositionMatrix2 = BatchDataHelpers.BuildEdgeMatrix(MathUtils.Invert(edgeGeometry.m_End.m_Left), float3, leftOffsets2.wzyx);
        compositionParameters.m_CompositionMatrix3 = BatchDataHelpers.BuildEdgeMatrix(MathUtils.Invert(edgeGeometry.m_Start.m_Left), float3, leftOffsets1.wzyx);
      }
      else
      {
        compositionParameters.m_CompositionMatrix0 = BatchDataHelpers.BuildEdgeMatrix(edgeGeometry.m_Start.m_Left, float3, leftOffsets1);
        compositionParameters.m_CompositionMatrix1 = BatchDataHelpers.BuildEdgeMatrix(edgeGeometry.m_End.m_Left, float3, leftOffsets2);
        compositionParameters.m_CompositionMatrix2 = BatchDataHelpers.BuildEdgeMatrix(edgeGeometry.m_Start.m_Right, float3, rightOffsets1);
        compositionParameters.m_CompositionMatrix3 = BatchDataHelpers.BuildEdgeMatrix(edgeGeometry.m_End.m_Right, float3, rightOffsets2);
      }
      compositionParameters.m_CompositionMatrix4 = float4x4.identity;
      compositionParameters.m_CompositionMatrix5 = float4x4.identity;
      compositionParameters.m_CompositionMatrix6 = float4x4.identity;
      compositionParameters.m_CompositionMatrix7 = float4x4.identity;
      compositionParameters.m_CompositionSync0 = new float4(0.2f, 0.4f, 0.6f, 0.8f);
      compositionParameters.m_CompositionSync1 = new float4(0.2f, 0.4f, 0.6f, 0.8f);
      compositionParameters.m_CompositionSync2 = new float4(0.2f, 0.4f, 0.6f, 0.8f);
      compositionParameters.m_CompositionSync3 = new float4(0.2f, 0.4f, 0.6f, 0.8f);
    }

    public static void CalculateNodeParameters(
      EdgeNodeGeometry nodeGeometry,
      NetCompositionData prefabCompositionData,
      out BatchDataHelpers.CompositionParameters compositionParameters)
    {
      float3 float3 = MathUtils.Center(nodeGeometry.m_Bounds);
      float4 mappingOffsets1 = new float4(0.0f, 0.333333343f, 0.6666667f, 1f) * prefabCompositionData.m_Width;
      compositionParameters.m_TransformMatrix = TransformHelper.Translate(float3);
      compositionParameters.m_CompositionSync0 = prefabCompositionData.m_SyncVertexOffsetsLeft;
      compositionParameters.m_CompositionSync1 = nodeGeometry.m_SyncVertexTargetsLeft;
      compositionParameters.m_CompositionSync2 = prefabCompositionData.m_SyncVertexOffsetsRight;
      compositionParameters.m_CompositionSync3 = nodeGeometry.m_SyncVertexTargetsRight;
      if ((double) nodeGeometry.m_MiddleRadius > 0.0)
      {
        float2 float2 = nodeGeometry.m_Left.m_Length + nodeGeometry.m_Right.m_Length;
        float4 leftOffsets1;
        float4 rightOffsets1;
        BatchDataHelpers.CalculateMappingOffsets(nodeGeometry.m_Left, out leftOffsets1, out rightOffsets1, new float2(0.0f, nodeGeometry.m_Left.m_Length.x), new float2(0.0f, nodeGeometry.m_Left.m_Length.y));
        float4 leftOffsets2;
        float4 rightOffsets2;
        BatchDataHelpers.CalculateMappingOffsets(nodeGeometry.m_Right, out leftOffsets2, out rightOffsets2, new float2(nodeGeometry.m_Left.m_Length.x, float2.x), new float2(nodeGeometry.m_Left.m_Length.y, float2.y));
        float3 direction1 = MathUtils.StartTangent(nodeGeometry.m_Left.m_Left);
        float3 direction2 = MathUtils.StartTangent(nodeGeometry.m_Left.m_Right);
        Bezier4x3 output1;
        Bezier4x3 output2;
        MathUtils.Divide(nodeGeometry.m_Middle, out output1, out output2, 0.99f);
        float4 mappingOffsets2 = math.lerp(leftOffsets1, rightOffsets1, 0.5f);
        float4 mappingOffsets3 = math.lerp(leftOffsets2, rightOffsets2, 0.5f);
        compositionParameters.m_CompositionMatrix0 = BatchDataHelpers.BuildEdgeMatrix(nodeGeometry.m_Left.m_Left, float3, leftOffsets1);
        compositionParameters.m_CompositionMatrix1 = BatchDataHelpers.BuildEdgeMatrix(nodeGeometry.m_Right.m_Left, float3, leftOffsets2);
        compositionParameters.m_CompositionMatrix2 = BatchDataHelpers.BuildEdgeMatrix(nodeGeometry.m_Left.m_Right, float3, rightOffsets1);
        compositionParameters.m_CompositionMatrix3 = BatchDataHelpers.BuildEdgeMatrix(nodeGeometry.m_Right.m_Right, float3, rightOffsets2);
        compositionParameters.m_CompositionMatrix4 = BatchDataHelpers.BuildEdgeMatrix(output1, float3, mappingOffsets2);
        compositionParameters.m_CompositionMatrix5 = BatchDataHelpers.BuildEdgeMatrix(output2, float3, mappingOffsets3);
        compositionParameters.m_CompositionMatrix6 = BatchDataHelpers.BuildEdgeMatrix(BatchDataHelpers.BuildCurve(nodeGeometry.m_Left.m_Left.a, direction1, prefabCompositionData.m_Width), float3, mappingOffsets1);
        compositionParameters.m_CompositionMatrix7 = BatchDataHelpers.BuildEdgeMatrix(BatchDataHelpers.BuildCurve(nodeGeometry.m_Left.m_Right.a, direction2, prefabCompositionData.m_Width), float3, mappingOffsets1);
      }
      else
      {
        float4 leftOffsets3;
        float4 rightOffsets3;
        BatchDataHelpers.CalculateMappingOffsets(nodeGeometry.m_Left, out leftOffsets3, out rightOffsets3, new float2(0.0f, nodeGeometry.m_Left.m_Length.x), new float2(0.0f, nodeGeometry.m_Left.m_Length.y));
        float4 leftOffsets4;
        float4 rightOffsets4;
        BatchDataHelpers.CalculateMappingOffsets(nodeGeometry.m_Right, out leftOffsets4, out rightOffsets4, new float2(0.0f, nodeGeometry.m_Right.m_Length.x), new float2(0.0f, nodeGeometry.m_Right.m_Length.y));
        float3 direction3 = MathUtils.StartTangent(nodeGeometry.m_Left.m_Left);
        float3 direction4 = MathUtils.StartTangent(nodeGeometry.m_Right.m_Right);
        compositionParameters.m_CompositionMatrix0 = BatchDataHelpers.BuildEdgeMatrix(nodeGeometry.m_Left.m_Left, float3, leftOffsets3);
        compositionParameters.m_CompositionMatrix1 = BatchDataHelpers.BuildEdgeMatrix(nodeGeometry.m_Right.m_Left, float3, leftOffsets4);
        compositionParameters.m_CompositionMatrix2 = BatchDataHelpers.BuildEdgeMatrix(nodeGeometry.m_Left.m_Right, float3, rightOffsets3);
        compositionParameters.m_CompositionMatrix3 = BatchDataHelpers.BuildEdgeMatrix(nodeGeometry.m_Right.m_Right, float3, rightOffsets4);
        compositionParameters.m_CompositionMatrix4 = BatchDataHelpers.BuildEdgeMatrix(nodeGeometry.m_Middle, float3, math.lerp(leftOffsets4, rightOffsets3, 0.5f));
        compositionParameters.m_CompositionMatrix5 = float4x4.identity;
        compositionParameters.m_CompositionMatrix6 = BatchDataHelpers.BuildEdgeMatrix(BatchDataHelpers.BuildCurve(nodeGeometry.m_Left.m_Left.a, direction3, prefabCompositionData.m_Width), float3, mappingOffsets1);
        compositionParameters.m_CompositionMatrix7 = BatchDataHelpers.BuildEdgeMatrix(BatchDataHelpers.BuildCurve(nodeGeometry.m_Right.m_Right.a, direction4, prefabCompositionData.m_Width), float3, mappingOffsets1);
      }
    }

    public static void CalculateOrphanParameters(
      Node node,
      NodeGeometry nodeGeometry,
      NetCompositionData prefabCompositionData,
      bool isPrimary,
      out BatchDataHelpers.CompositionParameters compositionParameters)
    {
      float3 float3 = MathUtils.Center(nodeGeometry.m_Bounds);
      Game.Net.Segment segment = new Game.Net.Segment();
      Bezier4x3 left;
      float3 direction;
      if (isPrimary)
      {
        segment.m_Left.a = new float3(node.m_Position.x - prefabCompositionData.m_Width * 0.5f, node.m_Position.y, node.m_Position.z);
        segment.m_Left.b = new float3(node.m_Position.x - prefabCompositionData.m_Width * 0.5f, node.m_Position.y, node.m_Position.z + prefabCompositionData.m_Width * 0.2761424f);
        segment.m_Left.c = new float3(node.m_Position.x - prefabCompositionData.m_Width * 0.2761424f, node.m_Position.y, node.m_Position.z + prefabCompositionData.m_Width * 0.5f);
        segment.m_Left.d = new float3(node.m_Position.x, node.m_Position.y, node.m_Position.z + prefabCompositionData.m_Width * 0.5f);
        segment.m_Right = new Bezier4x3(node.m_Position, node.m_Position, node.m_Position, node.m_Position);
        segment.m_Length = new float2(prefabCompositionData.m_Width * 1.57079637f, 0.0f);
        left = segment.m_Left;
        left.a.x += prefabCompositionData.m_Width;
        left.b.x += prefabCompositionData.m_Width;
        left.c.x = node.m_Position.x * 2f - left.c.x;
        direction = new float3(0.0f, 0.0f, 1f);
      }
      else
      {
        segment.m_Left.a = new float3(node.m_Position.x + prefabCompositionData.m_Width * 0.5f, node.m_Position.y, node.m_Position.z);
        segment.m_Left.b = new float3(node.m_Position.x + prefabCompositionData.m_Width * 0.5f, node.m_Position.y, node.m_Position.z - prefabCompositionData.m_Width * 0.2761424f);
        segment.m_Left.c = new float3(node.m_Position.x + prefabCompositionData.m_Width * 0.2761424f, node.m_Position.y, node.m_Position.z - prefabCompositionData.m_Width * 0.5f);
        segment.m_Left.d = new float3(node.m_Position.x, node.m_Position.y, node.m_Position.z - prefabCompositionData.m_Width * 0.5f);
        segment.m_Right = new Bezier4x3(node.m_Position, node.m_Position, node.m_Position, node.m_Position);
        segment.m_Length = new float2(prefabCompositionData.m_Width * 1.57079637f, 0.0f);
        left = segment.m_Left;
        left.a.x -= prefabCompositionData.m_Width;
        left.b.x -= prefabCompositionData.m_Width;
        left.c.x = node.m_Position.x * 2f - left.c.x;
        direction = new float3(0.0f, 0.0f, -1f);
      }
      float4 leftOffsets;
      float4 rightOffsets;
      BatchDataHelpers.CalculateMappingOffsets(segment, out leftOffsets, out rightOffsets, new float2(0.0f, segment.m_Length.x), new float2(0.0f, 0.0f));
      float4 mappingOffsets = new float4(0.0f, 0.333333343f, 0.6666667f, 1f) * prefabCompositionData.m_Width;
      compositionParameters.m_TransformMatrix = TransformHelper.Translate(float3);
      compositionParameters.m_CompositionSync0 = prefabCompositionData.m_SyncVertexOffsetsLeft;
      compositionParameters.m_CompositionSync1 = prefabCompositionData.m_SyncVertexOffsetsLeft;
      compositionParameters.m_CompositionSync2 = prefabCompositionData.m_SyncVertexOffsetsRight;
      compositionParameters.m_CompositionSync3 = prefabCompositionData.m_SyncVertexOffsetsRight;
      compositionParameters.m_CompositionMatrix0 = BatchDataHelpers.BuildEdgeMatrix(segment.m_Left, float3, leftOffsets);
      compositionParameters.m_CompositionMatrix1 = BatchDataHelpers.BuildEdgeMatrix(segment.m_Right, float3, rightOffsets);
      compositionParameters.m_CompositionMatrix2 = compositionParameters.m_CompositionMatrix1;
      compositionParameters.m_CompositionMatrix3 = BatchDataHelpers.BuildEdgeMatrix(left, float3, leftOffsets);
      compositionParameters.m_CompositionMatrix4 = compositionParameters.m_CompositionMatrix1;
      compositionParameters.m_CompositionMatrix5 = float4x4.identity;
      compositionParameters.m_CompositionMatrix6 = BatchDataHelpers.BuildEdgeMatrix(BatchDataHelpers.BuildCurve(segment.m_Left.a, direction, prefabCompositionData.m_Width), float3, mappingOffsets);
      compositionParameters.m_CompositionMatrix7 = BatchDataHelpers.BuildEdgeMatrix(BatchDataHelpers.BuildCurve(left.a, direction, prefabCompositionData.m_Width), float3, mappingOffsets);
    }

    private static void CalculateMappingOffsets(
      Game.Net.Segment segment,
      out float4 leftOffsets,
      out float4 rightOffsets,
      float2 leftMappingOffset,
      float2 rightMappingOffset)
    {
      float2 float2_1;
      float2_1.x = Vector3.Distance((Vector3) segment.m_Left.a, (Vector3) segment.m_Left.b);
      float2_1.y = Vector3.Distance((Vector3) segment.m_Left.c, (Vector3) segment.m_Left.d);
      float2 float2_2;
      float2_2.x = Vector3.Distance((Vector3) segment.m_Right.a, (Vector3) segment.m_Right.b);
      float2_2.y = Vector3.Distance((Vector3) segment.m_Right.c, (Vector3) segment.m_Right.d);
      float2 float2_3 = float2_1 * ((leftMappingOffset.y - leftMappingOffset.x) / math.max(1f, segment.m_Length.x));
      float2 float2_4 = float2_2 * ((rightMappingOffset.y - rightMappingOffset.x) / math.max(1f, segment.m_Length.y));
      leftOffsets = new float4(leftMappingOffset.x, leftMappingOffset.x + float2_3.x, leftMappingOffset.y - float2_3.y, leftMappingOffset.y);
      rightOffsets = new float4(rightMappingOffset.x, rightMappingOffset.x + float2_4.x, rightMappingOffset.y - float2_4.y, rightMappingOffset.y);
    }

    private static float4x4 BuildEdgeMatrix(Bezier4x3 curve, float3 offset, float4 mappingOffsets)
    {
      return new float4x4()
      {
        c0 = new float4(curve.a - offset, mappingOffsets.x),
        c1 = new float4(curve.b - offset, mappingOffsets.y),
        c2 = new float4(curve.c - offset, mappingOffsets.z),
        c3 = new float4(curve.d - offset, mappingOffsets.w)
      };
    }

    private static Bezier4x3 BuildCurve(float3 startPos, float3 direction, float length)
    {
      direction = MathUtils.Normalize(direction, direction.xz);
      direction.y = math.clamp(direction.y, -1f, 1f);
      Bezier4x3 bezier4x3;
      bezier4x3.a = startPos;
      bezier4x3.b = startPos + direction * (length * 0.333333343f);
      bezier4x3.c = startPos + direction * (length * 0.6666667f);
      bezier4x3.d = startPos + direction * length;
      return bezier4x3;
    }

    public static float4x4 BuildTransformMatrix(
      Curve curve,
      float4 size,
      float4 curveScale,
      float smoothingDistance,
      bool isDecal,
      bool isLoaded)
    {
      if (isDecal)
      {
        float3 float3_1 = curve.m_Bezier.d - curve.m_Bezier.a;
        float3 float3_2 = math.select(new float3(2.5f, 2.5f, 2f), size.xyz, isLoaded);
        size.xy *= math.max(curveScale.xy, curveScale.zw);
        if (MathUtils.TryNormalize(ref float3_1))
        {
          float3 y1 = math.normalizesafe(MathUtils.StartTangent(curve.m_Bezier));
          float3 y2 = math.normalizesafe(MathUtils.EndTangent(curve.m_Bezier));
          curve.m_Bezier.a -= y1 * smoothingDistance;
          curve.m_Bezier.d += y2 * smoothingDistance;
          float3 float3_3 = new float3();
          float3_3.xz = math.normalizesafe(MathUtils.Right(float3_1.xz), new float2(1f, 0.0f));
          float3 float3_4 = math.cross(float3_1, float3_3);
          float3 x1 = curve.m_Bezier.b - curve.m_Bezier.a;
          float3 x2 = curve.m_Bezier.c - curve.m_Bezier.a;
          float3 x3 = curve.m_Bezier.d - curve.m_Bezier.a;
          float3 y3 = new float3(math.dot(x1, float3_3), math.dot(x1, float3_4), math.dot(x1, float3_1));
          float3 x4 = new float3(math.dot(x2, float3_3), math.dot(x2, float3_4), math.dot(x2, float3_1));
          float3 y4 = new float3(math.dot(x3, float3_3), math.dot(x3, float3_4), math.dot(x3, float3_1));
          float3 float3_5 = math.min(math.min((float3) 0.0f, y3), math.min(x4, y4));
          float3 float3_6 = math.max(math.max((float3) 0.0f, y3), math.max(x4, y4));
          float2 x5 = new float2(math.dot(float3_3, y1), math.dot(float3_3, y2));
          float2 x6 = new float2(y1.y, y2.y);
          float3 float3_7 = new float3(size.xy, (float) ((double) size.x * (double) math.cmax(math.abs(x5)) + (double) size.y * (double) math.cmax(math.abs(x6)))) * 0.5f;
          float3 x7 = float3_5 - float3_7;
          float3 y5 = float3_6 + float3_7;
          float3 v = math.lerp(x7, y5, 0.5f);
          quaternion quaternion = quaternion.LookRotation(float3_1, float3_4);
          float3 translation = curve.m_Bezier.a + math.rotate(quaternion, v);
          float3 scale = (y5 - x7) / float3_2;
          translation.y += size.w;
          translation -= float3_4 * (size.w * scale.y);
          return float4x4.TRS(translation, quaternion, scale);
        }
        float3 translation1 = math.lerp(curve.m_Bezier.a, curve.m_Bezier.d, 0.5f);
        quaternion identity = quaternion.identity;
        size.z += smoothingDistance;
        float3 float3_8 = size.xyx / float3_2;
        quaternion rotation = identity;
        float3 scale1 = float3_8;
        return float4x4.TRS(translation1, rotation, scale1);
      }
      if (isLoaded)
        return float4x4.Translate(math.lerp(curve.m_Bezier.a, curve.m_Bezier.d, 0.5f));
      float3 translation2 = math.lerp(curve.m_Bezier.a, curve.m_Bezier.d, 0.5f);
      quaternion identity1 = quaternion.identity;
      float3 float3 = new float3(math.max(size.xy, (float2) 0.02f) * 0.4f, 1f);
      quaternion rotation1 = identity1;
      float3 scale2 = float3;
      return float4x4.TRS(translation2, rotation1, scale2);
    }

    public static float4x4 BuildCurveMatrix(
      Curve curve,
      float3x4 transformMatrix,
      float4 size,
      int tilingCount)
    {
      float2 float2_1;
      float2_1.x = math.distance(curve.m_Bezier.a, curve.m_Bezier.b);
      float2_1.y = math.distance(curve.m_Bezier.c, curve.m_Bezier.d);
      float2 float2_2 = float2_1 / curve.m_Length;
      float4 float4_1 = new float4(0.0f, float2_2.x, 1f - float2_2.y, 1f);
      float3 c3 = transformMatrix.c3;
      float a = curve.m_Length / math.max(1f, size.z);
      float num = math.select(a, math.round(a * (float) tilingCount) / (float) tilingCount, tilingCount != 0);
      float4 float4_2 = float4_1 * num;
      float4x4 float4x4;
      float4x4.c0 = new float4(curve.m_Bezier.a - c3, float4_2.x);
      float4x4.c1 = new float4(curve.m_Bezier.b - c3, float4_2.y);
      float4x4.c2 = new float4(curve.m_Bezier.c - c3, float4_2.z);
      float4x4.c3 = new float4(curve.m_Bezier.d - c3, float4_2.w);
      return float4x4;
    }

    public static float4 BuildCurveParams(float4 size, NodeLane nodeLane)
    {
      return new float4(size.z, 0.0f, math.select(1f / new float2(1f + (float) nodeLane.m_SharedStartCount, 1f + (float) nodeLane.m_SharedEndCount), (float2) -1f, new bool2(nodeLane.m_SharedStartCount == byte.MaxValue, nodeLane.m_SharedEndCount == byte.MaxValue)));
    }

    public static float4 BuildCurveParams(float4 size, EdgeLane edgeLane)
    {
      return new float4(size.z, 0.0f, math.select((float2) 1f, (float2) 0.0f, new bool2(edgeLane.m_ConnectedStartCount == (byte) 0, edgeLane.m_ConnectedEndCount == (byte) 0)));
    }

    public static float4 BuildCurveParams(float4 size, Game.Net.Elevation elevation)
    {
      return new float4(size.z, math.select(0.0f, 1f, math.any(elevation.m_Elevation == float.MinValue)), 1f, 1f);
    }

    public static float4 BuildCurveParams(float4 size) => new float4(size.z, 1f, 1f, 1f);

    public static float4 BuildCurveScale(NodeLane nodeLane, NetLaneData netLaneData)
    {
      return new float4(math.select(1f + nodeLane.m_WidthOffset / netLaneData.m_Width, (float2) 1f, (double) netLaneData.m_Width == 0.0).xxyy);
    }

    public static float4 BuildCurveScale() => (float4) 1f;

    public static int GetTileCount(
      Curve curve,
      float length,
      int tilingCount,
      bool geometryTiling,
      out int clipCount)
    {
      if (tilingCount != 0)
      {
        float num = curve.m_Length / math.max(1f, length);
        clipCount = Mathf.RoundToInt(num * (float) tilingCount);
        int y = (clipCount + tilingCount - 1) / tilingCount;
        return math.select(math.min(1, y), math.min(256, y), geometryTiling);
      }
      if (geometryTiling)
      {
        double num = (double) curve.m_Length / (double) math.max(1f, length);
        clipCount = 0;
        return math.clamp(Mathf.CeilToInt((float) (num - 9.9999997473787516E-05)), 1, 256);
      }
      clipCount = 0;
      return 1;
    }

    public struct CompositionParameters
    {
      public float3x4 m_TransformMatrix;
      public float4x4 m_CompositionMatrix0;
      public float4x4 m_CompositionMatrix1;
      public float4x4 m_CompositionMatrix2;
      public float4x4 m_CompositionMatrix3;
      public float4x4 m_CompositionMatrix4;
      public float4x4 m_CompositionMatrix5;
      public float4x4 m_CompositionMatrix6;
      public float4x4 m_CompositionMatrix7;
      public float4 m_CompositionSync0;
      public float4 m_CompositionSync1;
      public float4 m_CompositionSync2;
      public float4 m_CompositionSync3;
    }
  }
}
