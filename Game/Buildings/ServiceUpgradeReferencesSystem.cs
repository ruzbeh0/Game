// Decompiled with JetBrains decompiler
// Type: Game.Buildings.ServiceUpgradeReferencesSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using Game.Common;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  [CompilerGenerated]
  public class ServiceUpgradeReferencesSystem : GameSystemBase
  {
    private EntityQuery m_UpgradeQuery;
    private ServiceUpgradeReferencesSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_UpgradeQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<ServiceUpgrade>(),
          ComponentType.ReadOnly<Owner>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_UpgradeQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RW_BufferLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Extension_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
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
      this.Dependency = new ServiceUpgradeReferencesSystem.UpdateUpgradeReferencesJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_CreatedType = this.__TypeHandle.__Game_Common_Created_RO_ComponentTypeHandle,
        m_BuildingType = this.__TypeHandle.__Game_Buildings_Building_RO_ComponentTypeHandle,
        m_ExtensionType = this.__TypeHandle.__Game_Buildings_Extension_RO_ComponentTypeHandle,
        m_InstalledUpgrades = this.__TypeHandle.__Game_Buildings_InstalledUpgrade_RW_BufferLookup
      }.Schedule<ServiceUpgradeReferencesSystem.UpdateUpgradeReferencesJob>(this.m_UpgradeQuery, this.Dependency);
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
    public ServiceUpgradeReferencesSystem()
    {
    }

    [BurstCompile]
    private struct UpdateUpgradeReferencesJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Created> m_CreatedType;
      [ReadOnly]
      public ComponentTypeHandle<Building> m_BuildingType;
      [ReadOnly]
      public ComponentTypeHandle<Extension> m_ExtensionType;
      public BufferLookup<InstalledUpgrade> m_InstalledUpgrades;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> nativeArray1 = chunk.GetNativeArray(this.m_EntityType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        if (chunk.Has<Created>(ref this.m_CreatedType))
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Building> nativeArray3 = chunk.GetNativeArray<Building>(ref this.m_BuildingType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<Extension> nativeArray4 = chunk.GetNativeArray<Extension>(ref this.m_ExtensionType);
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Entity upgrade = nativeArray1[index];
            Owner owner = nativeArray2[index];
            Building building;
            Extension extension;
            if (!CollectionUtils.TryGet<Building>(nativeArray3, index, out building) && CollectionUtils.TryGet<Extension>(nativeArray4, index, out extension) && (extension.m_Flags & ExtensionFlags.Disabled) != ExtensionFlags.None)
              building.m_OptionMask = 2U;
            // ISSUE: reference to a compiler-generated field
            CollectionUtils.TryAddUniqueValue<InstalledUpgrade>(this.m_InstalledUpgrades[owner.m_Owner], new InstalledUpgrade(upgrade, building.m_OptionMask));
          }
        }
        else
        {
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Entity upgrade = nativeArray1[index];
            // ISSUE: reference to a compiler-generated field
            CollectionUtils.RemoveValue<InstalledUpgrade>(this.m_InstalledUpgrades[nativeArray2[index].m_Owner], new InstalledUpgrade(upgrade, 0U));
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
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Created> __Game_Common_Created_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Building> __Game_Buildings_Building_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Extension> __Game_Buildings_Extension_RO_ComponentTypeHandle;
      public BufferLookup<InstalledUpgrade> __Game_Buildings_InstalledUpgrade_RW_BufferLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Created_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Created>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Building_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Building>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_Extension_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Extension>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Buildings_InstalledUpgrade_RW_BufferLookup = state.GetBufferLookup<InstalledUpgrade>();
      }
    }
  }
}
