// Decompiled with JetBrains decompiler
// Type: Game.Rendering.EffectRangeRenderSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.Common;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class EffectRangeRenderSystem : GameSystemBase
  {
    private EntityQuery m_ProviderQuery;
    private EntityQuery m_InfomodeQuery;
    private OverlayRenderSystem m_OverlayRenderSystem;
    private EffectRangeRenderSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_OverlayRenderSystem = this.World.GetOrCreateSystemManaged<OverlayRenderSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ProviderQuery = this.GetEntityQuery(ComponentType.ReadOnly<LocalEffectProvider>(), ComponentType.Exclude<Hidden>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Destroyed>());
      // ISSUE: reference to a compiler-generated field
      this.m_InfomodeQuery = this.GetEntityQuery(ComponentType.ReadOnly<InfomodeActive>(), ComponentType.ReadOnly<InfoviewLocalEffectData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_ProviderQuery);
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_InfomodeQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      JobHandle outJobHandle;
      // ISSUE: reference to a compiler-generated field
      NativeList<ArchetypeChunk> archetypeChunkListAsync = this.m_InfomodeQuery.ToArchetypeChunkListAsync((AllocatorManager.AllocatorHandle) Allocator.TempJob, out outJobHandle);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LocalModifierData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_FirewatchTower_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_InfoviewLocalEffectData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_FirewatchTower_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
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
      // ISSUE: reference to a compiler-generated field
      JobHandle jobHandle = new EffectRangeRenderSystem.EffectRangeRenderJob()
      {
        m_InfomodeChunks = archetypeChunkListAsync,
        m_OverlayBuffer = this.m_OverlayRenderSystem.GetBuffer(out dependencies),
        m_BuildingEfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_FirewatchTowerType = this.__TypeHandle.__Game_Buildings_FirewatchTower_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_InfoviewLocalEffectType = this.__TypeHandle.__Game_Prefabs_InfoviewLocalEffectData_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_FirewatchTowerData = this.__TypeHandle.__Game_Buildings_FirewatchTower_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_Efficiencies = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferLookup,
        m_LocalModifierData = this.__TypeHandle.__Game_Prefabs_LocalModifierData_RO_BufferLookup
      }.Schedule<EffectRangeRenderSystem.EffectRangeRenderJob>(this.m_ProviderQuery, JobHandle.CombineDependencies(this.Dependency, outJobHandle, dependencies));
      archetypeChunkListAsync.Dispose(jobHandle);
      // ISSUE: reference to a compiler-generated field
      this.m_OverlayRenderSystem.AddBufferWriter(jobHandle);
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
    public EffectRangeRenderSystem()
    {
    }

    [BurstCompile]
    private struct EffectRangeRenderJob : IJobChunk
    {
      [ReadOnly]
      public NativeList<ArchetypeChunk> m_InfomodeChunks;
      public OverlayRenderSystem.Buffer m_OverlayBuffer;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> m_BuildingEfficiencyType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.FirewatchTower> m_FirewatchTowerType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewLocalEffectData> m_InfoviewLocalEffectType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.FirewatchTower> m_FirewatchTowerData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public BufferLookup<Efficiency> m_Efficiencies;
      [ReadOnly]
      public BufferLookup<LocalModifierData> m_LocalModifierData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        NativeList<LocalModifierData> tempModifierList = new NativeList<LocalModifierData>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        // ISSUE: reference to a compiler-generated method
        uint modifierTypes = this.GetModifierTypes();
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Objects.Transform> nativeArray1 = chunk.GetNativeArray<Game.Objects.Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Game.Buildings.FirewatchTower> nativeArray2 = chunk.GetNativeArray<Game.Buildings.FirewatchTower>(ref this.m_FirewatchTowerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray3 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor1 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor2 = chunk.GetBufferAccessor<Efficiency>(ref this.m_BuildingEfficiencyType);
        for (int index1 = 0; index1 < nativeArray4.Length; ++index1)
        {
          PrefabRef prefabRef = nativeArray4[index1];
          // ISSUE: reference to a compiler-generated method
          this.InitializeTempList(tempModifierList, prefabRef.m_Prefab);
          if (bufferAccessor1.Length != 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.AddToTempList(tempModifierList, bufferAccessor1[index1]);
          }
          for (int index2 = 0; index2 < tempModifierList.Length; ++index2)
          {
            LocalModifierData localModifier = tempModifierList[index2];
            if ((1 << (int) (localModifier.m_Type & (LocalModifierType) 31) & (int) modifierTypes) != 0)
            {
              Game.Objects.Transform transform = nativeArray1[index1];
              Temp temp;
              if (CollectionUtils.TryGet<Temp>(nativeArray3, index1, out temp))
              {
                Game.Buildings.FirewatchTower componentData;
                DynamicBuffer<Efficiency> bufferData;
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                if (this.m_FirewatchTowerData.TryGetComponent(temp.m_Original, out componentData) && (componentData.m_Flags & FirewatchTowerFlags.HasCoverage) == (FirewatchTowerFlags) 0 || !this.m_Efficiencies.TryGetBuffer(temp.m_Original, out bufferData))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CheckModifier(localModifier, transform);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CheckModifier(localModifier, BuildingUtils.GetEfficiency(bufferData), transform);
                }
              }
              else
              {
                Game.Buildings.FirewatchTower firewatchTower;
                DynamicBuffer<Efficiency> buffer;
                if (CollectionUtils.TryGet<Game.Buildings.FirewatchTower>(nativeArray2, index1, out firewatchTower) && (firewatchTower.m_Flags & FirewatchTowerFlags.HasCoverage) == (FirewatchTowerFlags) 0 || !CollectionUtils.TryGet<Efficiency>(bufferAccessor2, index1, out buffer))
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CheckModifier(localModifier, transform);
                }
                else
                {
                  // ISSUE: reference to a compiler-generated method
                  this.CheckModifier(localModifier, BuildingUtils.GetEfficiency(buffer), transform);
                }
              }
            }
          }
        }
        tempModifierList.Dispose();
      }

      private void InitializeTempList(NativeList<LocalModifierData> tempModifierList, Entity prefab)
      {
        DynamicBuffer<LocalModifierData> bufferData;
        // ISSUE: reference to a compiler-generated field
        if (this.m_LocalModifierData.TryGetBuffer(prefab, out bufferData))
        {
          // ISSUE: reference to a compiler-generated method
          LocalEffectSystem.InitializeTempList(tempModifierList, bufferData);
        }
        else
          tempModifierList.Clear();
      }

      private void AddToTempList(
        NativeList<LocalModifierData> tempModifierList,
        DynamicBuffer<InstalledUpgrade> upgrades)
      {
        for (int index = 0; index < upgrades.Length; ++index)
        {
          InstalledUpgrade upgrade = upgrades[index];
          DynamicBuffer<LocalModifierData> bufferData;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          if (this.m_LocalModifierData.TryGetBuffer(this.m_PrefabRefData[upgrade.m_Upgrade].m_Prefab, out bufferData))
          {
            // ISSUE: reference to a compiler-generated method
            LocalEffectSystem.AddToTempList(tempModifierList, bufferData, BuildingUtils.CheckOption(upgrade, BuildingOption.Inactive));
          }
        }
      }

      private uint GetModifierTypes()
      {
        uint modifierTypes = 0;
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_InfomodeChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<InfoviewLocalEffectData> nativeArray = this.m_InfomodeChunks[index1].GetNativeArray<InfoviewLocalEffectData>(ref this.m_InfoviewLocalEffectType);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            InfoviewLocalEffectData infoviewLocalEffectData = nativeArray[index2];
            modifierTypes |= 1U << (int) (infoviewLocalEffectData.m_Type & (LocalModifierType) 31);
          }
        }
        return modifierTypes;
      }

      private void CheckModifier(
        LocalModifierData localModifier,
        float efficiency,
        Game.Objects.Transform transform)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_InfomodeChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<InfoviewLocalEffectData> nativeArray = this.m_InfomodeChunks[index1].GetNativeArray<InfoviewLocalEffectData>(ref this.m_InfoviewLocalEffectType);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            InfoviewLocalEffectData infoviewLocalEffectData = nativeArray[index2];
            if (infoviewLocalEffectData.m_Type == localModifier.m_Type)
            {
              float3 float3 = math.forward(transform.m_Rotation);
              float num = math.lerp(localModifier.m_Radius.min, localModifier.m_Radius.max, math.sqrt(efficiency));
              UnityEngine.Color color = RenderingUtils.ToColor(infoviewLocalEffectData.m_Color);
              UnityEngine.Color fillColor = color with
              {
                a = 0.0f
              };
              // ISSUE: reference to a compiler-generated field
              this.m_OverlayBuffer.DrawCircle(color, fillColor, num * 0.02f, OverlayRenderSystem.StyleFlags.Projected, float3.xz, transform.m_Position, num * 2f);
            }
          }
        }
      }

      private void CheckModifier(LocalModifierData localModifier, Game.Objects.Transform transform)
      {
        // ISSUE: reference to a compiler-generated field
        for (int index1 = 0; index1 < this.m_InfomodeChunks.Length; ++index1)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          NativeArray<InfoviewLocalEffectData> nativeArray = this.m_InfomodeChunks[index1].GetNativeArray<InfoviewLocalEffectData>(ref this.m_InfoviewLocalEffectType);
          for (int index2 = 0; index2 < nativeArray.Length; ++index2)
          {
            InfoviewLocalEffectData infoviewLocalEffectData = nativeArray[index2];
            if (infoviewLocalEffectData.m_Type == localModifier.m_Type)
            {
              float3 float3 = math.forward(transform.m_Rotation);
              float max = localModifier.m_Radius.max;
              UnityEngine.Color color = RenderingUtils.ToColor(infoviewLocalEffectData.m_Color);
              UnityEngine.Color fillColor = color with
              {
                a = 0.0f
              };
              // ISSUE: reference to a compiler-generated field
              this.m_OverlayBuffer.DrawCircle(color, fillColor, max * 0.02f, OverlayRenderSystem.StyleFlags.Projected, float3.xz, transform.m_Position, max * 2f);
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

    private struct TypeHandle
    {
      [ReadOnly]
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Objects.Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Game.Buildings.FirewatchTower> __Game_Buildings_FirewatchTower_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<InfoviewLocalEffectData> __Game_Prefabs_InfoviewLocalEffectData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Game.Buildings.FirewatchTower> __Game_Buildings_FirewatchTower_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<Efficiency> __Game_Buildings_Efficiency_RO_BufferLookup;
      [ReadOnly]
      public BufferLookup<LocalModifierData> __Game_Prefabs_LocalModifierData_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Objects.Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_FirewatchTower_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Game.Buildings.FirewatchTower>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_InfoviewLocalEffectData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<InfoviewLocalEffectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_FirewatchTower_RO_ComponentLookup = state.GetComponentLookup<Game.Buildings.FirewatchTower>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferLookup = state.GetBufferLookup<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LocalModifierData_RO_BufferLookup = state.GetBufferLookup<LocalModifierData>(true);
      }
    }
  }
}
