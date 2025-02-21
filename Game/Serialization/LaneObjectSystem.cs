// Decompiled with JetBrains decompiler
// Type: Game.Serialization.LaneObjectSystem
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
using Game.Simulation;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class LaneObjectSystem : GameSystemBase
  {
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private EntityQuery m_Query;
    private LaneObjectSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_Query = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[10]
        {
          ComponentType.ReadOnly<CarCurrentLane>(),
          ComponentType.ReadOnly<CarTrailerLane>(),
          ComponentType.ReadOnly<ParkedCar>(),
          ComponentType.ReadOnly<ParkedTrain>(),
          ComponentType.ReadOnly<WatercraftCurrentLane>(),
          ComponentType.ReadOnly<AircraftCurrentLane>(),
          ComponentType.ReadOnly<TrainCurrentLane>(),
          ComponentType.ReadOnly<HumanCurrentLane>(),
          ComponentType.ReadOnly<AnimalCurrentLane>(),
          ComponentType.ReadOnly<BlockedLane>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_Query);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LaneObject_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_BlockedLane_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_AircraftCurrentLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_WatercraftCurrentLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedTrain_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarTrailerLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new LaneObjectSystem.LaneObjectJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CarCurrentLaneType = this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RO_ComponentTypeHandle,
        m_CarTrailerLaneType = this.__TypeHandle.__Game_Vehicles_CarTrailerLane_RO_ComponentTypeHandle,
        m_ParkedCarType = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentTypeHandle,
        m_ParkedTrainType = this.__TypeHandle.__Game_Vehicles_ParkedTrain_RO_ComponentTypeHandle,
        m_WatercraftCurrentLaneType = this.__TypeHandle.__Game_Vehicles_WatercraftCurrentLane_RO_ComponentTypeHandle,
        m_AircraftCurrentLaneType = this.__TypeHandle.__Game_Vehicles_AircraftCurrentLane_RO_ComponentTypeHandle,
        m_TrainCurrentLaneType = this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RO_ComponentTypeHandle,
        m_HumanCurrentLaneType = this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RO_ComponentTypeHandle,
        m_AnimalCurrentLaneType = this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RO_ComponentTypeHandle,
        m_BlockedLaneType = this.__TypeHandle.__Game_Objects_BlockedLane_RO_BufferTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_ObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_LaneObjects = this.__TypeHandle.__Game_Net_LaneObject_RW_BufferLookup,
        m_SearchTree = this.m_ObjectSearchSystem.GetMovingSearchTree(false, out dependencies)
      }.Schedule<LaneObjectSystem.LaneObjectJob>(this.m_Query, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddMovingSearchTreeWriter(jobHandle);
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
    public LaneObjectSystem()
    {
    }

    [BurstCompile]
    private struct LaneObjectJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<CarCurrentLane> m_CarCurrentLaneType;
      [ReadOnly]
      public ComponentTypeHandle<CarTrailerLane> m_CarTrailerLaneType;
      [ReadOnly]
      public ComponentTypeHandle<ParkedCar> m_ParkedCarType;
      [ReadOnly]
      public ComponentTypeHandle<ParkedTrain> m_ParkedTrainType;
      [ReadOnly]
      public ComponentTypeHandle<WatercraftCurrentLane> m_WatercraftCurrentLaneType;
      [ReadOnly]
      public ComponentTypeHandle<AircraftCurrentLane> m_AircraftCurrentLaneType;
      [ReadOnly]
      public ComponentTypeHandle<TrainCurrentLane> m_TrainCurrentLaneType;
      [ReadOnly]
      public ComponentTypeHandle<HumanCurrentLane> m_HumanCurrentLaneType;
      [ReadOnly]
      public ComponentTypeHandle<AnimalCurrentLane> m_AnimalCurrentLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<BlockedLane> m_BlockedLaneType;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_ObjectGeometryData;
      public BufferLookup<LaneObject> m_LaneObjects;
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_SearchTree;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray2 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CarCurrentLane> nativeArray4 = chunk.GetNativeArray<CarCurrentLane>(ref this.m_CarCurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CarTrailerLane> nativeArray5 = chunk.GetNativeArray<CarTrailerLane>(ref this.m_CarTrailerLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ParkedCar> nativeArray6 = chunk.GetNativeArray<ParkedCar>(ref this.m_ParkedCarType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ParkedTrain> nativeArray7 = chunk.GetNativeArray<ParkedTrain>(ref this.m_ParkedTrainType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WatercraftCurrentLane> nativeArray8 = chunk.GetNativeArray<WatercraftCurrentLane>(ref this.m_WatercraftCurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<AircraftCurrentLane> nativeArray9 = chunk.GetNativeArray<AircraftCurrentLane>(ref this.m_AircraftCurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<TrainCurrentLane> nativeArray10 = chunk.GetNativeArray<TrainCurrentLane>(ref this.m_TrainCurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<HumanCurrentLane> nativeArray11 = chunk.GetNativeArray<HumanCurrentLane>(ref this.m_HumanCurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<AnimalCurrentLane> nativeArray12 = chunk.GetNativeArray<AnimalCurrentLane>(ref this.m_AnimalCurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<BlockedLane> bufferAccessor = chunk.GetBufferAccessor<BlockedLane>(ref this.m_BlockedLaneType);
        for (int index = 0; index < nativeArray4.Length; ++index)
        {
          Entity laneObject = nativeArray1[index];
          CarCurrentLane carCurrentLane = nativeArray4[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneObjects.HasBuffer(carCurrentLane.m_Lane))
          {
            // ISSUE: reference to a compiler-generated field
            NetUtils.AddLaneObject(this.m_LaneObjects[carCurrentLane.m_Lane], laneObject, carCurrentLane.m_CurvePosition.xy);
          }
          else
          {
            Transform transform = nativeArray2[index];
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData geometryData = this.m_ObjectGeometryData[nativeArray3[index].m_Prefab];
            Bounds3 bounds = ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, geometryData);
            // ISSUE: reference to a compiler-generated field
            this.m_SearchTree.Add(laneObject, new QuadTreeBoundsXZ(bounds));
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneObjects.HasBuffer(carCurrentLane.m_ChangeLane))
          {
            // ISSUE: reference to a compiler-generated field
            NetUtils.AddLaneObject(this.m_LaneObjects[carCurrentLane.m_ChangeLane], laneObject, carCurrentLane.m_CurvePosition.xy);
          }
        }
        for (int index = 0; index < nativeArray5.Length; ++index)
        {
          Entity laneObject = nativeArray1[index];
          CarTrailerLane carTrailerLane = nativeArray5[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneObjects.HasBuffer(carTrailerLane.m_Lane))
          {
            // ISSUE: reference to a compiler-generated field
            NetUtils.AddLaneObject(this.m_LaneObjects[carTrailerLane.m_Lane], laneObject, carTrailerLane.m_CurvePosition.xy);
          }
          else
          {
            Transform transform = nativeArray2[index];
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData geometryData = this.m_ObjectGeometryData[nativeArray3[index].m_Prefab];
            Bounds3 bounds = ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, geometryData);
            // ISSUE: reference to a compiler-generated field
            this.m_SearchTree.Add(laneObject, new QuadTreeBoundsXZ(bounds));
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneObjects.HasBuffer(carTrailerLane.m_NextLane))
          {
            // ISSUE: reference to a compiler-generated field
            NetUtils.AddLaneObject(this.m_LaneObjects[carTrailerLane.m_NextLane], laneObject, carTrailerLane.m_NextPosition.xy);
          }
        }
        for (int index = 0; index < nativeArray6.Length; ++index)
        {
          Entity laneObject = nativeArray1[index];
          ParkedCar parkedCar = nativeArray6[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneObjects.HasBuffer(parkedCar.m_Lane))
          {
            // ISSUE: reference to a compiler-generated field
            NetUtils.AddLaneObject(this.m_LaneObjects[parkedCar.m_Lane], laneObject, (float2) parkedCar.m_CurvePosition);
          }
          else
          {
            Transform transform = nativeArray2[index];
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData geometryData = this.m_ObjectGeometryData[nativeArray3[index].m_Prefab];
            Bounds3 bounds = ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, geometryData);
            // ISSUE: reference to a compiler-generated field
            this.m_SearchTree.Add(laneObject, new QuadTreeBoundsXZ(bounds));
          }
        }
        for (int index = 0; index < nativeArray8.Length; ++index)
        {
          Entity laneObject = nativeArray1[index];
          WatercraftCurrentLane watercraftCurrentLane = nativeArray8[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneObjects.HasBuffer(watercraftCurrentLane.m_Lane))
          {
            // ISSUE: reference to a compiler-generated field
            NetUtils.AddLaneObject(this.m_LaneObjects[watercraftCurrentLane.m_Lane], laneObject, watercraftCurrentLane.m_CurvePosition.xy);
          }
          else
          {
            Transform transform = nativeArray2[index];
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData geometryData = this.m_ObjectGeometryData[nativeArray3[index].m_Prefab];
            Bounds3 bounds = ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, geometryData);
            // ISSUE: reference to a compiler-generated field
            this.m_SearchTree.Add(laneObject, new QuadTreeBoundsXZ(bounds));
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneObjects.HasBuffer(watercraftCurrentLane.m_ChangeLane))
          {
            // ISSUE: reference to a compiler-generated field
            NetUtils.AddLaneObject(this.m_LaneObjects[watercraftCurrentLane.m_ChangeLane], laneObject, watercraftCurrentLane.m_CurvePosition.xy);
          }
        }
        for (int index = 0; index < nativeArray9.Length; ++index)
        {
          Entity laneObject = nativeArray1[index];
          AircraftCurrentLane aircraftCurrentLane = nativeArray9[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneObjects.HasBuffer(aircraftCurrentLane.m_Lane))
          {
            // ISSUE: reference to a compiler-generated field
            NetUtils.AddLaneObject(this.m_LaneObjects[aircraftCurrentLane.m_Lane], laneObject, aircraftCurrentLane.m_CurvePosition.xy);
          }
          // ISSUE: reference to a compiler-generated field
          if (!this.m_LaneObjects.HasBuffer(aircraftCurrentLane.m_Lane) || (aircraftCurrentLane.m_LaneFlags & AircraftLaneFlags.Flying) != (AircraftLaneFlags) 0)
          {
            Transform transform = nativeArray2[index];
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData geometryData = this.m_ObjectGeometryData[nativeArray3[index].m_Prefab];
            Bounds3 bounds = ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, geometryData);
            // ISSUE: reference to a compiler-generated field
            this.m_SearchTree.Add(laneObject, new QuadTreeBoundsXZ(bounds));
          }
        }
        for (int index = 0; index < nativeArray10.Length; ++index)
        {
          Entity laneObject = nativeArray1[index];
          TrainCurrentLane currentLane = nativeArray10[index];
          float2 pos1;
          float2 pos2;
          TrainNavigationHelpers.GetCurvePositions(ref currentLane, out pos1, out pos2);
          DynamicBuffer<LaneObject> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneObjects.TryGetBuffer(currentLane.m_Front.m_Lane, out bufferData))
            NetUtils.AddLaneObject(bufferData, laneObject, pos1);
          // ISSUE: reference to a compiler-generated field
          if (currentLane.m_Rear.m_Lane != currentLane.m_Front.m_Lane && this.m_LaneObjects.TryGetBuffer(currentLane.m_Rear.m_Lane, out bufferData))
            NetUtils.AddLaneObject(bufferData, laneObject, pos2);
        }
        for (int index = 0; index < nativeArray7.Length; ++index)
        {
          Entity laneObject = nativeArray1[index];
          ParkedTrain parkedTrain = nativeArray7[index];
          float2 pos1;
          float2 pos2;
          TrainNavigationHelpers.GetCurvePositions(ref parkedTrain, out pos1, out pos2);
          DynamicBuffer<LaneObject> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneObjects.TryGetBuffer(parkedTrain.m_FrontLane, out bufferData))
            NetUtils.AddLaneObject(bufferData, laneObject, pos1);
          // ISSUE: reference to a compiler-generated field
          if (parkedTrain.m_RearLane != parkedTrain.m_FrontLane && this.m_LaneObjects.TryGetBuffer(parkedTrain.m_RearLane, out bufferData))
            NetUtils.AddLaneObject(bufferData, laneObject, pos2);
        }
        for (int index = 0; index < nativeArray11.Length; ++index)
        {
          Entity laneObject = nativeArray1[index];
          HumanCurrentLane humanCurrentLane = nativeArray11[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneObjects.HasBuffer(humanCurrentLane.m_Lane))
          {
            // ISSUE: reference to a compiler-generated field
            NetUtils.AddLaneObject(this.m_LaneObjects[humanCurrentLane.m_Lane], laneObject, humanCurrentLane.m_CurvePosition.xx);
          }
          else
          {
            Transform transform = nativeArray2[index];
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData geometryData = this.m_ObjectGeometryData[nativeArray3[index].m_Prefab];
            Bounds3 bounds = ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, geometryData);
            // ISSUE: reference to a compiler-generated field
            this.m_SearchTree.Add(laneObject, new QuadTreeBoundsXZ(bounds));
          }
        }
        for (int index = 0; index < nativeArray12.Length; ++index)
        {
          Entity laneObject = nativeArray1[index];
          AnimalCurrentLane animalCurrentLane = nativeArray12[index];
          // ISSUE: reference to a compiler-generated field
          if (this.m_LaneObjects.HasBuffer(animalCurrentLane.m_Lane))
          {
            // ISSUE: reference to a compiler-generated field
            NetUtils.AddLaneObject(this.m_LaneObjects[animalCurrentLane.m_Lane], laneObject, animalCurrentLane.m_CurvePosition.xx);
          }
          else
          {
            Transform transform = nativeArray2[index];
            // ISSUE: reference to a compiler-generated field
            ObjectGeometryData geometryData = this.m_ObjectGeometryData[nativeArray3[index].m_Prefab];
            Bounds3 bounds = ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, geometryData);
            // ISSUE: reference to a compiler-generated field
            this.m_SearchTree.Add(laneObject, new QuadTreeBoundsXZ(bounds));
          }
        }
        for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
        {
          Entity laneObject = nativeArray1[index1];
          DynamicBuffer<BlockedLane> dynamicBuffer = bufferAccessor[index1];
          for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
          {
            BlockedLane blockedLane = dynamicBuffer[index2];
            // ISSUE: reference to a compiler-generated field
            if (this.m_LaneObjects.HasBuffer(blockedLane.m_Lane))
            {
              // ISSUE: reference to a compiler-generated field
              NetUtils.AddLaneObject(this.m_LaneObjects[blockedLane.m_Lane], laneObject, blockedLane.m_CurvePosition);
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
      public ComponentTypeHandle<CarCurrentLane> __Game_Vehicles_CarCurrentLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CarTrailerLane> __Game_Vehicles_CarTrailerLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ParkedTrain> __Game_Vehicles_ParkedTrain_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WatercraftCurrentLane> __Game_Vehicles_WatercraftCurrentLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<AircraftCurrentLane> __Game_Vehicles_AircraftCurrentLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<TrainCurrentLane> __Game_Vehicles_TrainCurrentLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<HumanCurrentLane> __Game_Creatures_HumanCurrentLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<AnimalCurrentLane> __Game_Creatures_AnimalCurrentLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<BlockedLane> __Game_Objects_BlockedLane_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      public BufferLookup<LaneObject> __Game_Net_LaneObject_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarCurrentLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CarCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarTrailerLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CarTrailerLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedTrain_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ParkedTrain>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_WatercraftCurrentLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WatercraftCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_AircraftCurrentLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AircraftCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainCurrentLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<TrainCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_HumanCurrentLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HumanCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_AnimalCurrentLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<AnimalCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_BlockedLane_RO_BufferTypeHandle = state.GetBufferTypeHandle<BlockedLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LaneObject_RW_BufferLookup = state.GetBufferLookup<LaneObject>();
      }
    }
  }
}
