// Decompiled with JetBrains decompiler
// Type: Game.Tools.UpgradeDeletedSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class UpgradeDeletedSystem : GameSystemBase
  {
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private ToolOutputBarrier m_ToolOutputBarrier;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private EntityQuery m_DeletedQuery;
    private UpgradeDeletedSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier = this.World.GetOrCreateSystemManaged<ToolOutputBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.ServiceUpgrade>(), ComponentType.ReadOnly<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_DeletedQuery);
    }

    protected override void OnGamePreload(Colossal.Serialization.Entities.Purpose purpose, GameMode mode)
    {
      base.OnGamePreload(purpose, mode);
      this.Enabled = mode.IsGame();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
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
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup.Update(ref this.CheckedStateRef);
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
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingTerraformData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Clear_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Lot_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new UpgradeDeletedSystem.UpgradeDeletedJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_PseudoRandomSeedData = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_ElevationData = this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup,
        m_LotData = this.__TypeHandle.__Game_Buildings_Lot_RO_ComponentLookup,
        m_ClearAreaData = this.__TypeHandle.__Game_Areas_Clear_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabAreaGeometryData = this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_PrefabBuildingTerraformData = this.__TypeHandle.__Game_Prefabs_BuildingTerraformData_RO_ComponentLookup,
        m_PrefabBuildingExtensionData = this.__TypeHandle.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup,
        m_PrefabNetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabSpawnableObjectData = this.__TypeHandle.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_SubAreas = this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup,
        m_AreaNodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
        m_AreaTriangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
        m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
        m_PrefabSubAreas = this.__TypeHandle.__Game_Prefabs_SubArea_RO_BufferLookup,
        m_PrefabSubAreaNodes = this.__TypeHandle.__Game_Prefabs_SubAreaNode_RO_BufferLookup,
        m_PrefabSubNets = this.__TypeHandle.__Game_Prefabs_SubNet_RO_BufferLookup,
        m_PrefabPlaceholderElements = this.__TypeHandle.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup,
        m_LefthandTraffic = this.m_CityConfigurationSystem.leftHandTraffic,
        m_RandomSeed = RandomSeed.Next(),
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_WaterSurfaceData = this.m_WaterSystem.GetSurfaceData(out deps),
        m_CommandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<UpgradeDeletedSystem.UpgradeDeletedJob>(this.m_DeletedQuery, JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier.AddJobHandleForProducer(jobHandle);
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
    public UpgradeDeletedSystem()
    {
    }

    [BurstCompile]
    private struct UpgradeDeletedJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> m_PseudoRandomSeedData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Lot> m_LotData;
      [ReadOnly]
      public ComponentLookup<Clear> m_ClearAreaData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> m_PrefabAreaGeometryData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<BuildingTerraformData> m_PrefabBuildingTerraformData;
      [ReadOnly]
      public ComponentLookup<BuildingExtensionData> m_PrefabBuildingExtensionData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabNetGeometryData;
      [ReadOnly]
      public ComponentLookup<SpawnableObjectData> m_PrefabSpawnableObjectData;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> m_SubAreas;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_AreaNodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_AreaTriangles;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubArea> m_PrefabSubAreas;
      [ReadOnly]
      public BufferLookup<SubAreaNode> m_PrefabSubAreaNodes;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubNet> m_PrefabSubNets;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> m_PrefabPlaceholderElements;
      [ReadOnly]
      public bool m_LefthandTraffic;
      [ReadOnly]
      public RandomSeed m_RandomSeed;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray3 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        NativeList<ClearAreaData> clearAreas1 = new NativeList<ClearAreaData>();
        NativeList<ClearAreaData> clearAreas2 = new NativeList<ClearAreaData>();
        NativeParallelHashMap<Entity, int> selectedSpawnables = new NativeParallelHashMap<Entity, int>();
        for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
        {
          Entity entity1 = nativeArray1[index1];
          Owner owner = nativeArray2[index1];
          Transform componentData1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_DeletedData.HasComponent(owner.m_Owner) && this.m_TransformData.TryGetComponent(owner.m_Owner, out componentData1))
          {
            Transform transform = nativeArray3[index1];
            PrefabRef prefabRef1 = nativeArray4[index1];
            DynamicBuffer<Game.Areas.SubArea> bufferData1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_SubAreas.TryGetBuffer(entity1, out bufferData1))
            {
              // ISSUE: reference to a compiler-generated field
              ObjectGeometryData objectGeometryData = this.m_PrefabObjectGeometryData[prefabRef1.m_Prefab];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              ClearAreaHelpers.FillClearAreas(bufferData1, transform, objectGeometryData, Entity.Null, ref this.m_ClearAreaData, ref this.m_AreaNodes, ref this.m_AreaTriangles, ref clearAreas1);
              ClearAreaHelpers.InitClearAreas(clearAreas1, componentData1);
            }
            if (clearAreas1.IsEmpty)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(unfilteredChunkIndex, owner.m_Owner, new Updated());
            }
            else
            {
              DynamicBuffer<InstalledUpgrade> bufferData2;
              // ISSUE: reference to a compiler-generated field
              if (this.m_InstalledUpgrades.TryGetBuffer(owner.m_Owner, out bufferData2))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                ClearAreaHelpers.FillClearAreas(bufferData2, entity1, this.m_TransformData, this.m_ClearAreaData, this.m_PrefabRefData, this.m_PrefabObjectGeometryData, this.m_SubAreas, this.m_AreaNodes, this.m_AreaTriangles, ref clearAreas2);
                ClearAreaHelpers.InitClearAreas(clearAreas2, componentData1);
              }
              // ISSUE: reference to a compiler-generated field
              PrefabRef prefabRef2 = this.m_PrefabRefData[owner.m_Owner];
              DynamicBuffer<Game.Prefabs.SubNet> bufferData3;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabSubNets.TryGetBuffer(prefabRef2.m_Prefab, out bufferData3))
              {
                NativeList<float4> nodePositions = new NativeList<float4>(bufferData3.Length * 2, (AllocatorManager.AllocatorHandle) Allocator.Temp);
                BuildingUtils.LotInfo lotInfo;
                // ISSUE: reference to a compiler-generated method
                bool ownerLot = this.GetOwnerLot(owner.m_Owner, out lotInfo);
                for (int index2 = 0; index2 < bufferData3.Length; ++index2)
                {
                  Game.Prefabs.SubNet subNet = bufferData3[index2];
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
                for (int index3 = 0; index3 < nodePositions.Length; ++index3)
                  nodePositions[index3] /= math.max(1f, nodePositions[index3].w);
                for (int index4 = 0; index4 < bufferData3.Length; ++index4)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  Game.Prefabs.SubNet subNet = NetUtils.GetSubNet(bufferData3, index4, this.m_LefthandTraffic, ref this.m_PrefabNetGeometryData);
                  // ISSUE: reference to a compiler-generated method
                  this.CreateSubNet(subNet.m_Prefab, subNet.m_Curve, subNet.m_NodeIndex, subNet.m_ParentMesh, subNet.m_Upgrades, nodePositions, componentData1, owner.m_Owner, clearAreas1, clearAreas2, lotInfo, ownerLot, unfilteredChunkIndex, ref random);
                }
                nodePositions.Dispose();
              }
              DynamicBuffer<Game.Prefabs.SubArea> bufferData4;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabSubAreas.TryGetBuffer(prefabRef2.m_Prefab, out bufferData4))
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<SubAreaNode> prefabSubAreaNode = this.m_PrefabSubAreaNodes[prefabRef2.m_Prefab];
                DynamicBuffer<Game.Areas.SubArea> bufferData5;
                // ISSUE: reference to a compiler-generated field
                if (this.m_SubAreas.TryGetBuffer(owner.m_Owner, out bufferData5))
                {
                  for (int index5 = 0; index5 < bufferData5.Length; ++index5)
                  {
                    Entity area = bufferData5[index5].m_Area;
                    // ISSUE: reference to a compiler-generated field
                    PrefabRef key = this.m_PrefabRefData[area];
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_PrefabSpawnableObjectData.HasComponent((Entity) key))
                    {
                      if (!selectedSpawnables.IsCreated)
                        selectedSpawnables = new NativeParallelHashMap<Entity, int>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
                      PseudoRandomSeed componentData2;
                      // ISSUE: reference to a compiler-generated field
                      int num = !this.m_PseudoRandomSeedData.TryGetComponent(area, out componentData2) ? random.NextInt() : (int) componentData2.m_Seed;
                      selectedSpawnables.TryAdd((Entity) key, num);
                    }
                  }
                }
                for (int index6 = 0; index6 < bufferData4.Length; ++index6)
                {
                  Game.Prefabs.SubArea subArea = bufferData4[index6];
                  DynamicBuffer<PlaceholderObjectElement> bufferData6;
                  int seed;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PrefabPlaceholderElements.TryGetBuffer(subArea.m_Prefab, out bufferData6))
                  {
                    if (!selectedSpawnables.IsCreated)
                      selectedSpawnables = new NativeParallelHashMap<Entity, int>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
                    // ISSUE: reference to a compiler-generated field
                    if (!AreaUtils.SelectAreaPrefab(bufferData6, this.m_PrefabSpawnableObjectData, selectedSpawnables, ref random, out subArea.m_Prefab, out seed))
                      continue;
                  }
                  else
                    seed = random.NextInt();
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PrefabAreaGeometryData[subArea.m_Prefab].m_Type == AreaType.Space && ClearAreaHelpers.ShouldClear(clearAreas1, prefabSubAreaNode, subArea.m_NodeRange, componentData1) && !ClearAreaHelpers.ShouldClear(clearAreas2, prefabSubAreaNode, subArea.m_NodeRange, componentData1))
                  {
                    // ISSUE: reference to a compiler-generated field
                    Entity entity2 = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex);
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<CreationDefinition>(unfilteredChunkIndex, entity2, new CreationDefinition()
                    {
                      m_Prefab = subArea.m_Prefab,
                      m_RandomSeed = seed,
                      m_Owner = owner.m_Owner,
                      m_Flags = CreationFlags.Permanent
                    });
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<Updated>(unfilteredChunkIndex, entity2, new Updated());
                    // ISSUE: reference to a compiler-generated field
                    DynamicBuffer<Game.Areas.Node> dynamicBuffer = this.m_CommandBuffer.AddBuffer<Game.Areas.Node>(unfilteredChunkIndex, entity2);
                    dynamicBuffer.ResizeUninitialized(subArea.m_NodeRange.y - subArea.m_NodeRange.x + 1);
                    // ISSUE: reference to a compiler-generated method
                    int index7 = ObjectToolBaseSystem.GetFirstNodeIndex(prefabSubAreaNode, subArea.m_NodeRange);
                    int index8 = 0;
                    for (int x = subArea.m_NodeRange.x; x <= subArea.m_NodeRange.y; ++x)
                    {
                      float3 position = prefabSubAreaNode[index7].m_Position;
                      float3 world = ObjectUtils.LocalToWorld(componentData1, position);
                      int parentMesh = prefabSubAreaNode[index7].m_ParentMesh;
                      float elevation = math.select(float.MinValue, position.y, parentMesh >= 0);
                      dynamicBuffer[index8] = new Game.Areas.Node(world, elevation);
                      ++index8;
                      if (++index7 == subArea.m_NodeRange.y)
                        index7 = subArea.m_NodeRange.x;
                    }
                  }
                }
              }
              // ISSUE: reference to a compiler-generated method
              this.UpdateObject(unfilteredChunkIndex, owner.m_Owner);
              clearAreas1.Clear();
              if (clearAreas2.IsCreated)
                clearAreas2.Clear();
              if (selectedSpawnables.IsCreated)
                selectedSpawnables.Clear();
            }
          }
        }
        if (clearAreas1.IsCreated)
          clearAreas1.Dispose();
        if (clearAreas2.IsCreated)
          clearAreas2.Dispose();
        if (!selectedSpawnables.IsCreated)
          return;
        selectedSpawnables.Dispose();
      }

      private void UpdateObject(int jobIndex, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_DeletedData.HasComponent(entity))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(jobIndex, entity, new Updated());
        DynamicBuffer<Game.Objects.SubObject> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_SubObjects.TryGetBuffer(entity, out bufferData))
          return;
        for (int index = 0; index < bufferData.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          this.UpdateObject(jobIndex, bufferData[index].m_SubObject);
        }
      }

      private void CreateSubNet(
        Entity netPrefab,
        Bezier4x3 curve,
        int2 nodeIndex,
        int2 parentMesh,
        CompositionFlags upgrades,
        NativeList<float4> nodePositions,
        Transform ownerTransform,
        Entity owner,
        NativeList<ClearAreaData> removedClearAreas,
        NativeList<ClearAreaData> remainingClearAreas,
        BuildingUtils.LotInfo lotInfo,
        bool hasLot,
        int jobIndex,
        ref Random random)
      {
        NetGeometryData componentData;
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabNetGeometryData.TryGetComponent(netPrefab, out componentData);
        CreationDefinition component1 = new CreationDefinition();
        component1.m_Prefab = netPrefab;
        component1.m_RandomSeed = random.NextInt();
        component1.m_Owner = owner;
        component1.m_Flags = CreationFlags.Permanent;
        bool linearMiddle = parentMesh.x >= 0 && parentMesh.y >= 0;
        NetCourse component2 = new NetCourse();
        if ((componentData.m_Flags & Game.Net.GeometryFlags.OnWater) != (Game.Net.GeometryFlags) 0)
        {
          curve.y = new Bezier4x1();
          Curve curve1 = new Curve()
          {
            m_Bezier = ObjectUtils.LocalToWorld(ownerTransform.m_Position, ownerTransform.m_Rotation, curve)
          };
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          component2.m_Curve = NetUtils.AdjustPosition(curve1, false, false, false, ref this.m_TerrainHeightData, ref this.m_WaterSurfaceData).m_Bezier;
        }
        else if (!linearMiddle)
        {
          Curve curve2 = new Curve()
          {
            m_Bezier = ObjectUtils.LocalToWorld(ownerTransform.m_Position, ownerTransform.m_Rotation, curve)
          };
          bool fixedStart = parentMesh.x >= 0;
          bool fixedEnd = parentMesh.y >= 0;
          linearMiddle = fixedStart | fixedEnd;
          if ((componentData.m_Flags & Game.Net.GeometryFlags.FlattenTerrain) != (Game.Net.GeometryFlags) 0)
          {
            if (hasLot)
            {
              component2.m_Curve = NetUtils.AdjustPosition(curve2, (bool2) fixedStart, linearMiddle, (bool2) fixedEnd, ref lotInfo).m_Bezier;
              component2.m_Curve.a.y += curve.a.y;
              component2.m_Curve.b.y += curve.b.y;
              component2.m_Curve.c.y += curve.c.y;
              component2.m_Curve.d.y += curve.d.y;
            }
            else
              component2.m_Curve = curve2.m_Bezier;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            component2.m_Curve = NetUtils.AdjustPosition(curve2, fixedStart, linearMiddle, fixedEnd, ref this.m_TerrainHeightData).m_Bezier;
            component2.m_Curve.a.y += curve.a.y;
            component2.m_Curve.b.y += curve.b.y;
            component2.m_Curve.c.y += curve.c.y;
            component2.m_Curve.d.y += curve.d.y;
          }
        }
        else
          component2.m_Curve = ObjectUtils.LocalToWorld(ownerTransform.m_Position, ownerTransform.m_Rotation, curve);
        bool onGround = !linearMiddle || (double) math.cmin(math.abs(curve.y.abcd)) < 2.0;
        if (!ClearAreaHelpers.ShouldClear(removedClearAreas, component2.m_Curve, onGround) || ClearAreaHelpers.ShouldClear(remainingClearAreas, component2.m_Curve, onGround))
          return;
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity(jobIndex);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<CreationDefinition>(jobIndex, entity, component1);
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(jobIndex, entity, new Updated());
        component2.m_StartPosition.m_Position = component2.m_Curve.a;
        component2.m_StartPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.StartTangent(component2.m_Curve), ownerTransform.m_Rotation);
        component2.m_StartPosition.m_CourseDelta = 0.0f;
        component2.m_StartPosition.m_Elevation = (float2) curve.a.y;
        component2.m_StartPosition.m_ParentMesh = parentMesh.x;
        if (nodeIndex.x >= 0)
        {
          if ((componentData.m_Flags & Game.Net.GeometryFlags.OnWater) != (Game.Net.GeometryFlags) 0)
            component2.m_StartPosition.m_Position.xz = ObjectUtils.LocalToWorld(ownerTransform, nodePositions[nodeIndex.x].xyz).xz;
          else
            component2.m_StartPosition.m_Position = ObjectUtils.LocalToWorld(ownerTransform, nodePositions[nodeIndex.x].xyz);
        }
        component2.m_EndPosition.m_Position = component2.m_Curve.d;
        component2.m_EndPosition.m_Rotation = NetUtils.GetNodeRotation(MathUtils.EndTangent(component2.m_Curve), ownerTransform.m_Rotation);
        component2.m_EndPosition.m_CourseDelta = 1f;
        component2.m_EndPosition.m_Elevation = (float2) curve.d.y;
        component2.m_EndPosition.m_ParentMesh = parentMesh.y;
        if (nodeIndex.y >= 0)
        {
          if ((componentData.m_Flags & Game.Net.GeometryFlags.OnWater) != (Game.Net.GeometryFlags) 0)
            component2.m_EndPosition.m_Position.xz = ObjectUtils.LocalToWorld(ownerTransform, nodePositions[nodeIndex.y].xyz).xz;
          else
            component2.m_EndPosition.m_Position = ObjectUtils.LocalToWorld(ownerTransform, nodePositions[nodeIndex.y].xyz);
        }
        component2.m_Length = MathUtils.Length(component2.m_Curve);
        component2.m_FixedIndex = -1;
        component2.m_StartPosition.m_Flags |= CoursePosFlags.IsFirst;
        component2.m_EndPosition.m_Flags |= CoursePosFlags.IsLast;
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

      private bool GetOwnerLot(Entity lotOwner, out BuildingUtils.LotInfo lotInfo)
      {
        Game.Buildings.Lot componentData1;
        Transform componentData2;
        PrefabRef componentData3;
        Game.Prefabs.BuildingData componentData4;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_LotData.TryGetComponent(lotOwner, out componentData1) && this.m_TransformData.TryGetComponent(lotOwner, out componentData2) && this.m_PrefabRefData.TryGetComponent(lotOwner, out componentData3) && this.m_PrefabBuildingData.TryGetComponent(componentData3.m_Prefab, out componentData4))
        {
          float2 extents = new float2(componentData4.m_LotSize) * 4f;
          Game.Objects.Elevation componentData5;
          // ISSUE: reference to a compiler-generated field
          this.m_ElevationData.TryGetComponent(lotOwner, out componentData5);
          DynamicBuffer<InstalledUpgrade> bufferData;
          // ISSUE: reference to a compiler-generated field
          this.m_InstalledUpgrades.TryGetBuffer(lotOwner, out bufferData);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          lotInfo = BuildingUtils.CalculateLotInfo(extents, componentData2, componentData5, componentData1, componentData3, bufferData, this.m_TransformData, this.m_PrefabRefData, this.m_PrefabObjectGeometryData, this.m_PrefabBuildingTerraformData, this.m_PrefabBuildingExtensionData, false, out bool _);
          return true;
        }
        lotInfo = new BuildingUtils.LotInfo();
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
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PseudoRandomSeed> __Game_Common_PseudoRandomSeed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> __Game_Objects_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Lot> __Game_Buildings_Lot_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Clear> __Game_Areas_Clear_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> __Game_Prefabs_AreaGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingTerraformData> __Game_Prefabs_BuildingTerraformData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingExtensionData> __Game_Prefabs_BuildingExtensionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableObjectData> __Game_Prefabs_SpawnableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> __Game_Areas_SubArea_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubArea> __Game_Prefabs_SubArea_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<SubAreaNode> __Game_Prefabs_SubAreaNode_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Prefabs.SubNet> __Game_Prefabs_SubNet_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<PlaceholderObjectElement> __Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PseudoRandomSeed_RO_ComponentLookup = state.GetComponentLookup<PseudoRandomSeed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Lot_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.Lot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Clear_RO_ComponentLookup = state.GetComponentLookup<Clear>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup = state.GetComponentLookup<AreaGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingTerraformData_RO_ComponentLookup = state.GetComponentLookup<BuildingTerraformData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup = state.GetComponentLookup<BuildingExtensionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableObjectData_RO_ComponentLookup = state.GetComponentLookup<SpawnableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Areas.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferLookup = state.GetBufferLookup<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubAreaNode_RO_BufferLookup = state.GetBufferLookup<SubAreaNode>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SubNet_RO_BufferLookup = state.GetBufferLookup<Game.Prefabs.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderObjectElement_RO_BufferLookup = state.GetBufferLookup<PlaceholderObjectElement>(true);
      }
    }
  }
}
