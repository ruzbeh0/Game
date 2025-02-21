// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.GarbageVehicleSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Common;
using Game.Vehicles;
using System;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class GarbageVehicleSection : VehicleSection
  {
    protected override string group => nameof (GarbageVehicleSection);

    private VehicleLocaleKey vehicleKey { get; set; }

    private bool Visible()
    {
      return this.EntityManager.HasComponent<Vehicle>(this.selectedEntity) && this.EntityManager.HasComponent<GarbageTruck>(this.selectedEntity) && this.EntityManager.HasComponent<Owner>(this.selectedEntity);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      GarbageTruck componentData = this.EntityManager.GetComponentData<GarbageTruck>(this.selectedEntity);
      this.stateKey = VehicleUIUtils.GetStateKey(this.selectedEntity, componentData, this.EntityManager);
      this.vehicleKey = (componentData.m_State & GarbageTruckFlags.IndustrialWasteOnly) != (GarbageTruckFlags) 0 ? VehicleLocaleKey.IndustrialWasteTruck : VehicleLocaleKey.GarbageTruck;
      this.tooltipKeys.Add(this.vehicleKey.ToString());
      // ISSUE: reference to a compiler-generated method
      base.OnProcess();
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated method
      base.OnWriteProperties(writer);
      writer.PropertyName("vehicleKey");
      writer.Write(Enum.GetName(typeof (VehicleLocaleKey), (object) this.vehicleKey));
    }

    [Preserve]
    public GarbageVehicleSection()
    {
    }
  }
}
