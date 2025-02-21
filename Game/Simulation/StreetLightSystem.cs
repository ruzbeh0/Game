// Decompiled with JetBrains decompiler
// Type: Game.Simulation.StreetLightSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Rendering;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Assertions;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class StreetLightSystem : GameSystemBase
  {
    private const uint UPDATE_INTERVAL = 256;
    private SimulationSystem m_SimulationSystem;
    private LightingSystem m_LightingSystem;
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_StreetLightQuery;
    private StreetLightSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_LightingSystem = this.World.GetOrCreateSystemManaged<LightingSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_StreetLightQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<UpdateFrame>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Road>(),
          ComponentType.ReadOnly<Building>(),
          ComponentType.ReadOnly<Watercraft>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Destroyed>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_StreetLightQuery);
      Assert.AreEqual(16, 16);
      Assert.AreEqual(16, 16);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_StreetLightQuery.ResetFilter();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_StreetLightQuery.SetSharedComponentFilter<UpdateFrame>(new UpdateFrame(SimulationUtils.GetUpdateFrameWithInterval(this.m_SimulationSystem.frameIndex, (uint) this.GetUpdateInterval(SystemUpdatePhase.GameSimulation), 16)));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_StreetLight_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Watercraft_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Road_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      JobHandle producerJob = new StreetLightSystem.UpdateStreetLightsJob()
      {
        m_EntityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PseudoRandomSeedType = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle,
        m_SubObjectType = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferTypeHandle,
        m_ElectricityNodeConnectionType = this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentTypeHandle,
        m_ElectricityConsumerType = this.__TypeHandle.__Game_Buildings_ElectricityConsumer_RO_ComponentTypeHandle,
        m_RoadType = this.__TypeHandle.__Game_Net_Road_RW_ComponentTypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RW_ComponentTypeHandle,
        m_WatercraftType = this.__TypeHandle.__Game_Vehicles_Watercraft_RW_ComponentTypeHandle,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_ConnectedFlowEdges = this.__TypeHandle.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup,
        m_ElectricityFlowEdges = this.__TypeHandle.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup,
        m_ElectricityNodeConnections = this.__TypeHandle.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup,
        m_StreetLightData = this.__TypeHandle.__Game_Objects_StreetLight_RW_ComponentLookup,
        m_Brightness = Mathf.RoundToInt(this.m_LightingSystem.dayLightBrightness * 1000f),
        m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer().AsParallelWriter()
      }.ScheduleParallel<StreetLightSystem.UpdateStreetLightsJob>(this.m_StreetLightQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.AddJobHandleForProducer(producerJob);
      this.Dependency = producerJob;
    }

    public static void UpdateStreetLightState(ref StreetLight streetLight, Road road)
    {
      if ((road.m_Flags & RoadFlags.LightsOff) != (RoadFlags) 0)
        streetLight.m_State |= StreetLightState.TurnedOff;
      else
        streetLight.m_State &= ~StreetLightState.TurnedOff;
    }

    public static void UpdateStreetLightState(ref StreetLight streetLight, Building building)
    {
      if ((building.m_Flags & BuildingFlags.StreetLightsOff) != BuildingFlags.None)
        streetLight.m_State |= StreetLightState.TurnedOff;
      else
        streetLight.m_State &= ~StreetLightState.TurnedOff;
    }

    public static void UpdateStreetLightState(ref StreetLight streetLight, Watercraft watercraft)
    {
      if ((watercraft.m_Flags & WatercraftFlags.LightsOff) != (WatercraftFlags) 0)
        streetLight.m_State |= StreetLightState.TurnedOff;
      else
        streetLight.m_State &= ~StreetLightState.TurnedOff;
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
    public StreetLightSystem()
    {
    }

    [BurstCompile]
    private struct UpdateStreetLightsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PseudoRandomSeed> m_PseudoRandomSeedType;
      [ReadOnly]
      public BufferTypeHandle<SubObject> m_SubObjectType;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityNodeConnection> m_ElectricityNodeConnectionType;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityConsumer> m_ElectricityConsumerType;
      public ComponentTypeHandle<Road> m_RoadType;
      public ComponentTypeHandle<Building> m_BuildingType;
      public ComponentTypeHandle<Watercraft> m_WatercraftType;
      [ReadOnly]
      public BufferLookup<SubObject> m_SubObjects;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> m_ConnectedFlowEdges;
      [ReadOnly]
      public ComponentLookup<ElectricityFlowEdge> m_ElectricityFlowEdges;
      [ReadOnly]
      public ComponentLookup<ElectricityNodeConnection> m_ElectricityNodeConnections;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<StreetLight> m_StreetLightData;
      [ReadOnly]
      public int m_Brightness;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityTypeHandle);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Road> nativeArray2 = chunk.GetNativeArray<Road>(ref this.m_RoadType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ElectricityNodeConnection> nativeArray3 = chunk.GetNativeArray<ElectricityNodeConnection>(ref this.m_ElectricityNodeConnectionType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Building> nativeArray4 = chunk.GetNativeArray<Building>(ref this.m_BuildingType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<ElectricityConsumer> nativeArray5 = chunk.GetNativeArray<ElectricityConsumer>(ref this.m_ElectricityConsumerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Watercraft> nativeArray6 = chunk.GetNativeArray<Watercraft>(ref this.m_WatercraftType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PseudoRandomSeed> nativeArray7 = chunk.GetNativeArray<PseudoRandomSeed>(ref this.m_PseudoRandomSeedType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubObject> bufferAccessor = chunk.GetBufferAccessor<SubObject>(ref this.m_SubObjectType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          ref Road local = ref nativeArray2.ElementAt<Road>(index);
          if ((local.m_Flags & RoadFlags.IsLit) != (RoadFlags) 0)
          {
            Unity.Mathematics.Random random = nativeArray7[index].GetRandom((uint) PseudoRandomSeed.kBrightnessLimit);
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated field
            bool flag = this.IsElectricityConnected(nativeArray3, index) && (this.m_Brightness < random.NextInt(200, 300) || (local.m_Flags & RoadFlags.AlwaysLit) != 0);
            if (flag == ((local.m_Flags & RoadFlags.LightsOff) != 0))
            {
              if (flag)
                local.m_Flags &= ~RoadFlags.LightsOff;
              else
                local.m_Flags |= RoadFlags.LightsOff;
              DynamicBuffer<SubObject> subObjects = bufferAccessor[index];
              // ISSUE: reference to a compiler-generated method
              this.UpdateStreetLightObjects(unfilteredChunkIndex, subObjects, local);
            }
          }
        }
        for (int index = 0; index < nativeArray4.Length; ++index)
        {
          ref Building local = ref nativeArray4.ElementAt<Building>(index);
          Unity.Mathematics.Random random = nativeArray7[index].GetRandom((uint) PseudoRandomSeed.kBrightnessLimit);
          // ISSUE: reference to a compiler-generated method
          bool flag1 = this.IsElectricityConnected(nativeArray5, index, in local);
          // ISSUE: reference to a compiler-generated field
          bool flag2 = flag1 && this.m_Brightness < random.NextInt(200, 300);
          if (flag2 == ((local.m_Flags & BuildingFlags.StreetLightsOff) != 0))
          {
            if (flag2)
              local.m_Flags &= ~BuildingFlags.StreetLightsOff;
            else
              local.m_Flags |= BuildingFlags.StreetLightsOff;
            DynamicBuffer<SubObject> subObjects = bufferAccessor[index];
            // ISSUE: reference to a compiler-generated method
            this.UpdateStreetLightObjects(unfilteredChunkIndex, subObjects, local);
          }
          if (flag1 != ((local.m_Flags & BuildingFlags.Illuminated) != 0))
          {
            if (flag1)
              local.m_Flags |= BuildingFlags.Illuminated;
            else
              local.m_Flags &= ~BuildingFlags.Illuminated;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<BatchesUpdated>(unfilteredChunkIndex, nativeArray1[index]);
          }
        }
        for (int index = 0; index < nativeArray6.Length; ++index)
        {
          ref Watercraft local = ref nativeArray6.ElementAt<Watercraft>(index);
          // ISSUE: reference to a compiler-generated field
          bool flag = this.m_Brightness < nativeArray7[index].GetRandom((uint) PseudoRandomSeed.kBrightnessLimit).NextInt(200, 300) & (local.m_Flags & WatercraftFlags.DeckLights) > (WatercraftFlags) 0;
          if (flag == (local.m_Flags & WatercraftFlags.LightsOff) > (WatercraftFlags) 0)
          {
            if (flag)
              local.m_Flags &= ~WatercraftFlags.LightsOff;
            else
              local.m_Flags |= WatercraftFlags.LightsOff;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<EffectsUpdated>(unfilteredChunkIndex, nativeArray1[index], new EffectsUpdated());
            if (bufferAccessor.Length != 0)
            {
              DynamicBuffer<SubObject> subObjects = bufferAccessor[index];
              // ISSUE: reference to a compiler-generated method
              this.UpdateStreetLightObjects(unfilteredChunkIndex, subObjects, local);
            }
          }
        }
      }

      private bool IsElectricityConnected(
        NativeArray<ElectricityNodeConnection> nodeConnections,
        int i)
      {
        // ISSUE: reference to a compiler-generated method
        return nodeConnections.Length == 0 || this.IsElectricityConnected(nodeConnections[i]);
      }

      private bool IsElectricityConnected(in ElectricityNodeConnection nodeConnection)
      {
        // ISSUE: reference to a compiler-generated field
        DynamicBuffer<ConnectedFlowEdge> connectedFlowEdge1 = this.m_ConnectedFlowEdges[nodeConnection.m_ElectricityNode];
        bool flag = false;
        foreach (ConnectedFlowEdge connectedFlowEdge2 in connectedFlowEdge1)
        {
          // ISSUE: reference to a compiler-generated field
          ElectricityFlowEdge electricityFlowEdge = this.m_ElectricityFlowEdges[(Entity) connectedFlowEdge2];
          flag |= electricityFlowEdge.isDisconnected;
        }
        return !flag;
      }

      private bool IsElectricityConnected(
        NativeArray<ElectricityConsumer> consumers,
        int i,
        in Building building)
      {
        if (consumers.Length != 0)
          return consumers[i].electricityConnected;
        ElectricityNodeConnection nodeConnection;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        return !this.m_ElectricityNodeConnections.TryGetComponent(building.m_RoadEdge, out nodeConnection) || this.IsElectricityConnected(in nodeConnection);
      }

      private void UpdateStreetLightObjects(
        int jobIndex,
        DynamicBuffer<SubObject> subObjects,
        Road road)
      {
        for (int index = 0; index < subObjects.Length; ++index)
        {
          Entity subObject = subObjects[index].m_SubObject;
          StreetLight componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_StreetLightData.TryGetComponent(subObject, out componentData))
          {
            // ISSUE: reference to a compiler-generated method
            StreetLightSystem.UpdateStreetLightState(ref componentData, road);
            // ISSUE: reference to a compiler-generated field
            this.m_StreetLightData[subObject] = componentData;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<EffectsUpdated>(jobIndex, subObject, new EffectsUpdated());
          }
        }
      }

      private void UpdateStreetLightObjects(
        int jobIndex,
        DynamicBuffer<SubObject> subObjects,
        Building building)
      {
        for (int index = 0; index < subObjects.Length; ++index)
        {
          Entity subObject = subObjects[index].m_SubObject;
          StreetLight componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_StreetLightData.TryGetComponent(subObject, out componentData))
          {
            // ISSUE: reference to a compiler-generated method
            StreetLightSystem.UpdateStreetLightState(ref componentData, building);
            // ISSUE: reference to a compiler-generated field
            this.m_StreetLightData[subObject] = componentData;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<EffectsUpdated>(jobIndex, subObject, new EffectsUpdated());
          }
          DynamicBuffer<SubObject> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubObjects.TryGetBuffer(subObject, out bufferData))
          {
            // ISSUE: reference to a compiler-generated method
            this.UpdateStreetLightObjects(jobIndex, bufferData, building);
          }
        }
      }

      private void UpdateStreetLightObjects(
        int jobIndex,
        DynamicBuffer<SubObject> subObjects,
        Watercraft watercraft)
      {
        for (int index = 0; index < subObjects.Length; ++index)
        {
          Entity subObject = subObjects[index].m_SubObject;
          StreetLight componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_StreetLightData.TryGetComponent(subObject, out componentData))
          {
            // ISSUE: reference to a compiler-generated method
            StreetLightSystem.UpdateStreetLightState(ref componentData, watercraft);
            // ISSUE: reference to a compiler-generated field
            this.m_StreetLightData[subObject] = componentData;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<EffectsUpdated>(jobIndex, subObject, new EffectsUpdated());
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
      [ReadOnly]
      public ComponentTypeHandle<PseudoRandomSeed> __Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<SubObject> __Game_Objects_SubObject_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityNodeConnection> __Game_Simulation_ElectricityNodeConnection_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ElectricityConsumer> __Game_Buildings_ElectricityConsumer_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Road> __Game_Net_Road_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Watercraft> __Game_Vehicles_Watercraft_RW_ComponentTypeHandle;
      [ReadOnly]
      public BufferLookup<SubObject> __Game_Objects_SubObject_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<ConnectedFlowEdge> __Game_Simulation_ConnectedFlowEdge_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityFlowEdge> __Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ElectricityNodeConnection> __Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup;
      public ComponentLookup<StreetLight> __Game_Objects_StreetLight_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PseudoRandomSeed_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PseudoRandomSeed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferTypeHandle = state.GetBufferTypeHandle<SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityNodeConnection_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityNodeConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ElectricityConsumer_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ElectricityConsumer>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Road_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Road>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Building>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Watercraft_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Watercraft>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<SubObject>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ConnectedFlowEdge_RO_BufferLookup = state.GetBufferLookup<ConnectedFlowEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityFlowEdge_RO_ComponentLookup = state.GetComponentLookup<ElectricityFlowEdge>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_ElectricityNodeConnection_RO_ComponentLookup = state.GetComponentLookup<ElectricityNodeConnection>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_StreetLight_RW_ComponentLookup = state.GetComponentLookup<StreetLight>();
      }
    }
  }
}
