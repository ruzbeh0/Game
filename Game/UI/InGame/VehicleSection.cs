// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.VehicleSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Common;
using Game.Prefabs;
using Game.Vehicles;
using System;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public abstract class VehicleSection : InfoSectionBase
  {
    protected VehicleStateLocaleKey stateKey { get; set; }

    protected VehicleUIUtils.EntityWrapper owner { get; set; }

    protected bool fromOutside { get; set; }

    protected VehicleUIUtils.EntityWrapper nextStop { get; set; }

    protected override Entity selectedEntity
    {
      get
      {
        Controller component;
        return this.EntityManager.TryGetComponent<Controller>(base.selectedEntity, out component) ? component.m_Controller : base.selectedEntity;
      }
    }

    protected override Entity selectedPrefab
    {
      get
      {
        Controller component1;
        PrefabRef component2;
        return this.EntityManager.TryGetComponent<Controller>(base.selectedEntity, out component1) && this.EntityManager.TryGetComponent<PrefabRef>(component1.m_Controller, out component2) ? component2.m_Prefab : base.selectedPrefab;
      }
    }

    protected override void Reset()
    {
      this.stateKey = VehicleStateLocaleKey.Unknown;
      this.owner = new VehicleUIUtils.EntityWrapper(Entity.Null);
      this.fromOutside = false;
      this.nextStop = new VehicleUIUtils.EntityWrapper(Entity.Null);
    }

    protected override void OnProcess()
    {
      Entity owner = this.EntityManager.GetComponentData<Owner>(this.selectedEntity).m_Owner;
      this.owner = new VehicleUIUtils.EntityWrapper(owner);
      this.fromOutside = this.EntityManager.HasComponent<Game.Objects.OutsideConnection>(owner);
      switch (this.stateKey)
      {
        case VehicleStateLocaleKey.Patrolling:
          break;
        case VehicleStateLocaleKey.Working:
          break;
        case VehicleStateLocaleKey.Collecting:
          break;
        case VehicleStateLocaleKey.Returning:
          break;
        default:
          this.nextStop = new VehicleUIUtils.EntityWrapper(VehicleUIUtils.GetDestination(this.EntityManager, this.selectedEntity));
          break;
      }
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("stateKey");
      writer.Write(Enum.GetName(typeof (VehicleStateLocaleKey), (object) this.stateKey));
      writer.PropertyName("owner");
      // ISSUE: reference to a compiler-generated field
      this.owner.Write(writer, this.m_NameSystem);
      writer.PropertyName("fromOutside");
      writer.Write(this.fromOutside);
      writer.PropertyName("nextStop");
      // ISSUE: reference to a compiler-generated field
      this.nextStop.Write(writer, this.m_NameSystem);
    }

    [Preserve]
    protected VehicleSection()
    {
    }
  }
}
