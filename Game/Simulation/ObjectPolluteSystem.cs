// Decompiled with JetBrains decompiler
// Type: Game.Simulation.ObjectPolluteSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
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
  public class ObjectPolluteSystem : GameSystemBase
  {
    public static readonly int kUpdatesPerDay = 32;
    private GroundPollutionSystem m_GroundPollutionSystem;
    private AirPollutionSystem m_AirPollutionSystem;
    private SimulationSystem m_SimulationSystem;
    private EntityQuery m_PollutionParameterQuery;
    private EntityQuery m_PollutableObjectQuery;
    private ObjectPolluteSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase)
    {
      // ISSUE: reference to a compiler-generated field
      return 262144 / (ObjectPolluteSystem.kUpdatesPerDay * 16);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_GroundPollutionSystem = this.World.GetOrCreateSystemManaged<GroundPollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AirPollutionSystem = this.World.GetOrCreateSystemManaged<AirPollutionSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SimulationSystem = this.World.GetOrCreateSystemManaged<SimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PollutionParameterQuery = this.GetEntityQuery(ComponentType.ReadOnly<PollutionParameterData>());
      // ISSUE: reference to a compiler-generated field
      this.m_PollutableObjectQuery = this.GetEntityQuery(ComponentType.ReadOnly<Plant>(), ComponentType.ReadOnly<Transform>(), ComponentType.ReadOnly<UpdateFrame>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      uint updateFrame = SimulationUtils.GetUpdateFrame(this.m_SimulationSystem.frameIndex, ObjectPolluteSystem.kUpdatesPerDay, 16);
      // ISSUE: reference to a compiler-generated field
      this.m_PollutableObjectQuery.ResetFilter();
      // ISSUE: reference to a compiler-generated field
      this.m_PollutableObjectQuery.SetSharedComponentFilter<UpdateFrame>(new UpdateFrame(updateFrame));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Plant_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies1;
      JobHandle dependencies2;
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
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new ObjectPolluteSystem.ObjectPolluteJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_PlantType = this.__TypeHandle.__Game_Objects_Plant_RW_ComponentTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_GroundPollutionMap = this.m_GroundPollutionSystem.GetMap(true, out dependencies1),
        m_AirPollutionMap = this.m_AirPollutionSystem.GetMap(true, out dependencies2),
        m_PollutionParameters = this.m_PollutionParameterQuery.GetSingleton<PollutionParameterData>()
      }.ScheduleParallel<ObjectPolluteSystem.ObjectPolluteJob>(this.m_PollutableObjectQuery, JobHandle.CombineDependencies(dependencies2, this.Dependency, dependencies1));
      // ISSUE: reference to a compiler-generated field
      this.m_GroundPollutionSystem.AddReader(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_AirPollutionSystem.AddReader(jobHandle);
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
    public ObjectPolluteSystem()
    {
    }

    [BurstCompile]
    private struct ObjectPolluteJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      public ComponentTypeHandle<Plant> m_PlantType;
      [ReadOnly]
      public NativeArray<GroundPollution> m_GroundPollutionMap;
      [ReadOnly]
      public NativeArray<AirPollution> m_AirPollutionMap;
      public PollutionParameterData m_PollutionParameters;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Plant> nativeArray2 = chunk.GetNativeArray<Plant>(ref this.m_PlantType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray3 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          float3 position = nativeArray3[index].m_Position;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          GroundPollution pollution1 = GroundPollutionSystem.GetPollution(position, this.m_GroundPollutionMap);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          AirPollution pollution2 = AirPollutionSystem.GetPollution(position, this.m_AirPollutionMap);
          Plant plant = nativeArray2[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          plant.m_Pollution = math.saturate(plant.m_Pollution + ((float) ((double) this.m_PollutionParameters.m_PlantGroundMultiplier * (double) pollution1.m_Pollution + (double) this.m_PollutionParameters.m_PlantAirMultiplier * (double) pollution2.m_Pollution) - this.m_PollutionParameters.m_PlantFade) / (float) ObjectPolluteSystem.kUpdatesPerDay);
          nativeArray2[index] = plant;
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
      public ComponentTypeHandle<Plant> __Game_Objects_Plant_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Plant_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Plant>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
      }
    }
  }
}
