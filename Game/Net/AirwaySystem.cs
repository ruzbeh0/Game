// Decompiled with JetBrains decompiler
// Type: Game.Net.AirwaySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Common;
using Game.Pathfind;
using Game.Prefabs;
using Game.Routes;
using Game.Serialization;
using Game.Simulation;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  [CompilerGenerated]
  public class AirwaySystem : GameSystemBase, IJobSerializable
  {
    private const float TERRAIN_SIZE = 14336f;
    private const int HELICOPTER_GRID_WIDTH = 28;
    private const int HELICOPTER_GRID_LENGTH = 28;
    private const float HELICOPTER_CELL_SIZE = 494.344818f;
    private const float HELICOPTER_PATH_HEIGHT = 200f;
    private const int AIRPLANE_GRID_WIDTH = 14;
    private const int AIRPLANE_GRID_LENGTH = 14;
    private const float AIRPLANE_CELL_SIZE = 988.689636f;
    private const float AIRPLANE_PATH_HEIGHT = 1000f;
    private LoadGameSystem m_LoadGameSystem;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private EntityQuery m_PrefabQuery;
    private EntityQuery m_AirplaneConnectionQuery;
    private EntityQuery m_OldConnectionQuery;
    private AirwayHelpers.AirwayData m_AirwayData;
    private AirwaySystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_LoadGameSystem = this.World.GetOrCreateSystemManaged<LoadGameSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<ConnectionLaneData>(), ComponentType.ReadOnly<PrefabData>());
      // ISSUE: reference to a compiler-generated field
      this.m_AirplaneConnectionQuery = this.GetEntityQuery(ComponentType.ReadOnly<AirplaneStop>(), ComponentType.ReadOnly<Game.Routes.TakeoffLocation>(), ComponentType.ReadOnly<Game.Objects.OutsideConnection>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_OldConnectionQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[1]
        {
          ComponentType.ReadOnly<ConnectionLane>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<OutsideConnection>(),
          ComponentType.ReadOnly<Owner>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PrefabQuery);
      // ISSUE: reference to a compiler-generated field
      this.m_AirwayData = new AirwayHelpers.AirwayData(new AirwayHelpers.AirwayMap(new int2(28, 28), 494.344818f, 200f, Allocator.Persistent), new AirwayHelpers.AirwayMap(new int2(14, 14), 988.689636f, 1000f, Allocator.Persistent));
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_AirwayData.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_LoadGameSystem.context.purpose != Colossal.Serialization.Entities.Purpose.NewGame || !this.m_OldConnectionQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      NativeArray<Entity> entityArray = this.m_PrefabQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      NetLaneArchetypeData componentData = this.EntityManager.GetComponentData<NetLaneArchetypeData>(entityArray[0]);
      EntityManager entityManager;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_AirplaneConnectionQuery.IsEmptyIgnoreFilter)
      {
        entityManager = this.EntityManager;
        // ISSUE: reference to a compiler-generated field
        entityManager.AddComponent<Updated>(this.m_AirplaneConnectionQuery);
      }
      entityManager = this.EntityManager;
      // ISSUE: reference to a compiler-generated field
      entityManager.CreateEntity(componentData.m_LaneArchetype, this.m_AirwayData.helicopterMap.entities);
      entityManager = this.EntityManager;
      // ISSUE: reference to a compiler-generated field
      entityManager.CreateEntity(componentData.m_LaneArchetype, this.m_AirwayData.airplaneMap.entities);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      TerrainHeightData heightData = this.m_TerrainSystem.GetHeightData(true);
      JobHandle deps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      WaterSurfaceData surfaceData = this.m_WaterSystem.GetSurfaceData(out deps);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectionLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AirwaySystem.GenerateAirwayLanesJob generateAirwayLanesJob = new AirwaySystem.GenerateAirwayLanesJob();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      generateAirwayLanesJob.m_AirwayMap = this.m_AirwayData.helicopterMap;
      // ISSUE: reference to a compiler-generated field
      generateAirwayLanesJob.m_Prefab = entityArray[0];
      // ISSUE: reference to a compiler-generated field
      generateAirwayLanesJob.m_RoadType = RoadTypes.Helicopter;
      // ISSUE: reference to a compiler-generated field
      generateAirwayLanesJob.m_TerrainHeightData = heightData;
      // ISSUE: reference to a compiler-generated field
      generateAirwayLanesJob.m_WaterSurfaceData = surfaceData;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      generateAirwayLanesJob.m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RW_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      generateAirwayLanesJob.m_LaneData = this.__TypeHandle.__Game_Net_Lane_RW_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      generateAirwayLanesJob.m_CurveData = this.__TypeHandle.__Game_Net_Curve_RW_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      generateAirwayLanesJob.m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RW_ComponentLookup;
      // ISSUE: variable of a compiler-generated type
      AirwaySystem.GenerateAirwayLanesJob jobData1 = generateAirwayLanesJob;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectionLane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Lane_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: object of a compiler-generated type is created
      generateAirwayLanesJob = new AirwaySystem.GenerateAirwayLanesJob();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      generateAirwayLanesJob.m_AirwayMap = this.m_AirwayData.airplaneMap;
      // ISSUE: reference to a compiler-generated field
      generateAirwayLanesJob.m_Prefab = entityArray[0];
      // ISSUE: reference to a compiler-generated field
      generateAirwayLanesJob.m_RoadType = RoadTypes.Airplane;
      // ISSUE: reference to a compiler-generated field
      generateAirwayLanesJob.m_TerrainHeightData = heightData;
      // ISSUE: reference to a compiler-generated field
      generateAirwayLanesJob.m_WaterSurfaceData = surfaceData;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      generateAirwayLanesJob.m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RW_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      generateAirwayLanesJob.m_LaneData = this.__TypeHandle.__Game_Net_Lane_RW_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      generateAirwayLanesJob.m_CurveData = this.__TypeHandle.__Game_Net_Curve_RW_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      generateAirwayLanesJob.m_ConnectionLaneData = this.__TypeHandle.__Game_Net_ConnectionLane_RW_ComponentLookup;
      // ISSUE: variable of a compiler-generated type
      AirwaySystem.GenerateAirwayLanesJob jobData2 = generateAirwayLanesJob;
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = jobData1.Schedule<AirwaySystem.GenerateAirwayLanesJob>(this.m_AirwayData.helicopterMap.entities.Length, 4, JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      int length = this.m_AirwayData.airplaneMap.entities.Length;
      JobHandle dependsOn = jobHandle;
      JobHandle handle = jobData2.Schedule<AirwaySystem.GenerateAirwayLanesJob>(length, 4, dependsOn);
      entityArray.Dispose();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(handle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(handle);
      this.Dependency = handle;
    }

    public AirwayHelpers.AirwayData GetAirwayData() => this.m_AirwayData;

    public JobHandle Serialize<TWriter>(EntityWriterData writerData, JobHandle inputDeps) where TWriter : struct, IWriter
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      return new AirwaySystem.SerializeJob<TWriter>()
      {
        m_HelicopterMap = this.m_AirwayData.helicopterMap,
        m_AirplaneMap = this.m_AirwayData.airplaneMap,
        m_WriterData = writerData
      }.Schedule<AirwaySystem.SerializeJob<TWriter>>(inputDeps);
    }

    public JobHandle Deserialize<TReader>(EntityReaderData readerData, JobHandle inputDeps) where TReader : struct, IReader
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      return new AirwaySystem.DeserializeJob<TReader>()
      {
        m_HelicopterMap = this.m_AirwayData.helicopterMap,
        m_AirplaneMap = this.m_AirwayData.airplaneMap,
        m_ReaderData = readerData
      }.Schedule<AirwaySystem.DeserializeJob<TReader>>(inputDeps);
    }

    public JobHandle SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      return new AirwaySystem.SetDefaultsJob()
      {
        m_Context = context,
        m_HelicopterMap = this.m_AirwayData.helicopterMap,
        m_AirplaneMap = this.m_AirwayData.airplaneMap
      }.Schedule<AirwaySystem.SetDefaultsJob>();
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
    public AirwaySystem()
    {
    }

    [BurstCompile]
    internal struct SerializeJob<TWriter> : IJob where TWriter : struct, IWriter
    {
      [ReadOnly]
      public AirwayHelpers.AirwayMap m_HelicopterMap;
      [ReadOnly]
      public AirwayHelpers.AirwayMap m_AirplaneMap;
      public EntityWriterData m_WriterData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        TWriter writer = this.m_WriterData.GetWriter<TWriter>();
        // ISSUE: reference to a compiler-generated field
        this.m_HelicopterMap.Serialize<TWriter>(writer);
        // ISSUE: reference to a compiler-generated field
        this.m_AirplaneMap.Serialize<TWriter>(writer);
      }
    }

    [BurstCompile]
    internal struct DeserializeJob<TReader> : IJob where TReader : struct, IReader
    {
      public AirwayHelpers.AirwayMap m_HelicopterMap;
      public AirwayHelpers.AirwayMap m_AirplaneMap;
      public EntityReaderData m_ReaderData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        TReader reader = this.m_ReaderData.GetReader<TReader>();
        // ISSUE: reference to a compiler-generated field
        this.m_HelicopterMap.Deserialize<TReader>(reader);
        if (reader.context.version >= Version.airplaneAirways)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_AirplaneMap.Deserialize<TReader>(reader);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_AirplaneMap.SetDefaults(reader.context);
        }
      }
    }

    [BurstCompile]
    private struct SetDefaultsJob : IJob
    {
      [ReadOnly]
      public Colossal.Serialization.Entities.Context m_Context;
      public AirwayHelpers.AirwayMap m_HelicopterMap;
      public AirwayHelpers.AirwayMap m_AirplaneMap;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_HelicopterMap.SetDefaults(this.m_Context);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_AirplaneMap.SetDefaults(this.m_Context);
      }
    }

    [BurstCompile]
    private struct GenerateAirwayLanesJob : IJobParallelFor
    {
      [ReadOnly]
      public AirwayHelpers.AirwayMap m_AirwayMap;
      [ReadOnly]
      public Entity m_Prefab;
      [ReadOnly]
      public RoadTypes m_RoadType;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Lane> m_LaneData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Curve> m_CurveData;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<ConnectionLane> m_ConnectionLaneData;

      public void Execute(int entityIndex)
      {
        AirwayHelpers.LaneDirection direction;
        // ISSUE: reference to a compiler-generated field
        int2 cellIndex = this.m_AirwayMap.GetCellIndex(entityIndex, out direction);
        switch (direction)
        {
          case AirwayHelpers.LaneDirection.HorizontalZ:
            // ISSUE: reference to a compiler-generated method
            this.CreateLane(entityIndex, cellIndex, new int2(cellIndex.x, cellIndex.y + 1));
            break;
          case AirwayHelpers.LaneDirection.HorizontalX:
            // ISSUE: reference to a compiler-generated method
            this.CreateLane(entityIndex, cellIndex, new int2(cellIndex.x + 1, cellIndex.y));
            break;
          case AirwayHelpers.LaneDirection.Diagonal:
            // ISSUE: reference to a compiler-generated method
            this.CreateLane(entityIndex, cellIndex, cellIndex + 1);
            break;
          case AirwayHelpers.LaneDirection.DiagonalCross:
            // ISSUE: reference to a compiler-generated method
            this.CreateLane(entityIndex, new int2(cellIndex.x + 1, cellIndex.y), new int2(cellIndex.x, cellIndex.y + 1));
            break;
        }
      }

      private void CreateLane(int entityIndex, int2 startNode, int2 endNode)
      {
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_AirwayMap.entities[entityIndex];
        Lane lane;
        // ISSUE: reference to a compiler-generated field
        lane.m_StartNode = this.m_AirwayMap.GetPathNode(startNode);
        lane.m_MiddleNode = new PathNode(entity, (ushort) 1);
        // ISSUE: reference to a compiler-generated field
        lane.m_EndNode = this.m_AirwayMap.GetPathNode(endNode);
        // ISSUE: reference to a compiler-generated field
        float3 nodePosition1 = this.m_AirwayMap.GetNodePosition(startNode);
        // ISSUE: reference to a compiler-generated field
        float3 nodePosition2 = this.m_AirwayMap.GetNodePosition(endNode);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        nodePosition1.y += WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, nodePosition1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        nodePosition2.y += WaterUtils.SampleHeight(ref this.m_WaterSurfaceData, ref this.m_TerrainHeightData, nodePosition2);
        Curve curve;
        curve.m_Bezier = NetUtils.StraightCurve(nodePosition1, nodePosition2);
        curve.m_Length = math.distance(curve.m_Bezier.a, curve.m_Bezier.d);
        ConnectionLane connectionLane;
        connectionLane.m_AccessRestriction = Entity.Null;
        connectionLane.m_Flags = ConnectionLaneFlags.AllowMiddle | ConnectionLaneFlags.Airway;
        connectionLane.m_TrackTypes = TrackTypes.None;
        // ISSUE: reference to a compiler-generated field
        connectionLane.m_RoadTypes = this.m_RoadType;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabRefData[entity] = new PrefabRef(this.m_Prefab);
        // ISSUE: reference to a compiler-generated field
        this.m_LaneData[entity] = lane;
        // ISSUE: reference to a compiler-generated field
        this.m_CurveData[entity] = curve;
        // ISSUE: reference to a compiler-generated field
        this.m_ConnectionLaneData[entity] = connectionLane;
      }
    }

    private struct TypeHandle
    {
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RW_ComponentLookup;
      public ComponentLookup<Lane> __Game_Net_Lane_RW_ComponentLookup;
      public ComponentLookup<Curve> __Game_Net_Curve_RW_ComponentLookup;
      public ComponentLookup<ConnectionLane> __Game_Net_ConnectionLane_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RW_ComponentLookup = state.GetComponentLookup<PrefabRef>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Lane_RW_ComponentLookup = state.GetComponentLookup<Lane>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RW_ComponentLookup = state.GetComponentLookup<Curve>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectionLane_RW_ComponentLookup = state.GetComponentLookup<ConnectionLane>();
      }
    }
  }
}
