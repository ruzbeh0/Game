// Decompiled with JetBrains decompiler
// Type: Game.Serialization.CheckPrefabReferencesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Serialization
{
  [CompilerGenerated]
  public class CheckPrefabReferencesSystem : GameSystemBase
  {
    private NativeArray<Entity> m_PrefabArray;
    private UnsafeList<bool> m_ReferencedPrefabs;
    private JobHandle m_DataDeps;
    private JobHandle m_UserDeps;
    private bool m_IsLoading;

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new CheckPrefabReferencesSystem.CheckPrefabReferencesJob()
      {
        m_PrefabArray = this.m_PrefabArray,
        m_PrefabData = this.GetComponentLookup<PrefabData>(false),
        m_ReferencedPrefabs = this.m_ReferencedPrefabs
      }.Schedule<CheckPrefabReferencesSystem.CheckPrefabReferencesJob>(this.m_PrefabArray.Length, 64, JobHandle.CombineDependencies(this.m_DataDeps, this.m_UserDeps, this.Dependency));
      // ISSUE: reference to a compiler-generated field
      this.m_DataDeps = jobHandle;
      // ISSUE: reference to a compiler-generated field
      this.m_UserDeps = jobHandle;
      this.Dependency = jobHandle;
    }

    public void BeginPrefabCheck(NativeArray<Entity> array, bool isLoading, JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabArray = array;
      // ISSUE: reference to a compiler-generated field
      this.m_ReferencedPrefabs = new UnsafeList<bool>(0, (AllocatorManager.AllocatorHandle) Allocator.TempJob);
      // ISSUE: reference to a compiler-generated field
      this.m_ReferencedPrefabs.Resize(array.Length, NativeArrayOptions.ClearMemory);
      // ISSUE: reference to a compiler-generated field
      this.m_DataDeps = dependencies;
      // ISSUE: reference to a compiler-generated field
      this.m_IsLoading = isLoading;
    }

    public void EndPrefabCheck(out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      dependencies = JobHandle.CombineDependencies(this.m_DataDeps, this.m_UserDeps);
      // ISSUE: reference to a compiler-generated field
      this.m_ReferencedPrefabs.Dispose(dependencies);
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabArray = new NativeArray<Entity>();
      // ISSUE: reference to a compiler-generated field
      this.m_ReferencedPrefabs = new UnsafeList<bool>();
      // ISSUE: reference to a compiler-generated field
      this.m_DataDeps = new JobHandle();
      // ISSUE: reference to a compiler-generated field
      this.m_UserDeps = new JobHandle();
    }

    public PrefabReferences GetPrefabReferences(SystemBase system, out JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      dependencies = this.m_DataDeps;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      return new PrefabReferences(this.m_PrefabArray, this.m_ReferencedPrefabs, system.GetComponentLookup<PrefabData>(true), this.m_IsLoading);
    }

    public void AddPrefabReferencesUser(JobHandle dependencies)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_UserDeps = JobHandle.CombineDependencies(this.m_UserDeps, dependencies);
    }

    [UnityEngine.Scripting.Preserve]
    public CheckPrefabReferencesSystem()
    {
    }

    [BurstCompile]
    private struct CheckPrefabReferencesJob : IJobParallelFor
    {
      [ReadOnly]
      public NativeArray<Entity> m_PrefabArray;
      [NativeDisableParallelForRestriction]
      public ComponentLookup<PrefabData> m_PrefabData;
      public UnsafeList<bool> m_ReferencedPrefabs;

      public void Execute(int index)
      {
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ReferencedPrefabs[index])
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabData.SetComponentEnabled(this.m_PrefabArray[index], true);
        // ISSUE: reference to a compiler-generated field
        this.m_ReferencedPrefabs[index] = false;
      }
    }
  }
}
