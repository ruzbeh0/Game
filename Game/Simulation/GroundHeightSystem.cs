// Decompiled with JetBrains decompiler
// Type: Game.Simulation.GroundHeightSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Rendering;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class GroundHeightSystem : GameSystemBase, IJobSerializable
  {
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private ModificationBarrier2 m_ModificationBarrier;
    private Game.Objects.SearchSystem m_ObjectSearchSystem;
    private Game.Net.SearchSystem m_NetSearchSystem;
    private Game.Areas.SearchSystem m_AreaSearchSystem;
    private ToolSystem m_ToolSystem;
    private NativeList<Bounds2> m_NewUpdates;
    private NativeList<Bounds2> m_PendingUpdates;
    private NativeList<Bounds2> m_ReadingUpdates;
    private NativeList<Bounds2> m_ReadyUpdates;
    private JobHandle m_UpdateDeps;
    private GroundHeightSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier2>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectSearchSystem = this.World.GetOrCreateSystemManaged<Game.Objects.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<Game.Net.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaSearchSystem = this.World.GetOrCreateSystemManaged<Game.Areas.SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      if (this.m_NewUpdates.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_NewUpdates.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_PendingUpdates.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_PendingUpdates.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_ReadingUpdates.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ReadingUpdates.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_ReadyUpdates.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ReadyUpdates.Dispose();
      }
      base.OnDestroy();
    }

    public NativeList<Bounds2> GetUpdateBuffer()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_NewUpdates.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ReadyUpdates.IsCreated && this.m_ReadyUpdates.Length == 0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_NewUpdates = this.m_ReadyUpdates;
          // ISSUE: reference to a compiler-generated field
          this.m_ReadyUpdates = new NativeList<Bounds2>();
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_NewUpdates = new NativeList<Bounds2>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
        }
      }
      // ISSUE: reference to a compiler-generated field
      return this.m_NewUpdates;
    }

    public void BeforeUpdateHeights()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_NewUpdates.IsCreated || this.m_NewUpdates.Length == 0)
        return;
      // ISSUE: reference to a compiler-generated field
      if (this.m_PendingUpdates.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PendingUpdates.AddRange(this.m_NewUpdates.AsArray());
        // ISSUE: reference to a compiler-generated field
        this.m_NewUpdates.Clear();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PendingUpdates = this.m_NewUpdates;
        // ISSUE: reference to a compiler-generated field
        this.m_NewUpdates = new NativeList<Bounds2>();
      }
    }

    public void BeforeReadHeights()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_PendingUpdates.IsCreated || this.m_PendingUpdates.Length == 0)
        return;
      // ISSUE: reference to a compiler-generated field
      if (this.m_ReadingUpdates.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ReadingUpdates.AddRange(this.m_PendingUpdates.AsArray());
        // ISSUE: reference to a compiler-generated field
        this.m_PendingUpdates.Clear();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ReadingUpdates = this.m_PendingUpdates;
        // ISSUE: reference to a compiler-generated field
        this.m_PendingUpdates = new NativeList<Bounds2>();
      }
    }

    public void AfterReadHeights()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ReadingUpdates.IsCreated || this.m_ReadingUpdates.Length == 0)
        return;
      // ISSUE: reference to a compiler-generated field
      if (this.m_ReadyUpdates.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ReadyUpdates.AddRange(this.m_ReadingUpdates.AsArray());
        // ISSUE: reference to a compiler-generated field
        this.m_ReadingUpdates.Clear();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ReadyUpdates = this.m_ReadingUpdates;
        // ISSUE: reference to a compiler-generated field
        this.m_ReadingUpdates = new NativeList<Bounds2>();
      }
    }

    public JobHandle Serialize<TWriter>(EntityWriterData writerData, JobHandle inputDeps) where TWriter : struct, IWriter
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GroundHeightSystem.SerializeJob<TWriter> jobData = new GroundHeightSystem.SerializeJob<TWriter>()
      {
        m_NewUpdates = this.m_NewUpdates,
        m_PendingUpdates = this.m_PendingUpdates,
        m_ReadingUpdates = this.m_ReadingUpdates,
        m_ReadyUpdates = this.m_ReadyUpdates,
        m_WriterData = writerData
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateDeps = jobData.Schedule<GroundHeightSystem.SerializeJob<TWriter>>(JobHandle.CombineDependencies(inputDeps, this.m_UpdateDeps));
      // ISSUE: reference to a compiler-generated field
      return this.m_UpdateDeps;
    }

    public JobHandle Deserialize<TReader>(EntityReaderData readerData, JobHandle inputDeps) where TReader : struct, IReader
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ReadingUpdates.IsCreated)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ReadingUpdates = new NativeList<Bounds2>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GroundHeightSystem.DeserializeJob<TReader> jobData = new GroundHeightSystem.DeserializeJob<TReader>()
      {
        m_NewUpdates = this.m_NewUpdates,
        m_PendingUpdates = this.m_PendingUpdates,
        m_ReadingUpdates = this.m_ReadingUpdates,
        m_ReadyUpdates = this.m_ReadyUpdates,
        m_ReaderData = readerData
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateDeps = jobData.Schedule<GroundHeightSystem.DeserializeJob<TReader>>(JobHandle.CombineDependencies(inputDeps, this.m_UpdateDeps));
      // ISSUE: reference to a compiler-generated field
      return this.m_UpdateDeps;
    }

    public JobHandle SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GroundHeightSystem.SetDefaultsJob jobData = new GroundHeightSystem.SetDefaultsJob()
      {
        m_NewUpdates = this.m_NewUpdates,
        m_PendingUpdates = this.m_PendingUpdates,
        m_ReadingUpdates = this.m_ReadingUpdates,
        m_ReadyUpdates = this.m_ReadyUpdates
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateDeps = jobData.Schedule<GroundHeightSystem.SetDefaultsJob>(this.m_UpdateDeps);
      // ISSUE: reference to a compiler-generated field
      return this.m_UpdateDeps;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateDeps.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ReadyUpdates.IsCreated || this.m_ReadyUpdates.Length == 0)
        return;
      NativeQueue<Entity> nativeQueue = new NativeQueue<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NativeList<Entity> nativeList = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Lot_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies1;
      JobHandle dependencies2;
      JobHandle dependencies3;
      JobHandle dependencies4;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GroundHeightSystem.BoundsFindJob jobData1 = new GroundHeightSystem.BoundsFindJob()
      {
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_ObjectElevationData = this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup,
        m_BuildingLotData = this.__TypeHandle.__Game_Buildings_Lot_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_NetElevationData = this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabNetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_AreaNodes = this.__TypeHandle.__Game_Areas_Node_RO_BufferLookup,
        m_Triangles = this.__TypeHandle.__Game_Areas_Triangle_RO_BufferLookup,
        m_ReadyUpdates = this.m_ReadyUpdates,
        m_StaticObjectSearchTree = this.m_ObjectSearchSystem.GetStaticSearchTree(true, out dependencies1),
        m_LaneSearchTree = this.m_NetSearchSystem.GetLaneSearchTree(true, out dependencies2),
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies3),
        m_AreaSearchTree = this.m_AreaSearchSystem.GetSearchTree(true, out dependencies4),
        m_Queue = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GroundHeightSystem.DequeueJob jobData2 = new GroundHeightSystem.DequeueJob()
      {
        m_Queue = nativeQueue,
        m_List = nativeList,
        m_ReadyUpdates = this.m_ReadyUpdates
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Node_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_CullingInfo_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingTerraformData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Lot_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_MovedLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GroundHeightSystem.UpdateHeightsJob jobData3 = new GroundHeightSystem.UpdateHeightsJob()
      {
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_NetElevationData = this.__TypeHandle.__Game_Net_Elevation_RO_ComponentLookup,
        m_ObjectElevationData = this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup,
        m_SpawnLocationData = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentLookup,
        m_MovedLocationData = this.__TypeHandle.__Game_Objects_MovedLocation_RO_ComponentLookup,
        m_BuildingLotData = this.__TypeHandle.__Game_Buildings_Lot_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabPlaceableObjectData = this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup,
        m_PrefabObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup,
        m_PrefabNetGeometryData = this.__TypeHandle.__Game_Prefabs_NetGeometryData_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_PrefabBuildingExtensionData = this.__TypeHandle.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup,
        m_PrefabBuildingTerraformData = this.__TypeHandle.__Game_Prefabs_BuildingTerraformData_RO_ComponentLookup,
        m_PrefabAreaGeometryData = this.__TypeHandle.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup,
        m_ConnectedEdges = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RW_ComponentLookup,
        m_CurveData = this.__TypeHandle.__Game_Net_Curve_RW_ComponentLookup,
        m_NodeData = this.__TypeHandle.__Game_Net_Node_RW_ComponentLookup,
        m_CullingInfoData = this.__TypeHandle.__Game_Rendering_CullingInfo_RW_ComponentLookup,
        m_AreaNodes = this.__TypeHandle.__Game_Areas_Node_RW_BufferLookup,
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_List = nativeList,
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_WaterSurfaceData = this.m_WaterSystem.GetSurfaceData(out deps),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle1 = jobData1.Schedule<GroundHeightSystem.BoundsFindJob>(this.m_ReadyUpdates.Length, 1, JobHandle.CombineDependencies(this.Dependency, dependencies4, JobHandle.CombineDependencies(dependencies1, dependencies2, dependencies3)));
      JobHandle jobHandle2 = jobData2.Schedule<GroundHeightSystem.DequeueJob>(jobHandle1);
      NativeList<Entity> list = nativeList;
      JobHandle dependsOn = JobHandle.CombineDependencies(jobHandle2, deps);
      JobHandle jobHandle3 = jobData3.Schedule<GroundHeightSystem.UpdateHeightsJob, Entity>(list, 4, dependsOn);
      nativeQueue.Dispose(jobHandle2);
      nativeList.Dispose(jobHandle3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectSearchSystem.AddStaticSearchTreeReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddLaneSearchTreeReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AreaSearchSystem.AddSearchTreeReader(jobHandle1);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(jobHandle3);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(jobHandle3);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle3);
      this.Dependency = jobHandle3;
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateDeps = jobHandle2;
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
    public GroundHeightSystem()
    {
    }

    [BurstCompile]
    internal struct SerializeJob<TWriter> : IJob where TWriter : struct, IWriter
    {
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeList<Bounds2> m_NewUpdates;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeList<Bounds2> m_PendingUpdates;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeList<Bounds2> m_ReadingUpdates;
      [NativeDisableContainerSafetyRestriction]
      [ReadOnly]
      public NativeList<Bounds2> m_ReadyUpdates;
      public EntityWriterData m_WriterData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        TWriter writer = this.m_WriterData.GetWriter<TWriter>();
        int num = 0;
        // ISSUE: reference to a compiler-generated field
        if (this.m_NewUpdates.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          num += this.m_NewUpdates.Length;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_PendingUpdates.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          num += this.m_PendingUpdates.Length;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ReadingUpdates.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          num += this.m_ReadingUpdates.Length;
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ReadyUpdates.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          num += this.m_ReadyUpdates.Length;
        }
        writer.Write(num);
        // ISSUE: reference to a compiler-generated field
        if (this.m_NewUpdates.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index = 0; index < this.m_NewUpdates.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            Bounds2 newUpdate = this.m_NewUpdates[index];
            writer.Write(newUpdate.min);
            writer.Write(newUpdate.max);
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_PendingUpdates.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index = 0; index < this.m_PendingUpdates.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            Bounds2 pendingUpdate = this.m_PendingUpdates[index];
            writer.Write(pendingUpdate.min);
            writer.Write(pendingUpdate.max);
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ReadingUpdates.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          for (int index = 0; index < this.m_ReadingUpdates.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            Bounds2 readingUpdate = this.m_ReadingUpdates[index];
            writer.Write(readingUpdate.min);
            writer.Write(readingUpdate.max);
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ReadyUpdates.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_ReadyUpdates.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          Bounds2 readyUpdate = this.m_ReadyUpdates[index];
          writer.Write(readyUpdate.min);
          writer.Write(readyUpdate.max);
        }
      }
    }

    [BurstCompile]
    internal struct DeserializeJob<TReader> : IJob where TReader : struct, IReader
    {
      [NativeDisableContainerSafetyRestriction]
      public NativeList<Bounds2> m_NewUpdates;
      [NativeDisableContainerSafetyRestriction]
      public NativeList<Bounds2> m_PendingUpdates;
      [NativeDisableContainerSafetyRestriction]
      public NativeList<Bounds2> m_ReadingUpdates;
      [NativeDisableContainerSafetyRestriction]
      public NativeList<Bounds2> m_ReadyUpdates;
      public EntityReaderData m_ReaderData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_NewUpdates.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_NewUpdates.Clear();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_PendingUpdates.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_PendingUpdates.Clear();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ReadyUpdates.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ReadyUpdates.Clear();
        }
        // ISSUE: reference to a compiler-generated field
        TReader reader = this.m_ReaderData.GetReader<TReader>();
        int length;
        reader.Read(out length);
        // ISSUE: reference to a compiler-generated field
        this.m_ReadingUpdates.ResizeUninitialized(length);
        for (int index = 0; index < length; ++index)
        {
          Bounds2 bounds2;
          reader.Read(out bounds2.min);
          reader.Read(out bounds2.max);
          // ISSUE: reference to a compiler-generated field
          this.m_ReadingUpdates[index] = bounds2;
        }
      }
    }

    [BurstCompile]
    private struct SetDefaultsJob : IJob
    {
      [NativeDisableContainerSafetyRestriction]
      public NativeList<Bounds2> m_NewUpdates;
      [NativeDisableContainerSafetyRestriction]
      public NativeList<Bounds2> m_PendingUpdates;
      [NativeDisableContainerSafetyRestriction]
      public NativeList<Bounds2> m_ReadingUpdates;
      [NativeDisableContainerSafetyRestriction]
      public NativeList<Bounds2> m_ReadyUpdates;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_NewUpdates.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_NewUpdates.Clear();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_PendingUpdates.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_PendingUpdates.Clear();
        }
        // ISSUE: reference to a compiler-generated field
        if (this.m_ReadingUpdates.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ReadingUpdates.Clear();
        }
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ReadyUpdates.IsCreated)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_ReadyUpdates.Clear();
      }
    }

    [BurstCompile]
    private struct BoundsFindJob : IJobParallelFor
    {
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> m_ObjectElevationData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Lot> m_BuildingLotData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> m_NetElevationData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabNetGeometryData;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> m_AreaNodes;
      [ReadOnly]
      public BufferLookup<Triangle> m_Triangles;
      [ReadOnly]
      public NativeList<Bounds2> m_ReadyUpdates;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_StaticObjectSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_LaneSearchTree;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public NativeQuadTree<AreaSearchItem, QuadTreeBoundsXZ> m_AreaSearchTree;
      public NativeQueue<Entity>.ParallelWriter m_Queue;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Bounds2 readyUpdate = this.m_ReadyUpdates[index];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GroundHeightSystem.BoundsFindJob.ObjectIterator iterator1 = new GroundHeightSystem.BoundsFindJob.ObjectIterator()
        {
          m_Bounds = readyUpdate,
          m_ElevationData = this.m_ObjectElevationData,
          m_PrefabRefData = this.m_PrefabRefData,
          m_PrefabObjectGeometryData = this.m_PrefabObjectGeometryData,
          m_Queue = this.m_Queue
        };
        // ISSUE: reference to a compiler-generated field
        this.m_StaticObjectSearchTree.Iterate<GroundHeightSystem.BoundsFindJob.ObjectIterator>(ref iterator1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GroundHeightSystem.BoundsFindJob.LaneIterator iterator2 = new GroundHeightSystem.BoundsFindJob.LaneIterator()
        {
          m_Bounds = readyUpdate,
          m_OwnerData = this.m_OwnerData,
          m_ObjectElevationData = iterator1.m_ElevationData,
          m_NetElevationData = this.m_NetElevationData,
          m_PrefabRefData = iterator1.m_PrefabRefData,
          m_PrefabObjectGeometryData = iterator1.m_PrefabObjectGeometryData,
          m_Queue = this.m_Queue
        };
        // ISSUE: reference to a compiler-generated field
        this.m_LaneSearchTree.Iterate<GroundHeightSystem.BoundsFindJob.LaneIterator>(ref iterator2);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GroundHeightSystem.BoundsFindJob.NetIterator iterator3 = new GroundHeightSystem.BoundsFindJob.NetIterator()
        {
          m_Bounds = readyUpdate,
          m_EdgeData = this.m_EdgeData,
          m_OwnerData = iterator2.m_OwnerData,
          m_BuildingLotData = this.m_BuildingLotData,
          m_ElevationData = iterator2.m_NetElevationData,
          m_PrefabRefData = iterator2.m_PrefabRefData,
          m_PrefabNetGeometryData = this.m_PrefabNetGeometryData,
          m_Queue = this.m_Queue
        };
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<GroundHeightSystem.BoundsFindJob.NetIterator>(ref iterator3);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        GroundHeightSystem.BoundsFindJob.AreaIterator iterator4 = new GroundHeightSystem.BoundsFindJob.AreaIterator()
        {
          m_Bounds = readyUpdate,
          m_Nodes = this.m_AreaNodes,
          m_Triangles = this.m_Triangles,
          m_Queue = this.m_Queue
        };
        // ISSUE: reference to a compiler-generated field
        this.m_AreaSearchTree.Iterate<GroundHeightSystem.BoundsFindJob.AreaIterator>(ref iterator4);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_OwnerData = iterator3.m_OwnerData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ObjectElevationData = iterator2.m_ObjectElevationData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingLotData = iterator3.m_BuildingLotData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_EdgeData = iterator3.m_EdgeData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_NetElevationData = iterator3.m_ElevationData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabRefData = iterator3.m_PrefabRefData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabObjectGeometryData = iterator1.m_PrefabObjectGeometryData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabNetGeometryData = iterator3.m_PrefabNetGeometryData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AreaNodes = iterator4.m_Nodes;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_Triangles = iterator4.m_Triangles;
      }

      private struct ObjectIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds2 m_Bounds;
        public ComponentLookup<Game.Objects.Elevation> m_ElevationData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
        public NativeQueue<Entity>.ParallelWriter m_Queue;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds))
            return;
          ObjectGeometryData componentData1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabObjectGeometryData.TryGetComponent(this.m_PrefabRefData[entity].m_Prefab, out componentData1))
          {
            if ((componentData1.m_Flags & Game.Objects.GeometryFlags.HasBase) == Game.Objects.GeometryFlags.None)
            {
              if ((componentData1.m_Flags & Game.Objects.GeometryFlags.DeleteOverridden) != Game.Objects.GeometryFlags.None || (componentData1.m_Flags & (Game.Objects.GeometryFlags.Overridable | Game.Objects.GeometryFlags.Marker)) == Game.Objects.GeometryFlags.None)
                return;
            }
            else
              goto label_7;
          }
          Game.Objects.Elevation componentData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ElevationData.TryGetComponent(entity, out componentData2) && (componentData2.m_Flags & ElevationFlags.OnGround) == (ElevationFlags) 0)
            return;
