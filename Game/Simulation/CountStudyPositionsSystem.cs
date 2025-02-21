// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CountStudyPositionsSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Game.Buildings;
using Game.Common;
using Game.Debug;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class CountStudyPositionsSystem : GameSystemBase, IDefaultSerializable, ISerializable
  {
    private EntityQuery m_SchoolQuery;
    [DebugWatchValue]
    private NativeArray<int> m_StudyPositionByEducation;
    private JobHandle m_WriteDependencies;
    private JobHandle m_ReadDependencies;
    private CountStudyPositionsSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 16;

    public NativeArray<int> GetStudyPositionsByEducation(out JobHandle deps)
    {
      // ISSUE: reference to a compiler-generated field
      deps = this.m_WriteDependencies;
      // ISSUE: reference to a compiler-generated field
      return this.m_StudyPositionByEducation;
    }

    public void AddReader(JobHandle reader)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ReadDependencies = JobHandle.CombineDependencies(this.m_ReadDependencies, reader);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_SchoolQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<Game.Buildings.School>(), ComponentType.ReadOnly<Student>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Game.Objects.OutsideConnection>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_StudyPositionByEducation = new NativeArray<int>(5, Allocator.Persistent);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_StudyPositionByEducation.Dispose();
      base.OnDestroy();
    }

    public void SetDefaults(Colossal.Serialization.Entities.Context context)
    {
      for (int index = 0; index < 5; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_StudyPositionByEducation[index] = 0;
      }
    }

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      // ISSUE: reference to a compiler-generated field
      writer.Write(this.m_StudyPositionByEducation);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version < Version.economyFix)
      {
        reader.Read(out int _);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        reader.Read(this.m_StudyPositionByEducation);
      }
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_SchoolData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Student_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle outJobHandle;
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
      CountStudyPositionsSystem.CountStudyPositionsJob jobData = new CountStudyPositionsSystem.CountStudyPositionsJob()
      {
        m_SchoolChunks = this.m_SchoolQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) this.World.UpdateAllocator.ToAllocator, out outJobHandle),
        m_PrefabType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_UpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_StudentType = this.__TypeHandle.__Game_Buildings_Student_RO_BufferTypeHandle,
        m_OutsideConnectionDatas = this.__TypeHandle.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup,
        m_PrefabDatas = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_SchoolDatas = this.__TypeHandle.__Game_Prefabs_SchoolData_RO_ComponentLookup,
        m_StudyPositionByEducation = this.m_StudyPositionByEducation
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.Schedule<CountStudyPositionsSystem.CountStudyPositionsJob>(JobHandle.CombineDependencies(this.Dependency, this.m_ReadDependencies, outJobHandle));
      // ISSUE: reference to a compiler-generated field
      this.m_WriteDependencies = this.Dependency;
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
    public CountStudyPositionsSystem()
    {
    }

    [BurstCompile]
    private struct CountStudyPositionsJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_SchoolChunks;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabType;
      [ReadOnly]
      public BufferTypeHandle<Student> m_StudentType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_UpgradeType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabDatas;
      [ReadOnly]
      public ComponentLookup<SchoolData> m_SchoolDatas;
      [ReadOnly]
      public ComponentLookup<OutsideConnectionData> m_OutsideConnectionDatas;
      public NativeArray<int> m_StudyPositionByEducation;

      public void Execute()
      {
        for (int index = 0; index < 5; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_StudyPositionByEducation[index] = 0;
        }
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_SchoolChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk schoolChunk = this.m_SchoolChunks[index1];
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<Student> bufferAccessor1 = schoolChunk.GetBufferAccessor<Student>(ref this.m_StudentType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray = schoolChunk.GetNativeArray<PrefabRef>(ref this.m_PrefabType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<InstalledUpgrade> bufferAccessor2 = schoolChunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_UpgradeType);
          bool flag = bufferAccessor2.Length != 0;
          for (int index2 = 0; index2 < bufferAccessor1.Length; ++index2)
          {
            Entity prefab = nativeArray[index2].m_Prefab;
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            if (!this.m_OutsideConnectionDatas.HasComponent(prefab) && this.m_SchoolDatas.HasComponent(prefab))
            {
              // ISSUE: reference to a compiler-generated field
              SchoolData schoolData = this.m_SchoolDatas[prefab];
              if (flag)
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                UpgradeUtils.CombineStats<SchoolData>(ref schoolData, bufferAccessor2[index2], ref this.m_PrefabDatas, ref this.m_SchoolDatas);
              }
              DynamicBuffer<Student> dynamicBuffer = bufferAccessor1[index2];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated field
              this.m_StudyPositionByEducation[(int) this.m_SchoolDatas[prefab].m_EducationLevel] += math.max(0, schoolData.m_StudentCapacity / 2 - dynamicBuffer.Length);
            }
          }
        }
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Student> __Game_Buildings_Student_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<OutsideConnectionData> __Game_Prefabs_OutsideConnectionData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<SchoolData> __Game_Prefabs_SchoolData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Student_RO_BufferTypeHandle = state.GetBufferTypeHandle<Student>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_OutsideConnectionData_RO_ComponentLookup = state.GetComponentLookup<OutsideConnectionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_SchoolData_RO_ComponentLookup = state.GetComponentLookup<SchoolData>(true);
      }
    }
  }
}
