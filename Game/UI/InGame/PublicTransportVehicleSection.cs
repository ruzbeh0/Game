// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.PublicTransportVehicleSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Common;
using Game.Prefabs;
using Game.Vehicles;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class PublicTransportVehicleSection : VehicleWithLineSection
  {
    protected override string group => nameof (PublicTransportVehicleSection);

    private VehicleLocaleKey vehicleKey { get; set; }

    private bool Visible()
    {
      return this.EntityManager.HasComponent<Vehicle>(this.selectedEntity) && this.EntityManager.HasComponent<Owner>(this.selectedEntity) && this.EntityManager.HasComponent<Game.Vehicles.PublicTransport>(this.selectedEntity);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      Game.Vehicles.PublicTransport componentData1 = this.EntityManager.GetComponentData<Game.Vehicles.PublicTransport>(this.selectedEntity);
      this.stateKey = VehicleUIUtils.GetStateKey(this.selectedEntity, componentData1, this.EntityManager);
      PublicTransportVehicleData componentData2 = this.EntityManager.GetComponentData<PublicTransportVehicleData>(this.selectedPrefab);
      this.vehicleKey = (componentData2.m_PurposeMask & PublicTransportPurpose.PrisonerTransport) != (PublicTransportPurpose) 0 ? VehicleLocaleKey.PrisonVan : ((componentData2.m_PurposeMask & PublicTransportPurpose.Evacuation) == (PublicTransportPurpose) 0 || (componentData1.m_State & PublicTransportFlags.Evacuating) == (PublicTransportFlags) 0 ? VehicleLocaleKey.PublicTransportVehicle : VehicleLocaleKey.EvacuationBus);
      this.tooltipKeys.Add(this.vehicleKey.ToString());
      base.OnProcess();
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      base.OnWriteProperties(writer);
      writer.PropertyName("vehicleKey");
      writer.Write(Enum.GetName(typeof (VehicleLocaleKey), (object) this.vehicleKey));
    }

    [Preserve]
    public PublicTransportVehicleSection()
    {
    }
  }
}
