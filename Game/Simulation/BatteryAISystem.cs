// Decompiled with JetBrains decompiler
// Type: Game.Simulation.BatteryAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Colossal.Mathematics;
using Game.Buildings;
using Game.Common;
using Game.Debug;
using Game.Notifications;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class BatteryAISystem : GameSystemBase
  {
    private EntityQuery m_BatteryQuery;
    private EntityQuery m_SettingsQuery;
    private IconCommandSystem m_IconCommandSystem;
    private BatteryAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 128;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 0;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_BatteryQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.Battery>(), ComponentType.ReadOnly<ElectricityBuildingConnection>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_SettingsQuery = this.GetEntityQuery(ComponentType.ReadOnly<ElectricityParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_BatteryQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ServiceUsage_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ResourceConsumer_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_EmergencyGeneratorData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BatteryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_EmergencyGenerator_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Battery_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      BatteryAISystem.BatteryTickJob jobData = new BatteryAISystem.BatteryTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_BuildingConnectionType = this.__TypeHandle.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_BatteryType = this.__TypeHandle.__Game_Buildings_Battery_RW_ComponentTypeHandle,
        m_EmergencyGeneratorType = this.__TypeHandle.__Game_Buildings_EmergencyGenerator_RW_ComponentTypeHandle,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_BatteryDatas = this.__TypeHandle.__Game_Prefabs_BatteryData_RO_ComponentLookup,
        m_EmergencyGeneratorDatas = this.__TypeHandle.__Game_Prefabs_EmergencyGeneratorData_RO_ComponentLookup,
        m_ResourceConsumers = this.__TypeHandle.__Game_Buildings_ResourceConsumer_RO_ComponentLookup,
        m_ServiceUsages = this.__TypeHandle.__Game_Buildings_ServiceUsage_RW_ComponentLookup,
        m_FlowEdges = this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RW_ComponentLookup,
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer(),
        m_ElectricityParameterData = this.m_SettingsQuery.GetSingleton<ElectricityParameterData>()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<BatteryAISystem.BatteryTickJob>(this.m_BatteryQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(this.Dependency);
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
    public BatteryAISystem()
    {
    }

    [BurstCompile]
    private struct BatteryTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityBuildingConnection> m_BuildingConnectionType;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> m_EfficiencyType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      public ComponentTypeHandle<Game.Buildings.Battery> m_BatteryType;
      public ComponentTypeHandle<Game.Buildings.EmergencyGenerator> m_EmergencyGeneratorType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<BatteryData> m_BatteryDatas;
      [ReadOnly]
      public ComponentLookup<EmergencyGeneratorData> m_EmergencyGeneratorDatas;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.ResourceConsumer> m_ResourceConsumers;
      [NativeDisableContainerSafetyRestriction]
      public ComponentLookup<ServiceUsage> m_ServiceUsages;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<ElectricityFlowEdge> m_FlowEdges;
      public IconCommandBuffer m_IconCommandBuffer;
      public ElectricityParameterData m_ElectricityParameterData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Buildings.Battery> nativeArray3 = chunk.GetNativeArray<Game.Buildings.Battery>(ref this.m_BatteryType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Buildings.EmergencyGenerator> nativeArray4 = chunk.GetNativeArray<Game.Buildings.EmergencyGenerator>(ref this.m_EmergencyGeneratorType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ElectricityBuildingConnection> nativeArray5 = chunk.GetNativeArray<ElectricityBuildingConnection>(ref this.m_BuildingConnectionType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor1 = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor2 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Entity owner = nativeArray1[index];
          Entity prefab = nativeArray2[index].m_Prefab;
          ref Game.Buildings.Battery local1 = ref nativeArray3.ElementAt<Game.Buildings.Battery>(index);
          ElectricityBuildingConnection buildingConnection = nativeArray5[index];
          float efficiency = BuildingUtils.GetEfficiency(bufferAccessor1, index);
          if (buildingConnection.m_ChargeEdge == Entity.Null || buildingConnection.m_DischargeEdge == Entity.Null)
          {
            UnityEngine.Debug.LogError((object) "Battery is missing charge or discharge edge!");
          }
          else
          {
            BatteryData componentData1;
            // ISSUE: reference to a compiler-generated field
            this.m_BatteryDatas.TryGetComponent(prefab, out componentData1);
            if (bufferAccessor2.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              UpgradeUtils.CombineStats<BatteryData>(ref componentData1, bufferAccessor2[index], ref this.m_Prefabs, ref this.m_BatteryDatas);
            }
            bool flag1 = local1.m_StoredEnergy == 0L;
            // ISSUE: reference to a compiler-generated field
            ElectricityFlowEdge flowEdge1 = this.m_FlowEdges[buildingConnection.m_DischargeEdge];
            // ISSUE: reference to a compiler-generated field
            ElectricityFlowEdge flowEdge2 = this.m_FlowEdges[buildingConnection.m_ChargeEdge];
            int num1 = flowEdge2.m_Flow - flowEdge1.m_Flow;
            local1.m_StoredEnergy = math.clamp(local1.m_StoredEnergy + (long) num1, 0L, componentData1.capacityTicks);
            local1.m_Capacity = componentData1.m_Capacity;
            local1.m_LastFlow = num1;
            bool flag2 = local1.m_StoredEnergy == 0L;
            if (flag2 && !flag1)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_IconCommandBuffer.Add(owner, this.m_ElectricityParameterData.m_BatteryEmptyNotificationPrefab, IconPriority.Problem);
            }
            else if (!flag2 & flag1)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_IconCommandBuffer.Remove(owner, this.m_ElectricityParameterData.m_BatteryEmptyNotificationPrefab);
            }
            if (nativeArray4.Length != 0)
            {
              Bounds1 bounds1 = new Bounds1();
              int x = 0;
              int num2 = 0;
              if (bufferAccessor2.Length != 0)
              {
                foreach (InstalledUpgrade installedUpgrade in bufferAccessor2[index])
                {
                  EmergencyGeneratorData componentData2;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (!BuildingUtils.CheckOption(installedUpgrade, BuildingOption.Inactive) && this.m_EmergencyGeneratorDatas.TryGetComponent((Entity) this.m_Prefabs[(Entity) installedUpgrade], out componentData2))
                  {
                    bounds1 = new Bounds1(math.max(bounds1.min, componentData2.m_ActivationThreshold.min), math.max(bounds1.max, componentData2.m_ActivationThreshold.max));
                    // ISSUE: reference to a compiler-generated method
                    if (this.HasResources((Entity) installedUpgrade))
                    {
                      x += Mathf.CeilToInt(efficiency * (float) componentData2.m_ElectricityProduction);
                      num2 += componentData2.m_ElectricityProduction;
                    }
                  }
                }
              }
              ref Game.Buildings.EmergencyGenerator local2 = ref nativeArray4.ElementAt<Game.Buildings.EmergencyGenerator>(index);
              float num3 = (float) local1.m_StoredEnergy / (float) math.max(1L, componentData1.capacityTicks);
              bool flag3 = local2.m_Production > 0;
              bool flag4 = (double) efficiency > 0.0 && ((double) num3 < (double) bounds1.min || flag3 && (double) num3 < (double) bounds1.max);
              local2.m_Production = flag4 ? math.min(x, (int) (componentData1.capacityTicks - local1.m_StoredEnergy)) : 0;
              float num4 = num2 > 0 ? (float) local2.m_Production / (float) num2 : 0.0f;
              if (bufferAccessor2.Length != 0)
              {
                foreach (InstalledUpgrade installedUpgrade in bufferAccessor2[index])
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (!BuildingUtils.CheckOption(installedUpgrade, BuildingOption.Inactive) && this.m_EmergencyGeneratorDatas.HasComponent((Entity) installedUpgrade) && this.m_ServiceUsages.HasComponent((Entity) installedUpgrade))
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    this.m_ServiceUsages[(Entity) installedUpgrade] = new ServiceUsage()
                    {
                      m_Usage = this.HasResources((Entity) installedUpgrade) ? num4 : 0.0f
                    };
                  }
                }
              }
              Assert.IsTrue(local2.m_Production >= 0);
              local1.m_StoredEnergy += (long) local2.m_Production;
            }
            flowEdge1.m_Capacity = (double) efficiency > 0.0 ? (int) math.min((long) componentData1.m_PowerOutput, local1.m_StoredEnergy) : 0;
            // ISSUE: reference to a compiler-generated field
            this.m_FlowEdges[buildingConnection.m_DischargeEdge] = flowEdge1;
            flowEdge2.m_Capacity = (int) math.min((long) Mathf.RoundToInt(efficiency * (float) componentData1.m_PowerOutput), componentData1.capacityTicks - local1.m_StoredEnergy);
            // ISSUE: reference to a compiler-generated field
            this.m_FlowEdges[buildingConnection.m_ChargeEdge] = flowEdge2;
          }
        }
      }

      private bool HasResources(Entity upgrade)
      {
        Game.Buildings.ResourceConsumer componentData;
        // ISSUE: reference to a compiler-generated field
        return !this.m_ResourceConsumers.TryGetComponent(upgrade, out componentData) || componentData.m_ResourceAvailability > (byte) 0;
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
      public ComponentTypeHandle<ElectricityBuildingConnection> __Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      public ComponentTypeHandle<Game.Buildings.Battery> __Game_Buildings_Battery_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Buildings.EmergencyGenerator> __Game_Buildings_EmergencyGenerator_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BatteryData> __Game_Prefabs_BatteryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<EmergencyGeneratorData> __Game_Prefabs_EmergencyGeneratorData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.ResourceConsumer> __Game_Buildings_ResourceConsumer_RO_ComponentLookup;
      public ComponentLookup<ServiceUsage> __Game_Buildings_ServiceUsage_RW_ComponentLookup;
      public ComponentLookup<ElectricityFlowEdge> __Game_Simulation_ElectricityFlowEdge_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityBuildingConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityBuildingConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Battery_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.Battery>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_EmergencyGenerator_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.EmergencyGenerator>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BatteryData_RO_ComponentLookup = state.GetComponentLookup<BatteryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_EmergencyGeneratorData_RO_ComponentLookup = state.GetComponentLookup<EmergencyGeneratorData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ResourceConsumer_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.ResourceConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ServiceUsage_RW_ComponentLookup = state.GetComponentLookup<ServiceUsage>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityFlowEdge_RW_ComponentLookup = state.GetComponentLookup<ElectricityFlowEdge>();
      }
    }
  }
}
