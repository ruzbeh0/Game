// Decompiled with JetBrains decompiler
// Type: Game.Simulation.NetUpkeepSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.City;
using Game.Common;
using Game.Net;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Assertions;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class NetUpkeepSystem : GameSystemBase
  {
    public static readonly int kUpdatesPerDay = 32;
    private CitySystem m_CitySystem;
    private SimulationSystem m_SimulationSystem;
    private CityStatisticsSystem m_CityStatisticsSystem;
    private EntityQuery m_UpkeepQuery;
    private NetUpkeepSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / (NetUpkeepSystem.kUpdatesPerDay * 16);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityStatisticsSystem = this.World.GetOrCreateSystemManaged<CityStatisticsSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpkeepQuery = this.GetEntityQuery(ComponentType.ReadOnly<Composition>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<Owner>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Native>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_UpkeepQuery);
      // ISSUE: reference to a compiler-generated field
      Assert.AreEqual(NetUpkeepSystem.kUpdatesPerDay, 32);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      UpdateFrame sharedComponent = new UpdateFrame()
      {
        m_Index = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, NetUpkeepSystem.kUpdatesPerDay, 16)
      };
      // ISSUE: reference to a compiler-generated field
      this.m_UpkeepQuery.ResetFilter();
      // ISSUE: reference to a compiler-generated field
      this.m_UpkeepQuery.SetSharedComponentFilter<UpdateFrame>(sharedComponent);
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_UpkeepQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableNetComposition_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle deps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      NetUpkeepSystem.NetUpkeepJob jobData = new NetUpkeepSystem.NetUpkeepJob()
      {
        m_CompositionType = this.__TypeHandle.__Game_Net_Composition_RO_ComponentTypeHandle,
        m_CurveType = this.__TypeHandle.__Game_Net_Curve_RO_ComponentTypeHandle,
        m_PlaceableNetCompositionData = this.__TypeHandle.__Game_Prefabs_PlaceableNetComposition_RO_ComponentLookup,
        m_Chunks = archetypeChunkListAsync,
        m_StatisticsEventQueue = this.m_CityStatisticsSystem.GetStatisticsEventQueue(out deps).AsParallelWriter()
      };
      this.Dependency = jobData.Schedule<NetUpkeepSystem.NetUpkeepJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle, deps));
      archetypeChunkListAsync.Dispose(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityStatisticsSystem.AddWriter(this.Dependency);
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
    public NetUpkeepSystem()
    {
    }

    [BurstCompile]
    private struct NetUpkeepJob : IJob
    {
      [ReadOnly]
      public ComponentTypeHandle<Composition> m_CompositionType;
      [ReadOnly]
      public ComponentTypeHandle<Curve> m_CurveType;
      [ReadOnly]
      public ComponentLookup<PlaceableNetComposition> m_PlaceableNetCompositionData;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      public NativeQueue<StatisticsEvent>.ParallelWriter m_StatisticsEventQueue;

      public void Execute()
      {
        int num = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          NativeArray<Composition> nativeArray1 = chunk.GetNativeArray<Composition>(ref this.m_CompositionType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Curve> nativeArray2 = chunk.GetNativeArray<Curve>(ref this.m_CurveType);
          for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
          {
            Composition composition = nativeArray1[index2];
            Curve curve = nativeArray2[index2];
            // ISSUE: reference to a compiler-generated field
            if (this.m_PlaceableNetCompositionData.HasComponent(composition.m_Edge))
            {
              // ISSUE: reference to a compiler-generated field
              PlaceableNetComposition placeableNetData = this.m_PlaceableNetCompositionData[composition.m_Edge];
              num += NetUtils.GetUpkeepCost(curve, placeableNetData);
            }
          }
        }
        if (num == 0)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_StatisticsEventQueue.Enqueue(new StatisticsEvent()
        {
          m_Statistic = StatisticType.Expense,
          m_Change = (float) num,
          m_Parameter = 5
        });
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Composition> __Game_Net_Composition_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Curve> __Game_Net_Curve_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<PlaceableNetComposition> __Game_Prefabs_PlaceableNetComposition_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Composition_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Composition>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Net_Curve_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Curve>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableNetComposition_RO_ComponentLookup = state.GetComponentLookup<PlaceableNetComposition>(true);
      }
    }
  }
}
