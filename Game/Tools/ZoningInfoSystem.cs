// Decompiled with JetBrains decompiler
// Type: Game.Tools.ZoningInfoSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Simulation;
using Game.Zones;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class ZoningInfoSystem : GameSystemBase, IZoningInfoSystem
  {
    private EntityQuery m_ZoningPreferenceGroup;
    private EntityQuery m_ProcessQuery;
    private NativeList<ZoneEvaluationUtils.ZoningEvaluationResult> m_EvaluationResults;
    private ZoneToolSystem m_ZoneToolSystem;
    private IndustrialDemandSystem m_IndustrialDemandSystem;
    private GroundPollutionSystem m_GroundPollutionSystem;
    private AirPollutionSystem m_AirPollutionSystem;
    private NoisePollutionSystem m_NoisePollutionSystem;
    private PrefabSystem m_PrefabSystem;
    private ResourceSystem m_ResourceSystem;
    private ToolRaycastSystem m_ToolRaycastSystem;
    private ZoningInfoSystem.TypeHandle __TypeHandle;

    public NativeList<ZoneEvaluationUtils.ZoningEvaluationResult> evaluationResults
    {
      get => this.m_EvaluationResults;
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ZoneToolSystem = this.World.GetOrCreateSystemManaged<ZoneToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IndustrialDemandSystem = this.World.GetOrCreateSystemManaged<IndustrialDemandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_GroundPollutionSystem = this.World.GetOrCreateSystemManaged<GroundPollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AirPollutionSystem = this.World.GetOrCreateSystemManaged<AirPollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NoisePollutionSystem = this.World.GetOrCreateSystemManaged<NoisePollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolRaycastSystem = this.World.GetOrCreateSystemManaged<ToolRaycastSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ProcessQuery = this.GetEntityQuery(ComponentType.ReadOnly<IndustrialProcessData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ZoningPreferenceGroup = this.GetEntityQuery(ComponentType.ReadOnly<ZonePreferenceData>());
      // ISSUE: reference to a compiler-generated field
      this.m_EvaluationResults = new NativeList<ZoneEvaluationUtils.ZoningEvaluationResult>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ProcessQuery);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_EvaluationResults.Dispose();
      base.OnDestroy();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_EvaluationResults.Clear();
      RaycastResult result;
      Game.Zones.Block component1;
      Owner component2;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ToolRaycastSystem.GetRaycastResult(out result) || !this.EntityManager.TryGetComponent<Game.Zones.Block>(result.m_Owner, out component1) || !this.EntityManager.TryGetComponent<Owner>(result.m_Owner, out component2))
        return;
      this.Dependency.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ResourceAvailability_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<ResourceAvailability> availabilityRoBufferLookup = this.__TypeHandle.__Game_Net_ResourceAvailability_RO_BufferLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_LandValue_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<LandValue> roComponentLookup1 = this.__TypeHandle.__Game_Net_LandValue_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      NativeArray<ZonePreferenceData> componentDataArray = this.m_ZoningPreferenceGroup.ToComponentDataArray<ZonePreferenceData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      JobHandle deps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeArray<int> industrialResourceDemands = this.m_IndustrialDemandSystem.GetIndustrialResourceDemands(out deps);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      ResourcePrefabs prefabs = this.m_ResourceSystem.GetPrefabs();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ResourceData> roComponentLookup2 = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup;
      ZonePreferenceData preferences = componentDataArray[0];
      Entity owner = component2.m_Owner;
      // ISSUE: reference to a compiler-generated field
      AreaType areaType = this.m_ZoneToolSystem.prefab.m_AreaType;
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      NativeArray<GroundPollution> map1 = this.m_GroundPollutionSystem.GetMap(true, out dependencies1);
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated field
      NativeArray<AirPollution> map2 = this.m_AirPollutionSystem.GetMap(true, out dependencies2);
      JobHandle dependencies3;
      // ISSUE: reference to a compiler-generated field
      NativeArray<NoisePollution> map3 = this.m_NoisePollutionSystem.GetMap(true, out dependencies3);
      deps.Complete();
      dependencies1.Complete();
      dependencies2.Complete();
      dependencies3.Complete();
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      float pollution = (float) GroundPollutionSystem.GetPollution(component1.m_Position, map1).m_Pollution + (float) AirPollutionSystem.GetPollution(component1.m_Position, map2).m_Pollution + (float) NoisePollutionSystem.GetPollution(component1.m_Position, map3).m_Pollution;
      float landValue = roComponentLookup1[owner].m_LandValue;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      Entity entity = this.m_PrefabSystem.GetEntity((PrefabBase) this.m_ZoneToolSystem.prefab);
      DynamicBuffer<ProcessEstimate> buffer = this.World.EntityManager.GetBuffer<ProcessEstimate>(entity, true);
      if (this.World.EntityManager.HasComponent<ZonePropertiesData>(entity))
      {
        ZonePropertiesData componentData = this.World.EntityManager.GetComponentData<ZonePropertiesData>(entity);
        float num = areaType != AreaType.Residential ? componentData.m_SpaceMultiplier : (componentData.m_ScaleResidentials ? componentData.m_ResidentialProperties : componentData.m_ResidentialProperties / 8f);
        landValue /= num;
      }
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<IndustrialProcessData> componentDataListAsync = this.m_ProcessQuery.ToComponentDataListAsync<IndustrialProcessData>((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      outJobHandle.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ZoneEvaluationUtils.GetFactors(areaType, this.m_ZoneToolSystem.prefab.m_Office, availabilityRoBufferLookup[owner], result.m_Hit.m_CurvePosition, ref preferences, this.m_EvaluationResults, industrialResourceDemands, pollution, landValue, buffer, componentDataListAsync, prefabs, roComponentLookup2);
      componentDataListAsync.Dispose();
      componentDataArray.Dispose();
      // ISSUE: reference to a compiler-generated field
      this.m_EvaluationResults.Sort<ZoneEvaluationUtils.ZoningEvaluationResult>();
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

    [Preserve]
    public ZoningInfoSystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public BufferLookup<ResourceAvailability> __Game_Net_ResourceAvailability_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<LandValue> __Game_Net_LandValue_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ResourceAvailability_RO_BufferLookup = state.GetBufferLookup<ResourceAvailability>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_LandValue_RO_ComponentLookup = state.GetComponentLookup<LandValue>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
      }
    }
  }
}
