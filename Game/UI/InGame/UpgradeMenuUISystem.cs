// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.UpgradeMenuUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Buildings;
using Game.Common;
using Game.Input;
using Game.Prefabs;
using Game.Tools;
using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class UpgradeMenuUISystem : UISystemBase
  {
    private const string kGroup = "upgradeMenu";
    private EntityQuery m_UnlockedUpgradeQuery;
    private EntityQuery m_CreatedExtensionQuery;
    private EntityQuery m_DeletedExtensionQuery;
    private ToolSystem m_ToolSystem;
    private DefaultToolSystem m_DefaultTool;
    private ObjectToolSystem m_ObjectToolSystem;
    private UpgradeToolSystem m_UpgradeToolSystem;
    private PrefabSystem m_PrefabSystem;
    private PrefabUISystem m_PrefabUISystem;
    private ToolbarUISystem m_ToolbarUISystem;
    private SelectedInfoUISystem m_SelectedInfoUISystem;
    private RawMapBinding<Entity> m_UpgradesBinding;
    private RawMapBinding<Entity> m_UpgradeDetailsBinding;
    private ValueBinding<Entity> m_SelectedUpgradeBinding;
    private ValueBinding<bool> m_UpgradingBinding;
    private NativeList<Entity> m_Upgrades;

    public override GameMode gameMode => GameMode.Game;

    public bool upgrading
    {
      get
      {
        ValueBinding<Entity> selectedUpgradeBinding = this.m_SelectedUpgradeBinding;
        return selectedUpgradeBinding != null && selectedUpgradeBinding.active && this.m_SelectedUpgradeBinding.value != Entity.Null;
      }
    }

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_UnlockedUpgradeQuery = this.GetEntityQuery(ComponentType.ReadOnly<Unlock>());
      // ISSUE: reference to a compiler-generated field
      this.m_CreatedExtensionQuery = this.GetEntityQuery(ComponentType.ReadOnly<Extension>(), ComponentType.ReadOnly<Created>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_DeletedExtensionQuery = this.GetEntityQuery(ComponentType.ReadOnly<Extension>(), ComponentType.ReadOnly<Deleted>(), ComponentType.Exclude<Temp>());
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_DefaultTool = this.World.GetOrCreateSystemManaged<DefaultToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabSystem = this.World.GetOrCreateSystemManaged<PrefabSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PrefabUISystem = this.World.GetOrCreateSystemManaged<PrefabUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolbarUISystem = this.World.GetOrCreateSystemManaged<ToolbarUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedInfoUISystem = this.World.GetOrCreateSystemManaged<SelectedInfoUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpgradeToolSystem = this.World.GetOrCreateSystemManaged<UpgradeToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectToolSystem = this.World.GetOrCreateSystemManaged<ObjectToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_UpgradesBinding = new RawMapBinding<Entity>("upgradeMenu", "upgrades", (Action<IJsonWriter, Entity>) ((writer, upgradable) =>
      {
        PrefabRef component;
        if (!this.EntityManager.Exists(upgradable) || !this.EntityManager.TryGetComponent<PrefabRef>(upgradable, out component))
        {
          writer.WriteEmptyArray();
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_Upgrades.Clear();
          DynamicBuffer<BuildingUpgradeElement> buffer;
          if (this.EntityManager.TryGetBuffer<BuildingUpgradeElement>(component.m_Prefab, true, out buffer) && !this.EntityManager.HasComponent<Destroyed>(upgradable))
          {
            for (int index = 0; index < buffer.Length; ++index)
            {
              Entity upgrade = buffer[index].m_Upgrade;
              if (this.EntityManager.HasComponent<UIObjectData>(upgrade))
              {
                // ISSUE: reference to a compiler-generated field
                this.m_Upgrades.Add(in upgrade);
              }
            }
          }
          // ISSUE: reference to a compiler-generated field
          writer.ArrayBegin(this.m_Upgrades.Length);
          // ISSUE: reference to a compiler-generated field
          for (int index = 0; index < this.m_Upgrades.Length; ++index)
          {
            // ISSUE: reference to a compiler-generated field
            Entity upgrade = this.m_Upgrades[index];
            // ISSUE: reference to a compiler-generated method
            bool uniquePlaced = this.CheckExtensionBuiltStatus(upgradable, upgrade);
            // ISSUE: reference to a compiler-generated field
            // ISSUE: reference to a compiler-generated method
            this.m_ToolbarUISystem.BindAsset(writer, upgrade, uniquePlaced);
          }
          writer.ArrayEnd();
        }
      }))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_SelectedUpgradeBinding = new ValueBinding<Entity>("upgradeMenu", "selectedUpgrade", Entity.Null)));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_UpgradeDetailsBinding = new RawMapBinding<Entity>("upgradeMenu", "upgradeDetails", (Action<IJsonWriter, Entity>) ((writer, upgrade) =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        bool uniquePlaced = this.CheckExtensionBuiltStatus(this.m_SelectedInfoUISystem.selectedEntity, upgrade);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_PrefabUISystem.BindPrefabDetails(writer, upgrade, uniquePlaced);
      }))));
      this.AddBinding((IBinding) new TriggerBinding<Entity, Entity>("upgradeMenu", "selectUpgrade", (Action<Entity, Entity>) ((upgradable, upgrade) =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_SelectedUpgradeBinding.Update(upgrade);
        // ISSUE: reference to a compiler-generated method
        if (upgradable != Entity.Null && upgrade != Entity.Null && !this.EntityManager.HasEnabledComponent<Locked>(upgrade) && !this.CheckExtensionBuiltStatus(upgradable, upgrade))
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          PrefabBase prefab = this.m_PrefabSystem.GetPrefab<PrefabBase>(upgrade);
          // ISSUE: reference to a compiler-generated field
          this.m_UpgradingBinding.Update(true);
          // ISSUE: reference to a compiler-generated field
          this.m_UpgradesBinding.UpdateAll();
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_ToolSystem.ActivatePrefabTool(prefab);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_DefaultTool;
        }
      })));
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) new TriggerBinding("upgradeMenu", "clearUpgradeSelection", (System.Action) (() => this.SelectUpgrade(Entity.Null, Entity.Null))));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_UpgradingBinding = new ValueBinding<bool>("upgradeMenu", "upgrading", false)));
      // ISSUE: reference to a compiler-generated field
      this.m_Upgrades = new NativeList<Entity>(10, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_Upgrades.Dispose();
      base.OnDestroy();
    }

    [Preserve]
    protected override void OnStartRunning()
    {
      base.OnStartRunning();
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedInfoUISystem.eventSelectionChanged += (Action<Entity, Entity, float3>) ((entity, prefab, position) =>
      {
        if (InputManager.instance.activeControlScheme != InputManager.ControlScheme.KeyboardAndMouse)
          return;
        // ISSUE: reference to a compiler-generated method
        this.ClearUpgradeSelection();
      });
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem.EventToolChanged += (Action<ToolBaseSystem>) (tool =>
      {
        // ISSUE: reference to a compiler-generated field
        if (tool == this.m_DefaultTool && InputManager.instance.activeControlScheme == InputManager.ControlScheme.KeyboardAndMouse)
        {
          // ISSUE: reference to a compiler-generated method
          this.ClearUpgradeSelection();
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_UpgradingBinding.Update(tool == this.m_ObjectToolSystem && this.m_ObjectToolSystem.mode == ObjectToolSystem.Mode.Upgrade || tool == this.m_UpgradeToolSystem);
      });
    }

    [Preserve]
    protected override void OnStopRunning()
    {
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedInfoUISystem.eventSelectionChanged -= (Action<Entity, Entity, float3>) ((entity, prefab, position) =>
      {
        if (InputManager.instance.activeControlScheme != InputManager.ControlScheme.KeyboardAndMouse)
          return;
        // ISSUE: reference to a compiler-generated method
        this.ClearUpgradeSelection();
      });
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem.EventToolChanged -= (Action<ToolBaseSystem>) (tool =>
      {
        // ISSUE: reference to a compiler-generated field
        if (tool == this.m_DefaultTool && InputManager.instance.activeControlScheme == InputManager.ControlScheme.KeyboardAndMouse)
        {
          // ISSUE: reference to a compiler-generated method
          this.ClearUpgradeSelection();
        }
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_UpgradingBinding.Update(tool == this.m_ObjectToolSystem && this.m_ObjectToolSystem.mode == ObjectToolSystem.Mode.Upgrade || tool == this.m_UpgradeToolSystem);
      });
      base.OnStopRunning();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!PrefabUtils.HasUnlockedPrefabAll<BuildingUpgradeElement, UIObjectData>(this.EntityManager, this.m_UnlockedUpgradeQuery) && this.m_CreatedExtensionQuery.IsEmptyIgnoreFilter && this.m_DeletedExtensionQuery.IsEmptyIgnoreFilter && (!this.EntityManager.HasComponent<Updated>(this.m_SelectedInfoUISystem.selectedEntity) || this.EntityManager.HasComponent<Destroyed>(this.m_SelectedInfoUISystem.selectedEntity) == (this.m_Upgrades.Length == 0)))
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_UpgradesBinding.UpdateAll();
      // ISSUE: reference to a compiler-generated field
      this.m_UpgradeDetailsBinding.UpdateAll();
    }

    private void ClearUpgradeSelection() => this.SelectUpgrade(Entity.Null, Entity.Null);

    private void BindUpgrades(IJsonWriter writer, Entity upgradable)
    {
      PrefabRef component;
      if (!this.EntityManager.Exists(upgradable) || !this.EntityManager.TryGetComponent<PrefabRef>(upgradable, out component))
      {
        writer.WriteEmptyArray();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Upgrades.Clear();
        DynamicBuffer<BuildingUpgradeElement> buffer;
        if (this.EntityManager.TryGetBuffer<BuildingUpgradeElement>(component.m_Prefab, true, out buffer) && !this.EntityManager.HasComponent<Destroyed>(upgradable))
        {
          for (int index = 0; index < buffer.Length; ++index)
          {
            Entity upgrade = buffer[index].m_Upgrade;
            if (this.EntityManager.HasComponent<UIObjectData>(upgrade))
            {
              // ISSUE: reference to a compiler-generated field
              this.m_Upgrades.Add(in upgrade);
            }
          }
        }
        // ISSUE: reference to a compiler-generated field
        writer.ArrayBegin(this.m_Upgrades.Length);
        // ISSUE: reference to a compiler-generated field
        for (int index = 0; index < this.m_Upgrades.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          Entity upgrade = this.m_Upgrades[index];
          // ISSUE: reference to a compiler-generated method
          bool uniquePlaced = this.CheckExtensionBuiltStatus(upgradable, upgrade);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_ToolbarUISystem.BindAsset(writer, upgrade, uniquePlaced);
        }
        writer.ArrayEnd();
      }
    }

    private void BindUpgradeDetails(IJsonWriter writer, Entity upgrade)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      bool uniquePlaced = this.CheckExtensionBuiltStatus(this.m_SelectedInfoUISystem.selectedEntity, upgrade);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_PrefabUISystem.BindPrefabDetails(writer, upgrade, uniquePlaced);
    }

    private bool CheckExtensionBuiltStatus(Entity upgradableEntity, Entity upgradeEntity)
    {
      DynamicBuffer<InstalledUpgrade> buffer;
      if (this.EntityManager.HasComponent<BuildingExtensionData>(upgradeEntity) && this.EntityManager.TryGetBuffer<InstalledUpgrade>(upgradableEntity, true, out buffer))
      {
        for (int index = 0; index < buffer.Length; ++index)
        {
          PrefabRef component;
          if (this.EntityManager.TryGetComponent<PrefabRef>(buffer[index].m_Upgrade, out component) && component.m_Prefab == upgradeEntity)
            return true;
        }
      }
      return false;
    }

    private void SelectUpgrade(Entity upgradable, Entity upgrade)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_SelectedUpgradeBinding.Update(upgrade);
      // ISSUE: reference to a compiler-generated method
      if (upgradable != Entity.Null && upgrade != Entity.Null && !this.EntityManager.HasEnabledComponent<Locked>(upgrade) && !this.CheckExtensionBuiltStatus(upgradable, upgrade))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        PrefabBase prefab = this.m_PrefabSystem.GetPrefab<PrefabBase>(upgrade);
        // ISSUE: reference to a compiler-generated field
        this.m_UpgradingBinding.Update(true);
        // ISSUE: reference to a compiler-generated field
        this.m_UpgradesBinding.UpdateAll();
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ToolSystem.ActivatePrefabTool(prefab);
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_DefaultTool;
      }
    }

    private void OnSelectionChanged(Entity entity, Entity prefab, float3 position)
    {
      if (InputManager.instance.activeControlScheme != InputManager.ControlScheme.KeyboardAndMouse)
        return;
      // ISSUE: reference to a compiler-generated method
      this.ClearUpgradeSelection();
    }

    private void OnToolChanged(ToolBaseSystem tool)
    {
      // ISSUE: reference to a compiler-generated field
      if (tool == this.m_DefaultTool && InputManager.instance.activeControlScheme == InputManager.ControlScheme.KeyboardAndMouse)
      {
        // ISSUE: reference to a compiler-generated method
        this.ClearUpgradeSelection();
      }
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_UpgradingBinding.Update(tool == this.m_ObjectToolSystem && this.m_ObjectToolSystem.mode == ObjectToolSystem.Mode.Upgrade || tool == this.m_UpgradeToolSystem);
    }

    [Preserve]
    public UpgradeMenuUISystem()
    {
    }
  }
}
