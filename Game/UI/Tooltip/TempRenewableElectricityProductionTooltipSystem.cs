// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.TempRenewableElectricityProductionTooltipSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Buildings;
using Game.Common;
using Game.Objects;
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
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Tooltip
{
  [CompilerGenerated]
  public class TempRenewableElectricityProductionTooltipSystem : TooltipSystemBase
  {
    private WindSystem m_WindSystem;
    private GroundWaterSystem m_GroundWaterSystem;
    private EntityQuery m_ErrorQuery;
    private EntityQuery m_TempQuery;
    private ProgressTooltip m_Production;
    private StringTooltip m_WindWarning;
    private StringTooltip m_GroundWaterAvailabilityWarning;
    private TempRenewableElectricityProductionTooltipSystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_WindSystem = this.World.GetOrCreateSystemManaged<WindSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_GroundWaterSystem = this.World.GetOrCreateSystemManaged<GroundWaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ErrorQuery = this.GetEntityQuery(ComponentType.ReadOnly<Temp>(), ComponentType.ReadOnly<Error>());
      // ISSUE: reference to a compiler-generated field
      this.m_TempQuery = this.GetEntityQuery(ComponentType.ReadOnly<Temp>(), ComponentType.ReadOnly<Transform>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<RenewableElectricityProduction>(), ComponentType.Exclude<Deleted>());
      ProgressTooltip progressTooltip = new ProgressTooltip();
      progressTooltip.path = (PathSegment) "renewableElectricityProduction";
      progressTooltip.icon = "Media/Game/Icons/Electricity.svg";
      progressTooltip.label = LocalizedString.Id("Tools.ELECTRICITY_PRODUCTION_LABEL");
      progressTooltip.unit = "power";
      progressTooltip.omitMax = true;
      // ISSUE: reference to a compiler-generated field
      this.m_Production = progressTooltip;
      StringTooltip stringTooltip1 = new StringTooltip();
      stringTooltip1.path = (PathSegment) "windWarning";
      stringTooltip1.value = LocalizedString.Id("Tools.WARNING[NotEnoughWind]");
      stringTooltip1.color = TooltipColor.Warning;
      // ISSUE: reference to a compiler-generated field
      this.m_WindWarning = stringTooltip1;
      StringTooltip stringTooltip2 = new StringTooltip();
      stringTooltip2.path = (PathSegment) "groundWaterAvailabilityWarning";
      stringTooltip2.value = LocalizedString.Id("Tools.WARNING[NotEnoughGroundWater]");
      stringTooltip2.color = TooltipColor.Warning;
      // ISSUE: reference to a compiler-generated field
      this.m_GroundWaterAvailabilityWarning = stringTooltip2;
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TempQuery);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ErrorQuery.IsEmptyIgnoreFilter)
        return;
      this.CompleteDependency();
      float num1 = 0.0f;
      float num2 = 0.0f;
      bool flag1 = false;
      bool flag2 = false;
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      NativeArray<Wind> map1 = this.m_WindSystem.GetMap(true, out dependencies1);
      JobHandle dependencies2;
      // ISSUE: reference to a compiler-generated field
      NativeArray<GroundWater> map2 = this.m_GroundWaterSystem.GetMap(true, out dependencies2);
      dependencies1.Complete();
      dependencies2.Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WindPoweredData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<WindPoweredData> roComponentLookup1 = this.__TypeHandle.__Game_Prefabs_WindPoweredData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_GroundWaterPoweredData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<GroundWaterPoweredData> roComponentLookup2 = this.__TypeHandle.__Game_Prefabs_GroundWaterPoweredData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_TempQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PrefabRef> componentTypeHandle1 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Transform> componentTypeHandle2 = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Temp> componentTypeHandle3 = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_WaterPowered_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Game.Buildings.WaterPowered> componentTypeHandle4 = this.__TypeHandle.__Game_Buildings_WaterPowered_RO_ComponentTypeHandle;
        foreach (ArchetypeChunk archetypeChunk in archetypeChunkArray)
        {
          NativeArray<PrefabRef> nativeArray1 = archetypeChunk.GetNativeArray<PrefabRef>(ref componentTypeHandle1);
          NativeArray<Transform> nativeArray2 = archetypeChunk.GetNativeArray<Transform>(ref componentTypeHandle2);
          NativeArray<Temp> nativeArray3 = archetypeChunk.GetNativeArray<Temp>(ref componentTypeHandle3);
          NativeArray<Game.Buildings.WaterPowered> nativeArray4 = archetypeChunk.GetNativeArray<Game.Buildings.WaterPowered>(ref componentTypeHandle4);
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            if ((nativeArray3[index].m_Flags & (TempFlags.Create | TempFlags.Modify | TempFlags.Upgrade)) != (TempFlags) 0)
            {
              Entity prefab = nativeArray1[index].m_Prefab;
              if (roComponentLookup1.HasComponent(prefab))
              {
                Wind wind = WindSystem.GetWind(nativeArray2[index].m_Position, map1);
                // ISSUE: reference to a compiler-generated method
                float2 windProduction = PowerPlantAISystem.GetWindProduction(roComponentLookup1[prefab], wind, 1f);
                num1 += windProduction.x;
                num2 += windProduction.y;
                flag1 |= (double) windProduction.x < (double) windProduction.y * 0.75;
              }
              WaterPoweredData component;
              if (nativeArray4.Length != 0 && this.EntityManager.TryGetComponent<WaterPoweredData>(prefab, out component))
              {
                Game.Buildings.WaterPowered waterPowered = nativeArray4[index];
                // ISSUE: reference to a compiler-generated method
                float waterCapacity = PowerPlantAISystem.GetWaterCapacity(waterPowered, component);
                float num3 = math.min(waterCapacity, waterPowered.m_Estimate * component.m_ProductionFactor);
                num1 += num3;
                num2 += waterCapacity;
              }
              GroundWaterPoweredData componentData;
              if (roComponentLookup2.TryGetComponent(prefab, out componentData) && componentData.m_MaximumGroundWater > 0)
              {
                // ISSUE: reference to a compiler-generated method
                float2 groundWaterProduction = PowerPlantAISystem.GetGroundWaterProduction(componentData, nativeArray2[index].m_Position, 1f, map2);
                num1 += groundWaterProduction.x;
                num2 += groundWaterProduction.y;
                if ((double) groundWaterProduction.x < (double) groundWaterProduction.y * 0.75)
                  flag2 = true;
              }
            }
          }
        }
        if ((double) num2 <= 0.0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_Production.value = num1;
        // ISSUE: reference to a compiler-generated field
        this.m_Production.max = num2;
        // ISSUE: reference to a compiler-generated field
        ProgressTooltip.SetCapacityColor(this.m_Production);
        // ISSUE: reference to a compiler-generated field
        this.AddMouseTooltip((IWidget) this.m_Production);
        if (flag1)
        {
          // ISSUE: reference to a compiler-generated field
          this.AddMouseTooltip((IWidget) this.m_WindWarning);
        }
        if (!flag2)
          return;
        // ISSUE: reference to a compiler-generated field
        this.AddMouseTooltip((IWidget) this.m_GroundWaterAvailabilityWarning);
      }
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
    public TempRenewableElectricityProductionTooltipSystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<WindPoweredData> __Game_Prefabs_WindPoweredData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<GroundWaterPoweredData> __Game_Prefabs_GroundWaterPoweredData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.WaterPowered> __Game_Buildings_WaterPowered_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WindPoweredData_RO_ComponentLookup = state.GetComponentLookup<WindPoweredData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_GroundWaterPoweredData_RO_ComponentLookup = state.GetComponentLookup<GroundWaterPoweredData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterPowered_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.WaterPowered>(true);
      }
    }
  }
}
