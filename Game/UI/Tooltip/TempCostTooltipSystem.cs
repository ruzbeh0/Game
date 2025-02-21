// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.TempCostTooltipSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Common;
using Game.Simulation;
using Game.Tools;
using Game.UI.Localization;
using Game.UI.Widgets;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Tooltip
{
  [CompilerGenerated]
  public class TempCostTooltipSystem : TooltipSystemBase
  {
    private CitySystem m_CitySystem;
    private EntityQuery m_TempQuery;
    private IntTooltip m_Cost;
    private IntTooltip m_Refund;
    private TempCostTooltipSystem.TypeHandle __TypeHandle;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_CitySystem = this.World.GetOrCreateSystemManaged<CitySystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TempQuery = this.GetEntityQuery(ComponentType.ReadOnly<Temp>(), ComponentType.Exclude<Deleted>());
      IntTooltip intTooltip1 = new IntTooltip();
      intTooltip1.path = (PathSegment) "cost";
      intTooltip1.icon = "Media/Game/Icons/Money.svg";
      intTooltip1.unit = "money";
      // ISSUE: reference to a compiler-generated field
      this.m_Cost = intTooltip1;
      IntTooltip intTooltip2 = new IntTooltip();
      intTooltip2.path = (PathSegment) "refund";
      intTooltip2.icon = "Media/Game/Icons/Money.svg";
      intTooltip2.label = LocalizedString.Id("Tools.REFUND_AMOUNT_LABEL");
      intTooltip2.unit = "money";
      // ISSUE: reference to a compiler-generated field
      this.m_Refund = intTooltip2;
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TempQuery);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      this.CompleteDependency();
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_TempQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        int num = 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        ComponentTypeHandle<Temp> componentTypeHandle = this.__TypeHandle.__Game_Tools_Temp_RO_ComponentTypeHandle;
        foreach (ArchetypeChunk archetypeChunk in archetypeChunkArray)
        {
          foreach (Temp native in archetypeChunk.GetNativeArray<Temp>(ref componentTypeHandle))
          {
            if ((native.m_Flags & (TempFlags.Create | TempFlags.Delete | TempFlags.Modify | TempFlags.Replace | TempFlags.Upgrade | TempFlags.RemoveCost)) != (TempFlags) 0 && (native.m_Flags & TempFlags.Cancel) == (TempFlags) 0)
              num += native.m_Cost;
          }
        }
        if (num > 0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Cost.value = num;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_Cost.color = this.m_CitySystem.moneyAmount < num ? TooltipColor.Error : TooltipColor.Info;
          // ISSUE: reference to a compiler-generated field
          this.AddMouseTooltip((IWidget) this.m_Cost);
        }
        else
        {
          if (num >= 0)
            return;
          // ISSUE: reference to a compiler-generated field
          this.m_Refund.value = -num;
          // ISSUE: reference to a compiler-generated field
          this.AddMouseTooltip((IWidget) this.m_Refund);
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
    public TempCostTooltipSystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<Temp> __Game_Tools_Temp_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Tools_Temp_RO_ComponentTypeHandle = state.GetComponentTypeHandle<Temp>(true);
      }
    }
  }
}
