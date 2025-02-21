// Decompiled with JetBrains decompiler
// Type: Game.Policies.BuildingModifierInitializeSystem
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
using Unity.Mathematics;

#nullable disable
namespace Game.Policies
{
  [CompilerGenerated]
  public class BuildingModifierInitializeSystem : GameSystemBase
  {
    private EntityQuery m_CreatedQuery;
    private BuildingModifierInitializeSystem.BuildingModifierRefreshData m_BuildingModifierRefreshData;
    private BuildingModifierInitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_BuildingModifierRefreshData = new BuildingModifierInitializeSystem.BuildingModifierRefreshData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Created>(), ComponentType.ReadWrite<Building>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CreatedQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_BuildingModifierRefreshData.Update((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_BuildingModifier_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      BuildingModifierInitializeSystem.InitializeBuildingModifiersJob jobData = new BuildingModifierInitializeSystem.InitializeBuildingModifiersJob()
      {
        m_BuildingModifierRefreshData = this.m_BuildingModifierRefreshData,
        m_PolicyType = this.__TypeHandle.__Game_Policies_Policy_RO_BufferTypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RW_ComponentTypeHandle,
        m_BuildingModifierType = this.__TypeHandle.__Game_Buildings_BuildingModifier_RW_BufferTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<BuildingModifierInitializeSystem.InitializeBuildingModifiersJob>(this.m_CreatedQuery, this.Dependency);
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
    public BuildingModifierInitializeSystem()
    {
    }

    [BurstCompile]
    private struct InitializeBuildingModifiersJob : IJobChunk
    {
      [ReadOnly]
      public BuildingModifierInitializeSystem.BuildingModifierRefreshData m_BuildingModifierRefreshData;
      [ReadOnly]
      public BufferTypeHandle<Policy> m_PolicyType;
      public ComponentTypeHandle<Building> m_BuildingType;
      public BufferTypeHandle<BuildingModifier> m_BuildingModifierType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Building> nativeArray = chunk.GetNativeArray<Building>(ref this.m_BuildingType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<BuildingModifier> bufferAccessor1 = chunk.GetBufferAccessor<BuildingModifier>(ref this.m_BuildingModifierType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Policy> bufferAccessor2 = chunk.GetBufferAccessor<Policy>(ref this.m_PolicyType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          DynamicBuffer<Policy> policies = bufferAccessor2[index];
          if (policies.Length != 0)
          {
            Building building = nativeArray[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_BuildingModifierRefreshData.RefreshBuildingOptions(ref building, policies);
            nativeArray[index] = building;
            if (bufferAccessor1.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_BuildingModifierRefreshData.RefreshBuildingModifiers(bufferAccessor1[index], policies);
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

    public struct BuildingModifierRefreshData
    {
      public ComponentLookup<PolicySliderData> m_PolicySliderData;
      public ComponentLookup<BuildingOptionData> m_BuildingOptionData;
      public BufferLookup<BuildingModifierData> m_BuildingModifierData;

      public BuildingModifierRefreshData(SystemBase system)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_PolicySliderData = system.GetComponentLookup<PolicySliderData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingOptionData = system.GetComponentLookup<BuildingOptionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingModifierData = system.GetBufferLookup<BuildingModifierData>(true);
      }

      public void Update(SystemBase system)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_PolicySliderData.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingOptionData.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingModifierData.Update(system);
      }

      public void RefreshBuildingOptions(ref Building building, DynamicBuffer<Policy> policies)
      {
        building.m_OptionMask = 0U;
        for (int index = 0; index < policies.Length; ++index)
        {
          Policy policy = policies[index];
          // ISSUE: reference to a compiler-generated field
          if ((policy.m_Flags & PolicyFlags.Active) != (PolicyFlags) 0 && this.m_BuildingOptionData.HasComponent(policy.m_Policy))
          {
            // ISSUE: reference to a compiler-generated field
            BuildingOptionData buildingOptionData = this.m_BuildingOptionData[policy.m_Policy];
            building.m_OptionMask |= buildingOptionData.m_OptionMask;
          }
        }
      }

      public void RefreshBuildingModifiers(
        DynamicBuffer<BuildingModifier> modifiers,
        DynamicBuffer<Policy> policies)
      {
        modifiers.Clear();
        for (int index1 = 0; index1 < policies.Length; ++index1)
        {
          Policy policy = policies[index1];
          // ISSUE: reference to a compiler-generated field
          if ((policy.m_Flags & PolicyFlags.Active) != (PolicyFlags) 0 && this.m_BuildingModifierData.HasBuffer(policy.m_Policy))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<BuildingModifierData> dynamicBuffer = this.m_BuildingModifierData[policy.m_Policy];
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              BuildingModifierData modifierData = dynamicBuffer[index2];
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
              BuildingModifierInitializeSystem.BuildingModifierRefreshData.AddModifier(modifiers, modifierData, delta);
            }
          }
        }
      }

      private static void AddModifier(
        DynamicBuffer<BuildingModifier> modifiers,
        BuildingModifierData modifierData,
        float delta)
      {
        while ((BuildingModifierType) modifiers.Length <= modifierData.m_Type)
          modifiers.Add(new BuildingModifier());
        BuildingModifier modifier = modifiers[(int) modifierData.m_Type];
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
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RW_ComponentTypeHandle;
      public BufferTypeHandle<BuildingModifier> __Game_Buildings_BuildingModifier_RW_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Policies_Policy_RO_BufferTypeHandle = state.GetBufferTypeHandle<Policy>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Building>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_BuildingModifier_RW_BufferTypeHandle = state.GetBufferTypeHandle<BuildingModifier>();
      }
    }
  }
}
