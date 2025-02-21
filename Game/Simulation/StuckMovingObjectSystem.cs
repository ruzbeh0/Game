// Decompiled with JetBrains decompiler
// Type: Game.Simulation.StuckMovingObjectSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Creatures;
using Game.Pathfind;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class StuckMovingObjectSystem : GameSystemBase
  {
    private SimulationSystem m_SimulationSystem;
    private EntityQuery m_ObjectQuery;
    private StuckMovingObjectSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 4;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectQuery = this.GetEntityQuery(ComponentType.ReadOnly<Blocker>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ObjectQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      uint index = (this.m_SimulationSystem.frameIndex >> 2) % 16U;
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectQuery.ResetFilter();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectQuery.SetSharedComponentFilter<UpdateFrame>(new UpdateFrame(index));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_Dispatched_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedTrain_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Blocker_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Car_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_RideNeeder_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_GroupMember_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Blocker_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      StuckMovingObjectSystem.StuckCheckJob jobData = new StuckMovingObjectSystem.StuckCheckJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_BlockerType = this.__TypeHandle.__Game_Vehicles_Blocker_RO_ComponentTypeHandle,
        m_GroupMemberType = this.__TypeHandle.__Game_Creatures_GroupMember_RO_ComponentTypeHandle,
        m_CurrentVehicleType = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle,
        m_RideNeederType = this.__TypeHandle.__Game_Creatures_RideNeeder_RO_ComponentTypeHandle,
        m_TargetType = this.__TypeHandle.__Game_Common_Target_RO_ComponentTypeHandle,
        m_CarType = this.__TypeHandle.__Game_Vehicles_Car_RO_ComponentTypeHandle,
        m_BlockerData = this.__TypeHandle.__Game_Vehicles_Blocker_RO_ComponentLookup,
        m_ControllerData = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_ParkedTrainData = this.__TypeHandle.__Game_Vehicles_ParkedTrain_RO_ComponentLookup,
        m_CurrentVehicleData = this.__TypeHandle.__Game_Creatures_CurrentVehicle_RO_ComponentLookup,
        m_DispatchedData = this.__TypeHandle.__Game_Simulation_Dispatched_RO_ComponentLookup,
        m_PathOwnerType = this.__TypeHandle.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle,
        m_AnimalCurrentLaneType = this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RW_ComponentTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<StuckMovingObjectSystem.StuckCheckJob>(this.m_ObjectQuery, this.Dependency);
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
    public StuckMovingObjectSystem()
    {
    }

    [BurstCompile]
    private struct StuckCheckJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Blocker> m_BlockerType;
      [ReadOnly]
      public ComponentTypeHandle<GroupMember> m_GroupMemberType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentVehicle> m_CurrentVehicleType;
      [ReadOnly]
      public ComponentTypeHandle<RideNeeder> m_RideNeederType;
      [ReadOnly]
      public ComponentTypeHandle<Target> m_TargetType;
      [ReadOnly]
      public ComponentTypeHandle<Car> m_CarType;
      [ReadOnly]
      public ComponentLookup<Blocker> m_BlockerData;
      [ReadOnly]
      public ComponentLookup<Controller> m_ControllerData;
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      [ReadOnly]
      public ComponentLookup<ParkedTrain> m_ParkedTrainData;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> m_CurrentVehicleData;
      [ReadOnly]
      public ComponentLookup<Dispatched> m_DispatchedData;
      public ComponentTypeHandle<PathOwner> m_PathOwnerType;
      public ComponentTypeHandle<AnimalCurrentLane> m_AnimalCurrentLaneType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Blocker> nativeArray2 = chunk.GetNativeArray<Blocker>(ref this.m_BlockerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<GroupMember> nativeArray3 = chunk.GetNativeArray<GroupMember>(ref this.m_GroupMemberType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentVehicle> nativeArray4 = chunk.GetNativeArray<CurrentVehicle>(ref this.m_CurrentVehicleType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<RideNeeder> nativeArray5 = chunk.GetNativeArray<RideNeeder>(ref this.m_RideNeederType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Target> nativeArray6 = chunk.GetNativeArray<Target>(ref this.m_TargetType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PathOwner> nativeArray7 = chunk.GetNativeArray<PathOwner>(ref this.m_PathOwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<AnimalCurrentLane> nativeArray8 = chunk.GetNativeArray<AnimalCurrentLane>(ref this.m_AnimalCurrentLaneType);
        // ISSUE: reference to a compiler-generated field
        bool flag1 = chunk.Has<Car>(ref this.m_CarType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Blocker blocker = nativeArray2[index];
          if (blocker.m_Blocker != Entity.Null && blocker.m_MaxSpeed < (byte) 6)
          {
            Entity entity1 = nativeArray1[index];
            Entity entity2 = Entity.Null;
            bool flag2;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ParkedTrainData.HasComponent(blocker.m_Blocker) || !flag1 && this.m_ParkedCarData.HasComponent(blocker.m_Blocker))
            {
              flag2 = true;
            }
            else
            {
              if (nativeArray4.Length != 0)
                entity2 = nativeArray4[index].m_Vehicle;
              else if (nativeArray5.Length != 0)
              {
                Dispatched componentData;
                // ISSUE: reference to a compiler-generated field
                if (this.m_DispatchedData.TryGetComponent(nativeArray5[index].m_RideRequest, out componentData))
                  entity2 = componentData.m_Handler;
              }
              else
              {
                CurrentVehicle componentData;
                // ISSUE: reference to a compiler-generated field
                if (nativeArray3.Length != 0 && this.m_CurrentVehicleData.TryGetComponent(nativeArray3[index].m_Leader, out componentData))
                  entity2 = componentData.m_Vehicle;
              }
              if (nativeArray6.Length != 0 && entity2 == Entity.Null)
                entity2 = nativeArray6[index].m_Target;
              if (entity2 != Entity.Null)
              {
                Controller componentData;
                // ISSUE: reference to a compiler-generated field
                if (this.m_ControllerData.TryGetComponent(entity2, out componentData))
                  entity2 = componentData.m_Controller;
                // ISSUE: reference to a compiler-generated method
                flag2 = this.IsBlocked(entity1, entity2, blocker);
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                flag2 = this.IsBlocked(entity1, blocker);
              }
            }
            if (flag2)
            {
              if (nativeArray7.Length != 0)
              {
                PathOwner pathOwner = nativeArray7[index];
                if ((pathOwner.m_State & PathFlags.Pending) == (PathFlags) 0)
                {
                  pathOwner.m_State |= PathFlags.Stuck;
                  nativeArray7[index] = pathOwner;
                }
              }
              else if (nativeArray8.Length != 0)
              {
                AnimalCurrentLane animalCurrentLane = nativeArray8[index];
                animalCurrentLane.m_Flags |= CreatureLaneFlags.Stuck;
                nativeArray8[index] = animalCurrentLane;
              }
            }
          }
        }
      }

      private bool IsBlocked(Entity entity, Blocker blocker)
      {
        int num = 0;
        Controller componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ControllerData.TryGetComponent(blocker.m_Blocker, out componentData1))
          blocker.m_Blocker = componentData1.m_Controller;
        Blocker componentData2;
        // ISSUE: reference to a compiler-generated field
        while (this.m_BlockerData.TryGetComponent(blocker.m_Blocker, out componentData2))
        {
          if (++num == 100 || blocker.m_Blocker == entity)
            return true;
          blocker = componentData2;
          if (blocker.m_Blocker == Entity.Null || blocker.m_MaxSpeed >= (byte) 6)
            return false;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ControllerData.TryGetComponent(blocker.m_Blocker, out componentData1))
            blocker.m_Blocker = componentData1.m_Controller;
        }
        return false;
      }

      private bool IsBlocked(Entity entity1, Entity entity2, Blocker blocker)
      {
        int num = 0;
        Controller componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ControllerData.TryGetComponent(blocker.m_Blocker, out componentData1))
          blocker.m_Blocker = componentData1.m_Controller;
        Blocker componentData2;
        // ISSUE: reference to a compiler-generated field
        while (this.m_BlockerData.TryGetComponent(blocker.m_Blocker, out componentData2))
        {
          if (++num == 100 || blocker.m_Blocker == entity1 || blocker.m_Blocker == entity2)
            return true;
          blocker = componentData2;
          if (blocker.m_Blocker == Entity.Null || blocker.m_MaxSpeed >= (byte) 6)
            return false;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ControllerData.TryGetComponent(blocker.m_Blocker, out componentData1))
            blocker.m_Blocker = componentData1.m_Controller;
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
      public ComponentTypeHandle<Blocker> __Game_Vehicles_Blocker_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<GroupMember> __Game_Creatures_GroupMember_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<RideNeeder> __Game_Creatures_RideNeeder_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Target> __Game_Common_Target_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Car> __Game_Vehicles_Car_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Blocker> __Game_Vehicles_Blocker_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Controller> __Game_Vehicles_Controller_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedTrain> __Game_Vehicles_ParkedTrain_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CurrentVehicle> __Game_Creatures_CurrentVehicle_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Dispatched> __Game_Simulation_Dispatched_RO_ComponentLookup;
      public ComponentTypeHandle<PathOwner> __Game_Pathfind_PathOwner_RW_ComponentTypeHandle;
      public ComponentTypeHandle<AnimalCurrentLane> __Game_Creatures_AnimalCurrentLane_RW_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Blocker_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Blocker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_GroupMember_RO_ComponentTypeHandle = state.GetComponentTypeHandle<GroupMember>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_RideNeeder_RO_ComponentTypeHandle = state.GetComponentTypeHandle<RideNeeder>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Target>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Car_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Car>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Blocker_RO_ComponentLookup = state.GetComponentLookup<Blocker>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentLookup = state.GetComponentLookup<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedTrain_RO_ComponentLookup = state.GetComponentLookup<ParkedTrain>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_CurrentVehicle_RO_ComponentLookup = state.GetComponentLookup<CurrentVehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_Dispatched_RO_ComponentLookup = state.GetComponentLookup<Dispatched>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Pathfind_PathOwner_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PathOwner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_AnimalCurrentLane_RW_ComponentTypeHandle = state.GetComponentTypeHandle<AnimalCurrentLane>();
      }
    }
  }
}
