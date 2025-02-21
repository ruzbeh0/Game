// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.TitleSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.UI.Binding;
using Game.Areas;
using Game.Buildings;
using Game.Citizens;
using Game.Creatures;
using Game.Net;
using Game.Objects;
using Game.Routes;
using Game.Vehicles;
using System;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class TitleSection : InfoSectionBase
  {
    private ImageSystem m_ImageSystem;

    protected override string group => nameof (TitleSection);

    protected override bool displayForDestroyedObjects => true;

    protected override bool displayForOutsideConnections => true;

    protected override bool displayForUnderConstruction => true;

    protected override bool displayForUpgrades => true;

    [CanBeNull]
    private string icon { get; set; }

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_ImageSystem = this.World.GetOrCreateSystemManaged<ImageSystem>();
      this.AddBinding((IBinding) new TriggerBinding<string>(this.group, "renameEntity", (Action<string>) (newName =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NameSystem.SetCustomName(this.selectedEntity, newName);
        // ISSUE: reference to a compiler-generated method
        this.RequestUpdate();
      })));
    }

    protected override void Reset() => this.icon = (string) null;

    private void OnRename(string newName)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NameSystem.SetCustomName(this.selectedEntity, newName);
      // ISSUE: reference to a compiler-generated method
      this.RequestUpdate();
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.selectedEntity != Entity.Null;

    protected override void OnProcess()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.icon = this.m_ImageSystem.GetInstanceIcon(this.selectedEntity, this.selectedPrefab);
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("name");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NameSystem.BindName(writer, this.selectedEntity);
      writer.PropertyName("vkName");
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_NameSystem.BindNameForVirtualKeyboard(writer, this.selectedEntity);
      writer.PropertyName("vkLocaleKey");
      // ISSUE: reference to a compiler-generated method
      writer.Write(TitleSection.GetVirtualKeyboardLocaleKey(this.EntityManager, this.selectedEntity));
      writer.PropertyName("icon");
      if (this.icon == null)
        writer.WriteNull();
      else
        writer.Write(this.icon);
    }

    public static string GetVirtualKeyboardLocaleKey(EntityManager entityManager, Entity entity)
    {
      if (entityManager.HasComponent<Building>(entity))
        return "BuildingName";
      if (entityManager.HasComponent<Tree>(entity))
        return "PlantName";
      if (entityManager.HasComponent<Citizen>(entity))
        return "CitizenName";
      if (entityManager.HasComponent<Vehicle>(entity))
        return "VehicleName";
      if (entityManager.HasComponent<Animal>(entity))
        return "AnimalName";
      if (entityManager.HasComponent<TransportLine>(entity))
        return "LineName";
      if (entityManager.HasComponent<Aggregate>(entity))
        return "RoadName";
      return entityManager.HasComponent<District>(entity) ? "DistrictName" : "ObjectName";
    }

    [Preserve]
    public TitleSection()
    {
    }
  }
}
