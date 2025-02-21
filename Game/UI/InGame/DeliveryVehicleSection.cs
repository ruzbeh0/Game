// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.DeliveryVehicleSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Common;
using Game.Economy;
using Game.Vehicles;
using System;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class DeliveryVehicleSection : VehicleSection
  {
    protected override string group => nameof (DeliveryVehicleSection);

    private Resource resource { get; set; }

    private VehicleLocaleKey vehicleKey { get; set; }

    protected override void Reset()
    {
      // ISSUE: reference to a compiler-generated method
      base.Reset();
      this.resource = Resource.NoResource;
    }

    private bool Visible()
    {
      return this.EntityManager.HasComponent<Vehicle>(this.selectedEntity) && this.EntityManager.HasComponent<DeliveryTruck>(this.selectedEntity) && this.EntityManager.HasComponent<Owner>(this.selectedEntity);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      DeliveryTruck componentData = this.EntityManager.GetComponentData<DeliveryTruck>(this.selectedEntity);
      DynamicBuffer<LayoutElement> buffer;
      if (this.EntityManager.TryGetBuffer<LayoutElement>(this.selectedEntity, true, out buffer) && buffer.Length != 0)
      {
        Resource resource = Resource.NoResource;
        for (int index = 0; index < buffer.Length; ++index)
        {
          DeliveryTruck component;
          if (this.EntityManager.TryGetComponent<DeliveryTruck>(buffer[index].m_Vehicle, out component))
            resource |= component.m_Resource;
        }
        this.resource = resource;
      }
      else
        this.resource = componentData.m_Resource;
      this.stateKey = VehicleUIUtils.GetStateKey(this.selectedEntity, componentData, this.EntityManager);
      this.vehicleKey = (this.resource & (Resource.UnsortedMail | Resource.LocalMail | Resource.OutgoingMail)) != Resource.NoResource ? VehicleLocaleKey.PostTruck : VehicleLocaleKey.DeliveryTruck;
      this.tooltipKeys.Add(this.vehicleKey.ToString());
      // ISSUE: reference to a compiler-generated method
      base.OnProcess();
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      // ISSUE: reference to a compiler-generated method
      base.OnWriteProperties(writer);
      writer.PropertyName("resourceKey");
      writer.Write(Enum.GetName(typeof (Resource), (object) this.resource));
      writer.PropertyName("vehicleKey");
      writer.Write(Enum.GetName(typeof (VehicleLocaleKey), (object) this.vehicleKey));
    }

    [Preserve]
    public DeliveryVehicleSection()
    {
    }
  }
}
