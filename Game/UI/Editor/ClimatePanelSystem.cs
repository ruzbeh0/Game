// Decompiled with JetBrains decompiler
// Type: Game.UI.Editor.ClimatePanelSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs;
using Game.Prefabs.Climate;
using Game.Reflection;
using Game.SceneFlow;
using Game.Simulation;
using Game.Tools;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Editor
{
  [CompilerGenerated]
  public class ClimatePanelSystem : EditorPanelSystemBase, SeasonsField.IAdapter
  {
    private PrefabSystem m_PrefabSystem;
    private ClimateSystem m_ClimateSystem;
    private WindSimulationSystem m_WindSimulationSystem;
    private EntityQuery m_ClimateQuery;
    private EntityQuery m_ClimateSeasonQuery;
    private EntityQuery m_UpdatedQuery;
    private SeasonsField.SeasonCurves m_SeasonsCurves;
    private EntityQuery m_RenderQuery;
    private ToolSystem m_ToolSystem;
    private InfoviewPrefab m_WindInfoview;
    private double m_LastWindDirection;
    private int m_LastClimateHash;
    private int m_InfoviewCooldown;
    private EditorSection m_InspectorSection;
    private EditorGenerator m_Generator;
    private Coroutine m_DelayedInfomodeReset;
    private InfoviewPrefab m_PreviousInfoview;
    private EntityQuery m_AllInfoviewQuery;
    private ClimatePanelSystem.TypeHandle __TypeHandle;

    private ClimatePrefab currentClimate
    {
      get
      {
        return this.World.EntityManager.Exists(this.m_ClimateSystem.currentClimate) ? this.m_PrefabSystem.GetPrefab<ClimatePrefab>(this.m_ClimateSystem.currentClimate) : (ClimatePrefab) null;
      }
      set
      {
        this.m_ClimateSystem.currentClimate = this.m_PrefabSystem.GetEntity((PrefabBase) value);
        this.RebuildInspector();
      }
    }

    private double windDirection
    {
      get
      {
        float2 constantWind = this.m_WindSimulationSystem.constantWind;
        float windDirection = Mathf.Atan2(constantWind.y, constantWind.x) * 57.29578f;
        if ((double) windDirection < 0.0)
          windDirection += 360f;
        return (double) windDirection;
      }
      set
      {
        float num = math.radians((float) value);
        this.m_WindSimulationSystem.SetWind(new float2((float) Math.Cos((double) num), (float) Math.Sin((double) num)), 40f);
        if (this.m_DelayedInfomodeReset == null)
          this.m_PreviousInfoview = this.m_ToolSystem.activeInfoview;
        this.m_ToolSystem.infoview = this.m_WindInfoview;
        foreach (InfomodeInfo infomode in this.m_ToolSystem.GetInfomodes(this.m_WindInfoview))
          this.m_ToolSystem.SetInfomodeActive(infomode.m_Mode, infomode.m_Mode.name == "WindInfomode", infomode.m_Priority);
        if (this.m_DelayedInfomodeReset != null)
          GameManager.instance.StopCoroutine(this.m_DelayedInfomodeReset);
        this.m_DelayedInfomodeReset = GameManager.instance.StartCoroutine(this.DisableInfomode());
      }
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ClimateSystem = this.World.GetOrCreateSystemManaged<ClimateSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_WindSimulationSystem = this.World.GetOrCreateSystemManaged<WindSimulationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AllInfoviewQuery = this.GetEntityQuery(ComponentType.ReadOnly<PrefabData>(), ComponentType.ReadOnly<InfoviewData>());
      // ISSUE: reference to a compiler-generated method
      this.GetWindInfoView();
      this.title = (LocalizedString) "Editor.CLIMATE_SETTINGS";
      // ISSUE: reference to a compiler-generated field
      this.m_Generator = new EditorGenerator();
      IWidget[] widgetArray = new IWidget[1];
      IWidget[] children = new IWidget[1];
      EditorSection editorSection1 = new EditorSection();
      editorSection1.displayName = (LocalizedString) "Editor.CLIMATE_SETTINGS";
      editorSection1.tooltip = new LocalizedString?((LocalizedString) "Editor.CLIMATE_SETTINGS_TOOLTIP");
      editorSection1.expanded = true;
      EditorSection editorSection2 = editorSection1;
      // ISSUE: reference to a compiler-generated field
      this.m_InspectorSection = editorSection1;
      children[0] = (IWidget) editorSection2;
      widgetArray[0] = (IWidget) Scrollable.WithChildren((IList<IWidget>) children);
      this.children = (IList<IWidget>) widgetArray;
    }

    protected override void OnValueChanged(IWidget widget)
    {
      base.OnValueChanged(widget);
      if (this.currentClimate.builtin)
        return;
      this.currentClimate.RebuildCurves();
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      if (serializationContext.purpose != Colossal.Serialization.Entities.Purpose.LoadMap && serializationContext.purpose != Colossal.Serialization.Entities.Purpose.NewMap)
        return;
      // ISSUE: reference to a compiler-generated method
      this.RebuildInspector();
    }

    private void RebuildInspector()
    {
      List<IWidget> widgetList1 = new List<IWidget>();
      List<IWidget> widgetList2 = widgetList1;
      PopupValueField<PrefabBase> popupValueField = new PopupValueField<PrefabBase>();
      popupValueField.displayName = (LocalizedString) "Editor.CLIMATE_LOAD_PREFAB";
      popupValueField.tooltip = new LocalizedString?((LocalizedString) "Editor.CLIMATE_LOAD_PREFAB_TOOLTIP");
      popupValueField.accessor = (ITypedValueAccessor<PrefabBase>) new DelegateAccessor<PrefabBase>((Func<PrefabBase>) (() => (PrefabBase) this.currentClimate), (Action<PrefabBase>) (prefab => this.currentClimate = (ClimatePrefab) prefab));
      popupValueField.popup = (IValueFieldPopup<PrefabBase>) new PrefabPickerPopup(typeof (ClimatePrefab));
      widgetList2.Add((IWidget) popupValueField);
      ClimatePrefab currentClimate = this.currentClimate;
      int num = currentClimate.builtin ? 1 : 0;
      if (num != 0)
      {
        List<IWidget> widgetList3 = widgetList1;
        Label label = new Label();
        label.displayName = (LocalizedString) "Editor.CREATE_CUSTOM_CLIMATE_PROMPT";
        widgetList3.Add((IWidget) label);
        List<IWidget> widgetList4 = widgetList1;
        Button button = new Button();
        button.displayName = (LocalizedString) "Editor.CREATE_CUSTOM_CLIMATE";
        button.action = (System.Action) (() =>
        {
          PrefabBase prefab = this.currentClimate.Clone();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_PrefabSystem.AddPrefab(prefab);
          this.currentClimate = (ClimatePrefab) prefab;
        });
        widgetList4.Add((IWidget) button);
      }
      // ISSUE: reference to a compiler-generated field
      IWidget[] array = this.m_Generator.BuildMembers((IValueAccessor) new ObjectAccessor<PrefabBase>((PrefabBase) currentClimate), 0, "Climate Settings").ToArray<IWidget>();
      if (num != 0)
      {
        foreach (IWidget widget in array)
        {
          // ISSUE: reference to a compiler-generated method
          InspectorPanelSystem.DisableAllFields(widget);
        }
      }
      widgetList1.AddRange((IEnumerable<IWidget>) array);
      // ISSUE: reference to a compiler-generated field
      this.m_InspectorSection.children = (IList<IWidget>) widgetList1;
    }

    private void Duplicate()
    {
      PrefabBase prefab = this.currentClimate.Clone();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PrefabSystem.AddPrefab(prefab);
      this.currentClimate = (ClimatePrefab) prefab;
    }

    private IEnumerator DisableInfomode()
    {
      yield return (object) new WaitForSeconds(1f);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem.infoview = this.m_PreviousInfoview;
      // ISSUE: reference to a compiler-generated field
      this.m_DelayedInfomodeReset = (Coroutine) null;
    }

    private void GetWindInfoView()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle.Update(ref this.CheckedStateRef);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      ComponentTypeHandle<PrefabData> componentTypeHandle = this.__TypeHandle.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle;
      // ISSUE: reference to a compiler-generated field
      NativeArray<ArchetypeChunk> archetypeChunkArray = this.m_AllInfoviewQuery.ToArchetypeChunkArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
      for (int index1 = 0; index1 < archetypeChunkArray.Length; ++index1)
      {
        NativeArray<PrefabData> nativeArray = archetypeChunkArray[index1].GetNativeArray<PrefabData>(ref componentTypeHandle);
        for (int index2 = 0; index2 < nativeArray.Length; ++index2)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          InfoviewPrefab prefab = this.m_PrefabSystem.GetPrefab<InfoviewPrefab>(nativeArray[index2]);
          if (prefab.name == "AirPollution")
          {
            // ISSUE: reference to a compiler-generated field
            this.m_WindInfoview = prefab;
          }
        }
      }
      archetypeChunkArray.Dispose();
    }

    IEnumerable<ClimateSystem.SeasonInfo> SeasonsField.IAdapter.seasons
    {
      get => (IEnumerable<ClimateSystem.SeasonInfo>) this.currentClimate.m_Seasons;
      set => this.currentClimate.m_Seasons = value.ToArray<ClimateSystem.SeasonInfo>();
    }

    SeasonsField.SeasonCurves SeasonsField.IAdapter.curves
    {
      get => this.m_SeasonsCurves;
      set => this.m_SeasonsCurves = value;
    }

    public void RebuildCurves()
    {
      ClimatePrefab currentClimate = this.currentClimate;
      currentClimate.RebuildCurves();
      // ISSUE: reference to a compiler-generated field
      this.m_SeasonsCurves = new SeasonsField.SeasonCurves();
      // ISSUE: reference to a compiler-generated field
      this.m_SeasonsCurves.m_Temperature = currentClimate.m_Temperature;
      // ISSUE: reference to a compiler-generated field
      this.m_SeasonsCurves.m_Precipitation = currentClimate.m_Precipitation;
      // ISSUE: reference to a compiler-generated field
      this.m_SeasonsCurves.m_Cloudiness = currentClimate.m_Cloudiness;
      // ISSUE: reference to a compiler-generated field
      this.m_SeasonsCurves.m_Aurora = currentClimate.m_Aurora;
      // ISSUE: reference to a compiler-generated field
      this.m_SeasonsCurves.m_Fog = currentClimate.m_Fog;
    }

    public Entity selectedSeason { get; set; }

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
    public ClimatePanelSystem()
    {
    }

    private struct TypeHandle
    {
      [ReadOnly]
      public ComponentTypeHandle<PrefabData> __Game_Prefabs_PrefabData_RO_ComponentTypeHandle;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public void __AssignHandles(ref SystemState state)
      {
        // ISSUE: reference to a compiler-generated field
        this.__Game_Prefabs_PrefabData_RO_ComponentTypeHandle = state.GetComponentTypeHandle<PrefabData>(true);
      }
    }
  }
}
