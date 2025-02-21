// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.MaintenanceVehicleSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Common;
using Game.Prefabs;
using Game.Vehicles;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class MaintenanceVehicleSection : VehicleSection
  {
    protected override string group => nameof (MaintenanceVehicleSection);

    private int workShift { get; set; }

    protected override void Reset()
    {
      // ISSUE: reference to a compiler-generated method
      base.Reset();
      this.workShift = 0;
    }

    protected bool Visible()
    {
      return this.EntityManager.HasComponent<Owner>(this.selectedEntity) && this.EntityManager.HasComponent<Vehicle>(this.selectedEntity) && this.EntityManager.HasComponent<Game.Vehicles.MaintenanceVehicle>(this.selectedEntity) && this.EntityManager.HasComponent<MaintenanceVehicleData>(this.selectedPrefab);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      Game.Vehicles.MaintenanceVehicle componentData1 = this.EntityManager.GetComponentData<Game.Vehicles.MaintenanceVehicle>(this.selectedEntity);
      MaintenanceVehicleData componentData2 = this.EntityManager.GetComponentData<MaintenanceVehicleData>(this.selectedPrefab);
      componentData2.m_MaintenanceCapacity = Mathf.CeilToInt((float) componentData2.m_MaintenanceCapacity * componentData1.m_Efficiency);
      this.workShift = Mathf.CeilToInt((float) ((1.0 - (double) math.select((float) componentData1.m_Maintained / (float) componentData2.m_MaintenanceCapacity, 0.0f, componentData2.m_MaintenanceCapacity == 0)) * 100.0));
      this.stateKey = VehicleUIUtils.GetStateKey(this.selectedEntity, componentData1, this.EntityManager);
      // ISSUE: reference to a compiler-generated method
      base.OnProcess();
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated method
      base.OnWriteProperties(writer);
      writer.PropertyName("workShift");
      writer.Write(this.workShift);
    }

    [Preserve]
    public MaintenanceVehicleSection()
    {
    }
  }
}
