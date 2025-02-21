// Decompiled with JetBrains decompiler
// Type: Game.Simulation.LandValueSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Assertions;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class LandValueSystem : CellMapSystem<LandValueCell>, IJobSerializable
  {
    public static readonly int kTextureSize = 128;
    public static readonly int kUpdatesPerDay = 32;
    private EntityQuery m_EdgeGroup;
    private EntityQuery m_NodeGroup;
    private EntityQuery m_AttractivenessParameterQuery;
    private EntityQuery m_LandValueParameterQuery;
    private GroundPollutionSystem m_GroundPollutionSystem;
    private AirPollutionSystem m_AirPollutionSystem;
    private NoisePollutionSystem m_NoisePollutionSystem;
    private AvailabilityInfoToGridSystem m_AvailabilityInfoToGridSystem;
    private SearchSystem m_NetSearchSystem;
    private TerrainAttractivenessSystem m_TerrainAttractivenessSystem;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private TelecomCoverageSystem m_TelecomCoverageSystem;
    private LandValueSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / LandValueSystem.kUpdatesPerDay;
    }

    public int2 TextureSize => new int2(LandValueSystem.kTextureSize, LandValueSystem.kTextureSize);

    public static float3 GetCellCenter(int index)
    {
      // ISSUE: reference to a compiler-generated field
      return CellMapSystem<LandValueCell>.GetCellCenter(index, LandValueSystem.kTextureSize);
    }

    public static int GetCellIndex(float3 pos)
    {
      // ISSUE: reference to a compiler-generated field
      int num = CellMapSystem<LandValueCell>.kMapSize / LandValueSystem.kTextureSize;
      // ISSUE: reference to a compiler-generated field
      return Mathf.FloorToInt(((float) (CellMapSystem<LandValueCell>.kMapSize / 2) + pos.x) / (float) num) + Mathf.FloorToInt(((float) (CellMapSystem<LandValueCell>.kMapSize / 2) + pos.z) / (float) num) * LandValueSystem.kTextureSize;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Assert.IsTrue(LandValueSystem.kTextureSize == TerrainAttractivenessSystem.kTextureSize);
      // ISSUE: reference to a compiler-generated field
      this.CreateTextures(LandValueSystem.kTextureSize);
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<SearchSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_GroundPollutionSystem = this.World.GetOrCreateSystemManaged<GroundPollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AirPollutionSystem = this.World.GetOrCreateSystemManaged<AirPollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NoisePollutionSystem = this.World.GetOrCreateSystemManaged<NoisePollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainAttractivenessSystem = this.World.GetOrCreateSystemManaged<TerrainAttractivenessSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AvailabilityInfoToGridSystem = this.World.GetOrCreateSystemManaged<AvailabilityInfoToGridSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TelecomCoverageSystem = this.World.GetOrCreateSystemManaged<TelecomCoverageSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AttractivenessParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<AttractivenessParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_LandValueParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<LandValueParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_EdgeGroup = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Net.Edge>(),
          ComponentType.ReadWrite<LandValue>(),
          ComponentType.ReadOnly<Curve>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireAnyForUpdate(this.m_EdgeGroup);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_EdgeGroup.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_LandValue_RW_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ResourceAvailability_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_ServiceCoverage_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        LandValueSystem.EdgeUpdateJob jobData = new LandValueSystem.EdgeUpdateJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_EdgeType = this.__TypeHandle.__Game_Net_Edge_RO_ComponentTypeHandle,
          m_ServiceCoverageType = this.__TypeHandle.__Game_Net_ServiceCoverage_RO_BufferTypeHandle,
          m_AvailabilityType = this.__TypeHandle.__Game_Net_ResourceAvailability_RO_BufferTypeHandle,
          m_LandValues = this.__TypeHandle.__Game_Net_LandValue_RW_ComponentLookup,
          m_LandValueParameterData = this.m_LandValueParameterQuery.GetSingleton<LandValueParameterData>()
        };
        // ISSUE: reference to a compiler-generated field
        this.Dependency = jobData.ScheduleParallel<LandValueSystem.EdgeUpdateJob>(this.m_EdgeGroup, this.Dependency);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LandValue_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies1;
      JobHandle dependencies2;
      JobHandle dependencies3;
      JobHandle dependencies4;
      JobHandle dependencies5;
      JobHandle dependencies6;
      JobHandle dependencies7;
      JobHandle deps;
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      LandValueSystem.LandValueMapUpdateJob jobData1 = new LandValueSystem.LandValueMapUpdateJob()
      {
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies1),
        m_AttractiveMap = this.m_TerrainAttractivenessSystem.GetMap(true, out dependencies2),
        m_GroundPollutionMap = this.m_GroundPollutionSystem.GetMap(true, out dependencies3),
        m_AirPollutionMap = this.m_AirPollutionSystem.GetMap(true, out dependencies4),
        m_NoisePollutionMap = this.m_NoisePollutionSystem.GetMap(true, out dependencies5),
        m_AvailabilityInfoMap = this.m_AvailabilityInfoToGridSystem.GetMap(true, out dependencies6),
        m_TelecomCoverageMap = this.m_TelecomCoverageSystem.GetData(true, out dependencies7),
        m_LandValueMap = this.m_Map,
        m_LandValueData = this.__TypeHandle.__Game_Net_LandValue_RO_ComponentLookup,
        m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
        m_WaterSurfaceData = this.m_WaterSystem.GetSurfaceData(out deps),
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_AttractivenessParameterData = this.m_AttractivenessParameterQuery.GetSingleton<AttractivenessParameterData>(),
        m_LandValueParameterData = this.m_LandValueParameterQuery.GetSingleton<LandValueParameterData>(),
        m_CellSize = (float) CellMapSystem<LandValueCell>.kMapSize / (float) LandValueSystem.kTextureSize
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.Schedule<LandValueSystem.LandValueMapUpdateJob>(LandValueSystem.kTextureSize * LandValueSystem.kTextureSize, LandValueSystem.kTextureSize, JobHandle.CombineDependencies(dependencies1, dependencies2, JobHandle.CombineDependencies(this.m_WriteDependencies, this.m_ReadDependencies, JobHandle.CombineDependencies(this.Dependency, deps, JobHandle.CombineDependencies(dependencies3, dependencies5, JobHandle.CombineDependencies(dependencies6, dependencies4, dependencies7))))));
      this.AddWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainAttractivenessSystem.AddReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_GroundPollutionSystem.AddReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_AirPollutionSystem.AddReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_NoisePollutionSystem.AddReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_AvailabilityInfoToGridSystem.AddReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_TelecomCoverageSystem.AddReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(this.Dependency);
      this.Dependency = JobHandle.CombineDependencies(this.m_ReadDependencies, this.m_WriteDependencies, this.Dependency);
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
    public LandValueSystem()
    {
    }

    private struct NetIterator : 
      INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
    {
      public int m_TotalCount;
      public float m_TotalLandValueBonus;
      public Bounds3 m_Bounds;
      public ComponentLookup<LandValue> m_LandValueData;
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;

      public bool Intersect(QuadTreeBoundsXZ bounds)
      {
        // ISSUE: reference to a compiler-generated field
        return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
      }

      public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds) || !this.m_LandValueData.HasComponent(entity) || !this.m_EdgeGeometryData.HasComponent(entity))
          return;
        // ISSUE: reference to a compiler-generated field
        LandValue landValue = this.m_LandValueData[entity];
        if ((double) landValue.m_LandValue <= 0.0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_TotalLandValueBonus += landValue.m_LandValue;
        // ISSUE: reference to a compiler-generated field
        ++this.m_TotalCount;
      }
    }

    [BurstCompile]
    private struct LandValueMapUpdateJob : IJobParallelFor
    {
      public NativeArray<LandValueCell> m_LandValueMap;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public NativeArray<TerrainAttractiveness> m_AttractiveMap;
      [ReadOnly]
      public NativeArray<GroundPollution> m_GroundPollutionMap;
      [ReadOnly]
      public NativeArray<AirPollution> m_AirPollutionMap;
      [ReadOnly]
      public NativeArray<NoisePollution> m_NoisePollutionMap;
      [ReadOnly]
      public NativeArray<AvailabilityInfoCell> m_AvailabilityInfoMap;
      [ReadOnly]
      public CellMapData<TelecomCoverage> m_TelecomCoverageMap;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public ComponentLookup<LandValue> m_LandValueData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      [ReadOnly]
      public AttractivenessParameterData m_AttractivenessParameterData;
      [ReadOnly]
      public LandValueParameterData m_LandValueParameterData;
      public float m_CellSize;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        float3 cellCenter = CellMapSystem<LandValueCell>.GetCellCenter(index, LandValueSystem.kTextureSize);
        // ISSUE: reference to a compiler-generated field
        if ((double) WaterUtils.SampleDepth(ref this.m_WaterSurfaceData, cellCenter) > 1.0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_LandValueMap[index] = new LandValueCell()
          {
            m_LandValue = this.m_LandValueParameterData.m_LandValueBaseline
          };
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          LandValueSystem.NetIterator iterator = new LandValueSystem.NetIterator()
          {
            m_TotalCount = 0,
            m_TotalLandValueBonus = 0.0f,
            m_Bounds = new Bounds3(cellCenter - new float3(1.5f * this.m_CellSize, 10000f, 1.5f * this.m_CellSize), cellCenter + new float3(1.5f * this.m_CellSize, 10000f, 1.5f * this.m_CellSize)),
            m_EdgeGeometryData = this.m_EdgeGeometryData,
            m_LandValueData = this.m_LandValueData
          };
          // ISSUE: reference to a compiler-generated field
          this.m_NetSearchTree.Iterate<LandValueSystem.NetIterator>(ref iterator);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          float pollution1 = (float) GroundPollutionSystem.GetPollution(cellCenter, this.m_GroundPollutionMap).m_Pollution;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          float pollution2 = (float) AirPollutionSystem.GetPollution(cellCenter, this.m_AirPollutionMap).m_Pollution;
          // ISSUE: reference to a compiler-generated field
          float pollution3 = (float) NoisePollutionSystem.GetPollution(cellCenter, this.m_NoisePollutionMap).m_Pollution;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          double x = (double) AvailabilityInfoToGridSystem.GetAvailabilityInfo(cellCenter, this.m_AvailabilityInfoMap).m_AvailabilityInfo.x;
          // ISSUE: reference to a compiler-generated field
          float num1 = TelecomCoverage.SampleNetworkQuality(this.m_TelecomCoverageMap, cellCenter);
          // ISSUE: reference to a compiler-generated field
          LandValueCell landValue = this.m_LandValueMap[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float num2 = ((double) iterator.m_TotalCount > 0.0 ? iterator.m_TotalLandValueBonus / (float) iterator.m_TotalCount : 0.0f) + (math.min((float) (x - 5.0) * this.m_LandValueParameterData.m_AttractivenessBonusMultiplier, this.m_LandValueParameterData.m_CommonFactorMaxBonus) + math.min(num1 * this.m_LandValueParameterData.m_TelecomCoverageBonusMultiplier, this.m_LandValueParameterData.m_CommonFactorMaxBonus));
          // ISSUE: reference to a compiler-generated field
          if ((double) WaterUtils.SamplePolluted(ref this.m_WaterSurfaceData, cellCenter) <= 0.0 && (double) pollution1 <= 0.0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            float attractiveness = TerrainAttractivenessSystem.EvaluateAttractiveness(TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, cellCenter), this.m_AttractiveMap[index], this.m_AttractivenessParameterData);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            num2 += math.min(math.max(attractiveness - 5f, 0.0f) * this.m_LandValueParameterData.m_AttractivenessBonusMultiplier, this.m_LandValueParameterData.m_CommonFactorMaxBonus);
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float num3 = (float) ((double) pollution1 * (double) this.m_LandValueParameterData.m_GroundPollutionPenaltyMultiplier + (double) pollution2 * (double) this.m_LandValueParameterData.m_AirPollutionPenaltyMultiplier + (double) pollution3 * (double) this.m_LandValueParameterData.m_NoisePollutionPenaltyMultiplier);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float y = math.max(this.m_LandValueParameterData.m_LandValueBaseline, this.m_LandValueParameterData.m_LandValueBaseline + num2 - num3);
          if ((double) math.abs(landValue.m_LandValue - y) >= 0.10000000149011612)
            landValue.m_LandValue = math.lerp(landValue.m_LandValue, y, 0.4f);
          // ISSUE: reference to a compiler-generated field
          this.m_LandValueMap[index] = landValue;
        }
      }
    }

    [BurstCompile]
    private struct EdgeUpdateJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Edge> m_EdgeType;
      [ReadOnly]
      public BufferTypeHandle<Game.Net.ServiceCoverage> m_ServiceCoverageType;
      [ReadOnly]
      public BufferTypeHandle<ResourceAvailability> m_AvailabilityType;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<LandValue> m_LandValues;
      [ReadOnly]
      public LandValueParameterData m_LandValueParameterData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Net.Edge> nativeArray2 = chunk.GetNativeArray<Game.Net.Edge>(ref this.m_EdgeType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Net.ServiceCoverage> bufferAccessor1 = chunk.GetBufferAccessor<Game.Net.ServiceCoverage>(ref this.m_ServiceCoverageType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ResourceAvailability> bufferAccessor2 = chunk.GetBufferAccessor<ResourceAvailability>(ref this.m_AvailabilityType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          float num1 = 0.0f;
          float num2 = 0.0f;
          float num3 = 0.0f;
          if (bufferAccessor1.Length > 0)
          {
            DynamicBuffer<Game.Net.ServiceCoverage> dynamicBuffer = bufferAccessor1[index];
            Game.Net.ServiceCoverage serviceCoverage1 = dynamicBuffer[0];
            // ISSUE: reference to a compiler-generated field
            num1 = math.lerp(serviceCoverage1.m_Coverage.x, serviceCoverage1.m_Coverage.y, 0.5f) * this.m_LandValueParameterData.m_HealthCoverageBonusMultiplier;
            Game.Net.ServiceCoverage serviceCoverage2 = dynamicBuffer[5];
            // ISSUE: reference to a compiler-generated field
            num2 = math.lerp(serviceCoverage2.m_Coverage.x, serviceCoverage2.m_Coverage.y, 0.5f) * this.m_LandValueParameterData.m_EducationCoverageBonusMultiplier;
            Game.Net.ServiceCoverage serviceCoverage3 = dynamicBuffer[2];
            // ISSUE: reference to a compiler-generated field
            num3 = math.lerp(serviceCoverage3.m_Coverage.x, serviceCoverage3.m_Coverage.y, 0.5f) * this.m_LandValueParameterData.m_PoliceCoverageBonusMultiplier;
          }
          float num4 = 0.0f;
          float num5 = 0.0f;
          float num6 = 0.0f;
          if (bufferAccessor2.Length > 0)
          {
            DynamicBuffer<ResourceAvailability> dynamicBuffer = bufferAccessor2[index];
            ResourceAvailability resourceAvailability1 = dynamicBuffer[1];
            // ISSUE: reference to a compiler-generated field
            num4 = math.lerp(resourceAvailability1.m_Availability.x, resourceAvailability1.m_Availability.y, 0.5f) * this.m_LandValueParameterData.m_CommercialServiceBonusMultiplier;
            ResourceAvailability resourceAvailability2 = dynamicBuffer[31];
            // ISSUE: reference to a compiler-generated field
            num5 = math.lerp(resourceAvailability2.m_Availability.x, resourceAvailability2.m_Availability.y, 0.5f) * this.m_LandValueParameterData.m_BusBonusMultiplier;
            ResourceAvailability resourceAvailability3 = dynamicBuffer[32];
            // ISSUE: reference to a compiler-generated field
            num6 = math.lerp(resourceAvailability3.m_Availability.x, resourceAvailability3.m_Availability.y, 0.5f) * this.m_LandValueParameterData.m_TramSubwayBonusMultiplier;
          }
          // ISSUE: reference to a compiler-generated field
          LandValue landValue = this.m_LandValues[entity];
          float y = math.max(num1 + num2 + num3 + num4 + num5 + num6, 0.0f);
          if ((double) math.abs(landValue.m_LandValue - y) >= 0.10000000149011612)
          {
            float x = math.lerp(landValue.m_LandValue, y, 0.6f);
            landValue.m_LandValue = math.max(x, 0.0f);
            // ISSUE: reference to a compiler-generated field
            this.m_LandValues[entity] = landValue;
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
      public ComponentTypeHandle<Game.Net.Edge> __Game_Net_Edge_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Net.ServiceCoverage> __Game_Net_ServiceCoverage_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<ResourceAvailability> __Game_Net_ResourceAvailability_RO_BufferTypeHandle;
      public ComponentLookup<LandValue> __Game_Net_LandValue_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LandValue> __Game_Net_LandValue_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ServiceCoverage_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Net.ServiceCoverage>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ResourceAvailability_RO_BufferTypeHandle = state.GetBufferTypeHandle<ResourceAvailability>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LandValue_RW_ComponentLookup = state.GetComponentLookup<LandValue>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LandValue_RO_ComponentLookup = state.GetComponentLookup<LandValue>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
      }
    }
  }
}
