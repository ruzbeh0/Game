// Decompiled with JetBrains decompiler
// Type: Game.Simulation.AreaSpawnSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Companies;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
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
  public class AreaSpawnSystem : GameSystemBase
  {
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_AreaQuery;
    private EntityArchetype m_DefinitionArchetype;
    private AreaSpawnSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 64;

    public bool debugFastSpawn { get; set; }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaQuery = this.GetEntityQuery(ComponentType.ReadOnly<Area>(), ComponentType.ReadOnly<Geometry>(), ComponentType.ReadOnly<Game.Objects.SubObject>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_DefinitionArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<CreationDefinition>(), ComponentType.ReadWrite<ObjectDefinition>(), ComponentType.ReadWrite<Updated>(), ComponentType.ReadWrite<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_AreaQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubAreaNode_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubNet_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_StorageAreaData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_CompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Triangle_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Extractor_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Storage_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Area_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle deps;
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
      JobHandle jobHandle = new AreaSpawnSystem.AreaSpawnJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_AreaType = this.__TypeHandle.__Game_Areas_Area_RO_ComponentTypeHandle,
        m_GeometryType = this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentTypeHandle,
        m_StorageType = this.__TypeHandle.__Game_Areas_Storage_RO_ComponentTypeHandle,
        m_ExtractorType = this.__TypeHandle.__Game_Areas_Extractor_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_NodeType = this.__TypeHandle.__Game_Areas_Node_RO_BufferTypeHandle,
        m_TriangleType = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferTypeHandle,
        m_SubObjectType = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_AttachmentData = this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup,
        m_CompanyData = this.__TypeHandle.__Game_Companies_CompanyData_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabStorageAreaData = this.__TypeHandle.__Game_Prefabs_StorageAreaData_RO_ComponentLookup,
        m_PrefabExtractorAreaData = this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup,
        m_PrefabAreaGeometryData = this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabSpawnableObjectData = this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup,
        m_PrefabPlaceableObjectData = this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_PrefabNetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_SubAreas = this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup,
        m_BuildingRenters = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup,
        m_PrefabSubObjects = this.__TypeHandle.__Game_Prefabs_SubObject_RO_BufferLookup,
        m_PrefabSubNets = this.__TypeHandle.__Game_Prefabs_SubNet_RO_BufferLookup,
        m_PrefabSubAreas = this.__TypeHandle.__Game_Prefabs_SubArea_RO_BufferLookup,
        m_PrefabSubAreaNodes = this.__TypeHandle.__Game_Prefabs_SubAreaNode_RO_BufferLookup,
        m_PlaceholderObjectElements = this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup,
        m_ObjectRequirements = this.__TypeHandle.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup,
        m_DebugFastSpawn = this.debugFastSpawn,
        m_LefthandTraffic = this.m_CityConfigurationSystem.leftHandTraffic,
        m_RandomSeed = RandomSeed.Next(),
        m_DefinitionArchetype = this.m_DefinitionArchetype,
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_WaterSurfaceData = this.m_WaterSystem.GetSurfaceData(out deps),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<AreaSpawnSystem.AreaSpawnJob>(this.m_AreaQuery, JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle);
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
    public AreaSpawnSystem()
    {
    }

    [BurstCompile]
    private struct AreaSpawnJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Area> m_AreaType;
      [ReadOnly]
      public ComponentTypeHandle<Geometry> m_GeometryType;
      [ReadOnly]
      public ComponentTypeHandle<Storage> m_StorageType;
      [ReadOnly]
      public ComponentTypeHandle<Extractor> m_ExtractorType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<Game.Areas.Node> m_NodeType;
      [ReadOnly]
      public BufferTypeHandle<Triangle> m_TriangleType;
      [ReadOnly]
      public BufferTypeHandle<Game.Objects.SubObject> m_SubObjectType;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Attachment> m_AttachmentData;
      [ReadOnly]
      public ComponentLookup<CompanyData> m_CompanyData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> m_PrefabAreaGeometryData;
      [ReadOnly]
      public ComponentLookup<StorageAreaData> m_PrefabStorageAreaData;
      [ReadOnly]
      public ComponentLookup<ExtractorAreaData> m_PrefabExtractorAreaData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<SpawnableObjectData> m_PrefabSpawnableObjectData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabNetGeometryData;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> m_SubAreas;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> m_PrefabPlaceableObjectData;
      [ReadOnly]
      public BufferLookup<Renter> m_BuildingRenters;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubNet> m_PrefabSubNets;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubArea> m_PrefabSubAreas;
      [ReadOnly]
      public BufferLookup<SubAreaNode> m_PrefabSubAreaNodes;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubObject> m_PrefabSubObjects;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> m_PlaceholderObjectElements;
      [ReadOnly]
      public BufferLookup<ObjectRequirementElement> m_ObjectRequirements;
      [ReadOnly]
      public bool m_LefthandTraffic;
      [ReadOnly]
      public bool m_DebugFastSpawn;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public EntityArchetype m_DefinitionArchetype;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        NativeList<AreaUtils.ObjectItem> objects = new NativeList<AreaUtils.ObjectItem>();
        NativeParallelHashSet<Entity> placeholderRequirements = new NativeParallelHashSet<Entity>();
        // ISSUE: reference to a compiler-generated field
        NativeArray<Storage> nativeArray1 = chunk.GetNativeArray<Storage>(ref this.m_StorageType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Extractor> nativeArray2 = chunk.GetNativeArray<Extractor>(ref this.m_ExtractorType);
        if (nativeArray1.Length != 0 || nativeArray2.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray3 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Area> nativeArray4 = chunk.GetNativeArray<Area>(ref this.m_AreaType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Geometry> nativeArray5 = chunk.GetNativeArray<Geometry>(ref this.m_GeometryType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray6 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray7 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<Game.Areas.Node> bufferAccessor1 = chunk.GetBufferAccessor<Game.Areas.Node>(ref this.m_NodeType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<Triangle> bufferAccessor2 = chunk.GetBufferAccessor<Triangle>(ref this.m_TriangleType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<Game.Objects.SubObject> bufferAccessor3 = chunk.GetBufferAccessor<Game.Objects.SubObject>(ref this.m_SubObjectType);
          for (int index1 = 0; index1 < nativeArray3.Length; ++index1)
          {
            Geometry geometry = nativeArray5[index1];
            PrefabRef prefabRef1 = nativeArray7[index1];
            DynamicBuffer<Game.Objects.SubObject> dynamicBuffer = bufferAccessor3[index1];
            // ISSUE: reference to a compiler-generated field
            AreaGeometryData areaData = this.m_PrefabAreaGeometryData[prefabRef1.m_Prefab];
            float x = 0.0f;
            if (nativeArray1.Length != 0)
            {
              Storage storage = nativeArray1[index1];
              // ISSUE: reference to a compiler-generated field
              StorageAreaData prefabStorageData = this.m_PrefabStorageAreaData[prefabRef1.m_Prefab];
              // ISSUE: reference to a compiler-generated field
              if (this.m_DebugFastSpawn)
                storage.m_Amount = AreaUtils.CalculateStorageCapacity(geometry, prefabStorageData);
              x = math.max(x, AreaUtils.CalculateStorageObjectArea(geometry, storage, prefabStorageData));
            }
            if (nativeArray2.Length != 0)
            {
              Extractor extractor = nativeArray2[index1];
              // ISSUE: reference to a compiler-generated field
              ExtractorAreaData extractorAreaData = this.m_PrefabExtractorAreaData[prefabRef1.m_Prefab];
              // ISSUE: reference to a compiler-generated field
              if (this.m_DebugFastSpawn)
                extractor.m_TotalExtracted = extractor.m_ResourceAmount;
              x = math.max(x, AreaUtils.CalculateExtractorObjectArea(geometry, extractor, extractorAreaData));
            }
            if ((double) x >= 1.0)
            {
              if (!objects.IsCreated && dynamicBuffer.Length > 0)
                objects = new NativeList<AreaUtils.ObjectItem>(dynamicBuffer.Length, (AllocatorManager.AllocatorHandle) Allocator.Temp);
              float num = 0.0f;
              for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
              {
                Entity subObject = dynamicBuffer[index2].m_SubObject;
                // ISSUE: reference to a compiler-generated field
                PrefabRef prefabRef2 = this.m_PrefabRefData[subObject];
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabObjectGeometryData.HasComponent(prefabRef2.m_Prefab))
                {
                  // ISSUE: reference to a compiler-generated field
                  Transform transform = this.m_TransformData[subObject];
                  // ISSUE: reference to a compiler-generated field
                  ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefabRef2.m_Prefab];
                  float radius;
                  if ((objectGeometryData.m_Flags & Game.Objects.GeometryFlags.Circular) != Game.Objects.GeometryFlags.None)
                  {
                    radius = objectGeometryData.m_Size.x * 0.5f;
                  }
                  else
                  {
                    radius = math.length(MathUtils.Size(objectGeometryData.m_Bounds.xz)) * 0.5f;
                    transform.m_Position.xz -= math.rotate(transform.m_Rotation, MathUtils.Center(objectGeometryData.m_Bounds)).xz;
                  }
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PrefabBuildingData.HasComponent(prefabRef2.m_Prefab))
                    radius += AreaUtils.GetMinNodeDistance(areaData);
                  num += (float) ((double) radius * (double) radius * 3.1415927410125732);
                  objects.Add(new AreaUtils.ObjectItem(radius, transform.m_Position.xz, subObject));
                }
              }
              if ((double) num < (double) x)
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_PrefabSubObjects.HasBuffer(prefabRef1.m_Prefab))
                {
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<Game.Prefabs.SubObject> prefabSubObject = this.m_PrefabSubObjects[prefabRef1.m_Prefab];
                  Owner owner = new Owner();
                  if (nativeArray6.Length != 0)
                    owner = nativeArray6[index1];
                  Entity prefab;
                  // ISSUE: reference to a compiler-generated method
                  if (this.TryGetObjectPrefab(ref random, ref placeholderRequirements, owner, prefabSubObject, out prefab))
                  {
                    Area area = nativeArray4[index1];
                    DynamicBuffer<Game.Areas.Node> nodes = bufferAccessor1[index1];
                    DynamicBuffer<Triangle> triangles = bufferAccessor2[index1];
                    ObjectGeometryData objectGeometryData = new ObjectGeometryData();
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_PrefabObjectGeometryData.HasComponent(prefab))
                    {
                      // ISSUE: reference to a compiler-generated field
                      objectGeometryData = this.m_PrefabObjectGeometryData[prefab];
                    }
                    float extraRadius = 0.0f;
                    bool flag = false;
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_PrefabBuildingData.HasComponent(prefab))
                    {
                      extraRadius = AreaUtils.GetMinNodeDistance(areaData);
                      flag = true;
                    }
                    Transform transform;
                    if (AreaUtils.TryGetRandomObjectLocation(ref random, objectGeometryData, area, geometry, extraRadius, nodes, triangles, objects, out transform))
                    {
                      Entity entity1 = nativeArray3[index1];
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated field
                      transform = ObjectUtils.AdjustPosition(transform, new Game.Objects.Elevation(), prefab, out bool _, ref this.m_TerrainHeightData, ref this.m_WaterSurfaceData, ref this.m_PrefabPlaceableObjectData, ref this.m_PrefabObjectGeometryData);
                      // ISSUE: reference to a compiler-generated method
                      this.SpawnObject(unfilteredChunkIndex, entity1, prefab, transform, ref random);
                      if (objects.IsCreated && objects.Length != 0)
                      {
                        for (int index3 = 0; index3 < objects.Length; ++index3)
                        {
                          Entity entity2 = objects[index3].m_Entity;
                          // ISSUE: reference to a compiler-generated field
                          // ISSUE: reference to a compiler-generated field
                          if (this.m_PrefabBuildingData.HasComponent(this.m_PrefabRefData[entity2].m_Prefab))
                          {
                            objects.RemoveAtSwapBack(index3--);
                            // ISSUE: reference to a compiler-generated field
                            this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, entity2, new Deleted());
                            flag = true;
                          }
                        }
                        if (objects.Length != 0)
                        {
                          // ISSUE: reference to a compiler-generated field
                          this.m_CommandBuffer.AddComponent<Updated>(unfilteredChunkIndex, entity1, new Updated());
                        }
                      }
                      DynamicBuffer<Game.Areas.SubArea> bufferData;
                      // ISSUE: reference to a compiler-generated field
                      if (flag && this.m_SubAreas.TryGetBuffer(entity1, out bufferData))
                      {
                        for (int index4 = 0; index4 < bufferData.Length; ++index4)
                        {
                          // ISSUE: reference to a compiler-generated field
                          this.m_CommandBuffer.AddComponent<Updated>(unfilteredChunkIndex, bufferData[index4].m_Area, new Updated());
                        }
                      }
                    }
                  }
                }
                if (objects.IsCreated)
                  objects.Clear();
              }
            }
          }
        }
        if (objects.IsCreated)
          objects.Dispose();
        if (!placeholderRequirements.IsCreated)
          return;
        placeholderRequirements.Dispose();
      }

      private bool TryGetObjectPrefab(
        ref Random random,
        ref NativeParallelHashSet<Entity> placeholderRequirements,
        Owner owner,
        DynamicBuffer<Game.Prefabs.SubObject> prefabSubObjects,
        out Entity prefab)
      {
        if (prefabSubObjects.Length == 0)
        {
          prefab = Entity.Null;
          return false;
        }
        prefab = prefabSubObjects[random.NextInt(prefabSubObjects.Length)].m_Prefab;
        // ISSUE: reference to a compiler-generated method
        return this.TryGetObjectPrefab(ref random, ref prefab, ref placeholderRequirements, owner);
      }

      private bool TryGetObjectPrefab(
        ref Random random,
        ref Entity prefab,
        ref NativeParallelHashSet<Entity> placeholderRequirements,
        Owner owner)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PlaceholderObjectElements.HasBuffer(prefab))
          return true;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<PlaceholderObjectElement> placeholderObjectElement = this.m_PlaceholderObjectElements[prefab];
        int max = 0;
        bool flag1 = false;
        for (int index1 = 0; index1 < placeholderObjectElement.Length; ++index1)
        {
          Entity entity = placeholderObjectElement[index1].m_Object;
          DynamicBuffer<ObjectRequirementElement> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ObjectRequirements.TryGetBuffer(entity, out bufferData))
          {
            if (!flag1)
            {
              flag1 = true;
              // ISSUE: reference to a compiler-generated method
              this.FillRequirements(ref placeholderRequirements, owner);
            }
            int num = -1;
            bool flag2 = true;
            for (int index2 = 0; index2 < bufferData.Length; ++index2)
            {
              ObjectRequirementElement requirementElement = bufferData[index2];
              if ((int) requirementElement.m_Group != num)
              {
                if (flag2)
                {
                  num = (int) requirementElement.m_Group;
                  flag2 = false;
                }
                else
                  break;
              }
              flag2 |= placeholderRequirements.Contains(requirementElement.m_Requirement);
            }
            if (!flag2)
              continue;
          }
          // ISSUE: reference to a compiler-generated field
          int probability = this.m_PrefabSpawnableObjectData[entity].m_Probability;
          max += probability;
          if (random.NextInt(max) < probability)
            prefab = entity;
        }
        return random.NextInt(100) < max;
      }

      private void FillRequirements(
        ref NativeParallelHashSet<Entity> placeholderRequirements,
        Owner owner)
      {
        if (placeholderRequirements.IsCreated)
          placeholderRequirements.Clear();
        else
          placeholderRequirements = new NativeParallelHashSet<Entity>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        Entity entity = owner.m_Owner;
        Attachment componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_AttachmentData.TryGetComponent(owner.m_Owner, out componentData))
          entity = componentData.m_Attached;
        DynamicBuffer<Renter> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_BuildingRenters.TryGetBuffer(entity, out bufferData))
          return;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          Entity renter = bufferData[index].m_Renter;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CompanyData.HasComponent(renter))
          {
            // ISSUE: reference to a compiler-generated field
            Entity prefab = this.m_PrefabRefData[renter].m_Prefab;
            // ISSUE: reference to a compiler-generated field
            Entity brand = this.m_CompanyData[renter].m_Brand;
            if (brand != Entity.Null)
              placeholderRequirements.Add(brand);
            placeholderRequirements.Add(prefab);
          }
        }
      }

      private void Spawn(
        int jobIndex,
        OwnerDefinition ownerDefinition,
        DynamicBuffer<Game.Prefabs.SubArea> subAreas,
        DynamicBuffer<SubAreaNode> subAreaNodes,
        ref Random random)
      {
        NativeParallelHashMap<Entity, int> nativeParallelHashMap = new NativeParallelHashMap<Entity, int>();
        for (int index1 = 0; index1 < subAreas.Length; ++index1)
        {
          Game.Prefabs.SubArea subArea = subAreas[index1];
          int num;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PlaceholderObjectElements.HasBuffer(subArea.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<PlaceholderObjectElement> placeholderObjectElement = this.m_PlaceholderObjectElements[subArea.m_Prefab];
            if (!nativeParallelHashMap.IsCreated)
              nativeParallelHashMap = new NativeParallelHashMap<Entity, int>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
            // ISSUE: reference to a compiler-generated field
            ComponentLookup<SpawnableObjectData> spawnableObjectData = this.m_PrefabSpawnableObjectData;
            NativeParallelHashMap<Entity, int> selectedSpawnables = nativeParallelHashMap;
            ref Random local1 = ref random;
            ref Entity local2 = ref subArea.m_Prefab;
            ref int local3 = ref num;
            if (!AreaUtils.SelectAreaPrefab(placeholderObjectElement, spawnableObjectData, selectedSpawnables, ref local1, out local2, out local3))
              continue;
          }
          else
            num = random.NextInt();
          // ISSUE: reference to a compiler-generated field
          Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex);
          CreationDefinition component = new CreationDefinition();
          component.m_Prefab = subArea.m_Prefab;
          component.m_RandomSeed = num;
          component.m_Flags |= CreationFlags.Permanent;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<CreationDefinition>(jobIndex, entity, component);
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Updated>(jobIndex, entity, new Updated());
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<OwnerDefinition>(jobIndex, entity, ownerDefinition);
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Game.Areas.Node> dynamicBuffer = this.m_CommandBuffer.AddBuffer<Game.Areas.Node>(jobIndex, entity);
          dynamicBuffer.ResizeUninitialized(subArea.m_NodeRange.y - subArea.m_NodeRange.x + 1);
          // ISSUE: reference to a compiler-generated method
          int index2 = ObjectToolBaseSystem.GetFirstNodeIndex(subAreaNodes, subArea.m_NodeRange);
          int index3 = 0;
          for (int x = subArea.m_NodeRange.x; x <= subArea.m_NodeRange.y; ++x)
          {
            float3 position = subAreaNodes[index2].m_Position;
            float3 world = ObjectUtils.LocalToWorld(ownerDefinition.m_Position, ownerDefinition.m_Rotation, position);
            int parentMesh = subAreaNodes[index2].m_ParentMesh;
            float elevation = math.select(float.MinValue, position.y, parentMesh >= 0);
            dynamicBuffer[index3] = new Game.Areas.Node(world, elevation);
            ++index3;
            if (++index2 == subArea.m_NodeRange.y)
              index2 = subArea.m_NodeRange.x;
          }
        }
        if (!nativeParallelHashMap.IsCreated)
          return;
        nativeParallelHashMap.Dispose();
      }

      private void Spawn(
        int jobIndex,
        OwnerDefinition ownerDefinition,
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
          this.CreateSubNet(jobIndex, subNet.m_Prefab, subNet.m_Curve, subNet.m_NodeIndex, subNet.m_ParentMesh, subNet.m_Upgrades, nodePositions, ownerDefinition, ref random);
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
        OwnerDefinition ownerDefinition,
        ref Random random)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex);
        CreationDefinition component1 = new CreationDefinition();
        component1.m_Prefab = netPrefab;
        component1.m_RandomSeed = random.NextInt();
        component1.m_Flags |= CreationFlags.Permanent;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CreationDefinition>(jobIndex, entity, component1);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(jobIndex, entity, new Updated());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<OwnerDefinition>(jobIndex, entity, ownerDefinition);
        NetCourse component2 = new NetCourse()
        {
          m_Curve = ObjectUtils.LocalToWorld(ownerDefinition.m_Position, ownerDefinition.m_Rotation, curve)
        };
        component2.m_StartPosition.m_Position = component2.m_Curve.a;
        component2.m_StartPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.StartTangent(component2.m_Curve), ownerDefinition.m_Rotation);
        component2.m_StartPosition.m_CourseDelta = 0.0f;
        component2.m_StartPosition.m_Elevation = (float2) curve.a.y;
        component2.m_StartPosition.m_ParentMesh = parentMesh.x;
        if (nodeIndex.x >= 0)
          component2.m_StartPosition.m_Position = ObjectUtils.LocalToWorld(ownerDefinition.m_Position, ownerDefinition.m_Rotation, nodePositions[nodeIndex.x].xyz);
        component2.m_EndPosition.m_Position = component2.m_Curve.d;
        component2.m_EndPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.EndTangent(component2.m_Curve), ownerDefinition.m_Rotation);
        component2.m_EndPosition.m_CourseDelta = 1f;
        component2.m_EndPosition.m_Elevation = (float2) curve.d.y;
        component2.m_EndPosition.m_ParentMesh = parentMesh.y;
        if (nodeIndex.y >= 0)
          component2.m_EndPosition.m_Position = ObjectUtils.LocalToWorld(ownerDefinition.m_Position, ownerDefinition.m_Rotation, nodePositions[nodeIndex.y].xyz);
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

      private void SpawnObject(
        int jobIndex,
        Entity entity,
        Entity prefab,
        Transform transform,
        ref Random random)
      {
        CreationDefinition component1 = new CreationDefinition();
        component1.m_Owner = entity;
        component1.m_Prefab = prefab;
        component1.m_Flags |= CreationFlags.Permanent;
        component1.m_RandomSeed = random.NextInt();
        ObjectDefinition component2 = new ObjectDefinition();
        component2.m_ParentMesh = -1;
        component2.m_Position = transform.m_Position;
        component2.m_Rotation = transform.m_Rotation;
        component2.m_LocalPosition = transform.m_Position;
        component2.m_LocalRotation = transform.m_Rotation;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity1 = this.m_CommandBuffer.CreateEntity(jobIndex, this.m_DefinitionArchetype);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<CreationDefinition>(jobIndex, entity1, component1);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<ObjectDefinition>(jobIndex, entity1, component2);
        OwnerDefinition ownerDefinition = new OwnerDefinition();
        ownerDefinition.m_Prefab = prefab;
        ownerDefinition.m_Position = component2.m_Position;
        ownerDefinition.m_Rotation = component2.m_Rotation;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabSubAreas.HasBuffer(prefab))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.Spawn(jobIndex, ownerDefinition, this.m_PrefabSubAreas[prefab], this.m_PrefabSubAreaNodes[prefab], ref random);
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_PrefabSubNets.HasBuffer(prefab))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.Spawn(jobIndex, ownerDefinition, this.m_PrefabSubNets[prefab], ref random);
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
      public ComponentTypeHandle<Area> __Game_Areas_Area_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Geometry> __Game_Areas_Geometry_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Storage> __Game_Areas_Storage_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Extractor> __Game_Areas_Extractor_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Areas.Node> __Game_Areas_Node_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Triangle> __Game_Areas_Triangle_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attachment> __Game_Objects_Attachment_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CompanyData> __Game_Companies_CompanyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StorageAreaData> __Game_Prefabs_StorageAreaData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ExtractorAreaData> __Game_Prefabs_ExtractorAreaData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> __Game_Prefabs_AreaGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableObjectData> __Game_Prefabs_SpawnableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> __Game_Prefabs_PlaceableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> __Game_Areas_SubArea_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Renter> __Game_Buildings_Renter_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubObject> __Game_Prefabs_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubNet> __Game_Prefabs_SubNet_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubArea> __Game_Prefabs_SubArea_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubAreaNode> __Game_Prefabs_SubAreaNode_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> __Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ObjectRequirementElement> __Game_Prefabs_ObjectRequirementElement_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Area_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Area>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Geometry_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Geometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Storage_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Storage>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Extractor_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Extractor>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferTypeHandle = state.GetBufferTypeHandle<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attachment_RO_ComponentLookup = state.GetComponentLookup<Attachment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_CompanyData_RO_ComponentLookup = state.GetComponentLookup<CompanyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_StorageAreaData_RO_ComponentLookup = state.GetComponentLookup<StorageAreaData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup = state.GetComponentLookup<ExtractorAreaData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup = state.GetComponentLookup<AreaGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup = state.GetComponentLookup<SpawnableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup = state.GetComponentLookup<PlaceableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Areas.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferLookup = state.GetBufferLookup<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubNet_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubAreaNode_RO_BufferLookup = state.GetBufferLookup<SubAreaNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup = state.GetBufferLookup<PlaceholderObjectElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectRequirementElement_RO_BufferLookup = state.GetBufferLookup<ObjectRequirementElement>(true);
      }
    }
  }
}
