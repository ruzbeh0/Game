// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.PassengersSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Common;
using Game.Events;
using Game.Prefabs;
using Game.Vehicles;
using System;
using System.Runtime.CompilerServices;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class PassengersSection : InfoSectionBase
  {
    protected override string group => nameof (PassengersSection);

    private int passengers { get; set; }

    private int maxPassengers { get; set; }

    private int pets { get; set; }

    private VehiclePassengerLocaleKey vehiclePassengerKey { get; set; }

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
      this.passengers = 0;
      this.maxPassengers = 0;
      this.pets = 0;
    }

    private bool Visible()
    {
      if (!this.EntityManager.HasComponent<Vehicle>(this.selectedEntity) || !this.EntityManager.HasComponent<Passenger>(this.selectedEntity) || this.EntityManager.HasComponent<InvolvedInAccident>(this.selectedEntity))
        return false;
      PersonalCarData component1;
      if (this.EntityManager.HasComponent<Game.Vehicles.PersonalCar>(this.selectedEntity) && this.EntityManager.TryGetComponent<PersonalCarData>(this.selectedPrefab, out component1))
        return component1.m_PassengerCapacity > 0;
      TaxiData component2;
      if (this.EntityManager.HasComponent<Game.Vehicles.Taxi>(this.selectedEntity) && this.EntityManager.TryGetComponent<TaxiData>(this.selectedPrefab, out component2))
        return component2.m_PassengerCapacity > 0;
      PoliceCarData component3;
      if (this.EntityManager.HasComponent<Game.Vehicles.PoliceCar>(this.selectedEntity) && this.EntityManager.TryGetComponent<PoliceCarData>(this.selectedPrefab, out component3))
        return component3.m_CriminalCapacity > 0;
      int num = 0;
      DynamicBuffer<LayoutElement> buffer;
      if (this.EntityManager.TryGetBuffer<LayoutElement>(this.selectedEntity, true, out buffer))
      {
        for (int index = 0; index < buffer.Length; ++index)
        {
          PrefabRef component4;
          PublicTransportVehicleData component5;
          if (this.EntityManager.TryGetComponent<PrefabRef>(buffer[index].m_Vehicle, out component4) && this.EntityManager.TryGetComponent<PublicTransportVehicleData>(component4.m_Prefab, out component5))
            num += component5.m_PassengerCapacity;
        }
      }
      else
      {
        PublicTransportVehicleData component6;
        if (this.EntityManager.TryGetComponent<PublicTransportVehicleData>(this.selectedPrefab, out component6))
          num = component6.m_PassengerCapacity;
      }
      return num > 0;
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      DynamicBuffer<LayoutElement> buffer1;
      if (this.EntityManager.TryGetBuffer<LayoutElement>(this.selectedEntity, true, out buffer1))
      {
        for (int index1 = 0; index1 < buffer1.Length; ++index1)
        {
          Entity vehicle = buffer1[index1].m_Vehicle;
          DynamicBuffer<Passenger> buffer2;
          if (this.EntityManager.TryGetBuffer<Passenger>(vehicle, true, out buffer2))
          {
            for (int index2 = 0; index2 < buffer2.Length; ++index2)
            {
              if (this.EntityManager.HasComponent<Game.Creatures.Pet>(buffer2[index2].m_Passenger))
                ++this.pets;
              else
                ++this.passengers;
            }
          }
          PrefabRef component;
          if (this.EntityManager.TryGetComponent<PrefabRef>(vehicle, out component))
          {
            // ISSUE: reference to a compiler-generated method
            this.AddPassengerCapacity(component.m_Prefab);
          }
        }
      }
      else
      {
        DynamicBuffer<Passenger> buffer3;
        if (this.EntityManager.TryGetBuffer<Passenger>(this.selectedEntity, true, out buffer3))
        {
          for (int index = 0; index < buffer3.Length; ++index)
          {
            if (this.EntityManager.HasComponent<Game.Creatures.Pet>(buffer3[index].m_Passenger))
              ++this.pets;
            else
              ++this.passengers;
          }
        }
        // ISSUE: reference to a compiler-generated method
        this.AddPassengerCapacity(this.selectedPrefab);
      }
      bool flag = false;
      int min = 0;
      this.vehiclePassengerKey = VehiclePassengerLocaleKey.Passenger;
      Game.Vehicles.PersonalCar component1;
      if (this.EntityManager.TryGetComponent<Game.Vehicles.PersonalCar>(this.selectedEntity, out component1))
      {
        flag = (component1.m_State & PersonalCarFlags.DummyTraffic) > (PersonalCarFlags) 0;
        min = math.min(1, this.maxPassengers);
      }
      else
      {
        Game.Vehicles.PublicTransport component2;
        if (this.EntityManager.TryGetComponent<Game.Vehicles.PublicTransport>(this.selectedEntity, out component2))
        {
          if ((component2.m_State & PublicTransportFlags.PrisonerTransport) != (PublicTransportFlags) 0)
            this.vehiclePassengerKey = VehiclePassengerLocaleKey.Prisoner;
          flag = (component2.m_State & PublicTransportFlags.DummyTraffic) > (PublicTransportFlags) 0;
        }
      }
      PseudoRandomSeed component3;
      if (flag && this.EntityManager.TryGetComponent<PseudoRandomSeed>(this.selectedEntity, out component3))
      {
        Unity.Mathematics.Random random = component3.GetRandom((uint) PseudoRandomSeed.kDummyPassengers);
        this.passengers = math.max(this.passengers, random.NextInt(min, this.maxPassengers + 1));
        if (this.passengers != 0)
        {
          int num = random.NextInt(0, (this.passengers + random.NextInt(10)) / 10 + 1);
          for (int index = 0; index < num; ++index)
            this.pets += math.max(1, random.NextInt(0, 4));
        }
      }
      this.passengers = math.max(0, this.passengers);
    }

    private void AddPassengerCapacity(Entity prefab)
    {
      PersonalCarData component1;
      if (this.EntityManager.TryGetComponent<PersonalCarData>(prefab, out component1))
      {
        this.maxPassengers += component1.m_PassengerCapacity;
      }
      else
      {
        TaxiData component2;
        if (this.EntityManager.TryGetComponent<TaxiData>(prefab, out component2))
        {
          this.maxPassengers += component2.m_PassengerCapacity;
        }
        else
        {
          PoliceCarData component3;
          if (this.EntityManager.TryGetComponent<PoliceCarData>(prefab, out component3))
          {
            this.maxPassengers += component3.m_CriminalCapacity;
          }
          else
          {
            PublicTransportVehicleData component4;
            if (!this.EntityManager.TryGetComponent<PublicTransportVehicleData>(prefab, out component4))
              return;
            this.maxPassengers += component4.m_PassengerCapacity;
          }
        }
      }
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("passengers");
      writer.Write(this.passengers);
      writer.PropertyName("maxPassengers");
      writer.Write(this.maxPassengers);
      writer.PropertyName("pets");
      writer.Write(this.pets);
      writer.PropertyName("vehiclePassengerKey");
      writer.Write(Enum.GetName(typeof (VehiclePassengerLocaleKey), (object) this.vehiclePassengerKey));
    }

    [Preserve]
    public PassengersSection()
    {
    }
  }
}
