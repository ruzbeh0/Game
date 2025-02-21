// Decompiled with JetBrains decompiler
// Type: Game.Buildings.ZoneCheckSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Notifications;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using Game.Zones;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Buildings
{
  [CompilerGenerated]
  public class ZoneCheckSystem : GameSystemBase
  {
    private Game.Zones.UpdateCollectSystem m_ZoneUpdateCollectSystem;
    private Game.Zones.SearchSystem m_ZoneSearchSystem;
    private ModificationEndBarrier m_ModificationEndBarrier;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private ToolSystem m_ToolSystem;
    private IconCommandSystem m_IconCommandSystem;
    private EntityQuery m_BuildingSettingsQuery;
    private ZoneCheckSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneUpdateCollectSystem = this.World.GetOrCreateSystemManaged<Game.Zones.UpdateCollectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneSearchSystem = this.World.GetOrCreateSystemManaged<Game.Zones.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationEndBarrier = this.World.GetOrCreateSystemManaged<ModificationEndBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingSettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<BuildingConfigurationData>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ZoneUpdateCollectSystem.isUpdated || this.m_BuildingSettingsQuery.IsEmptyIgnoreFilter)
        return;
      NativeQueue<Entity> nativeQueue = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<Entity> nativeList = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeList<Bounds2> updatedBounds = this.m_ZoneUpdateCollectSystem.GetUpdatedBounds(true, out dependencies1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SignatureBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
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
      ZoneCheckSystem.FindSpawnableBuildingsJob jobData1 = new ZoneCheckSystem.FindSpawnableBuildingsJob()
      {
        m_Bounds = updatedBounds.AsDeferredJobArray(),
        m_SearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies2),
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabSpawnableBuildingData = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_PrefabSignatureBuildingData = this.__TypeHandle.__Game_Prefabs_SignatureBuildingData_RO_ComponentLookup,
        m_ResultQueue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ZoneCheckSystem.CollectEntitiesJob jobData2 = new ZoneCheckSystem.CollectEntitiesJob()
      {
        m_Queue = nativeQueue,
        m_List = nativeList
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Cell_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceholderBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_ValidArea_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Condemned_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies3;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ZoneCheckSystem.CheckBuildingZonesJob jobData3 = new ZoneCheckSystem.CheckBuildingZonesJob()
      {
        m_CondemnedData = this.__TypeHandle.__Game_Buildings_Condemned_RO_ComponentLookup,
        m_BlockData = this.__TypeHandle.__Game_Zones_Block_RO_ComponentLookup,
        m_ValidAreaData = this.__TypeHandle.__Game_Zones_ValidArea_RO_ComponentLookup,
        m_DestroyedData = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup,
        m_AbandonedData = this.__TypeHandle.__Game_Buildings_Abandoned_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_AttachedData = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabData = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_PrefabSpawnableBuildingData = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_PrefabPlaceholderBuildingData = this.__TypeHandle.__Game_Prefabs_PlaceholderBuildingData_RO_ComponentLookup,
        m_PrefabZoneData = this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup,
        m_Cells = this.__TypeHandle.__Game_Zones_Cell_RO_BufferLookup,
        m_BuildingConfigurationData = this.m_BuildingSettingsQuery.GetSingleton<BuildingConfigurationData>(),
        m_Buildings = nativeList.AsDeferredJobArray(),
        m_SearchTree = this.m_ZoneSearchSystem.GetSearchTree(true, out dependencies3),
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer(),
        m_CommandBuffer = this.m_ModificationEndBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      JobHandle jobHandle1 = jobData1.Schedule<ZoneCheckSystem.FindSpawnableBuildingsJob, Bounds2>(updatedBounds, 1, JobHandle.CombineDependencies(this.Dependency, dependencies1, dependencies2));
      JobHandle jobHandle2 = jobData2.Schedule<ZoneCheckSystem.CollectEntitiesJob>(jobHandle1);
      NativeList<Entity> list = nativeList;
      JobHandle dependsOn = JobHandle.CombineDependencies(jobHandle2, dependencies3);
      JobHandle jobHandle3 = jobData3.Schedule<ZoneCheckSystem.CheckBuildingZonesJob, Entity>(list, 1, dependsOn);
      nativeQueue.Dispose(jobHandle2);
      nativeList.Dispose(jobHandle3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ZoneUpdateCollectSystem.AddBoundsReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ZoneSearchSystem.AddSearchTreeReader(jobHandle3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(jobHandle3);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationEndBarrier.AddJobHandleForProducer(jobHandle3);
      this.Dependency = jobHandle3;
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
    public ZoneCheckSystem()
    {
    }

    [BurstCompile]
    private struct FindSpawnableBuildingsJob : IJobParallelForDefer
    {
      [ReadOnly]
      public NativeArray<Bounds2> m_Bounds;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_SearchTree;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_PrefabSpawnableBuildingData;
      [ReadOnly]
      public ComponentLookup<SignatureBuildingData> m_PrefabSignatureBuildingData;
      public NativeQueue<Entity>.ParallelWriter m_ResultQueue;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ZoneCheckSystem.FindSpawnableBuildingsJob.Iterator iterator = new ZoneCheckSystem.FindSpawnableBuildingsJob.Iterator()
        {
          m_Bounds = this.m_Bounds[index],
          m_ResultQueue = this.m_ResultQueue,
          m_BuildingData = this.m_BuildingData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabSpawnableBuildingData = this.m_PrefabSpawnableBuildingData,
          m_PrefabSignatureBuildingData = this.m_PrefabSignatureBuildingData
        };
        // ISSUE: reference to a compiler-generated field
        this.m_SearchTree.Iterate<ZoneCheckSystem.FindSpawnableBuildingsJob.Iterator>(ref iterator);
      }

      private struct Iterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds2 m_Bounds;
        public NativeQueue<Entity>.ParallelWriter m_ResultQueue;
        public ComponentLookup<Building> m_BuildingData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<SpawnableBuildingData> m_PrefabSpawnableBuildingData;
        public ComponentLookup<SignatureBuildingData> m_PrefabSignatureBuildingData;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity objectEntity)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds) || !this.m_BuildingData.HasComponent(objectEntity))
            return;
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[objectEntity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!this.m_PrefabSpawnableBuildingData.HasComponent(prefabRef.m_Prefab) || this.m_PrefabSignatureBuildingData.HasComponent(prefabRef.m_Prefab))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_ResultQueue.Enqueue(objectEntity);
        }
      }
    }

    [BurstCompile]
    private struct CollectEntitiesJob : IJob
    {
      public NativeQueue<Entity> m_Queue;
      public NativeList<Entity> m_List;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        int count = this.m_Queue.Count;
        if (count == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_List.ResizeUninitialized(count);
        for (int index = 0; index < count; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_List[index] = this.m_Queue.Dequeue();
        }
        // ISSUE: reference to a compiler-generated field
        NativeList<Entity> list = this.m_List;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ZoneCheckSystem.CollectEntitiesJob.EntityComparer entityComparer = new ZoneCheckSystem.CollectEntitiesJob.EntityComparer();
        // ISSUE: variable of a compiler-generated type
        ZoneCheckSystem.CollectEntitiesJob.EntityComparer comp = entityComparer;
        list.Sort<Entity, ZoneCheckSystem.CollectEntitiesJob.EntityComparer>(comp);
        Entity entity1 = Entity.Null;
        int num = 0;
        int index1 = 0;
        // ISSUE: reference to a compiler-generated field
        while (num < this.m_List.Length)
        {
          // ISSUE: reference to a compiler-generated field
          Entity entity2 = this.m_List[num++];
          if (entity2 != entity1)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_List[index1++] = entity2;
            entity1 = entity2;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (index1 >= this.m_List.Length)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_List.RemoveRangeSwapBack(index1, this.m_List.Length - index1);
      }

      [StructLayout(LayoutKind.Sequential, Size = 1)]
      private struct EntityComparer : IComparer<Entity>
      {
        public int Compare(Entity x, Entity y) => x.Index - y.Index;
      }
    }

    [BurstCompile]
    private struct CheckBuildingZonesJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<Condemned> m_CondemnedData;
      [ReadOnly]
      public ComponentLookup<Game.Zones.Block> m_BlockData;
      [ReadOnly]
      public ComponentLookup<ValidArea> m_ValidAreaData;
      [ReadOnly]
      public ComponentLookup<Destroyed> m_DestroyedData;
      [ReadOnly]
      public ComponentLookup<Abandoned> m_AbandonedData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Attached> m_AttachedData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<PrefabData> m_PrefabData;
      [ReadOnly]
      public ComponentLookup<BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_PrefabSpawnableBuildingData;
      [ReadOnly]
      public ComponentLookup<PlaceholderBuildingData> m_PrefabPlaceholderBuildingData;
      [ReadOnly]
      public ComponentLookup<ZoneData> m_PrefabZoneData;
      [ReadOnly]
      public BufferLookup<Cell> m_Cells;
      [ReadOnly]
      public BuildingConfigurationData m_BuildingConfigurationData;
      [ReadOnly]
      public NativeArray<Entity> m_Buildings;
      [ReadOnly]
      public NativeQuadTree<Entity, Bounds2> m_SearchTree;
      [ReadOnly]
      public bool m_EditorMode;
      public IconCommandBuffer m_IconCommandBuffer;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity building = this.m_Buildings[index];
        // ISSUE: reference to a compiler-generated field
        PrefabRef prefabRef = this.m_PrefabRefData[building];
        // ISSUE: reference to a compiler-generated field
        BuildingData prefabBuildingData = this.m_PrefabBuildingData[prefabRef.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        SpawnableBuildingData prefabSpawnableBuildingData = this.m_PrefabSpawnableBuildingData[prefabRef.m_Prefab];
        // ISSUE: reference to a compiler-generated field
        bool flag = this.m_EditorMode;
        if (!flag)
        {
          // ISSUE: reference to a compiler-generated method
          flag = this.ValidateAttachedParent(building, prefabBuildingData, prefabSpawnableBuildingData);
        }
        if (!flag)
        {
          // ISSUE: reference to a compiler-generated method
          flag = this.ValidateZoneBlocks(building, prefabBuildingData, prefabSpawnableBuildingData);
        }
        if (flag)
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_CondemnedData.HasComponent(building))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Condemned>(index, building);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_IconCommandBuffer.Remove(building, this.m_BuildingConfigurationData.m_CondemnedNotification);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_CondemnedData.HasComponent(building))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Condemned>(index, building, new Condemned());
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_DestroyedData.HasComponent(building) || this.m_AbandonedData.HasComponent(building))
            return;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_IconCommandBuffer.Add(building, this.m_BuildingConfigurationData.m_CondemnedNotification, IconPriority.FatalProblem);
        }
      }

      private bool ValidateAttachedParent(
        Entity building,
        BuildingData prefabBuildingData,
        SpawnableBuildingData prefabSpawnableBuildingData)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_AttachedData.HasComponent(building))
        {
          // ISSUE: reference to a compiler-generated field
          Attached attached = this.m_AttachedData[building];
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabRefData.HasComponent(attached.m_Parent))
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[attached.m_Parent];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabPlaceholderBuildingData.HasComponent(prefabRef.m_Prefab) && this.m_PrefabPlaceholderBuildingData[prefabRef.m_Prefab].m_ZonePrefab == prefabSpawnableBuildingData.m_ZonePrefab)
              return true;
          }
        }
        return false;
      }

      private bool ValidateZoneBlocks(
        Entity building,
        BuildingData prefabBuildingData,
        SpawnableBuildingData prefabSpawnableBuildingData)
      {
        // ISSUE: reference to a compiler-generated field
        Transform transform = this.m_TransformData[building];
        ZoneData componentData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabZoneData.TryGetComponent(prefabSpawnableBuildingData.m_ZonePrefab, out componentData) && !componentData.m_ZoneType.Equals(ZoneType.None) && !this.m_PrefabData.IsComponentEnabled(prefabSpawnableBuildingData.m_ZonePrefab))
          return false;
        float2 xz1 = math.rotate(transform.m_Rotation, new float3(8f, 0.0f, 0.0f)).xz;
        float2 xz2 = math.rotate(transform.m_Rotation, new float3(0.0f, 0.0f, 8f)).xz;
        float2 x1 = xz1 * (float) ((double) prefabBuildingData.m_LotSize.x * 0.5 - 0.5);
        float2 x2 = xz2 * (float) ((double) prefabBuildingData.m_LotSize.y * 0.5 - 0.5);
        float2 float2 = math.abs(x2) + math.abs(x1);
        NativeArray<bool> nativeArray = new NativeArray<bool>(prefabBuildingData.m_LotSize.x * prefabBuildingData.m_LotSize.y, Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        ZoneCheckSystem.CheckBuildingZonesJob.Iterator iterator = new ZoneCheckSystem.CheckBuildingZonesJob.Iterator()
        {
          m_Bounds = new Bounds2(transform.m_Position.xz - float2, transform.m_Position.xz + float2),
          m_LotSize = prefabBuildingData.m_LotSize,
          m_StartPosition = transform.m_Position.xz + x2 + x1,
          m_Right = xz1,
          m_Forward = xz2,
          m_ZoneType = componentData.m_ZoneType,
          m_Validated = nativeArray,
          m_BlockData = this.m_BlockData,
          m_ValidAreaData = this.m_ValidAreaData,
          m_Cells = this.m_Cells
        };
        // ISSUE: reference to a compiler-generated field
        this.m_SearchTree.Iterate<ZoneCheckSystem.CheckBuildingZonesJob.Iterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        bool flag = (iterator.m_Directions & CellFlags.Roadside) != 0;
        for (int index = 0; index < nativeArray.Length; ++index)
          flag &= nativeArray[index];
        nativeArray.Dispose();
        return flag;
      }

      private struct Iterator : 
        INativeQuadTreeIterator<Entity, Bounds2>,
        IUnsafeQuadTreeIterator<Entity, Bounds2>
      {
        public Bounds2 m_Bounds;
        public int2 m_LotSize;
        public float2 m_StartPosition;
        public float2 m_Right;
        public float2 m_Forward;
        public ZoneType m_ZoneType;
        public CellFlags m_Directions;
        public NativeArray<bool> m_Validated;
        public ComponentLookup<Game.Zones.Block> m_BlockData;
        public ComponentLookup<ValidArea> m_ValidAreaData;
        public BufferLookup<Cell> m_Cells;

        public bool Intersect(Bounds2 bounds) => MathUtils.Intersect(bounds, this.m_Bounds);

        public void Iterate(Bounds2 bounds, Entity blockEntity)
        {
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds, this.m_Bounds))
            return;
          // ISSUE: reference to a compiler-generated field
          ValidArea validArea = this.m_ValidAreaData[blockEntity];
          if (validArea.m_Area.y <= validArea.m_Area.x)
            return;
          // ISSUE: reference to a compiler-generated field
          Game.Zones.Block target = new Game.Zones.Block()
          {
            m_Direction = this.m_Forward
          };
          // ISSUE: reference to a compiler-generated field
          Game.Zones.Block block = this.m_BlockData[blockEntity];
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<Cell> cell1 = this.m_Cells[blockEntity];
          // ISSUE: reference to a compiler-generated field
          float2 startPosition = this.m_StartPosition;
          int2 int2;
          // ISSUE: reference to a compiler-generated field
          for (int2.y = 0; int2.y < this.m_LotSize.y; ++int2.y)
          {
            float2 position = startPosition;
            // ISSUE: reference to a compiler-generated field
            for (int2.x = 0; int2.x < this.m_LotSize.x; ++int2.x)
            {
              int2 cellIndex = ZoneUtils.GetCellIndex(block, position);
              if (math.all(cellIndex >= validArea.m_Area.xz & cellIndex < validArea.m_Area.yw))
              {
                int index = cellIndex.y * block.m_Size.x + cellIndex.x;
                Cell cell2 = cell1[index];
                // ISSUE: reference to a compiler-generated field
                if ((cell2.m_State & CellFlags.Visible) != CellFlags.None && cell2.m_Zone.Equals(this.m_ZoneType))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_Validated[int2.y * this.m_LotSize.x + int2.x] = true;
                  if ((cell2.m_State & (CellFlags.Roadside | CellFlags.RoadLeft | CellFlags.RoadRight | CellFlags.RoadBack)) != CellFlags.None)
                  {
                    CellFlags roadDirection = ZoneUtils.GetRoadDirection(target, block, cell2.m_State);
                    int4 int4 = new int4(512, 4, 1024, 2048);
                    // ISSUE: reference to a compiler-generated field
                    int4 = math.select((int4) 0, int4, new bool4(int2 == 0, int2 == this.m_LotSize - 1));
                    // ISSUE: reference to a compiler-generated field
                    this.m_Directions |= roadDirection & (CellFlags) math.csum(int4);
                  }
                }
              }
              // ISSUE: reference to a compiler-generated field
              position -= this.m_Right;
            }
            // ISSUE: reference to a compiler-generated field
            startPosition -= this.m_Forward;
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SignatureBuildingData> __Game_Prefabs_SignatureBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Condemned> __Game_Buildings_Condemned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Zones.Block> __Game_Zones_Block_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ValidArea> __Game_Zones_ValidArea_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Destroyed> __Game_Common_Destroyed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Abandoned> __Game_Buildings_Abandoned_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attached> __Game_Objects_Attached_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceholderBuildingData> __Game_Prefabs_PlaceholderBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ZoneData> __Game_Prefabs_ZoneData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Cell> __Game_Zones_Cell_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SignatureBuildingData_RO_ComponentLookup = state.GetComponentLookup<SignatureBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Condemned_RO_ComponentLookup = state.GetComponentLookup<Condemned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Block_RO_ComponentLookup = state.GetComponentLookup<Game.Zones.Block>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_ValidArea_RO_ComponentLookup = state.GetComponentLookup<ValidArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentLookup = state.GetComponentLookup<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Abandoned_RO_ComponentLookup = state.GetComponentLookup<Abandoned>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentLookup = state.GetComponentLookup<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentLookup = state.GetComponentLookup<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceholderBuildingData_RO_ComponentLookup = state.GetComponentLookup<PlaceholderBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZoneData_RO_ComponentLookup = state.GetComponentLookup<ZoneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Zones_Cell_RO_BufferLookup = state.GetBufferLookup<Cell>(true);
      }
    }
  }
}
