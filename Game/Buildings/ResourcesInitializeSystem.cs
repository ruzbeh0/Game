// Decompiled with JetBrains decompiler
// Type: Game.Buildings.ResourcesInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using Game.Economy;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Buildings
{
  [CompilerGenerated]
  public class ResourcesInitializeSystem : GameSystemBase
  {
    private EntityQuery m_Additions;
    private ResourceSystem m_ResourceSystem;
    private ResourcesInitializeSystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_Additions = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadWrite<Resources>(),
          ComponentType.ReadWrite<PrefabRef>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Updated>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<ServiceUpgrade>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_Additions);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Created_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ResourceConsumer_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ServiceUpkeepData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InitialResourceData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ResourceConsumer_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ResourcesInitializeSystem.InitializeCityServiceJob jobData = new ResourcesInitializeSystem.InitializeCityServiceJob()
      {
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
        m_ResourcesType = this.__TypeHandle.__Game_Economy_Resources_RW_BufferTypeHandle,
        m_ResourceConsumerType = this.__TypeHandle.__Game_Buildings_ResourceConsumer_RW_ComponentTypeHandle,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ResourceDatas = this.__TypeHandle.__Game_Prefabs_ResourceData_RO_ComponentLookup,
        m_InitialResourceDatas = this.__TypeHandle.__Game_Prefabs_InitialResourceData_RO_BufferLookup,
        m_ServiceUpkeepDatas = this.__TypeHandle.__Game_Prefabs_ServiceUpkeepData_RO_BufferLookup,
        m_ResourceConsumers = this.__TypeHandle.__Game_Buildings_ResourceConsumer_RW_ComponentLookup,
        m_CreatedDatas = this.__TypeHandle.__Game_Common_Created_RO_ComponentLookup,
        m_ResourcePrefabs = this.m_ResourceSystem.GetPrefabs()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<ResourcesInitializeSystem.InitializeCityServiceJob>(this.m_Additions, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ResourceSystem.AddPrefabsReader(this.Dependency);
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
    public ResourcesInitializeSystem()
    {
    }

    private struct InitializeCityServiceJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      [ReadOnly]
      public ComponentTypeHandle<Created> m_CreatedType;
      public BufferTypeHandle<Resources> m_ResourcesType;
      public ComponentTypeHandle<ResourceConsumer> m_ResourceConsumerType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<ResourceData> m_ResourceDatas;
      [ReadOnly]
      public BufferLookup<InitialResourceData> m_InitialResourceDatas;
      [ReadOnly]
      public BufferLookup<ServiceUpkeepData> m_ServiceUpkeepDatas;
      [ReadOnly]
      public ComponentLookup<Created> m_CreatedDatas;
      [NativeDisableContainerSafetyRestriction]
      public ComponentLookup<ResourceConsumer> m_ResourceConsumers;
      [ReadOnly]
      public ResourcePrefabs m_ResourcePrefabs;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeArray<int> nativeArray1 = new NativeArray<int>(EconomyUtils.ResourceCount, Allocator.Temp);
        NativeList<ServiceUpkeepData> nativeList = new NativeList<ServiceUpkeepData>(4, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor1 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        // ISSUE: reference to a compiler-generated field
        bool flag = chunk.Has<Created>(ref this.m_CreatedType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Resources> bufferAccessor2 = chunk.GetBufferAccessor<Resources>(ref this.m_ResourcesType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ResourceConsumer> nativeArray3 = chunk.GetNativeArray<ResourceConsumer>(ref this.m_ResourceConsumerType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          PrefabRef prefab = nativeArray2[index];
          DynamicBuffer<Resources> resources = bufferAccessor2[index];
          if (flag)
          {
            // ISSUE: reference to a compiler-generated method
            this.ProcessAddition((Entity) prefab, resources);
          }
          if (bufferAccessor1.Length != 0)
          {
            foreach (InstalledUpgrade installedUpgrade in bufferAccessor1[index])
            {
              PrefabRef componentData;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_CreatedDatas.HasComponent(installedUpgrade.m_Upgrade) && this.m_Prefabs.TryGetComponent(installedUpgrade.m_Upgrade, out componentData))
              {
                // ISSUE: reference to a compiler-generated method
                this.ProcessAddition((Entity) componentData, resources);
              }
            }
          }
          nativeArray1.Fill<int>(0);
          nativeList.Clear();
          DynamicBuffer<ServiceUpkeepData> bufferData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ServiceUpkeepDatas.TryGetBuffer((Entity) prefab, out bufferData1))
          {
            // ISSUE: reference to a compiler-generated method
            this.AddStorageTargets(nativeArray1, bufferData1);
            nativeList.AddRange(bufferData1.AsNativeArray());
          }
          if (bufferAccessor1.Length != 0)
          {
            foreach (InstalledUpgrade installedUpgrade in bufferAccessor1[index])
            {
              PrefabRef componentData;
              DynamicBuffer<ServiceUpkeepData> bufferData2;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!BuildingUtils.CheckOption(installedUpgrade, BuildingOption.Inactive) && this.m_Prefabs.TryGetComponent((Entity) installedUpgrade, out componentData) && this.m_ServiceUpkeepDatas.TryGetBuffer((Entity) componentData, out bufferData2))
              {
                // ISSUE: reference to a compiler-generated method
                this.AddStorageTargets(nativeArray1, bufferData2);
                // ISSUE: reference to a compiler-generated field
                if (!this.m_ResourceConsumers.HasComponent((Entity) installedUpgrade))
                  UpgradeUtils.CombineStats<ServiceUpkeepData>(nativeList, bufferData2);
              }
            }
          }
          if (nativeArray3.Length != 0)
          {
            // ISSUE: reference to a compiler-generated method
            nativeArray3.ElementAt<ResourceConsumer>(index).m_ResourceAvailability = CityServiceUpkeepSystem.GetResourceAvailability(nativeList, resources, nativeArray1);
          }
          if (bufferAccessor1.Length != 0)
          {
            foreach (InstalledUpgrade installedUpgrade in bufferAccessor1[index])
            {
              ResourceConsumer componentData1;
              // ISSUE: reference to a compiler-generated field
              if (!BuildingUtils.CheckOption(installedUpgrade, BuildingOption.Inactive) && this.m_ResourceConsumers.TryGetComponent((Entity) installedUpgrade, out componentData1))
              {
                PrefabRef componentData2;
                DynamicBuffer<ServiceUpkeepData> bufferData3;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_Prefabs.TryGetComponent((Entity) installedUpgrade, out componentData2) && this.m_ServiceUpkeepDatas.TryGetBuffer((Entity) componentData2, out bufferData3))
                {
                  nativeList.CopyFrom(bufferData3.AsNativeArray());
                  // ISSUE: reference to a compiler-generated method
                  componentData1.m_ResourceAvailability = CityServiceUpkeepSystem.GetResourceAvailability(nativeList, resources, nativeArray1);
                }
                else
                  componentData1.m_ResourceAvailability = byte.MaxValue;
                // ISSUE: reference to a compiler-generated field
                this.m_ResourceConsumers[(Entity) installedUpgrade] = componentData1;
              }
            }
          }
        }
      }

      private void ProcessAddition(Entity prefab, DynamicBuffer<Resources> resources)
      {
        DynamicBuffer<InitialResourceData> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_InitialResourceDatas.TryGetBuffer(prefab, out bufferData))
          return;
        foreach (InitialResourceData initialResourceData in bufferData)
          EconomyUtils.AddResources(initialResourceData.m_Value.m_Resource, initialResourceData.m_Value.m_Amount, resources);
      }

      private void AddStorageTargets(
        NativeArray<int> storageTargets,
        DynamicBuffer<ServiceUpkeepData> upkeeps)
      {
        foreach (ServiceUpkeepData upkeep in upkeeps)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (EconomyUtils.IsMaterial(upkeep.m_Upkeep.m_Resource, this.m_ResourcePrefabs, ref this.m_ResourceDatas))
            storageTargets[EconomyUtils.GetResourceIndex(upkeep.m_Upkeep.m_Resource)] += upkeep.m_Upkeep.m_Amount;
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
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Created> __Game_Common_Created_RO_ComponentTypeHandle;
      public BufferTypeHandle<Resources> __Game_Economy_Resources_RW_BufferTypeHandle;
      public ComponentTypeHandle<ResourceConsumer> __Game_Buildings_ResourceConsumer_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ResourceData> __Game_Prefabs_ResourceData_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<InitialResourceData> __Game_Prefabs_InitialResourceData_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ServiceUpkeepData> __Game_Prefabs_ServiceUpkeepData_RO_BufferLookup;
      public ComponentLookup<ResourceConsumer> __Game_Buildings_ResourceConsumer_RW_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Created> __Game_Common_Created_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RW_BufferTypeHandle = state.GetBufferTypeHandle<Resources>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ResourceConsumer_RW_ComponentTypeHandle = state.GetComponentTypeHandle<ResourceConsumer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceData_RO_ComponentLookup = state.GetComponentLookup<ResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InitialResourceData_RO_BufferLookup = state.GetBufferLookup<InitialResourceData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceUpkeepData_RO_BufferLookup = state.GetBufferLookup<ServiceUpkeepData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ResourceConsumer_RW_ComponentLookup = state.GetComponentLookup<ResourceConsumer>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentLookup = state.GetComponentLookup<Created>(true);
      }
    }
  }
}
