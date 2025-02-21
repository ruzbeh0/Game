// Decompiled with JetBrains decompiler
// Type: Game.Simulation.BuildingConstructionSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Common;
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
  public class BuildingConstructionSystem : GameSystemBase
  {
    private const int UPDATE_INTERVAL_BITS = 6;
    public const uint UPDATE_INTERVAL = 64;
    private SimulationSystem m_SimulationSystem;
    private TerrainSystem m_TerrainSystem;
    private ZoneSpawnSystem m_ZoneSpawnSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_BuildingQuery;
    private BuildingConstructionSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 64;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneSpawnSystem = this.World.GetOrCreateSystemManaged<ZoneSpawnSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<UnderConstruction>(), ComponentType.ReadOnly<Building>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_BuildingQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PointOfInterest_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubNet_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubAreaNode_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_MeshBatch_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CraneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Crane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_UnderConstruction_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle jobHandle = new BuildingConstructionSystem.BuildingConstructionJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_UnderConstructionType = this.__TypeHandle.__Game_Objects_UnderConstruction_RW_ComponentTypeHandle,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_CraneData = this.__TypeHandle.__Game_Objects_Crane_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabSpawnableObjectData = this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabCraneData = this.__TypeHandle.__Game_Prefabs_CraneData_RO_ComponentLookup,
        m_PrefabNetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_SubAreas = this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup,
        m_SubNets = this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_MeshBatches = this.__TypeHandle.__Game_Rendering_MeshBatch_RO_BufferLookup,
        m_PrefabSubAreas = this.__TypeHandle.__Game_Prefabs_SubArea_RO_BufferLookup,
        m_PrefabSubAreaNodes = this.__TypeHandle.__Game_Prefabs_SubAreaNode_RO_BufferLookup,
        m_PrefabSubNets = this.__TypeHandle.__Game_Prefabs_SubNet_RO_BufferLookup,
        m_PrefabPlaceholderElements = this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup,
        m_PointOfInterest = this.__TypeHandle.__Game_Common_PointOfInterest_RW_ComponentLookup,
        m_LefthandTraffic = this.m_CityConfigurationSystem.leftHandTraffic,
        m_DebugFastSpawn = this.m_ZoneSpawnSystem.debugFastSpawn,
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_RandomSeed = RandomSeed.Next(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_PreviousPrefabMap = this.m_TerrainSystem.GetBuildingUpgradeWriter(this.m_BuildingQuery.CalculateEntityCountWithoutFiltering())
      }.ScheduleParallel<BuildingConstructionSystem.BuildingConstructionJob>(this.m_BuildingQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.SetBuildingUpgradeWriterDependency(jobHandle);
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
    public BuildingConstructionSystem()
    {
    }

    [BurstCompile]
    private struct BuildingConstructionJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<UnderConstruction> m_UnderConstructionType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<Crane> m_CraneData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<SpawnableObjectData> m_PrefabSpawnableObjectData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<CraneData> m_PrefabCraneData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabNetGeometryData;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> m_SubAreas;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> m_SubNets;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<MeshBatch> m_MeshBatches;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubArea> m_PrefabSubAreas;
      [ReadOnly]
      public BufferLookup<SubAreaNode> m_PrefabSubAreaNodes;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubNet> m_PrefabSubNets;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> m_PrefabPlaceholderElements;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<PointOfInterest> m_PointOfInterest;
      [ReadOnly]
      public bool m_LefthandTraffic;
      [ReadOnly]
      public bool m_DebugFastSpawn;
      [ReadOnly]
      public uint m_SimulationFrame;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeParallelHashMap<Entity, Entity>.ParallelWriter m_PreviousPrefabMap;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<UnderConstruction> nativeArray2 = chunk.GetNativeArray<UnderConstruction>(ref this.m_UnderConstructionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray3 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        NativeParallelHashMap<Entity, int> selectedSpawnables = new NativeParallelHashMap<Entity, int>();
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Transform transform = nativeArray3[index];
          PrefabRef prefabRef = nativeArray4[index];
          ref UnderConstruction local = ref nativeArray2.ElementAt<UnderConstruction>(index);
          if (local.m_Progress < (byte) 100)
          {
            if (local.m_Speed == (byte) 0)
              local.m_Speed = (byte) random.NextInt(39, 89);
            // ISSUE: reference to a compiler-generated field
            if (this.m_DebugFastSpawn)
              local.m_Progress = (byte) 100;
            else if (local.m_Progress == (byte) 0)
            {
              ++local.m_Progress;
              // ISSUE: reference to a compiler-generated method
              this.UpdateCranes(ref random, entity, transform, prefabRef);
            }
            else
            {
              int num1;
              // ISSUE: reference to a compiler-generated field
              uint num2 = (uint) ((ulong) (num1 = (int) (this.m_SimulationFrame >> 6) + (int) local.m_Speed) * (ulong) local.m_Speed >> 7);
              uint num3 = (uint) ((ulong) (uint) (num1 + 1) * (ulong) local.m_Speed >> 7);
              local.m_Progress = (byte) math.min((int) byte.MaxValue, (int) local.m_Progress + ((int) num3 - (int) num2));
              if (random.NextInt(10) == 0)
              {
                // ISSUE: reference to a compiler-generated method
                this.UpdateCranes(ref random, entity, transform, prefabRef);
              }
            }
          }
          else
          {
            if (local.m_NewPrefab == Entity.Null)
              local.m_NewPrefab = prefabRef.m_Prefab;
            // ISSUE: reference to a compiler-generated method
            this.UpdatePrefab(unfilteredChunkIndex, entity, local.m_NewPrefab, transform, ref random, ref selectedSpawnables);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.RemoveComponent<UnderConstruction>(unfilteredChunkIndex, entity);
            // ISSUE: reference to a compiler-generated field
            this.m_PreviousPrefabMap.TryAdd(entity, prefabRef.m_Prefab);
          }
        }
        if (!selectedSpawnables.IsCreated)
          return;
        selectedSpawnables.Dispose();
      }

      private void UpdateCranes(
        ref Random random,
        Entity entity,
        Transform transform,
        PrefabRef prefabRef)
      {
        // ISSUE: reference to a compiler-generated field
        ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefabRef.m_Prefab];
        DynamicBuffer<Game.Objects.SubObject> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.TryGetBuffer(entity, out bufferData))
          return;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Entity subObject = bufferData[index].m_SubObject;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CraneData.HasComponent(subObject))
          {
            // ISSUE: reference to a compiler-generated field
            Transform transform1 = this.m_TransformData[subObject];
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef1 = this.m_PrefabRefData[subObject];
            float3 position = random.NextFloat3(objectGeometryData.m_Bounds.min, objectGeometryData.m_Bounds.max);
            position = ObjectUtils.LocalToWorld(transform, position);
            CraneData componentData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabCraneData.TryGetComponent(prefabRef1.m_Prefab, out componentData))
            {
              position = ObjectUtils.WorldToLocal(ObjectUtils.InverseTransform(transform1), position);
              float num = math.length(position.xz);
              if ((double) num < (double) componentData.m_DistanceRange.min)
                position.xz = math.normalizesafe(position.xz, new float2(0.0f, 1f)) * componentData.m_DistanceRange.min;
              else if ((double) num > (double) componentData.m_DistanceRange.max)
                position.xz = math.normalizesafe(position.xz, new float2(0.0f, 1f)) * componentData.m_DistanceRange.max;
              position = ObjectUtils.LocalToWorld(transform1, position);
            }
            // ISSUE: reference to a compiler-generated field
            PointOfInterest pointOfInterest = this.m_PointOfInterest[subObject] with
            {
              m_Position = position,
              m_IsValid = true
            };
            // ISSUE: reference to a compiler-generated field
            this.m_PointOfInterest[subObject] = pointOfInterest;
          }
        }
      }

      private void UpdatePrefab(
        int jobIndex,
        Entity entity,
        Entity newPrefab,
        Transform transform,
        ref Random random,
        ref NativeParallelHashMap<Entity, int> selectedSpawnables)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PrefabRef>(jobIndex, entity, new PrefabRef(newPrefab));
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(jobIndex, entity, new Updated());
        DynamicBuffer<MeshBatch> bufferData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_MeshBatches.TryGetBuffer(entity, out bufferData1))
        {
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<MeshBatch> dynamicBuffer = this.m_CommandBuffer.SetBuffer<MeshBatch>(jobIndex, entity);
          dynamicBuffer.ResizeUninitialized(bufferData1.Length);
          for (int index = 0; index < bufferData1.Length; ++index)
          {
            MeshBatch meshBatch = bufferData1[index] with
            {
              m_MeshGroup = byte.MaxValue,
              m_MeshIndex = byte.MaxValue,
              m_TileIndex = byte.MaxValue
            };
            dynamicBuffer[index] = meshBatch;
          }
        }
        DynamicBuffer<Game.Objects.SubObject> bufferData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubObjects.TryGetBuffer(entity, out bufferData2))
        {
          for (int index = 0; index < bufferData2.Length; ++index)
          {
            Entity subObject = bufferData2[index].m_SubObject;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Updated>(jobIndex, subObject, new Updated());
          }
        }
        DynamicBuffer<Game.Areas.SubArea> bufferData3;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubAreas.TryGetBuffer(entity, out bufferData3))
        {
          for (int index = 0; index < bufferData3.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, bufferData3[index].m_Area);
          }
        }
        DynamicBuffer<Game.Prefabs.SubArea> bufferData4;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabSubAreas.TryGetBuffer(newPrefab, out bufferData4))
        {
          if (!bufferData3.IsCreated)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddBuffer<Game.Areas.SubArea>(jobIndex, entity);
          }
          if (selectedSpawnables.IsCreated)
            selectedSpawnables.Clear();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CreateAreas(jobIndex, entity, transform, bufferData4, this.m_PrefabSubAreaNodes[newPrefab], ref random, ref selectedSpawnables);
        }
        else if (bufferData3.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Game.Areas.SubArea>(jobIndex, entity);
        }
        DynamicBuffer<Game.Net.SubNet> bufferData5;
        // ISSUE: reference to a compiler-generated field
        if (this.m_SubNets.TryGetBuffer(entity, out bufferData5))
        {
          for (int index1 = 0; index1 < bufferData5.Length; ++index1)
          {
            Game.Net.SubNet subNet = bufferData5[index1];
            bool flag = true;
            DynamicBuffer<ConnectedEdge> bufferData6;
            // ISSUE: reference to a compiler-generated field
            if (this.m_ConnectedEdges.TryGetBuffer(subNet.m_SubNet, out bufferData6))
            {
              for (int index2 = 0; index2 < bufferData6.Length; ++index2)
              {
                Entity edge1 = bufferData6[index2].m_Edge;
                Owner componentData;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if ((!this.m_OwnerData.TryGetComponent(edge1, out componentData) || !(componentData.m_Owner == entity) && !this.m_DeletedData.HasComponent(componentData.m_Owner)) && !this.m_DeletedData.HasComponent(edge1))
                {
                  // ISSUE: reference to a compiler-generated field
                  Edge edge2 = this.m_EdgeData[edge1];
                  if (edge2.m_Start == subNet.m_SubNet || edge2.m_End == subNet.m_SubNet)
                    flag = false;
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Updated>(jobIndex, edge1, new Updated());
                  // ISSUE: reference to a compiler-generated field
                  if (!this.m_DeletedData.HasComponent(edge2.m_Start))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<Updated>(jobIndex, edge2.m_Start, new Updated());
                  }
                  // ISSUE: reference to a compiler-generated field
                  if (!this.m_DeletedData.HasComponent(edge2.m_End))
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<Updated>(jobIndex, edge2.m_End, new Updated());
                  }
                }
              }
            }
            if (flag)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Deleted>(jobIndex, subNet.m_SubNet);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Owner>(jobIndex, subNet.m_SubNet);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(jobIndex, subNet.m_SubNet, new Updated());
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabSubNets.HasBuffer(newPrefab))
        {
          if (bufferData5.IsCreated)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetBuffer<Game.Net.SubNet>(jobIndex, entity);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddBuffer<Game.Net.SubNet>(jobIndex, entity);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.CreateNets(jobIndex, entity, transform, this.m_PrefabSubNets[newPrefab], ref random);
        }
        else
        {
          if (!bufferData5.IsCreated)
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Game.Net.SubNet>(jobIndex, entity);
        }
      }

      private void CreateAreas(
        int jobIndex,
        Entity owner,
        Transform transform,
        DynamicBuffer<Game.Prefabs.SubArea> subAreas,
        DynamicBuffer<SubAreaNode> subAreaNodes,
        ref Random random,
        ref NativeParallelHashMap<Entity, int> selectedSpawnables)
      {
        for (int index1 = 0; index1 < subAreas.Length; ++index1)
        {
          Game.Prefabs.SubArea subArea = subAreas[index1];
          DynamicBuffer<PlaceholderObjectElement> bufferData;
          int seed;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabPlaceholderElements.TryGetBuffer(subArea.m_Prefab, out bufferData))
          {
            if (!selectedSpawnables.IsCreated)
              selectedSpawnables = new NativeParallelHashMap<Entity, int>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
            // ISSUE: reference to a compiler-generated field
            if (!AreaUtils.SelectAreaPrefab(bufferData, this.m_PrefabSpawnableObjectData, selectedSpawnables, ref random, out subArea.m_Prefab, out seed))
              continue;
          }
          else
            seed = random.NextInt();
          // ISSUE: reference to a compiler-generated field
          Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex);
          CreationDefinition component = new CreationDefinition();
          component.m_Prefab = subArea.m_Prefab;
          component.m_Owner = owner;
          component.m_RandomSeed = seed;
          component.m_Flags |= CreationFlags.Permanent;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<CreationDefinition>(jobIndex, entity, component);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(jobIndex, entity, new Updated());
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Areas.Node> dynamicBuffer = this.m_CommandBuffer.AddBuffer<Game.Areas.Node>(jobIndex, entity);
          dynamicBuffer.ResizeUninitialized(subArea.m_NodeRange.y - subArea.m_NodeRange.x + 1);
          // ISSUE: reference to a compiler-generated method
          int index2 = ObjectToolBaseSystem.GetFirstNodeIndex(subAreaNodes, subArea.m_NodeRange);
          int index3 = 0;
          for (int x = subArea.m_NodeRange.x; x <= subArea.m_NodeRange.y; ++x)
          {
            float3 position = subAreaNodes[index2].m_Position;
            float3 world = ObjectUtils.LocalToWorld(transform, position);
            int parentMesh = subAreaNodes[index2].m_ParentMesh;
            float elevation = math.select(float.MinValue, position.y, parentMesh >= 0);
            dynamicBuffer[index3] = new Game.Areas.Node(world, elevation);
            ++index3;
            if (++index2 == subArea.m_NodeRange.y)
              index2 = subArea.m_NodeRange.x;
          }
        }
      }

      private void CreateNets(
        int jobIndex,
        Entity owner,
        Transform transform,
        DynamicBuffer<Game.Prefabs.SubNet> subNets,
        ref Random random)
      {
        NativeList<float4> nodePositions = new NativeList<float4>(subNets.Length * 2, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index = 0; index < subNets.Length; ++index)
        {
          Game.Prefabs.SubNet subNet = subNets[index];
          float4 float4;
          if (subNet.m_NodeIndex.x >= 0)
          {
            while (nodePositions.Length <= subNet.m_NodeIndex.x)
            {
              ref NativeList<float4> local1 = ref nodePositions;
              float4 = new float4();
              ref float4 local2 = ref float4;
              local1.Add(in local2);
            }
            nodePositions[subNet.m_NodeIndex.x] += new float4(subNet.m_Curve.a, 1f);
          }
          if (subNet.m_NodeIndex.y >= 0)
          {
            while (nodePositions.Length <= subNet.m_NodeIndex.y)
            {
              ref NativeList<float4> local3 = ref nodePositions;
              float4 = new float4();
              ref float4 local4 = ref float4;
              local3.Add(in local4);
            }
            nodePositions[subNet.m_NodeIndex.y] += new float4(subNet.m_Curve.d, 1f);
          }
        }
        for (int index = 0; index < nodePositions.Length; ++index)
          nodePositions[index] /= math.max(1f, nodePositions[index].w);
        for (int index = 0; index < subNets.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          Game.Prefabs.SubNet subNet = NetUtils.GetSubNet(subNets, index, this.m_LefthandTraffic, ref this.m_PrefabNetGeometryData);
          // ISSUE: reference to a compiler-generated method
          this.CreateSubNet(jobIndex, subNet.m_Prefab, subNet.m_Curve, subNet.m_NodeIndex, subNet.m_ParentMesh, subNet.m_Upgrades, nodePositions, owner, transform, ref random);
        }
        nodePositions.Dispose();
      }

      private void CreateSubNet(
        int jobIndex,
        Entity netPrefab,
        Bezier4x3 curve,
        int2 nodeIndex,
        int2 parentMesh,
        CompositionFlags upgrades,
        NativeList<float4> nodePositions,
        Entity owner,
        Transform transform,
        ref Random random)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex);
        CreationDefinition component1 = new CreationDefinition();
        component1.m_Prefab = netPrefab;
        component1.m_Owner = owner;
        component1.m_RandomSeed = random.NextInt();
        component1.m_Flags |= CreationFlags.Permanent;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CreationDefinition>(jobIndex, entity, component1);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(jobIndex, entity, new Updated());
        NetCourse component2 = new NetCourse()
        {
          m_Curve = ObjectUtils.LocalToWorld(transform.m_Position, transform.m_Rotation, curve)
        };
        component2.m_StartPosition.m_Position = component2.m_Curve.a;
        component2.m_StartPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.StartTangent(component2.m_Curve), transform.m_Rotation);
        component2.m_StartPosition.m_CourseDelta = 0.0f;
        component2.m_StartPosition.m_Elevation = (float2) curve.a.y;
        component2.m_StartPosition.m_ParentMesh = parentMesh.x;
        if (nodeIndex.x >= 0)
          component2.m_StartPosition.m_Position = ObjectUtils.LocalToWorld(transform.m_Position, transform.m_Rotation, nodePositions[nodeIndex.x].xyz);
        component2.m_EndPosition.m_Position = component2.m_Curve.d;
        component2.m_EndPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.EndTangent(component2.m_Curve), transform.m_Rotation);
        component2.m_EndPosition.m_CourseDelta = 1f;
        component2.m_EndPosition.m_Elevation = (float2) curve.d.y;
        component2.m_EndPosition.m_ParentMesh = parentMesh.y;
        if (nodeIndex.y >= 0)
          component2.m_EndPosition.m_Position = ObjectUtils.LocalToWorld(transform.m_Position, transform.m_Rotation, nodePositions[nodeIndex.y].xyz);
        component2.m_Length = MathUtils.Length(component2.m_Curve);
        component2.m_FixedIndex = -1;
        component2.m_StartPosition.m_Flags |= CoursePosFlags.IsFirst | CoursePosFlags.DisableMerge;
        component2.m_EndPosition.m_Flags |= CoursePosFlags.IsLast | CoursePosFlags.DisableMerge;
        if (component2.m_StartPosition.m_Position.Equals(component2.m_EndPosition.m_Position))
        {
          component2.m_StartPosition.m_Flags |= CoursePosFlags.IsLast;
          component2.m_EndPosition.m_Flags |= CoursePosFlags.IsFirst;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<NetCourse>(jobIndex, entity, component2);
        if (!(upgrades != new CompositionFlags()))
          return;
        Upgraded component3 = new Upgraded()
        {
          m_Flags = upgrades
        };
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Upgraded>(jobIndex, entity, component3);
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
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<UnderConstruction> __Game_Objects_UnderConstruction_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Crane> __Game_Objects_Crane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableObjectData> __Game_Prefabs_SpawnableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CraneData> __Game_Prefabs_CraneData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> __Game_Areas_SubArea_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> __Game_Net_SubNet_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<MeshBatch> __Game_Rendering_MeshBatch_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubArea> __Game_Prefabs_SubArea_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubAreaNode> __Game_Prefabs_SubAreaNode_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubNet> __Game_Prefabs_SubNet_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> __Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup;
      public ComponentLookup<PointOfInterest> __Game_Common_PointOfInterest_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_UnderConstruction_RW_ComponentTypeHandle = state.GetComponentTypeHandle<UnderConstruction>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Crane_RO_ComponentLookup = state.GetComponentLookup<Crane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup = state.GetComponentLookup<SpawnableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CraneData_RO_ComponentLookup = state.GetComponentLookup<CraneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Areas.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubNet_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshBatch_RO_BufferLookup = state.GetBufferLookup<MeshBatch>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubAreaNode_RO_BufferLookup = state.GetBufferLookup<SubAreaNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubNet_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup = state.GetBufferLookup<PlaceholderObjectElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PointOfInterest_RW_ComponentLookup = state.GetComponentLookup<PointOfInterest>();
      }
    }
  }
}
