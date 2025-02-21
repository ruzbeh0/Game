// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.UIHighlightSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs;
using Game.Serialization;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Collections;
using Unity.Entities;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class UIHighlightSystem : GameSystemBase, IPreDeserialize
  {
    private EndFrameBarrier m_EndFrameBarrier;
    private EntityQuery m_UnlockedPrefabQuery;
    private bool m_SkipUpdate = true;
    private UIHighlightSystem.TypeHandle __TypeHandle;

    [UnityEngine.Scripting.Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier = this.World.GetOrCreateSystemManaged<EndFrameBarrier>();
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockedPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<Unlock>());
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_UnlockedPrefabQuery);
    }

    [UnityEngine.Scripting.Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_SkipUpdate)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SkipUpdate = false;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_UIToolbarGroupData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_UIAssetCategoryData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_UIObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_Unlock_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
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
        // ISSUE: variable of a compiler-generated type
        UIHighlightSystem.HighlightJob jobData = new UIHighlightSystem.HighlightJob()
        {
          m_UnlockType = this.__TypeHandle.__Game_Prefabs_Unlock_RO_ComponentTypeHandle,
          m_ObjectDatas = this.__TypeHandle.__Game_Prefabs_UIObjectData_RO_ComponentLookup,
          m_AssetCategories = this.__TypeHandle.__Game_Prefabs_UIAssetCategoryData_RO_ComponentLookup,
          m_ToolbarGroups = this.__TypeHandle.__Game_Prefabs_UIToolbarGroupData_RO_ComponentLookup,
          m_CommandBuffer = this.m_EndFrameBarrier.CreateCommandBuffer()
        };
        // ISSUE: reference to a compiler-generated field
        this.Dependency = jobData.Schedule<UIHighlightSystem.HighlightJob>(this.m_UnlockedPrefabQuery, this.Dependency);
        // ISSUE: reference to a compiler-generated field
        this.m_EndFrameBarrier.AddJobHandleForProducer(this.Dependency);
      }
    }

    public void PreDeserialize(Colossal.Serialization.Entities.Context context)
    {
      using (EntityQuery entityQuery = this.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<UIHighlight>()))
        this.EntityManager.RemoveComponent<UIHighlight>(entityQuery);
      // ISSUE: reference to a compiler-generated field
      this.m_SkipUpdate = true;
    }

    public void SkipUpdate() => this.m_SkipUpdate = true;

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
    public UIHighlightSystem()
    {
    }

    [BurstCompile]
    private struct HighlightJob : IJobChunk
    {
      [ReadOnly]
      public ComponentTypeHandle<Unlock> m_UnlockType;
      [ReadOnly]
      public ComponentLookup<UIObjectData> m_ObjectDatas;
      [ReadOnly]
      public ComponentLookup<UIAssetCategoryData> m_AssetCategories;
      [ReadOnly]
      public ComponentLookup<UIToolbarGroupData> m_ToolbarGroups;
      public EntityCommandBuffer m_CommandBuffer;

      public void Execute(
        in ArchetypeChunk chunk,
        int unfilteredChunkIndex,
        bool useEnabledMask,
        in v128 chunkEnabledMask)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<Unlock> nativeArray = chunk.GetNativeArray<Unlock>(ref this.m_UnlockType);
        for (int index = 0; index < chunk.Count; ++index)
        {
          Unlock unlock = nativeArray[index];
          UIObjectData componentData1;
          // ISSUE: reference to a compiler-generated field
          if (this.m_ObjectDatas.TryGetComponent(unlock.m_Prefab, out componentData1))
          {
            Entity group = componentData1.m_Group;
            UIAssetCategoryData componentData2;
            // ISSUE: reference to a compiler-generated field
            if (this.m_AssetCategories.TryGetComponent(group, out componentData2))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<UIHighlight>(unlock.m_Prefab);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<UIHighlight>(group);
              // ISSUE: reference to a compiler-generated field
              this.m_CommandBuffer.AddComponent<UIHighlight>(componentData2.m_Menu);
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.m_ToolbarGroups.HasComponent(group))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_CommandBuffer.AddComponent<UIHighlight>(unlock.m_Prefab);
              }
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
      public ComponentTypeHandle<Unlock> __Game_Prefabs_Unlock_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentLookup<UIObjectData> __Game_Prefabs_UIObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<UIAssetCategoryData> __Game_Prefabs_UIAssetCategoryData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<UIToolbarGroupData> __Game_Prefabs_UIToolbarGroupData_RO_ComponentLookup;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_Unlock_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Unlock>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UIObjectData_RO_ComponentLookup = state.GetComponentLookup<UIObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UIAssetCategoryData_RO_ComponentLookup = state.GetComponentLookup<UIAssetCategoryData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_UIToolbarGroupData_RO_ComponentLookup = state.GetComponentLookup<UIToolbarGroupData>(true);
      }
    }
  }
}
