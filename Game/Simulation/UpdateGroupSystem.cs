// Decompiled with JetBrains decompiler
// Type: Game.Simulation.UpdateGroupSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Creatures;
using Game.Net;
using Game.Objects;
using Game.Prefabs;
using Game.Rendering;
using Game.Tools;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class UpdateGroupSystem : GameSystemBase
  {
    private SimulationSystem m_SimulationSystem;
    private ModificationBarrier5 m_ModificationBarrier;
    private EntityQuery m_CreatedQuery;
    private EntityQuery m_UpdatedQuery;
    private UpdateGroupSystem.UpdateGroupTypes m_UpdateGroupTypes;
    private UpdateGroupSystem.UpdateGroupSizes m_UpdateGroupSizes;
    private UpdateGroupSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier5>();
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<UpdateFrame>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Updated>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.ReadOnly<Moving>(), ComponentType.Exclude<Created>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_UpdateGroupTypes = new UpdateGroupSystem.UpdateGroupTypes((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_UpdateGroupSizes = new UpdateGroupSystem.UpdateGroupSizes(Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_UpdateGroupSizes.Dispose();
      base.OnDestroy();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_CreatedQuery.IsEmptyIgnoreFilter)
      {
        JobHandle outJobHandle;
        // ISSUE: reference to a compiler-generated field
        NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_CreatedQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_UpdateGroupTypes.Update((SystemBase) this);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_TransformFrame_RW_BufferLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_UpdateFrameData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Created_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__EntityStorageInfoLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Applied_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: variable of a compiler-generated type
        UpdateGroupSystem.UpdateGroupJob jobData = new UpdateGroupSystem.UpdateGroupJob()
        {
          m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
          m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
          m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
          m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
          m_AppliedType = this.__TypeHandle.__Game_Common_Applied_RO_ComponentTypeHandle,
          m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
          m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
          m_ControllerType = this.__TypeHandle.__Game_Vehicles_Controller_RO_ComponentTypeHandle,
          m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
          m_LayoutElementType = this.__TypeHandle.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle,
          m_InterpolatedTransformType = this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RW_ComponentTypeHandle,
          m_EntityLookup = this.__TypeHandle.__EntityStorageInfoLookup,
          m_CreatedData = this.__TypeHandle.__Game_Common_Created_RO_ComponentLookup,
          m_PrefabUpdateFrameData = this.__TypeHandle.__Game_Prefabs_UpdateFrameData_RO_ComponentLookup,
          m_TransformFrameData = this.__TypeHandle.__Game_Objects_TransformFrame_RW_BufferLookup,
          m_Chunks = archetypeChunkListAsync,
          m_UpdateGroupTypes = this.m_UpdateGroupTypes,
          m_CommandBuffer = this.m_ModificationBarrier.CreateCommandBuffer(),
          m_UpdateGroupSizes = this.m_UpdateGroupSizes
        };
        this.Dependency = jobData.Schedule<UpdateGroupSystem.UpdateGroupJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
        archetypeChunkListAsync.Dispose(this.Dependency);
        // ISSUE: reference to a compiler-generated field
        this.m_ModificationBarrier.AddJobHandleForProducer(this.Dependency);
      }
      // ISSUE: reference to a compiler-generated field
      if (this.m_UpdatedQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_TransformFrame_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_HumanNavigation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      UpdateGroupSystem.MovingObjectsUpdatedJob jobData1 = new UpdateGroupSystem.MovingObjectsUpdatedJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_HumanNavigationType = this.__TypeHandle.__Game_Creatures_HumanNavigation_RO_ComponentTypeHandle,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_InterpolatedTransformType = this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RW_ComponentTypeHandle,
        m_TransformFrameData = this.__TypeHandle.__Game_Objects_TransformFrame_RW_BufferLookup,
        m_SimulationFrame = this.m_SimulationSystem.frameIndex
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData1.ScheduleParallel<UpdateGroupSystem.MovingObjectsUpdatedJob>(this.m_UpdatedQuery, this.Dependency);
    }

    public UpdateGroupSystem.UpdateGroupSizes GetUpdateGroupSizes() => this.m_UpdateGroupSizes;

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
    public UpdateGroupSystem()
    {
    }

    public struct UpdateGroupTypes
    {
      public ComponentTypeHandle<Moving> m_MovingType;
      public ComponentTypeHandle<Stopped> m_StoppedType;
      public ComponentTypeHandle<Plant> m_PlantType;
      public ComponentTypeHandle<Building> m_BuildingType;
      public ComponentTypeHandle<Game.Net.Node> m_NodeType;
      public ComponentTypeHandle<Game.Net.Edge> m_EdgeType;
      public ComponentTypeHandle<Lane> m_LaneType;
      public ComponentTypeHandle<CompanyData> m_CompanyType;
      public ComponentTypeHandle<Household> m_HouseholdType;
      public ComponentTypeHandle<Citizen> m_CitizenType;
      public ComponentTypeHandle<HouseholdPet> m_HouseholdPetType;
      public ComponentTypeHandle<CurrentVehicle> m_CurrentVehicleType;

      public UpdateGroupTypes(SystemBase system)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_MovingType = system.GetComponentTypeHandle<Moving>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_StoppedType = system.GetComponentTypeHandle<Stopped>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_PlantType = system.GetComponentTypeHandle<Plant>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingType = system.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_NodeType = system.GetComponentTypeHandle<Game.Net.Node>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_EdgeType = system.GetComponentTypeHandle<Game.Net.Edge>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_LaneType = system.GetComponentTypeHandle<Lane>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_CompanyType = system.GetComponentTypeHandle<CompanyData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_HouseholdType = system.GetComponentTypeHandle<Household>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_CitizenType = system.GetComponentTypeHandle<Citizen>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_HouseholdPetType = system.GetComponentTypeHandle<HouseholdPet>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_CurrentVehicleType = system.GetComponentTypeHandle<CurrentVehicle>(true);
      }

      public void Update(SystemBase system)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_MovingType.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_StoppedType.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_PlantType.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingType.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_NodeType.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_EdgeType.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_LaneType.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_CompanyType.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_HouseholdType.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_CitizenType.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_HouseholdPetType.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_CurrentVehicleType.Update(system);
      }
    }

    public struct UpdateGroupSizes
    {
      private NativeArray<int> m_MovingObjectUpdateGroupSizes;
      private NativeArray<int> m_TreeUpdateGroupSizes;
      private NativeArray<int> m_BuildingUpdateGroupSizes;
      private NativeArray<int> m_NetUpdateGroupSizes;
      private NativeArray<int> m_LaneUpdateGroupSizes;
      private NativeArray<int> m_CompanyUpdateGroupSizes;
      private NativeArray<int> m_HouseholdUpdateGroupSizes;
      private NativeArray<int> m_CitizenUpdateGroupSizes;
      private NativeArray<int> m_HouseholdPetUpdateGroupSizes;

      public UpdateGroupSizes(Allocator allocator)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_MovingObjectUpdateGroupSizes = new NativeArray<int>(16, allocator);
        // ISSUE: reference to a compiler-generated field
        this.m_TreeUpdateGroupSizes = new NativeArray<int>(16, allocator);
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingUpdateGroupSizes = new NativeArray<int>(16, allocator);
        // ISSUE: reference to a compiler-generated field
        this.m_NetUpdateGroupSizes = new NativeArray<int>(16, allocator);
        // ISSUE: reference to a compiler-generated field
        this.m_LaneUpdateGroupSizes = new NativeArray<int>(16, allocator);
        // ISSUE: reference to a compiler-generated field
        this.m_CompanyUpdateGroupSizes = new NativeArray<int>(16, allocator);
        // ISSUE: reference to a compiler-generated field
        this.m_HouseholdUpdateGroupSizes = new NativeArray<int>(16, allocator);
        // ISSUE: reference to a compiler-generated field
        this.m_CitizenUpdateGroupSizes = new NativeArray<int>(16, allocator);
        // ISSUE: reference to a compiler-generated field
        this.m_HouseholdPetUpdateGroupSizes = new NativeArray<int>(16, allocator);
      }

      public void Clear()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_MovingObjectUpdateGroupSizes.Fill<int>(0);
        // ISSUE: reference to a compiler-generated field
        this.m_TreeUpdateGroupSizes.Fill<int>(0);
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingUpdateGroupSizes.Fill<int>(0);
        // ISSUE: reference to a compiler-generated field
        this.m_NetUpdateGroupSizes.Fill<int>(0);
        // ISSUE: reference to a compiler-generated field
        this.m_LaneUpdateGroupSizes.Fill<int>(0);
        // ISSUE: reference to a compiler-generated field
        this.m_CompanyUpdateGroupSizes.Fill<int>(0);
        // ISSUE: reference to a compiler-generated field
        this.m_HouseholdUpdateGroupSizes.Fill<int>(0);
        // ISSUE: reference to a compiler-generated field
        this.m_CitizenUpdateGroupSizes.Fill<int>(0);
        // ISSUE: reference to a compiler-generated field
        this.m_HouseholdPetUpdateGroupSizes.Fill<int>(0);
      }

      public void Dispose()
      {
        // ISSUE: reference to a compiler-generated field
        this.m_MovingObjectUpdateGroupSizes.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_TreeUpdateGroupSizes.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingUpdateGroupSizes.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_NetUpdateGroupSizes.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_LaneUpdateGroupSizes.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_CompanyUpdateGroupSizes.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_HouseholdUpdateGroupSizes.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_CitizenUpdateGroupSizes.Dispose();
        // ISSUE: reference to a compiler-generated field
        this.m_HouseholdPetUpdateGroupSizes.Dispose();
      }

      public NativeArray<int> Get(ArchetypeChunk chunk, UpdateGroupSystem.UpdateGroupTypes types)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Moving>(ref types.m_MovingType) || chunk.Has<Stopped>(ref types.m_StoppedType) || chunk.Has<CurrentVehicle>(ref types.m_CurrentVehicleType))
        {
          // ISSUE: reference to a compiler-generated field
          return this.m_MovingObjectUpdateGroupSizes;
        }
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Plant>(ref types.m_PlantType))
        {
          // ISSUE: reference to a compiler-generated field
          return this.m_TreeUpdateGroupSizes;
        }
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Building>(ref types.m_BuildingType))
        {
          // ISSUE: reference to a compiler-generated field
          return this.m_BuildingUpdateGroupSizes;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Game.Net.Node>(ref types.m_NodeType) || chunk.Has<Game.Net.Edge>(ref types.m_EdgeType))
        {
          // ISSUE: reference to a compiler-generated field
          return this.m_NetUpdateGroupSizes;
        }
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Lane>(ref types.m_LaneType))
        {
          // ISSUE: reference to a compiler-generated field
          return this.m_LaneUpdateGroupSizes;
        }
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<CompanyData>(ref types.m_CompanyType))
        {
          // ISSUE: reference to a compiler-generated field
          return this.m_CompanyUpdateGroupSizes;
        }
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Household>(ref types.m_HouseholdType))
        {
          // ISSUE: reference to a compiler-generated field
          return this.m_HouseholdUpdateGroupSizes;
        }
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Citizen>(ref types.m_CitizenType))
        {
          // ISSUE: reference to a compiler-generated field
          return this.m_CitizenUpdateGroupSizes;
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return chunk.Has<HouseholdPet>(ref types.m_HouseholdPetType) ? this.m_HouseholdPetUpdateGroupSizes : new NativeArray<int>();
      }
    }

    [BurstCompile]
    private struct UpdateGroupJob : IJob
    {
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Applied> m_AppliedType;
      [ReadOnly]
      public ComponentTypeHandle<Created> m_CreatedType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Controller> m_ControllerType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> m_LayoutElementType;
      public ComponentTypeHandle<InterpolatedTransform> m_InterpolatedTransformType;
      [ReadOnly]
      public EntityStorageInfoLookup m_EntityLookup;
      [ReadOnly]
      public ComponentLookup<Created> m_CreatedData;
      [ReadOnly]
      public ComponentLookup<UpdateFrameData> m_PrefabUpdateFrameData;
      public BufferLookup<TransformFrame> m_TransformFrameData;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public UpdateGroupSystem.UpdateGroupTypes m_UpdateGroupTypes;
      public EntityCommandBuffer m_CommandBuffer;
      public UpdateGroupSystem.UpdateGroupSizes m_UpdateGroupSizes;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Chunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<Deleted>(ref this.m_DeletedType) && !chunk.Has<Created>(ref this.m_CreatedType))
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckDeleted(chunk);
          }
        }
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Chunks.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<Created>(ref this.m_CreatedType) && !chunk.Has<Deleted>(ref this.m_DeletedType))
          {
            // ISSUE: reference to a compiler-generated method
            this.CheckCreated(chunk);
          }
        }
      }

      private NativeArray<int> GetGroupSizeArray(ArchetypeChunk chunk)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        NativeArray<int> groupSizeArray = this.m_UpdateGroupSizes.Get(chunk, this.m_UpdateGroupTypes);
        if (!groupSizeArray.IsCreated)
        {
          NativeArray<ComponentType> componentTypes = chunk.Archetype.GetComponentTypes();
          UnityEngine.Debug.Log((object) "UpdateFrame added to unsupported type");
          for (int index = 0; index < componentTypes.Length; ++index)
            UnityEngine.Debug.Log((object) string.Format("Component: {0}", (object) componentTypes[index]));
          componentTypes.Dispose();
        }
        return groupSizeArray;
      }

      private void CheckDeleted(ArchetypeChunk chunk)
      {
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Temp>(ref this.m_TempType))
          return;
        // ISSUE: reference to a compiler-generated field
        uint index = chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index;
        // ISSUE: reference to a compiler-generated method
        NativeArray<int> groupSizeArray = this.GetGroupSizeArray(chunk);
        if ((long) index >= (long) groupSizeArray.Length)
          return;
        groupSizeArray[(int) index] = groupSizeArray[(int) index] - chunk.Count;
      }

      private int GetUpdateFrame(Entity entity)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_CreatedData.HasComponent(entity) || !this.m_EntityLookup.Exists(entity))
          return -1;
        // ISSUE: reference to a compiler-generated field
        ArchetypeChunk chunk = this.m_EntityLookup[entity].Chunk;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        return !chunk.Has<UpdateFrame>(this.m_UpdateFrameType) ? -1 : (int) chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index;
      }

      private void CheckCreated(ArchetypeChunk chunk)
      {
        // ISSUE: reference to a compiler-generated method
        NativeArray<int> groupSizeArray = this.GetGroupSizeArray(chunk);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray2 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<InterpolatedTransform> nativeArray3 = chunk.GetNativeArray<InterpolatedTransform>(ref this.m_InterpolatedTransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Controller> nativeArray4 = chunk.GetNativeArray<Controller>(ref this.m_ControllerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray5 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<LayoutElement> bufferAccessor = chunk.GetBufferAccessor<LayoutElement>(ref this.m_LayoutElementType);
        if (nativeArray2.Length != 0)
        {
          for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
          {
            Entity e = nativeArray1[index1];
            int originalIndex = -1;
            if (nativeArray4.Length != 0)
            {
              Controller controller = nativeArray4[index1];
              if (controller.m_Controller != Entity.Null && controller.m_Controller != e)
              {
                // ISSUE: reference to a compiler-generated method
                originalIndex = this.GetUpdateFrame(controller.m_Controller);
                if (originalIndex == -1)
                  continue;
              }
            }
            Temp temp = nativeArray2[index1];
            if (temp.m_Original != Entity.Null)
            {
              // ISSUE: reference to a compiler-generated method
              originalIndex = this.GetUpdateFrame(temp.m_Original);
            }
            uint updateIndex;
            if (nativeArray5.Length != 0)
            {
              PrefabRef prefabRef = nativeArray5[index1];
              // ISSUE: reference to a compiler-generated method
              updateIndex = this.FindUpdateIndex(originalIndex, groupSizeArray, prefabRef);
            }
            else
            {
              // ISSUE: reference to a compiler-generated method
              updateIndex = this.FindUpdateIndex(originalIndex, groupSizeArray);
            }
            // ISSUE: reference to a compiler-generated field
            this.m_CommandBuffer.SetSharedComponent<UpdateFrame>(e, new UpdateFrame(updateIndex));
            if (bufferAccessor.Length != 0)
            {
              DynamicBuffer<LayoutElement> dynamicBuffer = bufferAccessor[index1];
              for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
              {
                Entity vehicle = dynamicBuffer[index2].m_Vehicle;
                if (vehicle != e)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CommandBuffer.SetSharedComponent<UpdateFrame>(vehicle, new UpdateFrame(updateIndex));
                }
              }
            }
          }
          if (nativeArray3.Length == 0)
            return;
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Objects.Transform> nativeArray6 = chunk.GetNativeArray<Game.Objects.Transform>(ref this.m_TransformType);
          for (int index3 = 0; index3 < nativeArray1.Length; ++index3)
          {
            Entity entity = nativeArray1[index3];
            Temp temp = nativeArray2[index3];
            Game.Objects.Transform transform = nativeArray6[index3];
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransformFrameData.HasBuffer(entity))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<TransformFrame> dynamicBuffer1 = this.m_TransformFrameData[entity];
              // ISSUE: reference to a compiler-generated field
              if (this.m_TransformFrameData.HasBuffer(temp.m_Original))
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<TransformFrame> dynamicBuffer2 = this.m_TransformFrameData[temp.m_Original];
                dynamicBuffer1.ResizeUninitialized(dynamicBuffer2.Length);
                for (int index4 = 0; index4 < dynamicBuffer1.Length; ++index4)
                  dynamicBuffer1[index4] = dynamicBuffer2[index4];
              }
              else
              {
                dynamicBuffer1.ResizeUninitialized(4);
                for (int index5 = 0; index5 < dynamicBuffer1.Length; ++index5)
                  dynamicBuffer1[index5] = new TransformFrame(transform);
              }
            }
            nativeArray3[index3] = new InterpolatedTransform(transform);
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          uint index6 = chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index;
          // ISSUE: reference to a compiler-generated field
          bool flag = chunk.Has<Applied>(ref this.m_AppliedType);
          for (int index7 = 0; index7 < nativeArray1.Length; ++index7)
          {
            Entity e = nativeArray1[index7];
            uint index8;
            if (flag)
            {
              index8 = index6;
            }
            else
            {
              int originalIndex = -1;
              if (nativeArray4.Length != 0 && !flag)
              {
                Controller controller = nativeArray4[index7];
                if (controller.m_Controller != Entity.Null && controller.m_Controller != e)
                {
                  // ISSUE: reference to a compiler-generated method
                  originalIndex = this.GetUpdateFrame(controller.m_Controller);
                  if (originalIndex == -1)
                    continue;
                }
              }
              if (nativeArray5.Length != 0)
              {
                PrefabRef prefabRef = nativeArray5[index7];
                // ISSUE: reference to a compiler-generated method
                index8 = this.FindUpdateIndex(originalIndex, groupSizeArray, prefabRef);
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                index8 = this.FindUpdateIndex(originalIndex, groupSizeArray);
              }
            }
            int num = 1;
            if (!flag)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.SetSharedComponent<UpdateFrame>(e, new UpdateFrame(index8));
              if (bufferAccessor.Length != 0)
              {
                DynamicBuffer<LayoutElement> dynamicBuffer = bufferAccessor[index7];
                for (int index9 = 0; index9 < dynamicBuffer.Length; ++index9)
                {
                  Entity vehicle = dynamicBuffer[index9].m_Vehicle;
                  if (vehicle != e)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_CommandBuffer.SetSharedComponent<UpdateFrame>(vehicle, new UpdateFrame(index8));
                    ++num;
                  }
                }
              }
            }
            if ((long) index8 < (long) groupSizeArray.Length)
              groupSizeArray[(int) index8] = groupSizeArray[(int) index8] + num;
          }
          if (nativeArray3.Length == 0)
            return;
          // ISSUE: reference to a compiler-generated field
          NativeArray<Game.Objects.Transform> nativeArray7 = chunk.GetNativeArray<Game.Objects.Transform>(ref this.m_TransformType);
          for (int index10 = 0; index10 < nativeArray1.Length; ++index10)
          {
            Entity entity = nativeArray1[index10];
            Game.Objects.Transform transform = nativeArray7[index10];
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransformFrameData.HasBuffer(entity))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<TransformFrame> dynamicBuffer = this.m_TransformFrameData[entity];
              dynamicBuffer.ResizeUninitialized(4);
              for (int index11 = 0; index11 < dynamicBuffer.Length; ++index11)
                dynamicBuffer[index11] = new TransformFrame(transform);
            }
            nativeArray3[index10] = new InterpolatedTransform(transform);
          }
        }
      }

      private uint FindUpdateIndex(NativeArray<int> groupSizes, PrefabRef prefabRef)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        return this.m_PrefabUpdateFrameData.HasComponent(prefabRef.m_Prefab) ? (uint) this.m_PrefabUpdateFrameData[prefabRef.m_Prefab].m_UpdateGroupIndex : this.FindUpdateIndex(groupSizes);
      }

      private uint FindUpdateIndex(
        int originalIndex,
        NativeArray<int> groupSizes,
        PrefabRef prefabRef)
      {
        // ISSUE: reference to a compiler-generated method
        return originalIndex != -1 ? (uint) originalIndex : this.FindUpdateIndex(groupSizes, prefabRef);
      }

      private uint FindUpdateIndex(NativeArray<int> groupSizes)
      {
        uint updateIndex = uint.MaxValue;
        int num = int.MaxValue;
        for (int index = 0; index < groupSizes.Length; ++index)
        {
          int groupSiz = groupSizes[index];
          if (groupSiz < num)
          {
            num = groupSiz;
            updateIndex = (uint) index;
          }
        }
        return updateIndex;
      }

      private uint FindUpdateIndex(int originalIndex, NativeArray<int> groupSizes)
      {
        // ISSUE: reference to a compiler-generated method
        return originalIndex != -1 ? (uint) originalIndex : this.FindUpdateIndex(groupSizes);
      }
    }

    [BurstCompile]
    private struct MovingObjectsUpdatedJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentTypeHandle<HumanNavigation> m_HumanNavigationType;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      public ComponentTypeHandle<InterpolatedTransform> m_InterpolatedTransformType;
      [NativeDisableParallelForRestriction]
      public BufferLookup<TransformFrame> m_TransformFrameData;
      [ReadOnly]
      public uint m_SimulationFrame;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Objects.Transform> nativeArray2 = chunk.GetNativeArray<Game.Objects.Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<HumanNavigation> nativeArray3 = chunk.GetNativeArray<HumanNavigation>(ref this.m_HumanNavigationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<InterpolatedTransform> nativeArray4 = chunk.GetNativeArray<InterpolatedTransform>(ref this.m_InterpolatedTransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray5 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        uint num = (this.m_SimulationFrame + this.m_SimulationFrame % 16U - chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index) / 16U % 4U;
        for (int index1 = 0; index1 < nativeArray4.Length; ++index1)
        {
          Entity entity = nativeArray1[index1];
          Game.Objects.Transform transform = nativeArray2[index1];
          Temp temp = new Temp();
          if (nativeArray5.Length != 0)
            temp = nativeArray5[index1];
          DynamicBuffer<TransformFrame> bufferData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_TransformFrameData.TryGetBuffer(entity, out bufferData1))
          {
            DynamicBuffer<TransformFrame> bufferData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_TransformFrameData.TryGetBuffer(temp.m_Original, out bufferData2))
            {
              bufferData1.ResizeUninitialized(bufferData2.Length);
              for (int index2 = 0; index2 < bufferData1.Length; ++index2)
                bufferData1[index2] = bufferData2[index2];
            }
            else
            {
              bufferData1.ResizeUninitialized(4);
              TransformFrame transformFrame1 = new TransformFrame(transform);
              HumanNavigation humanNavigation;
              if (CollectionUtils.TryGet<HumanNavigation>(nativeArray3, index1, out humanNavigation))
              {
                transformFrame1.m_Activity = humanNavigation.m_LastActivity;
                transformFrame1.m_State = humanNavigation.m_TransformState;
              }
              for (int index3 = 0; index3 < bufferData1.Length; ++index3)
              {
                TransformFrame transformFrame2 = transformFrame1 with
                {
                  m_StateTimer = (ushort) index3
                };
                int a = index3 - (int) num - 1;
                int index4 = math.select(a, a + bufferData1.Length, a < 0);
                bufferData1[index4] = transformFrame2;
              }
            }
          }
          nativeArray4[index1] = new InterpolatedTransform(transform);
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
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Created> __Game_Common_Created_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Applied> __Game_Common_Applied_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Controller> __Game_Vehicles_Controller_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<LayoutElement> __Game_Vehicles_LayoutElement_RO_BufferTypeHandle;
      public ComponentTypeHandle<InterpolatedTransform> __Game_Rendering_InterpolatedTransform_RW_ComponentTypeHandle;
      [ReadOnly]
      public EntityStorageInfoLookup __EntityStorageInfoLookup;
      [ReadOnly]
      public ComponentLookup<Created> __Game_Common_Created_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<UpdateFrameData> __Game_Prefabs_UpdateFrameData_RO_ComponentLookup;
      public BufferLookup<TransformFrame> __Game_Objects_TransformFrame_RW_BufferLookup;
      [ReadOnly]
      public ComponentTypeHandle<HumanNavigation> __Game_Creatures_HumanNavigation_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Applied_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Applied>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_Controller_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Controller>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Vehicles_LayoutElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<LayoutElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_InterpolatedTransform_RW_ComponentTypeHandle = state.GetComponentTypeHandle<InterpolatedTransform>();
        // ISSUE: reference to a compiler-generated field
        this.__EntityStorageInfoLookup = state.GetEntityStorageInfoLookup();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentLookup = state.GetComponentLookup<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UpdateFrameData_RO_ComponentLookup = state.GetComponentLookup<UpdateFrameData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_TransformFrame_RW_BufferLookup = state.GetBufferLookup<TransformFrame>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_HumanNavigation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HumanNavigation>(true);
      }
    }
  }
}
