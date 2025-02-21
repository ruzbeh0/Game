// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CompanyMoveAwaySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Agents;
using Game.Buildings;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Notifications;
using Game.Prefabs;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class CompanyMoveAwaySystem : GameSystemBase
  {
    public static readonly int kUpdatesPerDay = 16;
    private EntityQuery m_CompanyQuery;
    private EntityQuery m_MovingAwayQuery;
    private EntityQuery m_EconomyParameterQuery;
    private EntityArchetype m_RentEventArchetype;
    private SimulationSystem m_SimulationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private ResourceSystem m_ResourceSystem;
    private TaxSystem m_TaxSystem;
    private IconCommandSystem m_IconCommandSystem;
    private CompanyMoveAwaySystem.TypeHandle __TypeHandle;
    private EntityQuery __query_731167828_0;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / (CompanyMoveAwaySystem.kUpdatesPerDay * 16);
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
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TaxSystem = this.World.GetOrCreateSystemManaged<TaxSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CompanyQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Companies.ProcessingCompany>(), ComponentType.ReadOnly<PropertyRenter>(), ComponentType.ReadOnly<WorkProvider>(), ComponentType.ReadOnly<Resources>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Game.Companies.ExtractorCompany>(), ComponentType.Exclude<MovingAway>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_MovingAwayQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Companies.ProcessingCompany>(), ComponentType.ReadOnly<MovingAway>(), ComponentType.ReadOnly<PropertyRenter>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_RentEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<RentersUpdated>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.RequireAnyForUpdate(this.m_CompanyQuery, this.m_MovingAwayQuery);
      this.RequireForUpdate<WorkProviderParameterData>();
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EconomyParameterQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, CompanyMoveAwaySystem.kUpdatesPerDay, 16);
      JobHandle jobHandle1 = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CompanyQuery.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_OfficeProperty_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Companies_ServiceAvailable_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Economy_Resources_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: reference to a compiler-generated field
        jobHandle1 = new CompanyMoveAwaySystem.CheckMoveAwayJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_ResourceType = this.__TypeHandle.__Game_Economy_Resources_RO_BufferTypeHandle,
          m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
          m_PropertyRenterType = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle,
          m_DeliveryTrucks = this.__TypeHandle.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup,
          m_OwnedVehicles = this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RO_BufferLookup,
          m_LayoutElements = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferLookup,
          m_IndustrialProcessDatas = this.__TypeHandle.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup,
          m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
          m_WorkplaceDatas = this.__TypeHandle.__Game_Prefabs_WorkplaceData_RO_ComponentLookup,
          m_ServiceAvailables = this.__TypeHandle.__Game_Companies_ServiceAvailable_RO_ComponentLookup,
          m_OfficeProperties = this.__TypeHandle.__Game_Buildings_OfficeProperty_RO_ComponentLookup,
          m_WorkProviders = this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentLookup,
          m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs(),
          m_EconomyParameters = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>(),
          m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter(),
          m_TaxRates = this.m_TaxSystem.GetTaxRates(),
          m_RandomSeed = RandomSeed.Next(),
          m_UpdateFrameIndex = updateFrame
        }.ScheduleParallel<CompanyMoveAwaySystem.CheckMoveAwayJob>(this.m_CompanyQuery, this.Dependency);
        // ISSUE: reference to a compiler-generated field
        this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ResourceSystem.AddPrefabsReader(jobHandle1);
        this.Dependency = jobHandle1;
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_MovingAwayQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyOnMarket_RO_ComponentLookup.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle2 = new CompanyMoveAwaySystem.MovingAwayJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_RenterType = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle,
        m_OnMarkets = this.__TypeHandle.__Game_Buildings_PropertyOnMarket_RO_ComponentLookup,
        m_WorkProviders = this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentLookup,
        m_RentEventArchetype = this.m_RentEventArchetype,
        m_WorkProviderParameterData = this.__query_731167828_0.GetSingleton<WorkProviderParameterData>(),
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer(),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<CompanyMoveAwaySystem.MovingAwayJob>(this.m_MovingAwayQuery, JobHandle.CombineDependencies(jobHandle1, this.Dependency));
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(jobHandle2);
      this.Dependency = JobHandle.CombineDependencies(jobHandle2, jobHandle1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void __AssignQueries(ref SystemState state)
    {
      // ISSUE: reference to a compiler-generated field
      this.__query_731167828_0 = state.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<WorkProviderParameterData>()
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
    public CompanyMoveAwaySystem()
    {
    }

    [BurstCompile]
    private struct CheckMoveAwayJob : IJobChunk
    {
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> m_PropertyRenterType;
      [ReadOnly]
      public BufferTypeHandle<Resources> m_ResourceType;
      [ReadOnly]
      public BufferLookup<OwnedVehicle> m_OwnedVehicles;
      [ReadOnly]
      public BufferLookup<LayoutElement> m_LayoutElements;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.DeliveryTruck> m_DeliveryTrucks;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> m_WorkplaceDatas;
      [ReadOnly]
      public ComponentLookup<ServiceAvailable> m_ServiceAvailables;
      [ReadOnly]
      public ComponentLookup<OfficeProperty> m_OfficeProperties;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> m_IndustrialProcessDatas;
      [ReadOnly]
      public ComponentLookup<WorkProvider> m_WorkProviders;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;
      [ReadOnly]
      public EconomyParameterData m_EconomyParameters;
      [ReadOnly]
      public NativeArray<int> m_TaxRates;
      public RandomSeed m_RandomSeed;
      public uint m_UpdateFrameIndex;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

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
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PropertyRenter> nativeArray3 = chunk.GetNativeArray<PropertyRenter>(ref this.m_PropertyRenterType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Resources> bufferAccessor = chunk.GetBufferAccessor<Resources>(ref this.m_ResourceType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          DynamicBuffer<Resources> resources = bufferAccessor[index];
          Entity entity = nativeArray1[index];
          Entity prefab = nativeArray2[index].m_Prefab;
          Entity property = nativeArray3[index].m_Property;
          // ISSUE: reference to a compiler-generated field
          if (this.m_WorkplaceDatas.HasComponent(prefab))
          {
            int companyTotalWorth;
            // ISSUE: reference to a compiler-generated field
            if (this.m_OwnedVehicles.HasBuffer(entity))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<OwnedVehicle> ownedVehicle = this.m_OwnedVehicles[entity];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              companyTotalWorth = EconomyUtils.GetCompanyTotalWorth(resources, ownedVehicle, this.m_LayoutElements, this.m_DeliveryTrucks, this.m_ResourcePrefabs, this.m_ResourceDatas);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              companyTotalWorth = EconomyUtils.GetCompanyTotalWorth(resources, this.m_ResourcePrefabs, this.m_ResourceDatas);
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            int companyMoveAwayChance = CompanyUtils.GetCompanyMoveAwayChance(entity, prefab, property, ref this.m_ServiceAvailables, ref this.m_OfficeProperties, ref this.m_IndustrialProcessDatas, ref this.m_WorkProviders, this.m_TaxRates);
            // ISSUE: reference to a compiler-generated field
            if (companyTotalWorth < this.m_EconomyParameters.m_CompanyBankruptcyLimit || random.NextInt(100) < companyMoveAwayChance)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<MovingAway>(unfilteredChunkIndex, entity, new MovingAway());
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

    [BurstCompile]
    private struct MovingAwayJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> m_RenterType;
      [ReadOnly]
      public ComponentLookup<PropertyOnMarket> m_OnMarkets;
      [ReadOnly]
      public ComponentLookup<WorkProvider> m_WorkProviders;
      [ReadOnly]
      public EntityArchetype m_RentEventArchetype;
      [ReadOnly]
      public WorkProviderParameterData m_WorkProviderParameterData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public IconCommandBuffer m_IconCommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PropertyRenter> nativeArray2 = chunk.GetNativeArray<PropertyRenter>(ref this.m_RenterType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity1 = nativeArray1[index];
          Entity property = nativeArray2[index].m_Property;
          if (property != Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            if (!this.m_OnMarkets.HasComponent(property))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<PropertyToBeOnMarket>(unfilteredChunkIndex, property, new PropertyToBeOnMarket());
            }
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity entity2 = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex, this.m_RentEventArchetype);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<RentersUpdated>(unfilteredChunkIndex, entity2, new RentersUpdated(property));
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_WorkProviders.HasComponent(entity1))
          {
            // ISSUE: reference to a compiler-generated field
            WorkProvider workProvider = this.m_WorkProviders[entity1];
            if (workProvider.m_EducatedNotificationEntity != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_IconCommandBuffer.Remove(workProvider.m_EducatedNotificationEntity, this.m_WorkProviderParameterData.m_EducatedNotificationPrefab);
            }
            if (workProvider.m_UneducatedNotificationEntity != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_IconCommandBuffer.Remove(workProvider.m_UneducatedNotificationEntity, this.m_WorkProviderParameterData.m_UneducatedNotificationPrefab);
            }
          }
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(unfilteredChunkIndex, entity1, new Deleted());
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

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Resources> __Game_Economy_Resources_RO_BufferTypeHandle;
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.DeliveryTruck> __Game_Vehicles_DeliveryTruck_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<OwnedVehicle> __Game_Vehicles_OwnedVehicle_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<IndustrialProcessData> __Game_Prefabs_IndustrialProcessData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WorkplaceData> __Game_Prefabs_WorkplaceData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceAvailable> __Game_Companies_ServiceAvailable_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<OfficeProperty> __Game_Buildings_OfficeProperty_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WorkProvider> __Game_Companies_WorkProvider_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PropertyOnMarket> __Game_Buildings_PropertyOnMarket_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RO_BufferTypeHandle = state.GetBufferTypeHandle<Resources>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_DeliveryTruck_RO_ComponentLookup = state.GetComponentLookup<Game.Vehicles.DeliveryTruck>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_OwnedVehicle_RO_BufferLookup = state.GetBufferLookup<OwnedVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferLookup = state.GetBufferLookup<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_IndustrialProcessData_RO_ComponentLookup = state.GetComponentLookup<IndustrialProcessData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WorkplaceData_RO_ComponentLookup = state.GetComponentLookup<WorkplaceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_ServiceAvailable_RO_ComponentLookup = state.GetComponentLookup<ServiceAvailable>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_OfficeProperty_RO_ComponentLookup = state.GetComponentLookup<OfficeProperty>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_WorkProvider_RO_ComponentLookup = state.GetComponentLookup<WorkProvider>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyOnMarket_RO_ComponentLookup = state.GetComponentLookup<PropertyOnMarket>(true);
      }
    }
  }
}
