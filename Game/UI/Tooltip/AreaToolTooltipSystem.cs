// Decompiled with JetBrains decompiler
// Type: Game.UI.Tooltip.AreaToolTooltipSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Areas;
using Game.Buildings;
using Game.Common;
using Game.Prefabs;
using Game.Tools;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Tooltip
{
  [CompilerGenerated]
  public class AreaToolTooltipSystem : TooltipSystemBase
  {
    private ToolSystem m_ToolSystem;
    private AreaToolSystem m_AreaTool;
    private EntityQuery m_TempQuery;
    private StringTooltip m_Tooltip;
    private CachedLocalizedStringBuilder<AreaToolSystem.Tooltip> m_StringBuilder;
    private IntTooltip m_Resources;
    private IntTooltip m_Storage;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaTool = this.World.GetOrCreateSystemManaged<AreaToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TempQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[2]
        {
          ComponentType.ReadOnly<Temp>(),
          ComponentType.ReadOnly<Area>()
        },
        None = new ComponentType[1]
        {
          ComponentType.ReadOnly<Deleted>()
        }
      });
      // ISSUE: reference to a compiler-generated field
      this.RequireForUpdate(this.m_TempQuery);
      StringTooltip stringTooltip = new StringTooltip();
      stringTooltip.path = (PathSegment) "areaTool";
      // ISSUE: reference to a compiler-generated field
      this.m_Tooltip = stringTooltip;
      // ISSUE: reference to a compiler-generated field
      this.m_StringBuilder = CachedLocalizedStringBuilder<AreaToolSystem.Tooltip>.Id((Func<AreaToolSystem.Tooltip, string>) (t => string.Format("Tools.INFO[{0:G}]", (object) t)));
      IntTooltip intTooltip1 = new IntTooltip();
      intTooltip1.path = (PathSegment) "areaToolResources";
      intTooltip1.label = LocalizedString.Id("Tools.RESOURCES_LABEL");
      intTooltip1.unit = "weight";
      // ISSUE: reference to a compiler-generated field
      this.m_Resources = intTooltip1;
      IntTooltip intTooltip2 = new IntTooltip();
      intTooltip2.path = (PathSegment) "areaToolStorage";
      intTooltip2.label = LocalizedString.Id("Tools.STORAGECAPACITY_LABEL");
      intTooltip2.unit = "weight";
      // ISSUE: reference to a compiler-generated field
      this.m_Storage = intTooltip2;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.activeTool != this.m_AreaTool || this.m_AreaTool.tooltip == AreaToolSystem.Tooltip.None)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Tooltip.value = this.m_StringBuilder[this.m_AreaTool.tooltip];
      // ISSUE: reference to a compiler-generated field
      this.AddMouseTooltip((IWidget) this.m_Tooltip);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      if (!AreaToolTooltipSystem.ShouldShowResources(this.m_AreaTool.tooltip))
        return;
      // ISSUE: reference to a compiler-generated field
      using (NativeArray<Entity> entityArray = this.m_TempQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        int num1 = 0;
        int num2 = 0;
        bool flag1 = false;
        bool flag2 = false;
        for (int index = 0; index < entityArray.Length; ++index)
        {
          Entity entity = entityArray[index];
          Extractor component1;
          if (this.EntityManager.TryGetComponent<Extractor>(entity, out component1))
          {
            num1 += (int) math.round(component1.m_ResourceAmount);
            flag1 = true;
          }
          EntityManager entityManager = this.EntityManager;
          if (entityManager.HasComponent<Storage>(entity))
          {
            entityManager = this.EntityManager;
            Geometry componentData1 = entityManager.GetComponentData<Geometry>(entity);
            entityManager = this.EntityManager;
            PrefabRef componentData2 = entityManager.GetComponentData<PrefabRef>(entity);
            entityManager = this.EntityManager;
            StorageAreaData componentData3 = entityManager.GetComponentData<StorageAreaData>(componentData2.m_Prefab);
            int storageCapacity = AreaUtils.CalculateStorageCapacity(componentData1, componentData3);
            Owner component2;
            PrefabRef component3;
            GarbageFacilityData component4;
            if (this.EntityManager.TryGetComponent<Owner>(entity, out component2) && this.EntityManager.TryGetComponent<PrefabRef>(component2.m_Owner, out component3) && this.EntityManager.TryGetComponent<GarbageFacilityData>(component3.m_Prefab, out component4))
            {
              DynamicBuffer<InstalledUpgrade> buffer;
              if (this.EntityManager.TryGetBuffer<InstalledUpgrade>(component2.m_Owner, true, out buffer))
                UpgradeUtils.CombineStats<GarbageFacilityData>(this.EntityManager, ref component4, buffer);
              storageCapacity += component4.m_GarbageCapacity;
            }
            num2 += storageCapacity;
            flag2 = true;
          }
        }
        if (flag1)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Resources.value = num1;
          // ISSUE: reference to a compiler-generated field
          this.AddMouseTooltip((IWidget) this.m_Resources);
        }
        if (!flag2)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_Storage.value = num2;
        // ISSUE: reference to a compiler-generated field
        this.AddMouseTooltip((IWidget) this.m_Storage);
      }
    }

    private static bool ShouldShowResources(AreaToolSystem.Tooltip tooltip)
    {
      switch (tooltip)
      {
        case AreaToolSystem.Tooltip.AddNode:
        case AreaToolSystem.Tooltip.InsertNode:
        case AreaToolSystem.Tooltip.MoveNode:
        case AreaToolSystem.Tooltip.MergeNodes:
        case AreaToolSystem.Tooltip.CompleteArea:
          return true;
        default:
          return false;
      }
    }

    [Preserve]
    public AreaToolTooltipSystem()
    {
    }
  }
}
