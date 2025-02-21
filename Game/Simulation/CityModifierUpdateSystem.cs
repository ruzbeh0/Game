// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CityModifierUpdateSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.City;
using Game.Common;
using Game.Policies;
using Game.Prefabs;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class CityModifierUpdateSystem : GameSystemBase
  {
    private EntityQuery m_CityQuery;
    private EntityQuery m_EffectProviderQuery;
    private CityModifierUpdateSystem.CityModifierRefreshData m_CityModifierRefreshData;
    private CityModifierUpdateSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: object of a compiler-generated type is created
      this.m_CityModifierRefreshData = new CityModifierUpdateSystem.CityModifierRefreshData((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      this.m_CityQuery = this.GetEntityQuery(ComponentType.ReadWrite<Game.City.City>());
      // ISSUE: reference to a compiler-generated field
      this.m_EffectProviderQuery = this.GetEntityQuery(ComponentType.ReadOnly<CityEffectProvider>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_CityQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_EffectProviderQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_CityModifierRefreshData.Update((SystemBase) this);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_CityModifier_RW_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_City_City_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated field
      JobHandle inputDeps = new CityModifierUpdateSystem.UpdateCityModifiersJob()
      {
        m_CityModifierRefreshData = this.m_CityModifierRefreshData,
        m_EffectProviderChunks = archetypeChunkListAsync,
        m_PolicyType = this.__TypeHandle.__Game_Policies_Policy_RO_BufferTypeHandle,
        m_CityType = this.__TypeHandle.__Game_City_City_RW_ComponentTypeHandle,
        m_CityModifierType = this.__TypeHandle.__Game_City_CityModifier_RW_BufferTypeHandle
      }.ScheduleParallel<CityModifierUpdateSystem.UpdateCityModifiersJob>(this.m_CityQuery, JobHandle.CombineDependencies(this.Dependency, outJobHandle));
      archetypeChunkListAsync.Dispose(inputDeps);
      this.Dependency = inputDeps;
    }

    public static void InitializeTempList(NativeList<CityModifierData> tempModifierList)
    {
      tempModifierList.Clear();
    }

    public static void InitializeTempList(
      NativeList<CityModifierData> tempModifierList,
      DynamicBuffer<CityModifierData> cityModifiers)
    {
      tempModifierList.Clear();
      tempModifierList.AddRange(cityModifiers.AsNativeArray());
    }

    public static void AddToTempList(
      NativeList<CityModifierData> tempModifierList,
      DynamicBuffer<CityModifierData> cityModifiers)
    {
label_10:
      for (int index1 = 0; index1 < cityModifiers.Length; ++index1)
      {
        CityModifierData cityModifier = cityModifiers[index1];
        for (int index2 = 0; index2 < tempModifierList.Length; ++index2)
        {
          CityModifierData tempModifier = tempModifierList[index2];
          if (tempModifier.m_Type == cityModifier.m_Type)
          {
            if (tempModifier.m_Mode != cityModifier.m_Mode)
              throw new Exception(string.Format("Modifier mode mismatch (type: {0})", (object) cityModifier.m_Type));
            tempModifier.m_Range.min += cityModifier.m_Range.min;
            tempModifier.m_Range.max += cityModifier.m_Range.max;
            tempModifierList[index2] = tempModifier;
            goto label_10;
          }
        }
        tempModifierList.Add(in cityModifier);
      }
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
    public CityModifierUpdateSystem()
    {
    }

    [BurstCompile]
    private struct UpdateCityModifiersJob : IJobChunk
    {
      [ReadOnly]
      public CityModifierUpdateSystem.CityModifierRefreshData m_CityModifierRefreshData;
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_EffectProviderChunks;
      [ReadOnly]
      public BufferTypeHandle<Policy> m_PolicyType;
      public ComponentTypeHandle<Game.City.City> m_CityType;
      public BufferTypeHandle<CityModifier> m_CityModifierType;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeList<CityModifierData> tempModifierList = new NativeList<CityModifierData>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.City.City> nativeArray = chunk.GetNativeArray<Game.City.City>(ref this.m_CityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<CityModifier> bufferAccessor1 = chunk.GetBufferAccessor<CityModifier>(ref this.m_CityModifierType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Policy> bufferAccessor2 = chunk.GetBufferAccessor<Policy>(ref this.m_PolicyType);
        for (int index = 0; index < nativeArray.Length; ++index)
        {
          Game.City.City city = nativeArray[index];
          DynamicBuffer<CityModifier> modifiers = bufferAccessor1[index];
          DynamicBuffer<Policy> policies = bufferAccessor2[index];
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_CityModifierRefreshData.RefreshCityOptions(ref city, policies);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_CityModifierRefreshData.RefreshCityModifiers(modifiers, policies, this.m_EffectProviderChunks, tempModifierList);
          nativeArray[index] = city;
        }
        tempModifierList.Dispose();
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

    public struct CityModifierRefreshData
    {
      public BufferTypeHandle<Efficiency> m_BuildingEfficiencyType;
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      public ComponentTypeHandle<Game.Buildings.Signature> m_SignatureType;
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      public ComponentLookup<PolicySliderData> m_PolicySliderData;
      public ComponentLookup<CityOptionData> m_CityOptionData;
      public BufferLookup<CityModifierData> m_CityModifierData;

      public CityModifierRefreshData(SystemBase system)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingEfficiencyType = system.GetBufferTypeHandle<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabRefType = system.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_InstalledUpgradeType = system.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_SignatureType = system.GetComponentTypeHandle<Game.Buildings.Signature>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabRefData = system.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_PolicySliderData = system.GetComponentLookup<PolicySliderData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_CityOptionData = system.GetComponentLookup<CityOptionData>(true);
        // ISSUE: reference to a compiler-generated field
        this.m_CityModifierData = system.GetBufferLookup<CityModifierData>(true);
      }

      public void Update(SystemBase system)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_BuildingEfficiencyType.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabRefType.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_InstalledUpgradeType.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_SignatureType.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_PrefabRefData.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_PolicySliderData.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_CityOptionData.Update(system);
        // ISSUE: reference to a compiler-generated field
        this.m_CityModifierData.Update(system);
      }

      public void RefreshCityOptions(ref Game.City.City city, DynamicBuffer<Policy> policies)
      {
        city.m_OptionMask = 0U;
        for (int index = 0; index < policies.Length; ++index)
        {
          Policy policy = policies[index];
          // ISSUE: reference to a compiler-generated field
          if ((policy.m_Flags & PolicyFlags.Active) != (PolicyFlags) 0 && this.m_CityOptionData.HasComponent(policy.m_Policy))
          {
            // ISSUE: reference to a compiler-generated field
            CityOptionData cityOptionData = this.m_CityOptionData[policy.m_Policy];
            city.m_OptionMask |= cityOptionData.m_OptionMask;
          }
        }
      }

      public void RefreshCityModifiers(
        DynamicBuffer<CityModifier> modifiers,
        DynamicBuffer<Policy> policies,
        NativeList<ArchetypeChunk> effectProviderChunks,
        NativeList<CityModifierData> tempModifierList)
      {
        modifiers.Clear();
        for (int index1 = 0; index1 < policies.Length; ++index1)
        {
          Policy policy = policies[index1];
          // ISSUE: reference to a compiler-generated field
          if ((policy.m_Flags & PolicyFlags.Active) != (PolicyFlags) 0 && this.m_CityModifierData.HasBuffer(policy.m_Policy))
          {
            // ISSUE: reference to a compiler-generated field
            DynamicBuffer<CityModifierData> dynamicBuffer = this.m_CityModifierData[policy.m_Policy];
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              CityModifierData modifierData = dynamicBuffer[index2];
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
              CityModifierUpdateSystem.CityModifierRefreshData.AddModifier(modifiers, modifierData, delta);
            }
          }
        }
        for (int index3 = 0; index3 < effectProviderChunks.Length; ++index3)
        {
          ArchetypeChunk effectProviderChunk = effectProviderChunks[index3];
          // ISSUE: reference to a compiler-generated field
          int num = effectProviderChunk.Has<Game.Buildings.Signature>(ref this.m_SignatureType) ? 1 : 0;
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<Efficiency> bufferAccessor1 = effectProviderChunk.GetBufferAccessor<Efficiency>(ref this.m_BuildingEfficiencyType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray = effectProviderChunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<InstalledUpgrade> bufferAccessor2 = effectProviderChunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
          if (num == 0 && bufferAccessor1.Length != 0)
          {
            for (int index4 = 0; index4 < nativeArray.Length; ++index4)
            {
              PrefabRef prefabRef = nativeArray[index4];
              float efficiency = BuildingUtils.GetEfficiency(bufferAccessor1[index4]);
              // ISSUE: reference to a compiler-generated field
              if (this.m_CityModifierData.HasBuffer(prefabRef.m_Prefab))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                CityModifierUpdateSystem.InitializeTempList(tempModifierList, this.m_CityModifierData[prefabRef.m_Prefab]);
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                CityModifierUpdateSystem.InitializeTempList(tempModifierList);
              }
              if (bufferAccessor2.Length != 0)
              {
                // ISSUE: reference to a compiler-generated method
                this.AddToTempList(tempModifierList, bufferAccessor2[index4]);
              }
              for (int index5 = 0; index5 < tempModifierList.Length; ++index5)
              {
                CityModifierData tempModifier = tempModifierList[index5];
                float delta = math.lerp(tempModifier.m_Range.min, tempModifier.m_Range.max, efficiency);
                // ISSUE: reference to a compiler-generated method
                CityModifierUpdateSystem.CityModifierRefreshData.AddModifier(modifiers, tempModifier, delta);
              }
            }
          }
          else
          {
            for (int index6 = 0; index6 < nativeArray.Length; ++index6)
            {
              PrefabRef prefabRef = nativeArray[index6];
              // ISSUE: reference to a compiler-generated field
              if (this.m_CityModifierData.HasBuffer(prefabRef.m_Prefab))
              {
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated method
                CityModifierUpdateSystem.InitializeTempList(tempModifierList, this.m_CityModifierData[prefabRef.m_Prefab]);
              }
              else
              {
                // ISSUE: reference to a compiler-generated method
                CityModifierUpdateSystem.InitializeTempList(tempModifierList);
              }
              if (bufferAccessor2.Length != 0)
              {
                // ISSUE: reference to a compiler-generated method
                this.AddToTempList(tempModifierList, bufferAccessor2[index6]);
              }
              for (int index7 = 0; index7 < tempModifierList.Length; ++index7)
              {
                CityModifierData tempModifier = tempModifierList[index7];
                // ISSUE: reference to a compiler-generated method
                CityModifierUpdateSystem.CityModifierRefreshData.AddModifier(modifiers, tempModifier, tempModifier.m_Range.max);
              }
            }
          }
        }
      }

      private void AddToTempList(
        NativeList<CityModifierData> tempModifierList,
        DynamicBuffer<InstalledUpgrade> upgrades)
      {
        for (int index = 0; index < upgrades.Length; ++index)
        {
          InstalledUpgrade upgrade = upgrades[index];
          DynamicBuffer<CityModifierData> bufferData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (!BuildingUtils.CheckOption(upgrade, BuildingOption.Inactive) && this.m_CityModifierData.TryGetBuffer(this.m_PrefabRefData[upgrade.m_Upgrade].m_Prefab, out bufferData))
          {
            // ISSUE: reference to a compiler-generated method
            CityModifierUpdateSystem.AddToTempList(tempModifierList, bufferData);
          }
        }
      }

      private static void AddModifier(
        DynamicBuffer<CityModifier> modifiers,
        CityModifierData modifierData,
        float delta)
      {
        while ((CityModifierType) modifiers.Length <= modifierData.m_Type)
          modifiers.Add(new CityModifier());
        CityModifier modifier = modifiers[(int) modifierData.m_Type];
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
      public ComponentTypeHandle<Game.City.City> __Game_City_City_RW_ComponentTypeHandle;
      public BufferTypeHandle<CityModifier> __Game_City_CityModifier_RW_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Policies_Policy_RO_BufferTypeHandle = state.GetBufferTypeHandle<Policy>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_City_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Game.City.City>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_City_CityModifier_RW_BufferTypeHandle = state.GetBufferTypeHandle<CityModifier>();
      }
    }
  }
}
