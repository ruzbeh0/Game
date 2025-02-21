// Decompiled with JetBrains decompiler
// Type: Game.Effects.CompleteEnabledSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Effects
{
  [CompilerGenerated]
  public class CompleteEnabledSystem : GameSystemBase
  {
    private EffectControlSystem m_EffectControlSystem;
    private VFXSystem m_VFXSystem;
    private CompleteEnabledSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_EffectControlSystem = this.World.GetOrCreateSystemManaged<EffectControlSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_VFXSystem = this.World.GetOrCreateSystemManaged<VFXSystem>();
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Effects_EnabledEffect_RW_BufferLookup.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      JobHandle jobHandle = new CompleteEnabledSystem.EffectCleanupJob()
      {
        m_EffectOwners = this.__TypeHandle.__Game_Effects_EnabledEffect_RW_BufferLookup,
        m_EnabledData = this.m_EffectControlSystem.GetEnabledData(false, out dependencies),
        m_VFXUpdateQueue = this.m_VFXSystem.GetSourceUpdateData()
      }.Schedule<CompleteEnabledSystem.EffectCleanupJob>(JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_EffectControlSystem.AddEnabledDataWriter(jobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_VFXSystem.AddSourceUpdateWriter(jobHandle);
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
    public CompleteEnabledSystem()
    {
    }

    [BurstCompile]
    private struct EffectCleanupJob : IJob
    {
      public BufferLookup<EnabledEffect> m_EffectOwners;
      public NativeList<EnabledEffectData> m_EnabledData;
      public NativeQueue<VFXUpdateInfo> m_VFXUpdateQueue;

      public void Execute()
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_EnabledData.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          ref EnabledEffectData local1 = ref this.m_EnabledData.ElementAt(index1);
          if ((local1.m_Flags & (EnabledEffectFlags.EnabledUpdated | EnabledEffectFlags.OwnerUpdated)) != (EnabledEffectFlags) 0)
          {
            if ((local1.m_Flags & EnabledEffectFlags.IsEnabled) == (EnabledEffectFlags) 0)
            {
              if ((local1.m_Flags & EnabledEffectFlags.Deleted) == (EnabledEffectFlags) 0)
              {
                // ISSUE: reference to a compiler-generated field
                DynamicBuffer<EnabledEffect> effectOwner = this.m_EffectOwners[local1.m_Owner];
                for (int index2 = 0; index2 < effectOwner.Length; ++index2)
                {
                  if (effectOwner[index2].m_EffectIndex == local1.m_EffectIndex)
                  {
                    effectOwner.RemoveAt(index2);
                    break;
                  }
                }
              }
              // ISSUE: reference to a compiler-generated field
              this.m_EnabledData.RemoveAtSwapBack(index1);
              // ISSUE: reference to a compiler-generated field
              if (index1 < this.m_EnabledData.Length)
              {
                // ISSUE: reference to a compiler-generated field
                ref EnabledEffectData local2 = ref this.m_EnabledData.ElementAt(index1);
                if ((local2.m_Flags & EnabledEffectFlags.Deleted) == (EnabledEffectFlags) 0)
                {
                  // ISSUE: reference to a compiler-generated field
                  DynamicBuffer<EnabledEffect> effectOwner = this.m_EffectOwners[local2.m_Owner];
                  for (int index3 = 0; index3 < effectOwner.Length; ++index3)
                  {
                    ref EnabledEffect local3 = ref effectOwner.ElementAt(index3);
                    if (local3.m_EffectIndex == local2.m_EffectIndex)
                    {
                      local3.m_EnabledIndex = index1;
                      break;
                    }
                  }
                }
                if ((local2.m_Flags & (EnabledEffectFlags.IsEnabled | EnabledEffectFlags.IsVFX)) == (EnabledEffectFlags.IsEnabled | EnabledEffectFlags.IsVFX))
                {
                  // ISSUE: reference to a compiler-generated field
                  // ISSUE: reference to a compiler-generated field
                  this.m_VFXUpdateQueue.Enqueue(new VFXUpdateInfo()
                  {
                    m_Type = VFXUpdateType.MoveIndex,
                    m_EnabledIndex = new int2(index1, this.m_EnabledData.Length)
                  });
                }
              }
              --index1;
            }
            else
              local1.m_Flags &= ~(EnabledEffectFlags.EnabledUpdated | EnabledEffectFlags.OwnerUpdated);
          }
        }
      }
    }

    private struct TypeHandle
    {
      public BufferLookup<EnabledEffect> __Game_Effects_EnabledEffect_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Effects_EnabledEffect_RW_BufferLookup = state.GetBufferLookup<EnabledEffect>();
      }
    }
  }
}
