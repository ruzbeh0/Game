// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CarStorageTransferRequestSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Economy;
using Game.Prefabs;
using Game.Tools;
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
  public class CarStorageTransferRequestSystem : GameSystemBase
  {
    private EntityQuery m_TransferGroup;
    private VehicleCapacitySystem m_VehicleCapacitySystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private CarStorageTransferRequestSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleCapacitySystem = this.World.GetOrCreateSystemManaged<VehicleCapacitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_TransferGroup = this.GetEntityQuery(ComponentType.ReadOnly<StorageTransferRequest>(), ComponentType.ReadOnly<Resources>(), ComponentType.ReadWrite<TripNeeded>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TransferGroup);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_TripNeeded_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_StorageTransferRequest_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      CarStorageTransferRequestSystem.TransferJob jobData = new CarStorageTransferRequestSystem.TransferJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_RequestType = this.__TypeHandle.__Game_Companies_StorageTransferRequest_RW_BufferTypeHandle,
        m_ResourceType = this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle,
        m_TripType = this.__TypeHandle.__Game_Citizens_TripNeeded_RW_BufferTypeHandle,
        m_Properties = this.__TypeHandle.__Game_Buildings_PropertyRenter_RO_ComponentLookup,
        m_OutsideConnections = this.__TypeHandle.__Game_Objects_OutsideConnection_RO_ComponentLookup,
        m_DeliveryTruckSelectData = this.m_VehicleCapacitySystem.GetDeliveryTruckSelectData()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<CarStorageTransferRequestSystem.TransferJob>(this.m_TransferGroup, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
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
    public CarStorageTransferRequestSystem()
    {
    }

    [BurstCompile]
    private struct TransferJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public BufferTypeHandle<StorageTransferRequest> m_RequestType;
      public BufferTypeHandle<Resources> m_ResourceType;
      public BufferTypeHandle<TripNeeded> m_TripType;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> m_Properties;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> m_OutsideConnections;
      [ReadOnly]
      public DeliveryTruckSelectData m_DeliveryTruckSelectData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<StorageTransferRequest> bufferAccessor1 = chunk.GetBufferAccessor<StorageTransferRequest>(ref this.m_RequestType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Resources> bufferAccessor2 = chunk.GetBufferAccessor<Resources>(ref this.m_ResourceType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<TripNeeded> bufferAccessor3 = chunk.GetBufferAccessor<TripNeeded>(ref this.m_TripType);
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          Entity entity = nativeArray[index1];
          DynamicBuffer<StorageTransferRequest> dynamicBuffer1 = bufferAccessor1[index1];
          DynamicBuffer<Resources> resources1 = bufferAccessor2[index1];
          DynamicBuffer<TripNeeded> dynamicBuffer2 = bufferAccessor3[index1];
          for (int index2 = 0; index2 < dynamicBuffer1.Length; ++index2)
          {
            StorageTransferRequest storageTransferRequest = dynamicBuffer1[index2];
            int resources2 = EconomyUtils.GetResources(storageTransferRequest.m_Resource, resources1);
            if ((storageTransferRequest.m_Flags & StorageTransferFlags.Incoming) == (StorageTransferFlags) 0 && (storageTransferRequest.m_Flags & StorageTransferFlags.Car) != (StorageTransferFlags) 0 && resources2 >= storageTransferRequest.m_Amount)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_Properties.HasComponent(storageTransferRequest.m_Target) || this.m_OutsideConnections.HasComponent(storageTransferRequest.m_Target))
              {
                if (storageTransferRequest.m_Amount > 0 && resources2 > 0)
                {
                  int max;
                  // ISSUE: reference to a compiler-generated field
                  this.m_DeliveryTruckSelectData.GetCapacityRange(storageTransferRequest.m_Resource, out int _, out max);
                  TripNeeded elem = new TripNeeded();
                  elem.m_TargetAgent = storageTransferRequest.m_Target;
                  elem.m_Purpose = Game.Citizens.Purpose.StorageTransfer;
                  elem.m_Resource = storageTransferRequest.m_Resource;
                  elem.m_Data = math.min(math.min(max, storageTransferRequest.m_Amount), resources2);
                  dynamicBuffer2.Add(elem);
                  EconomyUtils.AddResources(storageTransferRequest.m_Resource, -elem.m_Data, resources1);
                  storageTransferRequest.m_Amount -= elem.m_Data;
                }
                if (storageTransferRequest.m_Amount <= 0)
                {
                  dynamicBuffer1.RemoveAt(index2);
                  break;
                }
                dynamicBuffer1[index2] = storageTransferRequest;
                break;
              }
              dynamicBuffer1.RemoveAt(index2);
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
        // ISSUE: reference to a compiler-generated method
        this.Execute(in chunk, unfilteredChunkIndex, useEnabledMask, in chunkEnabledMask);
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      public BufferTypeHandle<StorageTransferRequest> __Game_Companies_StorageTransferRequest_RW_BufferTypeHandle;
      public BufferTypeHandle<Resources> __Game_Economy_Resources_RW_BufferTypeHandle;
      public BufferTypeHandle<TripNeeded> __Game_Citizens_TripNeeded_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<PropertyRenter> __Game_Buildings_PropertyRenter_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.OutsideConnection> __Game_Objects_OutsideConnection_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_StorageTransferRequest_RW_BufferTypeHandle = state.GetBufferTypeHandle<StorageTransferRequest>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferTypeHandle = state.GetBufferTypeHandle<Resources>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_TripNeeded_RW_BufferTypeHandle = state.GetBufferTypeHandle<TripNeeded>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_PropertyRenter_RO_ComponentLookup = state.GetComponentLookup<PropertyRenter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_OutsideConnection_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.OutsideConnection>(true);
      }
    }
  }
}
