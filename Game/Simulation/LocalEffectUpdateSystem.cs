// Decompiled with JetBrains decompiler
// Type: Game.Simulation.LocalEffectUpdateSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Buildings;
using Game.Common;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

#nullable disable
namespace Game.Simulation
{
  [CompilerGenerated]
  public class LocalEffectUpdateSystem : GameSystemBase
  {
    private LocalEffectSystem m_LocalEffectSystem;
    private EntityQuery m_EffectProviderQuery;
    private LocalEffectUpdateSystem.TypeHandle __TypeHandle;

    public override int GetUpdateInterval(SystemUpdatePhase phase) => 256;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_LocalEffectSystem = this.World.GetOrCreateSystemManaged<LocalEffectSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_EffectProviderQuery = this.GetEntityQuery(ComponentType.ReadOnly<LocalEffectProvider>(), ComponentType.ReadOnly<Efficiency>(), ComponentType.Exclude<Signature>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Destroyed>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_EffectProviderQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_LocalModifierData_RO_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
      JobHandle dependencies;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
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
      JobHandle jobHandle = new LocalEffectUpdateSystem.UpdateLocalEffectsJob()
      {
        m_SearchTree = this.m_LocalEffectSystem.GetSearchTree(false, out dependencies),
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_BuildingEfficiencyType = this.__TypeHandle.__Game_Buildings_Efficiency_RO_BufferTypeHandle,
        m_TransformType = this.__TypeHandle.__Game_Objects_Transform_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_InstalledUpgradeType = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_LocalModifierData = this.__TypeHandle.__Game_Prefabs_LocalModifierData_RO_BufferLookup
      }.Schedule<LocalEffectUpdateSystem.UpdateLocalEffectsJob>(this.m_EffectProviderQuery, JobHandle.CombineDependencies(this.Dependency, dependencies));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_LocalEffectSystem.AddLocalEffectWriter(jobHandle);
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
    public LocalEffectUpdateSystem()
    {
    }

    [BurstCompile]
    private struct UpdateLocalEffectsJob : IJobChunk
    {
      public NativeQuadTree<LocalEffectSystem.EffectItem, LocalEffectSystem.EffectBounds> m_SearchTree;
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> m_BuildingEfficiencyType;
      [ReadOnly]
      public ComponentTypeHandle<Transform> m_TransformType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> m_InstalledUpgradeType;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public BufferLookup<LocalModifierData> m_LocalModifierData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<Efficiency> bufferAccessor1 = chunk.GetBufferAccessor<Efficiency>(ref this.m_BuildingEfficiencyType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Transform> nativeArray2 = chunk.GetNativeArray<Transform>(ref this.m_TransformType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> nativeArray3 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
        // ISSUE: reference to a compiler-generated field
        BufferAccessor<InstalledUpgrade> bufferAccessor2 = chunk.GetBufferAccessor<InstalledUpgrade>(ref this.m_InstalledUpgradeType);
        NativeList<LocalModifierData> tempModifierList = new NativeList<LocalModifierData>(10, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        for (int index1 = 0; index1 < chunk.Count; ++index1)
        {
          Entity provider = nativeArray1[index1];
          Transform transform = nativeArray2[index1];
          PrefabRef prefabRef = nativeArray3[index1];
          float efficiency = BuildingUtils.GetEfficiency(bufferAccessor1[index1]);
          // ISSUE: reference to a compiler-generated method
          this.InitializeTempList(tempModifierList, prefabRef.m_Prefab);
          if (bufferAccessor2.Length != 0)
          {
            // ISSUE: reference to a compiler-generated method
            this.AddToTempList(tempModifierList, bufferAccessor2[index1]);
          }
          for (int index2 = 0; index2 < tempModifierList.Length; ++index2)
          {
            LocalModifierData localModifier = tempModifierList[index2];
            // ISSUE: object of a compiler-generated type is created
            // ISSUE: variable of a compiler-generated type
            LocalEffectSystem.EffectItem effectItem = new LocalEffectSystem.EffectItem(provider, localModifier.m_Type);
            // ISSUE: variable of a compiler-generated type
            LocalEffectSystem.EffectBounds effectBounds;
            // ISSUE: reference to a compiler-generated method
            if (LocalEffectSystem.GetEffectBounds(transform, efficiency, localModifier, out effectBounds))
            {
              // ISSUE: variable of a compiler-generated type
              LocalEffectSystem.EffectBounds bounds;
              // ISSUE: reference to a compiler-generated field
              if (this.m_SearchTree.TryGet(effectItem, out bounds))
              {
                // ISSUE: reference to a compiler-generated method
                if (!effectBounds.Equals(bounds))
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_SearchTree.Update(effectItem, effectBounds);
                }
              }
              else
              {
                // ISSUE: reference to a compiler-generated field
                this.m_SearchTree.Add(effectItem, effectBounds);
              }
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              this.m_SearchTree.TryRemove(effectItem);
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
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public BufferTypeHandle<Efficiency> __Game_Buildings_Efficiency_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Transform> __Game_Objects_Transform_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public BufferLookup<LocalModifierData> __Game_Prefabs_LocalModifierData_RO_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Efficiency_RO_BufferTypeHandle = state.GetBufferTypeHandle<Efficiency>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Objects_Transform_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Transform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_LocalModifierData_RO_BufferLookup = state.GetBufferLookup<LocalModifierData>(true);
      }
    }
  }
}
