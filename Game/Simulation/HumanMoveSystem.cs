// Decompiled with JetBrains decompiler
// Type: Game.Simulation.HumanMoveSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Creatures;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Rendering;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class HumanMoveSystem : GameSystemBase
  {
    private SimulationSystem m_SimulationSystem;
    private TerrainSystem m_TerrainSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private EntityQuery m_HumanQuery;
    private HumanMoveSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_HumanQuery = this.GetEntityQuery(ComponentType.ReadOnly<Human>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadWrite<TransformFrame>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint index = this.m_SimulationSystem.frameIndex % 16U;
      // ISSUE: reference to a compiler-generated field
      this.m_HumanQuery.ResetFilter();
      // ISSUE: reference to a compiler-generated field
      this.m_HumanQuery.SetSharedComponentFilter<UpdateFrame>(new UpdateFrame(index));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AnimationMotion_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AnimationClip_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CharacterElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Orphan_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TransformFrame_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Moving_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_HumanNavigation_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Stumbling_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Human_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new HumanMoveSystem.UpdateTransformDataJob()
      {
        m_HumanType = this.__TypeHandle.__Game_Creatures_Human_RO_ComponentTypeHandle,
        m_StumblingType = this.__TypeHandle.__Game_Creatures_Stumbling_RO_ComponentTypeHandle,
        m_CurrentVehicleType = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_MeshGroupType = this.__TypeHandle.__Game_Rendering_MeshGroup_RO_BufferTypeHandle,
        m_NavigationType = this.__TypeHandle.__Game_Creatures_HumanNavigation_RW_ComponentTypeHandle,
        m_MovingType = this.__TypeHandle.__Game_Objects_Moving_RW_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RW_ComponentTypeHandle,
        m_TransformFrameType = this.__TypeHandle.__Game_Objects_TransformFrame_RW_BufferTypeHandle,
        m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_OrphanData = this.__TypeHandle.__Game_Net_Orphan_RO_ComponentLookup,
        m_NodeData = this.__TypeHandle.__Game_Net_Node_RO_ComponentLookup,
        m_NodeGeometryData = this.__TypeHandle.__Game_Net_NodeGeometry_RO_ComponentLookup,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_StartNodeGeometryData = this.__TypeHandle.__Game_Net_StartNodeGeometry_RO_ComponentLookup,
        m_EndNodeGeometryData = this.__TypeHandle.__Game_Net_EndNodeGeometry_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_PrefabGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_SubMeshGroups = this.__TypeHandle.__Game_Prefabs_SubMeshGroup_RO_BufferLookup,
        m_CharacterElements = this.__TypeHandle.__Game_Prefabs_CharacterElement_RO_BufferLookup,
        m_AnimationClips = this.__TypeHandle.__Game_Prefabs_AnimationClip_RO_BufferLookup,
        m_AnimationMotions = this.__TypeHandle.__Game_Prefabs_AnimationMotion_RO_BufferLookup,
        m_SubMeshes = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup,
        m_ActivityLocations = this.__TypeHandle.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup,
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies),
        m_TransformFrameIndex = (this.m_SimulationSystem.frameIndex / 16U % 4U)
      }.ScheduleParallel<HumanMoveSystem.UpdateTransformDataJob>(this.m_HumanQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle);
      this.Dependency = jobHandle;
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
    public HumanMoveSystem()
    {
    }

    [BurstCompile]
    private struct UpdateTransformDataJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Human> m_HumanType;
      [ReadOnly]
      public ComponentTypeHandle<Stumbling> m_StumblingType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentVehicle> m_CurrentVehicleType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<MeshGroup> m_MeshGroupType;
      public ComponentTypeHandle<HumanNavigation> m_NavigationType;
      public ComponentTypeHandle<Moving> m_MovingType;
      public ComponentTypeHandle<Transform> m_TransformType;
      public BufferTypeHandle<TransformFrame> m_TransformFrameType;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<Orphan> m_OrphanData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> m_NodeData;
      [ReadOnly]
      public ComponentLookup<NodeGeometry> m_NodeGeometryData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_PrefabCompositionData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabGeometryData;
      [ReadOnly]
      public BufferLookup<SubMeshGroup> m_SubMeshGroups;
      [ReadOnly]
      public BufferLookup<CharacterElement> m_CharacterElements;
      [ReadOnly]
      public BufferLookup<AnimationClip> m_AnimationClips;
      [ReadOnly]
      public BufferLookup<AnimationMotion> m_AnimationMotions;
      [ReadOnly]
      public BufferLookup<SubMesh> m_SubMeshes;
      [ReadOnly]
      public BufferLookup<ActivityLocationElement> m_ActivityLocations;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public uint m_TransformFrameIndex;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Stumbling>(ref this.m_StumblingType))
        {
          // ISSUE: reference to a compiler-generated method
          this.StumblingMove(chunk);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.NormalMove(chunk);
        }
      }

      private void NormalMove(ArchetypeChunk chunk)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Human> nativeArray1 = chunk.GetNativeArray<Human>(ref this.m_HumanType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentVehicle> nativeArray2 = chunk.GetNativeArray<CurrentVehicle>(ref this.m_CurrentVehicleType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<HumanNavigation> nativeArray3 = chunk.GetNativeArray<HumanNavigation>(ref this.m_NavigationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Moving> nativeArray4 = chunk.GetNativeArray<Moving>(ref this.m_MovingType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray5 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray6 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TransformFrame> bufferAccessor1 = chunk.GetBufferAccessor<TransformFrame>(ref this.m_TransformFrameType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<MeshGroup> bufferAccessor2 = chunk.GetBufferAccessor<MeshGroup>(ref this.m_MeshGroupType);
        float timeStep = 0.266666681f;
        // ISSUE: reference to a compiler-generated field
        int transformFrameIndex = (int) this.m_TransformFrameIndex;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int index1 = math.select((int) this.m_TransformFrameIndex - 1, 3, this.m_TransformFrameIndex == 0U);
        for (int index2 = 0; index2 < chunk.Count; ++index2)
        {
          Human human = nativeArray1[index2];
          HumanNavigation humanNavigation = nativeArray3[index2];
          Moving moving = nativeArray4[index2];
          Transform transform = nativeArray5[index2];
          PrefabRef prefabRef = nativeArray6[index2];
          DynamicBuffer<TransformFrame> dynamicBuffer = bufferAccessor1[index2];
          TransformFrame transformFrame1 = dynamicBuffer[index1];
          float3 float3_1 = humanNavigation.m_TargetPosition - transform.m_Position;
          MathUtils.TryNormalize(ref float3_1, humanNavigation.m_MaxSpeed);
          TransformFrame transformFrame2 = new TransformFrame();
          if ((double) humanNavigation.m_MaxSpeed >= 0.10000000149011612 && humanNavigation.m_TargetActivity == (byte) 0)
          {
            transformFrame2.m_State = TransformState.Move;
          }
          else
          {
            transformFrame2.m_State = TransformState.Idle;
            transformFrame2.m_Activity = humanNavigation.m_TargetActivity;
          }
          float3 targetDirection = new float3(humanNavigation.m_TargetDirection.x, 0.0f, humanNavigation.m_TargetDirection.y);
          if (!targetDirection.Equals(new float3()))
            transform.m_Rotation = quaternion.LookRotation(targetDirection, math.up());
          CurrentVehicle currentVehicle;
          CollectionUtils.TryGet<CurrentVehicle>(nativeArray2, index2, out currentVehicle);
          DynamicBuffer<MeshGroup> meshGroups;
          CollectionUtils.TryGet<MeshGroup>(bufferAccessor2, index2, out meshGroups);
          // ISSUE: reference to a compiler-generated method
          AnimatedPropID propId1 = this.GetPropID(in transformFrame1, currentVehicle);
          // ISSUE: reference to a compiler-generated method
          AnimatedPropID propId2 = this.GetPropID(in transformFrame2, currentVehicle);
          ActivityCondition conditions = CreatureUtils.GetConditions(human);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          ObjectUtils.UpdateAnimation(prefabRef.m_Prefab, timeStep, meshGroups, ref this.m_SubMeshGroups, ref this.m_CharacterElements, ref this.m_SubMeshes, ref this.m_AnimationClips, ref this.m_AnimationMotions, propId1, propId2, conditions, ref humanNavigation.m_MaxSpeed, ref humanNavigation.m_TargetActivity, ref humanNavigation.m_TargetPosition, ref targetDirection, ref transform, ref transformFrame1, ref transformFrame2);
          humanNavigation.m_TransformState = transformFrame2.m_State;
          humanNavigation.m_LastActivity = transformFrame2.m_Activity;
          if (math.any(targetDirection.xz != humanNavigation.m_TargetDirection))
          {
            humanNavigation.m_TargetDirection = math.normalizesafe(targetDirection.xz);
            if (!targetDirection.Equals(new float3()))
              transform.m_Rotation = quaternion.LookRotation(targetDirection, math.up());
          }
          float3 float3_2 = float3_1 * 8f + moving.m_Velocity;
          MathUtils.TryNormalize(ref float3_2, humanNavigation.m_MaxSpeed);
          moving.m_Velocity = float3_2;
          float3 float3_3 = moving.m_Velocity * (timeStep * 0.5f);
          transform.m_Position += float3_3;
          float2 xz = moving.m_Velocity.xz;
          if ((double) math.length(xz) >= 0.10000000149011612)
          {
            float2 float2 = math.normalize(xz);
            transform.m_Rotation = quaternion.LookRotation(new float3(float2.x, 0.0f, float2.y), math.up());
          }
          transformFrame2.m_Position = transform.m_Position;
          transformFrame2.m_Velocity += moving.m_Velocity;
          transformFrame2.m_Rotation = transform.m_Rotation;
          transform.m_Position += float3_3;
          dynamicBuffer[transformFrameIndex] = transformFrame2;
          nativeArray3[index2] = humanNavigation;
          nativeArray4[index2] = moving;
          nativeArray5[index2] = transform;
        }
      }

      private AnimatedPropID GetPropID(
        in TransformFrame transformFrame,
        CurrentVehicle currentVehicle)
      {
        AnimatedPropID propId = new AnimatedPropID(-1);
        PrefabRef componentData;
        DynamicBuffer<ActivityLocationElement> bufferData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((transformFrame.m_Activity == (byte) 10 || transformFrame.m_Activity == (byte) 11) && this.m_PrefabRefData.TryGetComponent(currentVehicle.m_Vehicle, out componentData) && this.m_ActivityLocations.TryGetBuffer(componentData.m_Prefab, out bufferData) && bufferData.Length != 0)
          propId = bufferData[0].m_PropID;
        return propId;
      }

      private void StumblingMove(ArchetypeChunk chunk)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray1 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<HumanNavigation> nativeArray2 = chunk.GetNativeArray<HumanNavigation>(ref this.m_NavigationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Moving> nativeArray3 = chunk.GetNativeArray<Moving>(ref this.m_MovingType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray4 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TransformFrame> bufferAccessor = chunk.GetBufferAccessor<TransformFrame>(ref this.m_TransformFrameType);
        int num1 = 4;
        float timeStep = 0.266666681f;
        float num2 = timeStep / (float) num1;
        float grip = 10f;
        float gravity = 10f;
        float num3 = math.pow(0.9f, num2);
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          PrefabRef prefabRef = nativeArray1[index1];
          HumanNavigation humanNavigation = nativeArray2[index1];
          Moving moving = nativeArray3[index1];
          Transform transform = nativeArray4[index1];
          // ISSUE: reference to a compiler-generated field
          ObjectGeometryData objectGeometryData = this.m_PrefabGeometryData[prefabRef.m_Prefab];
          for (int index2 = 0; index2 < num1; ++index2)
          {
            float3 momentOfInertia = ObjectUtils.CalculateMomentOfInertia(transform.m_Rotation, objectGeometryData.m_Size);
            float3 origin = transform.m_Position + math.mul(transform.m_Rotation, new float3(0.0f, objectGeometryData.m_Bounds.max.y * 0.5f, 0.0f));
            Quad3 cornerPositions;
            cornerPositions.a = ObjectUtils.LocalToWorld(transform, new float3(0.0f, objectGeometryData.m_Bounds.min.y + 0.2f, 0.0f));
            cornerPositions.b = ObjectUtils.LocalToWorld(transform, new float3(0.0f, objectGeometryData.m_Bounds.max.y - 0.2f, 0.0f));
            cornerPositions.c = ObjectUtils.LocalToWorld(transform, new float3(objectGeometryData.m_Bounds.min.x, objectGeometryData.m_Bounds.max.y * 0.5f, 0.0f));
            cornerPositions.d = ObjectUtils.LocalToWorld(transform, new float3(objectGeometryData.m_Bounds.max.x, objectGeometryData.m_Bounds.max.y * 0.5f, 0.0f));
            float4 heights;
            // ISSUE: reference to a compiler-generated method
            this.GetGroundHeight(cornerPositions, moving.m_Velocity, timeStep, gravity, out heights);
            heights += 0.2f;
            Quad3 quad3_1;
            // ISSUE: reference to a compiler-generated method
            quad3_1.a = this.CalculatePointVelocityDelta(cornerPositions.a, origin, moving, grip, num2, heights.x, gravity);
            // ISSUE: reference to a compiler-generated method
            quad3_1.b = this.CalculatePointVelocityDelta(cornerPositions.b, origin, moving, grip, num2, heights.y, gravity);
            // ISSUE: reference to a compiler-generated method
            quad3_1.c = this.CalculatePointVelocityDelta(cornerPositions.c, origin, moving, grip, num2, heights.z, gravity);
            // ISSUE: reference to a compiler-generated method
            quad3_1.d = this.CalculatePointVelocityDelta(cornerPositions.d, origin, moving, grip, num2, heights.w, gravity);
            Quad3 quad3_2;
            // ISSUE: reference to a compiler-generated method
            quad3_2.a = this.CalculatePointAngularVelocityDelta(cornerPositions.a, origin, quad3_1.a, momentOfInertia);
            // ISSUE: reference to a compiler-generated method
            quad3_2.b = this.CalculatePointAngularVelocityDelta(cornerPositions.b, origin, quad3_1.b, momentOfInertia);
            // ISSUE: reference to a compiler-generated method
            quad3_2.c = this.CalculatePointAngularVelocityDelta(cornerPositions.c, origin, quad3_1.c, momentOfInertia);
            // ISSUE: reference to a compiler-generated method
            quad3_2.d = this.CalculatePointAngularVelocityDelta(cornerPositions.d, origin, quad3_1.d, momentOfInertia);
            float3 float3_1 = (quad3_1.a + quad3_1.b + quad3_1.c + quad3_1.d) * 0.125f;
            float3 float3_2 = (quad3_2.a + quad3_2.b + quad3_2.c + quad3_2.d) * 0.125f;
            float3_1.y -= gravity * num2;
            moving.m_Velocity *= num3;
            moving.m_AngularVelocity *= num3;
            moving.m_Velocity += float3_1;
            moving.m_AngularVelocity += float3_2;
            float num4 = math.length(moving.m_AngularVelocity);
            if ((double) num4 > 9.9999997473787516E-06)
            {
              quaternion a = quaternion.AxisAngle(moving.m_AngularVelocity / num4, num4 * num2);
              transform.m_Rotation = math.normalize(math.mul(a, transform.m_Rotation));
              float3 float3_3 = transform.m_Position + math.mul(transform.m_Rotation, new float3(0.0f, objectGeometryData.m_Bounds.max.y * 0.5f, 0.0f));
              transform.m_Position += origin - float3_3;
            }
            transform.m_Position += moving.m_Velocity * num2;
          }
          // ISSUE: reference to a compiler-generated field
          bufferAccessor[index1][(int) this.m_TransformFrameIndex] = new TransformFrame()
          {
            m_Position = transform.m_Position - moving.m_Velocity * (timeStep * 0.5f),
            m_Velocity = moving.m_Velocity,
            m_Rotation = transform.m_Rotation
          };
          nativeArray3[index1] = moving;
          nativeArray4[index1] = transform;
        }
      }

      private float3 CalculatePointVelocityDelta(
        float3 position,
        float3 origin,
        Moving moving,
        float grip,
        float timeStep,
        float groundHeight,
        float gravity)
      {
        float3 pointVelocity;
        float3 float3 = (pointVelocity = ObjectUtils.CalculatePointVelocity(position - origin, moving)) with
        {
          y = 0.0f
        };
        float num1 = pointVelocity.y - gravity * timeStep;
        position.y += num1 * timeStep;
        float num2 = math.max(0.0f, groundHeight - position.y) / timeStep;
        float3 pointVelocityDelta = MathUtils.ClampLength(-float3, grip * math.min(timeStep, num2 / gravity));
        pointVelocityDelta.y += num2;
        return pointVelocityDelta;
      }

      private float3 CalculatePointAngularVelocityDelta(
        float3 position,
        float3 origin,
        float3 velocityDelta,
        float3 momentOfInertia)
      {
        return math.cross(position - origin, velocityDelta) / momentOfInertia;
      }

      private void GetGroundHeight(
        Quad3 cornerPositions,
        float3 velocity,
        float timeStep,
        float gravity,
        out float4 heights)
      {
        float4 b;
        // ISSUE: reference to a compiler-generated field
        b.x = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, cornerPositions.a);
        // ISSUE: reference to a compiler-generated field
        b.y = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, cornerPositions.b);
        // ISSUE: reference to a compiler-generated field
        b.z = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, cornerPositions.c);
        // ISSUE: reference to a compiler-generated field
        b.w = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, cornerPositions.d);
        float4 float4 = new float4(cornerPositions.a.y, cornerPositions.b.y, cornerPositions.c.y, cornerPositions.d.y);
        heights = math.select((float4) float.MinValue, b, b < float4 + 4f);
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        HumanMoveSystem.UpdateTransformDataJob.NetIterator iterator = new HumanMoveSystem.UpdateTransformDataJob.NetIterator();
        // ISSUE: reference to a compiler-generated field
        iterator.m_Bounds = MathUtils.Bounds(cornerPositions);
        // ISSUE: reference to a compiler-generated field
        iterator.m_Bounds.min.y += (math.min(0.0f, velocity.y) - gravity * timeStep) * timeStep;
        // ISSUE: reference to a compiler-generated field
        iterator.m_Bounds.max.y += math.max(0.0f, velocity.y) * timeStep;
        // ISSUE: reference to a compiler-generated field
        iterator.m_CornerPositions = cornerPositions;
        // ISSUE: reference to a compiler-generated field
        iterator.m_GroundHeights = heights;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        iterator.m_CompositionData = this.m_CompositionData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        iterator.m_OrphanData = this.m_OrphanData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        iterator.m_NodeData = this.m_NodeData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        iterator.m_NodeGeometryData = this.m_NodeGeometryData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        iterator.m_EdgeGeometryData = this.m_EdgeGeometryData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        iterator.m_StartNodeGeometryData = this.m_StartNodeGeometryData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        iterator.m_EndNodeGeometryData = this.m_EndNodeGeometryData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        iterator.m_PrefabCompositionData = this.m_PrefabCompositionData;
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<HumanMoveSystem.UpdateTransformDataJob.NetIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        heights = iterator.m_GroundHeights;
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

      private struct NetIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds3 m_Bounds;
        public Quad3 m_CornerPositions;
        public float4 m_GroundHeights;
        public ComponentLookup<Composition> m_CompositionData;
        public ComponentLookup<Orphan> m_OrphanData;
        public ComponentLookup<Game.Net.Node> m_NodeData;
        public ComponentLookup<NodeGeometry> m_NodeGeometryData;
        public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
        public ComponentLookup<StartNodeGeometry> m_StartNodeGeometryData;
        public ComponentLookup<EndNodeGeometry> m_EndNodeGeometryData;
        public ComponentLookup<NetCompositionData> m_PrefabCompositionData;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity item)
        {
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds))
            return;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CompositionData.HasComponent(item))
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckEdge(item);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_OrphanData.HasComponent(item))
              return;
            // ISSUE: reference to a compiler-generated method
            this.CheckNode(item);
          }
        }

        private void CheckNode(Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(this.m_NodeGeometryData[entity].m_Bounds, this.m_Bounds))
            return;
          // ISSUE: reference to a compiler-generated field
          Game.Net.Node node = this.m_NodeData[entity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NetCompositionData netCompositionData = this.m_PrefabCompositionData[this.m_OrphanData[entity].m_Composition];
          float3 position = node.m_Position;
          position.y += netCompositionData.m_SurfaceHeight.max;
          // ISSUE: reference to a compiler-generated method
          this.CheckCircle(position, netCompositionData.m_Width * 0.5f);
        }

        private void CheckEdge(Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          EdgeGeometry edgeGeometry = this.m_EdgeGeometryData[entity];
          // ISSUE: reference to a compiler-generated field
          EdgeNodeGeometry geometry1 = this.m_StartNodeGeometryData[entity].m_Geometry;
          // ISSUE: reference to a compiler-generated field
          EdgeNodeGeometry geometry2 = this.m_EndNodeGeometryData[entity].m_Geometry;
          bool3 x;
          // ISSUE: reference to a compiler-generated field
          x.x = MathUtils.Intersect(edgeGeometry.m_Bounds, this.m_Bounds);
          // ISSUE: reference to a compiler-generated field
          x.y = MathUtils.Intersect(geometry1.m_Bounds, this.m_Bounds);
          // ISSUE: reference to a compiler-generated field
          x.z = MathUtils.Intersect(geometry2.m_Bounds, this.m_Bounds);
          if (!math.any(x))
            return;
          // ISSUE: reference to a compiler-generated field
          Composition composition = this.m_CompositionData[entity];
          if (x.x)
          {
            // ISSUE: reference to a compiler-generated field
            NetCompositionData prefabCompositionData = this.m_PrefabCompositionData[composition.m_Edge];
            // ISSUE: reference to a compiler-generated method
            this.CheckSegment(edgeGeometry.m_Start, prefabCompositionData);
            // ISSUE: reference to a compiler-generated method
            this.CheckSegment(edgeGeometry.m_End, prefabCompositionData);
          }
          if (x.y)
          {
            // ISSUE: reference to a compiler-generated field
            NetCompositionData prefabCompositionData = this.m_PrefabCompositionData[composition.m_StartNode];
            if ((double) geometry1.m_MiddleRadius > 0.0)
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckSegment(geometry1.m_Left, prefabCompositionData);
              Game.Net.Segment right1 = geometry1.m_Right;
              Game.Net.Segment right2 = geometry1.m_Right;
              right1.m_Right = MathUtils.Lerp(geometry1.m_Right.m_Left, geometry1.m_Right.m_Right, 0.5f);
              right2.m_Left = MathUtils.Lerp(geometry1.m_Right.m_Left, geometry1.m_Right.m_Right, 0.5f);
              right1.m_Right.d = geometry1.m_Middle.d;
              right2.m_Left.d = geometry1.m_Middle.d;
              // ISSUE: reference to a compiler-generated method
              this.CheckSegment(right1, prefabCompositionData);
              // ISSUE: reference to a compiler-generated method
              this.CheckSegment(right2, prefabCompositionData);
            }
            else
            {
              Game.Net.Segment left = geometry1.m_Left;
              Game.Net.Segment right = geometry1.m_Right;
              // ISSUE: reference to a compiler-generated method
              this.CheckSegment(left, prefabCompositionData);
              // ISSUE: reference to a compiler-generated method
              this.CheckSegment(right, prefabCompositionData);
              left.m_Right = geometry1.m_Middle;
              right.m_Left = geometry1.m_Middle;
              // ISSUE: reference to a compiler-generated method
              this.CheckSegment(left, prefabCompositionData);
              // ISSUE: reference to a compiler-generated method
              this.CheckSegment(right, prefabCompositionData);
            }
          }
          if (!x.z)
            return;
          // ISSUE: reference to a compiler-generated field
          NetCompositionData prefabCompositionData1 = this.m_PrefabCompositionData[composition.m_EndNode];
          if ((double) geometry2.m_MiddleRadius > 0.0)
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckSegment(geometry2.m_Left, prefabCompositionData1);
            Game.Net.Segment right3 = geometry2.m_Right;
            Game.Net.Segment right4 = geometry2.m_Right;
            right3.m_Right = MathUtils.Lerp(geometry2.m_Right.m_Left, geometry2.m_Right.m_Right, 0.5f);
            right3.m_Right.d = geometry2.m_Middle.d;
            right4.m_Left = right3.m_Right;
            // ISSUE: reference to a compiler-generated method
            this.CheckSegment(right3, prefabCompositionData1);
            // ISSUE: reference to a compiler-generated method
            this.CheckSegment(right4, prefabCompositionData1);
          }
          else
          {
            Game.Net.Segment left = geometry2.m_Left;
            Game.Net.Segment right = geometry2.m_Right;
            // ISSUE: reference to a compiler-generated method
            this.CheckSegment(left, prefabCompositionData1);
            // ISSUE: reference to a compiler-generated method
            this.CheckSegment(right, prefabCompositionData1);
            left.m_Right = geometry2.m_Middle;
            right.m_Left = geometry2.m_Middle;
            // ISSUE: reference to a compiler-generated method
            this.CheckSegment(left, prefabCompositionData1);
            // ISSUE: reference to a compiler-generated method
            this.CheckSegment(right, prefabCompositionData1);
          }
        }

        private void CheckSegment(Game.Net.Segment segment, NetCompositionData prefabCompositionData)
        {
          float3 float3_1 = segment.m_Left.a;
          float3 float3_2 = segment.m_Right.a;
          float3_1.y += prefabCompositionData.m_SurfaceHeight.max;
          float3_2.y += prefabCompositionData.m_SurfaceHeight.max;
          Bounds3 bounds3_1 = MathUtils.Bounds(float3_1, float3_2);
          for (int index = 1; index <= 8; ++index)
          {
            float t = (float) index / 8f;
            float3 float3_3 = MathUtils.Position(segment.m_Left, t);
            float3 float3_4 = MathUtils.Position(segment.m_Right, t);
            float3_3.y += prefabCompositionData.m_SurfaceHeight.max;
            float3_4.y += prefabCompositionData.m_SurfaceHeight.max;
            Bounds3 bounds3_2 = MathUtils.Bounds(float3_3, float3_4);
            // ISSUE: reference to a compiler-generated field
            if (MathUtils.Intersect(bounds3_1 | bounds3_2, this.m_Bounds))
            {
              // ISSUE: reference to a compiler-generated method
              this.CheckTriangle(new Triangle3(float3_1, float3_2, float3_3));
              // ISSUE: reference to a compiler-generated method
              this.CheckTriangle(new Triangle3(float3_4, float3_3, float3_2));
            }
            float3_1 = float3_3;
            float3_2 = float3_4;
            bounds3_1 = bounds3_2;
          }
        }

        private void CheckCircle(float3 center, float radius)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckCircle(center, radius, this.m_CornerPositions.a, ref this.m_GroundHeights.x);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckCircle(center, radius, this.m_CornerPositions.b, ref this.m_GroundHeights.y);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckCircle(center, radius, this.m_CornerPositions.c, ref this.m_GroundHeights.z);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckCircle(center, radius, this.m_CornerPositions.d, ref this.m_GroundHeights.w);
        }

        private void CheckTriangle(Triangle3 triangle)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckTriangle(triangle, this.m_CornerPositions.a, ref this.m_GroundHeights.x);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckTriangle(triangle, this.m_CornerPositions.b, ref this.m_GroundHeights.y);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckTriangle(triangle, this.m_CornerPositions.c, ref this.m_GroundHeights.z);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CheckTriangle(triangle, this.m_CornerPositions.d, ref this.m_GroundHeights.w);
        }

        private void CheckCircle(
          float3 center,
          float radius,
          float3 position,
          ref float groundHeight)
        {
          if ((double) math.distance(center.xz, position.xz) > (double) radius)
            return;
          float y = center.y;
          groundHeight = math.select(groundHeight, y, (double) y < (double) position.y + 4.0 & (double) y > (double) groundHeight);
        }

        private void CheckTriangle(Triangle3 triangle, float3 position, ref float groundHeight)
        {
          float2 t;
          if (!MathUtils.Intersect(triangle.xz, position.xz, out t))
            return;
          float y = MathUtils.Position(triangle, t).y;
          groundHeight = math.select(groundHeight, y, (double) y < (double) position.y + 4.0 & (double) y > (double) groundHeight);
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Human> __Game_Creatures_Human_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Stumbling> __Game_Creatures_Stumbling_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<MeshGroup> __Game_Rendering_MeshGroup_RO_BufferTypeHandle;
      public ComponentTypeHandle<HumanNavigation> __Game_Creatures_HumanNavigation_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Moving> __Game_Objects_Moving_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RW_ComponentTypeHandle;
      public BufferTypeHandle<TransformFrame> __Game_Objects_TransformFrame_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Orphan> __Game_Net_Orphan_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NodeGeometry> __Game_Net_NodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StartNodeGeometry> __Game_Net_StartNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EndNodeGeometry> __Game_Net_EndNodeGeometry_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SubMeshGroup> __Game_Prefabs_SubMeshGroup_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<CharacterElement> __Game_Prefabs_CharacterElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<AnimationClip> __Game_Prefabs_AnimationClip_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<AnimationMotion> __Game_Prefabs_AnimationMotion_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubMesh> __Game_Prefabs_SubMesh_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ActivityLocationElement> __Game_Prefabs_ActivityLocationElement_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Human_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Human>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Stumbling_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Stumbling>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshGroup_RO_BufferTypeHandle = state.GetBufferTypeHandle<MeshGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_HumanNavigation_RW_ComponentTypeHandle = state.GetComponentTypeHandle<HumanNavigation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Moving_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Moving>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TransformFrame_RW_BufferTypeHandle = state.GetBufferTypeHandle<TransformFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Orphan_RO_ComponentLookup = state.GetComponentLookup<Orphan>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_NodeGeometry_RO_ComponentLookup = state.GetComponentLookup<NodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_StartNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<StartNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EndNodeGeometry_RO_ComponentLookup = state.GetComponentLookup<EndNodeGeometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMeshGroup_RO_BufferLookup = state.GetBufferLookup<SubMeshGroup>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CharacterElement_RO_BufferLookup = state.GetBufferLookup<CharacterElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AnimationClip_RO_BufferLookup = state.GetBufferLookup<AnimationClip>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AnimationMotion_RO_BufferLookup = state.GetBufferLookup<AnimationMotion>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RO_BufferLookup = state.GetBufferLookup<SubMesh>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ActivityLocationElement_RO_BufferLookup = state.GetBufferLookup<ActivityLocationElement>(true);
      }
    }
  }
}
