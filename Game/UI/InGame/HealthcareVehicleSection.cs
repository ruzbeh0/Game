// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.HealthcareVehicleSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

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
  public class HealthcareVehicleSection : VehicleSection
  {
    protected override string group => nameof (HealthcareVehicleSection);

    private Entity patientEntity { get; set; }

    private VehicleLocaleKey vehicleKey { get; set; }

    protected override void Reset()
    {
      // ISSUE: reference to a compiler-generated method
      base.Reset();
      this.patientEntity = Entity.Null;
    }

    private bool Visible()
    {
      return this.EntityManager.HasComponent<Vehicle>(this.selectedEntity) && this.EntityManager.HasComponent<Game.Vehicles.Ambulance>(this.selectedEntity) && this.EntityManager.HasComponent<Owner>(this.selectedEntity);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      Game.Vehicles.Ambulance componentData = this.EntityManager.GetComponentData<Game.Vehicles.Ambulance>(this.selectedEntity);
      this.patientEntity = componentData.m_TargetPatient;
      this.stateKey = VehicleUIUtils.GetStateKey(this.selectedEntity, componentData, this.EntityManager);
      this.vehicleKey = this.EntityManager.HasComponent<HelicopterData>(this.selectedPrefab) ? VehicleLocaleKey.MedicalHelicopter : VehicleLocaleKey.Ambulance;
      this.tooltipKeys.Add(this.vehicleKey.ToString());
      // ISSUE: reference to a compiler-generated method
      base.OnProcess();
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated method
      base.OnWriteProperties(writer);
      writer.PropertyName("patient");
      if (this.patientEntity == Entity.Null)
      {
        writer.WriteNull();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NameSystem.BindName(writer, this.patientEntity);
      }
      writer.PropertyName("patientEntity");
      if (this.patientEntity == Entity.Null)
        writer.WriteNull();
      else
        writer.Write(this.patientEntity);
      writer.PropertyName("vehicleKey");
      writer.Write(Enum.GetName(typeof (VehicleLocaleKey), (object) this.vehicleKey));
    }

    [Preserve]
    public HealthcareVehicleSection()
    {
    }
  }
}
