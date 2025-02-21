// Decompiled with JetBrains decompiler
// Type: Game.Simulation.XPBuiltSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class XPBuiltSystem : GameSystemBase
  {
    private EntityQuery m_BuiltGroup;
    private EntityQuery m_ElectricityGroup;
    private XPSystem m_XPSystem;
    private ToolSystem m_ToolSystem;
    private CitySystem m_CitySystem;
    private ModificationEndBarrier m_ModificationEndBarrier;
    private static readonly int kElectricityGridXPBonus = 25;
    private XPBuiltSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_XPSystem = this.World.GetOrCreateSystemManaged<XPSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationEndBarrier = this.World.GetOrCreateSystemManaged<ModificationEndBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_BuiltGroup = this.GetEntityQuery(ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Created>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_ElectricityGroup = this.GetEntityQuery(ComponentType.ReadOnly<ElectricityConsumer>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.RequireAnyForUpdate(this.m_BuiltGroup, this.m_ElectricityGroup);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_ToolSystem.actionMode.IsGame())
        return;
      JobHandle deps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeQueue<XPGain> queue = this.m_XPSystem.GetQueue(out deps);
      // ISSUE: reference to a compiler-generated field
      if (!this.m_BuiltGroup.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_ServiceUpgradeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PlacedSignatureBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_SignatureBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: variable of a compiler-generated type
        XPBuiltSystem.XPBuiltJob jobData = new XPBuiltSystem.XPBuiltJob()
        {
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RW_ComponentTypeHandle,
          m_PlaceableObjectDatas = this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup,
          m_SignatureBuildingDatas = this.__TypeHandle.__Game_Prefabs_SignatureBuildingData_RO_ComponentLookup,
          m_PlacedSignatureBuildingDatas = this.__TypeHandle.__Game_Prefabs_PlacedSignatureBuildingData_RO_ComponentLookup,
          m_ServiceUpgradeDatas = this.__TypeHandle.__Game_Prefabs_ServiceUpgradeData_RO_ComponentLookup,
          m_XPQueue = queue,
          m_CommandBuffer = this.m_ModificationEndBarrier.CreateCommandBuffer().AsParallelWriter()
        };
        // ISSUE: reference to a compiler-generated field
        this.Dependency = jobData.Schedule<XPBuiltSystem.XPBuiltJob>(this.m_BuiltGroup, JobHandle.CombineDependencies(deps, this.Dependency));
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if ((this.EntityManager.GetComponentData<XP>(this.m_CitySystem.City).m_XPRewardRecord & XPRewardFlags.ElectricityGridBuilt) == (XPRewardFlags) 0 && !this.m_ElectricityGroup.IsEmptyIgnoreFilter)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_City_XP_RW_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        XPBuiltSystem.XPElectricityJob jobData = new XPBuiltSystem.XPElectricityJob()
        {
          m_ElectricityConsumers = this.m_ElectricityGroup.ToComponentDataArray<ElectricityConsumer>((AllocatorManager.AllocatorHandle) Allocator.TempJob),
          m_City = this.m_CitySystem.City,
          m_CityXPs = this.__TypeHandle.__Game_City_XP_RW_ComponentLookup,
          m_XPQueue = queue
        };
        this.Dependency = jobData.Schedule<XPBuiltSystem.XPElectricityJob>(this.Dependency);
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_XPSystem.AddQueueWriter(this.Dependency);
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
    public XPBuiltSystem()
    {
    }

    [BurstCompile]
    public struct XPBuiltJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> m_PlaceableObjectDatas;
      [ReadOnly]
      public ComponentLookup<SignatureBuildingData> m_SignatureBuildingDatas;
      [ReadOnly]
      public ComponentLookup<PlacedSignatureBuildingData> m_PlacedSignatureBuildingDatas;
      [ReadOnly]
      public ComponentLookup<ServiceUpgradeData> m_ServiceUpgradeDatas;
      public NativeQueue<XPGain> m_XPQueue;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray1 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Entity prefab = nativeArray2[index].m_Prefab;
          XPGain xpGain1;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_PlaceableObjectDatas.HasComponent(prefab) && !this.m_PlacedSignatureBuildingDatas.HasComponent(prefab))
          {
            // ISSUE: reference to a compiler-generated field
            PlaceableObjectData placeableObjectData = this.m_PlaceableObjectDatas[prefab];
            if (placeableObjectData.m_XPReward > 0)
            {
              // ISSUE: reference to a compiler-generated field
              ref NativeQueue<XPGain> local = ref this.m_XPQueue;
              xpGain1 = new XPGain();
              xpGain1.amount = placeableObjectData.m_XPReward;
              xpGain1.entity = (Entity) nativeArray1[index];
              xpGain1.reason = XPReason.ServiceBuilding;
              XPGain xpGain2 = xpGain1;
              local.Enqueue(xpGain2);
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_SignatureBuildingDatas.HasComponent(prefab))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<PlacedSignatureBuildingData>(unfilteredChunkIndex, prefab);
            }
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_ServiceUpgradeDatas.HasComponent(prefab))
          {
            // ISSUE: reference to a compiler-generated field
            ServiceUpgradeData serviceUpgradeData = this.m_ServiceUpgradeDatas[prefab];
            if (serviceUpgradeData.m_XPReward > 0)
            {
              // ISSUE: reference to a compiler-generated field
              ref NativeQueue<XPGain> local = ref this.m_XPQueue;
              xpGain1 = new XPGain();
              xpGain1.amount = serviceUpgradeData.m_XPReward;
              xpGain1.entity = (Entity) nativeArray1[index];
              xpGain1.reason = XPReason.ServiceUpgrade;
              XPGain xpGain3 = xpGain1;
              local.Enqueue(xpGain3);
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
    public struct XPElectricityJob : IJob
    {
      [DeallocateOnJobCompletion]
      [ReadOnly]
      public NativeArray<ElectricityConsumer> m_ElectricityConsumers;
      public NativeQueue<XPGain> m_XPQueue;
      [ReadOnly]
      public Entity m_City;
      public ComponentLookup<XP> m_CityXPs;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_ElectricityConsumers.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          if (this.m_ElectricityConsumers[index].m_FulfilledConsumption > 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_XPQueue.Enqueue(new XPGain()
            {
              amount = XPBuiltSystem.kElectricityGridXPBonus,
              entity = Entity.Null,
              reason = XPReason.ElectricityNetwork
            });
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            XP cityXp = this.m_CityXPs[this.m_City];
            cityXp.m_XPRewardRecord |= XPRewardFlags.ElectricityGridBuilt;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_CityXPs[this.m_City] = cityXp;
            break;
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> __Game_Prefabs_PlaceableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SignatureBuildingData> __Game_Prefabs_SignatureBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlacedSignatureBuildingData> __Game_Prefabs_PlacedSignatureBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceUpgradeData> __Game_Prefabs_ServiceUpgradeData_RO_ComponentLookup;
      public ComponentLookup<XP> __Game_City_XP_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup = state.GetComponentLookup<PlaceableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SignatureBuildingData_RO_ComponentLookup = state.GetComponentLookup<SignatureBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlacedSignatureBuildingData_RO_ComponentLookup = state.GetComponentLookup<PlacedSignatureBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceUpgradeData_RO_ComponentLookup = state.GetComponentLookup<ServiceUpgradeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_XP_RW_ComponentLookup = state.GetComponentLookup<XP>();
      }
    }
  }
}
