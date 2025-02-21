// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.PollutionSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Buildings;
using Game.Citizens;
using Game.City;
using Game.Companies;
using Game.Prefabs;
using Game.Simulation;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class PollutionSection : InfoSectionBase
  {
    private EntityQuery m_UIConfigQuery;
    private PollutionSection.TypeHandle __TypeHandle;
    private EntityQuery __query_1774369403_0;
    private EntityQuery __query_1774369403_1;

    protected override string group => nameof (PollutionSection);

    protected override bool displayForDestroyedObjects => true;

    private PollutionThreshold groundPollutionKey { get; set; }

    private PollutionThreshold airPollutionKey { get; set; }

    private PollutionThreshold noisePollutionKey { get; set; }

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_UIConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<UIPollutionConfigurationData>());
    }

    protected override void Reset()
    {
      this.groundPollutionKey = PollutionThreshold.None;
      this.airPollutionKey = PollutionThreshold.None;
      this.noisePollutionKey = PollutionThreshold.None;
    }

    private bool Visible()
    {
      if (!this.EntityManager.HasComponent<Building>(this.selectedEntity))
        return false;
      // ISSUE: reference to a compiler-generated method
      PollutionData pollution = this.GetPollution();
      return (double) pollution.m_GroundPollution > 0.0 || (double) pollution.m_AirPollution > 0.0 || (double) pollution.m_NoisePollution > 0.0;
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      // ISSUE: reference to a compiler-generated method
      PollutionData pollution = this.GetPollution();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      UIPollutionConfigurationPrefab singletonPrefab = this.m_PrefabSystem.GetSingletonPrefab<UIPollutionConfigurationPrefab>(this.m_UIConfigQuery);
      this.groundPollutionKey = PollutionUIUtils.GetPollutionKey(singletonPrefab.m_GroundPollution, pollution.m_GroundPollution);
      this.airPollutionKey = PollutionUIUtils.GetPollutionKey(singletonPrefab.m_AirPollution, pollution.m_AirPollution);
      this.noisePollutionKey = PollutionUIUtils.GetPollutionKey(singletonPrefab.m_NoisePollution, pollution.m_NoisePollution);
    }

    private PollutionData GetPollution()
    {
      this.CompleteDependency();
      bool destroyed = this.EntityManager.HasComponent<Game.Common.Destroyed>(this.selectedEntity);
      bool abandoned = this.EntityManager.HasComponent<Abandoned>(this.selectedEntity);
      bool isPark = this.EntityManager.HasComponent<Game.Buildings.Park>(this.selectedEntity);
      DynamicBuffer<Efficiency> buffer1;
      float efficiency = this.EntityManager.TryGetBuffer<Efficiency>(this.selectedEntity, true, out buffer1) ? BuildingUtils.GetEfficiency(buffer1) : 1f;
      DynamicBuffer<Renter> buffer2;
      this.EntityManager.TryGetBuffer<Renter>(this.selectedEntity, true, out buffer2);
      DynamicBuffer<InstalledUpgrade> buffer3;
      this.EntityManager.TryGetBuffer<InstalledUpgrade>(this.selectedEntity, true, out buffer3);
      // ISSUE: reference to a compiler-generated field
      PollutionParameterData singleton = this.__query_1774369403_0.GetSingleton<PollutionParameterData>();
      // ISSUE: reference to a compiler-generated field
      DynamicBuffer<CityModifier> singletonBuffer = this.__query_1774369403_1.GetSingletonBuffer<CityModifier>(true);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<PrefabRef> roComponentLookup1 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<Game.Prefabs.BuildingData> roComponentLookup2 = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<SpawnableBuildingData> roComponentLookup3 = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PollutionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<PollutionData> roComponentLookup4 = this.__TypeHandle.__Game_Prefabs_PollutionData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PollutionModifierData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<PollutionModifierData> roComponentLookup5 = this.__TypeHandle.__Game_Prefabs_PollutionModifierData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ZoneData> roComponentLookup6 = this.__TypeHandle.__Game_Prefabs_ZoneData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<Employee> employeeRoBufferLookup = this.__TypeHandle.__Game_Companies_Employee_RO_BufferLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferLookup<HouseholdCitizen> citizenRoBufferLookup = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<Citizen> roComponentLookup7 = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated method
      return BuildingPollutionAddSystem.GetBuildingPollution(this.selectedPrefab, destroyed, abandoned, isPark, efficiency, buffer2, buffer3, singleton, singletonBuffer, ref roComponentLookup1, ref roComponentLookup2, ref roComponentLookup3, ref roComponentLookup4, ref roComponentLookup5, ref roComponentLookup6, ref employeeRoBufferLookup, ref citizenRoBufferLookup, ref roComponentLookup7);
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("groundPollutionKey");
      writer.Write((int) this.groundPollutionKey);
      writer.PropertyName("airPollutionKey");
      writer.Write((int) this.airPollutionKey);
      writer.PropertyName("noisePollutionKey");
      writer.Write((int) this.noisePollutionKey);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_1774369403_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<PollutionParameterData>()
        },
        Any = new ComponentType[0],
        None = new ComponentType[0],
        Disabled = new ComponentType[0],
        Absent = new ComponentType[0],
        Options = EntityQueryOptions.IncludeSystems
      });
      // ISSUE: reference to a compiler-generated field
      this.__query_1774369403_1 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadWrite<CityModifier>()
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

    [Preserve]
    public PollutionSection()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PollutionData> __Game_Prefabs_PollutionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PollutionModifierData> __Game_Prefabs_PollutionModifierData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ZoneData> __Game_Prefabs_ZoneData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Employee> __Game_Companies_Employee_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PollutionData_RO_ComponentLookup = state.GetComponentLookup<PollutionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PollutionModifierData_RO_ComponentLookup = state.GetComponentLookup<PollutionModifierData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ZoneData_RO_ComponentLookup = state.GetComponentLookup<ZoneData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RO_BufferLookup = state.GetBufferLookup<Employee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
      }
    }
  }
}
