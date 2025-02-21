// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.TerrainToolTooltipSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Game.UI.Localization;
using Game.UI.Widgets;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Tooltip
{
  [CompilerGenerated]
  public class TerrainToolTooltipSystem : TooltipSystemBase
  {
    private ToolSystem m_ToolSystem;
    private TerrainToolSystem m_TerrainTool;
    private ToolRaycastSystem m_ToolRaycastSystem;
    private GroundWaterSystem m_GroundWaterSystem;
    private EntityQuery m_ParameterQuery;
    private IntTooltip m_GroundwaterVolume;
    private NativeReference<TempWaterPumpingTooltipSystem.GroundWaterReservoirResult> m_ReservoirResult;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainTool = this.World.GetOrCreateSystemManaged<TerrainToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolRaycastSystem = this.World.GetOrCreateSystemManaged<ToolRaycastSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_GroundWaterSystem = this.World.GetOrCreateSystemManaged<GroundWaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<WaterPipeParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ParameterQuery);
      IntTooltip intTooltip = new IntTooltip();
      intTooltip.path = (PathSegment) "groundWaterCapacity";
      intTooltip.icon = "Media/Game/Icons/Water.svg";
      intTooltip.label = LocalizedString.Id("Tools.GROUNDWATER_VOLUME");
      intTooltip.unit = "volume";
      // ISSUE: reference to a compiler-generated field
      this.m_GroundwaterVolume = intTooltip;
      // ISSUE: reference to a compiler-generated field
      this.m_ReservoirResult = new NativeReference<TempWaterPumpingTooltipSystem.GroundWaterReservoirResult>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ReservoirResult.Dispose();
      base.OnDestroy();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      RaycastResult result;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.activeTool == this.m_TerrainTool && (Object) this.m_TerrainTool.prefab != (Object) null && this.m_TerrainTool.prefab.m_Target == TerraformingTarget.GroundWater && this.m_ToolRaycastSystem.GetRaycastResult(out result))
      {
        // ISSUE: reference to a compiler-generated method
        this.ProcessResults();
        // ISSUE: reference to a compiler-generated field
        ref NativeReference<TempWaterPumpingTooltipSystem.GroundWaterReservoirResult> local = ref this.m_ReservoirResult;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TempWaterPumpingTooltipSystem.GroundWaterReservoirResult waterReservoirResult1 = new TempWaterPumpingTooltipSystem.GroundWaterReservoirResult();
        // ISSUE: variable of a compiler-generated type
        TempWaterPumpingTooltipSystem.GroundWaterReservoirResult waterReservoirResult2 = waterReservoirResult1;
        local.Value = waterReservoirResult2;
        int2 cell;
        // ISSUE: reference to a compiler-generated method
        if (!GroundWaterSystem.TryGetCell(result.m_Hit.m_HitPosition, out cell))
          return;
        JobHandle dependencies;
        // ISSUE: reference to a compiler-generated field
        NativeArray<GroundWater> map = this.m_GroundWaterSystem.GetMap(true, out dependencies);
        NativeList<int2> nativeList = new NativeList<int2>(1, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
        nativeList.Add(in cell);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TempWaterPumpingTooltipSystem.GroundWaterReservoirJob jobData = new TempWaterPumpingTooltipSystem.GroundWaterReservoirJob()
        {
          m_GroundWaterMap = map,
          m_PumpCapacityMap = new NativeParallelHashMap<int2, int>(0, (AllocatorManager.AllocatorHandle) Allocator.TempJob),
          m_TempGroundWaterPumpCells = nativeList,
          m_Queue = new NativeQueue<int2>((AllocatorManager.AllocatorHandle) Allocator.TempJob),
          m_Result = this.m_ReservoirResult
        };
        this.Dependency = jobData.Schedule<TempWaterPumpingTooltipSystem.GroundWaterReservoirJob>(JobHandle.CombineDependencies(this.Dependency, dependencies));
        // ISSUE: reference to a compiler-generated field
        jobData.m_Queue.Dispose(this.Dependency);
        // ISSUE: reference to a compiler-generated field
        jobData.m_PumpCapacityMap.Dispose(this.Dependency);
        nativeList.Dispose(this.Dependency);
        // ISSUE: reference to a compiler-generated field
        this.m_GroundWaterSystem.AddReader(this.Dependency);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        ref NativeReference<TempWaterPumpingTooltipSystem.GroundWaterReservoirResult> local = ref this.m_ReservoirResult;
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        TempWaterPumpingTooltipSystem.GroundWaterReservoirResult waterReservoirResult3 = new TempWaterPumpingTooltipSystem.GroundWaterReservoirResult();
        // ISSUE: variable of a compiler-generated type
        TempWaterPumpingTooltipSystem.GroundWaterReservoirResult waterReservoirResult4 = waterReservoirResult3;
        local.Value = waterReservoirResult4;
      }
    }

    private void ProcessResults()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      TempWaterPumpingTooltipSystem.GroundWaterReservoirResult waterReservoirResult = this.m_ReservoirResult.Value;
      // ISSUE: reference to a compiler-generated field
      if (waterReservoirResult.m_Volume <= 0)
        return;
      // ISSUE: reference to a compiler-generated field
      WaterPipeParameterData singleton = this.m_ParameterQuery.GetSingleton<WaterPipeParameterData>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_GroundwaterVolume.value = Mathf.RoundToInt(singleton.m_GroundwaterReplenish / singleton.m_GroundwaterUsageMultiplier * (float) waterReservoirResult.m_Volume);
      // ISSUE: reference to a compiler-generated field
      if (this.m_GroundwaterVolume.value <= 0)
        return;
      // ISSUE: reference to a compiler-generated field
      this.AddMouseTooltip((IWidget) this.m_GroundwaterVolume);
    }

    [Preserve]
    public TerrainToolTooltipSystem()
    {
    }
  }
}
