// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.GarbageSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Areas;
using Game.Buildings;
using Game.Economy;
using Game.Prefabs;
using System;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class GarbageSection : InfoSectionBase
  {
    protected override string group => nameof (GarbageSection);

    private int garbage { get; set; }

    private int garbageCapacity { get; set; }

    private int processingSpeed { get; set; }

    private int processingCapacity { get; set; }

    private GarbageSection.LoadKey loadKey { get; set; }

    protected override void Reset()
    {
      this.garbage = 0;
      this.garbageCapacity = 0;
      this.processingSpeed = 0;
      this.processingCapacity = 0;
      this.loadKey = GarbageSection.LoadKey.Garbage;
    }

    private bool Visible() => this.EntityManager.HasComponent<Game.Buildings.GarbageFacility>(this.selectedEntity);

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      Game.Buildings.GarbageFacility component1;
      if (this.EntityManager.TryGetComponent<Game.Buildings.GarbageFacility>(this.selectedEntity, out component1))
      {
        DynamicBuffer<Game.Economy.Resources> buffer;
        if (this.EntityManager.TryGetBuffer<Game.Economy.Resources>(this.selectedEntity, true, out buffer))
          this.garbage = EconomyUtils.GetResources(Resource.Garbage, buffer);
        GarbageFacilityData data;
        // ISSUE: reference to a compiler-generated method
        if (this.TryGetComponentWithUpgrades<GarbageFacilityData>(this.selectedEntity, this.selectedPrefab, out data))
        {
          this.garbageCapacity = data.m_GarbageCapacity;
          this.processingSpeed = component1.m_ProcessingRate;
          this.processingCapacity = data.m_ProcessingSpeed;
          if (data.m_LongTermStorage)
            this.tooltipKeys.Add("Landfill");
          if (this.EntityManager.HasComponent<Game.Buildings.ResourceProducer>(this.selectedEntity))
            this.tooltipKeys.Add("RecyclingCenter");
          if (this.EntityManager.HasComponent<ElectricityProducer>(this.selectedEntity))
            this.tooltipKeys.Add("Incinerator");
          if (data.m_IndustrialWasteOnly)
          {
            this.tooltipKeys.Add("HazardousWaste");
            this.loadKey = GarbageSection.LoadKey.IndustrialWaste;
          }
        }
      }
      DynamicBuffer<Game.Areas.SubArea> buffer1;
      if (!this.EntityManager.TryGetBuffer<Game.Areas.SubArea>(this.selectedEntity, true, out buffer1))
        return;
      for (int index = 0; index < buffer1.Length; ++index)
      {
        Entity area = buffer1[index].m_Area;
        Storage component2;
        if (this.EntityManager.TryGetComponent<Storage>(area, out component2))
        {
          PrefabRef componentData1 = this.EntityManager.GetComponentData<PrefabRef>(area);
          Geometry componentData2 = this.EntityManager.GetComponentData<Geometry>(area);
          StorageAreaData component3;
          if (this.EntityManager.TryGetComponent<StorageAreaData>(componentData1.m_Prefab, out component3))
          {
            this.garbageCapacity += AreaUtils.CalculateStorageCapacity(componentData2, component3);
            this.garbage += component2.m_Amount;
          }
        }
      }
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("garbage");
      writer.Write(this.garbage);
      writer.PropertyName("garbageCapacity");
      writer.Write(this.garbageCapacity);
      writer.PropertyName("processingSpeed");
      writer.Write(this.processingSpeed);
      writer.PropertyName("processingCapacity");
      writer.Write(this.processingCapacity);
      writer.PropertyName("loadKey");
      writer.Write(Enum.GetName(typeof (GarbageSection.LoadKey), (object) this.loadKey));
    }

    [Preserve]
    public GarbageSection()
    {
    }

    private enum LoadKey
    {
      IndustrialWaste,
      Garbage,
    }
  }
}
