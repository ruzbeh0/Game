// Decompiled with JetBrains decompiler
// Type: Game.Policies.RouteModifierInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Prefabs;
using Game.Routes;
using Game.Tools;
using System;
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
  public class RouteModifierInitializeSystem : GameSystemBase
  {
    private EntityQuery m_CreatedQuery;
    private RouteModifierInitializeSystem.RouteModifierRefreshData m_RouteModifierRefreshData;
    private RouteModifierInitializeSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_RouteModifierRefreshData = new RouteModifierInitializeSystem.RouteModifierRefreshData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedQuery = this.GetEntityQuery(ComponentType.ReadOnly<Created>(), ComponentType.ReadWrite<Route>(), ComponentType.ReadOnly<Policy>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CreatedQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_RouteModifierRefreshData.Update((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_RouteModifier_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Routes_Route_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      RouteModifierInitializeSystem.InitializeRouteModifiersJob jobData = new RouteModifierInitializeSystem.InitializeRouteModifiersJob()
      {
        m_RouteModifierRefreshData = this.m_RouteModifierRefreshData,
        m_PolicyType = this.__TypeHandle.__Game_Policies_Policy_RO_BufferTypeHandle,
        m_RouteType = this.__TypeHandle.__Game_Routes_Route_RW_ComponentTypeHandle,
        m_RouteModifierType = this.__TypeHandle.__Game_Routes_RouteModifier_RW_BufferTypeHandle
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<RouteModifierInitializeSystem.InitializeRouteModifiersJob>(this.m_CreatedQuery, this.Dependency);
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
    public RouteModifierInitializeSystem()
    {
    }

    [BurstCompile]
    private struct InitializeRouteModifiersJob : IJobChunk
    {
      [ReadOnly]
      public RouteModifierInitializeSystem.RouteModifierRefreshData m_RouteModifierRefreshData;
      [ReadOnly]
      public BufferTypeHandle<Policy> m_PolicyType;
      public ComponentTypeHandle<Route> m_RouteType;
      public BufferTypeHandle<RouteModifier> m_RouteModifierType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Route> nativeArray = chunk.GetNativeArray<Route>(ref this.m_RouteType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<RouteModifier> bufferAccessor1 = chunk.GetBufferAccessor<RouteModifier>(ref this.m_RouteModifierType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Policy> bufferAccessor2 = chunk.GetBufferAccessor<Policy>(ref this.m_PolicyType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          DynamicBuffer<Policy> policies = bufferAccessor2[index];
          if (policies.Length != 0)
          {
            Route route = nativeArray[index];
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_RouteModifierRefreshData.RefreshRouteOptions(ref route, policies);
            nativeArray[index] = route;
            if (bufferAccessor1.Length != 0)
            {
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              this.m_RouteModifierRefreshData.RefreshRouteModifiers(bufferAccessor1[index], policies);
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

    public struct RouteModifierRefreshData
    {
      public ComponentLookup<PolicySliderData> m_PolicySliderData;
      public ComponentLookup<RouteOptionData> m_RouteOptionData;
      public BufferLookup<RouteModifierData> m_RouteModifierData;

      public RouteModifierRefreshData(SystemBase system)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_PolicySliderData = system.GetComponentLookup<PolicySliderData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_RouteOptionData = system.GetComponentLookup<RouteOptionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_RouteModifierData = system.GetBufferLookup<RouteModifierData>(true);
      }

      public void Update(SystemBase system)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_PolicySliderData.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_RouteOptionData.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_RouteModifierData.Update(system);
      }

      public void RefreshRouteOptions(ref Route route, DynamicBuffer<Policy> policies)
      {
        route.m_OptionMask = 0U;
        for (int index = 0; index < policies.Length; ++index)
        {
          Policy policy = policies[index];
          // ISSUE: reference to a compiler-generated field
          if ((policy.m_Flags & PolicyFlags.Active) != (PolicyFlags) 0 && this.m_RouteOptionData.HasComponent(policy.m_Policy))
          {
            // ISSUE: reference to a compiler-generated field
            RouteOptionData routeOptionData = this.m_RouteOptionData[policy.m_Policy];
            route.m_OptionMask |= routeOptionData.m_OptionMask;
          }
        }
      }

      public void RefreshRouteModifiers(
        DynamicBuffer<RouteModifier> modifiers,
        DynamicBuffer<Policy> policies)
      {
        modifiers.Clear();
        for (int index1 = 0; index1 < policies.Length; ++index1)
        {
          Policy policy = policies[index1];
          // ISSUE: reference to a compiler-generated field
          if ((policy.m_Flags & PolicyFlags.Active) != (PolicyFlags) 0 && this.m_RouteModifierData.HasBuffer(policy.m_Policy))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<RouteModifierData> dynamicBuffer = this.m_RouteModifierData[policy.m_Policy];
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              RouteModifierData modifierData = dynamicBuffer[index2];
              // ISSUE: reference to a compiler-generated field
              // ISSUE: reference to a compiler-generated method
              float modifierDelta = RouteModifierInitializeSystem.RouteModifierRefreshData.GetModifierDelta(modifierData, policy.m_Adjustment, policy.m_Policy, this.m_PolicySliderData);
              // ISSUE: reference to a compiler-generated method
              RouteModifierInitializeSystem.RouteModifierRefreshData.AddModifier(modifiers, modifierData, modifierDelta);
            }
          }
        }
      }

      public static float GetModifierDelta(
        RouteModifierData modifierData,
        float policyAdjustment,
        Entity policy,
        ComponentLookup<PolicySliderData> policySliderData)
      {
        if (!policySliderData.HasComponent(policy))
          return modifierData.m_Range.min;
        PolicySliderData policySliderData1 = policySliderData[policy];
        float s = math.saturate(math.select((float) (((double) policyAdjustment - (double) policySliderData1.m_Range.min) / ((double) policySliderData1.m_Range.max - (double) policySliderData1.m_Range.min)), 0.0f, (double) policySliderData1.m_Range.min == (double) policySliderData1.m_Range.max));
        return math.lerp(modifierData.m_Range.min, modifierData.m_Range.max, s);
      }

      public static float GetPolicyAdjustmentFromModifierDelta(
        RouteModifierData modifierData,
        float modifierDelta,
        PolicySliderData sliderData)
      {
        return math.clamp(math.remap(modifierData.m_Range.min, modifierData.m_Range.max, sliderData.m_Range.min, sliderData.m_Range.max, modifierDelta), sliderData.m_Range.min, sliderData.m_Range.max);
      }

      public static void AddModifierData(
        ref RouteModifier modifier,
        RouteModifierData modifierData,
        float delta)
      {
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
      }

      public static float GetDeltaFromModifier(
        RouteModifier modifier,
        RouteModifierData modifierData)
      {
        switch (modifierData.m_Mode)
        {
          case ModifierValueMode.Relative:
            return modifier.m_Delta.y;
          case ModifierValueMode.Absolute:
            return modifier.m_Delta.x;
          case ModifierValueMode.InverseRelative:
            return (float) (-(double) modifier.m_Delta.y / (1.0 + (double) modifier.m_Delta.y));
          default:
            throw new ArgumentException();
        }
      }

      private static void AddModifier(
        DynamicBuffer<RouteModifier> modifiers,
        RouteModifierData modifierData,
        float delta)
      {
        while ((RouteModifierType) modifiers.Length <= modifierData.m_Type)
          modifiers.Add(new RouteModifier());
        RouteModifier modifier = modifiers[(int) modifierData.m_Type];
        // ISSUE: reference to a compiler-generated method
        RouteModifierInitializeSystem.RouteModifierRefreshData.AddModifierData(ref modifier, modifierData, delta);
        modifiers[(int) modifierData.m_Type] = modifier;
      }
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public BufferTypeHandle<Policy> __Game_Policies_Policy_RO_BufferTypeHandle;
      public ComponentTypeHandle<Route> __Game_Routes_Route_RW_ComponentTypeHandle;
      public BufferTypeHandle<RouteModifier> __Game_Routes_RouteModifier_RW_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Policies_Policy_RO_BufferTypeHandle = state.GetBufferTypeHandle<Policy>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_Route_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Route>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Routes_RouteModifier_RW_BufferTypeHandle = state.GetBufferTypeHandle<RouteModifier>();
      }
    }
  }
}
