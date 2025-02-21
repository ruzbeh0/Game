// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.CargoSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Economy;
using Game.Prefabs;
using Game.Vehicles;
using System;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class CargoSection : InfoSectionBase
  {
    protected override string group => nameof (CargoSection);

    private int cargo { get; set; }

    private int capacity { get; set; }

    private CargoSection.CargoKey cargoKey { get; set; }

    private NativeList<UIResource> resources { get; set; }

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
      this.resources.Clear();
      this.cargo = 0;
      this.capacity = 0;
      this.cargoKey = CargoSection.CargoKey.Cargo;
    }

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      this.resources = new NativeList<UIResource>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
    }

    [Preserve]
    protected override void OnDestroy()
    {
      this.resources.Dispose();
      base.OnDestroy();
    }

    private bool Visible()
    {
      if (!this.EntityManager.HasComponent<Vehicle>(this.selectedEntity))
        return false;
      DynamicBuffer<LayoutElement> buffer;
      if (this.EntityManager.TryGetBuffer<LayoutElement>(this.selectedEntity, true, out buffer) && buffer.Length != 0)
      {
        for (int index = 0; index < buffer.Length; ++index)
        {
          PrefabRef component1;
          if (this.EntityManager.TryGetComponent<PrefabRef>(buffer[index].m_Vehicle, out component1))
          {
            DeliveryTruckData component2;
            if (this.EntityManager.TryGetComponent<DeliveryTruckData>(component1.m_Prefab, out component2))
            {
              this.capacity += component2.m_CargoCapacity;
            }
            else
            {
              CargoTransportVehicleData component3;
              if (this.EntityManager.TryGetComponent<CargoTransportVehicleData>(component1.m_Prefab, out component3))
                this.capacity += component3.m_CargoCapacity;
            }
          }
        }
      }
      else
      {
        DeliveryTruckData component4;
        if (this.EntityManager.TryGetComponent<DeliveryTruckData>(this.selectedPrefab, out component4))
        {
          this.capacity = component4.m_CargoCapacity;
        }
        else
        {
          CargoTransportVehicleData component5;
          if (this.EntityManager.TryGetComponent<CargoTransportVehicleData>(this.selectedPrefab, out component5))
            this.capacity = component5.m_CargoCapacity;
        }
      }
      return this.capacity > 0;
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      this.cargoKey = CargoSection.CargoKey.Cargo;
      Game.Vehicles.DeliveryTruck component1;
      if (this.EntityManager.TryGetComponent<Game.Vehicles.DeliveryTruck>(this.selectedEntity, out component1))
      {
        Resource resource = Resource.NoResource;
        DynamicBuffer<LayoutElement> buffer;
        if (this.EntityManager.TryGetBuffer<LayoutElement>(this.selectedEntity, true, out buffer) && buffer.Length != 0)
        {
          int num = 0;
          for (int index = 0; index < buffer.Length; ++index)
          {
            Game.Vehicles.DeliveryTruck component2;
            if (this.EntityManager.TryGetComponent<Game.Vehicles.DeliveryTruck>(buffer[index].m_Vehicle, out component2))
            {
              resource |= component2.m_Resource;
              if ((component2.m_State & DeliveryTruckFlags.Loaded) != (DeliveryTruckFlags) 0)
                num += component2.m_Amount;
            }
          }
          this.cargo = num;
        }
        else
        {
          resource = component1.m_Resource;
          this.cargo = (component1.m_State & DeliveryTruckFlags.Loaded) != (DeliveryTruckFlags) 0 ? component1.m_Amount : 0;
        }
        if ((resource & (Resource.UnsortedMail | Resource.LocalMail | Resource.OutgoingMail)) != Resource.NoResource)
          this.cargoKey = CargoSection.CargoKey.Mail;
        this.resources.Add(new UIResource(resource, this.cargo));
      }
      else
      {
        NativeList<Game.Economy.Resources> target = new NativeList<Game.Economy.Resources>(32, (AllocatorManager.AllocatorHandle) Allocator.Temp);
        DynamicBuffer<LayoutElement> buffer1;
        if (this.EntityManager.TryGetBuffer<LayoutElement>(this.selectedEntity, true, out buffer1))
        {
          for (int index = 0; index < buffer1.Length; ++index)
          {
            DynamicBuffer<Game.Economy.Resources> buffer2;
            if (this.EntityManager.TryGetBuffer<Game.Economy.Resources>(buffer1[index].m_Vehicle, true, out buffer2))
            {
              // ISSUE: reference to a compiler-generated method
              this.AddResources(buffer2, target);
            }
          }
        }
        else
        {
          DynamicBuffer<Game.Economy.Resources> buffer3;
          if (this.EntityManager.TryGetBuffer<Game.Economy.Resources>(this.selectedEntity, true, out buffer3))
          {
            // ISSUE: reference to a compiler-generated method
            this.AddResources(buffer3, target);
          }
        }
        for (int index = 0; index < target.Length; ++index)
        {
          Game.Economy.Resources resource = target[index];
          this.resources.Add(new UIResource(resource));
          this.cargo += resource.m_Amount;
        }
        target.Dispose();
      }
    }

    private void AddResources(DynamicBuffer<Game.Economy.Resources> source, NativeList<Game.Economy.Resources> target)
    {
label_9:
      for (int index1 = 0; index1 < source.Length; ++index1)
      {
        Game.Economy.Resources resources1 = source[index1];
        if (resources1.m_Amount != 0)
        {
          for (int index2 = 0; index2 < target.Length; ++index2)
          {
            Game.Economy.Resources resources2 = target[index2];
            if (resources2.m_Resource == resources1.m_Resource)
            {
              resources2.m_Amount += resources1.m_Amount;
              target[index2] = resources2;
              goto label_9;
            }
          }
          target.Add(in resources1);
        }
      }
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("cargo");
      writer.Write(this.cargo);
      writer.PropertyName("capacity");
      writer.Write(this.capacity);
      this.resources.Sort<UIResource>();
      writer.PropertyName("resources");
      writer.ArrayBegin(this.resources.Length);
      for (int index = 0; index < this.resources.Length; ++index)
        writer.Write<UIResource>(this.resources[index]);
      writer.ArrayEnd();
      writer.PropertyName("cargoKey");
      writer.Write(Enum.GetName(typeof (CargoSection.CargoKey), (object) this.cargoKey));
    }

    [Preserve]
    public CargoSection()
    {
    }

    private enum CargoKey
    {
      Cargo,
      Mail,
    }
  }
}
