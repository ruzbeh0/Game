// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.FireVehicleSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Common;
using Game.Events;
using Game.Prefabs;
using Game.Simulation;
using Game.Vehicles;
using System;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class FireVehicleSection : VehicleSection
  {
    protected override string group => nameof (FireVehicleSection);

    private VehicleLocaleKey vehicleKey { get; set; }

    private bool Visible()
    {
      return this.EntityManager.HasComponent<Vehicle>(this.selectedEntity) && this.EntityManager.HasComponent<Game.Vehicles.FireEngine>(this.selectedEntity) && this.EntityManager.HasComponent<Owner>(this.selectedEntity);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      Game.Vehicles.FireEngine componentData = this.EntityManager.GetComponentData<Game.Vehicles.FireEngine>(this.selectedEntity);
      DynamicBuffer<ServiceDispatch> buffer;
      this.EntityManager.TryGetBuffer<ServiceDispatch>(this.selectedEntity, true, out buffer);
      this.vehicleKey = this.EntityManager.HasComponent<HelicopterData>(this.selectedPrefab) ? VehicleLocaleKey.FireHelicopter : VehicleLocaleKey.FireEngine;
      this.stateKey = VehicleUIUtils.GetStateKey(this.selectedEntity, componentData, buffer, this.EntityManager);
      if (this.stateKey == VehicleStateLocaleKey.Dispatched)
      {
        FireRescueRequest component1;
        if (!buffer.IsCreated || buffer.Length == 0 || !this.EntityManager.TryGetComponent<FireRescueRequest>(buffer[0].m_Request, out component1))
          return;
        OnFire component2;
        if (this.EntityManager.TryGetComponent<OnFire>(component1.m_Target, out component2) && component2.m_Event != Entity.Null)
        {
          this.nextStop = new VehicleUIUtils.EntityWrapper(component2.m_Event);
        }
        else
        {
          Game.Common.Destroyed component3;
          if (this.EntityManager.TryGetComponent<Game.Common.Destroyed>(component1.m_Target, out component3) && component3.m_Event != Entity.Null)
            this.nextStop = new VehicleUIUtils.EntityWrapper(component3.m_Event);
          else if (component1.m_Target != Entity.Null)
            this.nextStop = new VehicleUIUtils.EntityWrapper(component1.m_Target);
        }
      }
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
    public FireVehicleSection()
    {
    }
  }
}
