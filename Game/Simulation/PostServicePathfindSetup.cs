// Decompiled with JetBrains decompiler
// Type: Game.Simulation.PostServicePathfindSetup
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Pathfind;
using Game.Prefabs;
using Game.Tools;
using Game.Vehicles;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  public struct PostServicePathfindSetup
  {
    private EntityQuery m_PostVanQuery;
    private EntityQuery m_MailTransferQuery;
    private EntityQuery m_MailBoxQuery;
    private EntityQuery m_PostVanRequestQuery;
    private EntityTypeHandle m_EntityType;
    private ComponentTypeHandle<PathOwner> m_PathOwnerType;
    private ComponentTypeHandle<Owner> m_OwnerType;
    private ComponentTypeHandle<Game.Objects.OutsideConnection> m_OutsideConnectionType;
    private ComponentTypeHandle<ServiceRequest> m_ServiceRequestType;
    private ComponentTypeHandle<PostVanRequest> m_PostVanRequestType;
    private ComponentTypeHandle<Game.Buildings.PostFacility> m_PostFacilityType;
    private ComponentTypeHandle<Game.Vehicles.PostVan> m_PostVanType;
    private ComponentTypeHandle<Game.Routes.MailBox> m_MailBoxType;
    private ComponentTypeHandle<Game.Routes.TransportStop> m_TransportStopType;
    private ComponentTypeHandle<PrefabRef> m_PrefabRefType;
    private BufferTypeHandle<PathElement> m_PathElementType;
    private BufferTypeHandle<ServiceDispatch> m_ServiceDispatchType;
    private BufferTypeHandle<Resources> m_ResourcesType;
    private BufferTypeHandle<TradeCost> m_TradeCostType;
    private BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
    private ComponentLookup<PathInformation> m_PathInformationData;
    private ComponentLookup<PostVanRequest> m_PostVanRequestData;
    private ComponentLookup<Game.Buildings.PostFacility> m_PostFacilityData;
    private ComponentLookup<Game.Vehicles.PostVan> m_PostVanData;
    private ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
    private ComponentLookup<StorageLimitData> m_StorageLimitData;
    private ComponentLookup<StorageCompanyData> m_StorageCompanyData;
    private ComponentLookup<MailBoxData> m_MailBoxData;
    private BufferLookup<PathElement> m_PathElements;
    private BufferLookup<ServiceDistrict> m_ServiceDistricts;

    public PostServicePathfindSetup(PathfindSetupSystem system)
    {
      // ISSUE: reference to a compiler-generated method
      this.m_PostVanQuery = system.GetSetupQuery(new EntityQueryDesc()
      {
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Game.Buildings.PostFacility>(),
          ComponentType.ReadOnly<Game.Vehicles.PostVan>()
        },
        None = new ComponentType[4]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<Game.Buildings.ServiceUpgrade>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated method
      this.m_MailTransferQuery = system.GetSetupQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Buildings.PostFacility>(),
          ComponentType.ReadOnly<ServiceDispatch>(),
          ComponentType.ReadOnly<PrefabRef>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<Temp>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[4]
        {
          ComponentType.ReadOnly<Game.Companies.StorageCompany>(),
          ComponentType.ReadOnly<PrefabRef>(),
          ComponentType.ReadOnly<Resources>(),
          ComponentType.ReadOnly<TradeCost>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated method
      this.m_MailBoxQuery = system.GetSetupQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<PrefabRef>()
        },
        Any = new ComponentType[1]
        {
          ComponentType.ReadOnly<Game.Routes.MailBox>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated method
      this.m_PostVanRequestQuery = system.GetSetupQuery(ComponentType.ReadOnly<PostVanRequest>(), ComponentType.Exclude<Dispatched>(), ComponentType.Exclude<PathInformation>());
      this.m_EntityType = system.GetEntityTypeHandle();
      this.m_PathOwnerType = system.GetComponentTypeHandle<PathOwner>(true);
      this.m_OwnerType = system.GetComponentTypeHandle<Owner>(true);
      this.m_OutsideConnectionType = system.GetComponentTypeHandle<Game.Objects.OutsideConnection>(true);
      this.m_ServiceRequestType = system.GetComponentTypeHandle<ServiceRequest>(true);
      this.m_PostVanRequestType = system.GetComponentTypeHandle<PostVanRequest>(true);
      this.m_PostFacilityType = system.GetComponentTypeHandle<Game.Buildings.PostFacility>(true);
      this.m_PostVanType = system.GetComponentTypeHandle<Game.Vehicles.PostVan>(true);
      this.m_MailBoxType = system.GetComponentTypeHandle<Game.Routes.MailBox>(true);
      this.m_TransportStopType = system.GetComponentTypeHandle<Game.Routes.TransportStop>(true);
      this.m_PrefabRefType = system.GetComponentTypeHandle<PrefabRef>(true);
      this.m_PathElementType = system.GetBufferTypeHandle<PathElement>(true);
      this.m_ServiceDispatchType = system.GetBufferTypeHandle<ServiceDispatch>(true);
      this.m_ResourcesType = system.GetBufferTypeHandle<Resources>(true);
      this.m_TradeCostType = system.GetBufferTypeHandle<TradeCost>(true);
      this.m_InstalledUpgradeType = system.GetBufferTypeHandle<InstalledUpgrade>(true);
      this.m_PathInformationData = system.GetComponentLookup<PathInformation>(true);
      this.m_PostVanRequestData = system.GetComponentLookup<PostVanRequest>(true);
      this.m_PostFacilityData = system.GetComponentLookup<Game.Buildings.PostFacility>(true);
      this.m_PostVanData = system.GetComponentLookup<Game.Vehicles.PostVan>(true);
      this.m_CurrentDistrictData = system.GetComponentLookup<CurrentDistrict>(true);
      this.m_StorageLimitData = system.GetComponentLookup<StorageLimitData>(true);
      this.m_StorageCompanyData = system.GetComponentLookup<StorageCompanyData>(true);
      this.m_MailBoxData = system.GetComponentLookup<MailBoxData>(true);
      this.m_PathElements = system.GetBufferLookup<PathElement>(true);
      this.m_ServiceDistricts = system.GetBufferLookup<ServiceDistrict>(true);
    }

    public JobHandle SetupPostVans(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_PostFacilityType.Update((SystemBase) system);
      this.m_PostVanType.Update((SystemBase) system);
      this.m_PathOwnerType.Update((SystemBase) system);
      this.m_OwnerType.Update((SystemBase) system);
      this.m_PathElementType.Update((SystemBase) system);
      this.m_ServiceDispatchType.Update((SystemBase) system);
      this.m_PathInformationData.Update((SystemBase) system);
      this.m_PathElements.Update((SystemBase) system);
      this.m_ServiceDistricts.Update((SystemBase) system);
      return new PostServicePathfindSetup.SetupPostVansJob()
      {
        m_EntityType = this.m_EntityType,
        m_PostFacilityType = this.m_PostFacilityType,
        m_PostVanType = this.m_PostVanType,
        m_PathOwnerType = this.m_PathOwnerType,
        m_OwnerType = this.m_OwnerType,
        m_PathElementType = this.m_PathElementType,
        m_ServiceDispatchType = this.m_ServiceDispatchType,
        m_PathInformationData = this.m_PathInformationData,
        m_PathElements = this.m_PathElements,
        m_ServiceDistricts = this.m_ServiceDistricts,
        m_SetupData = setupData
      }.ScheduleParallel<PostServicePathfindSetup.SetupPostVansJob>(this.m_PostVanQuery, inputDeps);
    }

    public JobHandle SetupMailTransfer(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_PostFacilityType.Update((SystemBase) system);
      this.m_PrefabRefType.Update((SystemBase) system);
      this.m_OutsideConnectionType.Update((SystemBase) system);
      this.m_ResourcesType.Update((SystemBase) system);
      this.m_TradeCostType.Update((SystemBase) system);
      this.m_InstalledUpgradeType.Update((SystemBase) system);
      this.m_StorageCompanyData.Update((SystemBase) system);
      this.m_StorageLimitData.Update((SystemBase) system);
      this.m_ServiceDistricts.Update((SystemBase) system);
      return new PostServicePathfindSetup.SetupMailTransferJob()
      {
        m_EntityType = this.m_EntityType,
        m_PostFacilityType = this.m_PostFacilityType,
        m_PrefabRefType = this.m_PrefabRefType,
        m_OutsideConnectionType = this.m_OutsideConnectionType,
        m_ResourcesType = this.m_ResourcesType,
        m_TradeCostType = this.m_TradeCostType,
        m_InstalledUpgradeType = this.m_InstalledUpgradeType,
        m_StorageCompanyData = this.m_StorageCompanyData,
        m_StorageLimitData = this.m_StorageLimitData,
        m_ServiceDistricts = this.m_ServiceDistricts,
        m_SetupData = setupData
      }.ScheduleParallel<PostServicePathfindSetup.SetupMailTransferJob>(this.m_MailTransferQuery, inputDeps);
    }

    public JobHandle SetupMailBoxes(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_PrefabRefType.Update((SystemBase) system);
      this.m_MailBoxType.Update((SystemBase) system);
      this.m_TransportStopType.Update((SystemBase) system);
      this.m_MailBoxData.Update((SystemBase) system);
      return new PostServicePathfindSetup.SetupMailBoxesJob()
      {
        m_EntityType = this.m_EntityType,
        m_PrefabRefType = this.m_PrefabRefType,
        m_MailBoxType = this.m_MailBoxType,
        m_TransportStopType = this.m_TransportStopType,
        m_MailBoxData = this.m_MailBoxData,
        m_SetupData = setupData
      }.ScheduleParallel<PostServicePathfindSetup.SetupMailBoxesJob>(this.m_MailBoxQuery, inputDeps);
    }

    public JobHandle SetupPostVanRequest(
      PathfindSetupSystem system,
      PathfindSetupSystem.SetupData setupData,
      JobHandle inputDeps)
    {
      this.m_EntityType.Update((SystemBase) system);
      this.m_ServiceRequestType.Update((SystemBase) system);
      this.m_PostVanRequestType.Update((SystemBase) system);
      this.m_PostVanRequestData.Update((SystemBase) system);
      this.m_CurrentDistrictData.Update((SystemBase) system);
      this.m_PostFacilityData.Update((SystemBase) system);
      this.m_PostVanData.Update((SystemBase) system);
      this.m_ServiceDistricts.Update((SystemBase) system);
      return new PostServicePathfindSetup.PostVanRequestsJob()
      {
        m_EntityType = this.m_EntityType,
        m_ServiceRequestType = this.m_ServiceRequestType,
        m_PostVanRequestType = this.m_PostVanRequestType,
        m_PostVanRequestData = this.m_PostVanRequestData,
        m_CurrentDistrictData = this.m_CurrentDistrictData,
        m_PostFacilityData = this.m_PostFacilityData,
        m_PostVanData = this.m_PostVanData,
        m_ServiceDistricts = this.m_ServiceDistricts,
        m_SetupData = setupData
      }.ScheduleParallel<PostServicePathfindSetup.PostVanRequestsJob>(this.m_PostVanRequestQuery, inputDeps);
    }

    [BurstCompile]
    private struct SetupPostVansJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.PostFacility> m_PostFacilityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Vehicles.PostVan> m_PostVanType;
      [ReadOnly]
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public BufferTypeHandle<PathElement> m_PathElementType;
      [ReadOnly]
      public BufferTypeHandle<ServiceDispatch> m_ServiceDispatchType;
      [ReadOnly]
      public ComponentLookup<PathInformation> m_PathInformationData;
      [ReadOnly]
      public BufferLookup<PathElement> m_PathElements;
      [ReadOnly]
      public BufferLookup<ServiceDistrict> m_ServiceDistricts;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        NativeArray<Game.Buildings.PostFacility> nativeArray2 = chunk.GetNativeArray<Game.Buildings.PostFacility>(ref this.m_PostFacilityType);
        if (nativeArray2.Length != 0)
        {
          for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
          {
            Game.Buildings.PostFacility postFacility = nativeArray2[index1];
            for (int index2 = 0; index2 < this.m_SetupData.Length; ++index2)
            {
              Entity entity1;
              PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
              // ISSUE: reference to a compiler-generated method
              this.m_SetupData.GetItem(index2, out entity1, out targetSeeker);
              if ((targetSeeker.m_SetupQueueTarget.m_Flags & SetupTargetFlags.Export) != SetupTargetFlags.None && (postFacility.m_Flags & PostFacilityFlags.CanDeliverMailWithVan) != (PostFacilityFlags) 0 || (targetSeeker.m_SetupQueueTarget.m_Flags & SetupTargetFlags.Import) != SetupTargetFlags.None && (postFacility.m_Flags & PostFacilityFlags.CanCollectMailWithVan) != (PostFacilityFlags) 0)
              {
                Entity entity2 = nativeArray1[index1];
                if (AreaUtils.CheckServiceDistrict(entity1, entity2, this.m_ServiceDistricts))
                {
                  float cost = targetSeeker.m_PathfindParameters.m_Weights.time * 10f;
                  targetSeeker.FindTargets(entity2, cost);
                }
              }
            }
          }
        }
        else
        {
          NativeArray<Game.Vehicles.PostVan> nativeArray3 = chunk.GetNativeArray<Game.Vehicles.PostVan>(ref this.m_PostVanType);
          if (nativeArray3.Length == 0)
            return;
          NativeArray<PathOwner> nativeArray4 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
          NativeArray<Owner> nativeArray5 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          BufferAccessor<PathElement> bufferAccessor1 = chunk.GetBufferAccessor<PathElement>(ref this.m_PathElementType);
          BufferAccessor<ServiceDispatch> bufferAccessor2 = chunk.GetBufferAccessor<ServiceDispatch>(ref this.m_ServiceDispatchType);
          for (int index3 = 0; index3 < nativeArray3.Length; ++index3)
          {
            Entity entity3 = nativeArray1[index3];
            Game.Vehicles.PostVan postVan = nativeArray3[index3];
            if ((postVan.m_State & PostVanFlags.Disabled) == (PostVanFlags) 0)
            {
              for (int index4 = 0; index4 < this.m_SetupData.Length; ++index4)
              {
                Entity entity4;
                PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
                // ISSUE: reference to a compiler-generated method
                this.m_SetupData.GetItem(index4, out entity4, out targetSeeker);
                if (((targetSeeker.m_SetupQueueTarget.m_Flags & SetupTargetFlags.Export) == SetupTargetFlags.None || (postVan.m_State & PostVanFlags.EstimatedEmpty) == (PostVanFlags) 0) && ((targetSeeker.m_SetupQueueTarget.m_Flags & SetupTargetFlags.Import) == SetupTargetFlags.None || (postVan.m_State & PostVanFlags.EstimatedFull) == (PostVanFlags) 0) && (nativeArray5.Length == 0 || AreaUtils.CheckServiceDistrict(entity4, nativeArray5[index3].m_Owner, this.m_ServiceDistricts)))
                {
                  if ((postVan.m_State & PostVanFlags.Returning) != (PostVanFlags) 0 || nativeArray4.Length == 0)
                  {
                    targetSeeker.FindTargets(entity3, 0.0f);
                  }
                  else
                  {
                    PathOwner pathOwner = nativeArray4[index3];
                    DynamicBuffer<ServiceDispatch> dynamicBuffer1 = bufferAccessor2[index3];
                    int num = math.min(postVan.m_RequestCount, dynamicBuffer1.Length);
                    PathElement pathElement = new PathElement();
                    float cost = 0.0f;
                    bool flag = false;
                    if (num >= 1)
                    {
                      DynamicBuffer<PathElement> dynamicBuffer2 = bufferAccessor1[index3];
                      if (pathOwner.m_ElementIndex < dynamicBuffer2.Length)
                      {
                        cost += (float) (dynamicBuffer2.Length - pathOwner.m_ElementIndex) * postVan.m_PathElementTime * targetSeeker.m_PathfindParameters.m_Weights.time;
                        pathElement = dynamicBuffer2[dynamicBuffer2.Length - 1];
                        flag = true;
                      }
                    }
                    for (int index5 = 1; index5 < num; ++index5)
                    {
                      Entity request = dynamicBuffer1[index5].m_Request;
                      PathInformation componentData;
                      if (this.m_PathInformationData.TryGetComponent(request, out componentData))
                        cost += componentData.m_Duration * targetSeeker.m_PathfindParameters.m_Weights.time;
                      DynamicBuffer<PathElement> bufferData;
                      if (this.m_PathElements.TryGetBuffer(request, out bufferData) && bufferData.Length != 0)
                      {
                        pathElement = bufferData[bufferData.Length - 1];
                        flag = true;
                      }
                    }
                    if (flag)
                      targetSeeker.m_Buffer.Enqueue(new PathTarget(entity3, pathElement.m_Target, pathElement.m_TargetDelta.y, cost));
                    else
                      targetSeeker.FindTargets(entity3, entity3, cost, EdgeFlags.DefaultMask, true, num >= 1);
                  }
                }
              }
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
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    [BurstCompile]
    private struct SetupMailTransferJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.PostFacility> m_PostFacilityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.OutsideConnection> m_OutsideConnectionType;
      [ReadOnly]
      public BufferTypeHandle<Resources> m_ResourcesType;
      [ReadOnly]
      public BufferTypeHandle<TradeCost> m_TradeCostType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      [ReadOnly]
      public ComponentLookup<StorageLimitData> m_StorageLimitData;
      [ReadOnly]
      public ComponentLookup<StorageCompanyData> m_StorageCompanyData;
      [ReadOnly]
      public BufferLookup<ServiceDistrict> m_ServiceDistricts;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        bool flag = chunk.Has<Game.Objects.OutsideConnection>(ref this.m_OutsideConnectionType);
        Entity entity1;
        if (!flag)
        {
          NativeArray<Game.Buildings.PostFacility> nativeArray2 = chunk.GetNativeArray<Game.Buildings.PostFacility>(ref this.m_PostFacilityType);
          if (nativeArray2.Length != 0)
          {
            for (int index1 = 0; index1 < nativeArray2.Length; ++index1)
            {
              Game.Buildings.PostFacility postFacility = nativeArray2[index1];
              for (int index2 = 0; index2 < this.m_SetupData.Length; ++index2)
              {
                PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
                // ISSUE: reference to a compiler-generated method
                this.m_SetupData.GetItem(index2, out entity1, out targetSeeker);
                Resource resource = targetSeeker.m_SetupQueueTarget.m_Resource;
                if ((resource & (Resource.UnsortedMail | Resource.LocalMail)) != Resource.NoResource && ((targetSeeker.m_SetupQueueTarget.m_Flags & SetupTargetFlags.RequireTransport) == SetupTargetFlags.None || (postFacility.m_Flags & PostFacilityFlags.HasAvailableTrucks) != (PostFacilityFlags) 0))
                {
                  if ((targetSeeker.m_SetupQueueTarget.m_Flags & SetupTargetFlags.Import) != SetupTargetFlags.None)
                  {
                    if ((resource & Resource.UnsortedMail) != Resource.NoResource && (postFacility.m_Flags & PostFacilityFlags.AcceptsUnsortedMail) != (PostFacilityFlags) 0 || (resource & Resource.LocalMail) != Resource.NoResource && (postFacility.m_Flags & PostFacilityFlags.AcceptsLocalMail) != (PostFacilityFlags) 0)
                    {
                      Entity entity2 = nativeArray1[index1];
                      targetSeeker.FindTargets(entity2, 0.0f);
                    }
                  }
                  else if ((targetSeeker.m_SetupQueueTarget.m_Flags & SetupTargetFlags.Export) != SetupTargetFlags.None && ((resource & Resource.UnsortedMail) != Resource.NoResource && (postFacility.m_Flags & PostFacilityFlags.DeliversUnsortedMail) != (PostFacilityFlags) 0 || (resource & Resource.LocalMail) != Resource.NoResource && (postFacility.m_Flags & PostFacilityFlags.DeliversLocalMail) != (PostFacilityFlags) 0))
                  {
                    Entity entity3 = nativeArray1[index1];
                    targetSeeker.FindTargets(entity3, 0.0f);
                  }
                }
              }
            }
            return;
          }
        }
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        BufferAccessor<Resources> bufferAccessor1 = chunk.GetBufferAccessor<Resources>(ref this.m_ResourcesType);
        BufferAccessor<TradeCost> bufferAccessor2 = chunk.GetBufferAccessor<TradeCost>(ref this.m_TradeCostType);
        BufferAccessor<InstalledUpgrade> bufferAccessor3 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        for (int index3 = 0; index3 < nativeArray1.Length; ++index3)
        {
          Entity entity4 = nativeArray1[index3];
          Entity prefab = nativeArray3[index3].m_Prefab;
          StorageCompanyData storageCompanyData = this.m_StorageCompanyData[prefab];
          for (int index4 = 0; index4 < this.m_SetupData.Length; ++index4)
          {
            PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
            // ISSUE: reference to a compiler-generated method
            this.m_SetupData.GetItem(index4, out entity1, out targetSeeker);
            Resource resource = targetSeeker.m_SetupQueueTarget.m_Resource;
            int num1 = targetSeeker.m_SetupQueueTarget.m_Value;
            switch (resource)
            {
              case Resource.UnsortedMail:
              case Resource.OutgoingMail:
                if ((targetSeeker.m_SetupQueueTarget.m_Flags & SetupTargetFlags.Export) > SetupTargetFlags.None & flag)
                  break;
                goto default;
              case Resource.LocalMail:
                if (!((targetSeeker.m_SetupQueueTarget.m_Flags & SetupTargetFlags.Import) > SetupTargetFlags.None & flag))
                  goto default;
                else
                  break;
              default:
                if ((resource & storageCompanyData.m_StoredResources) != Resource.NoResource)
                {
                  float resources = (float) EconomyUtils.GetResources(resource, bufferAccessor1[index3]);
                  if ((targetSeeker.m_SetupQueueTarget.m_Flags & SetupTargetFlags.Export) != SetupTargetFlags.None)
                  {
                    if ((double) resources >= (double) num1)
                    {
                      TradeCost tradeCost = EconomyUtils.GetTradeCost(resource, bufferAccessor2[index3]);
                      float num2 = resources - tradeCost.m_BuyCost * (float) num1;
                      if ((double) num2 >= (double) num1)
                      {
                        targetSeeker.FindTargets(entity4, math.max(0.0f, 500f - num2));
                        break;
                      }
                      break;
                    }
                    break;
                  }
                  if ((targetSeeker.m_SetupQueueTarget.m_Flags & SetupTargetFlags.Import) != SetupTargetFlags.None)
                  {
                    int num3 = num1;
                    if (this.m_StorageLimitData.HasComponent(prefab))
                    {
                      StorageLimitData data = this.m_StorageLimitData[prefab];
                      if (bufferAccessor3.Length != 0)
                        UpgradeUtils.CombineStats<StorageLimitData>(ref data, bufferAccessor3[index3], ref targetSeeker.m_PrefabRef, ref this.m_StorageLimitData);
                      num3 = data.m_Limit - EconomyUtils.GetResources(resource, bufferAccessor1[index3]);
                    }
                    if (num3 >= num1)
                    {
                      TradeCost tradeCost = EconomyUtils.GetTradeCost(resource, bufferAccessor2[index3]);
                      targetSeeker.FindTargets(entity4, math.max(0.0f, (float) (-0.10000000149011612 * (double) num3 + (double) tradeCost.m_SellCost * (double) num1)));
                      break;
                    }
                    break;
                  }
                  break;
                }
                break;
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
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    [BurstCompile]
    private struct SetupMailBoxesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Routes.MailBox> m_MailBoxType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Routes.TransportStop> m_TransportStopType;
      [ReadOnly]
      public ComponentLookup<MailBoxData> m_MailBoxData;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        NativeArray<Game.Routes.MailBox> nativeArray3 = chunk.GetNativeArray<Game.Routes.MailBox>(ref this.m_MailBoxType);
        NativeArray<Game.Routes.TransportStop> nativeArray4 = chunk.GetNativeArray<Game.Routes.TransportStop>(ref this.m_TransportStopType);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          Entity entity = nativeArray1[index1];
          Entity prefab = nativeArray2[index1].m_Prefab;
          Game.Routes.MailBox mailBox = nativeArray3[index1];
          MailBoxData mailBoxData = this.m_MailBoxData[prefab];
          if (mailBox.m_MailAmount < mailBoxData.m_MailCapacity)
          {
            for (int index2 = 0; index2 < this.m_SetupData.Length; ++index2)
            {
              PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
              // ISSUE: reference to a compiler-generated method
              this.m_SetupData.GetItem(index2, out Entity _, out targetSeeker);
              float cost = (float) mailBox.m_MailAmount * 100f / (float) mailBoxData.m_MailCapacity;
              if (nativeArray4.Length != 0)
                cost += (float) (10.0 * (1.0 - (double) nativeArray4[index1].m_ComfortFactor)) * targetSeeker.m_PathfindParameters.m_Weights.m_Value.w;
              targetSeeker.FindTargets(entity, cost);
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
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    [BurstCompile]
    private struct PostVanRequestsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<ServiceRequest> m_ServiceRequestType;
      [ReadOnly]
      public ComponentTypeHandle<PostVanRequest> m_PostVanRequestType;
      [ReadOnly]
      public ComponentLookup<PostVanRequest> m_PostVanRequestData;
      [ReadOnly]
      public ComponentLookup<CurrentDistrict> m_CurrentDistrictData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.PostFacility> m_PostFacilityData;
      [ReadOnly]
      public ComponentLookup<Game.Vehicles.PostVan> m_PostVanData;
      [ReadOnly]
      public BufferLookup<ServiceDistrict> m_ServiceDistricts;
      public PathfindSetupSystem.SetupData m_SetupData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        NativeArray<ServiceRequest> nativeArray2 = chunk.GetNativeArray<ServiceRequest>(ref this.m_ServiceRequestType);
        NativeArray<PostVanRequest> nativeArray3 = chunk.GetNativeArray<PostVanRequest>(ref this.m_PostVanRequestType);
        for (int index1 = 0; index1 < this.m_SetupData.Length; ++index1)
        {
          Entity owner;
          PathfindTargetSeeker<PathfindSetupBuffer> targetSeeker;
          // ISSUE: reference to a compiler-generated method
          this.m_SetupData.GetItem(index1, out Entity _, out owner, out targetSeeker);
          PostVanRequest componentData1;
          if (this.m_PostVanRequestData.TryGetComponent(owner, out componentData1))
          {
            Random random = targetSeeker.m_RandomSeed.GetRandom(unfilteredChunkIndex);
            Entity service = Entity.Null;
            Game.Vehicles.PostVan componentData2;
            bool flag1;
            bool flag2;
            if (this.m_PostVanData.TryGetComponent(componentData1.m_Target, out componentData2))
            {
              flag1 = (componentData2.m_State & PostVanFlags.EstimatedFull) == (PostVanFlags) 0;
              flag2 = (componentData2.m_State & PostVanFlags.EstimatedEmpty) == (PostVanFlags) 0;
              Owner componentData3;
              if (targetSeeker.m_Owner.TryGetComponent(componentData1.m_Target, out componentData3))
                service = componentData3.m_Owner;
            }
            else
            {
              Game.Buildings.PostFacility componentData4;
              if (this.m_PostFacilityData.TryGetComponent(componentData1.m_Target, out componentData4))
              {
                flag1 = (componentData4.m_Flags & PostFacilityFlags.CanCollectMailWithVan) != 0;
                flag2 = (componentData4.m_Flags & PostFacilityFlags.CanDeliverMailWithVan) != 0;
                service = componentData1.m_Target;
              }
              else
                continue;
            }
            for (int index2 = 0; index2 < nativeArray3.Length; ++index2)
            {
              if ((nativeArray2[index2].m_Flags & ServiceRequestFlags.Reversed) == (ServiceRequestFlags) 0)
              {
                PostVanRequest postVanRequest = nativeArray3[index2];
                if (((postVanRequest.m_Flags & PostVanRequestFlags.Collect) == (PostVanRequestFlags) 0 || flag1) && ((postVanRequest.m_Flags & PostVanRequestFlags.Deliver) == (PostVanRequestFlags) 0 || flag2))
                {
                  Entity district = Entity.Null;
                  CurrentDistrict componentData5;
                  if (this.m_CurrentDistrictData.TryGetComponent(postVanRequest.m_Target, out componentData5))
                    district = componentData5.m_District;
                  if (AreaUtils.CheckServiceDistrict(district, service, this.m_ServiceDistricts))
                  {
                    float cost = random.NextFloat(30f);
                    targetSeeker.FindTargets(nativeArray1[index2], postVanRequest.m_Target, cost, EdgeFlags.DefaultMask, true, false);
                  }
                }
              }
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
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }
  }
}
