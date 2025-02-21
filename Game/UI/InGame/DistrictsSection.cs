// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.DistrictsSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Areas;
using Game.Common;
using Game.Prefabs;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class DistrictsSection : InfoSectionBase
  {
    private ToolSystem m_ToolSystem;
    private AreaToolSystem m_AreaToolSystem;
    private DefaultToolSystem m_DefaultToolSystem;
    private SelectionToolSystem m_SelectionToolSystem;
    private EntityQuery m_ConfigQuery;
    private EntityQuery m_DistrictQuery;
    private EntityQuery m_DistrictPrefabQuery;
    private EntityQuery m_DistrictModifiedQuery;
    private ValueBinding<bool> m_Selecting;

    protected override string group => nameof (DistrictsSection);

    private NativeList<Entity> districts { get; set; }

    private bool districtMissing { get; set; }

    protected override void Reset() => this.districts.Clear();

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AreaToolSystem = this.World.GetOrCreateSystemManaged<AreaToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_DefaultToolSystem = this.World.GetOrCreateSystemManaged<DefaultToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SelectionToolSystem = this.World.GetOrCreateSystemManaged<SelectionToolSystem>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem.EventToolChanged += (Action<ToolBaseSystem>) (tool => this.m_Selecting.Update(tool == this.m_SelectionToolSystem && this.m_SelectionToolSystem.selectionType == SelectionType.ServiceDistrict));
      this.districts = new NativeList<Entity>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<AreasConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_DistrictQuery = this.GetEntityQuery(ComponentType.ReadOnly<District>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_DistrictPrefabQuery = this.GetEntityQuery(ComponentType.ReadOnly<DistrictData>(), ComponentType.Exclude<Locked>());
      // ISSUE: reference to a compiler-generated field
      this.m_DistrictModifiedQuery = this.GetEntityQuery(new EntityQueryDesc()
      {
        All = new ComponentType[1]
        {
          ComponentType.ReadOnly<District>()
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
      });
      this.AddBinding((IBinding) new TriggerBinding<Entity>(this.group, "removeDistrict", (Action<Entity>) (district =>
      {
        DynamicBuffer<ServiceDistrict> buffer = this.EntityManager.GetBuffer<ServiceDistrict>(this.selectedEntity);
        bool flag = false;
        for (int index = 0; index < buffer.Length; ++index)
        {
          if (buffer[index].m_District == district)
          {
            buffer.RemoveAt(index);
            flag = true;
          }
        }
        if (!flag)
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_InfoUISystem.RequestUpdate();
      })));
      this.AddBinding((IBinding) new TriggerBinding(this.group, "toggleSelectionTool", (System.Action) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ToolSystem.activeTool == this.m_SelectionToolSystem)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_DefaultToolSystem;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_SelectionToolSystem.selectionType = SelectionType.ServiceDistrict;
          // ISSUE: reference to a compiler-generated field
          this.m_SelectionToolSystem.selectionOwner = this.selectedEntity;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_SelectionToolSystem;
        }
      })));
      this.AddBinding((IBinding) new TriggerBinding(this.group, "toggleDistrictTool", (System.Action) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ToolSystem.activeTool == this.m_AreaToolSystem)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_DefaultToolSystem;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_AreaToolSystem.prefab = this.m_PrefabSystem.GetPrefab<AreasConfigurationPrefab>(this.m_ConfigQuery.GetSingletonEntity()).m_DefaultDistrictPrefab;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_AreaToolSystem;
        }
      })));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_Selecting = new ValueBinding<bool>(this.group, "selecting", false)));
    }

    private void OnToolChanged(ToolBaseSystem tool)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Selecting.Update(tool == this.m_SelectionToolSystem && this.m_SelectionToolSystem.selectionType == SelectionType.ServiceDistrict);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      this.districts.Dispose();
      base.OnDestroy();
    }

    private bool Visible()
    {
      // ISSUE: reference to a compiler-generated field
      return this.EntityManager.HasComponent<ServiceDistrict>(this.selectedEntity) && !this.m_DistrictPrefabQuery.IsEmpty;
    }

    protected override void OnPreUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnPreUpdate();
      // ISSUE: reference to a compiler-generated field
      if (this.m_DistrictModifiedQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated method
      this.RequestUpdate();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated method
      this.visible = this.Visible();
      // ISSUE: reference to a compiler-generated field
      this.districtMissing = this.m_DistrictQuery.IsEmptyIgnoreFilter;
    }

    protected override void OnProcess()
    {
      DynamicBuffer<ServiceDistrict> buffer = this.EntityManager.GetBuffer<ServiceDistrict>(this.selectedEntity, true);
      for (int index = 0; index < buffer.Length; ++index)
        this.districts.Add(in buffer[index].m_District);
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("districtMissing");
      writer.Write(this.districtMissing);
      writer.PropertyName("districts");
      writer.ArrayBegin(this.districts.Length);
      for (int index = 0; index < this.districts.Length; ++index)
      {
        Entity district = this.districts[index];
        writer.TypeBegin("selectedInfo.District");
        writer.PropertyName("name");
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NameSystem.BindName(writer, district);
        writer.PropertyName("entity");
        writer.Write(district);
        writer.TypeEnd();
      }
      writer.ArrayEnd();
    }

    public void RemoveServiceDistrict(Entity district)
    {
      DynamicBuffer<ServiceDistrict> buffer = this.EntityManager.GetBuffer<ServiceDistrict>(this.selectedEntity);
      bool flag = false;
      for (int index = 0; index < buffer.Length; ++index)
      {
        if (buffer[index].m_District == district)
        {
          buffer.RemoveAt(index);
          flag = true;
        }
      }
      if (!flag)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.RequestUpdate();
    }

    private void ToggleSelectionTool()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.activeTool == this.m_SelectionToolSystem)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_DefaultToolSystem;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SelectionToolSystem.selectionType = SelectionType.ServiceDistrict;
        // ISSUE: reference to a compiler-generated field
        this.m_SelectionToolSystem.selectionOwner = this.selectedEntity;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_SelectionToolSystem;
      }
    }

    private void ToggleDistrictTool()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.activeTool == this.m_AreaToolSystem)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_DefaultToolSystem;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AreaToolSystem.prefab = this.m_PrefabSystem.GetPrefab<AreasConfigurationPrefab>(this.m_ConfigQuery.GetSingletonEntity()).m_DefaultDistrictPrefab;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_AreaToolSystem;
      }
    }

    [Preserve]
    public DistrictsSection()
    {
    }
  }
}
