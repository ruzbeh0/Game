// Decompiled with JetBrains decompiler
// Type: Game.Serialization.DataMigration.ResidentPseudoRandomSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Citizens;
using Game.Common;
using Game.Creatures;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Serialization.DataMigration
{
  [CompilerGenerated]
  public class ResidentPseudoRandomSystem : GameSystemBase
  {
    private LoadGameSystem m_LoadGameSystem;
    private EntityQuery m_Query;
    private ResidentPseudoRandomSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_LoadGameSystem = this.World.GetOrCreateSystemManaged<LoadGameSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_Query = this.GetEntityQuery(ComponentType.ReadOnly<Resident>(), ComponentType.ReadWrite<PseudoRandomSeed>());
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_LoadGameSystem.context.version >= Version.residentPseudoRandomFix || this.m_Query.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_PseudoRandomSeed_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      ResidentPseudoRandomSystem.ResidentPseudoRandomJob jobData = new ResidentPseudoRandomSystem.ResidentPseudoRandomJob()
      {
        m_ResidentType = this.__TypeHandle.__Game_Creatures_Resident_RO_ComponentTypeHandle,
        m_PseudoRandomSeedType = this.__TypeHandle.__Game_Common_PseudoRandomSeed_RW_ComponentTypeHandle,
        m_CitizenData = this.__TypeHandle.__Game_Citizens_Citizen_RO_ComponentLookup
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<ResidentPseudoRandomSystem.ResidentPseudoRandomJob>(this.m_Query, this.Dependency);
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
    public ResidentPseudoRandomSystem()
    {
    }

    [BurstCompile]
    private struct ResidentPseudoRandomJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Resident> m_ResidentType;
      public ComponentTypeHandle<PseudoRandomSeed> m_PseudoRandomSeedType;
      [ReadOnly]
      public ComponentLookup<Citizen> m_CitizenData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Resident> nativeArray1 = chunk.GetNativeArray<Resident>(ref this.m_ResidentType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PseudoRandomSeed> nativeArray2 = chunk.GetNativeArray<PseudoRandomSeed>(ref this.m_PseudoRandomSeedType);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          Resident resident = nativeArray1[index];
          ref PseudoRandomSeed local = ref nativeArray2.ElementAt<PseudoRandomSeed>(index);
          Citizen componentData;
          // ISSUE: reference to a compiler-generated field
          if (this.m_CitizenData.TryGetComponent(resident.m_Citizen, out componentData))
          {
            Random pseudoRandom = componentData.GetPseudoRandom(CitizenPseudoRandom.SpawnResident);
            local = new PseudoRandomSeed(ref pseudoRandom);
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
      public ComponentTypeHandle<Resident> __Game_Creatures_Resident_RO_ComponentTypeHandle;
      public ComponentTypeHandle<PseudoRandomSeed> __Game_Common_PseudoRandomSeed_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<Citizen> __Game_Citizens_Citizen_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Creatures_Resident_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Resident>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_PseudoRandomSeed_RW_ComponentTypeHandle = state.GetComponentTypeHandle<PseudoRandomSeed>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_Citizen_RO_ComponentLookup = state.GetComponentLookup<Citizen>(true);
      }
    }
  }
}
