// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.LoadSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Prefabs;
using Game.Vehicles;
using System;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  public class LoadSection : InfoSectionBase
  {
    protected override string group => nameof (LoadSection);

    private float load { get; set; }

    private float capacity { get; set; }

    private LoadSection.LoadKey loadKey { get; set; }

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
      this.loadKey = LoadSection.LoadKey.None;
      this.load = 0.0f;
      this.capacity = 0.0f;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      FireEngineData component1;
      if (this.EntityManager.TryGetComponent<FireEngineData>(this.selectedPrefab, out component1))
      {
        this.capacity = component1.m_ExtinguishingCapacity;
      }
      else
      {
        GarbageTruckData component2;
        if (this.EntityManager.TryGetComponent<GarbageTruckData>(this.selectedPrefab, out component2))
        {
          this.capacity = (float) component2.m_GarbageCapacity;
        }
        else
        {
          PostVanData component3;
          if (this.EntityManager.TryGetComponent<PostVanData>(this.selectedPrefab, out component3))
            this.capacity = (float) component3.m_MailCapacity;
        }
      }
      this.visible = (double) this.capacity > 0.0;
    }

    protected override void OnProcess()
    {
      Game.Vehicles.FireEngine component1;
      if (this.EntityManager.TryGetComponent<Game.Vehicles.FireEngine>(this.selectedEntity, out component1))
      {
        this.load = component1.m_ExtinguishingAmount;
        this.loadKey = LoadSection.LoadKey.Water;
      }
      else
      {
        Game.Vehicles.GarbageTruck component2;
        if (this.EntityManager.TryGetComponent<Game.Vehicles.GarbageTruck>(this.selectedEntity, out component2))
        {
          this.load = (float) component2.m_Garbage;
          this.loadKey = (component2.m_State & GarbageTruckFlags.IndustrialWasteOnly) != (GarbageTruckFlags) 0 ? LoadSection.LoadKey.IndustrialWaste : LoadSection.LoadKey.Garbage;
        }
        else
        {
          Game.Vehicles.PostVan component3;
          if (this.EntityManager.TryGetComponent<Game.Vehicles.PostVan>(this.selectedEntity, out component3))
          {
            this.load = (float) (component3.m_DeliveringMail + component3.m_CollectedMail);
            this.loadKey = LoadSection.LoadKey.Mail;
          }
        }
      }
      this.tooltipKeys.Add(this.loadKey.ToString());
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("load");
      writer.Write(this.load);
      writer.PropertyName("capacity");
      writer.Write(this.capacity);
      writer.PropertyName("loadKey");
      writer.Write(Enum.GetName(typeof (LoadSection.LoadKey), (object) this.loadKey));
    }

    [Preserve]
    public LoadSection()
    {
    }

    private enum LoadKey
    {
      Water,
      IndustrialWaste,
      Garbage,
      Mail,
      None,
    }
  }
}
