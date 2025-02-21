// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ResourceProducerSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Common;
using Game.Economy;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class ResourceProducerSystem : GameSystemBase
  {
    public static readonly int kUpdatesPerDay = 16;
    private SimulationSystem m_SimulationSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_ResourceProducerQuery;
    private ResourceProducerSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / (ResourceProducerSystem.kUpdatesPerDay * 16);
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
      this.m_ResourceProducerQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.ResourceProducer>(), ComponentType.ReadOnly<Resources>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ResourceProducerQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceProducerQuery.ResetFilter();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceProducerQuery.SetSharedComponentFilter<UpdateFrame>(new UpdateFrame(SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, ResourceProducerSystem.kUpdatesPerDay, 16)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ResourceProductionData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Economy_Resources_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle producerJob = new ResourceProducerSystem.ResourceProducerJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_ResourcesType = this.__TypeHandle.__Game_Economy_Resources_RO_BufferTypeHandle,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ResourceProductionData = this.__TypeHandle.__Game_Prefabs_ResourceProductionData_RO_BufferLookup,
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<ResourceProducerSystem.ResourceProducerJob>(this.m_ResourceProducerQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(producerJob);
      this.Dependency = producerJob;
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
    public ResourceProducerSystem()
    {
    }

    [BurstCompile]
    private struct ResourceProducerJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      [ReadOnly]
      public BufferTypeHandle<Resources> m_ResourcesType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public BufferLookup<ResourceProductionData> m_ResourceProductionData;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      [NativeDisableContainerSafetyRestriction]
      private NativeList<ResourceProductionData> m_ResourceProductionBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ResourceProductionBuffer.IsCreated)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ResourceProductionBuffer = new NativeList<ResourceProductionData>((AllocatorManager.AllocatorHandle) Allocator.Temp);
        }
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor1 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Resources> bufferAccessor2 = chunk.GetBufferAccessor<Resources>(ref this.m_ResourcesType);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          Entity entity1 = nativeArray1[index1];
          PrefabRef prefabRef = nativeArray2[index1];
          DynamicBuffer<Resources> resources1 = bufferAccessor2[index1];
          // ISSUE: reference to a compiler-generated field
          if (this.m_ResourceProductionData.HasBuffer(prefabRef.m_Prefab))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            ResourceProductionData.Combine(this.m_ResourceProductionBuffer, this.m_ResourceProductionData[prefabRef.m_Prefab]);
          }
          if (bufferAccessor1.Length != 0)
          {
            DynamicBuffer<InstalledUpgrade> dynamicBuffer = bufferAccessor1[index1];
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              InstalledUpgrade installedUpgrade = dynamicBuffer[index2];
              DynamicBuffer<ResourceProductionData> bufferData;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (!BuildingUtils.CheckOption(installedUpgrade, BuildingOption.Inactive) && this.m_ResourceProductionData.TryGetBuffer(this.m_PrefabRefData[installedUpgrade.m_Upgrade].m_Prefab, out bufferData))
              {
                // ISSUE: reference to a compiler-generated field
                ResourceProductionData.Combine(this.m_ResourceProductionBuffer, bufferData);
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          for (int index3 = 0; index3 < this.m_ResourceProductionBuffer.Length; ++index3)
          {
            // ISSUE: reference to a compiler-generated field
            ResourceProductionData resourceProductionData = this.m_ResourceProductionBuffer[index3];
            int resources2 = EconomyUtils.GetResources(resourceProductionData.m_Type, resources1);
            if (resources2 >= math.min(2000, resourceProductionData.m_StorageCapacity))
            {
              // ISSUE: reference to a compiler-generated field
              Entity entity2 = this.m_CommandBuffer.CreateEntity(unfilteredChunkIndex);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<GoodsDeliveryRequest>(unfilteredChunkIndex, entity2, new GoodsDeliveryRequest()
              {
                m_Amount = resources2,
                m_Flags = GoodsDeliveryFlags.CommercialAllowed | GoodsDeliveryFlags.IndustrialAllowed | GoodsDeliveryFlags.ImportAllowed | GoodsDeliveryFlags.ResourceExportTarget,
                m_Resource = resourceProductionData.m_Type,
                m_Target = entity1
              });
            }
          }
          // ISSUE: reference to a compiler-generated field
          this.m_ResourceProductionBuffer.Clear();
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
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Resources> __Game_Economy_Resources_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<ResourceProductionData> __Game_Prefabs_ResourceProductionData_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Economy_Resources_RO_BufferTypeHandle = state.GetBufferTypeHandle<Resources>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ResourceProductionData_RO_BufferLookup = state.GetBufferLookup<ResourceProductionData>(true);
      }
    }
  }
}
