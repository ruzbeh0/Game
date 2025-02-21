// Decompiled with JetBrains decompiler
// Type: Game.Rendering.CompleteCullingSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class CompleteCullingSystem : GameSystemBase
  {
    private PreCullingSystem m_CullingSystem;
    private CompleteCullingSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CullingSystem = this.World.GetOrCreateSystemManaged<PreCullingSystem>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_CullingInfo_RW_ComponentLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      JobHandle dependencies2 = new CompleteCullingSystem.CullingCleanupJob()
      {
        m_CullingInfo = this.__TypeHandle.__Game_Rendering_CullingInfo_RW_ComponentLookup,
        m_CullingData = this.m_CullingSystem.GetCullingData(false, out dependencies1)
      }.Schedule<CompleteCullingSystem.CullingCleanupJob>(JobHandle.CombineDependencies(this.Dependency, dependencies1));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CullingSystem.AddCullingDataWriter(dependencies2);
      this.Dependency = dependencies2;
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
    public CompleteCullingSystem()
    {
    }

    [BurstCompile]
    private struct CullingCleanupJob : IJob
    {
      public ComponentLookup<CullingInfo> m_CullingInfo;
      public NativeList<PreCullingData> m_CullingData;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_CullingData.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          ref PreCullingData local1 = ref this.m_CullingData.ElementAt(index);
          if ((local1.m_Flags & (PreCullingFlags.NearCameraUpdated | PreCullingFlags.Updated | PreCullingFlags.Created | PreCullingFlags.Applied | PreCullingFlags.BatchesUpdated | PreCullingFlags.ColorsUpdated)) != (PreCullingFlags) 0)
          {
            if ((local1.m_Flags & PreCullingFlags.NearCamera) == (PreCullingFlags) 0)
            {
              if ((local1.m_Flags & PreCullingFlags.Deleted) == (PreCullingFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CullingInfo.GetRefRW(local1.m_Entity).ValueRW.m_CullingIndex = 0;
              }
              // ISSUE: reference to a compiler-generated field
              this.m_CullingData.RemoveAtSwapBack(index);
              // ISSUE: reference to a compiler-generated field
              if (index < this.m_CullingData.Length)
              {
                // ISSUE: reference to a compiler-generated field
                ref PreCullingData local2 = ref this.m_CullingData.ElementAt(index);
                if ((local2.m_Flags & PreCullingFlags.Deleted) == (PreCullingFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_CullingInfo.GetRefRW(local2.m_Entity).ValueRW.m_CullingIndex = index;
                }
              }
              --index;
            }
            else
              local1.m_Flags &= ~(PreCullingFlags.NearCameraUpdated | PreCullingFlags.Updated | PreCullingFlags.Created | PreCullingFlags.Applied | PreCullingFlags.BatchesUpdated | PreCullingFlags.ColorsUpdated);
          }
        }
      }
    }

    private struct TypeHandle
    {
      public ComponentLookup<CullingInfo> __Game_Rendering_CullingInfo_RW_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_CullingInfo_RW_ComponentLookup = state.GetComponentLookup<CullingInfo>();
      }
    }
  }
}
