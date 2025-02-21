// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.DestroyedBuildingSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Buildings;
using Game.Common;
using Game.Objects;
using Game.Prefabs;
using Game.Simulation;
using Game.Tools;
using Game.Vehicles;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class DestroyedBuildingSection : InfoSectionBase
  {
    private ToolSystem m_ToolSystem;
    private DefaultToolSystem m_DefaultToolSystem;
    private UpgradeToolSystem m_UpgradeToolSystem;
    private ValueBinding<bool> m_Rebuilding;
    private EntityQuery m_FireStationQuery;
    private EntityQuery m_ServiceDispatchQuery;

    protected override string group => nameof (DestroyedBuildingSection);

    private Entity destroyer { get; set; }

    private bool cleared { get; set; }

    private float progress { get; set; }

    private DestroyedBuildingSection.Status status { get; set; }

    protected override bool displayForDestroyedObjects => true;

    protected override bool displayForUpgrades => true;

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_DefaultToolSystem = this.World.GetOrCreateSystemManaged<DefaultToolSystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_UpgradeToolSystem = this.World.GetOrCreateSystemManaged<UpgradeToolSystem>();
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem.EventToolChanged += (Action<ToolBaseSystem>) (tool => this.m_Rebuilding.Update(tool == this.m_UpgradeToolSystem));
      // ISSUE: reference to a compiler-generated field
      this.m_FireStationQuery = this.GetEntityQuery(ComponentType.ReadOnly<Building>(), ComponentType.ReadOnly<Game.Buildings.FireStation>(), ComponentType.ReadOnly<PrefabRef>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      // ISSUE: reference to a compiler-generated field
      this.m_ServiceDispatchQuery = this.GetEntityQuery(ComponentType.ReadOnly<Vehicle>(), ComponentType.ReadOnly<ServiceDispatch>(), ComponentType.Exclude<Deleted>(), ComponentType.Exclude<Temp>());
      this.AddBinding((IBinding) new TriggerBinding(this.group, "toggleRebuild", (System.Action) (() =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        if (this.m_ToolSystem.activeTool == this.m_UpgradeToolSystem)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_DefaultToolSystem;
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          this.m_UpgradeToolSystem.prefab = (ObjectPrefab) null;
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated field
          this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_UpgradeToolSystem;
        }
      })));
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_Rebuilding = new ValueBinding<bool>(this.group, "rebuilding", false)));
    }

    [Preserve]
    protected override void OnDestroy()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_ToolSystem.EventToolChanged -= (Action<ToolBaseSystem>) (tool => this.m_Rebuilding.Update(tool == this.m_UpgradeToolSystem));
      base.OnDestroy();
    }

    private void OnToolChanged(ToolBaseSystem tool)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      this.m_Rebuilding.Update(tool == this.m_UpgradeToolSystem);
    }

    private void OnToggleRebuild()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.m_ToolSystem.activeTool == this.m_UpgradeToolSystem)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_DefaultToolSystem;
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        this.m_UpgradeToolSystem.prefab = (ObjectPrefab) null;
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_ToolSystem.activeTool = (ToolBaseSystem) this.m_UpgradeToolSystem;
      }
    }

    protected override void Reset()
    {
      this.destroyer = Entity.Null;
      this.status = DestroyedBuildingSection.Status.None;
      this.cleared = false;
      this.progress = 0.0f;
    }

    private bool Visible()
    {
      if (!this.Destroyed || !this.EntityManager.HasComponent<Building>(this.selectedEntity) || this.EntityManager.HasComponent<Owner>(this.selectedEntity) && !this.EntityManager.HasComponent<Game.Buildings.ServiceUpgrade>(this.selectedEntity))
        return false;
      return !this.EntityManager.HasComponent<SpawnableBuildingData>(this.selectedPrefab) || this.EntityManager.HasComponent<PlacedSignatureBuildingData>(this.selectedPrefab) || this.EntityManager.HasComponent<Attached>(this.selectedEntity);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      Game.Common.Destroyed componentData = this.EntityManager.GetComponentData<Game.Common.Destroyed>(this.selectedEntity);
      PrefabRef component1;
      this.EntityManager.TryGetComponent<PrefabRef>(componentData.m_Event, out component1);
      this.destroyer = component1.m_Prefab;
      this.progress = math.max(0.0f, componentData.m_Cleared);
      this.cleared = (double) this.progress >= 1.0;
      if (!this.cleared)
      {
        // ISSUE: reference to a compiler-generated field
        NativeArray<PrefabRef> componentDataArray = this.m_FireStationQuery.ToComponentDataArray<PrefabRef>((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        // ISSUE: reference to a compiler-generated field
        NativeArray<Entity> entityArray = this.m_ServiceDispatchQuery.ToEntityArray((AllocatorManager.AllocatorHandle) Allocator.TempJob);
        bool flag = false;
        for (int index = 0; index < componentDataArray.Length; ++index)
        {
          FireStationData component2;
          if (this.EntityManager.TryGetComponent<FireStationData>(componentDataArray[index].m_Prefab, out component2) && component2.m_DisasterResponseCapacity > 0)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
        {
          this.status = DestroyedBuildingSection.Status.NoService;
        }
        else
        {
          for (int index1 = 0; index1 < entityArray.Length; ++index1)
          {
            DynamicBuffer<ServiceDispatch> buffer = this.EntityManager.GetBuffer<ServiceDispatch>(entityArray[index1], true);
            for (int index2 = 0; index2 < buffer.Length; ++index2)
            {
              FireRescueRequest component3;
              // ISSUE: reference to a compiler-generated method
              if (this.EntityManager.TryGetComponent<FireRescueRequest>(buffer[index2].m_Request, out component3) && component3.m_Type == FireRescueRequestType.Disaster && component3.m_Target == this.selectedEntity && this.VehicleAtTarget(entityArray[index1]))
              {
                this.status = DestroyedBuildingSection.Status.Searching;
                break;
              }
            }
          }
        }
        if (this.status == DestroyedBuildingSection.Status.None)
          this.status = DestroyedBuildingSection.Status.Waiting;
        componentDataArray.Dispose();
        entityArray.Dispose();
      }
      else
        this.status = DestroyedBuildingSection.Status.Rebuild;
      if (this.status != DestroyedBuildingSection.Status.None)
      {
        List<string> tooltipKeys = this.tooltipKeys;
        // ISSUE: variable of a compiler-generated type
        DestroyedBuildingSection.Status status = this.status;
        string str = status.ToString();
        tooltipKeys.Add(str);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_InfoUISystem.tooltipTags.Add(TooltipTags.Destroyed);
    }

    private bool VehicleAtTarget(Entity vehicle)
    {
      Game.Vehicles.FireEngine component;
      return this.EntityManager.TryGetComponent<Game.Vehicles.FireEngine>(vehicle, out component) && (component.m_State & FireEngineFlags.Rescueing) > (FireEngineFlags) 0;
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("destroyer");
      if (this.destroyer != Entity.Null)
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        PrefabBase prefab = this.m_PrefabSystem.GetPrefab<PrefabBase>(this.destroyer);
        writer.Write(prefab.name);
      }
      else
        writer.WriteNull();
      writer.PropertyName("progress");
      writer.Write(this.progress * 100f);
      writer.PropertyName("cleared");
      writer.Write(this.cleared);
      writer.PropertyName("status");
      IJsonWriter jsonWriter = writer;
      // ISSUE: variable of a compiler-generated type
      DestroyedBuildingSection.Status status = this.status;
      string str = status.ToString();
      jsonWriter.Write(str);
    }

    [Preserve]
    public DestroyedBuildingSection()
    {
    }

    private enum Status
    {
      None,
      Waiting,
      NoService,
      Searching,
      Rebuild,
    }
  }
}
