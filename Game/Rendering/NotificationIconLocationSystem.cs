// Decompiled with JetBrains decompiler
// Type: Game.Rendering.NotificationIconLocationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Game.Common;
using Game.Notifications;
using Game.Objects;
using Game.Prefabs;
using Game.Tools;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.Rendering
{
  [CompilerGenerated]
  public class NotificationIconLocationSystem : GameSystemBase
  {
    private EntityQuery m_IconQuery;
    private NotificationIconLocationSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_IconQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadWrite<Icon>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<DisallowCluster>(),
          ComponentType.ReadOnly<Game.Notifications.Animation>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_IconQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Notifications_Icon_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
      this.Dependency = new NotificationIconLocationSystem.NotificationIconMoveJob()
      {
        m_OwnerType = this.__TypeHandle.__Game_Common_Owner_RO_ComponentTypeHandle,
        m_TempType = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle,
        m_IconType = this.__TypeHandle.__Game_Notifications_Icon_RW_ComponentTypeHandle,
        m_InterpolatedTransformData = this.__TypeHandle.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup,
        m_PrefabRefData = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentLookup,
        m_ObjectGeometryData = this.__TypeHandle.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup
      }.ScheduleParallel<NotificationIconLocationSystem.NotificationIconMoveJob>(this.m_IconQuery, this.Dependency);
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
    public NotificationIconLocationSystem()
    {
    }

    [BurstCompile]
    private struct NotificationIconMoveJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Owner> m_OwnerType;
      [ReadOnly]
      public ComponentTypeHandle<Temp> m_TempType;
      public ComponentTypeHandle<Icon> m_IconType;
      [ReadOnly]
      public ComponentLookup<InterpolatedTransform> m_InterpolatedTransformData;
      [ReadOnly]
      public ComponentLookup<PrefabRef> m_PrefabRefData;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> m_ObjectGeometryData;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Icon> nativeArray1 = chunk.GetNativeArray<Icon>(ref this.m_IconType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Owner> nativeArray2 = chunk.GetNativeArray<Owner>(ref this.m_OwnerType);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Temp> nativeArray3 = chunk.GetNativeArray<Temp>(ref this.m_TempType);
        for (int index = 0; index < nativeArray2.Length; ++index)
        {
          Icon icon = nativeArray1[index];
          Owner owner = nativeArray2[index];
          // ISSUE: reference to a compiler-generated field
          if ((nativeArray3.Length == 0 || !(nativeArray3[index].m_Original != Entity.Null)) && this.m_InterpolatedTransformData.HasComponent(owner.m_Owner))
          {
            // ISSUE: reference to a compiler-generated field
            PrefabRef prefabRef = this.m_PrefabRefData[owner.m_Owner];
            // ISSUE: reference to a compiler-generated field
            Transform transform = this.m_InterpolatedTransformData[owner.m_Owner].ToTransform();
            // ISSUE: reference to a compiler-generated field
            Bounds3 bounds = ObjectUtils.CalculateBounds(transform.m_Position, transform.m_Rotation, this.m_ObjectGeometryData[prefabRef.m_Prefab]);
            icon.m_Location.xz = transform.m_Position.xz;
            icon.m_Location.y = bounds.max.y;
            nativeArray1[index] = icon;
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
      public ComponentTypeHandle<Owner> __Game_Common_Owner_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      public ComponentTypeHandle<Icon> __Game_Notifications_Icon_RW_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<InterpolatedTransform> __Game_Rendering_InterpolatedTransform_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ObjectGeometryData> __Game_Prefabs_ObjectGeometryData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Common_Owner_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Owner>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Notifications_Icon_RW_ComponentTypeHandle = state.GetComponentTypeHandle<Icon>();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Rendering_InterpolatedTransform_RO_ComponentLookup = state.GetComponentLookup<InterpolatedTransform>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentLookup = state.GetComponentLookup<PrefabRef>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ObjectGeometryData_RO_ComponentLookup = state.GetComponentLookup<ObjectGeometryData>(true);
      }
    }
  }
}
