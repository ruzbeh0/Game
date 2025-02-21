// Decompiled with JetBrains decompiler
// Type: Game.Policies.DistrictModifierInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Game.Common;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Policies
{
  [CompilerGenerated]
  public class DistrictModifierInitializeSystem : GameSystemBase
  {
    private EntityQuery m_CreatedQuery;
    private DistrictModifierInitializeSystem.DistrictModifierRefreshData m_DistrictModifierRefreshData;
    private DistrictModifierInitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_DistrictModifierRefreshData = new DistrictModifierInitializeSystem.DistrictModifierRefreshData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Created>(), ComponentType.ReadWrite<District>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CreatedQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_DistrictModifierRefreshData.Update((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_DistrictModifier_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Areas_District_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Policies_Policy_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      DistrictModifierInitializeSystem.InitializeDistrictModifiersJob jobData = new DistrictModifierInitializeSystem.InitializeDistrictModifiersJob()
      {
        m_DistrictModifierRefreshData = this.m_DistrictModifierRefreshData,
        m_PolicyType = this.__TypeHandle.__Game_Policies_Policy_RO_BufferTypeHandle,
        m_DistrictType = this.__TypeHandle.__Game_Areas_District_RW_ComponentTypeHandle,
        m_DistrictModifierType = this.__TypeHandle.__Game_Areas_DistrictModifier_RW_BufferTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<DistrictModifierInitializeSystem.InitializeDistrictModifiersJob>(this.m_CreatedQuery, this.Dependency);
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
    public DistrictModifierInitializeSystem()
    {
    }

    [BurstCompile]
    private struct InitializeDistrictModifiersJob : IJobChunk
    {
      [ReadOnly]
      public DistrictModifierInitializeSystem.DistrictModifierRefreshData m_DistrictModifierRefreshData;
      [ReadOnly]
      public BufferTypeHandle<Policy> m_PolicyType;
      public ComponentTypeHandle<District> m_DistrictType;
      public BufferTypeHandle<DistrictModifier> m_DistrictModifierType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<District> nativeArray = chunk.GetNativeArray<District>(ref this.m_DistrictType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<DistrictModifier> bufferAccessor1 = chunk.GetBufferAccessor<DistrictModifier>(ref this.m_DistrictModifierType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Policy> bufferAccessor2 = chunk.GetBufferAccessor<Policy>(ref this.m_PolicyType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          DynamicBuffer<Policy> policies = bufferAccessor2[index];
          if (policies.Length != 0)
          {
            District district = nativeArray[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_DistrictModifierRefreshData.RefreshDistrictOptions(ref district, policies);
            nativeArray[index] = district;
            if (bufferAccessor1.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_DistrictModifierRefreshData.RefreshDistrictModifiers(bufferAccessor1[index], policies);
            }
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

    public struct DistrictModifierRefreshData
    {
      public ComponentLookup<PolicySliderData> m_PolicySliderData;
      public ComponentLookup<DistrictOptionData> m_DistrictOptionData;
      public BufferLookup<DistrictModifierData> m_DistrictModifierData;

      public DistrictModifierRefreshData(SystemBase system)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_PolicySliderData = system.GetComponentLookup<PolicySliderData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_DistrictOptionData = system.GetComponentLookup<DistrictOptionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_DistrictModifierData = system.GetBufferLookup<DistrictModifierData>(true);
      }

      public void Update(SystemBase system)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_PolicySliderData.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_DistrictOptionData.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_DistrictModifierData.Update(system);
      }

      public void RefreshDistrictOptions(ref District district, DynamicBuffer<Policy> policies)
      {
        district.m_OptionMask = 0U;
        for (int index = 0; index < policies.Length; ++index)
        {
          Policy policy = policies[index];
          // ISSUE: reference to a compiler-generated field
          if ((policy.m_Flags & PolicyFlags.Active) != (PolicyFlags) 0 && this.m_DistrictOptionData.HasComponent(policy.m_Policy))
          {
            // ISSUE: reference to a compiler-generated field
            DistrictOptionData districtOptionData = this.m_DistrictOptionData[policy.m_Policy];
            district.m_OptionMask |= districtOptionData.m_OptionMask;
          }
        }
      }

      public void RefreshDistrictModifiers(
        DynamicBuffer<DistrictModifier> modifiers,
        DynamicBuffer<Policy> policies)
      {
        modifiers.Clear();
        for (int index1 = 0; index1 < policies.Length; ++index1)
        {
          Policy policy = policies[index1];
          // ISSUE: reference to a compiler-generated field
          if ((policy.m_Flags & PolicyFlags.Active) != (PolicyFlags) 0 && this.m_DistrictModifierData.HasBuffer(policy.m_Policy))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<DistrictModifierData> dynamicBuffer = this.m_DistrictModifierData[policy.m_Policy];
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              DistrictModifierData modifierData = dynamicBuffer[index2];
              float delta;
              // ISSUE: reference to a compiler-generated field
              if (this.m_PolicySliderData.HasComponent(policy.m_Policy))
              {
                // ISSUE: reference to a compiler-generated field
                PolicySliderData policySliderData = this.m_PolicySliderData[policy.m_Policy];
                float s = math.saturate(math.select((float) (((double) policy.m_Adjustment - (double) policySliderData.m_Range.min) / ((double) policySliderData.m_Range.max - (double) policySliderData.m_Range.min)), 0.0f, (double) policySliderData.m_Range.min == (double) policySliderData.m_Range.max));
                delta = math.lerp(modifierData.m_Range.min, modifierData.m_Range.max, s);
              }
              else
                delta = modifierData.m_Range.min;
              // ISSUE: reference to a compiler-generated method
              DistrictModifierInitializeSystem.DistrictModifierRefreshData.AddModifier(modifiers, modifierData, delta);
            }
          }
        }
      }

      private static void AddModifier(
        DynamicBuffer<DistrictModifier> modifiers,
        DistrictModifierData modifierData,
        float delta)
      {
        while ((DistrictModifierType) modifiers.Length <= modifierData.m_Type)
          modifiers.Add(new DistrictModifier());
        DistrictModifier modifier = modifiers[(int) modifierData.m_Type];
        switch (modifierData.m_Mode)
        {
          case ModifierValueMode.Relative:
            modifier.m_Delta.y = modifier.m_Delta.y * (1f + delta) + delta;
            break;
          case ModifierValueMode.Absolute:
            modifier.m_Delta.x += delta;
            break;
          case ModifierValueMode.InverseRelative:
            delta = (float) (1.0 / (double) math.max(1f / 1000f, 1f + delta) - 1.0);
            modifier.m_Delta.y = modifier.m_Delta.y * (1f + delta) + delta;
            break;
        }
        modifiers[(int) modifierData.m_Type] = modifier;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public BufferTypeHandle<Policy> __Game_Policies_Policy_RO_BufferTypeHandle;
      public ComponentTypeHandle<District> __Game_Areas_District_RW_ComponentTypeHandle;
      public BufferTypeHandle<DistrictModifier> __Game_Areas_DistrictModifier_RW_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Policies_Policy_RO_BufferTypeHandle = state.GetBufferTypeHandle<Policy>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_District_RW_ComponentTypeHandle = state.GetComponentTypeHandle<District>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Areas_DistrictModifier_RW_BufferTypeHandle = state.GetBufferTypeHandle<DistrictModifier>();
      }
    }
  }
}
