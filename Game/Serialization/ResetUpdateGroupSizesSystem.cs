// Decompiled with JetBrains decompiler
// Type: Game.Serialization.ResetUpdateGroupSizesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Simulation;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class ResetUpdateGroupSizesSystem : GameSystemBase
  {
    private UpdateGroupSystem m_UpdateGroupSystem;
    private UpdateGroupSystem.UpdateGroupTypes m_UpdateGroupTypes;
    private EntityQuery m_UpdateFrameQuery;
    private ResetUpdateGroupSizesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateGroupSystem = this.World.GetOrCreateSystemManaged<UpdateGroupSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdateFrameQuery = this.GetEntityQuery(ComponentType.ReadOnly<UpdateFrame>());
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_UpdateGroupTypes = new UpdateGroupSystem.UpdateGroupTypes((SystemBase) this);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_UpdateFrameQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_UpdateGroupTypes.Update((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      JobHandle inputDeps = new ResetUpdateGroupSizesSystem.ResetUpdateGroupSizesJob()
      {
        m_Chunks = archetypeChunkListAsync,
        m_UpdateFrameType = this.__TypeHandle.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle,
        m_UpdateGroupTypes = this.m_UpdateGroupTypes,
        m_UpdateGroupSizes = this.m_UpdateGroupSystem.GetUpdateGroupSizes()
      }.Schedule<ResetUpdateGroupSizesSystem.ResetUpdateGroupSizesJob>(JobHandle.CombineDependencies(this.Dependency, outJobHandle));
      archetypeChunkListAsync.Dispose(inputDeps);
      this.Dependency = inputDeps;
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
    public ResetUpdateGroupSizesSystem()
    {
    }

    [BurstCompile]
    private struct ResetUpdateGroupSizesJob : IJob
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_Chunks;
      [ReadOnly]
      public SharedComponentTypeHandle<UpdateFrame> m_UpdateFrameType;
      [ReadOnly]
      public UpdateGroupSystem.UpdateGroupTypes m_UpdateGroupTypes;
      public UpdateGroupSystem.UpdateGroupSizes m_UpdateGroupSizes;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_UpdateGroupSizes.Clear();
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_Chunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ArchetypeChunk chunk = this.m_Chunks[index1];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          NativeArray<int> nativeArray = this.m_UpdateGroupSizes.Get(chunk, this.m_UpdateGroupTypes);
          if (nativeArray.IsCreated)
          {
            // ISSUE: reference to a compiler-generated field
            uint index2 = chunk.GetSharedComponent<UpdateFrame>(this.m_UpdateFrameType).m_Index;
            if ((long) index2 < (long) nativeArray.Length)
              nativeArray[(int) index2] = nativeArray[(int) index2] + chunk.Count;
          }
        }
      }
    }

    private struct TypeHandle
    {
      public SharedComponentTypeHandle<UpdateFrame> __Game_Simulation_UpdateFrame_SharedComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Simulation_UpdateFrame_SharedComponentTypeHandle = state.GetSharedComponentTypeHandle<UpdateFrame>();
      }
    }
  }
}
