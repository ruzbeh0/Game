// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.TempWaterPumpingTooltipSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.Common;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Game.UI.Localization;
using Game.UI.Widgets;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.UI.Tooltip
{
  [CompilerGenerated]
  public class TempWaterPumpingTooltipSystem : TooltipSystemBase
  {
    private GroundWaterSystem m_GroundWaterSystem;
    private WaterSystem m_WaterSystem;
    private TerrainSystem m_TerrainSystem;
    private EntityQuery m_ErrorQuery;
    private EntityQuery m_TempQuery;
    private EntityQuery m_PumpQuery;
    private EntityQuery m_ParameterQuery;
    private ProgressTooltip m_Capacity;
    private IntTooltip m_ReservoirUsage;
    private StringTooltip m_OverRefreshCapacityWarning;
    private StringTooltip m_AvailabilityWarning;
    private LocalizedString m_GroundWarning;
    private LocalizedString m_SurfaceWarning;
    private NativeReference<TempWaterPumpingTooltipSystem.TempResult> m_TempResult;
    private NativeReference<TempWaterPumpingTooltipSystem.GroundWaterReservoirResult> m_ReservoirResult;
    private TempWaterPumpingTooltipSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_GroundWaterSystem = this.World.GetOrCreateSystemManaged<GroundWaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ErrorQuery = this.GetEntityQuery(ComponentType.ReadOnly<Temp>(), ComponentType.ReadOnly<Error>());
      // ISSUE: reference to a compiler-generated field
      this.m_TempQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.WaterPumpingStation>(), ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<Game.Objects.Transform>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Temp>(), ComponentType.Exclude<Hidden>(), ComponentType.Exclude<Error>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_PumpQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.WaterPumpingStation>(), ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<Game.Objects.Transform>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Hidden>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_ParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<WaterPipeParameterData>());
      ProgressTooltip progressTooltip = new ProgressTooltip();
      progressTooltip.path = (PathSegment) "groundWaterCapacity";
      progressTooltip.icon = "Media/Game/Icons/Water.svg";
      progressTooltip.label = LocalizedString.Id("Tools.WATER_OUTPUT_LABEL");
      progressTooltip.unit = "volume";
      progressTooltip.omitMax = true;
      // ISSUE: reference to a compiler-generated field
      this.m_Capacity = progressTooltip;
      IntTooltip intTooltip = new IntTooltip();
      intTooltip.path = (PathSegment) "groundWaterReservoirUsage";
      intTooltip.label = LocalizedString.Id("Tools.GROUND_WATER_RESERVOIR_USAGE");
      intTooltip.unit = "percentage";
      // ISSUE: reference to a compiler-generated field
      this.m_ReservoirUsage = intTooltip;
      StringTooltip stringTooltip1 = new StringTooltip();
      stringTooltip1.path = (PathSegment) "groundWaterOverRefreshCapacityWarning";
      stringTooltip1.value = LocalizedString.Id("Tools.WARNING[OverRefreshCapacity]");
      stringTooltip1.color = TooltipColor.Warning;
      // ISSUE: reference to a compiler-generated field
      this.m_OverRefreshCapacityWarning = stringTooltip1;
      StringTooltip stringTooltip2 = new StringTooltip();
      stringTooltip2.path = (PathSegment) "waterAvailabilityWarning";
      stringTooltip2.color = TooltipColor.Warning;
      // ISSUE: reference to a compiler-generated field
      this.m_AvailabilityWarning = stringTooltip2;
      // ISSUE: reference to a compiler-generated field
      this.m_GroundWarning = LocalizedString.Id("Tools.WARNING[NotEnoughGroundWater]");
      // ISSUE: reference to a compiler-generated field
      this.m_SurfaceWarning = LocalizedString.Id("Tools.WARNING[NotEnoughFreshWater]");
      // ISSUE: reference to a compiler-generated field
      this.m_TempResult = new NativeReference<TempWaterPumpingTooltipSystem.TempResult>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ReservoirResult = new NativeReference<TempWaterPumpingTooltipSystem.GroundWaterReservoirResult>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_TempResult.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_ReservoirResult.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ErrorQuery.IsEmptyIgnoreFilter || this.m_TempQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        ref NativeReference<TempWaterPumpingTooltipSystem.TempResult> local1 = ref this.m_TempResult;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TempWaterPumpingTooltipSystem.TempResult tempResult1 = new TempWaterPumpingTooltipSystem.TempResult();
        // ISSUE: variable of a compiler-generated type
        TempWaterPumpingTooltipSystem.TempResult tempResult2 = tempResult1;
        local1.Value = tempResult2;
        // ISSUE: reference to a compiler-generated field
        ref NativeReference<TempWaterPumpingTooltipSystem.GroundWaterReservoirResult> local2 = ref this.m_ReservoirResult;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TempWaterPumpingTooltipSystem.GroundWaterReservoirResult waterReservoirResult1 = new TempWaterPumpingTooltipSystem.GroundWaterReservoirResult();
        // ISSUE: variable of a compiler-generated type
        TempWaterPumpingTooltipSystem.GroundWaterReservoirResult waterReservoirResult2 = waterReservoirResult1;
        local2.Value = waterReservoirResult2;
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.ProcessResults();
        // ISSUE: reference to a compiler-generated field
        ref NativeReference<TempWaterPumpingTooltipSystem.TempResult> local3 = ref this.m_TempResult;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TempWaterPumpingTooltipSystem.TempResult tempResult3 = new TempWaterPumpingTooltipSystem.TempResult();
        // ISSUE: variable of a compiler-generated type
        TempWaterPumpingTooltipSystem.TempResult tempResult4 = tempResult3;
        local3.Value = tempResult4;
        // ISSUE: reference to a compiler-generated field
        ref NativeReference<TempWaterPumpingTooltipSystem.GroundWaterReservoirResult> local4 = ref this.m_ReservoirResult;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TempWaterPumpingTooltipSystem.GroundWaterReservoirResult waterReservoirResult3 = new TempWaterPumpingTooltipSystem.GroundWaterReservoirResult();
        // ISSUE: variable of a compiler-generated type
        TempWaterPumpingTooltipSystem.GroundWaterReservoirResult waterReservoirResult4 = waterReservoirResult3;
        local4.Value = waterReservoirResult4;
        JobHandle dependencies;
        // ISSUE: reference to a compiler-generated field
        NativeArray<GroundWater> map = this.m_GroundWaterSystem.GetMap(true, out dependencies);
        // ISSUE: reference to a compiler-generated field
        WaterPipeParameterData singleton = this.m_ParameterQuery.GetSingleton<WaterPipeParameterData>();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Simulation_WaterSourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_WaterPumpingStationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated field
        JobHandle jobHandle1 = new TempWaterPumpingTooltipSystem.TempJob()
        {
          m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
          m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
          m_SubObjectType = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle,
          m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
          m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_PumpDatas = this.__TypeHandle.__Game_Prefabs_WaterPumpingStationData_RO_ComponentLookup,
          m_Transforms = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
          m_WaterSources = this.__TypeHandle.__Game_Simulation_WaterSourceData_RO_ComponentLookup,
          m_GroundWaterMap = map,
          m_WaterSurfaceData = this.m_WaterSystem.GetSurfaceData(out deps),
          m_TerrainHeightData = this.m_TerrainSystem.GetHeightData(),
          m_Result = this.m_TempResult,
          m_Parameters = singleton
        }.Schedule<TempWaterPumpingTooltipSystem.TempJob>(this.m_TempQuery, JobHandle.CombineDependencies(this.Dependency, dependencies, deps));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_WaterSystem.AddSurfaceReader(jobHandle1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_TerrainSystem.AddCPUHeightReader(jobHandle1);
        NativeParallelHashMap<int2, int> nativeParallelHashMap = new NativeParallelHashMap<int2, int>(8, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
        NativeList<int2> nativeList = new NativeList<int2>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_WaterPumpingStationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        JobHandle dependsOn = new TempWaterPumpingTooltipSystem.GroundWaterPumpJob()
        {
          m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
          m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
          m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
          m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
          m_PumpDatas = this.__TypeHandle.__Game_Prefabs_WaterPumpingStationData_RO_ComponentLookup,
          m_GroundWaterMap = map,
          m_PumpCapacityMap = nativeParallelHashMap,
          m_TempGroundWaterPumpCells = nativeList,
          m_Parameters = singleton
        }.Schedule<TempWaterPumpingTooltipSystem.GroundWaterPumpJob>(this.m_PumpQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TempWaterPumpingTooltipSystem.GroundWaterReservoirJob jobData = new TempWaterPumpingTooltipSystem.GroundWaterReservoirJob()
        {
          m_GroundWaterMap = map,
          m_PumpCapacityMap = nativeParallelHashMap,
          m_TempGroundWaterPumpCells = nativeList,
          m_Queue = new NativeQueue<int2>((AllocatorManager.AllocatorHandle) Allocator.TempJob),
          m_Result = this.m_ReservoirResult
        };
        JobHandle jobHandle2 = jobData.Schedule<TempWaterPumpingTooltipSystem.GroundWaterReservoirJob>(dependsOn);
        // ISSUE: reference to a compiler-generated field
        jobData.m_Queue.Dispose(jobHandle2);
        nativeParallelHashMap.Dispose(jobHandle2);
        nativeList.Dispose(jobHandle2);
        this.Dependency = JobHandle.CombineDependencies(jobHandle1, jobHandle2);
        // ISSUE: reference to a compiler-generated field
        this.m_GroundWaterSystem.AddReader(this.Dependency);
      }
    }

    private void ProcessResults()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      TempWaterPumpingTooltipSystem.TempResult temp = this.m_TempResult.Value;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      TempWaterPumpingTooltipSystem.GroundWaterReservoirResult reservoir = this.m_ReservoirResult.Value;
      // ISSUE: reference to a compiler-generated field
      if (temp.m_MaxCapacity <= 0)
        return;
      // ISSUE: reference to a compiler-generated field
      if ((temp.m_Types & AllowedWaterTypes.Groundwater) != AllowedWaterTypes.None)
      {
        // ISSUE: reference to a compiler-generated method
        this.ProcessProduction(temp);
        // ISSUE: reference to a compiler-generated field
        if (reservoir.m_Volume > 0)
        {
          // ISSUE: reference to a compiler-generated method
          this.ProcessReservoir(reservoir);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.ProcessAvailabilityWarning(temp, this.m_GroundWarning);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        if ((temp.m_Types & AllowedWaterTypes.SurfaceWater) != AllowedWaterTypes.None)
        {
          // ISSUE: reference to a compiler-generated method
          this.ProcessProduction(temp);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.ProcessAvailabilityWarning(temp, this.m_SurfaceWarning);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.ProcessProduction(temp);
        }
      }
    }

    private void ProcessReservoir(
      TempWaterPumpingTooltipSystem.GroundWaterReservoirResult reservoir)
    {
      // ISSUE: reference to a compiler-generated field
      WaterPipeParameterData singleton = this.m_ParameterQuery.GetSingleton<WaterPipeParameterData>();
      // ISSUE: reference to a compiler-generated field
      float num = singleton.m_GroundwaterReplenish / singleton.m_GroundwaterUsageMultiplier * (float) reservoir.m_Volume;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      float f = (double) num <= 0.0 || reservoir.m_PumpCapacity <= 0 ? 0.0f : math.clamp(100f * (float) reservoir.m_PumpCapacity / num, 1f, 999f);
      // ISSUE: reference to a compiler-generated field
      this.m_ReservoirUsage.value = Mathf.RoundToInt(f);
      // ISSUE: reference to a compiler-generated field
      this.m_ReservoirUsage.color = (double) f > 100.0 ? TooltipColor.Warning : TooltipColor.Info;
      // ISSUE: reference to a compiler-generated field
      this.AddMouseTooltip((IWidget) this.m_ReservoirUsage);
      if ((double) f <= 100.0)
        return;
      // ISSUE: reference to a compiler-generated field
      this.AddMouseTooltip((IWidget) this.m_OverRefreshCapacityWarning);
    }

    private void ProcessProduction(TempWaterPumpingTooltipSystem.TempResult temp)
    {
      // ISSUE: reference to a compiler-generated field
      if (temp.m_Production <= 0)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Capacity.value = (float) temp.m_Production;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Capacity.max = (float) temp.m_MaxCapacity;
      // ISSUE: reference to a compiler-generated field
      ProgressTooltip.SetCapacityColor(this.m_Capacity);
      // ISSUE: reference to a compiler-generated field
      this.AddMouseTooltip((IWidget) this.m_Capacity);
    }

    private void ProcessAvailabilityWarning(
      TempWaterPumpingTooltipSystem.TempResult temp,
      LocalizedString warningText)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (temp.m_Production <= 0 || (double) temp.m_Production >= (double) temp.m_MaxCapacity * 0.75)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_AvailabilityWarning.value = warningText;
      // ISSUE: reference to a compiler-generated field
      this.AddMouseTooltip((IWidget) this.m_AvailabilityWarning);
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
    public TempWaterPumpingTooltipSystem()
    {
    }

    private struct TempResult
    {
      public AllowedWaterTypes m_Types;
      public int m_Production;
      public int m_MaxCapacity;
    }

    [BurstCompile]
    private struct TempJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> m_TransformType;
      [ReadOnly]
      public BufferTypeHandle<Game.Objects.SubObject> m_SubObjectType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<WaterPumpingStationData> m_PumpDatas;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_Transforms;
      [ReadOnly]
      public ComponentLookup<Game.Simulation.WaterSourceData> m_WaterSources;
      [ReadOnly]
      public NativeArray<GroundWater> m_GroundWaterMap;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      [ReadOnly]
      public TerrainHeightData m_TerrainHeightData;
      public NativeReference<TempWaterPumpingTooltipSystem.TempResult> m_Result;
      public WaterPipeParameterData m_Parameters;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        ref TempWaterPumpingTooltipSystem.TempResult local = ref this.m_Result.ValueAsRef<TempWaterPumpingTooltipSystem.TempResult>();
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray1 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Objects.Transform> nativeArray3 = chunk.GetNativeArray<Game.Objects.Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Objects.SubObject> bufferAccessor1 = chunk.GetBufferAccessor<Game.Objects.SubObject>(ref this.m_SubObjectType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor2 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          if ((nativeArray2[index1].m_Flags & (TempFlags.Create | TempFlags.Modify | TempFlags.Upgrade)) != (TempFlags) 0)
          {
            WaterPumpingStationData componentData1;
            // ISSUE: reference to a compiler-generated field
            this.m_PumpDatas.TryGetComponent(nativeArray1[index1].m_Prefab, out componentData1);
            if (bufferAccessor2.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              UpgradeUtils.CombineStats<WaterPumpingStationData>(ref componentData1, bufferAccessor2[index1], ref this.m_Prefabs, ref this.m_PumpDatas);
            }
            int x = 0;
            if (componentData1.m_Types != AllowedWaterTypes.None)
            {
              if ((componentData1.m_Types & AllowedWaterTypes.Groundwater) != AllowedWaterTypes.None)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                // ISSUE: reference to a compiler-generated field
                int num = Mathf.RoundToInt(math.clamp((float) GroundWaterSystem.GetGroundWater(nativeArray3[index1].m_Position, this.m_GroundWaterMap).m_Max / this.m_Parameters.m_GroundwaterPumpEffectiveAmount, 0.0f, 1f) * (float) componentData1.m_Capacity);
                x += num;
              }
              if ((componentData1.m_Types & AllowedWaterTypes.SurfaceWater) != AllowedWaterTypes.None && bufferAccessor1.Length != 0)
              {
                DynamicBuffer<Game.Objects.SubObject> dynamicBuffer = bufferAccessor1[index1];
                for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
                {
                  Entity subObject = dynamicBuffer[index2].m_SubObject;
                  Game.Objects.Transform componentData2;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_WaterSources.HasComponent(subObject) && this.m_Transforms.TryGetComponent(subObject, out componentData2))
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    float waterAvailability = WaterPumpingStationAISystem.GetSurfaceWaterAvailability(componentData2.m_Position, componentData1.m_Types, this.m_WaterSurfaceData, this.m_Parameters.m_SurfaceWaterPumpEffectiveDepth);
                    x += Mathf.RoundToInt(waterAvailability * (float) componentData1.m_Capacity);
                  }
                }
              }
            }
            else
              x = componentData1.m_Capacity;
            // ISSUE: reference to a compiler-generated field
            local.m_Types |= componentData1.m_Types;
            // ISSUE: reference to a compiler-generated field
            local.m_Production += math.min(x, componentData1.m_Capacity);
            // ISSUE: reference to a compiler-generated field
            local.m_MaxCapacity += componentData1.m_Capacity;
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

    [BurstCompile]
    private struct GroundWaterPumpJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> m_TransformType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<WaterPumpingStationData> m_PumpDatas;
      [ReadOnly]
      public NativeArray<GroundWater> m_GroundWaterMap;
      public NativeParallelHashMap<int2, int> m_PumpCapacityMap;
      public NativeList<int2> m_TempGroundWaterPumpCells;
      public WaterPipeParameterData m_Parameters;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray1 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Objects.Transform> nativeArray3 = chunk.GetNativeArray<Game.Objects.Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        bool flag = nativeArray2.Length != 0;
        for (int index = 0; index < chunk.Count; ++index)
        {
          if (!flag || (nativeArray2[index].m_Flags & (TempFlags.Create | TempFlags.Modify | TempFlags.Upgrade)) != (TempFlags) 0)
          {
            WaterPumpingStationData componentData;
            // ISSUE: reference to a compiler-generated field
            this.m_PumpDatas.TryGetComponent(nativeArray1[index].m_Prefab, out componentData);
            if (bufferAccessor.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              UpgradeUtils.CombineStats<WaterPumpingStationData>(ref componentData, bufferAccessor[index], ref this.m_Prefabs, ref this.m_PumpDatas);
            }
            int2 cell;
            // ISSUE: reference to a compiler-generated method
            if ((componentData.m_Types & AllowedWaterTypes.Groundwater) != AllowedWaterTypes.None && GroundWaterSystem.TryGetCell(nativeArray3[index].m_Position, out cell))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated field
              int num = Mathf.CeilToInt(math.clamp((float) GroundWaterSystem.GetGroundWater(nativeArray3[index].m_Position, this.m_GroundWaterMap).m_Max / this.m_Parameters.m_GroundwaterPumpEffectiveAmount, 0.0f, 1f) * (float) componentData.m_Capacity);
              // ISSUE: reference to a compiler-generated field
              if (!this.m_PumpCapacityMap.ContainsKey(cell))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_PumpCapacityMap.Add(cell, num);
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_PumpCapacityMap[cell] += num;
              }
              if (flag)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_TempGroundWaterPumpCells.Add(in cell);
              }
            }
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

    public struct GroundWaterReservoirResult
    {
      public int m_PumpCapacity;
      public int m_Volume;
    }

    [BurstCompile]
    public struct GroundWaterReservoirJob : IJob
    {
      [ReadOnly]
      public NativeArray<GroundWater> m_GroundWaterMap;
      [ReadOnly]
      public NativeParallelHashMap<int2, int> m_PumpCapacityMap;
      [ReadOnly]
      public NativeList<int2> m_TempGroundWaterPumpCells;
      public NativeQueue<int2> m_Queue;
      public NativeReference<TempWaterPumpingTooltipSystem.GroundWaterReservoirResult> m_Result;

      public void Execute()
      {
        NativeParallelHashSet<int2> processedCells = new NativeParallelHashSet<int2>(128, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        ref TempWaterPumpingTooltipSystem.GroundWaterReservoirResult local = ref this.m_Result.ValueAsRef<TempWaterPumpingTooltipSystem.GroundWaterReservoirResult>();
        // ISSUE: reference to a compiler-generated field
        foreach (int2 groundWaterPumpCell in this.m_TempGroundWaterPumpCells)
        {
          // ISSUE: reference to a compiler-generated method
          this.EnqueueIfUnprocessed(groundWaterPumpCell, processedCells);
        }
        int2 key;
        // ISSUE: reference to a compiler-generated field
        while (this.m_Queue.TryDequeue(out key))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          GroundWater groundWater = this.m_GroundWaterMap[key.x + key.y * GroundWaterSystem.kTextureSize];
          int num;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PumpCapacityMap.TryGetValue(key, out num))
          {
            // ISSUE: reference to a compiler-generated field
            local.m_PumpCapacity += num;
          }
          if (groundWater.m_Max > (short) 500)
          {
            // ISSUE: reference to a compiler-generated field
            local.m_Volume += (int) groundWater.m_Max;
            // ISSUE: reference to a compiler-generated method
            this.EnqueueIfUnprocessed(new int2(key.x - 1, key.y), processedCells);
            // ISSUE: reference to a compiler-generated method
            this.EnqueueIfUnprocessed(new int2(key.x + 1, key.y), processedCells);
            // ISSUE: reference to a compiler-generated method
            this.EnqueueIfUnprocessed(new int2(key.x, key.y - 1), processedCells);
            // ISSUE: reference to a compiler-generated method
            this.EnqueueIfUnprocessed(new int2(key.x, key.y + 2), processedCells);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (local.m_Volume > 0)
            {
              // ISSUE: reference to a compiler-generated field
              local.m_Volume += (int) groundWater.m_Max;
            }
          }
        }
      }

      private void EnqueueIfUnprocessed(int2 cell, NativeParallelHashSet<int2> processedCells)
      {
        // ISSUE: reference to a compiler-generated method
        if (!GroundWaterSystem.IsValidCell(cell) || !processedCells.Add(cell))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_Queue.Enqueue(cell);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterPumpingStationData> __Game_Prefabs_WaterPumpingStationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Simulation.WaterSourceData> __Game_Simulation_WaterSourceData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WaterPumpingStationData_RO_ComponentLookup = state.GetComponentLookup<WaterPumpingStationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterSourceData_RO_ComponentLookup = state.GetComponentLookup<Game.Simulation.WaterSourceData>(true);
      }
    }
  }
}
