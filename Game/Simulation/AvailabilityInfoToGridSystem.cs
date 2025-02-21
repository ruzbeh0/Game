// Decompiled with JetBrains decompiler
// Type: Game.Simulation.AvailabilityInfoToGridSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Colossal.Serialization.Entities;
using Game.Common;
using Game.Net;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class AvailabilityInfoToGridSystem : CellMapSystem<AvailabilityInfoCell>, IJobSerializable
  {
    public static readonly int kTextureSize = 128;
    public static readonly int kUpdatesPerDay = 32;
    private SearchSystem m_NetSearchSystem;
    private AvailabilityInfoToGridSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / AvailabilityInfoToGridSystem.kUpdatesPerDay;
    }

    public int2 TextureSize
    {
      get
      {
        return new int2(AvailabilityInfoToGridSystem.kTextureSize, AvailabilityInfoToGridSystem.kTextureSize);
      }
    }

    public static float3 GetCellCenter(int index)
    {
      // ISSUE: reference to a compiler-generated field
      return CellMapSystem<AvailabilityInfoCell>.GetCellCenter(index, AvailabilityInfoToGridSystem.kTextureSize);
    }

    public static AvailabilityInfoCell GetAvailabilityInfo(
      float3 position,
      NativeArray<AvailabilityInfoCell> AvailabilityInfoMap)
    {
      AvailabilityInfoCell availabilityInfo1 = new AvailabilityInfoCell();
      // ISSUE: reference to a compiler-generated field
      int2 cell = CellMapSystem<AvailabilityInfoCell>.GetCell(position, CellMapSystem<AvailabilityInfoCell>.kMapSize, AvailabilityInfoToGridSystem.kTextureSize);
      // ISSUE: reference to a compiler-generated field
      float2 cellCoords = CellMapSystem<AvailabilityInfoCell>.GetCellCoords(position, CellMapSystem<AvailabilityInfoCell>.kMapSize, AvailabilityInfoToGridSystem.kTextureSize);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (cell.x < 0 || cell.x >= AvailabilityInfoToGridSystem.kTextureSize || cell.y < 0 || cell.y >= AvailabilityInfoToGridSystem.kTextureSize)
        return new AvailabilityInfoCell();
      // ISSUE: reference to a compiler-generated field
      float4 availabilityInfo2 = AvailabilityInfoMap[cell.x + AvailabilityInfoToGridSystem.kTextureSize * cell.y].m_AvailabilityInfo;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float4 availabilityInfo3 = cell.x < AvailabilityInfoToGridSystem.kTextureSize - 1 ? AvailabilityInfoMap[cell.x + 1 + AvailabilityInfoToGridSystem.kTextureSize * cell.y].m_AvailabilityInfo : (float4) 0;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float4 availabilityInfo4 = cell.y < AvailabilityInfoToGridSystem.kTextureSize - 1 ? AvailabilityInfoMap[cell.x + AvailabilityInfoToGridSystem.kTextureSize * (cell.y + 1)].m_AvailabilityInfo : (float4) 0;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float4 availabilityInfo5 = cell.x >= AvailabilityInfoToGridSystem.kTextureSize - 1 || cell.y >= AvailabilityInfoToGridSystem.kTextureSize - 1 ? (float4) 0 : AvailabilityInfoMap[cell.x + 1 + AvailabilityInfoToGridSystem.kTextureSize * (cell.y + 1)].m_AvailabilityInfo;
      availabilityInfo1.m_AvailabilityInfo = math.lerp(math.lerp(availabilityInfo2, availabilityInfo3, cellCoords.x - (float) cell.x), math.lerp(availabilityInfo4, availabilityInfo5, cellCoords.x - (float) cell.x), cellCoords.y - (float) cell.y);
      return availabilityInfo1;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.CreateTextures(AvailabilityInfoToGridSystem.kTextureSize);
      // ISSUE: reference to a compiler-generated field
      this.m_NetSearchSystem = this.World.GetOrCreateSystemManaged<SearchSystem>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ResourceAvailability_RO_BufferLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      AvailabilityInfoToGridSystem.AvailabilityInfoToGridJob jobData = new AvailabilityInfoToGridSystem.AvailabilityInfoToGridJob()
      {
        m_NetSearchTree = this.m_NetSearchSystem.GetNetSearchTree(true, out dependencies),
        m_AvailabilityInfoMap = this.m_Map,
        m_AvailabilityData = this.__TypeHandle.__Game_Net_ResourceAvailability_RO_BufferLookup,
        m_EdgeGeometryData = this.__TypeHandle.__Game_Net_EdgeGeometry_RO_ComponentLookup,
        m_CellSize = (float) CellMapSystem<AvailabilityInfoCell>.kMapSize / (float) AvailabilityInfoToGridSystem.kTextureSize
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<AvailabilityInfoToGridSystem.AvailabilityInfoToGridJob>(AvailabilityInfoToGridSystem.kTextureSize * AvailabilityInfoToGridSystem.kTextureSize, AvailabilityInfoToGridSystem.kTextureSize, JobHandle.CombineDependencies(dependencies, JobHandle.CombineDependencies(this.m_WriteDependencies, this.m_ReadDependencies, this.Dependency)));
      this.AddWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NetSearchSystem.AddNetSearchTreeReader(this.Dependency);
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
    public AvailabilityInfoToGridSystem()
    {
    }

    private struct NetIterator : 
      INativeQuadTreeIterator<Entity, QuadTreeBoundsXZ>,
      IUnsafeQuadTreeIterator<Entity, QuadTreeBoundsXZ>
    {
      public AvailabilityInfoCell m_TotalWeight;
      public AvailabilityInfoCell m_Result;
      public float m_CellSize;
      public Bounds3 m_Bounds;
      public BufferLookup<ResourceAvailability> m_Availabilities;
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;

      public bool Intersect(QuadTreeBoundsXZ bounds)
      {
        // ISSUE: reference to a compiler-generated field
        return MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds);
      }

      private void AddData(
        float2 attractiveness2,
        float2 uneducated2,
        float2 educated2,
        float2 services2,
        float2 workplaces2,
        float2 t,
        float3 curvePos,
        float weight)
      {
        float num1 = math.lerp(attractiveness2.x, attractiveness2.y, t.y);
        float num2 = 0.5f * math.lerp(uneducated2.x + educated2.x, uneducated2.y + educated2.y, t.y);
        float num3 = math.lerp(services2.x, services2.y, t.y);
        float num4 = math.lerp(workplaces2.x, workplaces2.y, t.y);
        // ISSUE: reference to a compiler-generated field
        this.m_Result.AddAttractiveness(weight * num1);
        // ISSUE: reference to a compiler-generated field
        this.m_TotalWeight.AddAttractiveness(weight);
        // ISSUE: reference to a compiler-generated field
        this.m_Result.AddConsumers(weight * num2);
        // ISSUE: reference to a compiler-generated field
        this.m_TotalWeight.AddConsumers(weight);
        // ISSUE: reference to a compiler-generated field
        this.m_Result.AddServices(weight * num3);
        // ISSUE: reference to a compiler-generated field
        this.m_TotalWeight.AddServices(weight);
        // ISSUE: reference to a compiler-generated field
        this.m_Result.AddWorkplaces(weight * num4);
        // ISSUE: reference to a compiler-generated field
        this.m_TotalWeight.AddWorkplaces(weight);
      }

      public void Iterate(QuadTreeBoundsXZ bounds, Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (!MathUtils.Intersect(bounds.m_Bounds, this.m_Bounds) || !this.m_Availabilities.HasBuffer(entity) || !this.m_EdgeGeometryData.HasComponent(entity))
          return;
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ResourceAvailability> availability1 = this.m_Availabilities[entity];
        float2 availability2 = availability1[18].m_Availability;
        float2 availability3 = availability1[2].m_Availability;
        float2 availability4 = availability1[3].m_Availability;
        float2 availability5 = availability1[1].m_Availability;
        float2 availability6 = availability1[0].m_Availability;
        // ISSUE: reference to a compiler-generated field
        EdgeGeometry edgeGeometry = this.m_EdgeGeometryData[entity];
        int x1 = (int) math.ceil(edgeGeometry.m_Start.middleLength * 0.05f);
        int x2 = (int) math.ceil(edgeGeometry.m_End.middleLength * 0.05f);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float3 float3 = 0.5f * (this.m_Bounds.min + this.m_Bounds.max);
        for (int index = 1; index <= x1; ++index)
        {
          float2 t = (float) index / new float2((float) x1, (float) (x1 + x2));
          float3 curvePos = math.lerp(MathUtils.Position(edgeGeometry.m_Start.m_Left, t.x), MathUtils.Position(edgeGeometry.m_Start.m_Right, t.x), 0.5f);
          // ISSUE: reference to a compiler-generated field
          float weight = math.max(0.0f, (float) (1.0 - (double) math.distance(float3.xz, curvePos.xz) / (1.5 * (double) this.m_CellSize)));
          // ISSUE: reference to a compiler-generated method
          this.AddData(availability2, availability3, availability4, availability5, availability6, t, curvePos, weight);
        }
        for (int x3 = 1; x3 <= x2; ++x3)
        {
          float2 t = new float2((float) x3, (float) (x1 + x3)) / new float2((float) x2, (float) (x1 + x2));
          float3 curvePos = math.lerp(MathUtils.Position(edgeGeometry.m_End.m_Left, t.x), MathUtils.Position(edgeGeometry.m_End.m_Right, t.x), 0.5f);
          // ISSUE: reference to a compiler-generated field
          float weight = math.max(0.0f, (float) (1.0 - (double) math.distance(float3.xz, curvePos.xz) / (1.5 * (double) this.m_CellSize)));
          // ISSUE: reference to a compiler-generated method
          this.AddData(availability2, availability3, availability4, availability5, availability6, t, curvePos, weight);
        }
      }
    }

    [BurstCompile]
    private struct AvailabilityInfoToGridJob : IJobParallelFor
    {
      public NativeArray<AvailabilityInfoCell> m_AvailabilityInfoMap;
      [ReadOnly]
      public NativeQuadTree<Entity, QuadTreeBoundsXZ> m_NetSearchTree;
      [ReadOnly]
      public BufferLookup<ResourceAvailability> m_AvailabilityData;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> m_EdgeGeometryData;
      public float m_CellSize;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        float3 cellCenter = CellMapSystem<AvailabilityInfoCell>.GetCellCenter(index, AvailabilityInfoToGridSystem.kTextureSize);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        AvailabilityInfoToGridSystem.NetIterator iterator = new AvailabilityInfoToGridSystem.NetIterator()
        {
          m_TotalWeight = new AvailabilityInfoCell(),
          m_Result = new AvailabilityInfoCell(),
          m_Bounds = new Bounds3(cellCenter - new float3(1.5f * this.m_CellSize, 10000f, 1.5f * this.m_CellSize), cellCenter + new float3(1.5f * this.m_CellSize, 10000f, 1.5f * this.m_CellSize)),
          m_CellSize = this.m_CellSize,
          m_EdgeGeometryData = this.m_EdgeGeometryData,
          m_Availabilities = this.m_AvailabilityData
        };
        // ISSUE: reference to a compiler-generated field
        this.m_NetSearchTree.Iterate<AvailabilityInfoToGridSystem.NetIterator>(ref iterator);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        AvailabilityInfoCell availabilityInfo = this.m_AvailabilityInfoMap[index] with
        {
          m_AvailabilityInfo = math.select(iterator.m_Result.m_AvailabilityInfo / iterator.m_TotalWeight.m_AvailabilityInfo, (float4) 0.0f, iterator.m_TotalWeight.m_AvailabilityInfo == 0.0f)
        };
        // ISSUE: reference to a compiler-generated field
        this.m_AvailabilityInfoMap[index] = availabilityInfo;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public BufferLookup<ResourceAvailability> __Game_Net_ResourceAvailability_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<EdgeGeometry> __Game_Net_EdgeGeometry_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ResourceAvailability_RO_BufferLookup = state.GetBufferLookup<ResourceAvailability>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_EdgeGeometry_RO_ComponentLookup = state.GetComponentLookup<EdgeGeometry>(true);
      }
    }
  }
}
