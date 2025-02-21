// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.UIInitializeSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Common;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Prefabs
{
  [CompilerGenerated]
  public class UIInitializeSystem : GameSystemBase
  {
    private EntityQuery m_PrefabQuery;
    private PrefabSystem m_PrefabSystem;
    private EntityQuery m_PolicyQuery;
    private UIInitializeSystem.TypeHandle __TypeHandle;

    public IEnumerable<PolicyPrefab> policies
    {
      get
      {
        if (!this.m_PolicyQuery.IsEmptyIgnoreFilter)
        {
          NativeArray<PrefabData> prefabs = this.m_PolicyQuery.ToComponentDataArray<PrefabData>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
          for (int i = 0; i < prefabs.Length; ++i)
            yield return this.m_PrefabSystem.GetPrefab<PolicyPrefab>(prefabs[i]);
          prefabs.Dispose();
          prefabs = new NativeArray<PrefabData>();
        }
      }
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<PrefabData>(),
          ComponentType.ReadOnly<Deleted>()
        },
        Any = new ComponentType[2]
        {
          ComponentType.ReadOnly<UIObjectData>(),
          ComponentType.ReadOnly<UIAssetCategoryData>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.m_PolicyQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<PrefabData>(),
          ComponentType.ReadOnly<PolicyData>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_PrefabQuery);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_PrefabQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      try
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Unity_Entities_Entity_TypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        EntityTypeHandle entityTypeHandle = this.__TypeHandle.__Unity_Entities_Entity_TypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_UIObjectData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<UIObjectData> componentTypeHandle1 = this.__TypeHandle.__Game_Prefabs_UIObjectData_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_UIAssetCategoryData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<UIAssetCategoryData> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_UIAssetCategoryData_RO_ComponentTypeHandle;
        this.CompleteDependency();
        for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
        {
          ArchetypeChunk archetypeChunk = archetypeChunkArray[index1];
          NativeArray<Entity> nativeArray1 = archetypeChunk.GetNativeArray(entityTypeHandle);
          NativeArray<UIObjectData> nativeArray2 = archetypeChunk.GetNativeArray<UIObjectData>(ref componentTypeHandle1);
          NativeArray<UIAssetCategoryData> nativeArray3 = archetypeChunk.GetNativeArray<UIAssetCategoryData>(ref componentTypeHandle2);
          for (int index2 = 0; index2 < nativeArray2.Length; ++index2)
          {
            Entity entity = nativeArray1[index2];
            UIObjectData uiObjectData = nativeArray2[index2];
            DynamicBuffer<UIGroupElement> buffer1;
            if (this.EntityManager.TryGetBuffer<UIGroupElement>(uiObjectData.m_Group, false, out buffer1))
            {
              // ISSUE: reference to a compiler-generated method
              this.RemoveFrom(entity, buffer1);
            }
            DynamicBuffer<UnlockRequirement> buffer2;
            if (this.EntityManager.TryGetBuffer<UnlockRequirement>(uiObjectData.m_Group, false, out buffer2))
            {
              // ISSUE: reference to a compiler-generated method
              this.RemoveFrom(entity, buffer2);
            }
          }
          for (int index3 = 0; index3 < nativeArray3.Length; ++index3)
          {
            Entity entity = nativeArray1[index3];
            UIAssetCategoryData assetCategoryData = nativeArray3[index3];
            DynamicBuffer<UIGroupElement> buffer3;
            if (this.EntityManager.TryGetBuffer<UIGroupElement>(assetCategoryData.m_Menu, false, out buffer3))
            {
              // ISSUE: reference to a compiler-generated method
              this.RemoveFrom(entity, buffer3);
            }
            DynamicBuffer<UnlockRequirement> buffer4;
            if (this.EntityManager.TryGetBuffer<UnlockRequirement>(assetCategoryData.m_Menu, false, out buffer4))
            {
              // ISSUE: reference to a compiler-generated method
              this.RemoveFrom(entity, buffer4);
            }
          }
        }
      }
      finally
      {
        archetypeChunkArray.Dispose(this.Dependency);
      }
    }

    private void RemoveFrom(Entity entity, DynamicBuffer<UIGroupElement> uiGroupElements)
    {
      for (int index = 0; index < uiGroupElements.Length; ++index)
      {
        if (uiGroupElements[index].m_Prefab == entity)
        {
          uiGroupElements.RemoveAtSwapBack(index);
          break;
        }
      }
    }

    private void RemoveFrom(
      Entity entity,
      DynamicBuffer<UnlockRequirement> unlockRequirements)
    {
      for (int index = 0; index < unlockRequirements.Length; ++index)
      {
        if (unlockRequirements[index].m_Prefab == entity)
        {
          unlockRequirements.RemoveAtSwapBack(index);
          break;
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
    public UIInitializeSystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public EntityTypeHandle __Unity_Entities_Entity_TypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<UIObjectData> __Game_Prefabs_UIObjectData_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<UIAssetCategoryData> __Game_Prefabs_UIAssetCategoryData_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Unity_Entities_Entity_TypeHandle = state.GetEntityTypeHandle();
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UIObjectData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<UIObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UIAssetCategoryData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<UIAssetCategoryData>(true);
      }
    }
  }
}
