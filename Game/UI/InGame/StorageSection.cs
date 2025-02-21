// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.StorageSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Buildings;
using Game.Companies;
using Game.Economy;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class StorageSection : InfoSectionBase
  {
    private ResourceSystem m_ResourceSystem;
    private Entity m_CompanyEntity;

    protected override string group => nameof (StorageSection);

    private int stored { get; set; }

    private int capacity { get; set; }

    private NativeList<UIResource> resources { get; set; }

    protected override void Reset()
    {
      this.resources.Clear();
      this.stored = 0;
      this.capacity = 0;
      // ISSUE: reference to a compiler-generated field
      this.m_CompanyEntity = Entity.Null;
    }

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      this.resources = new NativeList<UIResource>((AllocatorManager.AllocatorHandle) Allocator.Persistent);
      // ISSUE: reference to a compiler-generated field
      this.m_ResourceSystem = this.World.GetOrCreateSystemManaged<ResourceSystem>();
    }

    [Preserve]
    protected override void OnDestroy()
    {
      this.resources.Dispose();
      base.OnDestroy();
    }

    private bool Visible()
    {
      StorageLimitData storageLimitData;
      PrefabRef component1;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.TryGetComponentWithUpgrades<StorageLimitData>(this.selectedEntity, this.selectedPrefab, out storageLimitData) || CompanyUIUtils.HasCompany(this.EntityManager, this.selectedEntity, this.selectedPrefab, out this.m_CompanyEntity) && this.EntityManager.TryGetComponent<PrefabRef>(this.m_CompanyEntity, out component1) && this.EntityManager.HasComponent<StorageCompanyData>(component1.m_Prefab) && this.EntityManager.TryGetComponent<StorageLimitData>(component1.m_Prefab, out storageLimitData))
      {
        PropertyRenter component2;
        PrefabRef component3;
        BuildingPropertyData component4;
        SpawnableBuildingData component5;
        BuildingData component6;
        this.capacity = (!this.EntityManager.TryGetComponent<PropertyRenter>(this.selectedEntity, out component2) || !this.EntityManager.TryGetComponent<PrefabRef>(component2.m_Property, out component3) || !this.EntityManager.TryGetComponent<BuildingPropertyData>(component3.m_Prefab, out component4) || !this.EntityManager.TryGetComponent<SpawnableBuildingData>(component3.m_Prefab, out component5) || !this.EntityManager.TryGetComponent<BuildingData>(component3.m_Prefab, out component6)) && (!this.EntityManager.TryGetComponent<BuildingPropertyData>(this.selectedPrefab, out component4) || !this.EntityManager.TryGetComponent<SpawnableBuildingData>(this.selectedPrefab, out component5) || !this.EntityManager.TryGetComponent<BuildingData>(this.selectedPrefab, out component6)) || component4.m_AllowedStored == Resource.NoResource ? storageLimitData.m_Limit : storageLimitData.GetAdjustedLimit(component5, component6);
      }
      return this.capacity > 0;
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      DynamicBuffer<Game.Economy.Resources> buffer;
      // ISSUE: reference to a compiler-generated field
      if (this.EntityManager.TryGetBuffer<Game.Economy.Resources>(this.selectedEntity, true, out buffer) || this.EntityManager.TryGetBuffer<Game.Economy.Resources>(this.m_CompanyEntity, true, out buffer))
      {
        for (int index = 0; index < buffer.Length; ++index)
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          if ((double) EconomyUtils.GetWeight(this.EntityManager, buffer[index].m_Resource, this.m_ResourceSystem.GetPrefabs()) != 0.0)
          {
            this.resources.Add(new UIResource(buffer[index]));
            this.stored += buffer[index].m_Amount;
          }
        }
      }
      this.stored = math.min(math.max(this.stored, 0), this.capacity);
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("stored");
      writer.Write(this.stored);
      writer.PropertyName("capacity");
      writer.Write(this.capacity);
      this.resources.Sort<UIResource>();
      writer.PropertyName("resources");
      writer.ArrayBegin(this.resources.Length);
      for (int index = 0; index < this.resources.Length; ++index)
        writer.Write<UIResource>(this.resources[index]);
      writer.ArrayEnd();
    }

    [Preserve]
    public StorageSection()
    {
    }
  }
}
