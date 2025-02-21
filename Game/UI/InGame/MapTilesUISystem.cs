// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.MapTilesUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Areas;
using Game.Audio;
using Game.City;
using Game.Prefabs;
using Game.SceneFlow;
using Game.Simulation;
using System;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class MapTilesUISystem : UISystemBase
  {
    private const string kGroup = "mapTiles";
    private MapTilePurchaseSystem m_MapTileSystem;
    private CityConfigurationSystem m_CityConfigurationSystem;
    private AudioManager m_AudioManager;
    private GameScreenUISystem m_GameScreenUISystem;
    private EntityQuery m_SoundQuery;
    private GetterValueBinding<bool> m_MapTilesPanelVisibleBinding;
    private GetterValueBinding<bool> m_MapTilesViewActiveBinding;
    private RawValueBinding m_ResourcesBinding;
    private ValueBinding<MapTilesUISystem.UIMapTileResource> m_BuildableLandBinding;
    private ValueBinding<MapTilesUISystem.UIMapTileResource> m_WaterBinding;
    private GetterValueBinding<int> m_PurchasePriceBinding;
    private GetterValueBinding<int> m_PurchaseUpkeepBinding;
    private GetterValueBinding<int> m_PurchaseFlagsBinding;
    private GetterValueBinding<int> m_ExpansionPermitsBinding;
    private GetterValueBinding<int> m_ExpansionPermitCostBinding;
    private int m_LastSelected;
    private bool m_IsLastTimeZoomOut;

    public static bool mapTileViewActive { get; private set; }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_MapTileSystem = this.World.GetOrCreateSystemManaged<MapTilePurchaseSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_CityConfigurationSystem = this.World.GetOrCreateSystemManaged<CityConfigurationSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AudioManager = this.World.GetOrCreateSystemManaged<AudioManager>();
      // ISSUE: reference to a compiler-generated field
      this.m_GameScreenUISystem = this.World.GetOrCreateSystemManaged<GameScreenUISystem>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_MapTilesPanelVisibleBinding = new GetterValueBinding<bool>("mapTiles", "mapTilePanelVisible", (Func<bool>) (() => MapTilesUISystem.mapTileViewActive && !this.m_CityConfigurationSystem.unlockMapTiles))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_MapTilesViewActiveBinding = new GetterValueBinding<bool>("mapTiles", "mapTileViewActive", (Func<bool>) (() => MapTilesUISystem.mapTileViewActive))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) (this.m_BuildableLandBinding = new ValueBinding<MapTilesUISystem.UIMapTileResource>("mapTiles", "buildableLand", this.GetResource(MapFeature.BuildableLand), (IWriter<MapTilesUISystem.UIMapTileResource>) new ValueWriter<MapTilesUISystem.UIMapTileResource>())));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) (this.m_WaterBinding = new ValueBinding<MapTilesUISystem.UIMapTileResource>("mapTiles", "water", this.GetResource(MapFeature.GroundWater), (IWriter<MapTilesUISystem.UIMapTileResource>) new ValueWriter<MapTilesUISystem.UIMapTileResource>())));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_PurchasePriceBinding = new GetterValueBinding<int>("mapTiles", "purchasePrice", (Func<int>) (() => this.m_MapTileSystem.cost))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_PurchaseUpkeepBinding = new GetterValueBinding<int>("mapTiles", "purchaseUpkeep", (Func<int>) (() => this.m_MapTileSystem.upkeep))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_PurchaseFlagsBinding = new GetterValueBinding<int>("mapTiles", "purchaseFlags", (Func<int>) (() => (int) this.m_MapTileSystem.status))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) (this.m_ExpansionPermitsBinding = new GetterValueBinding<int>("mapTiles", "expansionPermits", (Func<int>) (() => this.m_MapTileSystem.GetAvailableTiles()))));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) (this.m_ExpansionPermitCostBinding = new GetterValueBinding<int>("mapTiles", "expansionPermitCost", (Func<int>) (() => this.m_MapTileSystem.GetSelectedTileCount()))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ResourcesBinding = new RawValueBinding("mapTiles", "resources", (Action<IJsonWriter>) (binder =>
      {
        binder.ArrayBegin(4U);
        // ISSUE: reference to a compiler-generated method
        binder.Write<MapTilesUISystem.UIMapTileResource>(this.GetResource(MapFeature.FertileLand));
        // ISSUE: reference to a compiler-generated method
        binder.Write<MapTilesUISystem.UIMapTileResource>(this.GetResource(MapFeature.Forest));
        // ISSUE: reference to a compiler-generated method
        binder.Write<MapTilesUISystem.UIMapTileResource>(this.GetResource(MapFeature.Oil));
        // ISSUE: reference to a compiler-generated method
        binder.Write<MapTilesUISystem.UIMapTileResource>(this.GetResource(MapFeature.Ore));
        binder.ArrayEnd();
      }))));
      this.AddBinding((IBinding) new TriggerBinding<bool>("mapTiles", "setMapTileViewActive", (Action<bool>) (enabled =>
      {
        // ISSUE: reference to a compiler-generated field
        if (enabled && this.m_GameScreenUISystem.activeScreen != 0)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_GameScreenUISystem.SetScreen(0);
        }
        MapTilesUISystem.mapTileViewActive = enabled;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_MapTileSystem.selecting = enabled && !this.m_CityConfigurationSystem.unlockMapTiles;
        // ISSUE: reference to a compiler-generated field
        if (this.m_IsLastTimeZoomOut != enabled && !GameManager.instance.isGameLoading)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_AudioManager.PlayUISound(enabled ? this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_CameraZoomInSound : this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_CameraZoomOutSound);
        }
        // ISSUE: reference to a compiler-generated field
        this.m_IsLastTimeZoomOut = enabled;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) new TriggerBinding("mapTiles", "purchaseMapTiles", (Action) (() => this.m_MapTileSystem.PurchaseSelection())));
      // ISSUE: reference to a compiler-generated field
      this.m_SoundQuery = this.GetEntityQuery(ComponentType.ReadOnly<ToolUXSoundSettingsData>());
      // ISSUE: reference to a compiler-generated field
      this.m_IsLastTimeZoomOut = false;
    }

    private void BindResources(IJsonWriter binder)
    {
      binder.ArrayBegin(4U);
      // ISSUE: reference to a compiler-generated method
      binder.Write<MapTilesUISystem.UIMapTileResource>(this.GetResource(MapFeature.FertileLand));
      // ISSUE: reference to a compiler-generated method
      binder.Write<MapTilesUISystem.UIMapTileResource>(this.GetResource(MapFeature.Forest));
      // ISSUE: reference to a compiler-generated method
      binder.Write<MapTilesUISystem.UIMapTileResource>(this.GetResource(MapFeature.Oil));
      // ISSUE: reference to a compiler-generated method
      binder.Write<MapTilesUISystem.UIMapTileResource>(this.GetResource(MapFeature.Ore));
      binder.ArrayEnd();
    }

    private MapTilesUISystem.UIMapTileResource GetResource(MapFeature feature)
    {
      switch (feature)
      {
        case MapFeature.BuildableLand:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: object of a compiler-generated type is created
          return new MapTilesUISystem.UIMapTileResource("BuildableLand", "Media/Game/Icons/MapTile.svg", this.m_MapTileSystem.GetFeatureAmount(MapFeature.BuildableLand), "area");
        case MapFeature.FertileLand:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: object of a compiler-generated type is created
          return new MapTilesUISystem.UIMapTileResource("FertileLand", "Media/Game/Icons/Fertility.svg", this.m_MapTileSystem.GetFeatureAmount(MapFeature.FertileLand), "area");
        case MapFeature.Forest:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: object of a compiler-generated type is created
          return new MapTilesUISystem.UIMapTileResource("Forest", "Media/Game/Icons/Forest.svg", this.m_MapTileSystem.GetFeatureAmount(MapFeature.Forest), "weight");
        case MapFeature.Oil:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: object of a compiler-generated type is created
          return new MapTilesUISystem.UIMapTileResource("Oil", "Media/Game/Icons/Oil.svg", this.m_MapTileSystem.GetFeatureAmount(MapFeature.Oil), "weight");
        case MapFeature.Ore:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: object of a compiler-generated type is created
          return new MapTilesUISystem.UIMapTileResource("Ore", "Media/Game/Icons/Coal.svg", this.m_MapTileSystem.GetFeatureAmount(MapFeature.Ore), "weight");
        default:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          // ISSUE: object of a compiler-generated type is created
          return new MapTilesUISystem.UIMapTileResource("Water", "Media/Game/Icons/Water.svg", this.m_MapTileSystem.GetFeatureAmount(MapFeature.GroundWater), "volume");
      }
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_MapTilesViewActiveBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_MapTilesPanelVisibleBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_ExpansionPermitsBinding.Update();
      if (!MapTilesUISystem.mapTileViewActive)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_MapTileSystem.Update();
      // ISSUE: reference to a compiler-generated field
      if (!this.m_MapTileSystem.selecting)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_PurchaseFlagsBinding.Update();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      int selectedTileCount = this.m_MapTileSystem.GetSelectedTileCount();
      // ISSUE: reference to a compiler-generated field
      if (this.m_LastSelected == selectedTileCount)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_LastSelected = selectedTileCount;
      // ISSUE: reference to a compiler-generated field
      this.m_PurchasePriceBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_PurchaseUpkeepBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_ExpansionPermitCostBinding.Update();
      // ISSUE: reference to a compiler-generated field
      this.m_ResourcesBinding.Update();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_BuildableLandBinding.Update(this.GetResource(MapFeature.BuildableLand));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_WaterBinding.Update(this.GetResource(MapFeature.GroundWater));
    }

    protected override void OnGamePreload(Colossal.Serialization.Entities.Purpose purpose, GameMode mode)
    {
      base.OnGamePreload(purpose, mode);
      MapTilesUISystem.mapTileViewActive = false;
    }

    private void SetMapTileViewActive(bool enabled)
    {
      // ISSUE: reference to a compiler-generated field
      if (enabled && this.m_GameScreenUISystem.activeScreen != 0)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_GameScreenUISystem.SetScreen(0);
      }
      MapTilesUISystem.mapTileViewActive = enabled;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_MapTileSystem.selecting = enabled && !this.m_CityConfigurationSystem.unlockMapTiles;
      // ISSUE: reference to a compiler-generated field
      if (this.m_IsLastTimeZoomOut != enabled && !GameManager.instance.isGameLoading)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AudioManager.PlayUISound(enabled ? this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_CameraZoomInSound : this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_CameraZoomOutSound);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_IsLastTimeZoomOut = enabled;
    }

    private void PurchaseMapTiles() => this.m_MapTileSystem.PurchaseSelection();

    [Preserve]
    public MapTilesUISystem()
    {
    }

    public readonly struct UIMapTileResource : IJsonWritable
    {
      public string id { get; }

      public string icon { get; }

      public float value { get; }

      public string unit { get; }

      public UIMapTileResource(string id, string icon, float value, string unit)
      {
        this.id = id;
        this.icon = icon;
        this.value = value;
        this.unit = unit;
      }

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin("mapTiles.UIMapTileResource");
        writer.PropertyName("id");
        writer.Write(this.id);
        writer.PropertyName("icon");
        writer.Write(this.icon);
        writer.PropertyName("value");
        writer.Write(this.value);
        writer.PropertyName("unit");
        writer.Write(this.unit);
        writer.TypeEnd();
      }
    }
  }
}
