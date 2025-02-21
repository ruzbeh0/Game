// Decompiled with JetBrains decompiler
// Type: Game.Buildings.ReferencesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Areas;
using Game.Common;
using Game.Net;
using Game.Objects;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  [CompilerGenerated]
  public class ReferencesSystem : GameSystemBase
  {
    private EntityQuery m_SpawnLocationQuery;
    private ReferencesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SpawnLocationQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Owner>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<SpawnLocation>(),
          ComponentType.ReadOnly<HangaroundLocation>(),
          ComponentType.ReadOnly<ParkingLane>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Owner>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<SpawnLocation>(),
          ComponentType.ReadOnly<HangaroundLocation>(),
          ComponentType.ReadOnly<ParkingLane>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_SpawnLocationQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_SpawnLocationElement_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_HangaroundLocation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ReferencesSystem.UpdateBuildingReferencesJob jobData = new ReferencesSystem.UpdateBuildingReferencesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_SpawnLocationType = this.__TypeHandle.__Game_Objects_SpawnLocation_RO_ComponentTypeHandle,
        m_HangaroundLocationType = this.__TypeHandle.__Game_Areas_HangaroundLocation_RO_ComponentTypeHandle,
        m_ParkingLaneType = this.__TypeHandle.__Game_Net_ParkingLane_RO_ComponentTypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_TempData = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentLookup,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_SpawnLocations = this.__TypeHandle.__Game_Buildings_SpawnLocationElement_RW_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<ReferencesSystem.UpdateBuildingReferencesJob>(this.m_SpawnLocationQuery, this.Dependency);
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
    public ReferencesSystem()
    {
    }

    [BurstCompile]
    private struct UpdateBuildingReferencesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<SpawnLocation> m_SpawnLocationType;
      [ReadOnly]
      public ComponentTypeHandle<HangaroundLocation> m_HangaroundLocationType;
      [ReadOnly]
      public ComponentTypeHandle<ParkingLane> m_ParkingLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public ComponentLookup<Temp> m_TempData;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      public BufferLookup<SpawnLocationElement> m_SpawnLocations;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        SpawnLocationType type = SpawnLocationType.None;
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<SpawnLocation>(ref this.m_SpawnLocationType))
        {
          type = SpawnLocationType.SpawnLocation;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (chunk.Has<HangaroundLocation>(ref this.m_HangaroundLocationType))
          {
            type = SpawnLocationType.HangaroundLocation;
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (chunk.Has<ParkingLane>(ref this.m_ParkingLaneType))
              type = SpawnLocationType.ParkingLane;
          }
        }
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Deleted>(ref this.m_DeletedType))
        {
          for (int index = 0; index < nativeArray2.Length; ++index)
          {
            Entity spawnLocation = nativeArray1[index];
            Owner owner = nativeArray2[index];
            DynamicBuffer<SpawnLocationElement> bufferData;
            // ISSUE: reference to a compiler-generated field
            if (this.m_SpawnLocations.TryGetBuffer(owner.m_Owner, out bufferData))
              CollectionUtils.RemoveValue<SpawnLocationElement>(bufferData, new SpawnLocationElement(spawnLocation, type));
            Owner componentData;
            // ISSUE: reference to a compiler-generated field
            while (this.m_OwnerData.TryGetComponent(owner.m_Owner, out componentData))
            {
              owner = componentData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_SpawnLocations.TryGetBuffer(owner.m_Owner, out bufferData))
                CollectionUtils.RemoveValue<SpawnLocationElement>(bufferData, new SpawnLocationElement(spawnLocation, type));
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          bool flag = chunk.Has<Temp>(ref this.m_TempType);
          for (int index = 0; index < nativeArray2.Length; ++index)
          {
            Entity spawnLocation = nativeArray1[index];
            Owner owner = nativeArray2[index];
            // ISSUE: reference to a compiler-generated field
            if (!flag || this.m_TempData.HasComponent(owner.m_Owner))
            {
              DynamicBuffer<SpawnLocationElement> bufferData;
              // ISSUE: reference to a compiler-generated field
              if (this.m_SpawnLocations.TryGetBuffer(owner.m_Owner, out bufferData))
                CollectionUtils.TryAddUniqueValue<SpawnLocationElement>(bufferData, new SpawnLocationElement(spawnLocation, type));
              Owner componentData;
              // ISSUE: reference to a compiler-generated field
              while (this.m_OwnerData.TryGetComponent(owner.m_Owner, out componentData))
              {
                owner = componentData;
                // ISSUE: reference to a compiler-generated field
                if (!flag || this.m_TempData.HasComponent(owner.m_Owner))
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_SpawnLocations.TryGetBuffer(owner.m_Owner, out bufferData))
                    CollectionUtils.TryAddUniqueValue<SpawnLocationElement>(bufferData, new SpawnLocationElement(spawnLocation, type));
                }
                else
                  break;
              }
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

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<SpawnLocation> __Game_Objects_SpawnLocation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<HangaroundLocation> __Game_Areas_HangaroundLocation_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ParkingLane> __Game_Net_ParkingLane_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Temp> __Game_Tools_Temp_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      public BufferLookup<SpawnLocationElement> __Game_Buildings_SpawnLocationElement_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<SpawnLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_HangaroundLocation_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HangaroundLocation>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_ParkingLane_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ParkingLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentLookup = state.GetComponentLookup<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_SpawnLocationElement_RW_BufferLookup = state.GetBufferLookup<SpawnLocationElement>();
      }
    }
  }
}
