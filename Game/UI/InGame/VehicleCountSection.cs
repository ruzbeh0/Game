// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.VehicleCountSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Pathfind;
using Game.Policies;
using Game.Prefabs;
using Game.Routes;
using Game.Simulation;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class VehicleCountSection : InfoSectionBase
  {
    private PoliciesUISystem m_PoliciesUISystem;
    private Entity m_VehicleCountPolicy;
    private EntityQuery m_ConfigQuery;
    private NativeArray<int> m_IntResults;
    private NativeReference<float> m_DurationResult;
    private VehicleCountSection.TypeHandle __TypeHandle;

    protected override string group => nameof (VehicleCountSection);

    private int vehicleCountMin { get; set; }

    private int vehicleCountMax { get; set; }

    private int vehicleCount { get; set; }

    private int activeVehicles { get; set; }

    private float stableDuration { get; set; }

    protected override void Reset()
    {
      this.vehicleCountMin = 0;
      this.vehicleCountMax = 0;
      this.vehicleCount = 0;
      this.activeVehicles = 0;
      this.stableDuration = 0.0f;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PoliciesUISystem = this.World.GetOrCreateSystemManaged<PoliciesUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<UITransportConfigurationData>());
      this.AddBinding((IBinding) new TriggerBinding<float>(this.group, "setVehicleCount", (Action<float>) (newVehicleCount =>
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<RouteModifierData> buffer = this.EntityManager.GetBuffer<RouteModifierData>(this.m_VehicleCountPolicy, true);
        // ISSUE: reference to a compiler-generated field
        PolicySliderData componentData1 = this.EntityManager.GetComponentData<PolicySliderData>(this.m_VehicleCountPolicy);
        TransportLineData componentData2 = this.EntityManager.GetComponentData<TransportLineData>(this.selectedPrefab);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        this.m_PoliciesUISystem.SetPolicy(this.selectedEntity, this.m_VehicleCountPolicy, true, VehicleCountSection.CalculateVehicleCountJob.CalculateAdjustmentFromVehicleCount((int) newVehicleCount, componentData2.m_DefaultVehicleInterval, this.stableDuration, buffer, componentData1));
      })));
      // ISSUE: reference to a compiler-generated field
      this.m_IntResults = new NativeArray<int>(4, Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_DurationResult = new NativeReference<float>(0.0f, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_IntResults.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_DurationResult.Dispose();
      base.OnDestroy();
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_ConfigQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.m_VehicleCountPolicy = this.m_PrefabSystem.GetEntity((PrefabBase) this.m_PrefabSystem.GetSingletonPrefab<UITransportConfigurationPrefab>(this.m_ConfigQuery).m_VehicleCountPolicy);
    }

    private void OnSetVehicleCount(float newVehicleCount)
    {
      // ISSUE: reference to a compiler-generated field
      DynamicBuffer<RouteModifierData> buffer = this.EntityManager.GetBuffer<RouteModifierData>(this.m_VehicleCountPolicy, true);
      // ISSUE: reference to a compiler-generated field
      PolicySliderData componentData1 = this.EntityManager.GetComponentData<PolicySliderData>(this.m_VehicleCountPolicy);
      TransportLineData componentData2 = this.EntityManager.GetComponentData<TransportLineData>(this.selectedPrefab);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.m_PoliciesUISystem.SetPolicy(this.selectedEntity, this.m_VehicleCountPolicy, true, VehicleCountSection.CalculateVehicleCountJob.CalculateAdjustmentFromVehicleCount((int) newVehicleCount, componentData2.m_DefaultVehicleInterval, this.stableDuration, buffer, componentData1));
    }

    private bool Visible()
    {
      return this.EntityManager.HasComponent<Route>(this.selectedEntity) && this.EntityManager.HasComponent<TransportLine>(this.selectedEntity) && this.EntityManager.HasComponent<RouteWaypoint>(this.selectedEntity) && this.EntityManager.HasComponent<Policy>(this.selectedEntity);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      this.visible = this.Visible();
      if (!this.visible)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RouteModifierData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteModifier_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteSegment_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteVehicle_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_VehicleTiming_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PolicySliderData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      new VehicleCountSection.CalculateVehicleCountJob()
      {
        m_SelectedEntity = this.selectedEntity,
        m_SelectedPrefab = this.selectedPrefab,
        m_Policy = this.m_VehicleCountPolicy,
        m_TransportLineDatas = this.__TypeHandle.__Game_Prefabs_TransportLineData_RO_ComponentLookup,
        m_PolicySliderDatas = this.__TypeHandle.__Game_Prefabs_PolicySliderData_RO_ComponentLookup,
        m_VehicleTimings = this.__TypeHandle.__Game_Routes_VehicleTiming_RO_ComponentLookup,
        m_PathInformations = this.__TypeHandle.__Game_Pathfind_PathInformation_RO_ComponentLookup,
        m_RouteVehicles = this.__TypeHandle.__Game_Routes_RouteVehicle_RO_BufferLookup,
        m_RouteWaypoints = this.__TypeHandle.__Game_Routes_RouteWaypoint_RO_BufferLookup,
        m_RouteSegments = this.__TypeHandle.__Game_Routes_RouteSegment_RO_BufferLookup,
        m_RouteModifiers = this.__TypeHandle.__Game_Routes_RouteModifier_RO_BufferLookup,
        m_RouteModifierDatas = this.__TypeHandle.__Game_Prefabs_RouteModifierData_RO_BufferLookup,
        m_IntResults = this.m_IntResults,
        m_Duration = this.m_DurationResult
      }.Schedule<VehicleCountSection.CalculateVehicleCountJob>(this.Dependency).Complete();
    }

    protected override void OnProcess()
    {
      // ISSUE: reference to a compiler-generated field
      this.vehicleCountMin = this.m_IntResults[2];
      // ISSUE: reference to a compiler-generated field
      this.vehicleCountMax = this.m_IntResults[3];
      // ISSUE: reference to a compiler-generated field
      this.vehicleCount = this.m_IntResults[0];
      // ISSUE: reference to a compiler-generated field
      this.activeVehicles = this.m_IntResults[1];
      // ISSUE: reference to a compiler-generated field
      this.stableDuration = this.m_DurationResult.Value;
      this.tooltipTags.Add("TransportLine");
      this.tooltipTags.Add("CargoRoute");
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("vehicleCountMin");
      writer.Write(this.vehicleCountMin);
      writer.PropertyName("vehicleCountMax");
      writer.Write(this.vehicleCountMax);
      writer.PropertyName("vehicleCount");
      writer.Write(this.vehicleCount);
      writer.PropertyName("activeVehicles");
      writer.Write(this.activeVehicles);
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
    public VehicleCountSection()
    {
    }

    private enum Result
    {
      VehicleCount,
      ActiveVehicles,
      VehicleCountMin,
      VehicleCountMax,
      Count,
    }

    [BurstCompile]
    private struct CalculateVehicleCountJob : IJob
    {
      [ReadOnly]
      public Entity m_SelectedEntity;
      [ReadOnly]
      public Entity m_SelectedPrefab;
      [ReadOnly]
      public Entity m_Policy;
      [ReadOnly]
      public ComponentLookup<VehicleTiming> m_VehicleTimings;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformations;
      [ReadOnly]
      public ComponentLookup<TransportLineData> m_TransportLineDatas;
      [ReadOnly]
      public ComponentLookup<PolicySliderData> m_PolicySliderDatas;
      [ReadOnly]
      public BufferLookup<RouteVehicle> m_RouteVehicles;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> m_RouteWaypoints;
      [ReadOnly]
      public BufferLookup<RouteSegment> m_RouteSegments;
      [ReadOnly]
      public BufferLookup<RouteModifier> m_RouteModifiers;
      [ReadOnly]
      public BufferLookup<RouteModifierData> m_RouteModifierDatas;
      public NativeArray<int> m_IntResults;
      public NativeReference<float> m_Duration;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        TransportLineData transportLineData = this.m_TransportLineDatas[this.m_SelectedPrefab];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<RouteVehicle> routeVehicle = this.m_RouteVehicles[this.m_SelectedEntity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<RouteModifier> routeModifier = this.m_RouteModifiers[this.m_SelectedEntity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        PolicySliderData policySliderData = this.m_PolicySliderDatas[this.m_Policy];
        float defaultVehicleInterval = transportLineData.m_DefaultVehicleInterval;
        float vehicleInterval = defaultVehicleInterval;
        RouteUtils.ApplyModifier(ref vehicleInterval, routeModifier, RouteModifierType.VehicleInterval);
        // ISSUE: reference to a compiler-generated method
        float stableDuration = this.CalculateStableDuration(transportLineData);
        // ISSUE: reference to a compiler-generated field
        this.m_Duration.Value = stableDuration;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_IntResults[0] = TransportLineSystem.CalculateVehicleCount(vehicleInterval, stableDuration);
        // ISSUE: reference to a compiler-generated field
        this.m_IntResults[1] = routeVehicle.Length;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_IntResults[2] = this.CalculateVehicleCountFromAdjustment(policySliderData.m_Range.min, defaultVehicleInterval, stableDuration);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_IntResults[3] = this.CalculateVehicleCountFromAdjustment(policySliderData.m_Range.max, defaultVehicleInterval, stableDuration);
      }

      private int CalculateVehicleCountFromAdjustment(
        float policyAdjustment,
        float interval,
        float duration)
      {
        RouteModifier modifier = new RouteModifier();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        foreach (RouteModifierData modifierData in this.m_RouteModifierDatas[this.m_Policy])
        {
          if (modifierData.m_Type == RouteModifierType.VehicleInterval)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            float modifierDelta = RouteModifierInitializeSystem.RouteModifierRefreshData.GetModifierDelta(modifierData, policyAdjustment, this.m_Policy, this.m_PolicySliderDatas);
            // ISSUE: reference to a compiler-generated method
            RouteModifierInitializeSystem.RouteModifierRefreshData.AddModifierData(ref modifier, modifierData, modifierDelta);
            break;
          }
        }
        interval += modifier.m_Delta.x;
        interval += interval * modifier.m_Delta.y;
        // ISSUE: reference to a compiler-generated method
        return TransportLineSystem.CalculateVehicleCount(interval, duration);
      }

      public static float CalculateAdjustmentFromVehicleCount(
        int vehicleCount,
        float originalInterval,
        float duration,
        DynamicBuffer<RouteModifierData> modifierDatas,
        PolicySliderData sliderData)
      {
        // ISSUE: reference to a compiler-generated method
        float vehicleInterval = TransportLineSystem.CalculateVehicleInterval(duration, vehicleCount);
        RouteModifier modifier = new RouteModifier();
        foreach (RouteModifierData modifierData in modifierDatas)
        {
          if (modifierData.m_Type == RouteModifierType.VehicleInterval)
          {
            if (modifierData.m_Mode == ModifierValueMode.Absolute)
              modifier.m_Delta.x = vehicleInterval - originalInterval;
            else
              modifier.m_Delta.y = (-originalInterval + vehicleInterval) / originalInterval;
            // ISSUE: reference to a compiler-generated method
            float deltaFromModifier = RouteModifierInitializeSystem.RouteModifierRefreshData.GetDeltaFromModifier(modifier, modifierData);
            // ISSUE: reference to a compiler-generated method
            return RouteModifierInitializeSystem.RouteModifierRefreshData.GetPolicyAdjustmentFromModifierDelta(modifierData, deltaFromModifier, sliderData);
          }
        }
        return -1f;
      }

      public float CalculateStableDuration(TransportLineData transportLineData)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<RouteWaypoint> routeWaypoint = this.m_RouteWaypoints[this.m_SelectedEntity];
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<RouteSegment> routeSegment = this.m_RouteSegments[this.m_SelectedEntity];
        int num = 0;
        for (int index = 0; index < routeWaypoint.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_VehicleTimings.HasComponent(routeWaypoint[index].m_Waypoint))
          {
            num = index;
            break;
          }
        }
        float stableDuration = 0.0f;
        for (int index = 0; index < routeWaypoint.Length; ++index)
        {
          int2 a = (int2) (num + index);
          ++a.y;
          a = math.select(a, a - routeWaypoint.Length, a >= routeWaypoint.Length);
          Entity waypoint = routeWaypoint[a.y].m_Waypoint;
          PathInformation componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_PathInformations.TryGetComponent(routeSegment[a.x].m_Segment, out componentData))
            stableDuration += componentData.m_Duration;
          // ISSUE: reference to a compiler-generated field
          if (this.m_VehicleTimings.HasComponent(waypoint))
            stableDuration += transportLineData.m_StopDuration;
        }
        return stableDuration;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<TransportLineData> __Game_Prefabs_TransportLineData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PolicySliderData> __Game_Prefabs_PolicySliderData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<VehicleTiming> __Game_Routes_VehicleTiming_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PathInformation> __Game_Pathfind_PathInformation_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<RouteVehicle> __Game_Routes_RouteVehicle_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<RouteWaypoint> __Game_Routes_RouteWaypoint_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<RouteSegment> __Game_Routes_RouteSegment_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<RouteModifier> __Game_Routes_RouteModifier_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<RouteModifierData> __Game_Prefabs_RouteModifierData_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_TransportLineData_RO_ComponentLookup = state.GetComponentLookup<TransportLineData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PolicySliderData_RO_ComponentLookup = state.GetComponentLookup<PolicySliderData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_VehicleTiming_RO_ComponentLookup = state.GetComponentLookup<VehicleTiming>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathInformation_RO_ComponentLookup = state.GetComponentLookup<PathInformation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteVehicle_RO_BufferLookup = state.GetBufferLookup<RouteVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteWaypoint_RO_BufferLookup = state.GetBufferLookup<RouteWaypoint>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteSegment_RO_BufferLookup = state.GetBufferLookup<RouteSegment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteModifier_RO_BufferLookup = state.GetBufferLookup<RouteModifier>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RouteModifierData_RO_BufferLookup = state.GetBufferLookup<RouteModifierData>(true);
      }
    }
  }
}
