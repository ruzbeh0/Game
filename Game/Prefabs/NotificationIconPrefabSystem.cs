// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NotificationIconPrefabSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Rendering;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class NotificationIconPrefabSystem : GameSystemBase
  {
    private EntityQuery m_UpdatedQuery;
    private EntityQuery m_PrefabQuery;
    private PrefabSystem m_PrefabSystem;
    private NotificationIconRenderSystem m_NotificationIconRenderSystem;
    private NotificationIconPrefabSystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_NotificationIconRenderSystem = this.World.GetOrCreateSystemManaged<NotificationIconRenderSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpdatedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<NotificationIconData>(),
          ComponentType.ReadOnly<PrefabData>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<Created>(),
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<NotificationIconData>(), ComponentType.ReadOnly<PrefabData>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_UpdatedQuery);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_PrefabQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      int num = 1;
      try
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PrefabData> componentTypeHandle1 = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_NotificationIconDisplayData_RW_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<NotificationIconDisplayData> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_NotificationIconDisplayData_RW_ComponentTypeHandle;
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
          NativeArray<PrefabData> nativeArray1 = archetypeChunk.GetNativeArray<PrefabData>(ref componentTypeHandle1);
          NativeArray<NotificationIconDisplayData> nativeArray2 = archetypeChunk.GetNativeArray<NotificationIconDisplayData>(ref componentTypeHandle2);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            NotificationIconPrefab prefab = this.m_PrefabSystem.GetPrefab<NotificationIconPrefab>(nativeArray1[index2]);
            NotificationIconDisplayData notificationIconDisplayData = nativeArray2[index2] with
            {
              m_IconIndex = num++,
              m_MinParams = new float2(prefab.m_DisplaySize.min, prefab.m_PulsateAmplitude.min),
              m_MaxParams = new float2(prefab.m_DisplaySize.max, prefab.m_PulsateAmplitude.max)
            };
            notificationIconDisplayData.m_CategoryMask = math.select(2147483648U, notificationIconDisplayData.m_CategoryMask, notificationIconDisplayData.m_CategoryMask > 0U);
            nativeArray2[index2] = notificationIconDisplayData;
          }
        }
      }
      finally
      {
        archetypeChunkArray.Dispose();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NotificationIconRenderSystem.DisplayDataUpdated();
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
    public NotificationIconPrefabSystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      public ComponentTypeHandle<NotificationIconDisplayData> __Game_Prefabs_NotificationIconDisplayData_RW_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_NotificationIconDisplayData_RW_ComponentTypeHandle = state.GetComponentTypeHandle<NotificationIconDisplayData>();
      }
    }
  }
}
