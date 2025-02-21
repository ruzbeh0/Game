// Decompiled with JetBrains decompiler
// Type: Game.Simulation.NetEdgeDensitySystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Companies;
using Game.Net;
using Game.Pathfind;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class NetEdgeDensitySystem : GameSystemBase
  {
    private EntityQuery m_EdgeQuery;
    private PathfindQueueSystem m_PathfindQueueSystem;
    private NetEdgeDensitySystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 1024;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PathfindQueueSystem = this.World.GetOrCreateSystemManaged<PathfindQueueSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EdgeQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Net.Edge>(), ComponentType.ReadOnly<ConnectedBuilding>(), ComponentType.ReadWrite<Density>(), ComponentType.ReadOnly<Curve>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EdgeQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      DensityAction action = new DensityAction(Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Density_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_SubLane_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ConnectedBuilding_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      NetEdgeDensitySystem.CalculateDensityJob jobData = new NetEdgeDensitySystem.CalculateDensityJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_ConnectedBuilding_RO_BufferTypeHandle,
        m_SubLaneType = this.__TypeHandle.__Game_Net_SubLane_RO_BufferTypeHandle,
        m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
        m_DensityType = this.__TypeHandle.__Game_Net_Density_RW_ComponentTypeHandle,
        m_Renters = this.__TypeHandle.__Game_Buildings_Renter_RO_BufferLookup,
        m_Workplaces = this.__TypeHandle.__Game_Companies_WorkProvider_RO_ComponentLookup,
        m_CarLaneData = this.__TypeHandle.__Game_Net_CarLane_RO_ComponentLookup,
        m_HouseholdCitizens = this.__TypeHandle.__Game_Citizens_HouseholdCitizen_RO_BufferLookup,
        m_DensityActions = action.m_DensityData.AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<NetEdgeDensitySystem.CalculateDensityJob>(this.m_EdgeQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PathfindQueueSystem.Enqueue(action, this.Dependency);
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
    public NetEdgeDensitySystem()
    {
    }

    [BurstCompile]
    private struct CalculateDensityJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<ConnectedBuilding> m_BuildingType;
      [ReadOnly]
      public BufferTypeHandle<SubLane> m_SubLaneType;
      [ReadOnly]
      public ComponentTypeHandle<Curve> m_CurveType;
      public ComponentTypeHandle<Density> m_DensityType;
      [ReadOnly]
      public ComponentLookup<WorkProvider> m_Workplaces;
      [ReadOnly]
      public ComponentLookup<CarLane> m_CarLaneData;
      [ReadOnly]
      public BufferLookup<Renter> m_Renters;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> m_HouseholdCitizens;
      public NativeQueue<DensityActionData>.ParallelWriter m_DensityActions;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Density> nativeArray2 = chunk.GetNativeArray<Density>(ref this.m_DensityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Curve> nativeArray3 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<ConnectedBuilding> bufferAccessor1 = chunk.GetBufferAccessor<ConnectedBuilding>(ref this.m_BuildingType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<SubLane> bufferAccessor2 = chunk.GetBufferAccessor<SubLane>(ref this.m_SubLaneType);
        for (int index1 = 0; index1 < nativeArray1.Length; ++index1)
        {
          Density density1 = nativeArray2[index1];
          float density2 = density1.m_Density;
          density1.m_Density = 0.0f;
          DynamicBuffer<ConnectedBuilding> dynamicBuffer1 = bufferAccessor1[index1];
          for (int index2 = 0; index2 < dynamicBuffer1.Length; ++index2)
          {
            Entity building = dynamicBuffer1[index2].m_Building;
            // ISSUE: reference to a compiler-generated field
            if (this.m_Renters.HasBuffer(building))
            {
              // ISSUE: reference to a compiler-generated field
              DynamicBuffer<Renter> renter1 = this.m_Renters[building];
              for (int index3 = 0; index3 < renter1.Length; ++index3)
              {
                Entity renter2 = renter1[index3].m_Renter;
                // ISSUE: reference to a compiler-generated field
                if (this.m_HouseholdCitizens.HasBuffer(renter2))
                {
                  // ISSUE: reference to a compiler-generated field
                  density1.m_Density += (float) this.m_HouseholdCitizens[renter2].Length;
                }
                else
                {
                  // ISSUE: reference to a compiler-generated field
                  if (this.m_Workplaces.HasComponent(renter2))
                  {
                    // ISSUE: reference to a compiler-generated field
                    density1.m_Density += (float) this.m_Workplaces[renter2].m_MaxWorkers;
                  }
                }
              }
            }
            // ISSUE: reference to a compiler-generated field
            if (this.m_Workplaces.HasComponent(building))
            {
              // ISSUE: reference to a compiler-generated field
              density1.m_Density += (float) this.m_Workplaces[building].m_MaxWorkers;
            }
          }
          density1.m_Density /= nativeArray3[index1].m_Length;
          nativeArray2[index1] = density1;
          if ((double) density1.m_Density != (double) density2)
          {
            float num = math.sqrt(math.max(0.01f, density1.m_Density));
            DynamicBuffer<SubLane> dynamicBuffer2 = bufferAccessor2[index1];
            for (int index4 = 0; index4 < dynamicBuffer2.Length; ++index4)
            {
              Entity subLane = dynamicBuffer2[index4].m_SubLane;
              // ISSUE: reference to a compiler-generated field
              if (this.m_CarLaneData.HasComponent(subLane))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_DensityActions.Enqueue(new DensityActionData()
                {
                  m_Owner = subLane,
                  m_Density = num
                });
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
      public BufferTypeHandle<ConnectedBuilding> __Game_Buildings_ConnectedBuilding_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<SubLane> __Game_Net_SubLane_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Density> __Game_Net_Density_RW_ComponentTypeHandle;
      [ReadOnly]
      public BufferLookup<Renter> __Game_Buildings_Renter_RO_BufferLookup;
      [ReadOnly]
      public ComponentLookup<WorkProvider> __Game_Companies_WorkProvider_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<CarLane> __Game_Net_CarLane_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<HouseholdCitizen> __Game_Citizens_HouseholdCitizen_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ConnectedBuilding_RO_BufferTypeHandle = state.GetBufferTypeHandle<ConnectedBuilding>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_SubLane_RO_BufferTypeHandle = state.GetBufferTypeHandle<SubLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Density_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Density>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Renter_RO_BufferLookup = state.GetBufferLookup<Renter>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Companies_WorkProvider_RO_ComponentLookup = state.GetComponentLookup<WorkProvider>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_CarLane_RO_ComponentLookup = state.GetComponentLookup<CarLane>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HouseholdCitizen_RO_BufferLookup = state.GetBufferLookup<HouseholdCitizen>(true);
      }
    }
  }
}
