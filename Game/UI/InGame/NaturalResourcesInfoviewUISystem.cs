// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.NaturalResourcesInfoviewUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class NaturalResourcesInfoviewUISystem : InfoviewUISystemBase
  {
    private const string kGroup = "naturalResourceInfo";
    private ResourceSystem m_ResourceSystem;
    private ValueBinding<float> m_AvailableOil;
    private ValueBinding<float> m_AvailableOre;
    private ValueBinding<float> m_AvailableForest;
    private ValueBinding<float> m_AvailableFertility;
    private ValueBinding<float> m_ForestRenewalRate;
    private ValueBinding<float> m_FertilityRenewalRate;
    private ValueBinding<float> m_OilExtractionRate;
    private ValueBinding<float> m_OreExtractionRate;
    private ValueBinding<float> m_ForestExtractionRate;
    private ValueBinding<float> m_FertilityExtractionRate;
    private EntityQuery m_MapTileQuery;
    private EntityQuery m_ExtractorQuery;
    private NativeArray<float> m_Results;
    private NaturalResourcesInfoviewUISystem.TypeHandle __TypeHandle;
    private EntityQuery __query_1701516005_0;

    public override GameMode gameMode => GameMode.GameOrEditor;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AvailableOil = new ValueBinding<float>("naturalResourceInfo", "availableOil", 0.0f)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AvailableOre = new ValueBinding<float>("naturalResourceInfo", "availableOre", 0.0f)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AvailableForest = new ValueBinding<float>("naturalResourceInfo", "availableForest", 0.0f)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_AvailableFertility = new ValueBinding<float>("naturalResourceInfo", "availableFertility", 0.0f)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ForestRenewalRate = new ValueBinding<float>("naturalResourceInfo", "forestRenewalRate", 0.0f)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_FertilityRenewalRate = new ValueBinding<float>("naturalResourceInfo", "fertilityRenewalRate", 0.0f)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_OilExtractionRate = new ValueBinding<float>("naturalResourceInfo", "oilExtractionRate", 0.0f)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_OreExtractionRate = new ValueBinding<float>("naturalResourceInfo", "oreExtractionRate", 0.0f)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ForestExtractionRate = new ValueBinding<float>("naturalResourceInfo", "forestExtractionRate", 0.0f)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_FertilityExtractionRate = new ValueBinding<float>("naturalResourceInfo", "fertilityExtractionRate", 0.0f)));
      // ISSUE: reference to a compiler-generated field
      this.m_MapTileQuery = this.GetEntityQuery(ComponentType.ReadOnly<MapTile>(), ComponentType.ReadOnly<MapFeatureElement>(), ComponentType.Exclude<Native>());
      // ISSUE: reference to a compiler-generated field
      this.m_ExtractorQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Companies.ExtractorCompany>(), ComponentType.ReadOnly<PropertyRenter>(), ComponentType.ReadOnly<WorkProvider>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_Results = new NativeArray<float>(10, Allocator.Persistent);
      this.RequireForUpdate<ExtractorParameterData>();
      this.RequireForUpdate<EconomyParameterData>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Results.Dispose();
      base.OnDestroy();
    }

    protected override bool Active
    {
      get
      {
        return base.Active || this.m_AvailableFertility.active || this.m_AvailableForest.active || this.m_AvailableOil.active || this.m_AvailableOre.active || this.m_FertilityExtractionRate.active || this.m_ForestExtractionRate.active || this.m_OilExtractionRate.active || this.m_OreExtractionRate.active;
      }
    }

    protected override void PerformUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.ResetResults<float>(this.m_Results);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_MapFeatureElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      new NaturalResourcesInfoviewUISystem.UpdateResourcesJob()
      {
        m_MapFeatureElementHandle = this.__TypeHandle.__Game_Areas_MapFeatureElement_RO_BufferTypeHandle,
        m_Results = this.m_Results
      }.Schedule<NaturalResourcesInfoviewUISystem.UpdateResourcesJob>(this.m_MapTileQuery, this.Dependency).Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Extractor_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      new NaturalResourcesInfoviewUISystem.UpdateExtractionJob()
      {
        m_EntityHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PropertyRenterHandle = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle,
        m_WorkProviderHandle = this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentTypeHandle,
        m_PrefabRefHandle = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_AttachedFromEntity = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup,
        m_ExtractorsFromEntity = this.__TypeHandle.__Game_Areas_Extractor_RO_ComponentLookup,
        m_PrefabRefFromEntity = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ExtractorAreaDataFromEntity = this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup,
        m_SpawnableBuildingDataFromEntity = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_WorkplaceDataFromEntity = this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup,
        m_IndustrialProcessDataFromEntity = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
        m_BuildingEfficiencyFromEntity = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup,
        m_SubAreaFromEntity = this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup,
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_EconomyParameters = this.__query_1701516005_0.GetSingleton<EconomyParameterData>(),
        m_Result = this.m_Results
      }.Schedule<NaturalResourcesInfoviewUISystem.UpdateExtractionJob>(this.m_ExtractorQuery, this.Dependency).Complete();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_FertilityExtractionRate.Update(this.m_Results[4]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ForestExtractionRate.Update(this.m_Results[5]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_OreExtractionRate.Update(this.m_Results[7]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_OilExtractionRate.Update(this.m_Results[6]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AvailableFertility.Update(this.m_Results[0]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AvailableForest.Update(this.m_Results[1]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AvailableOre.Update(this.m_Results[3]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_AvailableOil.Update(this.m_Results[2]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ForestRenewalRate.Update(this.m_Results[9]);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_FertilityRenewalRate.Update(this.m_Results[8]);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_1701516005_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<EconomyParameterData>()
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
    public NaturalResourcesInfoviewUISystem()
    {
    }

    private enum Result
    {
      FertilityAmount,
      ForestAmount,
      OilAmount,
      OreAmount,
      FertilityExtraction,
      ForestExtraction,
      OilExtraction,
      OreExtraction,
      FertilityRenewal,
      ForestRenewal,
      Count,
    }

    [BurstCompile]
    private struct UpdateResourcesJob : IJobChunk
    {
      [ReadOnly]
      public BufferTypeHandle<MapFeatureElement> m_MapFeatureElementHandle;
      public NativeArray<float> m_Results;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<MapFeatureElement> bufferAccessor = chunk.GetBufferAccessor<MapFeatureElement>(ref this.m_MapFeatureElementHandle);
        float num1 = 0.0f;
        float num2 = 0.0f;
        float num3 = 0.0f;
        float num4 = 0.0f;
        float num5 = 0.0f;
        float num6 = 0.0f;
        for (int index = 0; index < bufferAccessor.Length; ++index)
        {
          DynamicBuffer<MapFeatureElement> dynamicBuffer = bufferAccessor[index];
          num1 += dynamicBuffer[4].m_Amount;
          num2 += dynamicBuffer[5].m_Amount;
          num3 += dynamicBuffer[3].m_Amount;
          num4 += dynamicBuffer[3].m_RenewalRate;
          num5 += dynamicBuffer[2].m_Amount;
          num6 += dynamicBuffer[2].m_RenewalRate;
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Results[0] += num5;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[8] += num6;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[1] += num3;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[9] += num4;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[2] += num1;
        // ISSUE: reference to a compiler-generated field
        this.m_Results[3] += num2;
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
    private struct UpdateExtractionJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityHandle;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> m_PropertyRenterHandle;
      [ReadOnly]
      public ComponentTypeHandle<WorkProvider> m_WorkProviderHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefHandle;
      [ReadOnly]
      public ComponentLookup<Attached> m_AttachedFromEntity;
      [ReadOnly]
      public ComponentLookup<Extractor> m_ExtractorsFromEntity;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefFromEntity;
      [ReadOnly]
      public ComponentLookup<ExtractorAreaData> m_ExtractorAreaDataFromEntity;
      [ReadOnly]
      public BufferLookup<Efficiency> m_BuildingEfficiencyFromEntity;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableBuildingDataFromEntity;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> m_WorkplaceDataFromEntity;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_IndustrialProcessDataFromEntity;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> m_SubAreaFromEntity;
      public EconomyParameterData m_EconomyParameters;
      public NativeArray<float> m_Result;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PropertyRenter> nativeArray2 = chunk.GetNativeArray<PropertyRenter>(ref this.m_PropertyRenterHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WorkProvider> nativeArray3 = chunk.GetNativeArray<WorkProvider>(ref this.m_WorkProviderHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefHandle);
        float num1 = 0.0f;
        float num2 = 0.0f;
        float num3 = 0.0f;
        float num4 = 0.0f;
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          Entity company = nativeArray1[index1];
          PropertyRenter propertyRenter = nativeArray2[index1];
          WorkProvider workProvider = nativeArray3[index1];
          PrefabRef prefabRef = nativeArray4[index1];
          IndustrialProcessData componentData1;
          WorkplaceData componentData2;
          PrefabRef componentData3;
          Attached componentData4;
          DynamicBuffer<Game.Areas.SubArea> bufferData;
          SpawnableBuildingData componentData5;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_IndustrialProcessDataFromEntity.TryGetComponent(prefabRef.m_Prefab, out componentData1) && this.m_WorkplaceDataFromEntity.TryGetComponent(prefabRef.m_Prefab, out componentData2) && this.m_PrefabRefFromEntity.TryGetComponent(propertyRenter.m_Property, out componentData3) && this.m_AttachedFromEntity.TryGetComponent(propertyRenter.m_Property, out componentData4) && this.m_SubAreaFromEntity.TryGetBuffer(componentData4.m_Parent, out bufferData) && this.m_SpawnableBuildingDataFromEntity.TryGetComponent(componentData3.m_Prefab, out componentData5))
          {
            // ISSUE: reference to a compiler-generated field
            float efficiency = BuildingUtils.GetEfficiency(propertyRenter.m_Property, ref this.m_BuildingEfficiencyFromEntity);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            double resourcesInArea = (double) ExtractorAISystem.GetResourcesInArea(company, bufferData, this.m_ExtractorsFromEntity);
            int maxWorkers = workProvider.m_MaxWorkers;
            int level = (int) componentData5.m_Level;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            double productionPerDay = (double) EconomyUtils.GetCompanyProductionPerDay(efficiency, maxWorkers, level, true, componentData2, componentData1, this.m_ResourcePrefabs, this.m_ResourceDatas, ref this.m_EconomyParameters);
            int num5 = Mathf.FloorToInt(math.min((float) resourcesInArea, (float) productionPerDay));
            float num6 = 0.0f;
            float num7 = 0.0f;
            float num8 = 0.0f;
            float num9 = 0.0f;
            for (int index2 = 0; index2 < bufferData.Length; ++index2)
            {
              PrefabRef componentData6;
              ExtractorAreaData componentData7;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabRefFromEntity.TryGetComponent(bufferData[index2].m_Area, out componentData6) && this.m_ExtractorAreaDataFromEntity.TryGetComponent(componentData6.m_Prefab, out componentData7))
              {
                switch (componentData7.m_MapFeature)
                {
                  case MapFeature.FertileLand:
                    num9 += (float) num5;
                    continue;
                  case MapFeature.Forest:
                    num8 += (float) num5;
                    continue;
                  case MapFeature.Oil:
                    num6 += (float) num5;
                    continue;
                  case MapFeature.Ore:
                    num7 += (float) num5;
                    continue;
                  default:
                    continue;
                }
              }
            }
            num4 += num9;
            num3 += num8;
            num1 += num6;
            num2 += num7;
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_Result[4] += num4;
        // ISSUE: reference to a compiler-generated field
        this.m_Result[5] += num3;
        // ISSUE: reference to a compiler-generated field
        this.m_Result[6] += num1;
        // ISSUE: reference to a compiler-generated field
        this.m_Result[7] += num2;
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
      public BufferTypeHandle<MapFeatureElement> __Game_Areas_MapFeatureElement_RO_BufferTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WorkProvider> __Game_Companies_WorkProvider_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Attached> __Game_Objects_Attached_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Extractor> __Game_Areas_Extractor_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ExtractorAreaData> __Game_Prefabs_ExtractorAreaData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> __Game_Prefabs_WorkplaceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> __Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Efficiency> __Game_Buildings_Efficiency_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> __Game_Areas_SubArea_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_MapFeatureElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<MapFeatureElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_WorkProvider_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WorkProvider>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentLookup = state.GetComponentLookup<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Extractor_RO_ComponentLookup = state.GetComponentLookup<Extractor>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup = state.GetComponentLookup<ExtractorAreaData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WorkplaceData_RO_ComponentLookup = state.GetComponentLookup<WorkplaceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup = state.GetComponentLookup<IndustrialProcessData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferLookup = state.GetBufferLookup<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Areas.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
      }
    }
  }
}
