// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TelecomEfficiencySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Buildings;
using Game.Common;
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
  public class TelecomEfficiencySystem : GameSystemBase
  {
    private const int kUpdatesPerDay = 512;
    private SimulationSystem m_SimulationSystem;
    private TelecomCoverageSystem m_TelecomCoverageSystem;
    private EntityQuery m_BuildingQuery;
    private TelecomEfficiencySystem.TypeHandle __TypeHandle;
    private EntityQuery __query_450882671_0;
    private EntityQuery __query_450882671_1;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 32;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TelecomCoverageSystem = this.World.GetOrCreateSystemManaged<TelecomCoverageSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<TelecomConsumer>(), ComponentType.ReadWrite<Efficiency>(), ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadOnly<Transform>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_BuildingQuery);
      this.RequireForUpdate<TelecomParameterData>();
      this.RequireForUpdate<BuildingEfficiencyParameterData>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.EntityManager.HasEnabledComponent<Locked>(this.__query_450882671_0.GetSingleton<TelecomParameterData>().m_TelecomServicePrefab))
        return;
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, 512, 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ConsumptionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
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
      // ISSUE: variable of a compiler-generated type
      TelecomEfficiencySystem.TelecomEfficiencyJob jobData = new TelecomEfficiencySystem.TelecomEfficiencyJob()
      {
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferTypeHandle,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ConsumptionDatas = this.__TypeHandle.__Game_Prefabs_ConsumptionData_RO_ComponentLookup,
        m_TelecomCoverage = this.m_TelecomCoverageSystem.GetData(true, out dependencies),
        m_EfficiencyParameters = this.__query_450882671_1.GetSingleton<BuildingEfficiencyParameterData>(),
        m_UpdateFrameIndex = updateFrame
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<TelecomEfficiencySystem.TelecomEfficiencyJob>(this.m_BuildingQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated field
      this.m_TelecomCoverageSystem.AddReader(this.Dependency);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_450882671_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<TelecomParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_450882671_1 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<BuildingEfficiencyParameterData>()
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
    public TelecomEfficiencySystem()
    {
    }

    [BurstCompile]
    private struct TelecomEfficiencyJob : IJobChunk
    {
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      public BufferTypeHandle<Efficiency> m_EfficiencyType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<ConsumptionData> m_ConsumptionDatas;
      [ReadOnly]
      public CellMapData<TelecomCoverage> m_TelecomCoverage;
      public BuildingEfficiencyParameterData m_EfficiencyParameters;
      public uint m_UpdateFrameIndex;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if ((int) chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index != (int) this.m_UpdateFrameIndex)
          return;
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray1 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray2 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor1 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor2 = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          ConsumptionData componentData;
          // ISSUE: reference to a compiler-generated field
          this.m_ConsumptionDatas.TryGetComponent((Entity) nativeArray1[index], out componentData);
          if (bufferAccessor1.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            UpgradeUtils.CombineStats<ConsumptionData>(ref componentData, bufferAccessor1[index], ref this.m_Prefabs, ref this.m_ConsumptionDatas);
          }
          // ISSUE: reference to a compiler-generated method
          float telecomEfficiency = this.GetTelecomEfficiency(nativeArray2[index].m_Position, componentData.m_TelecomNeed);
          BuildingUtils.SetEfficiencyFactor(bufferAccessor2[index], EfficiencyFactor.Telecom, telecomEfficiency);
        }
      }

      private float GetTelecomEfficiency(float3 position, float telecomNeed)
      {
        // ISSUE: reference to a compiler-generated field
        float num1 = TelecomCoverage.SampleNetworkQuality(this.m_TelecomCoverage, position);
        // ISSUE: reference to a compiler-generated field
        float telecomBaseline = this.m_EfficiencyParameters.m_TelecomBaseline;
        float num2 = 0.0f;
        if ((double) num1 < (double) telecomBaseline)
        {
          double num3 = 1.0 - (double) num1 / (double) telecomBaseline;
          num2 = (float) (num3 * num3 * -0.0099999997764825821) * telecomNeed;
        }
        return 1f + num2;
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
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ConsumptionData> __Game_Prefabs_ConsumptionData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RW_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ConsumptionData_RO_ComponentLookup = state.GetComponentLookup<ConsumptionData>(true);
      }
    }
  }
}
