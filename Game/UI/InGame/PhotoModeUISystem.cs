// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.PhotoModeUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections.Generic;
using Colossal.PSI.Common;
using Colossal.UI.Binding;
using Game.Common;
using Game.Input;
using Game.Reflection;
using Game.Rendering;
using Game.Rendering.CinematicCamera;
using Game.SceneFlow;
using Game.Simulation;
using Game.Tools;
using Game.UI.Localization;
using Game.UI.Menu;
using Game.UI.Widgets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  public class PhotoModeUISystem : UISystemBase
  {
    public const string kGroup = "photoMode";
    private ToolSystem m_ToolSystem;
    private DefaultToolSystem m_DefaultToolSystem;
    private ToolRaycastSystem m_ToolRaycastSystem;
    private RenderingSystem m_RenderingSystem;
    private PlanetarySystem m_PlanetarySystem;
    private PhotoModeRenderSystem m_PhotoModeRenderSystem;
    private CameraUpdateSystem m_CameraUpdateSystem;
    private CinematicCameraUISystem m_CinematicCameraUISystem;
    private BulldozeToolSystem m_BulldozeTool;
    private InputBarrier m_ToolBarrier;
    private ValueBinding<bool> m_OverlayHiddenBinding;
    private GetterValueBinding<bool> m_OrbitCameraActiveBinding;
    private GetterValueBinding<float> m_FieldOfViewBinding;
    private GetterValueBinding<float> m_TimeOfDayBinding;
    private GetterValueBinding<float> m_SaturationBinding;
    private RawMapBinding<Entity> m_AdjustmentCategoriesBinding;
    private bool m_TimeOfDayChanged;
    private ValueBinding<bool> m_CinematicCameraVisibleBinding;
    private ValueBinding<string> m_ActiveTabBinding;
    private RawValueBinding m_TabNamesBinding;
    private WidgetBindings m_WidgetBindings;

    public bool orbitMode { get; set; }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      this.m_DefaultToolSystem = this.World.GetOrCreateSystemManaged<DefaultToolSystem>();
      this.m_PlanetarySystem = this.World.GetOrCreateSystemManaged<PlanetarySystem>();
      this.m_RenderingSystem = this.World.GetOrCreateSystemManaged<RenderingSystem>();
      this.m_ToolRaycastSystem = this.World.GetOrCreateSystemManaged<ToolRaycastSystem>();
      this.m_PhotoModeRenderSystem = this.World.GetOrCreateSystemManaged<PhotoModeRenderSystem>();
      this.m_CameraUpdateSystem = this.World.GetOrCreateSystemManaged<CameraUpdateSystem>();
      this.m_CinematicCameraUISystem = this.World.GetOrCreateSystemManaged<CinematicCameraUISystem>();
      this.m_BulldozeTool = this.World.GetOrCreateSystemManaged<BulldozeToolSystem>();
      this.m_ToolBarrier = InputManager.instance.CreateMapBarrier("Tool", nameof (PhotoModeUISystem));
      this.tabs = this.BuildProperties();
      this.InjectPresets();
      this.AddUpdateBinding((IUpdateBinding) (this.m_WidgetBindings = new WidgetBindings("photoMode")));
      this.m_WidgetBindings.AddDefaultBindings();
      this.AddBinding((IBinding) new TriggerBinding<string>("photoMode", "selectTab", new Action<string>(this.SelectTab)));
      this.AddBinding((IBinding) new TriggerBinding<bool>("photoMode", "setCinematicCameraVisible", new Action<bool>(this.SetCinematicCameraVisible)));
      this.AddBinding((IBinding) (this.m_CinematicCameraVisibleBinding = new ValueBinding<bool>("photoMode", "cinematicCameraVisible", false)));
      this.AddBinding((IBinding) (this.m_ActiveTabBinding = new ValueBinding<string>("photoMode", "activeTab", string.Empty)));
      this.AddBinding((IBinding) (this.m_TabNamesBinding = new RawValueBinding("photoMode", "tabs", new Action<IJsonWriter>(this.BindTabNames))));
      this.SelectTab("Camera");
      this.AddBinding((IBinding) (this.m_OverlayHiddenBinding = new ValueBinding<bool>("photoMode", "overlayHidden", false)));
      this.AddBinding((IBinding) (this.m_OrbitCameraActiveBinding = new GetterValueBinding<bool>("photoMode", "orbitCameraActive", (Func<bool>) (() => this.m_CameraUpdateSystem.activeCameraController is OrbitCameraController))));
      this.AddBinding((IBinding) new TriggerBinding<bool>("photoMode", "setOverlayHidden", new Action<bool>(this.SetOverlayHidden)));
      this.AddBinding((IBinding) new TriggerBinding("photoMode", "takeScreenshot", new Action(this.TakeScreenshot)));
      this.AddBinding((IBinding) new TriggerBinding("photoMode", "toggleOrbitCameraActive", new Action(this.ToggleOrbitCameraActive)));
    }

    public void Activate(bool enabled)
    {
      if (enabled)
      {
        this.orbitMode = this.m_CameraUpdateSystem.activeCameraController == this.m_CameraUpdateSystem.orbitCameraController;
        if (this.m_CameraUpdateSystem.activeCameraController == this.m_CameraUpdateSystem.gamePlayController)
        {
          this.m_CameraUpdateSystem.cinematicCameraController.TryMatchPosition(this.m_CameraUpdateSystem.activeCameraController);
          this.m_CameraUpdateSystem.activeCameraController = (IGameCameraController) this.m_CameraUpdateSystem.cinematicCameraController;
        }
        this.m_CameraUpdateSystem.orbitCameraController.mode = OrbitCameraController.Mode.PhotoMode;
        this.m_CameraUpdateSystem.orbitCameraController.collisionsEnabled = false;
        this.m_CameraUpdateSystem.cinematicCameraController.collisionsEnabled = false;
      }
      else
      {
        if (this.m_CameraUpdateSystem.activeCameraController != this.m_CameraUpdateSystem.orbitCameraController || this.m_CameraUpdateSystem.orbitCameraController.followedEntity == Entity.Null)
        {
          this.m_CameraUpdateSystem.gamePlayController.TryMatchPosition(this.m_CameraUpdateSystem.activeCameraController);
          this.m_CameraUpdateSystem.activeCameraController = (IGameCameraController) this.m_CameraUpdateSystem.gamePlayController;
        }
        this.m_CameraUpdateSystem.orbitCameraController.mode = OrbitCameraController.Mode.Follow;
        this.m_CameraUpdateSystem.orbitCameraController.collisionsEnabled = true;
        this.m_PhotoModeRenderSystem.DisableAllCameraProperties();
      }
      this.m_ToolBarrier.blocked = enabled;
      this.m_PhotoModeRenderSystem.Enable(enabled);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      base.OnUpdate();
      if (this.m_OverlayHiddenBinding.value)
      {
        if (this.m_ToolSystem.activeTool != this.m_BulldozeTool)
        {
          this.m_RenderingSystem.hideOverlay = true;
          this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.FreeCameraDisable;
          this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_DefaultToolSystem;
        }
        else
        {
          this.m_ToolRaycastSystem.raycastFlags &= ~RaycastFlags.FreeCameraDisable;
          this.m_OverlayHiddenBinding.Update(false);
        }
      }
      if (!this.m_TimeOfDayChanged)
        return;
      this.m_TimeOfDayChanged = false;
      this.m_PlanetarySystem.Update();
    }

    private List<PhotoModeUISystem.Tab> tabs { get; set; } = new List<PhotoModeUISystem.Tab>();

    private void SetOverlayHidden(bool overlayHidden)
    {
      this.m_RenderingSystem.hideOverlay = overlayHidden;
      if (overlayHidden)
      {
        this.m_ToolRaycastSystem.raycastFlags |= RaycastFlags.FreeCameraDisable;
        this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_DefaultToolSystem;
      }
      else
        this.m_ToolRaycastSystem.raycastFlags &= ~RaycastFlags.FreeCameraDisable;
      this.m_OverlayHiddenBinding.Update(overlayHidden);
    }

    private void BindTabNames(IJsonWriter writer)
    {
      writer.ArrayBegin(this.tabs.Count);
      foreach (PhotoModeUISystem.Tab tab in this.tabs)
        tab.Write(writer);
      writer.ArrayEnd();
    }

    private void SetCinematicCameraVisible(bool visible)
    {
      this.m_CinematicCameraVisibleBinding.Update(visible);
    }

    private void SelectTab(string tabID)
    {
      PhotoModeUISystem.Tab tab1 = this.tabs.Find((Predicate<PhotoModeUISystem.Tab>) (tab => tab.id == tabID));
      if (tab1 == null)
        return;
      this.m_ActiveTabBinding.Update(tab1.id);
      this.m_WidgetBindings.children = (IList<IWidget>) tab1.items;
    }

    private List<PhotoModeUISystem.Tab> BuildProperties()
    {
      HashSet<string> handledGroupsCache = new HashSet<string>();
      OrderedDictionary<string, PhotoModeUISystem.Tab> orderedDictionary = new OrderedDictionary<string, PhotoModeUISystem.Tab>();
      foreach (KeyValuePair<string, PhotoModeProperty> photoModeProperty in this.m_PhotoModeRenderSystem.photoModeProperties)
      {
        PhotoModeUISystem.Tab tab;
        if (!orderedDictionary.TryGetValue(photoModeProperty.Value.group, out tab))
        {
          tab = new PhotoModeUISystem.Tab()
          {
            id = photoModeProperty.Value.group,
            icon = "Media/PhotoMode/" + photoModeProperty.Value.group + ".svg",
            items = new List<IWidget>()
          };
          orderedDictionary.Add(photoModeProperty.Value.group, tab);
        }
        if (PhotoModeUISystem.CheckMultiPropertyHandled(handledGroupsCache, photoModeProperty.Value))
          tab.items.Add(this.BuildControl(photoModeProperty.Value));
      }
      return orderedDictionary.Values.ToList<PhotoModeUISystem.Tab>();
    }

    private void InjectPresets()
    {
      foreach (PhotoModeUIPreset preset in (IEnumerable<PhotoModeUIPreset>) this.m_PhotoModeRenderSystem.presets)
        this.InjectPreset(preset);
    }

    private void InjectPreset(PhotoModeUIPreset preset)
    {
      PhotoModeUISystem.Tab tab1 = this.tabs.Find((Predicate<PhotoModeUISystem.Tab>) (tab => tab.id == preset.injectionProperty.group));
      if (tab1 == null)
        return;
      DelegateAccessor<int> accessor = new DelegateAccessor<int>((Func<int>) (() =>
      {
        int num = -1;
        bool flag = false;
        foreach (KeyValuePair<PhotoModeProperty, float[]> keyValuePair in (IEnumerable<KeyValuePair<PhotoModeProperty, float[]>>) preset.descriptor.values)
        {
          if (flag && num == -1)
            return num;
          num = -1;
          flag = true;
          for (int index = 0; index < keyValuePair.Value.Length; ++index)
          {
            if ((double) keyValuePair.Key.getValue() == (double) keyValuePair.Value[index])
            {
              if (num != -1 && num != index)
                return -1;
              num = index;
            }
          }
        }
        return num;
      }), (Action<int>) (value =>
      {
        foreach (KeyValuePair<PhotoModeProperty, float[]> keyValuePair in (IEnumerable<KeyValuePair<PhotoModeProperty, float[]>>) preset.descriptor.values)
        {
          if (value >= 0)
            keyValuePair.Key.setValue(keyValuePair.Value[value]);
        }
      }));
      List<DropdownItem<int>> items = new List<DropdownItem<int>>();
      items.Add(new DropdownItem<int>()
      {
        value = -1,
        displayName = (LocalizedString) "PhotoMode.SENSORTYPE[Custom]"
      });
      int num1 = 0;
      foreach (string str in (IEnumerable<string>) preset.descriptor.optionsId)
        items.Add(new DropdownItem<int>()
        {
          value = num1++,
          displayName = (LocalizedString) str
        });
      // ISSUE: explicit non-virtual call
      int index1 = tab1.items.FindIndex((Predicate<IWidget>) (x => (x is NamedWidget namedWidget ? __nonvirtual (namedWidget.displayName).value : (string) null) == PhotoModeUtils.ExtractPropertyID(preset.injectionProperty)));
      tab1.items.Insert(index1, (IWidget) this.BuildDropdownGroup(preset.id, items, accessor));
    }

    private static bool CheckMultiPropertyHandled(
      HashSet<string> handledGroupsCache,
      PhotoModeProperty property)
    {
      int length = property.id.IndexOf("/");
      if (length < 0)
        return true;
      string str = property.id.Substring(0, length);
      if (handledGroupsCache.Contains(str))
        return false;
      handledGroupsCache.Add(str);
      return true;
    }

    private Group BuildGroupTitle(PhotoModeProperty property)
    {
      Group group = new Group();
      group.displayName = LocalizedString.IdWithFallback("PhotoMode.PROPERTY_TITLE[" + property.id + "]", property.id);
      group.tooltip = new LocalizedString?(LocalizedString.IdWithFallback("PhotoMode.PROPERTY_TOOLTIP[" + property.id + "]", string.Empty));
      group.tooltipPos = Group.TooltipPosition.Title;
      return group;
    }

    private Group BuildDropdownGroup(
      string groupName,
      List<DropdownItem<int>> items,
      DelegateAccessor<int> accessor)
    {
      Group group1 = new Group();
      group1.displayName = LocalizedString.IdWithFallback("PhotoMode.PROPERTY_TITLE[" + groupName + "]", groupName);
      group1.tooltip = new LocalizedString?(LocalizedString.IdWithFallback("PhotoMode.PROPERTY_TOOLTIP[" + groupName + "]", string.Empty));
      Group group2 = group1;
      IWidget[] widgetArray = new IWidget[1];
      DropdownField<int> dropdownField = new DropdownField<int>();
      dropdownField.displayName = (LocalizedString) (groupName + "Dropdown");
      dropdownField.accessor = (ITypedValueAccessor<int>) accessor;
      dropdownField.items = items.ToArray();
      widgetArray[0] = (IWidget) dropdownField;
      group2.children = (IList<IWidget>) widgetArray;
      return group1;
    }

    private Group BuildEnumGroup(PhotoModeProperty property, bool multiPropertyComponent = false)
    {
      bool flag = property.isEnabled != null && property.setEnabled != null;
      List<IWidget> children = new List<IWidget>();
      this.AddCommonFields((IList<IWidget>) children, property, multiPropertyComponent);
      List<IWidget> widgetList = children;
      EnumField enumField = new EnumField();
      enumField.displayName = (LocalizedString) (property.id + "Dropdown");
      enumField.disabled = flag ? (Func<bool>) (() => !property.isEnabled()) : (Func<bool>) (() => false);
      enumField.enumMembers = AutomaticSettings.GetEnumValues(property.enumType, "PhotoMode");
      enumField.accessor = (ITypedValueAccessor<ulong>) new DelegateAccessor<ulong>((Func<ulong>) (() => (ulong) Mathf.RoundToInt(property.getValue())), (Action<ulong>) (value => property.setValue((float) value)));
      widgetList.Add((IWidget) enumField);
      if (!multiPropertyComponent && property.reset != null)
        children.Add((IWidget) new IconButton()
        {
          icon = "Media/Glyphs/ArrowCircular.svg",
          tooltip = new LocalizedString?(LocalizedString.Id("PhotoMode.RESET_PROPERTY_TOOLTIP")),
          action = property.reset
        });
      Group group = new Group();
      group.displayName = LocalizedString.IdWithFallback("PhotoMode.PROPERTY_TITLE[" + property.id + "]", property.id);
      group.tooltip = new LocalizedString?(!multiPropertyComponent ? LocalizedString.IdWithFallback("PhotoMode.PROPERTY_TOOLTIP[" + property.id + "]", string.Empty) : (LocalizedString) (string) null);
      group.tooltipPos = Group.TooltipPosition.Title;
      group.children = (IList<IWidget>) children;
      return group;
    }

    private Group BuildValueGroup(PhotoModeProperty property, bool multiPropertyComponent = false)
    {
      bool flag = property.setEnabled != null && property.isEnabled != null;
      List<IWidget> children = new List<IWidget>();
      if (property.fractionDigits > 0)
      {
        List<IWidget> widgetList = children;
        FloatInputField floatInputField = new FloatInputField();
        floatInputField.displayName = (LocalizedString) (property.id + " Value");
        floatInputField.dynamicMin = property.min != null ? (Func<double>) (() => (double) property.min()) : (Func<double>) null;
        floatInputField.dynamicMax = property.max != null ? (Func<double>) (() => (double) property.max()) : (Func<double>) null;
        floatInputField.fractionDigits = property.fractionDigits;
        floatInputField.disabled = flag ? (Func<bool>) (() => !property.isEnabled()) : (Func<bool>) (() => false);
        floatInputField.accessor = (ITypedValueAccessor<double>) new DelegateAccessor<double>((Func<double>) (() => (double) property.getValue()), (Action<double>) (value => property.setValue((float) value)));
        widgetList.Add((IWidget) floatInputField);
      }
      else
      {
        List<IWidget> widgetList = children;
        IntInputField intInputField = new IntInputField();
        intInputField.displayName = (LocalizedString) (property.id + " Value");
        intInputField.dynamicMin = property.min != null ? (Func<int>) (() => Mathf.RoundToInt(property.min())) : (Func<int>) null;
        intInputField.dynamicMax = property.max != null ? (Func<int>) (() => Mathf.RoundToInt(property.max())) : (Func<int>) null;
        intInputField.disabled = flag ? (Func<bool>) (() => !property.isEnabled()) : (Func<bool>) (() => false);
        intInputField.accessor = (ITypedValueAccessor<int>) new DelegateAccessor<int>((Func<int>) (() => Mathf.RoundToInt(property.getValue())), (Action<int>) (value => property.setValue((float) value)));
        widgetList.Add((IWidget) intInputField);
      }
      this.AddCommonFields((IList<IWidget>) children, property, multiPropertyComponent);
      if (property.fractionDigits > 0)
      {
        List<IWidget> widgetList = children;
        FloatSliderField floatSliderField = new FloatSliderField();
        floatSliderField.displayName = LocalizedString.IdWithFallback("PhotoMode.PROPERTY_TITLE[" + property.id + "]", property.id);
        floatSliderField.tooltip = new LocalizedString?(LocalizedString.IdWithFallback("PhotoMode.PROPERTY_TOOLTIP[" + property.id + "]", string.Empty));
        floatSliderField.dynamicMin = property.min != null ? (Func<double>) (() => (double) property.min()) : (Func<double>) (() => double.NegativeInfinity);
        floatSliderField.dynamicMax = property.max != null ? (Func<double>) (() => (double) property.max()) : (Func<double>) (() => double.PositiveInfinity);
        floatSliderField.disabled = flag ? (Func<bool>) (() => !property.isEnabled()) : (Func<bool>) (() => false);
        floatSliderField.accessor = (ITypedValueAccessor<double>) new DelegateAccessor<double>((Func<double>) (() => (double) property.getValue()), (Action<double>) (value => property.setValue((float) value)));
        floatSliderField.fractionDigits = property.fractionDigits;
        widgetList.Add((IWidget) floatSliderField);
      }
      else
      {
        List<IWidget> widgetList = children;
        IntSliderField intSliderField = new IntSliderField();
        intSliderField.displayName = LocalizedString.IdWithFallback("PhotoMode.PROPERTY_TITLE[" + property.id + "]", property.id);
        intSliderField.tooltip = new LocalizedString?(LocalizedString.IdWithFallback("PhotoMode.PROPERTY_TOOLTIP[" + property.id + "]", string.Empty));
        intSliderField.dynamicMin = property.min != null ? (Func<int>) (() => Mathf.RoundToInt(property.min())) : (Func<int>) (() => Mathf.RoundToInt(float.NegativeInfinity));
        intSliderField.dynamicMax = property.max != null ? (Func<int>) (() => Mathf.RoundToInt(property.max())) : (Func<int>) (() => Mathf.RoundToInt(float.PositiveInfinity));
        intSliderField.disabled = flag ? (Func<bool>) (() => !property.isEnabled()) : (Func<bool>) (() => false);
        intSliderField.accessor = (ITypedValueAccessor<int>) new DelegateAccessor<int>((Func<int>) (() => Mathf.RoundToInt(property.getValue())), (Action<int>) (value => property.setValue((float) value)));
        widgetList.Add((IWidget) intSliderField);
      }
      if (!multiPropertyComponent && property.reset != null)
        children.Add((IWidget) new IconButton()
        {
          icon = "Media/Glyphs/ArrowCircular.svg",
          tooltip = new LocalizedString?(LocalizedString.Id("PhotoMode.RESET_PROPERTY_TOOLTIP")),
          action = property.reset
        });
      Group group = new Group();
      group.displayName = LocalizedString.IdWithFallback("PhotoMode.PROPERTY_TITLE[" + property.id + "]", property.id);
      group.tooltip = new LocalizedString?(!multiPropertyComponent ? LocalizedString.IdWithFallback("PhotoMode.PROPERTY_TOOLTIP[" + property.id + "]", string.Empty) : (LocalizedString) (string) null);
      group.tooltipPos = Group.TooltipPosition.Title;
      group.children = (IList<IWidget>) children;
      return group;
    }

    private void AddCommonFields(
      IList<IWidget> children,
      PhotoModeProperty property,
      bool multiPropertyComponent)
    {
      if (multiPropertyComponent)
        return;
      if ((property.isEnabled == null ? 0 : (property.setEnabled != null ? 1 : 0)) != 0)
      {
        IList<IWidget> widgetList = children;
        ToggleField toggleField = new ToggleField();
        toggleField.displayName = (LocalizedString) (property.id + "EnableToggle");
        toggleField.tooltip = new LocalizedString?((LocalizedString) "PhotoMode.ENABLE_PROPERTY_TOOLTIP");
        toggleField.accessor = (ITypedValueAccessor<bool>) new DelegateAccessor<bool>(property.isEnabled, property.setEnabled);
        toggleField.disabled = (Func<bool>) (() => property.isAvailable != null && !property.isAvailable());
        toggleField.tutorialTag = "UITagPrefab:PhotoModePropertyEnableCheckbox";
        widgetList.Add((IWidget) toggleField);
      }
      IList<IWidget> widgetList1 = children;
      IconButton iconButton = new IconButton();
      iconButton.icon = "Media/PhotoMode/AddKeyframe.svg";
      iconButton.tooltip = new LocalizedString?((LocalizedString) "PhotoMode.CAPTURE_PROPERTY_TOOLTIP");
      iconButton.disabled = (Func<bool>) (() =>
      {
        if (!this.m_CinematicCameraVisibleBinding.value)
          return true;
        return property.isEnabled != null && !property.isEnabled();
      });
      // ISSUE: reference to a compiler-generated method
      iconButton.action = (Action) (() => this.m_CinematicCameraUISystem.ToggleModifier(property));
      iconButton.tutorialTag = "UITagPrefab:PhotoModePropertyKeyframeButton";
      widgetList1.Add((IWidget) iconButton);
    }

    private IWidget BuildColorGroup(
      PhotoModeProperty property,
      IDictionary<string, PhotoModeProperty> allProperties)
    {
      PhotoModeProperty[] array = PhotoModeUtils.ExtractMultiPropertyComponents(property, allProperties).ToArray<PhotoModeProperty>();
      PhotoModeProperty r = ((IEnumerable<PhotoModeProperty>) array).FirstOrDefault<PhotoModeProperty>((Func<PhotoModeProperty, bool>) (c => c.id.EndsWith("/r")));
      PhotoModeProperty g = ((IEnumerable<PhotoModeProperty>) array).FirstOrDefault<PhotoModeProperty>((Func<PhotoModeProperty, bool>) (c => c.id.EndsWith("/g")));
      PhotoModeProperty b = ((IEnumerable<PhotoModeProperty>) array).FirstOrDefault<PhotoModeProperty>((Func<PhotoModeProperty, bool>) (c => c.id.EndsWith("/b")));
      PhotoModeProperty a = ((IEnumerable<PhotoModeProperty>) array).FirstOrDefault<PhotoModeProperty>((Func<PhotoModeProperty, bool>) (c => c.id.EndsWith("/a")));
      Func<Color> getter = (Func<Color>) (() =>
      {
        PhotoModeProperty photoModeProperty1 = r;
        double r1 = photoModeProperty1 != null ? (double) photoModeProperty1.getValue() : 0.0;
        PhotoModeProperty photoModeProperty2 = g;
        double g1 = photoModeProperty2 != null ? (double) photoModeProperty2.getValue() : 0.0;
        PhotoModeProperty photoModeProperty3 = b;
        double b1 = photoModeProperty3 != null ? (double) photoModeProperty3.getValue() : 0.0;
        PhotoModeProperty photoModeProperty4 = a;
        double a1 = photoModeProperty4 != null ? (double) photoModeProperty4.getValue() : 1.0;
        return new Color((float) r1, (float) g1, (float) b1, (float) a1);
      });
      Action<Color> setter = (Action<Color>) (c =>
      {
        PhotoModeProperty photoModeProperty5 = r;
        if (photoModeProperty5 != null)
          photoModeProperty5.setValue(c.r);
        PhotoModeProperty photoModeProperty6 = g;
        if (photoModeProperty6 != null)
          photoModeProperty6.setValue(c.g);
        PhotoModeProperty photoModeProperty7 = b;
        if (photoModeProperty7 != null)
          photoModeProperty7.setValue(c.b);
        PhotoModeProperty photoModeProperty8 = a;
        if (photoModeProperty8 == null)
          return;
        photoModeProperty8.setValue(c.a);
      });
      ColorField colorField = new ColorField();
      colorField.accessor = (ITypedValueAccessor<Color>) new DelegateAccessor<Color>(getter, setter);
      colorField.disabled = (Func<bool>) (() => property.isEnabled != null && !property.isEnabled());
      colorField.showAlpha = a != null;
      colorField.hdr = property.max == null;
      return (IWidget) colorField;
    }

    private Group BuildCheckboxGroup(PhotoModeProperty property, bool multiPropertyComponent = false)
    {
      bool flag = property.setEnabled != null && property.isEnabled != null;
      List<IWidget> children = new List<IWidget>();
      this.AddCommonFields((IList<IWidget>) children, property, multiPropertyComponent);
      List<IWidget> widgetList = children;
      ToggleField toggleField = new ToggleField();
      toggleField.displayName = (LocalizedString) property.id;
      toggleField.accessor = (ITypedValueAccessor<bool>) new DelegateAccessor<bool>((Func<bool>) (() => Mathf.RoundToInt(property.getValue()) != 0), (Action<bool>) (value => property.setValue(value ? 1f : 0.0f)));
      toggleField.disabled = flag ? (Func<bool>) (() => !property.isEnabled()) : (Func<bool>) (() => false);
      widgetList.Add((IWidget) toggleField);
      if (!multiPropertyComponent && property.reset != null)
        children.Add((IWidget) new IconButton()
        {
          icon = "Media/Glyphs/ArrowCircular.svg",
          tooltip = new LocalizedString?(LocalizedString.Id("PhotoMode.RESET_PROPERTY_TOOLTIP")),
          action = property.reset
        });
      Group group = new Group();
      group.displayName = LocalizedString.IdWithFallback("PhotoMode.PROPERTY_TITLE[" + property.id + "]", property.id);
      group.tooltip = new LocalizedString?(!multiPropertyComponent ? LocalizedString.IdWithFallback("PhotoMode.PROPERTY_TOOLTIP[" + property.id + "]", property.id) : (LocalizedString) (string) null);
      group.tooltipPos = Group.TooltipPosition.Title;
      group.children = (IList<IWidget>) children;
      return group;
    }

    private Group BuildMultiPropertyGroup(
      PhotoModeProperty property,
      IDictionary<string, PhotoModeProperty> allProperties)
    {
      string str = property.id.Substring(0, property.id.IndexOf("/"));
      List<IWidget> children = new List<IWidget>();
      this.AddCommonFields((IList<IWidget>) children, property, false);
      if (property.overrideControl == PhotoModeProperty.OverrideControl.ColorField)
      {
        children.Add(this.BuildColorGroup(property, allProperties));
        if (property.reset != null)
          children.Add((IWidget) new IconButton()
          {
            icon = "Media/Glyphs/ArrowCircular.svg",
            tooltip = new LocalizedString?(LocalizedString.Id("PhotoMode.RESET_PROPERTY_TOOLTIP")),
            action = property.reset
          });
      }
      else
      {
        if (property.reset != null)
          children.Add((IWidget) new IconButton()
          {
            icon = "Media/Glyphs/ArrowCircular.svg",
            tooltip = new LocalizedString?(LocalizedString.Id("PhotoMode.RESET_PROPERTY_TOOLTIP")),
            action = property.reset
          });
        foreach (PhotoModeProperty propertyComponent in PhotoModeUtils.ExtractMultiPropertyComponents(property, allProperties))
          children.Add(this.BuildControl(propertyComponent, true));
      }
      Group group = new Group();
      group.displayName = LocalizedString.IdWithFallback("PhotoMode.PROPERTY_TITLE[" + str + "]", str);
      group.tooltip = new LocalizedString?(LocalizedString.IdWithFallback("PhotoMode.PROPERTY_TOOLTIP[" + str + "]", string.Empty));
      group.tooltipPos = Group.TooltipPosition.Title;
      group.children = (IList<IWidget>) children;
      return group;
    }

    private IWidget BuildControl(PhotoModeProperty property, bool multiPropertyComponent = false)
    {
      if (!multiPropertyComponent && property.id.IndexOf("/") >= 0)
        return (IWidget) this.BuildMultiPropertyGroup(property, (IDictionary<string, PhotoModeProperty>) this.m_PhotoModeRenderSystem.photoModeProperties);
      if (property.setValue == null && property.getValue == null)
        return (IWidget) this.BuildGroupTitle(property);
      if (property.overrideControl == PhotoModeProperty.OverrideControl.Checkbox)
        return (IWidget) this.BuildCheckboxGroup(property, multiPropertyComponent);
      return property.enumType != (System.Type) null ? (IWidget) this.BuildEnumGroup(property, multiPropertyComponent) : (IWidget) this.BuildValueGroup(property, multiPropertyComponent);
    }

    private void TakeScreenshot()
    {
      PlatformManager.instance.UnlockAchievement(Game.Achievements.Achievements.Snapshot);
      GameManager.instance.StartCoroutine(PhotoModeUISystem.CaptureScreenshot());
    }

    private void ToggleOrbitCameraActive()
    {
      if (this.m_CameraUpdateSystem.activeCameraController is OrbitCameraController)
      {
        this.orbitMode = false;
        this.m_CameraUpdateSystem.cinematicCameraController.TryMatchPosition(this.m_CameraUpdateSystem.activeCameraController);
        this.m_CameraUpdateSystem.activeCameraController = (IGameCameraController) this.m_CameraUpdateSystem.cinematicCameraController;
      }
      else
      {
        this.orbitMode = true;
        this.m_CameraUpdateSystem.orbitCameraController.followedEntity = Entity.Null;
        this.m_CameraUpdateSystem.orbitCameraController.TryMatchPosition(this.m_CameraUpdateSystem.activeCameraController);
        this.m_CameraUpdateSystem.activeCameraController = (IGameCameraController) this.m_CameraUpdateSystem.orbitCameraController;
      }
      this.m_OrbitCameraActiveBinding.Update();
    }

    private static IEnumerator CaptureScreenshot()
    {
      UserInterface ui = GameManager.instance.userInterface;
      if (ui != null)
        ui.view.enabled = false;
      yield return (object) new WaitForEndOfFrame();
      PlatformManager.instance.TakeScreenshot();
      if (ui != null)
        ui.view.enabled = true;
    }

    [Preserve]
    public PhotoModeUISystem()
    {
    }

    public class Tab
    {
      public string id { get; set; }

      public string icon { get; set; }

      public List<IWidget> items { get; set; } = new List<IWidget>();

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin("photoMode.Tab");
        writer.PropertyName("id");
        writer.Write(this.id);
        writer.PropertyName("icon");
        writer.Write(this.icon);
        writer.TypeEnd();
      }
    }
  }
}
