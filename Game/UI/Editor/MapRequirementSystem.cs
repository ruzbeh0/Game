// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.MapRequirementSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Areas;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Routes;
using Game.Simulation;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.UI.Editor
{
  [CompilerGenerated]
  public class MapRequirementSystem : GameSystemBase
  {
    private MapTileSystem m_MapTileSystem;
    private WaterSystem m_WaterSystem;
    private EntityQuery m_TileQuery;
    private EntityQuery m_OutsideRoadNodeQuery;
    private EntityQuery m_OutsideTrainNodeQuery;
    private EntityQuery m_OutsideAirNodeQuery;
    private EntityQuery m_OutsideElectricityConnectionQuery;
    private JobHandle m_ResultDependency;
    private NativeValue<bool> m_WaterResult;
    private NativeArray<bool> m_StartingAreaResources;
    private NativeArray<bool> m_MapResources;
    private MapRequirementSystem.TypeHandle __TypeHandle;

    public bool hasStartingArea { get; private set; }

    public bool roadConnection { get; private set; }

    public bool trainConnection { get; private set; }

    public bool airConnection { get; private set; }

    public bool electricityConnection { get; private set; }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_MapTileSystem = this.World.GetOrCreateSystemManaged<MapTileSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TileQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<MapTile>(),
          ComponentType.ReadOnly<Geometry>(),
          ComponentType.ReadOnly<MapFeatureElement>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_OutsideRoadNodeQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Net.Node>(),
          ComponentType.ReadOnly<Road>(),
          ComponentType.ReadOnly<Game.Net.OutsideConnection>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_OutsideTrainNodeQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Net.Node>(),
          ComponentType.ReadOnly<TrainTrack>(),
          ComponentType.ReadOnly<Game.Net.OutsideConnection>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_OutsideAirNodeQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<AirplaneStop>(),
          ComponentType.ReadOnly<Game.Objects.OutsideConnection>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_OutsideElectricityConnectionQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<ElectricityOutsideConnection>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_WaterResult = new NativeValue<bool>(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_StartingAreaResources = new NativeArray<bool>(8, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_MapResources = new NativeArray<bool>(8, Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ResultDependency.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeArray<Entity> array = this.m_MapTileSystem.GetStartTiles().ToArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      this.hasStartingArea = array.Length != 0;
      // ISSUE: reference to a compiler-generated field
      this.roadConnection = !this.m_OutsideRoadNodeQuery.IsEmptyIgnoreFilter;
      // ISSUE: reference to a compiler-generated field
      this.trainConnection = !this.m_OutsideTrainNodeQuery.IsEmptyIgnoreFilter;
      // ISSUE: reference to a compiler-generated field
      this.airConnection = !this.m_OutsideAirNodeQuery.IsEmptyIgnoreFilter;
      // ISSUE: reference to a compiler-generated field
      this.electricityConnection = !this.m_OutsideElectricityConnectionQuery.IsEmptyIgnoreFilter;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle deps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      MapRequirementSystem.CheckWaterJob jobData1 = new MapRequirementSystem.CheckWaterJob()
      {
        m_Result = this.m_WaterResult,
        m_SurfaceData = this.m_WaterSystem.GetSurfaceData(out deps),
        m_StartingTiles = array,
        m_GeometryData = this.__TypeHandle.__Game_Areas_Geometry_RO_ComponentLookup
      };
      this.Dependency = jobData1.Schedule<MapRequirementSystem.CheckWaterJob>(JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_MapFeatureElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      MapRequirementSystem.CollectStartingResourcesJob jobData2 = new MapRequirementSystem.CollectStartingResourcesJob()
      {
        m_StartingTiles = array,
        m_MapFeatureElements = this.__TypeHandle.__Game_Areas_MapFeatureElement_RO_BufferLookup,
        m_Results = this.m_StartingAreaResources
      };
      this.Dependency = jobData2.Schedule<MapRequirementSystem.CollectStartingResourcesJob>(this.Dependency);
      array.Dispose(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_MapFeatureElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      MapRequirementSystem.CollectResourcesJob jobData3 = new MapRequirementSystem.CollectResourcesJob()
      {
        m_AreaChunks = this.m_TileQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob),
        m_MapFeatureElementType = this.__TypeHandle.__Game_Areas_MapFeatureElement_RO_BufferTypeHandle,
        m_Results = this.m_MapResources
      };
      this.Dependency = jobData3.Schedule<MapRequirementSystem.CollectResourcesJob>(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_ResultDependency = this.Dependency;
    }

    public bool StartingAreaHasResource(MapFeature feature)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ResultDependency.Complete();
      switch (feature)
      {
        case MapFeature.Area:
        case MapFeature.BuildableLand:
        case MapFeature.FertileLand:
        case MapFeature.Forest:
        case MapFeature.Oil:
        case MapFeature.Ore:
        case MapFeature.GroundWater:
          // ISSUE: reference to a compiler-generated field
          return this.m_StartingAreaResources[(int) feature];
        case MapFeature.SurfaceWater:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          return this.m_StartingAreaResources[(int) feature] || this.m_WaterResult.value;
        default:
          return false;
      }
    }

    public bool MapHasResource(MapFeature feature)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ResultDependency.Complete();
      // ISSUE: reference to a compiler-generated field
      return feature > MapFeature.None && feature < MapFeature.Count && this.m_MapResources[(int) feature];
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterResult.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_StartingAreaResources.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_MapResources.Dispose();
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
    public MapRequirementSystem()
    {
    }

    [BurstCompile]
    public struct CollectResourcesJob : IJob
    {
      [ReadOnly]
      [DeallocateOnJobCompletion]
      public NativeArray<ArchetypeChunk> m_AreaChunks;
      [ReadOnly]
      public BufferTypeHandle<MapFeatureElement> m_MapFeatureElementType;
      public NativeArray<bool> m_Results;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Results.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Results[index] = false;
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_AreaChunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.Check(this.m_AreaChunks[index].GetBufferAccessor<MapFeatureElement>(ref this.m_MapFeatureElementType));
        }
      }

      private void Check(
        BufferAccessor<MapFeatureElement> mapFeatureAccessor)
      {
        for (int index1 = 0; index1 < mapFeatureAccessor.Length; ++index1)
        {
          DynamicBuffer<MapFeatureElement> dynamicBuffer = mapFeatureAccessor[index1];
          for (int index2 = 0; index2 < 8; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Results[index2] |= (double) dynamicBuffer[index2].m_Amount > 0.0;
          }
        }
      }
    }

    [BurstCompile]
    public struct CollectStartingResourcesJob : IJob
    {
      [ReadOnly]
      public NativeArray<Entity> m_StartingTiles;
      [ReadOnly]
      public BufferLookup<MapFeatureElement> m_MapFeatureElements;
      public NativeArray<bool> m_Results;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Results.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Results[index] = false;
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_StartingTiles.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.Check(this.m_StartingTiles[index]);
        }
      }

      private void Check(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<MapFeatureElement> mapFeatureElement = this.m_MapFeatureElements[entity];
        for (int index = 0; index < 8; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Results[index] |= (double) mapFeatureElement[index].m_Amount > 0.0;
        }
      }
    }

    [BurstCompile]
    public struct CheckWaterJob : IJob
    {
      [ReadOnly]
      public WaterSurfaceData m_SurfaceData;
      [ReadOnly]
      public NativeArray<Entity> m_StartingTiles;
      [ReadOnly]
      public ComponentLookup<Geometry> m_GeometryData;
      public NativeValue<bool> m_Result;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Result.value = false;
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_StartingTiles.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if (this.HasWater(this.m_GeometryData[this.m_StartingTiles[index]].m_Bounds))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Result.value = true;
            break;
          }
        }
      }

      private bool HasWater(Bounds3 bounds)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int2 int2_1 = math.clamp((int2) math.floor(WaterUtils.ToSurfaceSpace(ref this.m_SurfaceData, bounds.min).xz), int2.zero, this.m_SurfaceData.resolution.xz);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int2 int2_2 = math.clamp((int2) math.floor(WaterUtils.ToSurfaceSpace(ref this.m_SurfaceData, bounds.max).xz), int2.zero, this.m_SurfaceData.resolution.xz);
        for (int y = int2_1.y; y < int2_2.y; ++y)
        {
          for (int x = int2_1.x; x < int2_2.x; ++x)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if ((double) this.m_SurfaceData.depths[y * this.m_SurfaceData.resolution.x + x].m_Depth > 0.0)
              return true;
          }
        }
        return false;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<Geometry> __Game_Areas_Geometry_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<MapFeatureElement> __Game_Areas_MapFeatureElement_RO_BufferLookup;
      [ReadOnly]
      public BufferTypeHandle<MapFeatureElement> __Game_Areas_MapFeatureElement_RO_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Geometry_RO_ComponentLookup = state.GetComponentLookup<Geometry>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_MapFeatureElement_RO_BufferLookup = state.GetBufferLookup<MapFeatureElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_MapFeatureElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<MapFeatureElement>(true);
      }
    }
  }
}
