// Decompiled with JetBrains decompiler
// Type: Game.Buildings.ServiceUpgradeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Buildings
{
  [CompilerGenerated]
  public class ServiceUpgradeSystem : GameSystemBase
  {
    private EntityQuery m_UpgradeQuery;
    private EntityQuery m_UpgradePrefabQuery;
    private PrefabSystem m_PrefabSystem;
    private ModificationBarrier4 m_ModificationBarrier;
    private ServiceUpgradeSystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModificationBarrier = this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpgradeQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<ServiceUpgrade>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<InstalledUpgrade>(),
          ComponentType.ReadOnly<Deleted>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Temp>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_UpgradePrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<ServiceUpgradeData>(), ComponentType.ReadOnly<PrefabData>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_UpgradeQuery);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      EntityCommandBuffer commandBuffer = this.m_ModificationBarrier.CreateCommandBuffer();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<Deleted> componentTypeHandle1 = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<Owner> componentTypeHandle2 = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<ServiceUpgrade> componentTypeHandle3 = this.__TypeHandle.__Game_Buildings_ServiceUpgrade_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<PrefabRef> componentTypeHandle4 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      BufferTypeHandle<InstalledUpgrade> bufferTypeHandle = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_UpgradeQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        this.CompleteDependency();
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
          if (archetypeChunk.Has<ServiceUpgrade>(ref componentTypeHandle3))
          {
            NativeArray<Owner> nativeArray1 = archetypeChunk.GetNativeArray<Owner>(ref componentTypeHandle2);
            NativeArray<PrefabRef> nativeArray2 = archetypeChunk.GetNativeArray<PrefabRef>(ref componentTypeHandle4);
            if (archetypeChunk.Has<Deleted>(ref componentTypeHandle1))
            {
              for (int index2 = 0; index2 < nativeArray1.Length; ++index2)
              {
                // ISSUE: reference to a compiler-generated method
                this.UpgradeRemoved(commandBuffer, nativeArray1[index2], nativeArray2[index2]);
              }
            }
            else
            {
              for (int index3 = 0; index3 < nativeArray1.Length; ++index3)
              {
                // ISSUE: reference to a compiler-generated method
                this.UpgradeInstalled(commandBuffer, nativeArray1[index3], nativeArray2[index3]);
              }
            }
          }
          else
          {
            BufferAccessor<InstalledUpgrade> bufferAccessor = archetypeChunk.GetBufferAccessor<InstalledUpgrade>(ref bufferTypeHandle);
            for (int index4 = 0; index4 < bufferAccessor.Length; ++index4)
            {
              // ISSUE: reference to a compiler-generated method
              this.OwnerDeleted(commandBuffer, bufferAccessor[index4]);
            }
          }
        }
      }
    }

    private void UpgradeInstalled(
      EntityCommandBuffer commandBuffer,
      Owner owner,
      PrefabRef prefabRef)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      List<ComponentBase> components1 = this.m_PrefabSystem.GetPrefab<PrefabBase>(prefabRef).components;
      HashSet<ComponentType> components2 = new HashSet<ComponentType>();
      for (int index = 0; index < components1.Count; ++index)
      {
        if (components1[index] is IServiceUpgrade serviceUpgrade)
          serviceUpgrade.GetUpgradeComponents(components2);
      }
      foreach (ComponentType componentType in components2)
      {
        if (!this.EntityManager.HasComponent(owner.m_Owner, componentType))
          commandBuffer.AddComponent(owner.m_Owner, componentType);
      }
    }

    private void UpgradeRemoved(
      EntityCommandBuffer commandBuffer,
      Owner owner,
      PrefabRef prefabRef)
    {
      if (this.EntityManager.HasComponent<Deleted>(owner.m_Owner))
        return;
      HashSet<ComponentType> components1 = new HashSet<ComponentType>();
      HashSet<ComponentType> components2 = new HashSet<ComponentType>();
      PrefabBase prefab1;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (this.m_PrefabSystem.TryGetPrefab<PrefabBase>(prefabRef, out prefab1))
      {
        List<ComponentBase> components3 = prefab1.components;
        for (int index = 0; index < components3.Count; ++index)
        {
          if (components3[index] is IServiceUpgrade serviceUpgrade)
            serviceUpgrade.GetUpgradeComponents(components1);
        }
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabData> componentDataArray = this.m_UpgradePrefabQuery.ToComponentDataArray<PrefabData>((AllocatorManager.AllocatorHandle) Allocator.Temp);
        foreach (PrefabData prefabData in componentDataArray)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          List<ComponentBase> components4 = this.m_PrefabSystem.GetPrefab<PrefabBase>(prefabData).components;
          for (int index = 0; index < components4.Count; ++index)
          {
            if (components4[index] is IServiceUpgrade serviceUpgrade)
              serviceUpgrade.GetUpgradeComponents(components1);
          }
        }
        componentDataArray.Dispose();
      }
      PrefabRef componentData = this.EntityManager.GetComponentData<PrefabRef>(owner.m_Owner);
      DynamicBuffer<InstalledUpgrade> buffer = this.EntityManager.GetBuffer<InstalledUpgrade>(owner.m_Owner, true);
      PrefabBase prefab2;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (!this.m_PrefabSystem.TryGetPrefab<PrefabBase>(componentData, out prefab2))
        return;
      List<ComponentBase> components5 = prefab2.components;
      for (int index = 0; index < components5.Count; ++index)
        components5[index].GetArchetypeComponents(components2);
      foreach (InstalledUpgrade installedUpgrade in buffer)
      {
        PrefabBase prefab3;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        if (this.m_PrefabSystem.TryGetPrefab<PrefabBase>(this.EntityManager.GetComponentData<PrefabRef>(installedUpgrade.m_Upgrade), out prefab3))
        {
          List<ComponentBase> components6 = prefab3.components;
          for (int index = 0; index < components6.Count; ++index)
          {
            if (components6[index] is IServiceUpgrade serviceUpgrade)
              serviceUpgrade.GetUpgradeComponents(components2);
          }
        }
      }
      foreach (ComponentType componentType in components1)
      {
        if (!components2.Contains(componentType) && this.EntityManager.HasComponent(owner.m_Owner, componentType))
          commandBuffer.RemoveComponent(owner.m_Owner, componentType);
      }
    }

    private void OwnerDeleted(
      EntityCommandBuffer commandBuffer,
      DynamicBuffer<InstalledUpgrade> installedUpgrades)
    {
      for (int index = 0; index < installedUpgrades.Length; ++index)
      {
        Entity upgrade = installedUpgrades[index].m_Upgrade;
        EntityManager entityManager = this.EntityManager;
        if (!entityManager.HasComponent<Object>(upgrade))
        {
          entityManager = this.EntityManager;
          if (!entityManager.HasComponent<Deleted>(upgrade))
            commandBuffer.AddComponent<Deleted>(upgrade, new Deleted());
        }
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

    [Preserve]
    public ServiceUpgradeSystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<ServiceUpgrade> __Game_Buildings_ServiceUpgrade_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_ServiceUpgrade_RO_ComponentTypeHandle = state.GetComponentTypeHandle<ServiceUpgrade>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RO_BufferTypeHandle = state.GetBufferTypeHandle<InstalledUpgrade>(true);
      }
    }
  }
}
