// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.PoliceVehicleSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Citizens;
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
  public class PoliceVehicleSection : VehicleSection
  {
    protected override string group => nameof (PoliceVehicleSection);

    private Entity criminalEntity { get; set; }

    private VehicleLocaleKey vehicleKey { get; set; }

    protected override void Reset()
    {
      // ISSUE: reference to a compiler-generated method
      base.Reset();
      this.criminalEntity = Entity.Null;
    }

    private bool Visible()
    {
      return this.EntityManager.HasComponent<Vehicle>(this.selectedEntity) && this.EntityManager.HasComponent<Game.Vehicles.PoliceCar>(this.selectedEntity) && this.EntityManager.HasComponent<Owner>(this.selectedEntity);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      Game.Vehicles.PoliceCar componentData1 = this.EntityManager.GetComponentData<Game.Vehicles.PoliceCar>(this.selectedEntity);
      PoliceCarData componentData2 = this.EntityManager.GetComponentData<PoliceCarData>(this.selectedPrefab);
      DynamicBuffer<ServiceDispatch> buffer1;
      this.EntityManager.TryGetBuffer<ServiceDispatch>(this.selectedEntity, true, out buffer1);
      DynamicBuffer<Passenger> buffer2;
      EntityManager entityManager;
      if (this.EntityManager.TryGetBuffer<Passenger>(this.selectedEntity, true, out buffer2))
      {
        for (int index = 0; index < buffer2.Length; ++index)
        {
          Entity entity = buffer2[index].m_Passenger;
          Game.Creatures.Resident component;
          if (this.EntityManager.TryGetComponent<Game.Creatures.Resident>(entity, out component))
            entity = component.m_Citizen;
          entityManager = this.EntityManager;
          if (entityManager.HasComponent<Citizen>(entity))
            this.criminalEntity = entity;
        }
      }
      entityManager = this.EntityManager;
      this.vehicleKey = entityManager.HasComponent<HelicopterData>(this.selectedPrefab) ? VehicleLocaleKey.PoliceHelicopter : VehicleUIUtils.GetPoliceVehicleLocaleKey(componentData2.m_PurposeMask);
      this.stateKey = VehicleUIUtils.GetStateKey(this.selectedEntity, componentData1, buffer1, this.EntityManager);
      if ((componentData1.m_State & PoliceCarFlags.AccidentTarget) != (PoliceCarFlags) 0 && (componentData1.m_State & PoliceCarFlags.AtTarget) == (PoliceCarFlags) 0)
      {
        PoliceEmergencyRequest component1;
        if (componentData1.m_RequestCount <= 0 || !buffer1.IsCreated || buffer1.Length <= 0 || !this.EntityManager.TryGetComponent<PoliceEmergencyRequest>(buffer1[0].m_Request, out component1))
          return;
        AccidentSite component2;
        if (this.EntityManager.TryGetComponent<AccidentSite>(component1.m_Site, out component2) && component2.m_Event != Entity.Null)
          this.nextStop = new VehicleUIUtils.EntityWrapper(component2.m_Event);
        else
          this.nextStop = new VehicleUIUtils.EntityWrapper(component1.m_Target);
      }
      this.tooltipKeys.Add(this.vehicleKey.ToString());
      // ISSUE: reference to a compiler-generated method
      base.OnProcess();
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated method
      base.OnWriteProperties(writer);
      writer.PropertyName("criminal");
      if (this.criminalEntity == Entity.Null)
      {
        writer.WriteNull();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NameSystem.BindName(writer, this.criminalEntity);
      }
      writer.PropertyName("criminalEntity");
      if (this.criminalEntity == Entity.Null)
        writer.WriteNull();
      else
        writer.Write(this.criminalEntity);
      writer.PropertyName("vehicleKey");
      writer.Write(Enum.GetName(typeof (VehicleLocaleKey), (object) this.vehicleKey));
    }

    [Preserve]
    public PoliceVehicleSection()
    {
    }
  }
}
