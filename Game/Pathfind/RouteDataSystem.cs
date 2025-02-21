// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.RouteDataSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Common;
using Game.Objects;
using Game.Prefabs;
using Game.Routes;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Pathfind
{
  [CompilerGenerated]
  public class RouteDataSystem : GameSystemBase
  {
    private EntityQuery m_UpdateQuery;
    private RouteDataSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Updated>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Routes.TransportStop>(),
          ComponentType.ReadOnly<Game.Routes.TakeoffLocation>(),
          ComponentType.ReadOnly<Game.Objects.SpawnLocation>()
        },
        None = new ComponentType[3]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Game.Routes.MailBox>(),
          ComponentType.ReadOnly<Temp>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<PathfindUpdated>()
        },
        Any = new ComponentType[3]
        {
          ComponentType.ReadOnly<Game.Routes.TransportStop>(),
          ComponentType.ReadOnly<Game.Routes.TakeoffLocation>(),
          ComponentType.ReadOnly<Game.Objects.SpawnLocation>()
        },
        None = new ComponentType[4]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Game.Routes.MailBox>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_UpdateQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_SpawnLocation_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_TakeoffLocation_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_TransportStop_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      RouteDataSystem.UpdateRouteDataJob jobData = new RouteDataSystem.UpdateRouteDataJob()
      {
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_TransportStopType = this.__TypeHandle.__Game_Routes_TransportStop_RW_ComponentTypeHandle,
        m_TakeoffLocationType = this.__TypeHandle.__Game_Routes_TakeoffLocation_RW_ComponentTypeHandle,
        m_SpawnLocationType = this.__TypeHandle.__Game_Objects_SpawnLocation_RW_ComponentTypeHandle,
        m_OwnerData = this.__TypeHandle.__Game_Common_Owner_RO_ComponentLookup,
        m_AttachmentData = this.__TypeHandle.__Game_Objects_Attachment_RO_ComponentLookup,
        m_BuildingData = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_PrefabBuildingData = this.__TypeHandle.__Game_Prefabs_BuildingData_RO_ComponentLookup,
        m_PrefabRouteConnectionData = this.__TypeHandle.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup,
        m_PrefabSpawnLocationData = this.__TypeHandle.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<RouteDataSystem.UpdateRouteDataJob>(this.m_UpdateQuery, this.Dependency);
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
    public RouteDataSystem()
    {
    }

    [BurstCompile]
    private struct UpdateRouteDataJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public ComponentTypeHandle<Game.Routes.TransportStop> m_TransportStopType;
      public ComponentTypeHandle<Game.Routes.TakeoffLocation> m_TakeoffLocationType;
      public ComponentTypeHandle<Game.Objects.SpawnLocation> m_SpawnLocationType;
      [ReadOnly]
      public ComponentLookup<Owner> m_OwnerData;
      [ReadOnly]
      public ComponentLookup<Attachment> m_AttachmentData;
      [ReadOnly]
      public ComponentLookup<Building> m_BuildingData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<BuildingData> m_PrefabBuildingData;
      [ReadOnly]
      public ComponentLookup<RouteConnectionData> m_PrefabRouteConnectionData;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> m_PrefabSpawnLocationData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Routes.TransportStop> nativeArray1 = chunk.GetNativeArray<Game.Routes.TransportStop>(ref this.m_TransportStopType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Routes.TakeoffLocation> nativeArray2 = chunk.GetNativeArray<Game.Routes.TakeoffLocation>(ref this.m_TakeoffLocationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Objects.SpawnLocation> nativeArray3 = chunk.GetNativeArray<Game.Objects.SpawnLocation>(ref this.m_SpawnLocationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray4 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray5 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        if (nativeArray1.Length != 0)
        {
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Game.Routes.TransportStop transportStop = nativeArray1[index] with
            {
              m_AccessRestriction = Entity.Null
            };
            transportStop.m_Flags &= ~StopFlags.AllowEnter;
            if (nativeArray4.Length != 0)
            {
              Owner owner = nativeArray4[index];
              // ISSUE: reference to a compiler-generated field
              RouteConnectionData routeConnectionData = this.m_PrefabRouteConnectionData[nativeArray5[index].m_Prefab];
              // ISSUE: reference to a compiler-generated method
              // ISSUE: reference to a compiler-generated method
              Game.Prefabs.BuildingFlags flag = this.GetRestrictFlag(routeConnectionData.m_AccessConnectionType) | this.GetRestrictFlag(routeConnectionData.m_RouteConnectionType);
              bool allowEnter;
              // ISSUE: reference to a compiler-generated method
              transportStop.m_AccessRestriction = this.GetAccessRestriction(owner, flag, out allowEnter);
              if (allowEnter)
                transportStop.m_Flags |= StopFlags.AllowEnter;
            }
            nativeArray1[index] = transportStop;
          }
        }
        if (nativeArray3.Length != 0)
        {
          for (int index = 0; index < nativeArray3.Length; ++index)
          {
            Game.Objects.SpawnLocation spawnLocation = nativeArray3[index] with
            {
              m_AccessRestriction = Entity.Null
            };
            spawnLocation.m_Flags &= ~SpawnLocationFlags.AllowEnter;
            if (nativeArray4.Length != 0)
            {
              Owner owner = nativeArray4[index];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              Game.Prefabs.BuildingFlags restrictFlag = this.GetRestrictFlag(this.m_PrefabSpawnLocationData[nativeArray5[index].m_Prefab].m_ConnectionType);
              bool allowEnter;
              // ISSUE: reference to a compiler-generated method
              spawnLocation.m_AccessRestriction = this.GetAccessRestriction(owner, restrictFlag, out allowEnter);
              if (allowEnter)
                spawnLocation.m_Flags |= SpawnLocationFlags.AllowEnter;
            }
            nativeArray3[index] = spawnLocation;
          }
        }
        if (nativeArray2.Length == 0)
          return;
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Game.Routes.TakeoffLocation takeoffLocation = nativeArray2[index] with
          {
            m_AccessRestriction = Entity.Null
          };
          takeoffLocation.m_Flags &= ~TakeoffLocationFlags.AllowEnter;
          if (nativeArray3.Length != 0)
          {
            Game.Objects.SpawnLocation spawnLocation = nativeArray3[index];
            takeoffLocation.m_AccessRestriction = spawnLocation.m_AccessRestriction;
            if ((spawnLocation.m_Flags & SpawnLocationFlags.AllowEnter) != (SpawnLocationFlags) 0)
              takeoffLocation.m_Flags |= TakeoffLocationFlags.AllowEnter;
          }
          else if (nativeArray4.Length != 0)
          {
            Owner owner = nativeArray4[index];
            // ISSUE: reference to a compiler-generated field
            RouteConnectionData routeConnectionData = this.m_PrefabRouteConnectionData[nativeArray5[index].m_Prefab];
            // ISSUE: reference to a compiler-generated method
            // ISSUE: reference to a compiler-generated method
            Game.Prefabs.BuildingFlags flag = this.GetRestrictFlag(routeConnectionData.m_AccessConnectionType) | this.GetRestrictFlag(routeConnectionData.m_RouteConnectionType);
            bool allowEnter;
            // ISSUE: reference to a compiler-generated method
            takeoffLocation.m_AccessRestriction = this.GetAccessRestriction(owner, flag, out allowEnter);
            if (allowEnter)
              takeoffLocation.m_Flags |= TakeoffLocationFlags.AllowEnter;
          }
          nativeArray2[index] = takeoffLocation;
        }
      }

      private Game.Prefabs.BuildingFlags GetRestrictFlag(RouteConnectionType routeConnectionType)
      {
        switch (routeConnectionType)
        {
          case RouteConnectionType.Road:
            return Game.Prefabs.BuildingFlags.RestrictedCar;
          case RouteConnectionType.Pedestrian:
            return Game.Prefabs.BuildingFlags.RestrictedPedestrian;
          case RouteConnectionType.Track:
            return Game.Prefabs.BuildingFlags.RestrictedTrack;
          case RouteConnectionType.Cargo:
            return Game.Prefabs.BuildingFlags.RestrictedCar;
          case RouteConnectionType.Air:
            return Game.Prefabs.BuildingFlags.RestrictedCar;
          case RouteConnectionType.Parking:
            return Game.Prefabs.BuildingFlags.RestrictedPedestrian | Game.Prefabs.BuildingFlags.RestrictedCar;
          default:
            return (Game.Prefabs.BuildingFlags) 0;
        }
      }

      private Entity GetAccessRestriction(Owner owner, Game.Prefabs.BuildingFlags flag, out bool allowEnter)
      {
        Entity entity = owner.m_Owner;
        // ISSUE: reference to a compiler-generated field
        while (this.m_OwnerData.TryGetComponent(entity, out owner))
          entity = owner.m_Owner;
        Attachment componentData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_AttachmentData.TryGetComponent(entity, out componentData) && componentData.m_Attached != Entity.Null)
          entity = componentData.m_Attached;
        // ISSUE: reference to a compiler-generated field
        if (this.m_BuildingData.HasComponent(entity))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bool flag1 = (this.m_PrefabBuildingData[this.m_PrefabRefData[entity].m_Prefab].m_Flags & flag) > (Game.Prefabs.BuildingFlags) 0;
          if (flag1 || (flag & (Game.Prefabs.BuildingFlags.RestrictedPedestrian | Game.Prefabs.BuildingFlags.RestrictedCar)) != (Game.Prefabs.BuildingFlags) 0)
          {
            allowEnter = !flag1;
            return entity;
          }
        }
        allowEnter = false;
        return Entity.Null;
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
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Routes.TransportStop> __Game_Routes_TransportStop_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Routes.TakeoffLocation> __Game_Routes_TakeoffLocation_RW_ComponentTypeHandle;
      public ComponentTypeHandle<Game.Objects.SpawnLocation> __Game_Objects_SpawnLocation_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Owner> __Game_Common_Owner_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Attachment> __Game_Objects_Attachment_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Building> __Game_Buildings_Building_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<BuildingData> __Game_Prefabs_BuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<RouteConnectionData> __Game_Prefabs_RouteConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Game.Prefabs.SpawnLocationData> __Game_Prefabs_SpawnLocationData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TransportStop_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Routes.TransportStop>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_TakeoffLocation_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Routes.TakeoffLocation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_SpawnLocation_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.SpawnLocation>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentLookup = state.GetComponentLookup<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Attachment_RO_ComponentLookup = state.GetComponentLookup<Attachment>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentLookup = state.GetComponentLookup<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_BuildingData_RO_ComponentLookup = state.GetComponentLookup<BuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_RouteConnectionData_RO_ComponentLookup = state.GetComponentLookup<RouteConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SpawnLocationData_RO_ComponentLookup = state.GetComponentLookup<Game.Prefabs.SpawnLocationData>(true);
      }
    }
  }
}
