// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.GamePanelUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.Serialization.Entities;
using Colossal.UI.Binding;
using Game.Citizens;
using Game.Input;
using Game.Prefabs;
using Game.PSI;
using Game.Serialization;
using Game.Settings;
using Game.Tools;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class GamePanelUISystem : UISystemBase, IPreDeserialize
  {
    private const string kGroup = "game";
    private PrefabSystem m_PrefabSystem;
    private ToolSystem m_ToolSystem;
    private DefaultToolSystem m_DefaultTool;
    private SelectedInfoUISystem m_SelectedInfoUISystem;
    private ToolbarUISystem m_ToolbarUISystem;
    private PhotoModeUISystem m_PhotoModeUISystem;
    private EntityQuery m_TransportConfigQuery;
    private InputBarrier m_ToolBarrier;
    private ValueBinding<GamePanel> m_ActivePanelBinding;
    private Dictionary<string, GamePanel> m_defaultArgs;
    public Action<GamePanel> eventPanelOpened;
    public Action<GamePanel> eventPanelClosed;
    private Entity m_PreviousSelectedEntity;
    private InfoviewPrefab m_PreviousInfoview;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_DefaultTool = this.World.GetOrCreateSystemManaged<DefaultToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedInfoUISystem = this.World.GetOrCreateSystemManaged<SelectedInfoUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolbarUISystem = this.World.GetOrCreateSystemManaged<ToolbarUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PhotoModeUISystem = this.World.GetOrCreateSystemManaged<PhotoModeUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_TransportConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<UITransportConfigurationData>());
      // ISSUE: reference to a compiler-generated field
      this.m_ToolBarrier = InputManager.instance.CreateMapBarrier("Tool", nameof (GamePanelUISystem));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_ActivePanelBinding = new ValueBinding<GamePanel>("game", "activePanel", (GamePanel) null, (IWriter<GamePanel>) new ValueWriter<GamePanel>().Nullable<GamePanel>())));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<bool>("game", "blockingPanelActive", (Func<bool>) (() =>
      {
        GamePanel activePanel = this.activePanel;
        return activePanel != null && activePanel.blocking;
      })));
      this.AddUpdateBinding((IUpdateBinding) new GetterValueBinding<int>("game", "activePanelPosition", (Func<int>) (() => this.activePanel == null ? 0 : (int) this.activePanel.position)));
      this.AddBinding((IBinding) new TriggerBinding<string>("game", "togglePanel", (Action<string>) (panelType =>
      {
        // ISSUE: reference to a compiler-generated field
        GamePanel previous = this.m_ActivePanelBinding.value;
        if (previous != null && previous.GetType().FullName == panelType)
        {
          // ISSUE: reference to a compiler-generated field
          this.m_ActivePanelBinding.Update((GamePanel) null);
          // ISSUE: reference to a compiler-generated method
          this.OnPanelChanged(previous, (GamePanel) null);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          this.ShowPanel(panelType);
        }
      })));
      this.AddBinding((IBinding) new TriggerBinding<string>("game", "showPanel", (Action<string>) (panelType =>
      {
        GamePanel panel;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_defaultArgs.TryGetValue(panelType, out panel))
          return;
        // ISSUE: reference to a compiler-generated method
        this.ShowPanel(panel);
      })));
      this.AddBinding((IBinding) new TriggerBinding<string>("game", "closePanel", (Action<string>) (panelType =>
      {
        // ISSUE: reference to a compiler-generated field
        GamePanel previous = this.m_ActivePanelBinding.value;
        if (previous == null || !(previous.GetType().FullName == panelType))
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_ActivePanelBinding.Update((GamePanel) null);
        // ISSUE: reference to a compiler-generated method
        this.OnPanelChanged(previous, (GamePanel) null);
      })));
      this.AddBinding((IBinding) new TriggerBinding("game", "closeActivePanel", (Action) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        GamePanel previous = this.m_ActivePanelBinding.value;
        if (previous == null)
          return;
        // ISSUE: reference to a compiler-generated field
        this.m_ActivePanelBinding.Update((GamePanel) null);
        // ISSUE: reference to a compiler-generated method
        this.OnPanelChanged(previous, (GamePanel) null);
      })));
      // ISSUE: reference to a compiler-generated field
      this.m_defaultArgs = new Dictionary<string, GamePanel>();
      // ISSUE: reference to a compiler-generated method
      this.InitializeDefaults();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      base.OnUpdate();
      // ISSUE: reference to a compiler-generated method
      if (this.activePanel != null && (this.NeedsClear || !this.IsPanelAllowed(this.activePanel)))
      {
        // ISSUE: reference to a compiler-generated method
        this.CloseActivePanel();
      }
      // ISSUE: reference to a compiler-generated field
      InputBarrier toolBarrier = this.m_ToolBarrier;
      GamePanel activePanel = this.activePanel;
      int num = activePanel != null ? (activePanel.blocking ? 1 : 0) : 0;
      toolBarrier.blocked = num != 0;
    }

    public void PreDeserialize(Context context)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ActivePanelBinding.Update((GamePanel) null);
    }

    public void SetDefaultArgs(GamePanel defaultArgs)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_defaultArgs[defaultArgs.GetType().FullName] = defaultArgs;
    }

    public void TogglePanel([CanBeNull] string panelType)
    {
      // ISSUE: reference to a compiler-generated field
      GamePanel previous = this.m_ActivePanelBinding.value;
      if (previous != null && previous.GetType().FullName == panelType)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ActivePanelBinding.Update((GamePanel) null);
        // ISSUE: reference to a compiler-generated method
        this.OnPanelChanged(previous, (GamePanel) null);
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        this.ShowPanel(panelType);
      }
    }

    public void ShowPanel(string panelType)
    {
      GamePanel panel;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_defaultArgs.TryGetValue(panelType, out panel))
        return;
      // ISSUE: reference to a compiler-generated method
      this.ShowPanel(panel);
    }

    public void ShowPanel(GamePanel panel)
    {
      // ISSUE: reference to a compiler-generated method
      if (!this.IsPanelAllowed(panel))
        return;
      // ISSUE: reference to a compiler-generated field
      GamePanel previous = this.m_ActivePanelBinding.value;
      // ISSUE: reference to a compiler-generated field
      this.m_ActivePanelBinding.Update(panel);
      // ISSUE: reference to a compiler-generated method
      this.OnPanelChanged(previous, panel);
    }

    public void ClosePanel(string panelType)
    {
      // ISSUE: reference to a compiler-generated field
      GamePanel previous = this.m_ActivePanelBinding.value;
      if (previous == null || !(previous.GetType().FullName == panelType))
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_ActivePanelBinding.Update((GamePanel) null);
      // ISSUE: reference to a compiler-generated method
      this.OnPanelChanged(previous, (GamePanel) null);
    }

    private void CloseActivePanel()
    {
      // ISSUE: reference to a compiler-generated field
      GamePanel previous = this.m_ActivePanelBinding.value;
      if (previous == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_ActivePanelBinding.Update((GamePanel) null);
      // ISSUE: reference to a compiler-generated method
      this.OnPanelChanged(previous, (GamePanel) null);
    }

    protected override void OnGameLoaded(Context serializationContext)
    {
      base.OnGameLoaded(serializationContext);
      // ISSUE: reference to a compiler-generated method
      this.InitializeDefaults();
    }

    [CanBeNull]
    public GamePanel activePanel => this.m_ActivePanelBinding.value;

    private void InitializeDefaults()
    {
      // ISSUE: reference to a compiler-generated method
      this.SetDefaultArgs((GamePanel) new InfoviewMenu());
      // ISSUE: reference to a compiler-generated method
      this.SetDefaultArgs((GamePanel) new ProgressionPanel());
      this.AddBinding((IBinding) new TriggerBinding<int>("game", "showProgressionPanel", (Action<int>) (tab =>
      {
        // ISSUE: reference to a compiler-generated method
        this.ShowPanel((GamePanel) new T()
        {
          selectedTab = tab
        });
      })));
      // ISSUE: reference to a compiler-generated method
      this.SetDefaultArgs((GamePanel) new EconomyPanel());
      this.AddBinding((IBinding) new TriggerBinding<int>("game", "showEconomyPanel", (Action<int>) (tab =>
      {
        // ISSUE: reference to a compiler-generated method
        this.ShowPanel((GamePanel) new T()
        {
          selectedTab = tab
        });
      })));
      // ISSUE: reference to a compiler-generated method
      this.SetDefaultArgs((GamePanel) new CityInfoPanel());
      this.AddBinding((IBinding) new TriggerBinding<int>("game", "showCityInfoPanel", (Action<int>) (tab =>
      {
        // ISSUE: reference to a compiler-generated method
        this.ShowPanel((GamePanel) new T()
        {
          selectedTab = tab
        });
      })));
      // ISSUE: reference to a compiler-generated method
      this.SetDefaultArgs((GamePanel) new StatisticsPanel());
      // ISSUE: reference to a compiler-generated method
      this.SetDefaultArgs((GamePanel) new TransportationOverviewPanel());
      this.AddBinding((IBinding) new TriggerBinding<int>("game", "showTransportationOverviewPanel", (Action<int>) (tab =>
      {
        // ISSUE: reference to a compiler-generated method
        this.ShowPanel((GamePanel) new T()
        {
          selectedTab = tab
        });
      })));
      // ISSUE: reference to a compiler-generated method
      this.SetDefaultArgs((GamePanel) new ChirperPanel());
      // ISSUE: reference to a compiler-generated method
      this.SetDefaultArgs((GamePanel) new LifePathPanel());
      this.AddBinding((IBinding) new TriggerBinding<Entity>("game", "showLifePathDetail", (Action<Entity>) (selectedEntity =>
      {
        // ISSUE: reference to a compiler-generated method
        this.ShowPanel((GamePanel) new T()
        {
          selectedEntity = selectedEntity
        });
      })));
      // ISSUE: reference to a compiler-generated method
      this.SetDefaultArgs((GamePanel) new JournalPanel());
      // ISSUE: reference to a compiler-generated method
      this.SetDefaultArgs((GamePanel) new RadioPanel());
      // ISSUE: reference to a compiler-generated method
      this.SetDefaultArgs((GamePanel) new PhotoModePanel());
      // ISSUE: reference to a compiler-generated method
      this.SetDefaultArgs((GamePanel) new CinematicCameraPanel());
      // ISSUE: reference to a compiler-generated method
      this.SetDefaultArgs((GamePanel) new NotificationsPanel());
    }

    public void ShowPanel<T>(int tab) where T : TabbedGamePanel, new()
    {
      T panel = new T();
      panel.selectedTab = tab;
      // ISSUE: reference to a compiler-generated method
      this.ShowPanel((GamePanel) panel);
    }

    public void ShowPanel<T>(Entity selectedEntity) where T : EntityGamePanel, new()
    {
      T panel = new T();
      panel.selectedEntity = selectedEntity;
      // ISSUE: reference to a compiler-generated method
      this.ShowPanel((GamePanel) panel);
    }

    private bool NeedsClear
    {
      get
      {
        if (this.m_SelectedInfoUISystem.selectedEntity != Entity.Null)
          return true;
        if (this.activePanel is InfoviewMenu)
          return false;
        return this.m_ToolSystem.activeTool != this.m_DefaultTool || this.m_ToolbarUISystem.hasActiveSelection;
      }
    }

    private bool IsPanelAllowed(GamePanel panel)
    {
      switch (panel)
      {
        case RadioPanel _:
          return SharedSettings.instance.audio.radioActive;
        case LifePathPanel lifePathPanel:
          if (lifePathPanel.selectedEntity != Entity.Null)
            return this.EntityManager.HasComponent<Followed>(lifePathPanel.selectedEntity);
          break;
      }
      return true;
    }

    private void OnPanelChanged([CanBeNull] GamePanel previous, [CanBeNull] GamePanel next)
    {
      if (previous != null && (next == null || next.GetType() != previous.GetType()))
      {
        // ISSUE: reference to a compiler-generated field
        Action<GamePanel> eventPanelClosed = this.eventPanelClosed;
        if (eventPanelClosed != null)
          eventPanelClosed(previous);
        // ISSUE: reference to a compiler-generated method
        this.OnPanelClosed(previous);
        if (next == null)
        {
          if (!(previous is InfoviewMenu))
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            this.m_ToolSystem.infoview = this.m_PreviousInfoview;
            // ISSUE: reference to a compiler-generated field
            this.m_PreviousInfoview = (InfoviewPrefab) null;
          }
          // ISSUE: reference to a compiler-generated field
          if (this.m_SelectedInfoUISystem.selectedEntity == Entity.Null)
          {
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_SelectedInfoUISystem.SetSelection(this.m_PreviousSelectedEntity);
            // ISSUE: reference to a compiler-generated field
            this.m_PreviousSelectedEntity = Entity.Null;
          }
        }
      }
      if (next == null || previous != null && !(next.GetType() != previous.GetType()))
        return;
      // ISSUE: reference to a compiler-generated method
      this.OnPanelOpened(next);
      // ISSUE: reference to a compiler-generated field
      Action<GamePanel> eventPanelOpened = this.eventPanelOpened;
      if (eventPanelOpened == null)
        return;
      eventPanelOpened(next);
    }

    private void OnPanelOpened(GamePanel panel)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_PreviousInfoview = this.m_ToolSystem.activeInfoview;
      if (panel is PhotoModePanel)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_PhotoModeUISystem.Activate(true);
      }
      if (!(panel is InfoviewMenu))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_DefaultTool;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ToolbarUISystem.ClearAssetSelection();
      }
      UITransportConfigurationPrefab config;
      // ISSUE: reference to a compiler-generated method
      if (panel is TransportationOverviewPanel && this.TryGetTransportConfig(out config))
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ToolSystem.infoview = config.m_TransportInfoview;
      }
      Telemetry.PanelOpened(panel);
      if (panel.retainSelection)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PreviousSelectedEntity = this.m_SelectedInfoUISystem.selectedEntity;
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_SelectedInfoUISystem.SetSelection(Entity.Null);
    }

    private void OnPanelClosed(GamePanel panel)
    {
      if (panel is PhotoModePanel)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_PhotoModeUISystem.Activate(false);
      }
      Telemetry.PanelClosed(panel);
      if (!panel.retainProperties)
        return;
      // ISSUE: reference to a compiler-generated method
      this.SetDefaultArgs(panel);
    }

    private bool TryGetTransportConfig(out UITransportConfigurationPrefab config)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      return this.m_PrefabSystem.TryGetSingletonPrefab<UITransportConfigurationPrefab>(this.m_TransportConfigQuery, out config);
    }

    [Preserve]
    public GamePanelUISystem()
    {
    }
  }
}
