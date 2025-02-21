// Decompiled with JetBrains decompiler
// Type: Game.Policies.ModifiedSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Prefabs;
using Game.Routes;
using Game.Simulation;
using Game.Tools;
using Game.Triggers;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Policies
{
  [CompilerGenerated]
  public class ModifiedSystem : GameSystemBase
  {
    private EntityQuery m_EventQuery;
    private EntityQuery m_EffectProviderQuery;
    private ModificationBarrier4 m_ModificationBarrier;
    private TriggerSystem m_TriggerSystem;
    private NativeQueue<ModifiedSystem.PolicyEventInfo> m_PolicyEventInfos;
    private DistrictModifierInitializeSystem.DistrictModifierRefreshData m_DistrictModifierRefreshData;
    private BuildingModifierInitializeSystem.BuildingModifierRefreshData m_BuildingModifierRefreshData;
    private RouteModifierInitializeSystem.RouteModifierRefreshData m_RouteModifierRefreshData;
    private CityModifierUpdateSystem.CityModifierRefreshData m_CityModifierRefreshData;
    private Entity m_TicketPricePolicy;
    private ModifiedSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PolicyEventInfos = new NativeQueue<ModifiedSystem.PolicyEventInfo>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_DistrictModifierRefreshData = new DistrictModifierInitializeSystem.DistrictModifierRefreshData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_BuildingModifierRefreshData = new BuildingModifierInitializeSystem.BuildingModifierRefreshData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_RouteModifierRefreshData = new RouteModifierInitializeSystem.RouteModifierRefreshData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_CityModifierRefreshData = new CityModifierUpdateSystem.CityModifierRefreshData((SystemBase) this);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      EntityQuery entityQuery = this.GetEntityQuery(ComponentType.ReadOnly<UITransportConfigurationData>());
      // ISSUE: reference to a compiler-generated method
      UITransportConfigurationPrefab singletonPrefab = systemManaged.GetSingletonPrefab<UITransportConfigurationPrefab>(entityQuery);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_TicketPricePolicy = systemManaged.GetEntity((PrefabBase) singletonPrefab.m_TicketPricePolicy);
      // ISSUE: reference to a compiler-generated field
      this.m_EventQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Common.Event>(), ComponentType.ReadOnly<Modify>());
      // ISSUE: reference to a compiler-generated field
      this.m_EffectProviderQuery = this.GetEntityQuery(ComponentType.ReadOnly<CityEffectProvider>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EventQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_EffectProviderQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DistrictModifierRefreshData.Update((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_BuildingModifierRefreshData.Update((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_RouteModifierRefreshData.Update((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityModifierRefreshData.Update((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeQueue<TriggerAction> nativeQueue = this.m_TriggerSystem.Enabled ? this.m_TriggerSystem.CreateActionBuffer() : new NativeQueue<TriggerAction>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Policies_Policy_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteModifier_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_BuildingModifier_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_DistrictModifier_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_City_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Route_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Extension_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_District_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Policies_Modify_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new ModifiedSystem.ModifyPolicyJob()
      {
        m_DistrictModifierRefreshData = this.m_DistrictModifierRefreshData,
        m_BuildingModifierRefreshData = this.m_BuildingModifierRefreshData,
        m_RouteModifierRefreshData = this.m_RouteModifierRefreshData,
        m_CityModifierRefreshData = this.m_CityModifierRefreshData,
        m_EffectProviderChunks = archetypeChunkListAsync,
        m_ModifyType = this.__TypeHandle.__Game_Policies_Modify_RO_ComponentTypeHandle,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_ServiceUpgradeData = this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup,
        m_TriggerBuffer = nativeQueue.AsParallelWriter(),
        m_DistrictData = this.__TypeHandle.__Game_Areas_District_RW_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RW_ComponentLookup,
        m_ExtensionData = this.__TypeHandle.__Game_Buildings_Extension_RW_ComponentLookup,
        m_RouteData = this.__TypeHandle.__Game_Routes_Route_RW_ComponentLookup,
        m_CityData = this.__TypeHandle.__Game_City_City_RW_ComponentLookup,
        m_DistrictModifiers = this.__TypeHandle.__Game_Areas_DistrictModifier_RW_BufferLookup,
        m_BuildingModifiers = this.__TypeHandle.__Game_Buildings_BuildingModifier_RW_BufferLookup,
        m_RouteModifiers = this.__TypeHandle.__Game_Routes_RouteModifier_RW_BufferLookup,
        m_CityModifiers = this.__TypeHandle.__Game_City_CityModifier_RW_BufferLookup,
        m_Policies = this.__TypeHandle.__Game_Policies_Policy_RW_BufferLookup,
        m_PolicyEventInfos = this.m_PolicyEventInfos.AsParallelWriter(),
        m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer(),
        m_TicketPricePolicy = this.m_TicketPricePolicy
      }.Schedule<ModifiedSystem.ModifyPolicyJob>(this.m_EventQuery, JobHandle.CombineDependencies(this.Dependency, outJobHandle));
      archetypeChunkListAsync.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier.AddJobHandleForProducer(jobHandle);
      // ISSUE: reference to a compiler-generated field
      if (this.m_TriggerSystem.Enabled)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_TriggerSystem.AddActionBufferWriter(jobHandle);
      }
      else
        nativeQueue.Dispose(jobHandle);
      this.Dependency = jobHandle;
      jobHandle.Complete();
      // ISSUE: reference to a compiler-generated field
      while (this.m_PolicyEventInfos.Count > 0)
      {
        // ISSUE: reference to a compiler-generated field
        Game.PSI.Telemetry.Policy(this.m_PolicyEventInfos.Dequeue());
      }
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy() => this.m_PolicyEventInfos.Dispose();

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
    public ModifiedSystem()
    {
    }

    public enum PolicyRange
    {
      None,
      Building,
      District,
      City,
      Route,
    }

    public struct PolicyEventInfo
    {
      public bool m_Activated;
      public Entity m_Entity;
      public ModifiedSystem.PolicyRange m_PolicyRange;
    }

    [BurstCompile]
    private struct ModifyPolicyJob : IJobChunk
    {
      [ReadOnly]
      public DistrictModifierInitializeSystem.DistrictModifierRefreshData m_DistrictModifierRefreshData;
      [ReadOnly]
      public BuildingModifierInitializeSystem.BuildingModifierRefreshData m_BuildingModifierRefreshData;
      [ReadOnly]
      public RouteModifierInitializeSystem.RouteModifierRefreshData m_RouteModifierRefreshData;
      [ReadOnly]
      public CityModifierUpdateSystem.CityModifierRefreshData m_CityModifierRefreshData;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_EffectProviderChunks;
      [ReadOnly]
      public ComponentTypeHandle<Modify> m_ModifyType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.ServiceUpgrade> m_ServiceUpgradeData;
      [ReadOnly]
      public Entity m_TicketPricePolicy;
      public ComponentLookup<District> m_DistrictData;
      public ComponentLookup<Building> m_BuildingData;
      public ComponentLookup<Extension> m_ExtensionData;
      public ComponentLookup<Route> m_RouteData;
      public ComponentLookup<Game.City.City> m_CityData;
      public BufferLookup<DistrictModifier> m_DistrictModifiers;
      public BufferLookup<BuildingModifier> m_BuildingModifiers;
      public BufferLookup<RouteModifier> m_RouteModifiers;
      public BufferLookup<CityModifier> m_CityModifiers;
      public BufferLookup<Policy> m_Policies;
      public EntityCommandBuffer m_CommandBuffer;
      public NativeQueue<TriggerAction>.ParallelWriter m_TriggerBuffer;
      public NativeQueue<ModifiedSystem.PolicyEventInfo>.ParallelWriter m_PolicyEventInfos;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Modify> nativeArray = chunk.GetNativeArray<Modify>(ref this.m_ModifyType);
        for (int index1 = 0; index1 < nativeArray.Length; ++index1)
        {
          Modify modify = nativeArray[index1];
          DynamicBuffer<Policy> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_Policies.TryGetBuffer(modify.m_Entity, out bufferData))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Updated>(modify.m_Entity);
            for (int index2 = 0; index2 < bufferData.Length; ++index2)
            {
              Policy policy = bufferData[index2];
              if (policy.m_Policy == modify.m_Policy)
              {
                if ((modify.m_Flags & PolicyFlags.Active) == (PolicyFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CheckFreeBusTicketEventTrigger(policy);
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_DistrictModifierRefreshData.m_PolicySliderData.HasComponent(policy.m_Policy))
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    if ((double) this.m_DistrictModifierRefreshData.m_PolicySliderData[policy.m_Policy].m_Default == (double) policy.m_Adjustment)
                    {
                      bufferData.RemoveAt(index2);
                      // ISSUE: reference to a compiler-generated method
                      this.RefreshEffects(modify.m_Entity, modify.m_Policy, bufferData);
                      // ISSUE: reference to a compiler-generated field
                      // ISSUE: reference to a compiler-generated method
                      // ISSUE: object of a compiler-generated type is created
                      this.m_PolicyEventInfos.Enqueue(new ModifiedSystem.PolicyEventInfo()
                      {
                        m_Activated = false,
                        m_Entity = modify.m_Policy,
                        m_PolicyRange = this.GetPolicyRange(modify.m_Entity, modify.m_Policy)
                      });
                      goto label_19;
                    }
                  }
                  else
                  {
                    bufferData.RemoveAt(index2);
                    // ISSUE: reference to a compiler-generated method
                    this.RefreshEffects(modify.m_Entity, modify.m_Policy, bufferData);
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    // ISSUE: object of a compiler-generated type is created
                    this.m_PolicyEventInfos.Enqueue(new ModifiedSystem.PolicyEventInfo()
                    {
                      m_Activated = false,
                      m_Entity = modify.m_Policy,
                      m_PolicyRange = this.GetPolicyRange(modify.m_Entity, modify.m_Policy)
                    });
                    goto label_19;
                  }
                }
                policy.m_Flags = modify.m_Flags;
                policy.m_Adjustment = modify.m_Adjustment;
                bufferData[index2] = policy;
                // ISSUE: reference to a compiler-generated method
                this.RefreshEffects(modify.m_Entity, modify.m_Policy, bufferData);
                goto label_19;
              }
            }
            if ((modify.m_Flags & PolicyFlags.Active) != (PolicyFlags) 0)
            {
              bufferData.Add(new Policy(modify.m_Policy, modify.m_Flags, modify.m_Adjustment));
              // ISSUE: reference to a compiler-generated method
              this.RefreshEffects(modify.m_Entity, modify.m_Policy, bufferData);
              // ISSUE: reference to a compiler-generated field
              this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.PolicyActivated, modify.m_Policy, Entity.Null, Entity.Null));
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              // ISSUE: object of a compiler-generated type is created
              this.m_PolicyEventInfos.Enqueue(new ModifiedSystem.PolicyEventInfo()
              {
                m_Activated = true,
                m_Entity = modify.m_Policy,
                m_PolicyRange = this.GetPolicyRange(modify.m_Entity, modify.m_Policy)
              });
            }
          }
          else
          {
            Extension componentData1;
            BuildingOptionData componentData2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ExtensionData.TryGetComponent(modify.m_Entity, out componentData1) && this.m_BuildingModifierRefreshData.m_BuildingOptionData.TryGetComponent(modify.m_Policy, out componentData2) && BuildingUtils.HasOption(componentData2, BuildingOption.Inactive))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<Updated>(modify.m_Entity);
              if ((modify.m_Flags & PolicyFlags.Active) != (PolicyFlags) 0)
                componentData1.m_Flags |= ExtensionFlags.Disabled;
              else
                componentData1.m_Flags &= ~ExtensionFlags.Disabled;
              // ISSUE: reference to a compiler-generated field
              this.m_ExtensionData[modify.m_Entity] = componentData1;
            }
          }
label_19:
          Owner componentData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_ServiceUpgradeData.HasComponent(modify.m_Entity) && this.m_OwnerData.TryGetComponent(modify.m_Entity, out componentData))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Updated>(componentData.m_Owner);
          }
        }
      }

      private void CheckFreeBusTicketEventTrigger(Policy policy)
      {
        // ISSUE: reference to a compiler-generated field
        if (!(this.m_TicketPricePolicy == policy.m_Policy))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_TriggerBuffer.Enqueue(new TriggerAction()
        {
          m_TriggerType = TriggerType.FreePublicTransport,
          m_Value = 0.0f,
          m_TriggerPrefab = policy.m_Policy,
          m_SecondaryTarget = Entity.Null,
          m_PrimaryTarget = Entity.Null
        });
      }

      private ModifiedSystem.PolicyRange GetPolicyRange(Entity entity, Entity policy)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_DistrictModifierRefreshData.m_DistrictOptionData.HasComponent(policy) && this.m_DistrictData.HasComponent(entity) || this.m_DistrictModifierRefreshData.m_DistrictModifierData.HasBuffer(policy) && this.m_DistrictModifiers.HasBuffer(entity))
          return ModifiedSystem.PolicyRange.District;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_BuildingModifierRefreshData.m_BuildingOptionData.HasComponent(policy) && this.m_BuildingData.HasComponent(entity) || this.m_BuildingModifierRefreshData.m_BuildingModifierData.HasBuffer(policy) && this.m_BuildingModifiers.HasBuffer(entity))
          return ModifiedSystem.PolicyRange.Building;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_RouteModifierRefreshData.m_RouteOptionData.HasComponent(policy) && this.m_RouteData.HasComponent(entity) || this.m_RouteModifierRefreshData.m_RouteModifierData.HasBuffer(policy) && this.m_RouteModifiers.HasBuffer(entity))
          return ModifiedSystem.PolicyRange.Route;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_CityModifierRefreshData.m_CityOptionData.HasComponent(policy) && this.m_CityData.HasComponent(entity) || this.m_CityModifierRefreshData.m_CityModifierData.HasBuffer(policy) && this.m_CityModifiers.HasBuffer(entity) ? ModifiedSystem.PolicyRange.City : ModifiedSystem.PolicyRange.None;
      }

      private void RefreshEffects(Entity entity, Entity policy, DynamicBuffer<Policy> policies)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_DistrictModifierRefreshData.m_DistrictOptionData.HasComponent(policy) && this.m_DistrictData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          District district = this.m_DistrictData[entity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_DistrictModifierRefreshData.RefreshDistrictOptions(ref district, policies);
          // ISSUE: reference to a compiler-generated field
          this.m_DistrictData[entity] = district;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_DistrictModifierRefreshData.m_DistrictModifierData.HasBuffer(policy) && this.m_DistrictModifiers.HasBuffer(entity))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_DistrictModifierRefreshData.RefreshDistrictModifiers(this.m_DistrictModifiers[entity], policies);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_BuildingModifierRefreshData.m_BuildingOptionData.HasComponent(policy) && this.m_BuildingData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          Building building = this.m_BuildingData[entity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_BuildingModifierRefreshData.RefreshBuildingOptions(ref building, policies);
          // ISSUE: reference to a compiler-generated field
          this.m_BuildingData[entity] = building;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_BuildingModifierRefreshData.m_BuildingModifierData.HasBuffer(policy) && this.m_BuildingModifiers.HasBuffer(entity))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_BuildingModifierRefreshData.RefreshBuildingModifiers(this.m_BuildingModifiers[entity], policies);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_RouteModifierRefreshData.m_RouteOptionData.HasComponent(policy) && this.m_RouteData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          Route route = this.m_RouteData[entity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_RouteModifierRefreshData.RefreshRouteOptions(ref route, policies);
          // ISSUE: reference to a compiler-generated field
          this.m_RouteData[entity] = route;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_RouteModifierRefreshData.m_RouteModifierData.HasBuffer(policy) && this.m_RouteModifiers.HasBuffer(entity))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_RouteModifierRefreshData.RefreshRouteModifiers(this.m_RouteModifiers[entity], policies);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_CityModifierRefreshData.m_CityOptionData.HasComponent(policy) && this.m_CityData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          Game.City.City city = this.m_CityData[entity];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_CityModifierRefreshData.RefreshCityOptions(ref city, policies);
          // ISSUE: reference to a compiler-generated field
          this.m_CityData[entity] = city;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_CityModifierRefreshData.m_CityModifierData.HasBuffer(policy) && this.m_CityModifiers.HasBuffer(entity))
        {
          NativeList<CityModifierData> tempModifierList = new NativeList<CityModifierData>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_CityModifierRefreshData.RefreshCityModifiers(this.m_CityModifiers[entity], policies, this.m_EffectProviderChunks, tempModifierList);
          tempModifierList.Dispose();
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(entity);
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
      public ComponentTypeHandle<Modify> __Game_Policies_Modify_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.ServiceUpgrade> __Game_Buildings_ServiceUpgrade_RO_ComponentLookup;
      public ComponentLookup<District> __Game_Areas_District_RW_ComponentLookup;
      public ComponentLookup<Building> __Game_Buildings_Building_RW_ComponentLookup;
      public ComponentLookup<Extension> __Game_Buildings_Extension_RW_ComponentLookup;
      public ComponentLookup<Route> __Game_Routes_Route_RW_ComponentLookup;
      public ComponentLookup<Game.City.City> __Game_City_City_RW_ComponentLookup;
      public BufferLookup<DistrictModifier> __Game_Areas_DistrictModifier_RW_BufferLookup;
      public BufferLookup<BuildingModifier> __Game_Buildings_BuildingModifier_RW_BufferLookup;
      public BufferLookup<RouteModifier> __Game_Routes_RouteModifier_RW_BufferLookup;
      public BufferLookup<CityModifier> __Game_City_CityModifier_RW_BufferLookup;
      public BufferLookup<Policy> __Game_Policies_Policy_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Policies_Modify_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Modify>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.ServiceUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_District_RW_ComponentLookup = state.GetComponentLookup<District>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RW_ComponentLookup = state.GetComponentLookup<Building>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Extension_RW_ComponentLookup = state.GetComponentLookup<Extension>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Route_RW_ComponentLookup = state.GetComponentLookup<Route>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_City_RW_ComponentLookup = state.GetComponentLookup<Game.City.City>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_DistrictModifier_RW_BufferLookup = state.GetBufferLookup<DistrictModifier>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_BuildingModifier_RW_BufferLookup = state.GetBufferLookup<BuildingModifier>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteModifier_RW_BufferLookup = state.GetBufferLookup<RouteModifier>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RW_BufferLookup = state.GetBufferLookup<CityModifier>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Policies_Policy_RW_BufferLookup = state.GetBufferLookup<Policy>();
      }
    }
  }
}
