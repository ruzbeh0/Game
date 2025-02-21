// Decompiled with JetBrains decompiler
// Type: Game.Notifications.IconDeletedSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Notifications
{
  [CompilerGenerated]
  public class IconDeletedSystem : GameSystemBase
  {
    private IconCommandSystem m_IconCommandSystem;
    private EntityQuery m_DeletedQuery;
    private IconDeletedSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_IconCommandSystem = this.World.GetOrCreateSystemManaged<IconCommandSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Icon>(),
          ComponentType.ReadOnly<IconElement>()
        }
      }, new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Updated>(),
          ComponentType.ReadOnly<IconElement>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_DeletedQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_IconElement_RO_BufferTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Target_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      // ISSUE: reference to a compiler-generated method
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      IconDeletedSystem.IconDeletedJob jobData = new IconDeletedSystem.IconDeletedJob()
      {
        m_EntityType = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle,
        m_DeletedType = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentTypeHandle,
        m_IconType = this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentTypeHandle,
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_TargetType = this.__TypeHandle.__Game_Common_Target_RO_ComponentTypeHandle,
        m_PrefabRefType = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle,
        m_IconElementType = this.__TypeHandle.__Game_Notifications_IconElement_RO_BufferTypeHandle,
        m_DeletedData = this.__TypeHandle.__Game_Common_Deleted_RO_ComponentLookup,
        m_IconData = this.__TypeHandle.__Game_Notifications_Icon_RO_ComponentLookup,
        m_TargetData = this.__TypeHandle.__Game_Common_Target_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_WarningCommandBuffer = this.m_IconCommandSystem.CreateCommandBuffer()
      };
      // ISSUE: reference to a compiler-generated field
      this.Dependency = jobData.ScheduleParallel<IconDeletedSystem.IconDeletedJob>(this.m_DeletedQuery, this.Dependency);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_IconCommandSystem.AddCommandBufferWriter(this.Dependency);
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
    public IconDeletedSystem()
    {
    }

    [BurstCompile]
    private struct IconDeletedJob : IJobChunk
    {
      [ReadOnly]
      public EntityTypeHandle m_EntityType;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> m_DeletedType;
      [ReadOnly]
      public ComponentTypeHandle<Icon> m_IconType;
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Target> m_TargetType;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> m_PrefabRefType;
      [ReadOnly]
      public BufferTypeHandle<IconElement> m_IconElementType;
      [ReadOnly]
      public ComponentLookup<Deleted> m_DeletedData;
      [ReadOnly]
      public ComponentLookup<Icon> m_IconData;
      [ReadOnly]
      public ComponentLookup<Target> m_TargetData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      public IconCommandBuffer m_WarningCommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Icon> nativeArray1 = chunk.GetNativeArray<Icon>(ref this.m_IconType);
        if (nativeArray1.Length != 0)
        {
          // ISSUE: reference to a compiler-generated field
          NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
          if (nativeArray2.Length == 0)
            return;
          // ISSUE: reference to a compiler-generated field
          NativeArray<Target> nativeArray3 = chunk.GetNativeArray<Target>(ref this.m_TargetType);
          // ISSUE: reference to a compiler-generated field
          NativeArray<PrefabRef> nativeArray4 = chunk.GetNativeArray<PrefabRef>(ref this.m_PrefabRefType);
          if (nativeArray3.Length != 0)
          {
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              Owner owner = nativeArray2[index];
              // ISSUE: reference to a compiler-generated field
              if (!this.m_DeletedData.HasComponent(owner.m_Owner))
              {
                Icon icon = nativeArray1[index];
                Target target = nativeArray3[index];
                PrefabRef prefabRef = nativeArray4[index];
                // ISSUE: reference to a compiler-generated field
                this.m_WarningCommandBuffer.Remove(owner.m_Owner, prefabRef.m_Prefab, target.m_Target, icon.m_Flags & IconFlags.SecondaryLocation);
              }
            }
          }
          else
          {
            for (int index = 0; index < nativeArray1.Length; ++index)
            {
              Owner owner = nativeArray2[index];
              // ISSUE: reference to a compiler-generated field
              if (!this.m_DeletedData.HasComponent(owner.m_Owner))
              {
                Icon icon = nativeArray1[index];
                PrefabRef prefabRef = nativeArray4[index];
                // ISSUE: reference to a compiler-generated field
                this.m_WarningCommandBuffer.Remove(owner.m_Owner, prefabRef.m_Prefab, Entity.Null, icon.m_Flags & IconFlags.SecondaryLocation);
              }
            }
          }
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          BufferAccessor<IconElement> bufferAccessor = chunk.GetBufferAccessor<IconElement>(ref this.m_IconElementType);
          if (bufferAccessor.Length == 0)
            return;
          // ISSUE: reference to a compiler-generated field
          NativeArray<Entity> nativeArray5 = chunk.GetNativeArray(this.m_EntityType);
          // ISSUE: reference to a compiler-generated field
          bool flag = chunk.Has<Deleted>(ref this.m_DeletedType);
          for (int index1 = 0; index1 < bufferAccessor.Length; ++index1)
          {
            Entity owner = nativeArray5[index1];
            DynamicBuffer<IconElement> dynamicBuffer = bufferAccessor[index1];
            for (int index2 = 0; index2 < dynamicBuffer.Length; ++index2)
            {
              Entity icon1 = dynamicBuffer[index2].m_Icon;
              // ISSUE: reference to a compiler-generated field
              if (!this.m_DeletedData.HasComponent(icon1))
              {
                // ISSUE: reference to a compiler-generated field
                Icon icon2 = this.m_IconData[icon1];
                // ISSUE: reference to a compiler-generated field
                PrefabRef prefabRef = this.m_PrefabRefData[icon1];
                // ISSUE: reference to a compiler-generated field
                if (this.m_TargetData.HasComponent(icon1))
                {
                  // ISSUE: reference to a compiler-generated field
                  Target target = this.m_TargetData[icon1];
                  // ISSUE: reference to a compiler-generated field
                  if ((flag ? 1 : (this.m_DeletedData.HasComponent(target.m_Target) ? 1 : 0)) != 0)
                  {
                    // ISSUE: reference to a compiler-generated field
                    this.m_WarningCommandBuffer.Remove(owner, prefabRef.m_Prefab, target.m_Target, icon2.m_Flags & IconFlags.SecondaryLocation);
                  }
                }
                else if (flag)
                {
                  // ISSUE: reference to a compiler-generated field
                  this.m_WarningCommandBuffer.Remove(owner, prefabRef.m_Prefab, Entity.Null, icon2.m_Flags & IconFlags.SecondaryLocation);
                }
              }
            }
            if (!flag)
            {
              // ISSUE: reference to a compiler-generated field
              this.m_WarningCommandBuffer.Update(owner);
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
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Deleted> __Game_Common_Deleted_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Icon> __Game_Notifications_Icon_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Target> __Game_Common_Target_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
      [ReadOnly]
      public BufferTypeHandle<IconElement> __Game_Notifications_IconElement_RO_BufferTypeHandle;
      [ReadOnly]
      public ComponentLookup<Deleted> __Game_Common_Deleted_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Icon> __Game_Notifications_Icon_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<Target> __Game_Common_Target_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_Icon_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Icon>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Target>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_IconElement_RO_BufferTypeHandle = state.GetBufferTypeHandle<IconElement>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Deleted_RO_ComponentLookup = state.GetComponentLookup<Deleted>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_Icon_RO_ComponentLookup = state.GetComponentLookup<Icon>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Target_RO_ComponentLookup = state.GetComponentLookup<Target>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
      }
    }
  }
}
