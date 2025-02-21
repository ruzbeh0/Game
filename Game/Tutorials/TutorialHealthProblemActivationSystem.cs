// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.TutorialHealthProblemActivationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Citizens;
using Game.Common;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Tutorials
{
  [CompilerGenerated]
  public class TutorialHealthProblemActivationSystem : GameSystemBase
  {
    protected EntityCommandBufferSystem m_BarrierSystem;
    private EntityQuery m_TutorialQuery;
    private EntityQuery m_HealthProblemQuery;
    private EntityQuery m_MedicalClinicQuery;
    private EntityQuery m_MedicalClinicUnlockedQuery;
    private EntityQuery m_CemeteryQuery;
    private EntityQuery m_CemeteryUnlockedQuery;
    private TutorialHealthProblemActivationSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_BarrierSystem = (EntityCommandBufferSystem) this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      // ISSUE: reference to a compiler-generated field
      this.m_TutorialQuery = this.GetEntityQuery(ComponentType.ReadOnly<HealthProblemActivationData>(), ComponentType.Exclude<TutorialActivated>(), ComponentType.Exclude<TutorialCompleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_HealthProblemQuery = this.GetEntityQuery(ComponentType.ReadOnly<Citizen>(), ComponentType.ReadOnly<HealthProblem>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_MedicalClinicQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<Game.Buildings.Hospital>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_MedicalClinicUnlockedQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabData>(), ComponentType.ReadOnly<HospitalData>(), ComponentType.ReadOnly<BuildingData>(), ComponentType.Exclude<Locked>());
      // ISSUE: reference to a compiler-generated field
      this.m_CemeteryQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<Game.Buildings.DeathcareFacility>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_CemeteryUnlockedQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabData>(), ComponentType.ReadOnly<DeathcareFacilityData>(), ComponentType.ReadOnly<BuildingData>(), ComponentType.Exclude<Locked>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_HealthProblemQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TutorialQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      bool flag1 = this.m_MedicalClinicQuery.IsEmptyIgnoreFilter && !this.m_MedicalClinicUnlockedQuery.IsEmpty;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      bool flag2 = this.m_CemeteryQuery.IsEmptyIgnoreFilter && !this.m_CemeteryUnlockedQuery.IsEmpty;
      if (!(flag1 | flag2))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tutorials_HealthProblemActivationData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle;
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
      TutorialHealthProblemActivationSystem.CheckProblemsJob jobData = new TutorialHealthProblemActivationSystem.CheckProblemsJob()
      {
        m_EntityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_ActivationType = this.__TypeHandle.__Game_Tutorials_HealthProblemActivationData_RO_ComponentTypeHandle,
        m_HealthProblemType = this.__TypeHandle.__Game_Citizens_HealthProblem_RO_ComponentTypeHandle,
        m_HealthProblemChunks = this.m_HealthProblemQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle),
        m_NoHospital = flag1,
        m_NoCemetery = flag2,
        m_Writer = this.m_BarrierSystem.CreateCommandBuffer().AsParallelWriter()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<TutorialHealthProblemActivationSystem.CheckProblemsJob>(this.m_TutorialQuery, JobHandle.CombineDependencies(this.Dependency, outJobHandle));
      // ISSUE: reference to a compiler-generated field
      jobData.m_HealthProblemChunks.Dispose(this.Dependency);
      // ISSUE: reference to a compiler-generated field
      this.m_BarrierSystem.AddJobHandleForProducer(this.Dependency);
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
    public TutorialHealthProblemActivationSystem()
    {
    }

    [BurstCompile]
    private struct CheckProblemsJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<HealthProblemActivationData> m_ActivationType;
      [ReadOnly]
      public ComponentTypeHandle<HealthProblem> m_HealthProblemType;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_HealthProblemChunks;
      public bool m_NoHospital;
      public bool m_NoCemetery;
      public EntityCommandBuffer.ParallelWriter m_Writer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<HealthProblemActivationData> nativeArray1 = chunk.GetNativeArray<HealthProblemActivationData>(ref this.m_ActivationType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray2 = chunk.GetNativeArray(this.m_EntityTypeHandle);
        for (int index = 0; index < nativeArray1.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated method
          if (this.Execute(nativeArray1[index]))
          {
            // ISSUE: reference to a compiler-generated field
            this.m_Writer.AddComponent<TutorialActivated>(unfilteredChunkIndex, nativeArray2[index]);
          }
        }
      }

      private bool Execute(HealthProblemActivationData data)
      {
        if ((data.m_Require & HealthProblemFlags.Dead) != HealthProblemFlags.None)
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_NoCemetery)
            return false;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (!this.m_NoHospital)
            return false;
        }
        int num = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_HealthProblemChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<HealthProblem> nativeArray = this.m_HealthProblemChunks[index1].GetNativeArray<HealthProblem>(ref this.m_HealthProblemType);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            if ((nativeArray[index2].m_Flags & data.m_Require) != HealthProblemFlags.None)
              ++num;
            if (num >= data.m_RequiredCount)
              return true;
          }
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
      [ReadOnly]
      public ComponentTypeHandle<HealthProblemActivationData> __Game_Tutorials_HealthProblemActivationData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<HealthProblem> __Game_Citizens_HealthProblem_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tutorials_HealthProblemActivationData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HealthProblemActivationData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Citizens_HealthProblem_RO_ComponentTypeHandle = state.GetComponentTypeHandle<HealthProblem>(true);
      }
    }
  }
}
