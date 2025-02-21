// Decompiled with JetBrains decompiler
// Type: Game.Tools.ApplyObjectsSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.Common;
using Game.Creatures;
using Game.Objects;
using Game.Pathfind;
using Game.Prefabs;
using Game.Rendering;
using Game.Simulation;
using Game.Triggers;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  [CompilerGenerated]
  public class ApplyObjectsSystem : GameSystemBase
  {
    private ToolOutputBarrier m_ToolOutputBarrier;
    private ToolSystem m_ToolSystem;
    private SimulationSystem m_SimulationSystem;
    private TriggerSystem m_TriggerSystem;
    private InstanceCountSystem m_InstanceCountSystem;
    private EntityQuery m_TempQuery;
    private EntityQuery m_EconomyParameterQuery;
    private EntityArchetype m_PathTargetEventArchetype;
    private ComponentTypeSet m_AppliedTypes;
    private ComponentTypeSet m_TempAnimationTypes;
    private ApplyObjectsSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier = this.World.GetOrCreateSystemManaged<ToolOutputBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TriggerSystem = this.World.GetOrCreateSystemManaged<TriggerSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_InstanceCountSystem = this.World.GetOrCreateSystemManaged<InstanceCountSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TempQuery = this.GetEntityQuery(ComponentType.ReadOnly<Temp>(), ComponentType.ReadOnly<Object>());
      // ISSUE: reference to a compiler-generated field
      this.m_PathTargetEventArchetype = this.EntityManager.CreateArchetype(ComponentType.ReadWrite<Game.Common.Event>(), ComponentType.ReadWrite<PathTargetMoved>());
      // ISSUE: reference to a compiler-generated field
      this.m_AppliedTypes = new ComponentTypeSet(ComponentType.ReadWrite<Applied>(), ComponentType.ReadWrite<Created>(), ComponentType.ReadWrite<Updated>());
      // ISSUE: reference to a compiler-generated field
      this.m_TempAnimationTypes = new ComponentTypeSet(ComponentType.ReadWrite<Temp>(), ComponentType.ReadWrite<Animation>(), ComponentType.ReadWrite<BackSide>());
      // ISSUE: reference to a compiler-generated field
      this.m_EconomyParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<EconomyParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TempQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EconomyParameterQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_OwnedCreature_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Creature_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ApplyObjectsSystem.PatchTempReferencesJob jobData1 = new ApplyObjectsSystem.PatchTempReferencesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_AttachedType = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentTypeHandle,
        m_VehicleType = this.__TypeHandle.__Game_Vehicles_Vehicle_RO_ComponentTypeHandle,
        m_CreatureType = this.__TypeHandle.__Game_Creatures_Creature_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RW_ComponentTypeHandle,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_AttachedData = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RW_BufferLookup,
        m_OwnedVehicles = this.__TypeHandle.__Game_Vehicles_OwnedVehicle_RW_BufferLookup,
        m_OwnedCreatures = this.__TypeHandle.__Game_Creatures_OwnedCreature_RW_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      NativeQueue<TriggerAction> nativeQueue = this.m_TriggerSystem.Enabled ? this.m_TriggerSystem.CreateActionBuffer() : new NativeQueue<TriggerAction>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_MeshColor_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_LocalTransformCache_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Stopped_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Static_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Recent_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_TransportStop_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_RescueTarget_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedTrain_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_AircraftCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_WatercraftCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attached_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
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
      ApplyObjectsSystem.HandleTempEntitiesJob jobData2 = new ApplyObjectsSystem.HandleTempEntitiesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_AttachedType = this.__TypeHandle.__Game_Objects_Attached_RO_ComponentTypeHandle,
        m_CarCurrentLaneData = this.__TypeHandle.__Game_Vehicles_CarCurrentLane_RO_ComponentLookup,
        m_TrainCurrentLaneData = this.__TypeHandle.__Game_Vehicles_TrainCurrentLane_RO_ComponentLookup,
        m_WatercraftCurrentLaneData = this.__TypeHandle.__Game_Vehicles_WatercraftCurrentLane_RO_ComponentLookup,
        m_AircraftCurrentLaneData = this.__TypeHandle.__Game_Vehicles_AircraftCurrentLane_RO_ComponentLookup,
        m_ParkedCarData = this.__TypeHandle.__Game_Vehicles_ParkedCar_RO_ComponentLookup,
        m_ParkedTrainData = this.__TypeHandle.__Game_Vehicles_ParkedTrain_RO_ComponentLookup,
        m_ControllerData = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentLookup,
        m_HumanCurrentLaneData = this.__TypeHandle.__Game_Creatures_HumanCurrentLane_RO_ComponentLookup,
        m_AnimalCurrentLaneData = this.__TypeHandle.__Game_Creatures_AnimalCurrentLane_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_ServiceUpgradeData = this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup,
        m_RescueTargetData = this.__TypeHandle.__Game_Buildings_RescueTarget_RO_ComponentLookup,
        m_TransportStopData = this.__TypeHandle.__Game_Routes_TransportStop_RO_ComponentLookup,
        m_HiddenData = this.__TypeHandle.__Game_Tools_Hidden_RO_ComponentLookup,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_RecentData = this.__TypeHandle.__Game_Tools_Recent_RO_ComponentLookup,
        m_ElevationData = this.__TypeHandle.__Game_Objects_Elevation_RO_ComponentLookup,
        m_TransformData = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_DestroyedData = this.__TypeHandle.__Game_Common_Destroyed_RO_ComponentLookup,
        m_DamagedData = this.__TypeHandle.__Game_Objects_Damaged_RO_ComponentLookup,
        m_StaticData = this.__TypeHandle.__Game_Objects_Static_RO_ComponentLookup,
        m_StoppedData = this.__TypeHandle.__Game_Objects_Stopped_RO_ComponentLookup,
        m_LocalTransformCacheData = this.__TypeHandle.__Game_Tools_LocalTransformCache_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_SubNets = this.__TypeHandle.__Game_Net_SubNet_RO_BufferLookup,
        m_SubLanes = this.__TypeHandle.__Game_Net_SubLane_RO_BufferLookup,
        m_SubAreas = this.__TypeHandle.__Game_Areas_SubArea_RO_BufferLookup,
        m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferLookup,
        m_MeshColors = this.__TypeHandle.__Game_Rendering_MeshColor_RO_BufferLookup,
        m_SubObjects = this.__TypeHandle.__Game_Objects_SubObject_RO_BufferLookup,
        m_SimulationFrame = this.m_SimulationSystem.frameIndex,
        m_EconomyParameterData = this.m_EconomyParameterQuery.GetSingleton<EconomyParameterData>(),
        m_EditorMode = this.m_ToolSystem.actionMode.IsEditor(),
        m_PathTargetEventArchetype = this.m_PathTargetEventArchetype,
        m_AppliedTypes = this.m_AppliedTypes,
        m_TempAnimationTypes = this.m_TempAnimationTypes,
        m_InstanceCounts = this.m_InstanceCountSystem.GetInstanceCounts(true, out dependencies),
        m_CommandBuffer = this.m_ToolOutputBarrier.CreateCommandBuffer().AsParallelWriter(),
        m_TriggerBuffer = nativeQueue.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      JobHandle job0 = jobData1.Schedule<ApplyObjectsSystem.PatchTempReferencesJob>(this.m_TempQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      EntityQuery tempQuery = this.m_TempQuery;
      JobHandle dependsOn = JobHandle.CombineDependencies(job0, dependencies);
      JobHandle jobHandle = jobData2.ScheduleParallel<ApplyObjectsSystem.HandleTempEntitiesJob>(tempQuery, dependsOn);
      // ISSUE: reference to a compiler-generated field
      this.m_ToolOutputBarrier.AddJobHandleForProducer(jobHandle);
      // ISSUE: reference to a compiler-generated field
      if (this.m_TriggerSystem.Enabled)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_TriggerSystem.AddActionBufferWriter(jobHandle);
      }
      else
        nativeQueue.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InstanceCountSystem.AddCountReader(jobHandle);
      this.Dependency = jobHandle;
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
    public ApplyObjectsSystem()
    {
    }

    [BurstCompile]
    private struct PatchTempReferencesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Attached> m_AttachedType;
      [ReadOnly]
      public ComponentTypeHandle<Vehicle> m_VehicleType;
      [ReadOnly]
      public ComponentTypeHandle<Creature> m_CreatureType;
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Attached> m_AttachedData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      public BufferLookup<OwnedVehicle> m_OwnedVehicles;
      public BufferLookup<OwnedCreature> m_OwnedCreatures;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray3 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Attached> nativeArray4 = chunk.GetNativeArray<Attached>(ref this.m_AttachedType);
        for (int index = 0; index < nativeArray4.Length; ++index)
        {
          Entity subObject = nativeArray1[index];
          Attached attached = nativeArray4[index];
          Temp temp = nativeArray2[index];
          if ((temp.m_Flags & (TempFlags.Delete | TempFlags.Cancel)) == (TempFlags) 0)
          {
            Entity entity = attached.m_Parent;
            bool flag = false;
            Temp componentData1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TempData.TryGetComponent(attached.m_Parent, out componentData1))
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabRefData.HasComponent(componentData1.m_Original) && (componentData1.m_Flags & (TempFlags.Replace | TempFlags.Combine)) == (TempFlags) 0)
              {
                entity = componentData1.m_Original;
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                flag |= this.m_PrefabRefData.HasComponent(temp.m_Original);
              }
            }
            Attached componentData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_AttachedData.TryGetComponent(temp.m_Original, out componentData2))
            {
              DynamicBuffer<Game.Objects.SubObject> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (componentData2.m_Parent != entity && this.m_SubObjects.TryGetBuffer(componentData2.m_Parent, out bufferData))
                CollectionUtils.RemoveValue<Game.Objects.SubObject>(bufferData, new Game.Objects.SubObject(temp.m_Original));
            }
            else
              flag |= attached.m_Parent != entity;
            DynamicBuffer<Game.Objects.SubObject> bufferData1;
            // ISSUE: reference to a compiler-generated field
            if (flag && this.m_SubObjects.TryGetBuffer(attached.m_Parent, out bufferData1))
              CollectionUtils.RemoveValue<Game.Objects.SubObject>(bufferData1, new Game.Objects.SubObject(subObject));
          }
        }
        // ISSUE: reference to a compiler-generated field
        bool flag1 = chunk.Has<Vehicle>(ref this.m_VehicleType);
        // ISSUE: reference to a compiler-generated field
        bool flag2 = chunk.Has<Creature>(ref this.m_CreatureType);
        for (int index = 0; index < nativeArray3.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Owner owner1 = nativeArray3[index];
          Temp temp1 = nativeArray2[index];
          if (temp1.m_Original == Entity.Null && (temp1.m_Flags & TempFlags.Delete) == (TempFlags) 0)
          {
            Owner owner2 = owner1;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TempData.HasComponent(owner1.m_Owner))
            {
              // ISSUE: reference to a compiler-generated field
              Temp temp2 = this.m_TempData[owner1.m_Owner];
              if (temp2.m_Original != Entity.Null && (temp2.m_Flags & TempFlags.Replace) == (TempFlags) 0)
                owner2.m_Owner = temp2.m_Original;
            }
            if (owner2.m_Owner != owner1.m_Owner)
            {
              if (flag1)
              {
                DynamicBuffer<OwnedVehicle> bufferData;
                // ISSUE: reference to a compiler-generated field
                if (this.m_OwnedVehicles.TryGetBuffer(owner1.m_Owner, out bufferData))
                  CollectionUtils.RemoveValue<OwnedVehicle>(bufferData, new OwnedVehicle(entity));
                // ISSUE: reference to a compiler-generated field
                if (this.m_OwnedVehicles.TryGetBuffer(owner2.m_Owner, out bufferData))
                  CollectionUtils.TryAddUniqueValue<OwnedVehicle>(bufferData, new OwnedVehicle(entity));
              }
              else if (flag2)
              {
                DynamicBuffer<OwnedCreature> bufferData;
                // ISSUE: reference to a compiler-generated field
                if (this.m_OwnedCreatures.TryGetBuffer(owner1.m_Owner, out bufferData))
                  CollectionUtils.RemoveValue<OwnedCreature>(bufferData, new OwnedCreature(entity));
                // ISSUE: reference to a compiler-generated field
                if (this.m_OwnedCreatures.TryGetBuffer(owner2.m_Owner, out bufferData))
                  CollectionUtils.TryAddUniqueValue<OwnedCreature>(bufferData, new OwnedCreature(entity));
              }
              else
              {
                DynamicBuffer<Game.Objects.SubObject> bufferData;
                // ISSUE: reference to a compiler-generated field
                if (this.m_SubObjects.TryGetBuffer(owner1.m_Owner, out bufferData))
                  CollectionUtils.RemoveValue<Game.Objects.SubObject>(bufferData, new Game.Objects.SubObject(entity));
                // ISSUE: reference to a compiler-generated field
                if (this.m_SubObjects.TryGetBuffer(owner2.m_Owner, out bufferData))
                  CollectionUtils.TryAddUniqueValue<Game.Objects.SubObject>(bufferData, new Game.Objects.SubObject(entity));
              }
              nativeArray3[index] = owner2;
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
    private struct HandleTempEntitiesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Attached> m_AttachedType;
      [ReadOnly]
      public ComponentLookup<CarCurrentLane> m_CarCurrentLaneData;
      [ReadOnly]
      public ComponentLookup<TrainCurrentLane> m_TrainCurrentLaneData;
      [ReadOnly]
      public ComponentLookup<WatercraftCurrentLane> m_WatercraftCurrentLaneData;
      [ReadOnly]
      public ComponentLookup<AircraftCurrentLane> m_AircraftCurrentLaneData;
      [ReadOnly]
      public ComponentLookup<ParkedCar> m_ParkedCarData;
      [ReadOnly]
      public ComponentLookup<ParkedTrain> m_ParkedTrainData;
      [ReadOnly]
      public ComponentLookup<Controller> m_ControllerData;
      [ReadOnly]
      public ComponentLookup<HumanCurrentLane> m_HumanCurrentLaneData;
      [ReadOnly]
      public ComponentLookup<AnimalCurrentLane> m_AnimalCurrentLaneData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.ServiceUpgrade> m_ServiceUpgradeData;
      [ReadOnly]
      public ComponentLookup<RescueTarget> m_RescueTargetData;
      [ReadOnly]
      public ComponentLookup<Game.Routes.TransportStop> m_TransportStopData;
      [ReadOnly]
      public ComponentLookup<Hidden> m_HiddenData;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Recent> m_RecentData;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> m_ElevationData;
      [ReadOnly]
      public ComponentLookup<Transform> m_TransformData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Destroyed> m_DestroyedData;
      [ReadOnly]
      public ComponentLookup<Damaged> m_DamagedData;
      [ReadOnly]
      public ComponentLookup<Static> m_StaticData;
      [ReadOnly]
      public ComponentLookup<Stopped> m_StoppedData;
      [ReadOnly]
      public ComponentLookup<LocalTransformCache> m_LocalTransformCacheData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> m_SubNets;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> m_SubLanes;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> m_SubAreas;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;
      [ReadOnly]
      public BufferLookup<MeshColor> m_MeshColors;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> m_SubObjects;
      [ReadOnly]
      public uint m_SimulationFrame;
      [ReadOnly]
      public EconomyParameterData m_EconomyParameterData;
      [ReadOnly]
      public bool m_EditorMode;
      [ReadOnly]
      public EntityArchetype m_PathTargetEventArchetype;
      [ReadOnly]
      public ComponentTypeSet m_AppliedTypes;
      [ReadOnly]
      public ComponentTypeSet m_TempAnimationTypes;
      [ReadOnly]
      public NativeParallelHashMap<Entity, int> m_InstanceCounts;
      public EntityCommandBuffer.ParallelWriter m_CommandBuffer;
      public NativeQueue<TriggerAction>.ParallelWriter m_TriggerBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray3 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Attached> nativeArray4 = chunk.GetNativeArray<Attached>(ref this.m_AttachedType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Entity entity = nativeArray1[index];
          Temp temp1 = nativeArray2[index];
          if ((temp1.m_Flags & TempFlags.Cancel) != (TempFlags) 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.Cancel(unfilteredChunkIndex, entity, temp1);
          }
          else if ((temp1.m_Flags & TempFlags.Delete) != (TempFlags) 0)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (this.m_ParkedCarData.HasComponent(entity) && !this.m_ParkedCarData.HasComponent(temp1.m_Original))
            {
              // ISSUE: reference to a compiler-generated method
              this.Cancel(unfilteredChunkIndex, entity, temp1);
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              this.Delete(unfilteredChunkIndex, entity, temp1);
            }
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_PrefabRefData.HasComponent(temp1.m_Original))
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_ParkedCarData.HasComponent(entity) || this.m_ParkedTrainData.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (!this.m_ParkedCarData.HasComponent(temp1.m_Original) && !this.m_ParkedTrainData.HasComponent(temp1.m_Original))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.Cancel(unfilteredChunkIndex, entity, temp1);
                  continue;
                }
                // ISSUE: reference to a compiler-generated method
                this.FixParkingLocation(unfilteredChunkIndex, temp1.m_Original);
              }
              if (nativeArray4.Length != 0)
              {
                Attached data = nativeArray4[index];
                // ISSUE: reference to a compiler-generated field
                if (this.m_TempData.HasComponent(data.m_Parent))
                {
                  // ISSUE: reference to a compiler-generated field
                  Temp temp2 = this.m_TempData[data.m_Parent];
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PrefabRefData.HasComponent(temp2.m_Original) && (temp2.m_Flags & (TempFlags.Replace | TempFlags.Combine)) == (TempFlags) 0)
                    data.m_Parent = temp2.m_Original;
                }
                // ISSUE: reference to a compiler-generated method
                this.CopyToOriginal<Attached>(unfilteredChunkIndex, temp1, data);
              }
              if ((temp1.m_Flags & TempFlags.Upgrade) != (TempFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.UpdateComponent<PrefabRef>(unfilteredChunkIndex, entity, temp1.m_Original, this.m_PrefabRefData, true);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.UpdateComponent<Destroyed>(unfilteredChunkIndex, entity, temp1.m_Original, this.m_DestroyedData, false);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.UpdateComponent<Damaged>(unfilteredChunkIndex, entity, temp1.m_Original, this.m_DamagedData, false);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                this.UpdateBuffer<MeshColor>(unfilteredChunkIndex, entity, temp1.m_Original, this.m_MeshColors, out DynamicBuffer<MeshColor> _, false);
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (!this.m_DestroyedData.HasComponent(entity) && this.m_RescueTargetData.HasComponent(temp1.m_Original))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.RemoveComponent<RescueTarget>(unfilteredChunkIndex, temp1.m_Original);
                }
              }
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.UpdateComponent<Game.Objects.Elevation>(unfilteredChunkIndex, entity, temp1.m_Original, this.m_ElevationData, true);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.UpdateComponent<LocalTransformCache>(unfilteredChunkIndex, entity, temp1.m_Original, this.m_LocalTransformCacheData, true);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.UpdateBuffer<Game.Net.SubNet>(unfilteredChunkIndex, entity, temp1.m_Original, this.m_SubNets, out DynamicBuffer<Game.Net.SubNet> _, false);
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.UpdateBuffer<Game.Areas.SubArea>(unfilteredChunkIndex, entity, temp1.m_Original, this.m_SubAreas, out DynamicBuffer<Game.Areas.SubArea> _, false);
              DynamicBuffer<Game.Net.SubLane> oldBuffer1;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              if (this.UpdateBuffer<Game.Net.SubLane>(unfilteredChunkIndex, entity, temp1.m_Original, this.m_SubLanes, out oldBuffer1, false))
              {
                // ISSUE: reference to a compiler-generated method
                this.RemoveOldSubItems(unfilteredChunkIndex, temp1.m_Original, oldBuffer1);
              }
              DynamicBuffer<Game.Objects.SubObject> oldBuffer2;
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              if (this.UpdateBuffer<Game.Objects.SubObject>(unfilteredChunkIndex, entity, temp1.m_Original, this.m_SubObjects, out oldBuffer2, false))
              {
                // ISSUE: reference to a compiler-generated method
                this.RemoveOldSubItems(unfilteredChunkIndex, temp1.m_Original, oldBuffer2);
              }
              // ISSUE: reference to a compiler-generated method
              this.Update(unfilteredChunkIndex, entity, temp1, nativeArray3[index]);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              if (this.m_ParkedCarData.HasComponent(entity) || this.m_ParkedTrainData.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated method
                this.FixParkingLocation(unfilteredChunkIndex, entity);
              }
              if (nativeArray4.Length != 0)
              {
                Attached component = nativeArray4[index];
                // ISSUE: reference to a compiler-generated field
                if (this.m_TempData.HasComponent(component.m_Parent))
                {
                  // ISSUE: reference to a compiler-generated field
                  Temp temp3 = this.m_TempData[component.m_Parent];
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_PrefabRefData.HasComponent(temp3.m_Original) && (temp3.m_Flags & (TempFlags.Replace | TempFlags.Combine)) == (TempFlags) 0)
                    component.m_Parent = temp3.m_Original;
                }
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<Attached>(unfilteredChunkIndex, entity, component);
              }
              // ISSUE: reference to a compiler-generated method
              this.Create(unfilteredChunkIndex, entity, temp1);
              // ISSUE: reference to a compiler-generated field
              if (this.m_PrefabRefData.HasComponent(entity))
              {
                // ISSUE: reference to a compiler-generated field
                Entity prefab = this.m_PrefabRefData[entity].m_Prefab;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                int instanceCount = this.m_InstanceCounts.ContainsKey(prefab) ? this.m_InstanceCounts[prefab] : 0;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                this.m_TriggerBuffer.Enqueue(new TriggerAction(TriggerType.ObjectCreated, this.m_PrefabRefData[entity].m_Prefab, entity, entity, (float) instanceCount));
              }
            }
          }
        }
      }

      private void FixParkingLocation(int chunkIndex, Entity entity)
      {
        Controller componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_ControllerData.TryGetComponent(entity, out componentData) && componentData.m_Controller != entity)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<FixParkingLocation>(chunkIndex, entity, new FixParkingLocation()
        {
          m_ResetLocation = entity
        });
      }

      private void RemoveOldSubItems(int chunkIndex, Entity original, DynamicBuffer<Game.Net.SubLane> items)
      {
        for (int index = 0; index < items.Length; ++index)
        {
          Entity subLane = items[index].m_SubLane;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, subLane, new Deleted());
        }
      }

      private void RemoveOldSubItems(
        int chunkIndex,
        Entity original,
        DynamicBuffer<Game.Objects.SubObject> items)
      {
        for (int index = 0; index < items.Length; ++index)
        {
          Entity subObject = items[index].m_SubObject;
          Owner componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_OwnerData.TryGetComponent(subObject, out componentData) && componentData.m_Owner == original)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, subObject, new Deleted());
          }
        }
      }

      private void Cancel(int chunkIndex, Entity entity, Temp temp)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_HiddenData.HasComponent(temp.m_Original))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<BatchesUpdated>(chunkIndex, temp.m_Original, new BatchesUpdated());
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Hidden>(chunkIndex, temp.m_Original);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, entity, new Deleted());
      }

      private void Delete(int chunkIndex, Entity entity, Temp temp)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_PrefabRefData.HasComponent(temp.m_Original))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, temp.m_Original, new Deleted());
          // ISSUE: reference to a compiler-generated field
          if (this.m_SubNets.HasBuffer(temp.m_Original))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<Game.Net.SubNet> subNet1 = this.m_SubNets[temp.m_Original];
            for (int index = 0; index < subNet1.Length; ++index)
            {
              Entity subNet2 = subNet1[index].m_SubNet;
              // ISSUE: reference to a compiler-generated field
              if (!this.m_HiddenData.HasComponent(subNet2))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.RemoveComponent<Owner>(chunkIndex, subNet2);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<Updated>(chunkIndex, subNet2, new Updated());
              }
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, entity, new Deleted());
      }

      private void UpdateComponent<T>(
        int chunkIndex,
        Entity entity,
        Entity original,
        ComponentLookup<T> data,
        bool updateValue)
        where T : unmanaged, IComponentData
      {
        if (data.HasComponent(entity))
        {
          if (data.HasComponent(original))
          {
            if (!updateValue)
              return;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<T>(chunkIndex, original, data[entity]);
          }
          else if (updateValue)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<T>(chunkIndex, original, data[entity]);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<T>(chunkIndex, original, default (T));
          }
        }
        else
        {
          if (!data.HasComponent(original))
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<T>(chunkIndex, original);
        }
      }

      private bool UpdateBuffer<T>(
        int chunkIndex,
        Entity entity,
        Entity original,
        BufferLookup<T> data,
        out DynamicBuffer<T> oldBuffer,
        bool updateValue)
        where T : unmanaged, IBufferElementData
      {
        if (data.HasBuffer(entity))
        {
          if (data.HasBuffer(original))
          {
            if (updateValue)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetBuffer<T>(chunkIndex, original).CopyFrom(data[entity]);
            }
          }
          else if (updateValue)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddBuffer<T>(chunkIndex, original).CopyFrom(data[entity]);
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddBuffer<T>(chunkIndex, original);
          }
        }
        else if (data.TryGetBuffer(original, out oldBuffer))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<T>(chunkIndex, original);
          return true;
        }
        oldBuffer = new DynamicBuffer<T>();
        return false;
      }

      private void Update(int chunkIndex, Entity entity, Temp temp)
      {
        // ISSUE: reference to a compiler-generated field
        if (this.m_HiddenData.HasComponent(temp.m_Original))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<Hidden>(chunkIndex, temp.m_Original);
        }
        if (temp.m_Cost != 0)
        {
          // ISSUE: reference to a compiler-generated field
          Recent component = new Recent()
          {
            m_ModificationFrame = this.m_SimulationFrame,
            m_ModificationCost = temp.m_Cost
          };
          Recent componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_RecentData.TryGetComponent(temp.m_Original, out componentData))
          {
            component.m_ModificationCost += componentData.m_ModificationCost;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            component.m_ModificationCost += ObjectUtils.GetRefundAmount(componentData, this.m_SimulationFrame, this.m_EconomyParameterData);
            // ISSUE: reference to a compiler-generated field
            componentData.m_ModificationFrame = this.m_SimulationFrame;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            component.m_ModificationCost -= ObjectUtils.GetRefundAmount(componentData, this.m_SimulationFrame, this.m_EconomyParameterData);
            component.m_ModificationCost = math.min(component.m_ModificationCost, temp.m_Value);
            if (component.m_ModificationCost > 0)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Recent>(chunkIndex, temp.m_Original, component);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.RemoveComponent<Recent>(chunkIndex, temp.m_Original);
            }
          }
          else if (component.m_ModificationCost > 0)
          {
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.AddComponent<Recent>(chunkIndex, temp.m_Original, component);
          }
        }
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Updated>(chunkIndex, temp.m_Original, new Updated());
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<Deleted>(chunkIndex, entity, new Deleted());
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (!this.m_EditorMode || !this.ShouldSaveInstance(entity, temp.m_Original))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent<SaveInstance>(chunkIndex, temp.m_Original, new SaveInstance());
      }

      private bool ShouldSaveInstance(Entity temp, Entity original)
      {
        DynamicBuffer<InstalledUpgrade> bufferData;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return this.m_OwnerData.HasComponent(original) ? this.m_ServiceUpgradeData.HasComponent(original) : !this.m_InstalledUpgrades.TryGetBuffer(original, out bufferData) || bufferData.Length == 0 && (!this.m_InstalledUpgrades.TryGetBuffer(temp, out bufferData) || bufferData.Length == 0);
      }

      private void Update(int chunkIndex, Entity entity, Temp temp, Transform transform)
      {
        // ISSUE: reference to a compiler-generated field
        Transform transform1 = this.m_TransformData[temp.m_Original];
        if (!transform1.Equals(transform))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.SetComponent<Transform>(chunkIndex, temp.m_Original, transform);
          // ISSUE: reference to a compiler-generated field
          if (this.m_CarCurrentLaneData.HasComponent(temp.m_Original))
          {
            // ISSUE: reference to a compiler-generated field
            CarCurrentLane component = this.m_CarCurrentLaneData[temp.m_Original];
            component.m_LaneFlags |= Game.Vehicles.CarLaneFlags.Obsolete;
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<CarCurrentLane>(chunkIndex, temp.m_Original, component);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<Moving>(chunkIndex, temp.m_Original, new Moving());
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.m_TrainCurrentLaneData.HasComponent(temp.m_Original))
            {
              // ISSUE: reference to a compiler-generated field
              TrainCurrentLane component = this.m_TrainCurrentLaneData[temp.m_Original];
              component.m_Front.m_LaneFlags |= TrainLaneFlags.Obsolete;
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<TrainCurrentLane>(chunkIndex, temp.m_Original, component);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetComponent<Moving>(chunkIndex, temp.m_Original, new Moving());
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_WatercraftCurrentLaneData.HasComponent(temp.m_Original))
              {
                // ISSUE: reference to a compiler-generated field
                WatercraftCurrentLane component = this.m_WatercraftCurrentLaneData[temp.m_Original];
                component.m_LaneFlags |= WatercraftLaneFlags.Obsolete;
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<WatercraftCurrentLane>(chunkIndex, temp.m_Original, component);
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.SetComponent<Moving>(chunkIndex, temp.m_Original, new Moving());
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                if (this.m_AircraftCurrentLaneData.HasComponent(temp.m_Original))
                {
                  // ISSUE: reference to a compiler-generated field
                  AircraftCurrentLane component = this.m_AircraftCurrentLaneData[temp.m_Original];
                  component.m_LaneFlags |= AircraftLaneFlags.Obsolete;
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponent<AircraftCurrentLane>(chunkIndex, temp.m_Original, component);
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetComponent<Moving>(chunkIndex, temp.m_Original, new Moving());
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_HumanCurrentLaneData.HasComponent(temp.m_Original))
                  {
                    // ISSUE: reference to a compiler-generated field
                    HumanCurrentLane component = this.m_HumanCurrentLaneData[temp.m_Original];
                    component.m_Flags |= CreatureLaneFlags.Obsolete;
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.SetComponent<HumanCurrentLane>(chunkIndex, temp.m_Original, component);
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.SetComponent<Moving>(chunkIndex, temp.m_Original, new Moving());
                  }
                  else
                  {
                    // ISSUE: reference to a compiler-generated field
                    if (this.m_AnimalCurrentLaneData.HasComponent(temp.m_Original))
                    {
                      // ISSUE: reference to a compiler-generated field
                      AnimalCurrentLane component = this.m_AnimalCurrentLaneData[temp.m_Original];
                      component.m_Flags |= CreatureLaneFlags.Obsolete;
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.SetComponent<AnimalCurrentLane>(chunkIndex, temp.m_Original, component);
                      // ISSUE: reference to a compiler-generated field
                      this.m_CommandBuffer.SetComponent<Moving>(chunkIndex, temp.m_Original, new Moving());
                    }
                  }
                }
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_BuildingData.HasComponent(temp.m_Original) || this.m_TransportStopData.HasComponent(temp.m_Original))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            Entity entity1 = this.m_CommandBuffer.CreateEntity(chunkIndex, this.m_PathTargetEventArchetype);
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetComponent<PathTargetMoved>(chunkIndex, entity1, new PathTargetMoved(temp.m_Original, transform1.m_Position, transform.m_Position));
          }
        }
        // ISSUE: reference to a compiler-generated method
        this.Update(chunkIndex, entity, temp);
      }

      private void CopyToOriginal<T>(int chunkIndex, Temp temp, T data) where T : unmanaged, IComponentData
      {
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.SetComponent<T>(chunkIndex, temp.m_Original, data);
      }

      private void Create(int chunkIndex, Entity entity, Temp temp)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.RemoveComponent(chunkIndex, entity, in this.m_TempAnimationTypes);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_StaticData.HasComponent(entity) || this.m_StoppedData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.RemoveComponent<InterpolatedTransform>(chunkIndex, entity);
        }
        if (temp.m_Cost > 0)
        {
          // ISSUE: reference to a compiler-generated field
          Recent component = new Recent()
          {
            m_ModificationFrame = this.m_SimulationFrame,
            m_ModificationCost = temp.m_Cost
          };
          // ISSUE: reference to a compiler-generated field
          this.m_CommandBuffer.AddComponent<Recent>(chunkIndex, entity, component);
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_CommandBuffer.AddComponent(chunkIndex, entity, in this.m_AppliedTypes);
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
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Attached> __Game_Objects_Attached_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Vehicle> __Game_Vehicles_Vehicle_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Creature> __Game_Creatures_Creature_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attached> __Game_Objects_Attached_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RW_BufferLookup;
      public BufferLookup<OwnedVehicle> __Game_Vehicles_OwnedVehicle_RW_BufferLookup;
      public BufferLookup<OwnedCreature> __Game_Creatures_OwnedCreature_RW_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<CarCurrentLane> __Game_Vehicles_CarCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<TrainCurrentLane> __Game_Vehicles_TrainCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<WatercraftCurrentLane> __Game_Vehicles_WatercraftCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AircraftCurrentLane> __Game_Vehicles_AircraftCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedCar> __Game_Vehicles_ParkedCar_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ParkedTrain> __Game_Vehicles_ParkedTrain_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Controller> __Game_Vehicles_Controller_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<HumanCurrentLane> __Game_Creatures_HumanCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<AnimalCurrentLane> __Game_Creatures_AnimalCurrentLane_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.ServiceUpgrade> __Game_Buildings_ServiceUpgrade_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RescueTarget> __Game_Buildings_RescueTarget_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Routes.TransportStop> __Game_Routes_TransportStop_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Hidden> __Game_Tools_Hidden_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Recent> __Game_Tools_Recent_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Objects.Elevation> __Game_Objects_Elevation_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Transform> __Game_Objects_Transform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Destroyed> __Game_Common_Destroyed_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Damaged> __Game_Objects_Damaged_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Static> __Game_Objects_Static_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Stopped> __Game_Objects_Stopped_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<LocalTransformCache> __Game_Tools_LocalTransformCache_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubNet> __Game_Net_SubNet_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Net.SubLane> __Game_Net_SubLane_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Areas.SubArea> __Game_Areas_SubArea_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<MeshColor> __Game_Rendering_MeshColor_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<Game.Objects.SubObject> __Game_Objects_SubObject_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Vehicle_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Vehicle>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Creature_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Creature>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attached_RO_ComponentLookup = state.GetComponentLookup<Attached>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RW_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_OwnedVehicle_RW_BufferLookup = state.GetBufferLookup<OwnedVehicle>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_OwnedCreature_RW_BufferLookup = state.GetBufferLookup<OwnedCreature>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_CarCurrentLane_RO_ComponentLookup = state.GetComponentLookup<CarCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_TrainCurrentLane_RO_ComponentLookup = state.GetComponentLookup<TrainCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_WatercraftCurrentLane_RO_ComponentLookup = state.GetComponentLookup<WatercraftCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_AircraftCurrentLane_RO_ComponentLookup = state.GetComponentLookup<AircraftCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedCar_RO_ComponentLookup = state.GetComponentLookup<ParkedCar>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_ParkedTrain_RO_ComponentLookup = state.GetComponentLookup<ParkedTrain>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentLookup = state.GetComponentLookup<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_HumanCurrentLane_RO_ComponentLookup = state.GetComponentLookup<HumanCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_AnimalCurrentLane_RO_ComponentLookup = state.GetComponentLookup<AnimalCurrentLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ServiceUpgrade_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.ServiceUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_RescueTarget_RO_ComponentLookup = state.GetComponentLookup<RescueTarget>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TransportStop_RO_ComponentLookup = state.GetComponentLookup<Game.Routes.TransportStop>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Hidden_RO_ComponentLookup = state.GetComponentLookup<Hidden>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Recent_RO_ComponentLookup = state.GetComponentLookup<Recent>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Elevation_RO_ComponentLookup = state.GetComponentLookup<Game.Objects.Elevation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentLookup = state.GetComponentLookup<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Destroyed_RO_ComponentLookup = state.GetComponentLookup<Destroyed>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Damaged_RO_ComponentLookup = state.GetComponentLookup<Damaged>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Static_RO_ComponentLookup = state.GetComponentLookup<Static>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Stopped_RO_ComponentLookup = state.GetComponentLookup<Stopped>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_LocalTransformCache_RO_ComponentLookup = state.GetComponentLookup<LocalTransformCache>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubNet_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubNet>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferLookup = state.GetBufferLookup<Game.Net.SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_SubArea_RO_BufferLookup = state.GetBufferLookup<Game.Areas.SubArea>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferLookup = state.GetBufferLookup<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_MeshColor_RO_BufferLookup = state.GetBufferLookup<MeshColor>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SubObject_RO_BufferLookup = state.GetBufferLookup<Game.Objects.SubObject>(true);
      }
    }
  }
}
