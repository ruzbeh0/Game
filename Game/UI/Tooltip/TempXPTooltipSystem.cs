// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.TempXPTooltipSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Prefabs;
using Game.Tools;
using Game.UI.Widgets;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Tooltip
{
  [CompilerGenerated]
  public class TempXPTooltipSystem : TooltipSystemBase
  {
    private ToolSystem m_ToolSystem;
    private EntityQuery m_TempQuery;
    private EntityQuery m_LockedMilestoneQuery;
    private IntTooltip m_Tooltip;
    private TempXPTooltipSystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TempQuery = this.GetEntityQuery(ComponentType.ReadOnly<Temp>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_LockedMilestoneQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[3]
        {
          ComponentType.ReadOnly<PrefabData>(),
          ComponentType.ReadOnly<MilestoneData>(),
          ComponentType.ReadOnly<Locked>()
        },
        None = new ComponentType[2]
        {
          ComponentType.ReadOnly<Deleted>(),
          ComponentType.ReadOnly<Temp>()
        }
      });
      IntTooltip intTooltip = new IntTooltip();
      intTooltip.path = (PathSegment) "xp";
      intTooltip.icon = "Media/Game/Icons/Trophy.svg";
      intTooltip.unit = "xp";
      intTooltip.signed = true;
      intTooltip.color = TooltipColor.Success;
      // ISSUE: reference to a compiler-generated field
      this.m_Tooltip = intTooltip;
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TempQuery);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.actionMode.IsEditor() || this.m_LockedMilestoneQuery.IsEmpty)
        return;
      this.CompleteDependency();
      int num = 0;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<PlaceableObjectData> roComponentLookup1 = this.__TypeHandle.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_ServiceUpgradeData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<ServiceUpgradeData> roComponentLookup2 = this.__TypeHandle.__Game_Prefabs_ServiceUpgradeData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PlacedSignatureBuildingData_RO_ComponentLookup.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentLookup<PlacedSignatureBuildingData> roComponentLookup3 = this.__TypeHandle.__Game_Prefabs_PlacedSignatureBuildingData_RO_ComponentLookup;
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_TempQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Temp> componentTypeHandle1 = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<PrefabRef> componentTypeHandle2 = this.__TypeHandle.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;
        foreach (ArchetypeChunk archetypeChunk in archetypeChunkArray)
        {
          NativeArray<Temp> nativeArray1 = archetypeChunk.GetNativeArray<Temp>(ref componentTypeHandle1);
          NativeArray<PrefabRef> nativeArray2 = archetypeChunk.GetNativeArray<PrefabRef>(ref componentTypeHandle2);
          for (int index = 0; index < nativeArray1.Length; ++index)
          {
            Temp temp = nativeArray1[index];
            if ((temp.m_Flags & TempFlags.Create) != (TempFlags) 0)
            {
              Entity prefab = nativeArray2[index].m_Prefab;
              if (roComponentLookup1.HasComponent(prefab) && !roComponentLookup3.HasComponent(prefab) && temp.m_Original == Entity.Null)
                num += roComponentLookup1[prefab].m_XPReward;
              if (roComponentLookup2.HasComponent(prefab) && temp.m_Original == Entity.Null)
                num += roComponentLookup2[prefab].m_XPReward;
            }
          }
        }
      }
      if (num <= 0)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_Tooltip.value = num;
      // ISSUE: reference to a compiler-generated field
      this.AddMouseTooltip((IWidget) this.m_Tooltip);
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
    public TempXPTooltipSystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentLookup<PlaceableObjectData> __Game_Prefabs_PlaceableObjectData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<ServiceUpgradeData> __Game_Prefabs_ServiceUpgradeData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentLookup<PlacedSignatureBuildingData> __Game_Prefabs_PlacedSignatureBuildingData_RO_ComponentLookup;
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;
      [ReadOnly]
      public ComponentTypeHandle<PrefabRef> __Game_Prefabs_PrefabRef_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlaceableObjectData_RO_ComponentLookup = state.GetComponentLookup<PlaceableObjectData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_ServiceUpgradeData_RO_ComponentLookup = state.GetComponentLookup<ServiceUpgradeData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PlacedSignatureBuildingData_RO_ComponentLookup = state.GetComponentLookup<PlacedSignatureBuildingData>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabRef_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabRef>(true);
      }
    }
  }
}
