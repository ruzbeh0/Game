// Decompiled with JetBrains decompiler
// Type: Game.Simulation.TelecomFacilityAISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Common;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class TelecomFacilityAISystem : GameSystemBase
  {
    private EntityQuery m_BuildingQuery;
    private TelecomFacilityAISystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;

    public override int GetUpdateOffset(SystemUpdatePhase phase) => 208;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_BuildingQuery = this.GetEntityQuery(ComponentType.ReadOnly<Game.Buildings.TelecomFacility>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_BuildingQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_TelecomFacility_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      this.Dependency = new TelecomFacilityAISystem.TelecomFacilityTickJob()
      {
        m_TelecomFacilityType = this.__TypeHandle.__Game_Buildings_TelecomFacility_RW_ComponentTypeHandle
      }.ScheduleParallel<TelecomFacilityAISystem.TelecomFacilityTickJob>(this.m_BuildingQuery, this.Dependency);
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
    public TelecomFacilityAISystem()
    {
    }

    [BurstCompile]
    private struct TelecomFacilityTickJob : IJobChunk
    {
      public ComponentTypeHandle<Game.Buildings.TelecomFacility> m_TelecomFacilityType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Buildings.TelecomFacility> nativeArray = chunk.GetNativeArray<Game.Buildings.TelecomFacility>(ref this.m_TelecomFacilityType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          Game.Buildings.TelecomFacility telecomFacility = nativeArray[index];
          telecomFacility.m_Flags |= TelecomFacilityFlags.HasCoverage;
          nativeArray[index] = telecomFacility;
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
      public ComponentTypeHandle<Game.Buildings.TelecomFacility> __Game_Buildings_TelecomFacility_RW_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_TelecomFacility_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.TelecomFacility>();
      }
    }
  }
}
