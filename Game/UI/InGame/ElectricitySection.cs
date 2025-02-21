// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.ElectricitySection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Buildings;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class ElectricitySection : InfoSectionBase
  {
    protected override string group => nameof (ElectricitySection);

    private int capacity { get; set; }

    private int production { get; set; }

    protected override void Reset()
    {
      this.capacity = 0;
      this.production = 0;
    }

    private bool Visible()
    {
      return this.EntityManager.HasComponent<ElectricityProducer>(this.selectedEntity);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      ElectricityProducer componentData = this.EntityManager.GetComponentData<ElectricityProducer>(this.selectedEntity);
      this.capacity = componentData.m_Capacity;
      this.production = componentData.m_LastProduction;
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetComponentWithUpgrades<SolarPoweredData>(this.selectedEntity, this.selectedPrefab, out SolarPoweredData _))
        this.tooltipKeys.Add("Solar");
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetComponentWithUpgrades<WindPoweredData>(this.selectedEntity, this.selectedPrefab, out WindPoweredData _))
        this.tooltipKeys.Add("Wind");
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetComponentWithUpgrades<GarbagePoweredData>(this.selectedEntity, this.selectedPrefab, out GarbagePoweredData _))
        this.tooltipKeys.Add("Garbage");
      if (!this.EntityManager.HasComponent<Game.Buildings.WaterPowered>(this.selectedEntity))
        return;
      this.tooltipKeys.Add("Water");
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("capacity");
      writer.Write(this.capacity);
      writer.PropertyName("production");
      writer.Write(this.production);
    }

    [Preserve]
    public ElectricitySection()
    {
    }
  }
}
