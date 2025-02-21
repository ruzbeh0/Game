// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.ToolUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Colossal.UI.Binding;
using Game.Prefabs;
using Game.SceneFlow;
using Game.Simulation;
using Game.Tools;
using Game.UI.Localization;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  public class ToolUISystem : UISystemBase
  {
    public const string kGroup = "tool";
    private ToolSystem m_ToolSystem;
    private NetToolSystem m_NetToolSystem;
    private AreaToolSystem m_AreaToolSystem;
    private ZoneToolSystem m_ZoneToolSystem;
    private RouteToolSystem m_RouteToolSystem;
    private ObjectToolSystem m_ObjectToolSystem;
    private TerrainToolSystem m_TerrainToolSystem;
    private TerrainSystem m_TerrainSystem;
    private DefaultToolSystem m_DefaultToolSystem;
    private UpgradeToolSystem m_UpgradeToolSystem;
    private BulldozeToolSystem m_BulldozeToolSystem;
    private SelectionToolSystem m_SelectionToolSystem;
    private PrefabSystem m_PrefabSystem;
    private EntityQuery m_BulldozeQuery;
    private EntityQuery m_BrushQuery;
    private RawValueBinding m_ActiveToolBinding;
    private List<ToolMode> m_ToolModes;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      this.m_NetToolSystem = this.World.GetOrCreateSystemManaged<NetToolSystem>();
      this.m_AreaToolSystem = this.World.GetOrCreateSystemManaged<AreaToolSystem>();
      this.m_ZoneToolSystem = this.World.GetOrCreateSystemManaged<ZoneToolSystem>();
      this.m_RouteToolSystem = this.World.GetOrCreateSystemManaged<RouteToolSystem>();
      this.m_ObjectToolSystem = this.World.GetOrCreateSystemManaged<ObjectToolSystem>();
      this.m_TerrainToolSystem = this.World.GetOrCreateSystemManaged<TerrainToolSystem>();
      this.m_TerrainSystem = this.World.GetOrCreateSystemManaged<TerrainSystem>();
      this.m_DefaultToolSystem = this.World.GetOrCreateSystemManaged<DefaultToolSystem>();
      this.m_UpgradeToolSystem = this.World.GetOrCreateSystemManaged<UpgradeToolSystem>();
      this.m_BulldozeToolSystem = this.World.GetOrCreateSystemManaged<BulldozeToolSystem>();
      this.m_SelectionToolSystem = this.World.GetOrCreateSystemManaged<SelectionToolSystem>();
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      this.m_BulldozeQuery = this.GetEntityQuery(ComponentType.ReadOnly<BulldozeData>(), ComponentType.ReadOnly<PrefabData>());
      this.m_BrushQuery = this.GetEntityQuery(ComponentType.ReadOnly<BrushData>(), ComponentType.ReadOnly<PrefabData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem.EventToolChanged += new Action<ToolBaseSystem>(this.OnToolChanged);
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem.EventPrefabChanged += new Action<PrefabBase>(this.OnPrefabChanged);
      // ISSUE: reference to a compiler-generated field
      this.m_BulldozeToolSystem.EventConfirmationRequested += new System.Action(this.OnBulldozeConfirmationRequested);
      this.AddBinding((IBinding) (this.m_ActiveToolBinding = new RawValueBinding("tool", "activeTool", new Action<IJsonWriter>(this.BindActiveTool))));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<uint>("tool", "availableSnapMask", (Func<uint>) (() =>
      {
        if (this.m_ToolSystem.activeTool == null)
          return 0;
        Snap onMask;
        Snap offMask;
        // ISSUE: reference to a compiler-generated method
        this.m_ToolSystem.activeTool.GetAvailableSnapMask(out onMask, out offMask);
        return (uint) (onMask & offMask);
      })));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<uint>("tool", "allSnapMask", (Func<uint>) (() =>
      {
        if (this.m_ToolSystem.activeTool == null)
          return 0;
        Snap onMask;
        Snap offMask;
        // ISSUE: reference to a compiler-generated method
        this.m_ToolSystem.activeTool.GetAvailableSnapMask(out onMask, out offMask);
        return (uint) (onMask & offMask & ~(Snap.AutoParent | Snap.PrefabType | Snap.ContourLines));
      })));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<uint>("tool", "selectedSnapMask", (Func<uint>) (() => this.m_ToolSystem.activeTool == null ? 0U : (uint) this.m_ToolSystem.activeTool.selectedSnap)));
      this.AddBinding((IBinding) new ValueBinding<string[]>("tool", "snapOptionNames", this.InitSnapOptionNames(), (IWriter<string[]>) new ArrayWriter<string>((IWriter<string>) new StringWriter())));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("tool", "colorSupported", new Func<bool>(this.GetColorSupported)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<Color32>("tool", "color", (Func<Color32>) (() =>
      {
        // ISSUE: variable of a compiler-generated type
        ToolBaseSystem activeTool = this.m_ToolSystem.activeTool;
        return activeTool == null ? new Color32() : activeTool.color;
      })));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<Bounds1>("tool", "elevationRange", new Func<Bounds1>(this.GetElevationRange)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>("tool", "elevation", (Func<float>) (() => this.m_NetToolSystem.elevation)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>("tool", "elevationStep", (Func<float>) (() => this.m_NetToolSystem.elevationStep)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("tool", "parallelModeSupported", new Func<bool>(this.GetParallelModeSupported)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("tool", "parallelMode", (Func<bool>) (() => this.GetParallelModeSupported() && this.m_NetToolSystem.parallelCount != 0)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>("tool", "parallelOffset", (Func<float>) (() => this.m_NetToolSystem.parallelOffset)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("tool", "undergroundModeSupported", (Func<bool>) (() => this.m_ToolSystem.activeTool != null && this.m_ToolSystem.activeTool.allowUnderground)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("tool", "undergroundMode", (Func<bool>) (() => this.m_ToolSystem.activeTool != null && this.m_ToolSystem.activeTool.requireUnderground)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("tool", "elevationDownDisabled", new Func<bool>(this.GetElevationDownDisabled)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("tool", "elevationUpDisabled", new Func<bool>(this.GetElevationUpDisabled)));
      // ISSUE: reference to a compiler-generated method
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("tool", "replacingTrees", (Func<bool>) (() => !this.m_ObjectToolSystem.GetNetUpgradeStates(out JobHandle _).IsEmpty)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>("tool", "distance", (Func<float>) (() => this.m_ObjectToolSystem.distance)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>("tool", "distanceScale", (Func<float>) (() => this.m_ObjectToolSystem.distanceScale)));
      this.AddBinding((IBinding) new TriggerBinding<string>("tool", "selectTool", new Action<string>(this.SelectTool)));
      this.AddBinding((IBinding) new TriggerBinding<int>("tool", "selectToolMode", new Action<int>(this.SelectToolMode)));
      this.AddBinding((IBinding) new TriggerBinding<uint>("tool", "setSelectedSnapMask", new Action<uint>(this.SetSelectedSnapMask)));
      this.AddBinding((IBinding) new TriggerBinding("tool", "elevationUp", new System.Action(this.OnElevationUp)));
      this.AddBinding((IBinding) new TriggerBinding("tool", "elevationDown", new System.Action(this.OnElevationDown)));
      this.AddBinding((IBinding) new TriggerBinding("tool", "elevationScroll", new System.Action(this.OnElevationScroll)));
      this.AddBinding((IBinding) new TriggerBinding<float>("tool", "setElevationStep", new Action<float>(this.SetElevationStep)));
      this.AddBinding((IBinding) new TriggerBinding("tool", "toggleParallelMode", new System.Action(this.ToggleParallelMode)));
      this.AddBinding((IBinding) new TriggerBinding<float>("tool", "setParallelOffset", new Action<float>(this.SetParallelOffset)));
      this.AddBinding((IBinding) new TriggerBinding<bool>("tool", "setUndergroundMode", new Action<bool>(this.SetUndergroundMode)));
      this.AddBinding((IBinding) new TriggerBinding<float>("tool", "setDistance", new Action<float>(this.SetDistance)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("tool", "allowBrush", new Func<bool>(this.AllowBrush)));
      // ISSUE: reference to a compiler-generated method
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<Entity>("tool", "selectedBrush", (Func<Entity>) (() => !this.AllowBrush() || !this.m_ToolSystem.activeTool.brushing ? Entity.Null : this.m_PrefabSystem.GetEntity((PrefabBase) this.m_ToolSystem.activeTool.brushType))));
      this.AddBinding((IBinding) new GetterValueBinding<ToolUISystem.Brush[]>("tool", "brushes", new Func<ToolUISystem.Brush[]>(this.BindBrushTypes), (IWriter<ToolUISystem.Brush[]>) new ArrayWriter<ToolUISystem.Brush>((IWriter<ToolUISystem.Brush>) new ValueWriter<ToolUISystem.Brush>())));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>("tool", "brushSize", (Func<float>) (() => !this.AllowBrush() ? 0.0f : this.m_ToolSystem.activeTool.brushSize)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float?>("tool", "brushHeight", (Func<float?>) (() => this.m_ToolSystem.activeTool != this.m_TerrainToolSystem || this.m_TerrainToolSystem.prefab.m_Type != TerraformingType.Level ? new float?() : new float?(this.m_TerrainToolSystem.brushHeight - WaterSystem.SeaLevel)), (IWriter<float?>) ValueWriters.Create<float>().Nullable<float>()));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>("tool", "brushStrength", (Func<float>) (() => !this.AllowBrush() ? 0.0f : this.m_ToolSystem.activeTool.brushStrength)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>("tool", "brushAngle", (Func<float>) (() => !this.AllowBrush() ? 0.0f : this.m_ToolSystem.activeTool.brushAngle)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>("tool", "brushSizeMin", (Func<float>) (() => 10f)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>("tool", "brushSizeMax", (Func<float>) (() => !this.m_ToolSystem.actionMode.IsEditor() ? 1000f : 5000f)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>("tool", "brushHeightMin", (Func<float>) (() => -this.m_TerrainSystem.heightScaleOffset.y - WaterSystem.SeaLevel)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<float>("tool", "brushHeightMax", (Func<float>) (() => this.m_TerrainSystem.heightScaleOffset.x - this.m_TerrainSystem.heightScaleOffset.y - WaterSystem.SeaLevel)));
      this.AddBinding((IBinding) new TriggerBinding<Entity>("tool", "selectBrush", new Action<Entity>(this.SelectBrush)));
      this.AddBinding((IBinding) new TriggerBinding<float>("tool", "setBrushHeight", new Action<float>(this.SetBrushHeight)));
      this.AddBinding((IBinding) new TriggerBinding<float>("tool", "setBrushSize", new Action<float>(this.SetBrushSize)));
      this.AddBinding((IBinding) new TriggerBinding<float>("tool", "setBrushStrength", new Action<float>(this.SetBrushStrength)));
      this.AddBinding((IBinding) new TriggerBinding<float>("tool", "setBrushAngle", new Action<float>(this.SetBrushAngle)));
      this.AddBinding((IBinding) new TriggerBinding<Color32>("tool", "setColor", new Action<Color32>(this.SetColor)));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("tool", "isEditor", new Func<bool>(this.IsEditor)));
      this.m_ToolModes = new List<ToolMode>();
    }

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem.EventToolChanged -= new Action<ToolBaseSystem>(this.OnToolChanged);
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem.EventPrefabChanged -= new Action<PrefabBase>(this.OnPrefabChanged);
      // ISSUE: reference to a compiler-generated field
      this.m_BulldozeToolSystem.EventConfirmationRequested -= new System.Action(this.OnBulldozeConfirmationRequested);
      base.OnDestroy();
    }

    private void BindActiveTool(IJsonWriter binder)
    {
      binder.TypeBegin("tool.UITool");
      binder.PropertyName("id");
      binder.Write(this.m_ToolSystem.activeTool.toolID);
      binder.PropertyName("modeIndex");
      binder.Write(this.m_ToolSystem.activeTool.uiModeIndex);
      binder.PropertyName("modes");
      this.BindToolModes(binder);
      binder.TypeEnd();
    }

    private void BindToolModes(IJsonWriter binder)
    {
      this.m_ToolModes.Clear();
      // ISSUE: reference to a compiler-generated method
      this.m_ToolSystem.activeTool.GetUIModes(this.m_ToolModes);
      binder.ArrayBegin(this.m_ToolModes.Count);
      for (int index = 0; index < this.m_ToolModes.Count; ++index)
      {
        ToolMode toolMode = this.m_ToolModes[index];
        binder.TypeBegin("tool.ToolMode");
        binder.PropertyName("id");
        binder.Write(toolMode.name);
        binder.PropertyName("index");
        binder.Write(toolMode.index);
        binder.PropertyName("icon");
        binder.Write("Media/Tools/" + this.m_ToolSystem.activeTool.toolID + "/" + toolMode.name + ".svg");
        binder.TypeEnd();
      }
      binder.ArrayEnd();
    }

    private void SetSelectedSnapMask(uint mask)
    {
      this.m_ToolSystem.activeTool.selectedSnap = (Snap) mask;
    }

    private string[] InitSnapOptionNames()
    {
      uint[] values = (uint[]) Enum.GetValues(typeof (Snap));
      List<string> stringList = new List<string>();
      foreach (uint num in values)
      {
        switch (num)
        {
          case 0:
          case uint.MaxValue:
            continue;
          default:
            stringList.Add(Enum.GetName(typeof (Snap), (object) num));
            continue;
        }
      }
      return stringList.ToArray();
    }

    private void SelectTool(string tool) => this.SelectTool(this.GetToolSystem(tool));

    public void SelectTool(ToolBaseSystem tool)
    {
      if (this.m_ToolSystem.activeTool == tool)
        return;
      this.m_ToolSystem.activeTool = tool;
      if (tool != this.m_BulldozeToolSystem || this.m_BulldozeQuery.IsEmptyIgnoreFilter)
        return;
      using (NativeArray<PrefabData> componentDataArray = this.m_BulldozeQuery.ToComponentDataArray<PrefabData>((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        // ISSUE: reference to a compiler-generated method
        this.m_BulldozeToolSystem.prefab = this.m_PrefabSystem.GetPrefab<BulldozePrefab>(componentDataArray[0]);
      }
    }

    private void SelectToolMode(int modeIndex)
    {
      // ISSUE: variable of a compiler-generated type
      ToolBaseSystem activeTool = this.m_ToolSystem.activeTool;
      switch (activeTool)
      {
        case NetToolSystem netToolSystem:
          netToolSystem.mode = (NetToolSystem.Mode) modeIndex;
          break;
        case ZoneToolSystem zoneToolSystem:
          zoneToolSystem.mode = (ZoneToolSystem.Mode) modeIndex;
          break;
        case BulldozeToolSystem bulldozeToolSystem:
          bulldozeToolSystem.mode = (BulldozeToolSystem.Mode) modeIndex;
          break;
        case AreaToolSystem areaToolSystem:
          areaToolSystem.mode = (AreaToolSystem.Mode) modeIndex;
          break;
        case ObjectToolSystem objectToolSystem:
          objectToolSystem.mode = (ObjectToolSystem.Mode) modeIndex;
          break;
      }
      this.m_ActiveToolBinding.Update();
    }

    private ToolBaseSystem GetToolSystem(string tool)
    {
      switch (tool)
      {
        case "Area Tool":
          return (ToolBaseSystem) this.m_AreaToolSystem;
        case "Bulldoze Tool":
          return (ToolBaseSystem) this.m_BulldozeToolSystem;
        case "Net Tool":
          return (ToolBaseSystem) this.m_NetToolSystem;
        case "Object Tool":
          return (ToolBaseSystem) this.m_ObjectToolSystem;
        case "Route Tool":
          return (ToolBaseSystem) this.m_RouteToolSystem;
        case "Selection Tool":
          return (ToolBaseSystem) this.m_SelectionToolSystem;
        case "Terrain Tool":
          return (ToolBaseSystem) this.m_TerrainToolSystem;
        case "Upgrade Tool":
          return (ToolBaseSystem) this.m_UpgradeToolSystem;
        case "Zone Tool":
          return (ToolBaseSystem) this.m_ZoneToolSystem;
        default:
          return (ToolBaseSystem) this.m_DefaultToolSystem;
      }
    }

    private void OnToolChanged(ToolBaseSystem tool)
    {
      if (tool != this.m_TerrainToolSystem)
      {
        // ISSUE: reference to a compiler-generated method
        this.m_TerrainToolSystem.SetDisableFX();
      }
      this.m_ActiveToolBinding.Update();
    }

    private void OnPrefabChanged(PrefabBase prefab) => this.m_ActiveToolBinding.Update();

    private void OnBulldozeConfirmationRequested()
    {
      GameManager.instance.userInterface.appBindings.ShowConfirmationDialog(new ConfirmationDialog(new LocalizedString?(), (LocalizedString) "Common.DIALOG_MESSAGE[Bulldozer]", (LocalizedString) "Common.DIALOG_ACTION[Yes]", new LocalizedString?((LocalizedString) "Common.DIALOG_ACTION[No]"), System.Array.Empty<LocalizedString>()), new Action<int>(this.OnConfirmBulldoze));
    }

    private void OnConfirmBulldoze(int msg) => this.m_BulldozeToolSystem.ConfirmAction(msg == 0);

    private bool GetElevationUpDisabled()
    {
      if (this.m_ToolSystem.activeTool == this.m_NetToolSystem)
      {
        Bounds1 elevationRange = this.GetElevationRange();
        if (elevationRange != new Bounds1())
          return (double) elevationRange.max <= (double) this.m_NetToolSystem.elevation;
      }
      return !this.m_ToolSystem.activeTool.requireUnderground;
    }

    private void OnElevationUp()
    {
      if (this.m_ToolSystem.activeTool == null)
        return;
      // ISSUE: reference to a compiler-generated method
      this.m_ToolSystem.activeTool.ElevationUp();
    }

    private bool GetElevationDownDisabled()
    {
      if (this.m_ToolSystem.activeTool == this.m_NetToolSystem)
      {
        Bounds1 elevationRange = this.GetElevationRange();
        if (elevationRange != new Bounds1())
          return (double) elevationRange.min >= (double) this.m_NetToolSystem.elevation;
      }
      return this.m_ToolSystem.activeTool.requireUnderground || !this.m_ToolSystem.activeTool.allowUnderground;
    }

    private void OnElevationDown()
    {
      if (this.m_ToolSystem.activeTool == null)
        return;
      // ISSUE: reference to a compiler-generated method
      this.m_ToolSystem.activeTool.ElevationDown();
    }

    private void OnElevationScroll()
    {
      if (this.m_ToolSystem.activeTool == null)
        return;
      // ISSUE: reference to a compiler-generated method
      this.m_ToolSystem.activeTool.ElevationScroll();
    }

    private Bounds1 GetElevationRange()
    {
      PlaceableNet component1;
      if (this.m_ToolSystem.activeTool != this.m_NetToolSystem || this.m_NetToolSystem.mode == NetToolSystem.Mode.Replace || !((UnityEngine.Object) this.m_NetToolSystem.prefab != (UnityEngine.Object) null) || !this.m_NetToolSystem.prefab.TryGet<PlaceableNet>(out component1))
        return new Bounds1();
      PlaceableNet component2;
      return (UnityEngine.Object) component1.m_UndergroundPrefab != (UnityEngine.Object) null && component1.m_UndergroundPrefab.TryGet<PlaceableNet>(out component2) ? component1.m_ElevationRange | component2.m_ElevationRange : component1.m_ElevationRange;
    }

    private void SetElevationStep(float step) => this.m_NetToolSystem.elevationStep = step;

    private bool GetParallelModeSupported()
    {
      PlaceableNet component;
      return this.m_ToolSystem.activeTool == this.m_NetToolSystem && this.m_NetToolSystem.mode != NetToolSystem.Mode.Grid && this.m_NetToolSystem.mode != NetToolSystem.Mode.Replace && ((UnityEngine.Object) this.m_NetToolSystem.prefab != (UnityEngine.Object) null && this.m_NetToolSystem.prefab.TryGet<PlaceableNet>(out component) && component.m_AllowParallelMode || (UnityEngine.Object) this.m_NetToolSystem.lane != (UnityEngine.Object) null);
    }

    private void ToggleParallelMode()
    {
      this.m_NetToolSystem.parallelCount = this.m_NetToolSystem.parallelCount != 0 ? 0 : 1;
    }

    private void SetParallelOffset(float offset) => this.m_NetToolSystem.parallelOffset = offset;

    private void SetUndergroundMode(bool enabled)
    {
      if (this.m_ToolSystem.activeTool == null)
        return;
      // ISSUE: reference to a compiler-generated method
      this.m_ToolSystem.activeTool.SetUnderground(enabled);
    }

    private void SetDistance(float distance) => this.m_ObjectToolSystem.distance = distance;

    private ToolUISystem.Brush[] BindBrushTypes()
    {
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      NativeArray<Entity> entityArray = this.m_BrushQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      ToolUISystem.Brush[] brushArray = new ToolUISystem.Brush[entityArray.Length];
      for (int index = 0; index < entityArray.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated method
        BrushPrefab prefab = systemManaged.GetPrefab<BrushPrefab>(entityArray[index]);
        brushArray[index] = new ToolUISystem.Brush()
        {
          m_Entity = entityArray[index],
          m_Name = prefab.name,
          m_Icon = string.Empty,
          m_Priority = prefab.m_Priority
        };
      }
      entityArray.Dispose();
      return brushArray;
    }

    private void SelectBrush(Entity entity)
    {
      if (!this.AllowBrush())
        return;
      if (entity != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated method
        this.m_ToolSystem.activeTool.brushType = this.m_PrefabSystem.GetPrefab<BrushPrefab>(entity);
        if (this.m_ToolSystem.activeTool != this.m_ObjectToolSystem)
          return;
        this.m_ObjectToolSystem.mode = ObjectToolSystem.Mode.Brush;
      }
      else
      {
        if (this.m_ToolSystem.activeTool != this.m_ObjectToolSystem || this.m_ObjectToolSystem.mode != ObjectToolSystem.Mode.Brush)
          return;
        this.m_ObjectToolSystem.mode = ObjectToolSystem.Mode.Create;
      }
    }

    private bool AllowBrush()
    {
      if (this.m_ToolSystem.activeTool == this.m_ObjectToolSystem)
        return this.m_ObjectToolSystem.allowBrush;
      return this.m_ToolSystem.activeTool == this.m_TerrainToolSystem;
    }

    private void SetBrushSize(float size) => this.m_ToolSystem.activeTool.brushSize = size;

    private void SetBrushHeight(float height)
    {
      this.m_TerrainToolSystem.brushHeight = height + WaterSystem.SeaLevel;
    }

    private void SetBrushStrength(float strength)
    {
      this.m_ToolSystem.activeTool.brushStrength = strength;
    }

    private void SetBrushAngle(float angle) => this.m_ToolSystem.activeTool.brushAngle = angle;

    private void SetColor(Color32 color) => this.m_ToolSystem.activeTool.color = color;

    private bool GetColorSupported() => this.m_ToolSystem.activePrefab is IColored;

    private bool IsEditor() => GameManager.instance.gameMode.IsEditor();

    [Preserve]
    public ToolUISystem()
    {
    }

    public struct Brush : IJsonWritable
    {
      public Entity m_Entity;
      public string m_Name;
      public string m_Icon;
      public int m_Priority;

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("entity");
        writer.Write(this.m_Entity);
        writer.PropertyName("name");
        writer.Write(this.m_Name);
        writer.PropertyName("icon");
        writer.Write(this.m_Icon);
        writer.PropertyName("priority");
        writer.Write(this.m_Priority);
        writer.TypeEnd();
      }
    }
  }
}
