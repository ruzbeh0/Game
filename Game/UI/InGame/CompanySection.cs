// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.CompanySection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Buildings;
using Game.Companies;
using Game.Economy;
using Game.Prefabs;
using Game.Zones;
using System;
using System.Runtime.CompilerServices;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class CompanySection : InfoSectionBase
  {
    private Entity companyEntity;

    protected override string group => nameof (CompanySection);

    private Resource input1 { get; set; }

    private Resource input2 { get; set; }

    private Resource output { get; set; }

    private Resource sells { get; set; }

    private Resource stores { get; set; }

    private int2 customers { get; set; }

    private float price { get; set; }

    protected override void Reset()
    {
      // ISSUE: reference to a compiler-generated field
      this.companyEntity = Entity.Null;
      this.input1 = Resource.NoResource;
      this.input2 = Resource.NoResource;
      this.output = Resource.NoResource;
      this.sells = Resource.NoResource;
      this.stores = Resource.NoResource;
      this.customers = int2.zero;
    }

    private bool Visible()
    {
      // ISSUE: reference to a compiler-generated field
      return CompanyUIUtils.HasCompany(this.EntityManager, this.selectedEntity, this.selectedPrefab, out this.companyEntity);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      SpawnableBuildingData component1;
      // ISSUE: reference to a compiler-generated field
      if (this.companyEntity == Entity.Null && this.EntityManager.TryGetComponent<SpawnableBuildingData>(this.selectedPrefab, out component1))
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        ZonePrefab prefab = this.m_PrefabSystem.GetPrefab<ZonePrefab>(component1.m_ZonePrefab);
        if ((UnityEngine.Object) prefab != (UnityEngine.Object) null)
        {
          switch (prefab.m_AreaType)
          {
            case AreaType.Commercial:
              this.tooltipKeys.Add("VacantCommercial");
              break;
            case AreaType.Industrial:
              this.tooltipKeys.Add(prefab.m_Office ? "VacantOffice" : "VacantIndustrial");
              break;
          }
        }
      }
      PrefabRef component2;
      IndustrialProcessData component3;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.EntityManager.TryGetBuffer<Game.Economy.Resources>(this.companyEntity, true, out DynamicBuffer<Game.Economy.Resources> _) && this.EntityManager.TryGetComponent<PrefabRef>(this.companyEntity, out component2) && this.EntityManager.TryGetComponent<IndustrialProcessData>(component2.m_Prefab, out component3))
      {
        // ISSUE: reference to a compiler-generated field
        if (this.EntityManager.HasComponent<ServiceAvailable>(this.companyEntity))
        {
          Resource resource1 = component3.m_Input1.m_Resource;
          Resource resource2 = component3.m_Input2.m_Resource;
          Resource resource3 = component3.m_Output.m_Resource;
          if (resource1 != Resource.NoResource && resource1 != resource3)
            this.input1 = resource1;
          if (resource2 != Resource.NoResource && resource2 != resource3 && resource2 != resource1)
          {
            this.input2 = resource2;
            this.tooltipKeys.Add("Requires");
          }
          this.sells = resource3;
          this.tooltipKeys.Add("Sells");
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          if (this.EntityManager.HasComponent<Game.Companies.ProcessingCompany>(this.companyEntity))
          {
            this.input1 = component3.m_Input1.m_Resource;
            this.input2 = component3.m_Input2.m_Resource;
            this.output = component3.m_Output.m_Resource;
            this.tooltipKeys.Add("Requires");
            this.tooltipKeys.Add("Produces");
          }
          else
          {
            // ISSUE: reference to a compiler-generated field
            if (this.EntityManager.HasComponent<Game.Companies.ExtractorCompany>(this.companyEntity))
            {
              this.output = component3.m_Output.m_Resource;
              this.tooltipKeys.Add("Produces");
            }
            else
            {
              // ISSUE: reference to a compiler-generated field
              if (this.EntityManager.HasComponent<Game.Companies.StorageCompany>(this.companyEntity))
              {
                this.stores = this.EntityManager.GetComponentData<StorageCompanyData>(component2.m_Prefab).m_StoredResources;
                this.tooltipKeys.Add("Stores");
              }
            }
          }
        }
      }
      LodgingProvider component4;
      DynamicBuffer<Renter> buffer;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (!this.EntityManager.TryGetComponent<LodgingProvider>(this.companyEntity, out component4) || !this.EntityManager.TryGetBuffer<Renter>(this.companyEntity, true, out buffer))
        return;
      this.customers = new int2(buffer.Length, buffer.Length + component4.m_FreeRooms);
      this.price = (float) component4.m_Price;
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("companyName");
      // ISSUE: reference to a compiler-generated field
      if (this.companyEntity == Entity.Null)
      {
        writer.WriteNull();
      }
      else
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated method
        this.m_NameSystem.BindName(writer, this.companyEntity);
      }
      writer.PropertyName("input1");
      if (this.input1 == Resource.NoResource)
        writer.WriteNull();
      else
        writer.Write(Enum.GetName(typeof (Resource), (object) this.input1));
      writer.PropertyName("input2");
      if (this.input2 == Resource.NoResource)
        writer.WriteNull();
      else
        writer.Write(Enum.GetName(typeof (Resource), (object) this.input2));
      writer.PropertyName("output");
      if (this.output == Resource.NoResource)
        writer.WriteNull();
      else
        writer.Write(Enum.GetName(typeof (Resource), (object) this.output));
      writer.PropertyName("sells");
      if (this.sells == Resource.NoResource)
        writer.WriteNull();
      else
        writer.Write(Enum.GetName(typeof (Resource), (object) this.sells));
      writer.PropertyName("stores");
      if (this.stores == Resource.NoResource)
        writer.WriteNull();
      else
        writer.Write(Enum.GetName(typeof (Resource), (object) this.stores));
      writer.PropertyName("customers");
      if (this.customers.y == 0)
        writer.WriteNull();
      else
        writer.Write(this.customers);
    }

    [Preserve]
    public CompanySection()
    {
    }

    private enum ExtractedKey
    {
      Harvested,
      Extracted,
    }
  }
}
