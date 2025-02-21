// Decompiled with JetBrains decompiler
// Type: Game.Objects.DestroySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Prefabs;
using Game.Rendering;
using Game.Simulation;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Objects
{
  [CompilerGenerated]
  public class DestroySystem : GameSystemBase
  {
    private ModificationBarrier2 m_ModificationBarrier;
    private ElectricityRoadConnectionGraphSystem m_ElectricityRoadConnectionGraphSystem;
    private WaterPipeRoadConnectionGraphSystem m_WaterPipeRoadConnectionGraphSystem;
    private EntityQuery m_EventQuery;
    private EntityQuery m_BuildingConfigurationQuery;
    private ComponentTypeSet m_DestroyedBuildingComponents;
    private DestroySystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier2>();
      // ISSUE: reference to a compiler-generated field
      this.m_ElectricityRoadConnectionGraphSystem = this.World.GetOrCreateSystemManaged<ElectricityRoadConnectionGraphSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterPipeRoadConnectionGraphSystem = this.World.GetOrCreateSystemManaged<WaterPipeRoadConnectionGraphSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EventQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Common.Event>(), ComponentType.ReadOnly<Destroy>());
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingConfigurationQuery = this.GetEntityQuery(ComponentType.ReadOnly<BuildingConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_DestroyedBuildingComponents = new ComponentTypeSet(ComponentType.ReadOnly<ElectricityConsumer>(), ComponentType.ReadOnly<WaterConsumer>(), ComponentType.ReadOnly<GarbageProducer>(), ComponentType.ReadOnly<MailProducer>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EventQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_BuildingConfigurationQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_MeshData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AreaData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Space_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Clip_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Destroy_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle deps1;
      JobHandle deps2;
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      DestroySystem.DestroyObjectsJob jobData = new DestroySystem.DestroyObjectsJob()
      {
        m_DestroyType = this.__TypeHandle.__Game_Objects_Destroy_RO_ComponentTypeHandle,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_DestroyedData = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup,
        m_NativeData = this.__TypeHandle.__Game_Common_Native_RO_ComponentLookup,
        m_Buildings = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_ElectricityConsumers = this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup,
        m_WaterConsumers = this.__TypeHandle.__Game_Buildings_WaterConsumer_RO_ComponentLookup,
        m_ClipAreas = this.__TypeHandle.__Game_Areas_Clip_RO_ComponentLookup,
        m_SpaceAreas = this.__TypeHandle.__Game_Areas_Space_RO_ComponentLookup,
        m_PrefabRefs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_AreaData = this.__TypeHandle.__Game_Prefabs_AreaData_RO_ComponentLookup,
        m_MeshData = this.__TypeHandle.__Game_Prefabs_MeshData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabSpawnableObjectData = this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_SubAreas = this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup,
        m_AreaNodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
        m_PrefabPlaceholderElements = this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup,
        m_PrefabSubMeshes = this.__TypeHandle.__Game_Prefabs_SubMesh_RO_BufferLookup,
        m_ProcessedObjects = new NativeHashSet<Entity>(32, (AllocatorManager.AllocatorHandle) Allocator.TempJob),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer(),
        m_UpdatedElectricityRoadEdges = this.m_ElectricityRoadConnectionGraphSystem.GetEdgeUpdateQueue(out deps1),
        m_UpdatedWaterPipeRoadEdges = this.m_WaterPipeRoadConnectionGraphSystem.GetEdgeUpdateQueue(out deps2),
        m_RandomSeed = RandomSeed.Next(),
        m_DestroyedBuildingComponents = this.m_DestroyedBuildingComponents,
        m_BuildingConfigurationData = this.m_BuildingConfigurationQuery.GetSingleton<BuildingConfigurationData>()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<DestroySystem.DestroyObjectsJob>(this.m_EventQuery, JobHandle.CombineDependencies(this.Dependency, deps1, deps2));
      // ISSUE: reference to a compiler-generated field
      jobData.m_ProcessedObjects.Dispose(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ElectricityRoadConnectionGraphSystem.AddQueueWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterPipeRoadConnectionGraphSystem.AddQueueWriter(this.Dependency);
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
    public DestroySystem()
    {
    }

    [BurstCompile]
    private struct DestroyObjectsJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Destroy> m_DestroyType;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Destroyed> m_DestroyedData;
      [ReadOnly]
      public ComponentLookup<Native> m_NativeData;
      [ReadOnly]
      public ComponentLookup<Building> m_Buildings;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> m_ElectricityConsumers;
      [ReadOnly]
      public ComponentLookup<WaterConsumer> m_WaterConsumers;
      [ReadOnly]
      public ComponentLookup<Clip> m_ClipAreas;
      [ReadOnly]
      public ComponentLookup<Space> m_SpaceAreas;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefs;
      [ReadOnly]
      public ComponentLookup<AreaData> m_AreaData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.MeshData> m_MeshData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<SpawnableObjectData> m_PrefabSpawnableObjectData;
      [ReadOnly]
      public BufferLookup<SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> m_SubAreas;
      [ReadOnly]
      public BufferLookup<Node> m_AreaNodes;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> m_PrefabPlaceholderElements;
      [ReadOnly]
      public BufferLookup<SubMesh> m_PrefabSubMeshes;
      public NativeHashSet<Entity> m_ProcessedObjects;
      public EntityCommandBuffer m_CommandBuffer;
      public NativeQueue<Entity> m_UpdatedElectricityRoadEdges;
      public NativeQueue<Entity> m_UpdatedWaterPipeRoadEdges;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public ComponentTypeSet m_DestroyedBuildingComponents;
      [ReadOnly]
      public BuildingConfigurationData m_BuildingConfigurationData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Destroy> nativeArray = chunk.GetNativeArray<Destroy>(ref this.m_DestroyType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Destroy destroyEvent = nativeArray[index];
          // ISSUE: reference to a compiler-generated method
          this.DestroyObject(ref random, destroyEvent.m_Object, destroyEvent);
        }
      }

      private void DestroyObject(ref Random random, Entity entity, Destroy destroyEvent)
      {
        PrefabRef componentData1;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_DestroyedData.HasComponent(entity) || !this.m_ProcessedObjects.Add(entity) || !this.m_PrefabRefs.TryGetComponent(entity, out componentData1))
          return;
        float y = 0.0f;
        ObjectGeometryData componentData2;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabObjectGeometryData.TryGetComponent(componentData1.m_Prefab, out componentData2) && (componentData2.m_Flags & (GeometryFlags.Physical | GeometryFlags.HasLot)) == (GeometryFlags.Physical | GeometryFlags.HasLot))
        {
          y = BuildingUtils.GetCollapseTime(componentData2.m_Size.y);
          bool flag = false;
          DynamicBuffer<SubMesh> bufferData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabSubMeshes.TryGetBuffer(componentData1.m_Prefab, out bufferData1))
          {
            for (int index1 = 0; index1 < bufferData1.Length; ++index1)
            {
              SubMesh subMesh = bufferData1[index1];
              Game.Prefabs.MeshData componentData3;
              // ISSUE: reference to a compiler-generated field
              if (this.m_MeshData.TryGetComponent(subMesh.m_SubMesh, out componentData3))
              {
                float2 float2_1 = MathUtils.Center(componentData3.m_Bounds.xz);
                float2 float2_2 = MathUtils.Extents(componentData3.m_Bounds.xz);
                float3 v1 = math.rotate(subMesh.m_Rotation, new float3(float2_2.x, 0.0f, 0.0f));
                float3 v2 = math.rotate(subMesh.m_Rotation, new float3(0.0f, 0.0f, float2_2.y));
                float3 position1 = subMesh.m_Position + math.rotate(subMesh.m_Rotation, new float3(float2_1.x, 0.0f, float2_1.y));
                // ISSUE: reference to a compiler-generated field
                Transform transform = this.m_TransformData[entity];
                float3 float3_1 = math.rotate(transform.m_Rotation, v1);
                float3 float3_2 = math.rotate(transform.m_Rotation, v2);
                float3 world = ObjectUtils.LocalToWorld(transform, position1);
                // ISSUE: reference to a compiler-generated field
                Entity result = this.m_BuildingConfigurationData.m_CollapsedSurface;
                DynamicBuffer<PlaceholderObjectElement> bufferData2;
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabPlaceholderElements.TryGetBuffer(result, out bufferData2))
                {
                  // ISSUE: reference to a compiler-generated field
                  AreaUtils.SelectAreaPrefab(bufferData2, this.m_PrefabSpawnableObjectData, new NativeParallelHashMap<Entity, int>(), ref random, out result, out int _);
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                Entity entity1 = this.m_CommandBuffer.CreateEntity(this.m_AreaData[result].m_Archetype);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<PrefabRef>(entity1, new PrefabRef(result));
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Owner>(entity1, new Owner(entity));
                // ISSUE: reference to a compiler-generated field
                if (this.m_NativeData.HasComponent(entity))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Native>(entity1, new Native());
                }
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<Node> dynamicBuffer = this.m_CommandBuffer.SetBuffer<Node>(entity1);
                dynamicBuffer.ResizeUninitialized(32);
                float4 float4_1 = (float4) random.NextInt4((int4) 3, (int4) 10);
                float4 float4_2 = (float4) random.NextFloat(-3.14159274f, 3.14159274f);
                float num = -6.28318548f / (float) dynamicBuffer.Length;
                for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
                {
                  float x1 = (float) index2 * num;
                  float2 x2 = new float2(math.cos(x1), math.sin(x1));
                  x2 = math.sign(x2) * math.sqrt(math.abs(x2)) * (1f + math.dot(math.sin(x1 * float4_1 + float4_2), (float4) 0.025f));
                  float3 position2 = world + float3_1 * x2.x + float3_2 * x2.y;
                  dynamicBuffer[index2] = new Node(position2, float.MinValue);
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<Area>(entity1, new Area(AreaFlags.Complete));
                flag = true;
              }
            }
          }
          DynamicBuffer<Game.Areas.SubArea> bufferData3;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubAreas.TryGetBuffer(entity, out bufferData3))
          {
            for (int index = 0; index < bufferData3.Length; ++index)
            {
              Entity area = bufferData3[index].m_Area;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              if (this.m_ClipAreas.HasComponent(area) || this.m_SpaceAreas.HasComponent(area) && !this.IsAnyOnGround(this.m_AreaNodes[area]))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Deleted>(bufferData3[index].m_Area);
              }
            }
          }
          else if (flag)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddBuffer<Game.Areas.SubArea>(entity);
          }
        }
        Destroyed component = new Destroyed(destroyEvent.m_Event);
        if ((double) y != 0.0)
          component.m_Cleared = 0.5f - math.max(1f, y);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Destroyed>(entity, component);
        if ((double) y != 0.0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<InterpolatedTransform>(entity, new InterpolatedTransform(this.m_TransformData[entity]));
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(entity);
        Building componentData4;
        // ISSUE: reference to a compiler-generated field
        if (this.m_Buildings.TryGetComponent(entity, out componentData4))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent(entity, in this.m_DestroyedBuildingComponents);
          if (componentData4.m_RoadEdge != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_ElectricityConsumers.HasComponent(entity))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_UpdatedElectricityRoadEdges.Enqueue(componentData4.m_RoadEdge);
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_WaterConsumers.HasComponent(entity))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_UpdatedWaterPipeRoadEdges.Enqueue(componentData4.m_RoadEdge);
            }
          }
        }
        DynamicBuffer<SubObject> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.TryGetBuffer(entity, out bufferData))
          return;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Entity subObject = bufferData[index].m_SubObject;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_Buildings.HasComponent(subObject))
          {
            // ISSUE: reference to a compiler-generated method
            this.DestroyObject(ref random, subObject, destroyEvent);
          }
        }
      }

      private bool IsAnyOnGround(DynamicBuffer<Node> nodes)
      {
        for (int index = 0; index < nodes.Length; ++index)
        {
          if ((double) nodes[index].m_Elevation == -3.4028234663852886E+38)
            return true;
        }
        return false;
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
      public ComponentTypeHandle<Destroy> __Game_Objects_Destroy_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Destroyed> __Game_Common_Destroyed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Native> __Game_Common_Native_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityConsumer> __Game_Buildings_ElectricityConsumer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterConsumer> __Game_Buildings_WaterConsumer_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Clip> __Game_Areas_Clip_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Space> __Game_Areas_Space_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AreaData> __Game_Prefabs_AreaData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.MeshData> __Game_Prefabs_MeshData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableObjectData> __Game_Prefabs_SpawnableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> __Game_Areas_SubArea_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> __Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubMesh> __Game_Prefabs_SubMesh_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Destroy_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Destroy>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentLookup = state.GetComponentLookup<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Native_RO_ComponentLookup = state.GetComponentLookup<Native>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityConsumer_RO_ComponentLookup = state.GetComponentLookup<ElectricityConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterConsumer_RO_ComponentLookup = state.GetComponentLookup<WaterConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Clip_RO_ComponentLookup = state.GetComponentLookup<Clip>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Space_RO_ComponentLookup = state.GetComponentLookup<Space>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AreaData_RO_ComponentLookup = state.GetComponentLookup<AreaData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_MeshData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.MeshData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup = state.GetComponentLookup<SpawnableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Areas.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup = state.GetBufferLookup<PlaceholderObjectElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubMesh_RO_BufferLookup = state.GetBufferLookup<SubMesh>(true);
      }
    }
  }
}
