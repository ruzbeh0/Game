// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.LandValueTooltipSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using Game.Debug;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Game.UI.Localization;
using Game.UI.Widgets;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.UI.Tooltip
{
  [CompilerGenerated]
  public class LandValueTooltipSystem : TooltipSystemBase
  {
    private ToolRaycastSystem m_ToolRaycastSystem;
    private ToolSystem m_ToolSystem;
    private TerrainToolSystem m_TerrainToolSystem;
    private LandValueSystem m_LandValueSystem;
    private LandValueDebugSystem m_LandValueDebugSystem;
    private TerrainAttractivenessSystem m_TerrainAttractivenessSystem;
    private TerrainSystem m_TerrainSystem;
    private PrefabSystem m_PrefabSystem;
    private GroundPollutionSystem m_GroundPollutionSystem;
    private AirPollutionSystem m_AirPollutionSystem;
    private NoisePollutionSystem m_NoisePollutionSystem;
    private EntityQuery m_AttractivenessParameterQuery;
    private EntityQuery m_LandValueParameterQuery;
    private FloatTooltip m_LandValueTooltip;
    private FloatTooltip m_TerrainAttractiveTooltip;
    private FloatTooltip m_AirPollutionTooltip;
    private FloatTooltip m_GroundPollutionTooltip;
    private FloatTooltip m_NoisePollutionTooltip;
    private NativeValue<float> m_LandValueResult;
    private NativeValue<float> m_TerrainAttractiveResult;
    private NativeValue<float> m_AirPollutionResult;
    private NativeValue<float> m_NoisePollutionResult;
    private NativeValue<float> m_GroundPollutionResult;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainToolSystem = this.World.GetOrCreateSystemManaged<TerrainToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolRaycastSystem = this.World.GetOrCreateSystemManaged<ToolRaycastSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LandValueSystem = this.World.GetOrCreateSystemManaged<LandValueSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LandValueDebugSystem = this.World.GetOrCreateSystemManaged<LandValueDebugSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainAttractivenessSystem = this.World.GetOrCreateSystemManaged<TerrainAttractivenessSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_GroundPollutionSystem = this.World.GetOrCreateSystemManaged<GroundPollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AirPollutionSystem = this.World.GetOrCreateSystemManaged<AirPollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NoisePollutionSystem = this.World.GetOrCreateSystemManaged<NoisePollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AttractivenessParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<AttractivenessParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_LandValueParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<LandValueParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_AttractivenessParameterQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_LandValueParameterQuery);
      FloatTooltip floatTooltip1 = new FloatTooltip();
      floatTooltip1.path = (PathSegment) "LandValue";
      floatTooltip1.icon = "Media/Game/Icons/LandValue.svg";
      floatTooltip1.label = LocalizedString.Id("Infoviews.INFOVIEW[LandValue]");
      floatTooltip1.unit = "money";
      // ISSUE: reference to a compiler-generated field
      this.m_LandValueTooltip = floatTooltip1;
      FloatTooltip floatTooltip2 = new FloatTooltip();
      floatTooltip2.path = (PathSegment) "TerrainAttractive";
      floatTooltip2.icon = "Media/Game/Icons/Tourism.svg";
      floatTooltip2.label = LocalizedString.Id("Properties.CITY_MODIFIER[Attractiveness]");
      floatTooltip2.unit = "integer";
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainAttractiveTooltip = floatTooltip2;
      FloatTooltip floatTooltip3 = new FloatTooltip();
      floatTooltip3.path = (PathSegment) "AirPollution";
      floatTooltip3.icon = "Media/Game/Icons/AirPollution.svg";
      floatTooltip3.label = LocalizedString.Id("Infoviews.INFOVIEW[AirPollution]");
      floatTooltip3.unit = "integer";
      // ISSUE: reference to a compiler-generated field
      this.m_AirPollutionTooltip = floatTooltip3;
      FloatTooltip floatTooltip4 = new FloatTooltip();
      floatTooltip4.path = (PathSegment) "GroundPollution";
      floatTooltip4.icon = "Media/Game/Icons/GroundPollution.svg";
      floatTooltip4.label = LocalizedString.Id("Infoviews.INFOVIEW[GroundPollution]");
      floatTooltip4.unit = "integer";
      // ISSUE: reference to a compiler-generated field
      this.m_GroundPollutionTooltip = floatTooltip4;
      FloatTooltip floatTooltip5 = new FloatTooltip();
      floatTooltip5.path = (PathSegment) "NoisePollution";
      floatTooltip5.icon = "Media/Game/Icons/NoisePollution.svg";
      floatTooltip5.label = LocalizedString.Id("Infoviews.INFOVIEW[NoisePollution]");
      floatTooltip5.unit = "integer";
      // ISSUE: reference to a compiler-generated field
      this.m_NoisePollutionTooltip = floatTooltip5;
      // ISSUE: reference to a compiler-generated field
      this.m_LandValueResult = new NativeValue<float>(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainAttractiveResult = new NativeValue<float>(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_NoisePollutionResult = new NativeValue<float>(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_AirPollutionResult = new NativeValue<float>(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_GroundPollutionResult = new NativeValue<float>(Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_LandValueResult.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_TerrainAttractiveResult.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_NoisePollutionResult.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_AirPollutionResult.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_GroundPollutionResult.Dispose();
      base.OnDestroy();
    }

    private bool IsInfomodeActivated()
    {
      InfoviewPrefab prefab;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      return this.m_PrefabSystem.TryGetPrefab<InfoviewPrefab>(this.m_LandValueParameterQuery.GetSingleton<LandValueParameterData>().m_LandValueInfoViewPrefab, out prefab) && (Object) this.m_ToolSystem.activeInfoview == (Object) prefab;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      if (this.IsInfomodeActivated() || this.m_LandValueDebugSystem.Enabled)
      {
        this.CompleteDependency();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_LandValueTooltip.value = this.m_LandValueResult.value;
        // ISSUE: reference to a compiler-generated field
        this.AddMouseTooltip((IWidget) this.m_LandValueTooltip);
        // ISSUE: reference to a compiler-generated field
        if (this.m_LandValueDebugSystem.Enabled)
        {
          // ISSUE: reference to a compiler-generated field
          if ((double) this.m_TerrainAttractiveResult.value > 0.0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_TerrainAttractiveTooltip.value = this.m_TerrainAttractiveResult.value;
            // ISSUE: reference to a compiler-generated field
            this.AddMouseTooltip((IWidget) this.m_TerrainAttractiveTooltip);
          }
          // ISSUE: reference to a compiler-generated field
          if ((double) this.m_AirPollutionResult.value > 0.0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_AirPollutionTooltip.value = this.m_AirPollutionResult.value;
            // ISSUE: reference to a compiler-generated field
            this.AddMouseTooltip((IWidget) this.m_AirPollutionTooltip);
          }
          // ISSUE: reference to a compiler-generated field
          if ((double) this.m_GroundPollutionResult.value > 0.0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_GroundPollutionTooltip.value = this.m_GroundPollutionResult.value;
            // ISSUE: reference to a compiler-generated field
            this.AddMouseTooltip((IWidget) this.m_GroundPollutionTooltip);
          }
          // ISSUE: reference to a compiler-generated field
          if ((double) this.m_NoisePollutionResult.value > 0.0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_NoisePollutionTooltip.value = this.m_NoisePollutionResult.value;
            // ISSUE: reference to a compiler-generated field
            this.AddMouseTooltip((IWidget) this.m_NoisePollutionTooltip);
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_LandValueResult.value = 0.0f;
        // ISSUE: reference to a compiler-generated field
        this.m_TerrainAttractiveResult.value = 0.0f;
        // ISSUE: reference to a compiler-generated field
        this.m_AirPollutionResult.value = 0.0f;
        // ISSUE: reference to a compiler-generated field
        this.m_GroundPollutionResult.value = 0.0f;
        // ISSUE: reference to a compiler-generated field
        this.m_NoisePollutionResult.value = 0.0f;
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.typeMask = TypeMask.Terrain | TypeMask.Water;
        RaycastResult result;
        // ISSUE: reference to a compiler-generated field
        this.m_ToolRaycastSystem.GetRaycastResult(out result);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        TerrainHeightData heightData = this.m_TerrainSystem.GetHeightData();
        JobHandle dependencies1;
        JobHandle dependencies2;
        JobHandle dependencies3;
        JobHandle dependencies4;
        JobHandle dependencies5;
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
        LandValueTooltipSystem.LandValueTooltipJob jobData = new LandValueTooltipSystem.LandValueTooltipJob()
        {
          m_LandValueMap = this.m_LandValueSystem.GetMap(true, out dependencies1),
          m_AttractiveMap = this.m_TerrainAttractivenessSystem.GetMap(true, out dependencies2),
          m_GroundPollutionMap = this.m_GroundPollutionSystem.GetMap(true, out dependencies3),
          m_AirPollutionMap = this.m_AirPollutionSystem.GetMap(true, out dependencies4),
          m_NoisePollutionMap = this.m_NoisePollutionSystem.GetMap(true, out dependencies5),
          m_TerrainHeight = TerrainUtils.SampleHeight(ref heightData, result.m_Hit.m_HitPosition),
          m_AttractivenessParameterData = this.m_AttractivenessParameterQuery.GetSingleton<AttractivenessParameterData>(),
          m_LandValueResult = this.m_LandValueResult,
          m_NoisePollutionResult = this.m_NoisePollutionResult,
          m_AirPollutionResult = this.m_AirPollutionResult,
          m_GroundPollutionResult = this.m_GroundPollutionResult,
          m_TerrainAttractiveResult = this.m_TerrainAttractiveResult,
          m_RaycastPosition = result.m_Hit.m_HitPosition
        };
        this.Dependency = jobData.Schedule<LandValueTooltipSystem.LandValueTooltipJob>(JobHandle.CombineDependencies(this.Dependency, JobHandle.CombineDependencies(dependencies2, dependencies1, JobHandle.CombineDependencies(dependencies3, dependencies4, dependencies5))));
        // ISSUE: reference to a compiler-generated field
        this.m_LandValueSystem.AddReader(this.Dependency);
        // ISSUE: reference to a compiler-generated field
        this.m_TerrainAttractivenessSystem.AddReader(this.Dependency);
        // ISSUE: reference to a compiler-generated field
        this.m_GroundPollutionSystem.AddReader(this.Dependency);
        // ISSUE: reference to a compiler-generated field
        this.m_AirPollutionSystem.AddReader(this.Dependency);
        // ISSUE: reference to a compiler-generated field
        this.m_NoisePollutionSystem.AddReader(this.Dependency);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_LandValueResult.value = 0.0f;
        // ISSUE: reference to a compiler-generated field
        this.m_TerrainAttractiveResult.value = 0.0f;
        // ISSUE: reference to a compiler-generated field
        this.m_AirPollutionResult.value = 0.0f;
        // ISSUE: reference to a compiler-generated field
        this.m_GroundPollutionResult.value = 0.0f;
        // ISSUE: reference to a compiler-generated field
        this.m_NoisePollutionResult.value = 0.0f;
      }
    }

    [UnityEngine.Scripting.Preserve]
    public LandValueTooltipSystem()
    {
    }

    [BurstCompile]
    private struct LandValueTooltipJob : IJob
    {
      [ReadOnly]
      public NativeArray<LandValueCell> m_LandValueMap;
      [ReadOnly]
      public NativeArray<TerrainAttractiveness> m_AttractiveMap;
      [ReadOnly]
      public NativeArray<GroundPollution> m_GroundPollutionMap;
      [ReadOnly]
      public NativeArray<AirPollution> m_AirPollutionMap;
      [ReadOnly]
      public NativeArray<NoisePollution> m_NoisePollutionMap;
      [ReadOnly]
      public AttractivenessParameterData m_AttractivenessParameterData;
      [ReadOnly]
      public float m_TerrainHeight;
      [ReadOnly]
      public float3 m_RaycastPosition;
      public NativeValue<float> m_LandValueResult;
      public NativeValue<float> m_TerrainAttractiveResult;
      public NativeValue<float> m_AirPollutionResult;
      public NativeValue<float> m_NoisePollutionResult;
      public NativeValue<float> m_GroundPollutionResult;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_LandValueResult.value = this.m_LandValueMap[LandValueSystem.GetCellIndex(this.m_RaycastPosition)].m_LandValue;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_TerrainAttractiveResult.value = TerrainAttractivenessSystem.EvaluateAttractiveness(this.m_TerrainHeight, TerrainAttractivenessSystem.GetAttractiveness(this.m_RaycastPosition, this.m_AttractiveMap), this.m_AttractivenessParameterData);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_GroundPollutionResult.value = (float) GroundPollutionSystem.GetPollution(this.m_RaycastPosition, this.m_GroundPollutionMap).m_Pollution;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AirPollutionResult.value = (float) AirPollutionSystem.GetPollution(this.m_RaycastPosition, this.m_AirPollutionMap).m_Pollution;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_NoisePollutionResult.value = (float) NoisePollutionSystem.GetPollution(this.m_RaycastPosition, this.m_NoisePollutionMap).m_Pollution;
      }
    }
  }
}
