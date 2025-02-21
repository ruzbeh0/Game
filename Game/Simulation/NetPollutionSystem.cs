// Decompiled with JetBrains decompiler
// Type: Game.Simulation.NetPollutionSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
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
  public class NetPollutionSystem : GameSystemBase
  {
    public static readonly int kUpdatesPerDay = 128;
    private SimulationSystem m_SimulationSystem;
    private EntityQuery m_PollutionQuery;
    private AirPollutionSystem m_AirPollutionSystem;
    private NoisePollutionSystem m_NoisePollutionSystem;
    private EntityQuery m_PollutionParameterQuery;
    private NetPollutionSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / (NetPollutionSystem.kUpdatesPerDay * 16);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AirPollutionSystem = this.World.GetOrCreateSystemManaged<AirPollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NoisePollutionSystem = this.World.GetOrCreateSystemManaged<NoisePollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PollutionQuery = this.GetEntityQuery(ComponentType.ReadWrite<Game.Net.Pollution>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_PollutionParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<PollutionParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PollutionQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PollutionParameterQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, NetPollutionSystem.kUpdatesPerDay, 16);
      // ISSUE: reference to a compiler-generated field
      this.m_PollutionQuery.ResetFilter();
      // ISSUE: reference to a compiler-generated field
      this.m_PollutionQuery.SetSharedComponentFilter<UpdateFrame>(new UpdateFrame(updateFrame));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_NetPollutionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Pollution_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Elevation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies1;
      JobHandle dependencies2;
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new NetPollutionSystem.UpdateNetPollutionJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_NodeType = this.__TypeHandle.__Game_Net_Node_RO_ComponentTypeHandle,
        m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
        m_UpgradedType = this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentTypeHandle,
        m_ElevationType = this.__TypeHandle.__Game_Net_Elevation_RO_ComponentTypeHandle,
        m_CompositionType = this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_ConnectedEdgeType = this.__TypeHandle.__Game_Net_ConnectedEdge_RO_BufferTypeHandle,
        m_PollutionType = this.__TypeHandle.__Game_Net_Pollution_RW_ComponentTypeHandle,
        m_EdgeData = this.__TypeHandle.__Game_Net_Edge_RO_ComponentLookup,
        m_UpgradedData = this.__TypeHandle.__Game_Net_Upgraded_RO_ComponentLookup,
        m_CompositionData = this.__TypeHandle.__Game_Net_Composition_RO_ComponentLookup,
        m_NetPollutionData = this.__TypeHandle.__Game_Prefabs_NetPollutionData_RO_ComponentLookup,
        m_NetCompositionData = this.__TypeHandle.__Game_Prefabs_NetCompositionData_RO_ComponentLookup,
        m_AirPollutionMap = this.m_AirPollutionSystem.GetMap(false, out dependencies1),
        m_NoisePollutionMap = this.m_NoisePollutionSystem.GetMap(false, out dependencies2),
        m_AirPollutionTextureSize = AirPollutionSystem.kTextureSize,
        m_NoisePollutionTextureSize = NoisePollutionSystem.kTextureSize,
        m_MapSize = CellMapSystem<AirPollution>.kMapSize,
        m_PollutionParameters = this.m_PollutionParameterQuery.GetSingleton<PollutionParameterData>()
      }.Schedule<NetPollutionSystem.UpdateNetPollutionJob>(this.m_PollutionQuery, JobHandle.CombineDependencies(dependencies1, dependencies2, this.Dependency));
      // ISSUE: reference to a compiler-generated field
      this.m_AirPollutionSystem.AddWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_NoisePollutionSystem.AddWriter(jobHandle);
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
    public NetPollutionSystem()
    {
    }

    [BurstCompile]
    private struct UpdateNetPollutionJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Net.Node> m_NodeType;
      [ReadOnly]
      public ComponentTypeHandle<Curve> m_CurveType;
      [ReadOnly]
      public ComponentTypeHandle<Upgraded> m_UpgradedType;
      [ReadOnly]
      public ComponentTypeHandle<Elevation> m_ElevationType;
      [ReadOnly]
      public ComponentTypeHandle<Composition> m_CompositionType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<ConnectedEdge> m_ConnectedEdgeType;
      public ComponentTypeHandle<Game.Net.Pollution> m_PollutionType;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> m_EdgeData;
      [ReadOnly]
      public ComponentLookup<Upgraded> m_UpgradedData;
      [ReadOnly]
      public ComponentLookup<Composition> m_CompositionData;
      [ReadOnly]
      public ComponentLookup<NetPollutionData> m_NetPollutionData;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> m_NetCompositionData;
      public int m_MapSize;
      public int m_AirPollutionTextureSize;
      public int m_NoisePollutionTextureSize;
      public NativeArray<AirPollution> m_AirPollutionMap;
      public NativeArray<NoisePollution> m_NoisePollutionMap;
      [ReadOnly]
      public PollutionParameterData m_PollutionParameters;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        PollutionParameterData pollutionParameters = this.m_PollutionParameters;
        // ISSUE: reference to a compiler-generated field
        float s = 4f / (float) NetPollutionSystem.kUpdatesPerDay;
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray1 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Net.Node> nativeArray2 = chunk.GetNativeArray<Game.Net.Node>(ref this.m_NodeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Net.Pollution> nativeArray3 = chunk.GetNativeArray<Game.Net.Pollution>(ref this.m_PollutionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Elevation> nativeArray4 = chunk.GetNativeArray<Elevation>(ref this.m_ElevationType);
        if (nativeArray2.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray5 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<ConnectedEdge> bufferAccessor = chunk.GetBufferAccessor<ConnectedEdge>(ref this.m_ConnectedEdgeType);
          for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
          {
            PrefabRef prefabRef = nativeArray1[index1];
            ref Game.Net.Pollution local = ref nativeArray3.ElementAt<Game.Net.Pollution>(index1);
            local.m_Accumulation = math.lerp(local.m_Accumulation, local.m_Pollution, s);
            local.m_Pollution = new float2();
            NetPollutionData componentData1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_NetPollutionData.TryGetComponent(prefabRef.m_Prefab, out componentData1))
            {
              Entity entity = nativeArray5[index1];
              Game.Net.Node node = nativeArray2[index1];
              float2 float2 = local.m_Accumulation * componentData1.m_Factors;
              float4 float4 = (float4) 0.0f;
              Elevation elevation;
              bool flag = CollectionUtils.TryGet<Elevation>(nativeArray4, index1, out elevation) && math.all(elevation.m_Elevation < 0.0f);
              DynamicBuffer<ConnectedEdge> dynamicBuffer = bufferAccessor[index1];
              for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
              {
                ConnectedEdge connectedEdge = dynamicBuffer[index2];
                // ISSUE: reference to a compiler-generated field
                Game.Net.Edge edge = this.m_EdgeData[connectedEdge.m_Edge];
                bool2 x1 = new bool2(edge.m_Start == entity, edge.m_End == entity);
                if (math.any(x1))
                {
                  float3 x2 = (float3) float2.x;
                  Composition componentData2;
                  // ISSUE: reference to a compiler-generated field
                  if (flag && this.m_CompositionData.TryGetComponent(connectedEdge.m_Edge, out componentData2))
                  {
                    // ISSUE: reference to a compiler-generated field
                    flag = (this.m_NetCompositionData[x1.x ? componentData2.m_StartNode : componentData2.m_EndNode].m_Flags.m_General & CompositionFlags.General.Tunnel) > (CompositionFlags.General) 0;
                  }
                  Upgraded componentData3;
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_UpgradedData.TryGetComponent(connectedEdge.m_Edge, out componentData3))
                  {
                    // ISSUE: reference to a compiler-generated method
                    this.CheckUpgrades(ref x2, componentData3);
                  }
                  float4 += new float4(x2, 1f);
                }
              }
              if (!flag)
              {
                if ((double) float4.w != 0.0)
                {
                  float4 /= float4.w;
                  float4.x = (float) (((double) float4.x + (double) float4.z) * 0.5);
                }
                // ISSUE: reference to a compiler-generated method
                this.ApplyPollution(node.m_Position, float4.xy, float2.y, ref pollutionParameters);
              }
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Curve> nativeArray6 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Upgraded> nativeArray7 = chunk.GetNativeArray<Upgraded>(ref this.m_UpgradedType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Composition> nativeArray8 = chunk.GetNativeArray<Composition>(ref this.m_CompositionType);
          for (int index = 0; index < nativeArray6.Length; ++index)
          {
            PrefabRef prefabRef = nativeArray1[index];
            ref Game.Net.Pollution local = ref nativeArray3.ElementAt<Game.Net.Pollution>(index);
            local.m_Accumulation = math.lerp(local.m_Accumulation, local.m_Pollution, s);
            local.m_Pollution = new float2();
            NetPollutionData componentData;
            Elevation elevation;
            Composition composition;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_NetPollutionData.TryGetComponent(prefabRef.m_Prefab, out componentData) && (!CollectionUtils.TryGet<Elevation>(nativeArray4, index, out elevation) || !CollectionUtils.TryGet<Composition>(nativeArray8, index, out composition) || !math.all(elevation.m_Elevation < 0.0f) || (this.m_NetCompositionData[composition.m_Edge].m_Flags.m_General & CompositionFlags.General.Tunnel) == (CompositionFlags.General) 0))
            {
              Curve curve = nativeArray6[index];
              float2 float2 = local.m_Accumulation * componentData.m_Factors;
              float3 x = (float3) float2.x;
              x.y *= 2f;
              Upgraded upgraded;
              if (CollectionUtils.TryGet<Upgraded>(nativeArray7, index, out upgraded))
              {
                // ISSUE: reference to a compiler-generated method
                this.CheckUpgrades(ref x, upgraded);
              }
              // ISSUE: reference to a compiler-generated method
              this.ApplyPollution(curve, x, float2.y, ref pollutionParameters);
            }
          }
        }
      }

      private void CheckUpgrades(ref float3 noisePollution, Upgraded upgraded)
      {
        if ((upgraded.m_Flags.m_Left & upgraded.m_Flags.m_Right & CompositionFlags.Side.SoundBarrier) != (CompositionFlags.Side) 0)
          noisePollution *= new float3(0.0f, 0.5f, 0.0f);
        else if ((upgraded.m_Flags.m_Left & CompositionFlags.Side.SoundBarrier) != (CompositionFlags.Side) 0)
          noisePollution *= new float3(0.0f, 0.5f, 1.5f);
        else if ((upgraded.m_Flags.m_Right & CompositionFlags.Side.SoundBarrier) != (CompositionFlags.Side) 0)
          noisePollution *= new float3(1.5f, 0.5f, 0.0f);
        if ((upgraded.m_Flags.m_Left & upgraded.m_Flags.m_Right & CompositionFlags.Side.PrimaryBeautification) != (CompositionFlags.Side) 0)
          noisePollution *= new float3(0.5f, 0.5f, 0.5f);
        else if ((upgraded.m_Flags.m_Left & CompositionFlags.Side.PrimaryBeautification) != (CompositionFlags.Side) 0)
          noisePollution *= new float3(0.5f, 0.75f, 1f);
        else if ((upgraded.m_Flags.m_Right & CompositionFlags.Side.PrimaryBeautification) != (CompositionFlags.Side) 0)
          noisePollution *= new float3(1f, 0.75f, 0.5f);
        if ((upgraded.m_Flags.m_Left & upgraded.m_Flags.m_Right & CompositionFlags.Side.SecondaryBeautification) != (CompositionFlags.Side) 0)
          noisePollution *= new float3(0.5f, 0.5f, 0.5f);
        else if ((upgraded.m_Flags.m_Left & CompositionFlags.Side.SecondaryBeautification) != (CompositionFlags.Side) 0)
          noisePollution *= new float3(0.5f, 0.75f, 1f);
        else if ((upgraded.m_Flags.m_Right & CompositionFlags.Side.SecondaryBeautification) != (CompositionFlags.Side) 0)
          noisePollution *= new float3(1f, 0.75f, 0.5f);
        if ((upgraded.m_Flags.m_General & CompositionFlags.General.PrimaryMiddleBeautification) != (CompositionFlags.General) 0)
          noisePollution *= new float3(0.875f, 0.5f, 0.875f);
        if ((upgraded.m_Flags.m_General & CompositionFlags.General.SecondaryMiddleBeautification) == (CompositionFlags.General) 0)
          return;
        noisePollution *= new float3(0.875f, 0.5f, 0.875f);
      }

      private void ApplyPollution(
        float3 position,
        float2 noisePollution,
        float airPollution,
        ref PollutionParameterData pollutionParameters)
      {
        if ((double) airPollution != 0.0)
        {
          short amount = (short) ((double) pollutionParameters.m_NetAirMultiplier * (double) airPollution);
          // ISSUE: reference to a compiler-generated method
          this.AddAirPollution(position, amount);
        }
        if (!math.any(noisePollution != 0.0f))
          return;
        int2 int2 = (int2) (pollutionParameters.m_NetNoiseMultiplier * noisePollution / 8f);
        // ISSUE: reference to a compiler-generated method
        this.AddNoise(position, (short) (4 * int2.y));
        // ISSUE: reference to a compiler-generated method
        this.AddNoise(position + new float3(-pollutionParameters.m_NetNoiseRadius, 0.0f, 0.0f), (short) int2.x);
        // ISSUE: reference to a compiler-generated method
        this.AddNoise(position + new float3(pollutionParameters.m_NetNoiseRadius, 0.0f, 0.0f), (short) int2.x);
        // ISSUE: reference to a compiler-generated method
        this.AddNoise(position + new float3(0.0f, 0.0f, pollutionParameters.m_NetNoiseRadius), (short) int2.x);
        // ISSUE: reference to a compiler-generated method
        this.AddNoise(position + new float3(0.0f, 0.0f, -pollutionParameters.m_NetNoiseRadius), (short) int2.x);
      }

      private void ApplyPollution(
        Curve curve,
        float3 noisePollution,
        float airPollution,
        ref PollutionParameterData pollutionParameters)
      {
        if ((double) airPollution != 0.0)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          float num1 = (float) this.m_MapSize / (float) this.m_AirPollutionTextureSize;
          int num2 = Mathf.CeilToInt(2f * curve.m_Length / num1);
          short amount = (short) ((double) pollutionParameters.m_NetAirMultiplier * (double) airPollution / (double) num2);
          for (int index = 1; index <= num2; ++index)
          {
            // ISSUE: reference to a compiler-generated method
            this.AddAirPollution(MathUtils.Position(curve.m_Bezier, (float) index / ((float) num2 + 1f)), amount);
          }
        }
        if (!math.any(noisePollution != 0.0f))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float num3 = (float) this.m_MapSize / (float) this.m_NoisePollutionTextureSize;
        int num4 = Mathf.CeilToInt(2f * curve.m_Length / num3);
        int3 int3 = (int3) (pollutionParameters.m_NetNoiseMultiplier * noisePollution / (4f * (float) num4));
        for (int index = 1; index <= num4; ++index)
        {
          float t = (float) index / ((float) num4 + 1f);
          float3 position = MathUtils.Position(curve.m_Bezier, t);
          float3 float3_1 = MathUtils.Tangent(curve.m_Bezier, t);
          float3 float3_2 = math.normalize(new float3(-float3_1.z, 0.0f, float3_1.x));
          // ISSUE: reference to a compiler-generated method
          this.AddNoise(position, (short) int3.y);
          if (int3.x != 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.AddNoise(position + pollutionParameters.m_NetNoiseRadius * float3_2, (short) int3.x);
          }
          if (int3.z != 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.AddNoise(position - pollutionParameters.m_NetNoiseRadius * float3_2, (short) int3.z);
          }
        }
      }

      private void AddAirPollution(float3 position, short amount)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int2 cell = CellMapSystem<AirPollution>.GetCell(position, this.m_MapSize, this.m_AirPollutionTextureSize);
        // ISSUE: reference to a compiler-generated field
        if (!math.all(cell >= 0 & cell < this.m_AirPollutionTextureSize))
          return;
        // ISSUE: reference to a compiler-generated field
        int index = cell.x + cell.y * this.m_AirPollutionTextureSize;
        // ISSUE: reference to a compiler-generated field
        AirPollution airPollution = this.m_AirPollutionMap[index];
        airPollution.Add(amount);
        // ISSUE: reference to a compiler-generated field
        this.m_AirPollutionMap[index] = airPollution;
      }

      private void AddNoise(float3 position, short amount)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        float2 cellCoords = CellMapSystem<NoisePollution>.GetCellCoords(position, this.m_MapSize, this.m_NoisePollutionTextureSize);
        float2 float2_1 = math.frac(cellCoords);
        float2 float2_2 = (double) float2_1.x < 0.5 ? new float2(0.0f, 1f) : new float2(1f, 0.0f);
        float2 float2_3 = (double) float2_1.y < 0.5 ? new float2(0.0f, 1f) : new float2(1f, 0.0f);
        int2 cell = new int2(Mathf.FloorToInt(cellCoords.x - float2_2.y), Mathf.FloorToInt(cellCoords.y - float2_3.y));
        // ISSUE: reference to a compiler-generated method
        this.AddNoiseSingle(cell, (short) ((0.5 + (double) float2_2.x - (double) float2_1.x) * (0.5 + (double) float2_3.x - (double) float2_1.y) * (double) amount));
        ++cell.x;
        // ISSUE: reference to a compiler-generated method
        this.AddNoiseSingle(cell, (short) (((double) float2_2.y - 0.5 + (double) float2_1.x) * (0.5 + (double) float2_3.x - (double) float2_1.y) * (double) amount));
        ++cell.y;
        // ISSUE: reference to a compiler-generated method
        this.AddNoiseSingle(cell, (short) (((double) float2_2.y - 0.5 + (double) float2_1.x) * ((double) float2_3.y - 0.5 + (double) float2_1.y) * (double) amount));
        --cell.x;
        // ISSUE: reference to a compiler-generated method
        this.AddNoiseSingle(cell, (short) ((0.5 + (double) float2_2.x - (double) float2_1.x) * ((double) float2_3.y - 0.5 + (double) float2_1.y) * (double) amount));
      }

      private void AddNoiseSingle(int2 cell, short amount)
      {
        // ISSUE: reference to a compiler-generated field
        if (!math.all(cell >= 0 & cell < this.m_NoisePollutionTextureSize))
          return;
        // ISSUE: reference to a compiler-generated field
        int index = cell.x + cell.y * this.m_NoisePollutionTextureSize;
        // ISSUE: reference to a compiler-generated field
        NoisePollution noisePollution = this.m_NoisePollutionMap[index];
        noisePollution.Add(amount);
        // ISSUE: reference to a compiler-generated field
        this.m_NoisePollutionMap[index] = noisePollution;
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
      public ComponentTypeHandle<Game.Net.Node> __Game_Net_Node_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Upgraded> __Game_Net_Upgraded_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Elevation> __Game_Net_Elevation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Composition> __Game_Net_Composition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<ConnectedEdge> __Game_Net_ConnectedEdge_RO_BufferTypeHandle;
      public ComponentTypeHandle<Game.Net.Pollution> __Game_Net_Pollution_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Net.Edge> __Game_Net_Edge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Upgraded> __Game_Net_Upgraded_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Composition> __Game_Net_Composition_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetPollutionData> __Game_Prefabs_NetPollutionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<NetCompositionData> __Game_Prefabs_NetCompositionData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Node_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Upgraded_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Upgraded>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Elevation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ConnectedEdge_RO_BufferTypeHandle = state.GetBufferTypeHandle<ConnectedEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Pollution_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Net.Pollution>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Edge_RO_ComponentLookup = state.GetComponentLookup<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Upgraded_RO_ComponentLookup = state.GetComponentLookup<Upgraded>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentLookup = state.GetComponentLookup<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetPollutionData_RO_ComponentLookup = state.GetComponentLookup<NetPollutionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NetCompositionData_RO_ComponentLookup = state.GetComponentLookup<NetCompositionData>(true);
      }
    }
  }
}
