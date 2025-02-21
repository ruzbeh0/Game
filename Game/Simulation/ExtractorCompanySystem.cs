// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ExtractorCompanySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Agents;
using Game.Areas;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Objects;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class ExtractorCompanySystem : GameSystemBase
  {
    private SimulationSystem m_SimulationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private TaxSystem m_TaxSystem;
    private ResourceSystem m_ResourceSystem;
    private VehicleCapacitySystem m_VehicleCapacitySystem;
    private EntityQuery m_CompanyGroup;
    private ExtractorCompanySystem.TypeHandle __TypeHandle;
    private EntityQuery __query_1012523221_0;
    private EntityQuery __query_1012523221_1;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      return 262144 / (EconomyUtils.kCompanyUpdatesPerDay * 16);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_TaxSystem = this.World.GetOrCreateSystemManaged<TaxSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleCapacitySystem = this.World.GetOrCreateSystemManaged<VehicleCapacitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CompanyGroup = this.GetEntityQuery(ComponentType.ReadOnly<Game.Companies.ExtractorCompany>(), ComponentType.ReadOnly<PropertyRenter>(), ComponentType.ReadWrite<Resources>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<WorkProvider>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadWrite<CompanyData>(), ComponentType.ReadWrite<Employee>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CompanyGroup);
      this.RequireForUpdate<EconomyParameterData>();
      this.RequireForUpdate<ExtractorParameterData>();
    }

    public static MapFeature GetRequiredMapFeature(
      Resource output,
      Entity lotPrefab,
      ResourcePrefabs resourcePrefabs,
      ComponentLookup<ResourceData> resourceDatas,
      ComponentLookup<ExtractorAreaData> extractorAreaDatas)
    {
      ResourceData componentData;
      return resourceDatas.TryGetComponent(resourcePrefabs[output], out componentData) && componentData.m_RequireNaturalResource && extractorAreaDatas.HasComponent(lotPrefab) ? extractorAreaDatas[lotPrefab].m_MapFeature : MapFeature.None;
    }

    public static bool GetBestConcentration(
      Resource resource,
      DynamicBuffer<Game.Areas.SubArea> subAreas,
      ComponentLookup<Extractor> extractors,
      ComponentLookup<PrefabRef> prefabs,
      ComponentLookup<ExtractorAreaData> extractorDatas,
      ExtractorParameterData extractorParameters,
      ResourcePrefabs resourcePrefabs,
      ComponentLookup<ResourceData> resourceDatas,
      out float concentration,
      out int areaIndex)
    {
      areaIndex = -1;
      concentration = 0.0f;
      Entity resourcePrefab = resourcePrefabs[resource];
      if (!resourceDatas.HasComponent(resourcePrefab) || !resourceDatas[resourcePrefab].m_RequireNaturalResource)
      {
        concentration = 1f;
        areaIndex = 0;
        return true;
      }
      for (int index = 0; index < subAreas.Length; ++index)
      {
        Entity area = subAreas[index].m_Area;
        if (extractors.HasComponent(area) && prefabs.HasComponent(area))
        {
          Extractor extractor = extractors[area];
          Entity prefab = prefabs[area].m_Prefab;
          if (extractorDatas.HasComponent(prefab))
          {
            // ISSUE: reference to a compiler-generated method
            float effectiveConcentration = ExtractorCompanySystem.GetEffectiveConcentration(extractorParameters, extractorDatas[prefab].m_MapFeature, extractor.m_MaxConcentration);
            if ((double) extractor.m_ResourceAmount > 0.0 && (double) effectiveConcentration > (double) concentration)
            {
              concentration = effectiveConcentration;
              areaIndex = index;
            }
          }
        }
      }
      float num = 1f;
      concentration = math.min(1f, concentration / num);
      return areaIndex >= 0;
    }

    public static float GetEffectiveConcentration(
      ExtractorParameterData extractorParameters,
      MapFeature feature,
      float concentration)
    {
      float num;
      switch (feature)
      {
        case MapFeature.FertileLand:
          num = extractorParameters.m_FullFertility;
          break;
        case MapFeature.Oil:
          num = extractorParameters.m_FullOil;
          break;
        case MapFeature.Ore:
          num = extractorParameters.m_FullOre;
          break;
        default:
          num = 1f;
          break;
      }
      return math.min(1f, concentration / num);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, EconomyUtils.kCompanyUpdatesPerDay, 16);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_StorageLimitData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Profitability_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Agents_TaxPayer_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Employee_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_CompanyData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_Extractor_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ExtractorCompanySystem.ExtractorJob jobData = new ExtractorCompanySystem.ExtractorJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CompanyResourceType = this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle,
        m_ExtractorAreas = this.__TypeHandle.__Game_Areas_Extractor_RW_ComponentLookup,
        m_CompanyType = this.__TypeHandle.__Game_Companies_CompanyData_RW_ComponentTypeHandle,
        m_EmployeeType = this.__TypeHandle.__Game_Companies_Employee_RO_BufferTypeHandle,
        m_PropertyType = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_TaxPayerType = this.__TypeHandle.__Game_Agents_TaxPayer_RW_ComponentTypeHandle,
        m_ProfitabilityType = this.__TypeHandle.__Game_Companies_Profitability_RW_ComponentTypeHandle,
        m_IndustrialProcessDatas = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
        m_Limits = this.__TypeHandle.__Game_Companies_StorageLimitData_RO_ComponentLookup,
        m_BuildingEfficiencies = this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferLookup,
        m_WorkplaceDatas = this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup,
        m_SpawnableDatas = this.__TypeHandle.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup,
        m_Attached = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup,
        m_SubAreas = this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ExtractorAreaDatas = this.__TypeHandle.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup,
        m_Citizens = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup,
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_TaxRates = this.m_TaxSystem.GetTaxRates(),
        m_EconomyParameters = this.__query_1012523221_0.GetSingleton<EconomyParameterData>(),
        m_ExtractorParameters = this.__query_1012523221_1.GetSingleton<ExtractorParameterData>(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_RandomSeed = RandomSeed.Next(),
        m_UpdateFrameIndex = updateFrame,
        m_DeliveryTruckSelectData = this.m_VehicleCapacitySystem.GetDeliveryTruckSelectData()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<ExtractorCompanySystem.ExtractorJob>(this.m_CompanyGroup, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TaxSystem.AddReader(this.Dependency);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_1012523221_0 = state.GetEntityQuery(new EntityQueryDesc()
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
      // ISSUE: reference to a compiler-generated field
      this.__query_1012523221_1 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<ExtractorParameterData>()
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
    public ExtractorCompanySystem()
    {
    }

    [BurstCompile]
    private struct ExtractorJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      public BufferTypeHandle<Resources> m_CompanyResourceType;
      [ReadOnly]
      public BufferTypeHandle<Employee> m_EmployeeType;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> m_PropertyType;
      public ComponentTypeHandle<CompanyData> m_CompanyType;
      public ComponentTypeHandle<TaxPayer> m_TaxPayerType;
      public ComponentTypeHandle<Profitability> m_ProfitabilityType;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_IndustrialProcessDatas;
      [ReadOnly]
      public ComponentLookup<StorageLimitData> m_Limits;
      [NativeDisableParallelForRestriction]
      public BufferLookup<Efficiency> m_BuildingEfficiencies;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> m_WorkplaceDatas;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> m_SpawnableDatas;
      [ReadOnly]
      public ComponentLookup<Attached> m_Attached;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> m_SubAreas;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<Extractor> m_ExtractorAreas;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<ExtractorAreaData> m_ExtractorAreaDatas;
      [ReadOnly]
      public ComponentLookup<Citizen> m_Citizens;
      public EconomyParameterData m_EconomyParameters;
      public ExtractorParameterData m_ExtractorParameters;
      [ReadOnly]
      public NativeArray<int> m_TaxRates;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      public RandomSeed m_RandomSeed;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public uint m_UpdateFrameIndex;
      [ReadOnly]
      public DeliveryTruckSelectData m_DeliveryTruckSelectData;

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
        Random random = this.m_RandomSeed.GetRandom(unfilteredChunkIndex);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Profitability> nativeArray2 = chunk.GetNativeArray<Profitability>(ref this.m_ProfitabilityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Resources> bufferAccessor1 = chunk.GetBufferAccessor<Resources>(ref this.m_CompanyResourceType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Employee> bufferAccessor2 = chunk.GetBufferAccessor<Employee>(ref this.m_EmployeeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PropertyRenter> nativeArray3 = chunk.GetNativeArray<PropertyRenter>(ref this.m_PropertyType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<TaxPayer> nativeArray4 = chunk.GetNativeArray<TaxPayer>(ref this.m_TaxPayerType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity entity = nativeArray1[index];
          Entity property = nativeArray3[index].m_Property;
          Profitability profitability = nativeArray2[index];
          DynamicBuffer<Resources> resources = bufferAccessor1[index];
          // ISSUE: reference to a compiler-generated field
          Entity prefab1 = this.m_Prefabs[entity].m_Prefab;
          // ISSUE: reference to a compiler-generated field
          Entity prefab2 = this.m_Prefabs[property].m_Prefab;
          // ISSUE: reference to a compiler-generated field
          IndustrialProcessData industrialProcessData = this.m_IndustrialProcessDatas[prefab1];
          // ISSUE: reference to a compiler-generated field
          StorageLimitData limit = this.m_Limits[prefab1];
          // ISSUE: reference to a compiler-generated field
          WorkplaceData workplaceData = this.m_WorkplaceDatas[prefab1];
          // ISSUE: reference to a compiler-generated field
          SpawnableBuildingData spawnableData = this.m_SpawnableDatas[prefab2];
          int totalStorageUsed = EconomyUtils.GetTotalStorageUsed(resources);
          int x = limit.m_Limit - totalStorageUsed;
          // ISSUE: reference to a compiler-generated field
          if (this.m_Attached.HasComponent(property))
          {
            // ISSUE: reference to a compiler-generated field
            Entity parent = this.m_Attached[property].m_Parent;
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Game.Areas.SubArea> subArea = this.m_SubAreas[parent];
            float concentration;
            int areaIndex;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            int num1 = ExtractorCompanySystem.GetBestConcentration(industrialProcessData.m_Output.m_Resource, subArea, this.m_ExtractorAreas, this.m_Prefabs, this.m_ExtractorAreaDatas, this.m_ExtractorParameters, this.m_ResourcePrefabs, this.m_ResourceDatas, out concentration, out areaIndex) ? 1 : 0;
            float buildingEfficiency = 1f;
            DynamicBuffer<Efficiency> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_BuildingEfficiencies.TryGetBuffer(property, out bufferData))
            {
              BuildingUtils.SetEfficiencyFactor(bufferData, EfficiencyFactor.NaturalResources, concentration);
              buildingEfficiency = BuildingUtils.GetEfficiency(bufferData);
            }
            if (num1 != 0)
            {
              // ISSUE: reference to a compiler-generated field
              Entity area = this.m_SubAreas[parent][areaIndex].m_Area;
              // ISSUE: reference to a compiler-generated field
              Extractor extractorArea = this.m_ExtractorAreas[area];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              int productionPerDay = EconomyUtils.GetCompanyProductionPerDay(buildingEfficiency, true, bufferAccessor2[index], industrialProcessData, this.m_ResourcePrefabs, this.m_ResourceDatas, this.m_Citizens, ref this.m_EconomyParameters);
              int intRandom = MathUtils.RoundToIntRandom(ref random, 1f * (float) productionPerDay / (float) EconomyUtils.kCompanyUpdatesPerDay);
              int num2 = math.min(x, intRandom);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_ResourceDatas[this.m_ResourcePrefabs[industrialProcessData.m_Output.m_Resource]].m_RequireNaturalResource)
              {
                float b = extractorArea.m_ResourceAmount - extractorArea.m_ExtractedAmount;
                num2 = math.clamp(num2, 0, (int) b);
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              profitability.m_Profitability = (byte) math.min((float) byte.MaxValue, (float) ((double) productionPerDay * (double) EconomyUtils.GetIndustrialPrice(industrialProcessData.m_Output.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas) / 100.0));
              nativeArray2[index] = profitability;
              // ISSUE: reference to a compiler-generated method
              float num3 = this.GetExtractionMultiplier(area) * (float) num2;
              extractorArea.m_ExtractedAmount += num3;
              extractorArea.m_TotalExtracted += num3;
              extractorArea.m_WorkAmount += (float) num2;
              // ISSUE: reference to a compiler-generated field
              this.m_ExtractorAreas[area] = extractorArea;
              ResourceStack output = industrialProcessData.m_Output;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              int industrialTaxRate = TaxSystem.GetIndustrialTaxRate(output.m_Resource, this.m_TaxRates);
              if (num2 > 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                int num4 = (int) math.ceil(math.max(0.0f, (float) num2 * EconomyUtils.GetIndustrialPrice(industrialProcessData.m_Output.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas)));
                ref TaxPayer local = ref nativeArray4.ElementAt<TaxPayer>(index);
                if (num4 > 0)
                  local.m_AverageTaxRate = (int) math.round(math.lerp((float) local.m_AverageTaxRate, (float) industrialTaxRate, (float) num4 / (float) (num4 + local.m_UntaxedIncome)));
                local.m_UntaxedIncome += num4;
              }
              int num5 = EconomyUtils.AddResources(output.m_Resource, num2, resources);
              if (x < 100 || num5 > 3 * limit.m_Limit / 4)
              {
                int max;
                // ISSUE: reference to a compiler-generated field
                this.m_DeliveryTruckSelectData.GetCapacityRange(output.m_Resource, out int _, out max);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<ResourceExporter>(unfilteredChunkIndex, entity, new ResourceExporter()
                {
                  m_Resource = output.m_Resource,
                  m_Amount = math.min(max, limit.m_Limit / 2)
                });
              }
            }
          }
        }
      }

      private float GetExtractionMultiplier(Entity subArea)
      {
        float extractionMultiplier = 1f;
        PrefabRef componentData1;
        ExtractorAreaData componentData2;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_Prefabs.TryGetComponent(subArea, out componentData1) && this.m_ExtractorAreaDatas.TryGetComponent((Entity) componentData1, out componentData2))
        {
          if (componentData2.m_MapFeature == MapFeature.FertileLand)
          {
            // ISSUE: reference to a compiler-generated field
            extractionMultiplier = this.m_ExtractorParameters.m_FertilityConsumption;
          }
          else if (componentData2.m_MapFeature == MapFeature.Forest)
          {
            // ISSUE: reference to a compiler-generated field
            extractionMultiplier = this.m_ExtractorParameters.m_ForestConsumption;
          }
          else if (componentData2.m_MapFeature == MapFeature.Oil)
            extractionMultiplier = 1f;
          else if (componentData2.m_MapFeature == MapFeature.Ore)
            extractionMultiplier = 1f;
        }
        return extractionMultiplier;
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
      public BufferTypeHandle<Resources> __Game_Economy_Resources_RW_BufferTypeHandle;
      public ComponentLookup<Extractor> __Game_Areas_Extractor_RW_ComponentLookup;
      public ComponentTypeHandle<CompanyData> __Game_Companies_CompanyData_RW_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Employee> __Game_Companies_Employee_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentTypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      public ComponentTypeHandle<TaxPayer> __Game_Agents_TaxPayer_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Profitability> __Game_Companies_Profitability_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> __Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<StorageLimitData> __Game_Companies_StorageLimitData_RO_ComponentLookup;
      public BufferLookup<Efficiency> __Game_Buildings_Efficiency_RW_BufferLookup;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> __Game_Prefabs_WorkplaceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SpawnableBuildingData> __Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attached> __Game_Objects_Attached_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> __Game_Areas_SubArea_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ExtractorAreaData> __Game_Prefabs_ExtractorAreaData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferTypeHandle = state.GetBufferTypeHandle<Resources>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_Extractor_RW_ComponentLookup = state.GetComponentLookup<Extractor>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_CompanyData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CompanyData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Employee_RO_BufferTypeHandle = state.GetBufferTypeHandle<Employee>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Agents_TaxPayer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<TaxPayer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Profitability_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Profitability>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup = state.GetComponentLookup<IndustrialProcessData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_StorageLimitData_RO_ComponentLookup = state.GetComponentLookup<StorageLimitData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RW_BufferLookup = state.GetBufferLookup<Efficiency>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WorkplaceData_RO_ComponentLookup = state.GetComponentLookup<WorkplaceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnableBuildingData_RO_ComponentLookup = state.GetComponentLookup<SpawnableBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentLookup = state.GetComponentLookup<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Areas.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ExtractorAreaData_RO_ComponentLookup = state.GetComponentLookup<ExtractorAreaData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
      }
    }
  }
}
