// Decompiled with JetBrains decompiler
// Type: Game.Simulation.SoilWaterSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Serialization.Entities;
using Game.Common;
using Game.Events;
using Game.Prefabs;
using Game.Serialization;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class SoilWaterSystem : CellMapSystem<SoilWater>, IJobSerializable, IPostDeserialize
  {
    public static readonly int kTextureSize = 128;
    public static readonly int kUpdatesPerDay = 1024;
    public static readonly int kLoadDistribution = 8;
    private SimulationSystem m_SimulationSystem;
    private TerrainSystem m_TerrainSystem;
    private WaterSystem m_WaterSystem;
    private ClimateSystem m_ClimateSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private Texture2D m_SoilWaterTexture;
    private EntityQuery m_SoilWaterParameterQuery;
    private EntityQuery m_FloodQuery;
    private EntityQuery m_FloodPrefabQuery;
    private SoilWaterSystem.TypeHandle __TypeHandle;
    private EntityQuery __query_336595330_0;

    public int2 TextureSize => new int2(SoilWaterSystem.kTextureSize, SoilWaterSystem.kTextureSize);

    public Texture soilTexture => (Texture) this.m_SoilWaterTexture;

    public static float3 GetCellCenter(int index)
    {
      // ISSUE: reference to a compiler-generated field
      return CellMapSystem<SoilWater>.GetCellCenter(index, SoilWaterSystem.kTextureSize);
    }

    public static SoilWater GetSoilWater(float3 position, NativeArray<SoilWater> soilWaterMap)
    {
      SoilWater soilWater = new SoilWater();
      // ISSUE: reference to a compiler-generated field
      int2 cell = CellMapSystem<SoilWater>.GetCell(position, CellMapSystem<SoilWater>.kMapSize, SoilWaterSystem.kTextureSize);
      // ISSUE: reference to a compiler-generated field
      float2 cellCoords = CellMapSystem<SoilWater>.GetCellCoords(position, CellMapSystem<SoilWater>.kMapSize, SoilWaterSystem.kTextureSize);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (cell.x < 0 || cell.x >= SoilWaterSystem.kTextureSize || cell.y < 0 || cell.y >= SoilWaterSystem.kTextureSize)
        return soilWater;
      // ISSUE: reference to a compiler-generated field
      float amount = (float) soilWaterMap[cell.x + SoilWaterSystem.kTextureSize * cell.y].m_Amount;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float y1 = cell.x < SoilWaterSystem.kTextureSize - 1 ? (float) soilWaterMap[cell.x + 1 + SoilWaterSystem.kTextureSize * cell.y].m_Amount : 0.0f;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float x = cell.y < SoilWaterSystem.kTextureSize - 1 ? (float) soilWaterMap[cell.x + SoilWaterSystem.kTextureSize * (cell.y + 1)].m_Amount : 0.0f;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float y2 = cell.x >= SoilWaterSystem.kTextureSize - 1 || cell.y >= SoilWaterSystem.kTextureSize - 1 ? 0.0f : (float) soilWaterMap[cell.x + 1 + SoilWaterSystem.kTextureSize * (cell.y + 1)].m_Amount;
      soilWater.m_Amount = (short) Mathf.RoundToInt(math.lerp(math.lerp(amount, y1, cellCoords.x - (float) cell.x), math.lerp(x, y2, cellCoords.x - (float) cell.x), cellCoords.y - (float) cell.y));
      return soilWater;
    }

    private void CreateFloodCounter()
    {
      this.EntityManager.CreateEntity(this.EntityManager.CreateArchetype(ComponentType.ReadWrite<FloodCounterData>()));
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_SoilWaterParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<SoilWaterParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_FloodQuery = this.GetEntityQuery(ComponentType.ReadOnly<Flood>());
      // ISSUE: reference to a compiler-generated field
      this.m_FloodPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<FloodData>());
      // ISSUE: reference to a compiler-generated method
      this.CreateFloodCounter();
      // ISSUE: reference to a compiler-generated field
      this.CreateTextures(SoilWaterSystem.kTextureSize);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Texture2D texture2D = new Texture2D(SoilWaterSystem.kTextureSize, SoilWaterSystem.kTextureSize, TextureFormat.RFloat, false, true);
      texture2D.name = "SoilWaterTexture";
      texture2D.hideFlags = HideFlags.HideAndDontSave;
      // ISSUE: reference to a compiler-generated field
      this.m_SoilWaterTexture = texture2D;
      // ISSUE: reference to a compiler-generated field
      NativeArray<float> rawTextureData = this.m_SoilWaterTexture.GetRawTextureData<float>();
      for (int index = 0; index < this.m_Map.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        double num1 = (double) (index % SoilWaterSystem.kTextureSize) / (double) SoilWaterSystem.kTextureSize;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        double num2 = (double) (index / SoilWaterSystem.kTextureSize) / (double) SoilWaterSystem.kTextureSize;
        SoilWater soilWater = new SoilWater()
        {
          m_Amount = 1024,
          m_Max = 8192
        };
        this.m_Map[index] = soilWater;
        rawTextureData[index] = 0.0f;
      }
      // ISSUE: reference to a compiler-generated field
      this.m_SoilWaterTexture.Apply();
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_SoilWaterParameterQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      TerrainHeightData heightData = this.m_TerrainSystem.GetHeightData();
      if (!heightData.isCreated)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_SoilWaterTexture.Apply();
      // ISSUE: reference to a compiler-generated field
      float num1 = this.m_ClimateSystem.precipitation.value;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int num2 = 262144 / (SoilWaterSystem.kUpdatesPerDay / SoilWaterSystem.kLoadDistribution) / this.m_WaterSystem.SimulationCycleSteps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      int num3 = (int) ((long) this.m_SimulationSystem.frameIndex / (long) (262144 / SoilWaterSystem.kUpdatesPerDay) % (long) SoilWaterSystem.kLoadDistribution);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_FloodCounterData_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_EventData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Events_WaterLevelChange_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle deps;
      JobHandle outJobHandle1;
      JobHandle outJobHandle2;
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      SoilWaterSystem.SoilWaterTickJob jobData = new SoilWaterSystem.SoilWaterTickJob()
      {
        m_SoilWaterMap = this.m_Map,
        m_TerrainHeightData = heightData,
        m_WaterSurfaceData = this.m_WaterSystem.GetSurfaceData(out deps),
        m_SoilWaterTextureData = this.m_SoilWaterTexture.GetRawTextureData<float>(),
        m_SoilWaterParameters = this.m_SoilWaterParameterQuery.GetSingleton<SoilWaterParameterData>(),
        m_FloodEntities = this.m_FloodQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle1),
        m_FloodPrefabEntities = this.m_FloodPrefabQuery.ToEntityListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle2),
        m_Changes = this.__TypeHandle.__Game_Events_WaterLevelChange_RW_ComponentLookup,
        m_Events = this.__TypeHandle.__Game_Prefabs_EventData_RO_ComponentLookup,
        m_FloodCounterDatas = this.__TypeHandle.__Game_Simulation_FloodCounterData_RW_ComponentLookup,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer(),
        m_FloodCounterEntity = this.__query_336595330_0.GetSingletonEntity(),
        m_Weather = num1,
        m_ShaderUpdatesPerSoilUpdate = num2,
        m_LoadDistributionIndex = num3
      };
      this.Dependency = jobData.Schedule<SoilWaterSystem.SoilWaterTickJob>(JobUtils.CombineDependencies(this.m_WriteDependencies, this.m_ReadDependencies, outJobHandle1, outJobHandle2, deps, this.Dependency));
      this.AddWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TerrainSystem.AddCPUHeightReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(this.Dependency);
      this.Dependency = JobHandle.CombineDependencies(this.m_ReadDependencies, this.m_WriteDependencies, this.Dependency);
    }

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
      using (EntityQuery entityQuery = this.EntityManager.CreateEntityQuery(ComponentType.ReadWrite<FloodCounterData>()))
      {
        if (entityQuery.CalculateEntityCount() != 0)
          return;
        // ISSUE: reference to a compiler-generated method
        this.CreateFloodCounter();
      }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_336595330_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<FloodCounterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
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
    public SoilWaterSystem()
    {
    }

    [BurstCompile]
    private struct SoilWaterTickJob : IJob
    {
      public NativeArray<SoilWater> m_SoilWaterMap;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      public NativeArray<float> m_SoilWaterTextureData;
      public SoilWaterParameterData m_SoilWaterParameters;
      public ComponentLookup<WaterLevelChange> m_Changes;
      public ComponentLookup<FloodCounterData> m_FloodCounterDatas;
      [ReadOnly]
      public ComponentLookup<EventData> m_Events;
      [ReadOnly]
      public NativeList<Entity> m_FloodEntities;
      [ReadOnly]
      public NativeList<Entity> m_FloodPrefabEntities;
      public EntityCommandBuffer m_CommandBuffer;
      public Entity m_FloodCounterEntity;
      public float m_Weather;
      public int m_ShaderUpdatesPerSoilUpdate;
      public int m_LoadDistributionIndex;

      private void HandleInterface(
        int index,
        int otherIndex,
        NativeArray<int> tmp,
        ref SoilWaterParameterData soilWaterParameters)
      {
        // ISSUE: reference to a compiler-generated field
        SoilWater soilWater1 = this.m_SoilWaterMap[index];
        // ISSUE: reference to a compiler-generated field
        SoilWater soilWater2 = this.m_SoilWaterMap[otherIndex];
        int num1 = tmp[index];
        int num2 = tmp[otherIndex];
        float num3 = soilWater2.m_Surface - soilWater1.m_Surface;
        float num4 = (float) ((double) soilWater2.m_Amount / (double) soilWater2.m_Max - (double) soilWater1.m_Amount / (double) soilWater1.m_Max);
        // ISSUE: reference to a compiler-generated field
        float y = (float) ((double) soilWaterParameters.m_HeightEffect * (double) num3 / (double) (CellMapSystem<SoilWater>.kMapSize / SoilWaterSystem.kTextureSize) + 0.25 * (double) num4);
        float num5 = (double) y < 0.0 ? math.max(-soilWaterParameters.m_MaxDiffusion, y) : math.min(soilWaterParameters.m_MaxDiffusion, y);
        int num6 = Mathf.RoundToInt(num5 * ((double) num5 > 0.0 ? (float) soilWater2.m_Amount : (float) soilWater1.m_Amount));
        int num7 = num1 + num6;
        int num8 = num2 - num6;
        tmp[index] = num7;
        tmp[otherIndex] = num8;
      }

      private void StartFlood()
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_FloodPrefabEntities.Length <= 0)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        Entity entity = this.m_CommandBuffer.CreateEntity(this.m_Events[this.m_FloodPrefabEntities[0]].m_Archetype);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<PrefabRef>(entity, new PrefabRef()
        {
          m_Prefab = this.m_FloodPrefabEntities[0]
        });
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<WaterLevelChange>(entity, new WaterLevelChange()
        {
          m_DangerHeight = 0.0f,
          m_Direction = new float2(0.0f, 0.0f),
          m_Intensity = 0.0f,
          m_MaxIntensity = 0.0f
        });
      }

      private void StopFlood()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Deleted>(this.m_FloodEntities[0]);
      }

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<int> tmp = new NativeArray<int>(this.m_SoilWaterMap.Length, Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_SoilWaterMap.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          int num1 = index % SoilWaterSystem.kTextureSize;
          // ISSUE: reference to a compiler-generated field
          int num2 = index / SoilWaterSystem.kTextureSize;
          // ISSUE: reference to a compiler-generated field
          int num3 = SoilWaterSystem.kTextureSize - 1;
          if (num1 < num3)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.HandleInterface(index, index + 1, tmp, ref this.m_SoilWaterParameters);
          }
          // ISSUE: reference to a compiler-generated field
          if (num2 < SoilWaterSystem.kTextureSize - 1)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.HandleInterface(index, index + SoilWaterSystem.kTextureSize, tmp, ref this.m_SoilWaterParameters);
          }
        }
        // ISSUE: reference to a compiler-generated field
        float num4 = math.max(0.0f, math.pow(2f * math.max(0.0f, this.m_Weather - 0.5f), 2f));
        // ISSUE: reference to a compiler-generated field
        float num5 = (float) (1.0 / (2.0 * (double) this.m_SoilWaterParameters.m_MaximumWaterDepth));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int2 int2 = this.m_WaterSurfaceData.resolution.xz / SoilWaterSystem.kTextureSize;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        FloodCounterData floodCounterData = this.m_FloodCounterDatas[this.m_FloodCounterEntity];
        floodCounterData.m_FloodCounter = math.max(0.0f, (float) (0.98000001907348633 * (double) floodCounterData.m_FloodCounter + 2.0 * (double) num4 - 0.10000000149011612));
        // ISSUE: reference to a compiler-generated field
        if ((double) floodCounterData.m_FloodCounter > 20.0 && this.m_FloodEntities.Length == 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.StartFlood();
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_FloodEntities.Length > 0)
          {
            if ((double) floodCounterData.m_FloodCounter == 0.0)
            {
              // ISSUE: reference to a compiler-generated method
              this.StopFlood();
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_Changes[this.m_FloodEntities[0]] = this.m_Changes[this.m_FloodEntities[0]] with
              {
                m_Intensity = math.max(0.0f, (float) (((double) floodCounterData.m_FloodCounter - 20.0) / 80.0))
              };
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_FloodCounterDatas[this.m_FloodCounterEntity] = floodCounterData;
        int num6 = 0;
        int num7 = 0;
        int num8 = 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num9 = this.m_LoadDistributionIndex * SoilWaterSystem.kTextureSize / SoilWaterSystem.kLoadDistribution;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num10 = num9 + SoilWaterSystem.kTextureSize / SoilWaterSystem.kLoadDistribution;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        for (int index1 = num9 * SoilWaterSystem.kTextureSize; index1 < num10 * SoilWaterSystem.kTextureSize; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          SoilWater soilWater = this.m_SoilWaterMap[index1];
          // ISSUE: reference to a compiler-generated field
          soilWater.m_Amount = (short) math.max(0, (int) soilWater.m_Amount + tmp[index1] + Mathf.RoundToInt(this.m_SoilWaterParameters.m_RainMultiplier * num4));
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          float num11 = TerrainUtils.SampleHeight(ref this.m_TerrainHeightData, SoilWaterSystem.GetCellCenter(index1));
          soilWater.m_Surface = num11;
          short val1 = (short) Mathf.RoundToInt(math.max(0.0f, (float) (0.10000000149011612 * (0.5 * (double) soilWater.m_Max - (double) soilWater.m_Amount))));
          // ISSUE: reference to a compiler-generated field
          float x = (float) val1 * this.m_SoilWaterParameters.m_WaterPerUnit / (float) soilWater.m_Max;
          int num12 = 0;
          int num13 = 0;
          float num14 = 0.0f;
          float num15 = 0.0f;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          int num16 = index1 % SoilWaterSystem.kTextureSize * int2.x + index1 / SoilWaterSystem.kTextureSize * this.m_WaterSurfaceData.resolution.x * int2.y;
          for (int index2 = 0; index2 < int2.x; index2 += 4)
          {
            for (int index3 = 0; index3 < int2.y; index3 += 4)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              float depth = this.m_WaterSurfaceData.depths[num16 + index2 + index3 * this.m_WaterSurfaceData.resolution.z].m_Depth;
              if ((double) depth > 0.0099999997764825821)
              {
                ++num12;
                // ISSUE: reference to a compiler-generated field
                num14 += math.min(this.m_SoilWaterParameters.m_MaximumWaterDepth, depth);
                num15 += math.min(x, depth);
              }
              ++num13;
            }
          }
          short num17 = (short) Math.Min((int) val1, Mathf.RoundToInt((float) soilWater.m_Max * 10f * num15));
          // ISSUE: reference to a compiler-generated field
          float num18 = (float) num17 * this.m_SoilWaterParameters.m_WaterPerUnit / (float) soilWater.m_Max;
          float num19 = (float) (1.0 - (double) num5 * (double) num14 / (double) num13) * (float) soilWater.m_Max;
          // ISSUE: reference to a compiler-generated field
          short num20 = (short) Mathf.RoundToInt(math.max(0.0f, this.m_SoilWaterParameters.m_OverflowRate * ((float) soilWater.m_Amount - num19)));
          float num21 = 0.0f;
          if ((double) num20 > 0.0)
          {
            num21 = (float) soilWater.m_Amount / (float) soilWater.m_Max;
            num18 = 0.0f;
          }
          if (num12 == 0)
            num18 = 0.0f;
          soilWater.m_Amount += num17;
          soilWater.m_Amount -= num20;
          short val2 = (short) Mathf.RoundToInt(math.sign((float) ((int) soilWater.m_Max / 8 - (int) soilWater.m_Amount)));
          soilWater.m_Amount += val2;
          num7 += (int) num17 + (int) Math.Max((short) 0, val2);
          num6 += (int) num20 + Math.Max(0, (int) -val2);
          num8 += (int) soilWater.m_Amount;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_SoilWaterTextureData[index1] = -num18 / (float) this.m_ShaderUpdatesPerSoilUpdate + num21;
          // ISSUE: reference to a compiler-generated field
          this.m_SoilWaterMap[index1] = soilWater;
        }
        tmp.Dispose();
      }
    }

    private struct TypeHandle
    {
      public ComponentLookup<WaterLevelChange> __Game_Events_WaterLevelChange_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EventData> __Game_Prefabs_EventData_RO_ComponentLookup;
      public ComponentLookup<FloodCounterData> __Game_Simulation_FloodCounterData_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Events_WaterLevelChange_RW_ComponentLookup = state.GetComponentLookup<WaterLevelChange>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_EventData_RO_ComponentLookup = state.GetComponentLookup<EventData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_FloodCounterData_RW_ComponentLookup = state.GetComponentLookup<FloodCounterData>();
      }
    }
  }
}
