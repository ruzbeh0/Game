// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.UpgradesSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Audio;
using Game.Buildings;
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
  public class UpgradesSection : InfoSectionBase
  {
    private ToolSystem m_ToolSystem;
    private ObjectToolSystem m_ObjectToolSystem;
    private UIInitializeSystem m_UIInitializeSystem;
    private PoliciesUISystem m_PoliciesUISystem;
    private PolicyPrefab m_BuildingOutOfServicePolicy;
    private AudioManager m_AudioManager;
    private EntityQuery m_SoundQuery;

    protected override string group => nameof (UpgradesSection);

    private NativeList<Entity> extensions { get; set; }

    private NativeList<Entity> subBuildings { get; set; }

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetExistingSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ObjectToolSystem = this.World.GetExistingSystemManaged<ObjectToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UIInitializeSystem = this.World.GetOrCreateSystemManaged<UIInitializeSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_PoliciesUISystem = this.World.GetOrCreateSystemManaged<PoliciesUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_AudioManager = this.World.GetOrCreateSystemManaged<AudioManager>();
      // ISSUE: reference to a compiler-generated field
      this.m_SoundQuery = this.GetEntityQuery(ComponentType.ReadOnly<ToolUXSoundSettingsData>());
      this.extensions = new NativeList<Entity>(5, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.subBuildings = new NativeList<Entity>(10, (AllocatorManager.AllocatorHandle) Allocator.Persistent);
      this.AddBinding((IBinding) new TriggerBinding<Entity>(this.group, "delete", (Action<Entity>) (entity =>
      {
        if (!this.EntityManager.Exists(entity))
          return;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_BulldozeSound);
        // ISSUE: reference to a compiler-generated field
        this.m_EndFrameBarrier.CreateCommandBuffer().AddComponent<Deleted>(entity);
      })));
      this.AddBinding((IBinding) new TriggerBinding<Entity>(this.group, "relocate", (Action<Entity>) (entity =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_ObjectToolSystem.StartMoving(entity);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_ObjectToolSystem;
      })));
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.AddBinding((IBinding) new TriggerBinding<Entity>(this.group, "focus", (Action<Entity>) (entity => this.m_InfoUISystem.Focus(!((UnityEngine.Object) SelectedInfoUISystem.s_CameraController != (UnityEngine.Object) null) || !SelectedInfoUISystem.s_CameraController.controllerEnabled || !(SelectedInfoUISystem.s_CameraController.followedEntity == entity) ? entity : Entity.Null))));
      this.AddBinding((IBinding) new TriggerBinding<Entity>(this.group, "toggle", (Action<Entity>) (entity =>
      {
        Building component1;
        Extension component2;
        bool flag = this.EntityManager.TryGetComponent<Building>(entity, out component1) && BuildingUtils.CheckOption(component1, BuildingOption.Inactive) || this.EntityManager.TryGetComponent<Extension>(entity, out component2) && (component2.m_Flags & ExtensionFlags.Disabled) != 0;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        this.m_PoliciesUISystem.SetSelectedInfoPolicy(entity, this.m_PrefabSystem.GetEntity((PrefabBase) this.m_BuildingOutOfServicePolicy), !flag);
      })));
    }

    [Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      this.extensions.Dispose();
      this.subBuildings.Dispose();
    }

    protected override void OnGameLoaded(Colossal.Serialization.Entities.Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      foreach (PolicyPrefab policy in this.m_UIInitializeSystem.policies)
      {
        if (policy.name == "Out of Service")
        {
          // ISSUE: reference to a compiler-generated field
          this.m_BuildingOutOfServicePolicy = policy;
        }
      }
    }

    private void OnToggle(Entity entity)
    {
      Building component1;
      Extension component2;
      bool flag = this.EntityManager.TryGetComponent<Building>(entity, out component1) && BuildingUtils.CheckOption(component1, BuildingOption.Inactive) || this.EntityManager.TryGetComponent<Extension>(entity, out component2) && (component2.m_Flags & ExtensionFlags.Disabled) != 0;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.m_PoliciesUISystem.SetSelectedInfoPolicy(entity, this.m_PrefabSystem.GetEntity((PrefabBase) this.m_BuildingOutOfServicePolicy), !flag);
    }

    private void OnRelocate(Entity entity)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_ObjectToolSystem.StartMoving(entity);
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_ObjectToolSystem;
    }

    private void OnDelete(Entity entity)
    {
      if (!this.EntityManager.Exists(entity))
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_AudioManager.PlayUISound(this.m_SoundQuery.GetSingleton<ToolUXSoundSettingsData>().m_BulldozeSound);
      // ISSUE: reference to a compiler-generated field
      this.m_EndFrameBarrier.CreateCommandBuffer().AddComponent<Deleted>(entity);
    }

    private void OnFocus(Entity entity)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.Focus(!((UnityEngine.Object) SelectedInfoUISystem.s_CameraController != (UnityEngine.Object) null) || !SelectedInfoUISystem.s_CameraController.controllerEnabled || !(SelectedInfoUISystem.s_CameraController.followedEntity == entity) ? entity : Entity.Null);
    }

    protected override void Reset()
    {
      this.extensions.Clear();
      this.subBuildings.Clear();
    }

    private bool Visible()
    {
      return this.EntityManager.HasBuffer<BuildingUpgradeElement>(this.selectedPrefab);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      DynamicBuffer<InstalledUpgrade> buffer;
      if (!this.EntityManager.TryGetBuffer<InstalledUpgrade>(this.selectedEntity, true, out buffer))
        return;
      for (int index = 0; index < buffer.Length; ++index)
      {
        Entity upgrade = buffer[index].m_Upgrade;
        NativeList<Entity> nativeList;
        if (this.EntityManager.HasComponent<Extension>(upgrade))
        {
          nativeList = this.extensions;
          nativeList.Add(in upgrade);
        }
        else
        {
          nativeList = this.subBuildings;
          nativeList.Add(in upgrade);
        }
      }
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("extensions");
      writer.ArrayBegin(this.extensions.Length);
      for (int index = 0; index < this.extensions.Length; ++index)
      {
        Entity extension = this.extensions[index];
        writer.TypeBegin(this.group + ".Upgrade");
        writer.PropertyName("name");
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NameSystem.BindName(writer, extension);
        writer.PropertyName("entity");
        writer.Write(extension);
        Extension component;
        bool flag1 = this.EntityManager.TryGetComponent<Extension>(extension, out component) && (component.m_Flags & ExtensionFlags.Disabled) != 0;
        writer.PropertyName("disabled");
        writer.Write(flag1);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        bool flag2 = (UnityEngine.Object) SelectedInfoUISystem.s_CameraController != (UnityEngine.Object) null && SelectedInfoUISystem.s_CameraController.controllerEnabled && SelectedInfoUISystem.s_CameraController.followedEntity == extension;
        writer.PropertyName("focused");
        writer.Write(flag2);
        writer.TypeEnd();
      }
      writer.ArrayEnd();
      writer.PropertyName("subBuildings");
      IJsonWriter writer1 = writer;
      NativeList<Entity> subBuildings = this.subBuildings;
      int length1 = subBuildings.Length;
      writer1.ArrayBegin(length1);
      int index1 = 0;
      while (true)
      {
        int num = index1;
        subBuildings = this.subBuildings;
        int length2 = subBuildings.Length;
        if (num < length2)
        {
          subBuildings = this.subBuildings;
          Entity entity = subBuildings[index1];
          writer.TypeBegin(this.group + ".Upgrade");
          writer.PropertyName("name");
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          this.m_NameSystem.BindName(writer, entity);
          writer.PropertyName("entity");
          writer.Write(entity);
          Building component;
          bool flag3 = this.EntityManager.TryGetComponent<Building>(entity, out component) && BuildingUtils.CheckOption(component, BuildingOption.Inactive);
          writer.PropertyName("disabled");
          writer.Write(flag3);
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          bool flag4 = (UnityEngine.Object) SelectedInfoUISystem.s_CameraController != (UnityEngine.Object) null && SelectedInfoUISystem.s_CameraController.controllerEnabled && SelectedInfoUISystem.s_CameraController.followedEntity == entity;
          writer.PropertyName("focused");
          writer.Write(flag4);
          writer.TypeEnd();
          ++index1;
        }
        else
          break;
      }
      writer.ArrayEnd();
    }

    [Preserve]
    public UpgradesSection()
    {
    }
  }
}
