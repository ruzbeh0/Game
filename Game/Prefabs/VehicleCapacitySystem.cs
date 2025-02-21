// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.VehicleCapacitySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using Game.Common;
using Game.Serialization;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class VehicleCapacitySystem : GameSystemBase, IPostDeserialize
  {
    private CityConfigurationSystem m_CityConfigurationSystem;
    private EntityQuery m_UpdatedQuery;
    private EntityQuery m_DeliveryTruckQuery;
    private NativeList<DeliveryTruckSelectItem> m_DeliveryTruckItems;
    private JobHandle m_WriteDependency;
    private VehicleSelectRequirementData m_VehicleSelectRequirementData;
    private bool m_RequireUpdate;
    private VehicleCapacitySystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleSelectRequirementData = new VehicleSelectRequirementData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<VehicleData>(),
          ComponentType.ReadOnly<PrefabData>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_DeliveryTruckQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[4]
        {
          ComponentType.ReadOnly<DeliveryTruckData>(),
          ComponentType.ReadOnly<CarData>(),
          ComponentType.ReadOnly<ObjectData>(),
          ComponentType.ReadOnly<PrefabData>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Locked>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_DeliveryTruckItems = new NativeList<DeliveryTruckSelectItem>(10, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_DeliveryTruckItems.Dispose();
      base.OnDestroy();
    }

    public void PostDeserialize(Colossal.Serialization.Entities.Context context)
    {
      if (context.purpose != Colossal.Serialization.Entities.Purpose.NewGame && context.purpose != Colossal.Serialization.Entities.Purpose.LoadGame)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_RequireUpdate = true;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.m_RequireUpdate && this.m_UpdatedQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_RequireUpdate = false;
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_DeliveryTruckQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_VehicleSelectRequirementData.Update((SystemBase) this, this.m_CityConfigurationSystem);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarTractorData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_CarTrailerData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_DeliveryTruckData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      JobHandle inputDeps = new UpdateDeliveryTruckSelectJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_DeliveryTruckDataType = this.__TypeHandle.__Game_Prefabs_DeliveryTruckData_RO_ComponentTypeHandle,
        m_CarTrailerDataType = this.__TypeHandle.__Game_Prefabs_CarTrailerData_RO_ComponentTypeHandle,
        m_CarTractorDataType = this.__TypeHandle.__Game_Prefabs_CarTractorData_RO_ComponentTypeHandle,
        m_PrefabChunks = archetypeChunkListAsync,
        m_RequirementData = this.m_VehicleSelectRequirementData,
        m_DeliveryTruckItems = this.m_DeliveryTruckItems
      }.Schedule<UpdateDeliveryTruckSelectJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
      archetypeChunkListAsync.Dispose(inputDeps);
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependency = inputDeps;
      this.Dependency = inputDeps;
    }

    public DeliveryTruckSelectData GetDeliveryTruckSelectData()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependency.Complete();
      // ISSUE: reference to a compiler-generated field
      return new DeliveryTruckSelectData(this.m_DeliveryTruckItems.AsArray());
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
    public VehicleCapacitySystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<DeliveryTruckData> __Game_Prefabs_DeliveryTruckData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CarTrailerData> __Game_Prefabs_CarTrailerData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<CarTractorData> __Game_Prefabs_CarTractorData_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_DeliveryTruckData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<DeliveryTruckData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarTrailerData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CarTrailerData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_CarTractorData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CarTractorData>(true);
      }
    }
  }
}
