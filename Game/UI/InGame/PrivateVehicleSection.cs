// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.PrivateVehicleSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Common;
using Game.Vehicles;
using System;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class PrivateVehicleSection : VehicleSection
  {
    protected override string group => nameof (PrivateVehicleSection);

    private Entity keeperEntity { get; set; }

    private VehicleLocaleKey vehicleKey { get; set; }

    protected override void Reset()
    {
      // ISSUE: reference to a compiler-generated method
      base.Reset();
      this.keeperEntity = Entity.Null;
    }

    private bool Visible()
    {
      if (!this.EntityManager.HasComponent<Vehicle>(this.selectedEntity) || !this.EntityManager.HasComponent<Owner>(this.selectedEntity))
        return false;
      return this.EntityManager.HasComponent<PersonalCar>(this.selectedEntity) || this.EntityManager.HasComponent<Taxi>(this.selectedEntity);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      this.stateKey = VehicleUIUtils.GetStateKey(this.selectedEntity, this.EntityManager);
      this.keeperEntity = Entity.Null;
      if (this.stateKey != VehicleStateLocaleKey.Parked)
      {
        PersonalCar component;
        this.keeperEntity = this.EntityManager.TryGetComponent<PersonalCar>(this.selectedEntity, out component) ? component.m_Keeper : Entity.Null;
      }
      this.vehicleKey = this.EntityManager.HasComponent<Taxi>(this.selectedEntity) ? VehicleLocaleKey.Taxi : VehicleLocaleKey.HouseholdVehicle;
      this.tooltipKeys.Add(this.vehicleKey.ToString());
      // ISSUE: reference to a compiler-generated method
      base.OnProcess();
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated method
      base.OnWriteProperties(writer);
      writer.PropertyName("keeper");
      if (this.keeperEntity == Entity.Null)
      {
        writer.WriteNull();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NameSystem.BindName(writer, this.keeperEntity);
      }
      writer.PropertyName("keeperEntity");
      if (this.keeperEntity == Entity.Null)
        writer.WriteNull();
      else
        writer.Write(this.keeperEntity);
      writer.PropertyName("vehicleKey");
      writer.Write(Enum.GetName(typeof (VehicleLocaleKey), (object) this.vehicleKey));
    }

    [Preserve]
    public PrivateVehicleSection()
    {
    }
  }
}