label_7:
          // ISSUE: reference to a compiler-generated field
          this.m_Queue.Enqueue(entity);
        }
      }

      private struct LaneIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds2 m_Bounds;
        public ComponentLookup<Owner> m_OwnerData;
        public ComponentLookup<Game.Objects.Elevation> m_ObjectElevationData;
        public ComponentLookup<Game.Net.Elevation> m_NetElevationData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
        public NativeQueue<Entity>.ParallelWriter m_Queue;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
        {
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds))
            return;
          Owner componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerData.TryGetComponent(entity, out componentData1))
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[componentData1.m_Owner];
            Game.Objects.Elevation componentData2;
            ObjectGeometryData componentData3;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ObjectElevationData.TryGetComponent(componentData1.m_Owner, out componentData2) && (componentData2.m_Flags & ElevationFlags.OnGround) == (ElevationFlags) 0 || !this.m_PrefabObjectGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData3) || (componentData3.m_Flags & Game.Objects.GeometryFlags.DeleteOverridden) == Game.Objects.GeometryFlags.None && (componentData3.m_Flags & (Game.Objects.GeometryFlags.Overridable | Game.Objects.GeometryFlags.Marker)) != Game.Objects.GeometryFlags.None)
              return;
          }
          Game.Net.Elevation componentData4;
          // ISSUE: reference to a compiler-generated field
          if (this.m_NetElevationData.TryGetComponent(entity, out componentData4) && math.all(componentData4.m_Elevation != float.MinValue))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_Queue.Enqueue(entity);
        }
      }

      private struct NetIterator : 
        INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
      {
        public Bounds2 m_Bounds;
        public ComponentLookup<Owner> m_OwnerData;
        public ComponentLookup<Game.Buildings.Lot> m_BuildingLotData;
        public ComponentLookup<Edge> m_EdgeData;
        public ComponentLookup<Game.Net.Elevation> m_ElevationData;
        public ComponentLookup<PrefabRef> m_PrefabRefData;
        public ComponentLookup<NetGeometryData> m_PrefabNetGeometryData;
        public NativeQueue<Entity>.ParallelWriter m_Queue;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
        {
          Edge componentData1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds) || this.m_ElevationData.HasComponent(entity) && (!this.m_EdgeData.TryGetComponent(entity, out componentData1) || this.m_ElevationData.HasComponent(componentData1.m_Start) && this.m_ElevationData.HasComponent(componentData1.m_End)))
            return;
          NetGeometryData componentData2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabNetGeometryData.TryGetComponent(this.m_PrefabRefData[entity].m_Prefab, out componentData2))
          {
            if ((componentData2.m_Flags & Game.Net.GeometryFlags.OnWater) != (Game.Net.GeometryFlags) 0)
              return;
            if ((componentData2.m_Flags & Game.Net.GeometryFlags.FlattenTerrain) != (Game.Net.GeometryFlags) 0)
            {
              bool flag = false;
              Entity entity1 = entity;
              // ISSUE: reference to a compiler-generated field
              while (this.m_OwnerData.HasComponent(entity1))
              {
                // ISSUE: reference to a compiler-generated field
                entity1 = this.m_OwnerData[entity1].m_Owner;
                // ISSUE: reference to a compiler-generated field
                if (this.m_BuildingLotData.HasComponent(entity1))
                {
                  flag = true;
                  break;
                }
              }
              if (!flag)
                return;
            }
          }
          // ISSUE: reference to a compiler-generated field
          this.m_Queue.Enqueue(entity);
        }
      }

      private struct AreaIterator : 
        INativeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>,
        IUnsafeQuadTreeIterator<AreaSearchItem, QuadTreeBoundsXZ>
      {
        public Bounds2 m_Bounds;
        public BufferLookup<Game.Areas.Node> m_Nodes;
        public BufferLookup<Triangle> m_Triangles;
        public NativeQueue<Entity>.ParallelWriter m_Queue;

        public bool Intersect(QuadTreeBoundsXZ bounds)
        {
          // ISSUE: reference to a compiler-generated field
          return MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds);
        }

        public void Iterate(QuadTreeBoundsXZ bounds, AreaSearchItem item)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!MathUtils.Intersect(bounds.m_Bounds.xz, this.m_Bounds) || !math.any(AreaUtils.GetElevations(this.m_Nodes[item.m_Area], this.m_Triangles[item.m_Area][item.m_Triangle]) == float.MinValue))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_Queue.Enqueue(item.m_Area);
        }
      }
    }

    [BurstCompile]
    private struct DequeueJob : IJob
    {
      public NativeQueue<Entity> m_Queue;
      public NativeList<Entity> m_List;
      public NativeList<Bounds2> m_ReadyUpdates;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> array = this.m_Queue.ToArray((AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        this.m_List.Capacity = array.Length;
        array.Sort<Entity>();
        Entity entity1 = Entity.Null;
        for (int index = 0; index < array.Length; ++index)
        {
          Entity entity2 = array[index];
          if (entity2 != entity1)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_List.Add(in entity2);
            entity1 = entity2;
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_ReadyUpdates.Clear();
        array.Dispose();
      }
    }

    [BurstCompile]
    private struct UpdateHeightsJob : IJobParallelForDefer
    {
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> m_NetElevationData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> m_ObjectElevationData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> m_SpawnLocationData;
      [ReadOnly]
      public ComponentLookup<MovedLocation> m_MovedLocationData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Lot> m_BuildingLotData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> m_PrefabPlaceableObjectData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_PrefabObjectGeometryData;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> m_PrefabNetGeometryData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<BuildingExtensionData> m_PrefabBuildingExtensionData;
      [ReadOnly]
      public ComponentLookup<BuildingTerraformData> m_PrefabBuildingTerraformData;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> m_PrefabAreaGeometryData;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> m_ConnectedEdges;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Transform> m_TransformData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Curve> m_CurveData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Game.Net.Node> m_NodeData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<CullingInfo> m_CullingInfoData;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Game.Areas.Node> m_AreaNodes;
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public NativeList<Entity> m_List;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_List[index];
        Transform componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransformData.TryGetComponent(entity, out componentData1))
        {
          bool flag1 = true;
          bool flag2 = false;
          // ISSUE: reference to a compiler-generated field
          PrefabRef prefabRef = this.m_PrefabRefData[entity];
          Game.Objects.Elevation componentData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ObjectElevationData.TryGetComponent(entity, out componentData2))
            flag1 = (componentData2.m_Flags & ElevationFlags.OnGround) != 0;
          ObjectGeometryData componentData3;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabObjectGeometryData.TryGetComponent(prefabRef.m_Prefab, out componentData3))
          {
            flag2 = (componentData3.m_Flags & Game.Objects.GeometryFlags.HasBase) != 0;
            flag1 = ((flag1 ? 1 : 0) & ((componentData3.m_Flags & Game.Objects.GeometryFlags.DeleteOverridden) != Game.Objects.GeometryFlags.None ? 0 : ((componentData3.m_Flags & (Game.Objects.GeometryFlags.Overridable | Game.Objects.GeometryFlags.Marker)) != 0 ? 1 : 0))) != 0;
          }
          if (flag1)
          {
            bool angledSample;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Transform adjusted = ObjectUtils.AdjustPosition(componentData1, componentData2, prefabRef.m_Prefab, out angledSample, ref this.m_TerrainHeightData, ref this.m_WaterSurfaceData, ref this.m_PrefabPlaceableObjectData, ref this.m_PrefabObjectGeometryData);
            if ((double) math.abs(adjusted.m_Position.y - componentData1.m_Position.y) >= 0.0099999997764825821 || angledSample && (double) MathUtils.RotationAngle(adjusted.m_Rotation, componentData1.m_Rotation) >= (double) math.radians(0.1f))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_TransformData[entity] = adjusted;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(index, entity, new Updated());
              flag2 = false;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_SpawnLocationData.HasComponent(entity) && !this.m_MovedLocationData.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<MovedLocation>(index, entity, new MovedLocation()
                {
                  m_OldPosition = componentData1.m_Position
                });
              }
              DynamicBuffer<Game.Objects.SubObject> bufferData;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_SubObjects.TryGetBuffer(entity, out bufferData) && (this.m_OwnerData.HasComponent(entity) || this.m_EditorMode))
              {
                // ISSUE: reference to a compiler-generated method
                this.HandleSubObjects(index, bufferData, componentData1, adjusted);
              }
            }
          }
          CullingInfo componentData4;
          // ISSUE: reference to a compiler-generated field
          if (!flag2 || !this.m_CullingInfoData.TryGetComponent(entity, out componentData4))
            return;
          // ISSUE: reference to a compiler-generated field
          float min = TerrainUtils.GetHeightRange(ref this.m_TerrainHeightData, componentData4.m_Bounds).min;
          if ((double) min >= (double) componentData4.m_Bounds.min.y)
            return;
          componentData4.m_Bounds.min.y = min;
          // ISSUE: reference to a compiler-generated field
          this.m_CullingInfoData[entity] = componentData4;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<BatchesUpdated>(index, entity, new BatchesUpdated());
        }
        else
        {
          Curve componentData5;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CurveData.TryGetComponent(entity, out componentData5))
          {
            Edge componentData6;
            // ISSUE: reference to a compiler-generated field
            if (this.m_EdgeData.TryGetComponent(entity, out componentData6))
            {
              // ISSUE: reference to a compiler-generated field
              bool flag3 = this.m_NetElevationData.HasComponent(componentData6.m_Start);
              // ISSUE: reference to a compiler-generated field
              bool flag4 = this.m_NetElevationData.HasComponent(componentData6.m_End);
              bool flag5 = false;
              bool flag6 = false;
              NetGeometryData componentData7;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabNetGeometryData.TryGetComponent(this.m_PrefabRefData[entity].m_Prefab, out componentData7))
                flag5 = (componentData7.m_Flags & Game.Net.GeometryFlags.FlattenTerrain) != 0;
              bool y1 = false;
              bool y2 = false;
              Entity lotOwner;
              // ISSUE: reference to a compiler-generated method
              if (this.GetLotOwner(entity, out lotOwner))
              {
                // ISSUE: reference to a compiler-generated method
                y1 = this.IsFixedNode(componentData6.m_Start, entity, lotOwner);
                // ISSUE: reference to a compiler-generated method
                y2 = this.IsFixedNode(componentData6.m_End, entity, lotOwner);
                flag3 |= y1;
                flag4 |= y2;
              }
              else
                flag6 = flag5;
              if (flag6)
                return;
              // ISSUE: reference to a compiler-generated field
              bool linearMiddle = flag3 | flag4 || this.m_NetElevationData.HasComponent(entity);
              BuildingUtils.LotInfo lotInfo;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              Curve curve = !flag5 ? NetUtils.AdjustPosition(componentData5, flag3, linearMiddle, flag4, ref this.m_TerrainHeightData) : (!this.GetOwnerLot(lotOwner, out lotInfo) ? componentData5 : NetUtils.AdjustPosition(componentData5, new bool2(flag3, y1), linearMiddle, new bool2(flag4, y2), ref lotInfo));
              Bezier4x1 y3 = curve.m_Bezier.y;
              float4 abcd1 = y3.abcd;
              y3 = componentData5.m_Bezier.y;
              float4 abcd2 = y3.abcd;
              bool4 x = math.abs(abcd1 - abcd2) >= 0.01f;
              if (!math.any(x))
                return;
              // ISSUE: reference to a compiler-generated field
              this.m_CurveData[entity] = curve;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(index, entity, new Updated());
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(index, componentData6.m_Start, new Updated());
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(index, componentData6.m_End, new Updated());
              if (x.x)
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<ConnectedEdge> connectedEdge = this.m_ConnectedEdges[componentData6.m_Start];
                for (int index1 = 0; index1 < connectedEdge.Length; ++index1)
                {
                  Entity edge1 = connectedEdge[index1].m_Edge;
                  if (edge1 != entity)
                  {
                    // ISSUE: reference to a compiler-generated field
                    Edge edge2 = this.m_EdgeData[edge1];
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<Updated>(index, edge1, new Updated());
                    if (edge2.m_Start != componentData6.m_Start)
                    {
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.AddComponent<Updated>(index, edge2.m_Start, new Updated());
                    }
                    if (edge2.m_End != componentData6.m_Start)
                    {
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.AddComponent<Updated>(index, edge2.m_End, new Updated());
                    }
                  }
                }
              }
              if (!x.w)
                return;
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<ConnectedEdge> connectedEdge1 = this.m_ConnectedEdges[componentData6.m_End];
              for (int index2 = 0; index2 < connectedEdge1.Length; ++index2)
              {
                Entity edge3 = connectedEdge1[index2].m_Edge;
                if (edge3 != entity)
                {
                  // ISSUE: reference to a compiler-generated field
                  Edge edge4 = this.m_EdgeData[edge3];
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.AddComponent<Updated>(index, edge3, new Updated());
                  if (edge4.m_Start != componentData6.m_End)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<Updated>(index, edge4.m_Start, new Updated());
                  }
                  if (edge4.m_End != componentData6.m_End)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.AddComponent<Updated>(index, edge4.m_End, new Updated());
                  }
                }
              }
            }
            else
            {
              bool2 x = (bool2) false;
              Game.Net.Elevation componentData8;
              // ISSUE: reference to a compiler-generated field
              if (this.m_NetElevationData.TryGetComponent(entity, out componentData8))
                x = componentData8.m_Elevation != float.MinValue;
              // ISSUE: reference to a compiler-generated field
              Curve curve = NetUtils.AdjustPosition(componentData5, x.x, math.any(x), x.y, ref this.m_TerrainHeightData);
              if (!math.any(math.abs(curve.m_Bezier.y.abcd - componentData5.m_Bezier.y.abcd) >= 0.01f))
                return;
              // ISSUE: reference to a compiler-generated field
              this.m_CurveData[entity] = curve;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(index, entity, new Updated());
            }
          }
          else
          {
            Game.Net.Node componentData9;
            // ISSUE: reference to a compiler-generated field
            if (this.m_NodeData.TryGetComponent(entity, out componentData9))
            {
              bool flag = false;
              NetGeometryData componentData10;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabNetGeometryData.TryGetComponent(this.m_PrefabRefData[entity].m_Prefab, out componentData10))
                flag = (componentData10.m_Flags & Game.Net.GeometryFlags.FlattenTerrain) != 0;
              Entity lotOwner;
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              if (!this.GetLotOwner(entity, out lotOwner) ? flag : this.IsFixedNode(entity, Entity.Null, lotOwner))
                return;
              BuildingUtils.LotInfo lotInfo;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              Game.Net.Node node = !flag ? NetUtils.AdjustPosition(componentData9, ref this.m_TerrainHeightData) : (!this.GetOwnerLot(lotOwner, out lotInfo) ? componentData9 : NetUtils.AdjustPosition(componentData9, ref lotInfo));
              if ((double) math.abs(node.m_Position.y - componentData9.m_Position.y) < 0.0099999997764825821)
                return;
              // ISSUE: reference to a compiler-generated field
              this.m_NodeData[entity] = node;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(index, entity, new Updated());
            }
            else
            {
              DynamicBuffer<Game.Areas.Node> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (!this.m_AreaNodes.TryGetBuffer(entity, out bufferData))
                return;
              bool flag7 = false;
              bool flag8 = false;
              AreaGeometryData componentData11;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabAreaGeometryData.TryGetComponent(this.m_PrefabRefData[entity].m_Prefab, out componentData11))
                flag8 = (componentData11.m_Flags & Game.Areas.GeometryFlags.OnWaterSurface) != 0;
              for (int index3 = 0; index3 < bufferData.Length; ++index3)
              {
                ref Game.Areas.Node local = ref bufferData.ElementAt(index3);
                if ((double) local.m_Elevation == -3.4028234663852886E+38)
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  Game.Areas.Node node = !flag8 ? AreaUtils.AdjustPosition(local, ref this.m_TerrainHeightData) : AreaUtils.AdjustPosition(local, ref this.m_TerrainHeightData, ref this.m_WaterSurfaceData);
                  bool c = (double) math.abs(node.m_Position.y - local.m_Position.y) >= 0.0099999997764825821;
                  local.m_Position = math.select(local.m_Position, node.m_Position, c);
                  flag7 |= c;
                }
              }
              if (!flag7)
                return;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(index, entity, new Updated());
            }
          }
        }
      }

      private bool GetLotOwner(Entity entity, out Entity lotOwner)
      {
        Entity entity1 = entity;
        // ISSUE: reference to a compiler-generated field
        while (this.m_OwnerData.HasComponent(entity1))
        {
          // ISSUE: reference to a compiler-generated field
          entity1 = this.m_OwnerData[entity1].m_Owner;
          PrefabRef componentData1;
          BuildingExtensionData componentData2;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_BuildingLotData.HasComponent(entity1) && (!this.m_PrefabRefData.TryGetComponent(entity1, out componentData1) || !this.m_PrefabBuildingExtensionData.TryGetComponent(componentData1.m_Prefab, out componentData2) || componentData2.m_External))
          {
            lotOwner = entity1;
            return true;
          }
        }
        lotOwner = Entity.Null;
        return false;
      }

      private bool GetOwnerLot(Entity lotOwner, out BuildingUtils.LotInfo lotInfo)
      {
        // ISSUE: reference to a compiler-generated field
        Game.Buildings.Lot lot = this.m_BuildingLotData[lotOwner];
        Transform componentData1;
        PrefabRef componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_TransformData.TryGetComponent(lotOwner, out componentData1) && this.m_PrefabRefData.TryGetComponent(lotOwner, out componentData2))
        {
          Game.Prefabs.BuildingData componentData3;
          bool hasExtensionLots;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabBuildingData.TryGetComponent(componentData2.m_Prefab, out componentData3))
          {
            float2 extents = new float2(componentData3.m_LotSize) * 4f;
            Game.Objects.Elevation componentData4;
            // ISSUE: reference to a compiler-generated field
            this.m_ObjectElevationData.TryGetComponent(lotOwner, out componentData4);
            DynamicBuffer<InstalledUpgrade> bufferData;
            // ISSUE: reference to a compiler-generated field
            this.m_InstalledUpgrades.TryGetBuffer(lotOwner, out bufferData);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            lotInfo = BuildingUtils.CalculateLotInfo(extents, componentData1, componentData4, lot, componentData2, bufferData, this.m_TransformData, this.m_PrefabRefData, this.m_PrefabObjectGeometryData, this.m_PrefabBuildingTerraformData, this.m_PrefabBuildingExtensionData, false, out hasExtensionLots);
            return true;
          }
          BuildingExtensionData componentData5;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PrefabBuildingExtensionData.TryGetComponent(componentData2.m_Prefab, out componentData5))
          {
            float2 extents = new float2(componentData5.m_LotSize) * 4f;
            Game.Objects.Elevation componentData6;
            // ISSUE: reference to a compiler-generated field
            this.m_ObjectElevationData.TryGetComponent(lotOwner, out componentData6);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            lotInfo = BuildingUtils.CalculateLotInfo(extents, componentData1, componentData6, lot, componentData2, new DynamicBuffer<InstalledUpgrade>(), this.m_TransformData, this.m_PrefabRefData, this.m_PrefabObjectGeometryData, this.m_PrefabBuildingTerraformData, this.m_PrefabBuildingExtensionData, false, out hasExtensionLots);
            return true;
          }
        }
        lotInfo = new BuildingUtils.LotInfo();
        return false;
      }

      private bool IsFixedNode(Entity node, Entity ignoreEdge, Entity lotOwner)
      {
        DynamicBuffer<ConnectedEdge> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ConnectedEdges.TryGetBuffer(node, out bufferData))
        {
          for (int index = 0; index < bufferData.Length; ++index)
          {
            Entity edge1 = bufferData[index].m_Edge;
            if (!(edge1 == ignoreEdge))
            {
              // ISSUE: reference to a compiler-generated field
              Edge edge2 = this.m_EdgeData[edge1];
              NetGeometryData componentData;
              Entity lotOwner1;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              if ((!(edge2.m_Start != node) || !(edge2.m_End != node)) && this.m_PrefabNetGeometryData.TryGetComponent(this.m_PrefabRefData[edge1].m_Prefab, out componentData) && (componentData.m_Flags & Game.Net.GeometryFlags.FlattenTerrain) != (Game.Net.GeometryFlags) 0 && (!this.GetLotOwner(edge1, out lotOwner1) || lotOwner1 != lotOwner))
                return true;
            }
          }
        }
        return false;
      }

      private void HandleSubObjects(
        int jobIndex,
        DynamicBuffer<Game.Objects.SubObject> subObjects,
        Transform transform,
        Transform adjusted)
      {
        if (subObjects.Length == 0)
          return;
        quaternion quaternion = math.mul(adjusted.m_Rotation, math.inverse(transform.m_Rotation));
        for (int index = 0; index < subObjects.Length; ++index)
        {
          Entity subObject = subObjects[index].m_SubObject;
          Game.Objects.Elevation componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ObjectElevationData.TryGetComponent(subObject, out componentData) && (componentData.m_Flags & ElevationFlags.OnGround) == (ElevationFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            Transform transform1 = this.m_TransformData[subObject];
            Transform adjusted1;
            adjusted1.m_Position = adjusted.m_Position + math.mul(quaternion, transform1.m_Position - transform.m_Position);
            adjusted1.m_Rotation = math.normalize(math.mul(quaternion, transform1.m_Rotation));
            // ISSUE: reference to a compiler-generated field
            this.m_TransformData[subObject] = adjusted1;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Updated>(jobIndex, subObject, new Updated());
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_SpawnLocationData.HasComponent(subObject) && !this.m_MovedLocationData.HasComponent(subObject))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<MovedLocation>(jobIndex, subObject, new MovedLocation()
              {
                m_OldPosition = transform.m_Position
              });
            }
            DynamicBuffer<Game.Objects.SubObject> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_SubObjects.TryGetBuffer(subObject, out bufferData))
            {
              // ISSUE: reference to a compiler-generated method
              this.HandleSubObjects(jobIndex, bufferData, transform1, adjusted1);
            }
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> __Game_Objects_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.Lot> __Game_Buildings_Lot_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Net.Elevation> __Game_Net_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetGeometryData> __Game_Prefabs_NetGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Triangle> __Game_Areas_Triangle_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<MovedLocation> __Game_Objects_MovedLocation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> __Game_Prefabs_PlaceableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingExtensionData> __Game_Prefabs_BuildingExtensionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingTerraformData> __Game_Prefabs_BuildingTerraformData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AreaGeometryData> __Game_Prefabs_AreaGeometryData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferLookup;
      public ComponentLookup<Transform> __Game_Objects_Transform_RW_ComponentLookup;
      public ComponentLookup<Curve> __Game_Net_Curve_RW_ComponentLookup;
      public ComponentLookup<Game.Net.Node> __Game_Net_Node_RW_ComponentLookup;
      public ComponentLookup<CullingInfo> __Game_Rendering_CullingInfo_RW_ComponentLookup;
      public BufferLookup<Game.Areas.Node> __Game_Areas_Node_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Lot_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.Lot>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetGeometryData_RO_ComponentLookup = state.GetComponentLookup<NetGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RO_BufferLookup = state.GetBufferLookup<Game.Areas.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Triangle_RO_BufferLookup = state.GetBufferLookup<Triangle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_MovedLocation_RO_ComponentLookup = state.GetComponentLookup<MovedLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup = state.GetComponentLookup<PlaceableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingExtensionData_RO_ComponentLookup = state.GetComponentLookup<BuildingExtensionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingTerraformData_RO_ComponentLookup = state.GetComponentLookup<BuildingTerraformData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_AreaGeometryData_RO_ComponentLookup = state.GetComponentLookup<AreaGeometryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferLookup = state.GetBufferLookup<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RW_ComponentLookup = state.GetComponentLookup<Transform>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RW_ComponentLookup = state.GetComponentLookup<Curve>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RW_ComponentLookup = state.GetComponentLookup<Game.Net.Node>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_CullingInfo_RW_ComponentLookup = state.GetComponentLookup<CullingInfo>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Node_RW_BufferLookup = state.GetBufferLookup<Game.Areas.Node>();
      }
    }
  }
}
