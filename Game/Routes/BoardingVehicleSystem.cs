// Decompiled with JetBrains decompiler
// Type: Game.Routes.BoardingVehicleSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Routes
{
  [CompilerGenerated]
  public class BoardingVehicleSystem : GameSystemBase
  {
    private EntityQuery m_WaypointQuery;
    private EntityQuery m_BoardingQuery;
    private bool m_Loaded;
    private BoardingVehicleSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_WaypointQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Waypoint>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_BoardingQuery = this.GetEntityQuery(ComponentType.ReadWrite<BoardingVehicle>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_BoardingQuery);
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = true;
    }

    private bool GetLoaded()
    {
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Loaded)
        return false;
      // ISSUE: reference to a compiler-generated field
      this.m_Loaded = false;
      return true;
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      if (!this.GetLoaded() && this.m_WaypointQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_BoardingVehicle_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      this.Dependency = new BoardingVehicleSystem.BoardingVehicleJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_BoardingVehicleType = this.__TypeHandle.__Game_Routes_BoardingVehicle_RW_ComponentTypeHandle,
        m_TargetData = this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup,
        m_ConnectedData = this.__TypeHandle.__Game_Routes_Connected_RO_ComponentLookup
      }.ScheduleParallel<BoardingVehicleSystem.BoardingVehicleJob>(this.m_BoardingQuery, this.Dependency);
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
    public BoardingVehicleSystem()
    {
    }

    [BurstCompile]
    private struct BoardingVehicleJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      public ComponentTypeHandle<BoardingVehicle> m_BoardingVehicleType;
      [ReadOnly]
      public ComponentLookup<Target> m_TargetData;
      [ReadOnly]
      public ComponentLookup<Connected> m_ConnectedData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<BoardingVehicle> nativeArray2 = chunk.GetNativeArray<BoardingVehicle>(ref this.m_BoardingVehicleType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          BoardingVehicle boardingVehicle = nativeArray2[index];
          // ISSUE: reference to a compiler-generated method
          if (boardingVehicle.m_Vehicle != Entity.Null && !this.IsValidBoarding(nativeArray1[index], boardingVehicle.m_Vehicle))
          {
            boardingVehicle.m_Vehicle = Entity.Null;
            nativeArray2[index] = boardingVehicle;
          }
        }
      }

      private bool IsValidBoarding(Entity stop, Entity vehicle)
      {
        Target componentData1;
        // ISSUE: reference to a compiler-generated field
        if (this.m_TargetData.TryGetComponent(vehicle, out componentData1))
        {
          if (componentData1.m_Target == stop)
            return true;
          Connected componentData2;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ConnectedData.TryGetComponent(componentData1.m_Target, out componentData2))
            return componentData2.m_Connected == stop;
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
      public ComponentTypeHandle<BoardingVehicle> __Game_Routes_BoardingVehicle_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Target> __Game_Common_Target_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Connected> __Game_Routes_Connected_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_BoardingVehicle_RW_ComponentTypeHandle = state.GetComponentTypeHandle<BoardingVehicle>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RO_ComponentLookup = state.GetComponentLookup<Target>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Connected_RO_ComponentLookup = state.GetComponentLookup<Connected>(true);
      }
    }
  }
}
