// Decompiled with JetBrains decompiler
// Type: Game.Simulation.SewageOutletAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.Common;
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
  public class SewageOutletAISystem : GameSystemBase
  {
    private IconCommandSystem m_IconCommandSystem;
    private EntityQuery m_OutletQuery;
    private EntityQuery m_ParameterQuery;
    private SewageOutletAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 64;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 128;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_OutletQuery = this.GetEntityQuery(ComponentType.ReadWrite<Game.Buildings.SewageOutlet>(), ComponentType.ReadOnly<WaterPipeBuildingConnection>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_ParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<WaterPipeParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_OutletQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterSourceData_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SewageOutletData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_SewageOutlet_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_IconElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      SewageOutletAISystem.OutletTickJob jobData = new SewageOutletAISystem.OutletTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_BuildingConnectionType = this.__TypeHandle.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentTypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_SubObjectType = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle,
        m_IconElementType = this.__TypeHandle.__Game_Notifications_IconElement_RO_BufferTypeHandle,
        m_SewageOutletType = this.__TypeHandle.__Game_Buildings_SewageOutlet_RW_ComponentTypeHandle,
        m_OutletDatas = this.__TypeHandle.__Game_Prefabs_SewageOutletData_RO_ComponentLookup,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_SewageOutletDatas = this.__TypeHandle.__Game_Prefabs_SewageOutletData_RO_ComponentLookup,
        m_FlowEdges = this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RW_ComponentLookup,
        m_WaterSources = this.__TypeHandle.__Game_Simulation_WaterSourceData_RW_ComponentLookup,
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer(),
        m_Parameters = this.m_ParameterQuery.GetSingleton<WaterPipeParameterData>()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<SewageOutletAISystem.OutletTickJob>(this.m_OutletQuery, this.Dependency);
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
    public SewageOutletAISystem()
    {
    }

    [BurstCompile]
    public struct OutletTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<WaterPipeBuildingConnection> m_BuildingConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<Game.Objects.SubObject> m_SubObjectType;
      [ReadOnly]
      public BufferTypeHandle<IconElement> m_IconElementType;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> m_EfficiencyType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      public ComponentTypeHandle<Game.Buildings.SewageOutlet> m_SewageOutletType;
      [ReadOnly]
      public ComponentLookup<SewageOutletData> m_OutletDatas;
      [ReadOnly]
      public ComponentLookup<SewageOutletData> m_SewageOutletDatas;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [NativeDisableContainerSafetyRestriction]
      public ComponentLookup<WaterPipeEdge> m_FlowEdges;
      [NativeDisableContainerSafetyRestriction]
      public ComponentLookup<WaterSourceData> m_WaterSources;
      public IconCommandBuffer m_IconCommandBuffer;
      public WaterPipeParameterData m_Parameters;

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
        NativeArray<WaterPipeBuildingConnection> nativeArray3 = chunk.GetNativeArray<WaterPipeBuildingConnection>(ref this.m_BuildingConnectionType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor1 = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor2 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Objects.SubObject> bufferAccessor3 = chunk.GetBufferAccessor<Game.Objects.SubObject>(ref this.m_SubObjectType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<IconElement> bufferAccessor4 = chunk.GetBufferAccessor<IconElement>(ref this.m_IconElementType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Buildings.SewageOutlet> nativeArray4 = chunk.GetNativeArray<Game.Buildings.SewageOutlet>(ref this.m_SewageOutletType);
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          Entity entity = nativeArray1[index1];
          Entity prefab = nativeArray2[index1].m_Prefab;
          WaterPipeBuildingConnection buildingConnection = nativeArray3[index1];
          DynamicBuffer<IconElement> iconElements = bufferAccessor4.Length != 0 ? bufferAccessor4[index1] : new DynamicBuffer<IconElement>();
          ref Game.Buildings.SewageOutlet local = ref nativeArray4.ElementAt<Game.Buildings.SewageOutlet>(index1);
          if (buildingConnection.m_ProducerEdge == Entity.Null)
          {
            UnityEngine.Debug.LogError((object) "SewageOutlet is missing producer edge!");
          }
          else
          {
            float efficiency = BuildingUtils.GetEfficiency(bufferAccessor1, index1);
            // ISSUE: reference to a compiler-generated field
            SewageOutletData outletData = this.m_OutletDatas[prefab];
            if (bufferAccessor2.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              UpgradeUtils.CombineStats<SewageOutletData>(ref outletData, bufferAccessor2[index1], ref this.m_Prefabs, ref this.m_SewageOutletDatas);
            }
            int num1 = math.max(0, local.m_LastProcessed - local.m_LastPurified);
            int num2 = local.m_LastPurified - local.m_UsedPurified;
            int num3 = num1 + num2;
            if (bufferAccessor3.Length != 0)
            {
              DynamicBuffer<Game.Objects.SubObject> dynamicBuffer = bufferAccessor3[index1];
              for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
              {
                Entity subObject = dynamicBuffer[index2].m_SubObject;
                WaterSourceData componentData;
                // ISSUE: reference to a compiler-generated field
                if (this.m_WaterSources.TryGetComponent(subObject, out componentData))
                {
                  // ISSUE: reference to a compiler-generated field
                  componentData.m_Amount = math.min(2.5f, this.m_Parameters.m_SurfaceWaterUsageMultiplier * (float) num3);
                  componentData.m_Polluted = (double) num3 != 0.0 ? (float) num1 / (float) num3 : 0.0f;
                  // ISSUE: reference to a compiler-generated field
                  this.m_WaterSources[subObject] = componentData;
                }
              }
            }
            local.m_Capacity = Mathf.RoundToInt(efficiency * (float) outletData.m_Capacity);
            // ISSUE: reference to a compiler-generated field
            WaterPipeEdge flowEdge = this.m_FlowEdges[buildingConnection.m_ProducerEdge];
            local.m_LastProcessed = flowEdge.m_SewageFlow;
            local.m_LastPurified = Mathf.RoundToInt(outletData.m_Purification * (float) local.m_LastProcessed);
            local.m_UsedPurified = 0;
            flowEdge.m_SewageCapacity = local.m_Capacity;
            // ISSUE: reference to a compiler-generated field
            this.m_FlowEdges[buildingConnection.m_ProducerEdge] = flowEdge;
            bool flag = (flowEdge.m_Flags & WaterPipeEdgeFlags.SewageBackup) != 0;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.UpdateNotification(entity, this.m_Parameters.m_NotEnoughSewageCapacityNotification, local.m_Capacity > 0 & flag, iconElements);
          }
        }
      }

      private void UpdateNotification(
        Entity entity,
        Entity notificationPrefab,
        bool enabled,
        DynamicBuffer<IconElement> iconElements)
      {
        // ISSUE: reference to a compiler-generated method
        bool flag = this.HasNotification(iconElements, notificationPrefab);
        if (enabled == flag)
          return;
        if (enabled)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_IconCommandBuffer.Add(entity, notificationPrefab);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_IconCommandBuffer.Remove(entity, notificationPrefab);
        }
      }

      private bool HasNotification(
        DynamicBuffer<IconElement> iconElements,
        Entity notificationPrefab)
      {
        if (iconElements.IsCreated)
        {
          foreach (IconElement iconElement in iconElements)
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_Prefabs[iconElement.m_Icon].m_Prefab == notificationPrefab)
              return true;
          }
        }
        return false;
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
      public ComponentTypeHandle<WaterPipeBuildingConnection> __Game_Simulation_WaterPipeBuildingConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<IconElement> __Game_Notifications_IconElement_RO_BufferTypeHandle;
      public ComponentTypeHandle<Game.Buildings.SewageOutlet> __Game_Buildings_SewageOutlet_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<SewageOutletData> __Game_Prefabs_SewageOutletData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      public ComponentLookup<WaterPipeEdge> __Game_Simulation_WaterPipeEdge_RW_ComponentLookup;
      public ComponentLookup<WaterSourceData> __Game_Simulation_WaterSourceData_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaterPipeBuildingConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_IconElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<IconElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_SewageOutlet_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.SewageOutlet>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SewageOutletData_RO_ComponentLookup = state.GetComponentLookup<SewageOutletData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeEdge_RW_ComponentLookup = state.GetComponentLookup<WaterPipeEdge>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterSourceData_RW_ComponentLookup = state.GetComponentLookup<WaterSourceData>();
      }
    }
  }
}
