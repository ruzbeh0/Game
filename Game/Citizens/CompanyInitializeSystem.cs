// Decompiled with JetBrains decompiler
// Type: Game.Citizens.CompanyInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Citizens
{
  [CompilerGenerated]
  public class CompanyInitializeSystem : GameSystemBase
  {
    private ResourceSystem m_ResourceSystem;
    private PropertyProcessingSystem m_PropertyProcessingSystem;
    private EntityQuery m_CreatedGroup;
    private CompanyInitializeSystem.TypeHandle __TypeHandle;
    private EntityQuery __query_1030701297_0;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PropertyProcessingSystem = this.World.GetOrCreateSystemManaged<PropertyProcessingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedGroup = this.GetEntityQuery(ComponentType.ReadWrite<CompanyData>(), ComponentType.ReadWrite<Profitability>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Created>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CreatedGroup);
      this.RequireForUpdate<EconomyParameterData>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_ServiceCompanyData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CompanyBrandElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_LodgingProvider_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_ServiceAvailable_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_Profitability_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_CompanyData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_ProcessingCompany_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CompanyInitializeSystem.InitializeCompanyJob jobData = new CompanyInitializeSystem.InitializeCompanyJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_ProcessingCompanyType = this.__TypeHandle.__Game_Companies_ProcessingCompany_RO_ComponentTypeHandle,
        m_CompanyType = this.__TypeHandle.__Game_Companies_CompanyData_RW_ComponentTypeHandle,
        m_ProfitabilityType = this.__TypeHandle.__Game_Companies_Profitability_RW_ComponentTypeHandle,
        m_ResourcesType = this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle,
        m_ServiceAvailableType = this.__TypeHandle.__Game_Companies_ServiceAvailable_RW_ComponentTypeHandle,
        m_LodgingProviderType = this.__TypeHandle.__Game_Companies_LodgingProvider_RW_ComponentTypeHandle,
        m_Brands = this.__TypeHandle.__Game_Prefabs_CompanyBrandElement_RO_BufferLookup,
        m_ProcessDatas = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
        m_ServiceCompanyDatas = this.__TypeHandle.__Game_Companies_ServiceCompanyData_RO_ComponentLookup,
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_PropertyRenters = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
        m_RandomSeed = RandomSeed.Next(),
        m_EconomyParameters = this.__query_1030701297_0.GetSingleton<EconomyParameterData>(),
        m_RentActionQueue = this.m_PropertyProcessingSystem.GetRentActionQueue(out deps).AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<CompanyInitializeSystem.InitializeCompanyJob>(this.m_CreatedGroup, JobHandle.CombineDependencies(this.Dependency, deps));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PropertyProcessingSystem.AddWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ResourceSystem.AddPrefabsReader(this.Dependency);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_1030701297_0 = state.GetEntityQuery(new EntityQueryDesc()
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
    public CompanyInitializeSystem()
    {
    }

    [BurstCompile]
    private struct InitializeCompanyJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Companies.ProcessingCompany> m_ProcessingCompanyType;
      public ComponentTypeHandle<CompanyData> m_CompanyType;
      public ComponentTypeHandle<Profitability> m_ProfitabilityType;
      public BufferTypeHandle<Resources> m_ResourcesType;
      public ComponentTypeHandle<ServiceAvailable> m_ServiceAvailableType;
      public ComponentTypeHandle<LodgingProvider> m_LodgingProviderType;
      [ReadOnly]
      public BufferLookup<CompanyBrandElement> m_Brands;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_ProcessDatas;
      [ReadOnly]
      public ComponentLookup<ServiceCompanyData> m_ServiceCompanyDatas;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_PropertyRenters;
      public EconomyParameterData m_EconomyParameters;
      public NativeQueue<RentAction>.ParallelWriter m_RentActionQueue;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      public RandomSeed m_RandomSeed;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CompanyData> nativeArray3 = chunk.GetNativeArray<CompanyData>(ref this.m_CompanyType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Profitability> nativeArray4 = chunk.GetNativeArray<Profitability>(ref this.m_ProfitabilityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Resources> bufferAccessor = chunk.GetBufferAccessor<Resources>(ref this.m_ResourcesType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ServiceAvailable> nativeArray5 = chunk.GetNativeArray<ServiceAvailable>(ref this.m_ServiceAvailableType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<LodgingProvider> nativeArray6 = chunk.GetNativeArray<LodgingProvider>(ref this.m_LodgingProviderType);
        bool flag1 = nativeArray5.Length != 0;
        // ISSUE: reference to a compiler-generated field
        bool flag2 = chunk.Has<Game.Companies.ProcessingCompany>(ref this.m_ProcessingCompanyType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity entity1 = nativeArray1[index];
          Entity prefab = nativeArray2[index].m_Prefab;
          // ISSUE: reference to a compiler-generated field
          Random random = this.m_RandomSeed.GetRandom(entity1.Index);
          // ISSUE: reference to a compiler-generated field
          DynamicBuffer<CompanyBrandElement> brand = this.m_Brands[prefab];
          Entity entity2 = brand.Length != 0 ? brand[random.NextInt(brand.Length)].m_Brand : Entity.Null;
          nativeArray3[index] = new CompanyData()
          {
            m_RandomSeed = random,
            m_Brand = entity2
          };
          nativeArray4[index] = new Profitability()
          {
            m_Profitability = (byte) 127
          };
          if (flag1)
          {
            // ISSUE: reference to a compiler-generated field
            ServiceCompanyData serviceCompanyData = this.m_ServiceCompanyDatas[prefab];
            nativeArray5[index] = new ServiceAvailable()
            {
              m_ServiceAvailable = serviceCompanyData.m_MaxService / 2,
              m_MeanPriority = 0.0f
            };
          }
          if (flag2)
          {
            // ISSUE: reference to a compiler-generated field
            IndustrialProcessData processData = this.m_ProcessDatas[prefab];
            DynamicBuffer<Resources> buffer = bufferAccessor[index];
            if (flag1)
            {
              // ISSUE: reference to a compiler-generated method
              this.AddStartingResources(buffer, processData.m_Input1.m_Resource, 3000);
              // ISSUE: reference to a compiler-generated method
              this.AddStartingResources(buffer, processData.m_Input2.m_Resource, 3000);
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              this.AddStartingResources(buffer, processData.m_Input1.m_Resource, 15000);
              // ISSUE: reference to a compiler-generated method
              this.AddStartingResources(buffer, processData.m_Input2.m_Resource, 15000);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              bool flag3 = EconomyUtils.IsMaterial(processData.m_Output.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas);
              // ISSUE: reference to a compiler-generated method
              this.AddStartingResources(buffer, processData.m_Output.m_Resource, flag3 ? 1000 : 0);
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PropertyRenters.HasComponent(entity1) && this.m_PropertyRenters[entity1].m_Property != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_RentActionQueue.Enqueue(new RentAction()
            {
              m_Property = this.m_PropertyRenters[entity1].m_Property,
              m_Renter = entity1
            });
          }
        }
        for (int index = 0; index < nativeArray6.Length; ++index)
          nativeArray6[index] = new LodgingProvider()
          {
            m_FreeRooms = 0,
            m_Price = -1
          };
      }

      private void AddStartingResources(
        DynamicBuffer<Resources> buffer,
        Resource resource,
        int amount)
      {
        if (resource == Resource.NoResource)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        int num = (int) math.round((float) amount * EconomyUtils.GetIndustrialPrice(resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas));
        EconomyUtils.AddResources(resource, amount, buffer);
        EconomyUtils.AddResources(Resource.Money, -num, buffer);
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
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Companies.ProcessingCompany> __Game_Companies_ProcessingCompany_RO_ComponentTypeHandle;
      public ComponentTypeHandle<CompanyData> __Game_Companies_CompanyData_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Profitability> __Game_Companies_Profitability_RW_ComponentTypeHandle;
      public BufferTypeHandle<Resources> __Game_Economy_Resources_RW_BufferTypeHandle;
      public ComponentTypeHandle<ServiceAvailable> __Game_Companies_ServiceAvailable_RW_ComponentTypeHandle;
      public ComponentTypeHandle<LodgingProvider> __Game_Companies_LodgingProvider_RW_ComponentTypeHandle;
      [ReadOnly]
      public BufferLookup<CompanyBrandElement> __Game_Prefabs_CompanyBrandElement_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> __Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceCompanyData> __Game_Companies_ServiceCompanyData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_ProcessingCompany_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Companies.ProcessingCompany>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_CompanyData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<CompanyData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_Profitability_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Profitability>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferTypeHandle = state.GetBufferTypeHandle<Resources>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_ServiceAvailable_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ServiceAvailable>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_LodgingProvider_RW_ComponentTypeHandle = state.GetComponentTypeHandle<LodgingProvider>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CompanyBrandElement_RO_BufferLookup = state.GetBufferLookup<CompanyBrandElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup = state.GetComponentLookup<IndustrialProcessData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_ServiceCompanyData_RO_ComponentLookup = state.GetComponentLookup<ServiceCompanyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
      }
    }
  }
}
