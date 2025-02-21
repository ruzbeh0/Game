// Decompiled with JetBrains decompiler
// Type: Game.Simulation.WaterPumpingStationAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.Common;
using Game.Notifications;
using Game.Prefabs;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class WaterPumpingStationAISystem : GameSystemBase
  {
    private GroundWaterSystem m_GroundWaterSystem;
    private WaterSystem m_WaterSystem;
    private IconCommandSystem m_IconCommandSystem;
    private EntityQuery m_PumpQuery;
    private EntityQuery m_ParameterQuery;
    private WaterPumpingStationAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 128;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 64;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_GroundWaterSystem = this.World.GetOrCreateSystemManaged<GroundWaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WaterSystem = this.World.GetOrCreateSystemManaged<WaterSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PumpQuery = this.GetEntityQuery(ComponentType.ReadWrite<Game.Buildings.WaterPumpingStation>(), ComponentType.ReadOnly<WaterPipeBuildingConnection>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.ReadOnly<Game.Objects.Transform>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_ParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<WaterPipeParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PumpQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ParameterQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterSourceData_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_WaterPumpingStationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_SewageOutlet_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_WaterPumpingStation_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_IconElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle deps;
      JobHandle dependencies;
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      WaterPumpingStationAISystem.PumpTickJob jobData = new WaterPumpingStationAISystem.PumpTickJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_SubObjectType = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_BuildingConnectionType = this.__TypeHandle.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentTypeHandle,
        m_IconElementType = this.__TypeHandle.__Game_Notifications_IconElement_RO_BufferTypeHandle,
        m_WaterPumpingStationType = this.__TypeHandle.__Game_Buildings_WaterPumpingStation_RW_ComponentTypeHandle,
        m_SewageOutletType = this.__TypeHandle.__Game_Buildings_SewageOutlet_RW_ComponentTypeHandle,
        m_EfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RW_BufferTypeHandle,
        m_Prefabs = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PumpDatas = this.__TypeHandle.__Game_Prefabs_WaterPumpingStationData_RO_ComponentLookup,
        m_Transforms = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_WaterSources = this.__TypeHandle.__Game_Simulation_WaterSourceData_RW_ComponentLookup,
        m_FlowEdges = this.__TypeHandle.__Game_Simulation_WaterPipeEdge_RW_ComponentLookup,
        m_WaterSurfaceData = this.m_WaterSystem.GetSurfaceData(out deps),
        m_GroundWaterMap = this.m_GroundWaterSystem.GetMap(false, out dependencies),
        m_IconCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer(),
        m_Parameters = this.m_ParameterQuery.GetSingleton<WaterPipeParameterData>()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<WaterPumpingStationAISystem.PumpTickJob>(this.m_PumpQuery, JobHandle.CombineDependencies(this.Dependency, deps, dependencies));
      // ISSUE: reference to a compiler-generated field
      this.m_GroundWaterSystem.AddWriter(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterSystem.AddSurfaceReader(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(this.Dependency);
    }

    public static float GetSurfaceWaterAvailability(
      float3 position,
      AllowedWaterTypes allowedTypes,
      WaterSurfaceData waterSurfaceData,
      float effectiveDepth)
    {
      return math.clamp(WaterUtils.SampleDepth(ref waterSurfaceData, position) / effectiveDepth, 0.0f, 1f);
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
    public WaterPumpingStationAISystem()
    {
    }

    [BurstCompile]
    public struct PumpTickJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> m_TransformType;
      [ReadOnly]
      public BufferTypeHandle<Game.Objects.SubObject> m_SubObjectType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      [ReadOnly]
      public ComponentTypeHandle<WaterPipeBuildingConnection> m_BuildingConnectionType;
      [ReadOnly]
      public BufferTypeHandle<IconElement> m_IconElementType;
      public ComponentTypeHandle<Game.Buildings.WaterPumpingStation> m_WaterPumpingStationType;
      public ComponentTypeHandle<Game.Buildings.SewageOutlet> m_SewageOutletType;
      public BufferTypeHandle<Efficiency> m_EfficiencyType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_Prefabs;
      [ReadOnly]
      public ComponentLookup<WaterPumpingStationData> m_PumpDatas;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> m_Transforms;
      public ComponentLookup<WaterSourceData> m_WaterSources;
      public ComponentLookup<WaterPipeEdge> m_FlowEdges;
      [ReadOnly]
      public WaterSurfaceData m_WaterSurfaceData;
      public NativeArray<GroundWater> m_GroundWaterMap;
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
        NativeArray<PrefabRef> nativeArray2 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Objects.Transform> nativeArray3 = chunk.GetNativeArray<Game.Objects.Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Game.Objects.SubObject> bufferAccessor1 = chunk.GetBufferAccessor<Game.Objects.SubObject>(ref this.m_SubObjectType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor2 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<WaterPipeBuildingConnection> nativeArray4 = chunk.GetNativeArray<WaterPipeBuildingConnection>(ref this.m_BuildingConnectionType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<IconElement> bufferAccessor3 = chunk.GetBufferAccessor<IconElement>(ref this.m_IconElementType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Buildings.WaterPumpingStation> nativeArray5 = chunk.GetNativeArray<Game.Buildings.WaterPumpingStation>(ref this.m_WaterPumpingStationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Buildings.SewageOutlet> nativeArray6 = chunk.GetNativeArray<Game.Buildings.SewageOutlet>(ref this.m_SewageOutletType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor4 = chunk.GetBufferAccessor<Efficiency>(ref this.m_EfficiencyType);
        Span<float> factors = stackalloc float[28];
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          Entity entity = nativeArray1[index1];
          Entity prefab = nativeArray2[index1].m_Prefab;
          WaterPipeBuildingConnection buildingConnection = nativeArray4[index1];
          DynamicBuffer<IconElement> iconElements = bufferAccessor3.Length != 0 ? bufferAccessor3[index1] : new DynamicBuffer<IconElement>();
          ref Game.Buildings.WaterPumpingStation local1 = ref nativeArray5.ElementAt<Game.Buildings.WaterPumpingStation>(index1);
          // ISSUE: reference to a compiler-generated field
          WaterPumpingStationData pumpData = this.m_PumpDatas[prefab];
          if (bufferAccessor2.Length != 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            UpgradeUtils.CombineStats<WaterPumpingStationData>(ref pumpData, bufferAccessor2[index1], ref this.m_Prefabs, ref this.m_PumpDatas);
          }
          if (buildingConnection.m_ProducerEdge == Entity.Null)
          {
            UnityEngine.Debug.LogError((object) "WaterPumpingStation is missing producer edge!");
          }
          else
          {
            if (bufferAccessor4.Length != 0)
            {
              BuildingUtils.GetEfficiencyFactors(bufferAccessor4[index1], factors);
              factors[19] = 1f;
            }
            else
              factors.Fill(1f);
            float efficiency = BuildingUtils.GetEfficiency(factors);
            // ISSUE: reference to a compiler-generated field
            WaterPipeEdge flowEdge = this.m_FlowEdges[buildingConnection.m_ProducerEdge];
            local1.m_LastProduction = flowEdge.m_FreshFlow;
            float num1 = (float) local1.m_LastProduction;
            local1.m_Pollution = 0.0f;
            local1.m_Capacity = 0;
            int num2 = 0;
            if (nativeArray6.Length != 0)
            {
              ref Game.Buildings.SewageOutlet local2 = ref nativeArray6.ElementAt<Game.Buildings.SewageOutlet>(index1);
              num2 = local2.m_LastPurified;
              local2.m_UsedPurified = math.min(local1.m_LastProduction, local2.m_LastPurified);
              num1 -= (float) local2.m_UsedPurified;
            }
            float num3 = 0.0f;
            float num4 = 0.0f;
            bool flag1 = false;
            bool flag2 = false;
            if (pumpData.m_Types != AllowedWaterTypes.None)
            {
              if ((pumpData.m_Types & AllowedWaterTypes.Groundwater) != AllowedWaterTypes.None)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                GroundWater groundWater = GroundWaterSystem.GetGroundWater(nativeArray3[index1].m_Position, this.m_GroundWaterMap);
                float num5 = (float) groundWater.m_Polluted / math.max(1f, (float) groundWater.m_Amount);
                // ISSUE: reference to a compiler-generated field
                double num6 = (double) groundWater.m_Amount / (double) this.m_Parameters.m_GroundwaterPumpEffectiveAmount;
                float num7 = math.clamp((float) num6 * (float) pumpData.m_Capacity, 0.0f, (float) pumpData.m_Capacity - num3);
                num3 += num7;
                num4 += num5 * num7;
                flag1 = num6 < 0.75 && (double) groundWater.m_Amount < 0.75 * (double) groundWater.m_Max;
                // ISSUE: reference to a compiler-generated field
                int x = (int) math.ceil(num1 * this.m_Parameters.m_GroundwaterUsageMultiplier);
                int amount = math.min(x, (int) groundWater.m_Amount);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                GroundWaterSystem.ConsumeGroundWater(nativeArray3[index1].m_Position, this.m_GroundWaterMap, amount);
                // ISSUE: reference to a compiler-generated field
                num1 = (float) Mathf.FloorToInt((float) (x - amount) / this.m_Parameters.m_GroundwaterUsageMultiplier);
              }
              if ((pumpData.m_Types & AllowedWaterTypes.SurfaceWater) != AllowedWaterTypes.None && bufferAccessor1.Length != 0)
              {
                DynamicBuffer<Game.Objects.SubObject> dynamicBuffer = bufferAccessor1[index1];
                for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
                {
                  Entity subObject = dynamicBuffer[index2].m_SubObject;
                  WaterSourceData componentData1;
                  Game.Objects.Transform componentData2;
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_WaterSources.TryGetComponent(subObject, out componentData1) && this.m_Transforms.TryGetComponent(subObject, out componentData2))
                  {
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated field
                    // ISSUE: reference to a compiler-generated method
                    double waterAvailability = (double) WaterPumpingStationAISystem.GetSurfaceWaterAvailability(componentData2.m_Position, pumpData.m_Types, this.m_WaterSurfaceData, this.m_Parameters.m_SurfaceWaterPumpEffectiveDepth);
                    // ISSUE: reference to a compiler-generated field
                    float num8 = WaterUtils.SamplePolluted(ref this.m_WaterSurfaceData, componentData2.m_Position);
                    float num9 = math.clamp((float) waterAvailability * (float) pumpData.m_Capacity, 0.0f, (float) pumpData.m_Capacity - num3);
                    num3 += num9;
                    num4 += num9 * num8;
                    flag2 = waterAvailability < 0.75;
                    // ISSUE: reference to a compiler-generated field
                    componentData1.m_Amount = -this.m_Parameters.m_SurfaceWaterUsageMultiplier * num1;
                    componentData1.m_Polluted = 0.0f;
                    // ISSUE: reference to a compiler-generated field
                    this.m_WaterSources[subObject] = componentData1;
                    num1 = 0.0f;
                  }
                }
              }
            }
            else
            {
              num3 = (float) pumpData.m_Capacity;
              num4 = 0.0f;
            }
            local1.m_Capacity = (int) math.round(efficiency * num3 + (float) num2);
            local1.m_Pollution = local1.m_Capacity > 0 ? (1f - pumpData.m_Purification) * num4 / (float) local1.m_Capacity : 0.0f;
            flowEdge.m_FreshCapacity = local1.m_Capacity;
            flowEdge.m_FreshPollution = local1.m_Capacity > 0 ? local1.m_Pollution : 0.0f;
            // ISSUE: reference to a compiler-generated field
            this.m_FlowEdges[buildingConnection.m_ProducerEdge] = flowEdge;
            if (bufferAccessor4.Length != 0)
            {
              if (pumpData.m_Capacity > 0)
              {
                float num10 = (num3 + (float) num2) / (float) (pumpData.m_Capacity + num2);
                factors[19] = num10;
              }
              BuildingUtils.SetEfficiencyFactors(bufferAccessor4[index1], factors);
            }
            bool flag3 = (double) num3 < 0.10000000149011612 * (double) pumpData.m_Capacity;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.UpdateNotification(entity, this.m_Parameters.m_NotEnoughGroundwaterNotification, flag1 & flag3, iconElements);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.UpdateNotification(entity, this.m_Parameters.m_NotEnoughSurfaceWaterNotification, flag2 & flag3, iconElements);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.UpdateNotification(entity, this.m_Parameters.m_DirtyWaterPumpNotification, (double) local1.m_Pollution > (double) this.m_Parameters.m_MaxToleratedPollution, iconElements);
            bool flag4 = (flowEdge.m_Flags & WaterPipeEdgeFlags.WaterShortage) != 0;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.UpdateNotification(entity, this.m_Parameters.m_NotEnoughWaterCapacityNotification, local1.m_Capacity > 0 & flag4, iconElements);
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
      public ComponentTypeHandle<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<WaterPipeBuildingConnection> __Game_Simulation_WaterPipeBuildingConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<IconElement> __Game_Notifications_IconElement_RO_BufferTypeHandle;
      public ComponentTypeHandle<Game.Buildings.WaterPumpingStation> __Game_Buildings_WaterPumpingStation_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Buildings.SewageOutlet> __Game_Buildings_SewageOutlet_RW_ComponentTypeHandle;
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RW_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WaterPumpingStationData> __Game_Prefabs_WaterPumpingStationData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentLookup;
      public ComponentLookup<WaterSourceData> __Game_Simulation_WaterSourceData_RW_ComponentLookup;
      public ComponentLookup<WaterPipeEdge> __Game_Simulation_WaterPipeEdge_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferTypeHandle = state.GetBufferTypeHandle<Game.Objects.SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeBuildingConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<WaterPipeBuildingConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_IconElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<IconElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_WaterPumpingStation_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.WaterPumpingStation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_SewageOutlet_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.SewageOutlet>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RW_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_WaterPumpingStationData_RO_ComponentLookup = state.GetComponentLookup<WaterPumpingStationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterSourceData_RW_ComponentLookup = state.GetComponentLookup<WaterSourceData>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_WaterPipeEdge_RW_ComponentLookup = state.GetComponentLookup<WaterPipeEdge>();
      }
    }
  }
}
