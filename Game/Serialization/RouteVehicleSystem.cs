// Decompiled with JetBrains decompiler
// Type: Game.Serialization.RouteVehicleSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Routes;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class RouteVehicleSystem : GameSystemBase
  {
    private EntityQuery m_Query;
    private RouteVehicleSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_Query = this.GetEntityQuery(ComponentType.ReadOnly<Vehicle>(), ComponentType.ReadOnly<CurrentRoute>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_Query);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteVehicle_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_CurrentRoute_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      RouteVehicleSystem.RouteVehicleJob jobData = new RouteVehicleSystem.RouteVehicleJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_CurrentRouteType = this.__TypeHandle.__Game_Routes_CurrentRoute_RO_ComponentTypeHandle,
        m_RouteVehicles = this.__TypeHandle.__Game_Routes_RouteVehicle_RW_BufferLookup
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<RouteVehicleSystem.RouteVehicleJob>(this.m_Query, this.Dependency);
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
    public RouteVehicleSystem()
    {
    }

    [BurstCompile]
    private struct RouteVehicleJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<CurrentRoute> m_CurrentRouteType;
      public BufferLookup<RouteVehicle> m_RouteVehicles;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<CurrentRoute> nativeArray2 = chunk.GetNativeArray<CurrentRoute>(ref this.m_CurrentRouteType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Entity vehicle = nativeArray1[index];
          DynamicBuffer<RouteVehicle> bufferData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_RouteVehicles.TryGetBuffer(nativeArray2[index].m_Route, out bufferData))
            bufferData.Add(new RouteVehicle(vehicle));
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
      public ComponentTypeHandle<CurrentRoute> __Game_Routes_CurrentRoute_RO_ComponentTypeHandle;
      public BufferLookup<RouteVehicle> __Game_Routes_RouteVehicle_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_CurrentRoute_RO_ComponentTypeHandle = state.GetComponentTypeHandle<CurrentRoute>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteVehicle_RW_BufferLookup = state.GetBufferLookup<RouteVehicle>();
      }
    }
  }
}
