// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.WaterSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class WaterSection : InfoSectionBase
  {
    protected override string group => nameof (WaterSection);

    private float pollution { get; set; }

    private int capacity { get; set; }

    private int lastProduction { get; set; }

    protected override void Reset()
    {
      this.pollution = 0.0f;
      this.capacity = 0;
      this.lastProduction = 0;
    }

    private bool Visible()
    {
      return this.EntityManager.HasComponent<Game.Buildings.WaterPumpingStation>(this.selectedEntity);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      Game.Buildings.WaterPumpingStation component;
      if (this.EntityManager.TryGetComponent<Game.Buildings.WaterPumpingStation>(this.selectedEntity, out component))
      {
        this.pollution = component.m_Pollution;
        this.capacity = component.m_Capacity;
        this.lastProduction = component.m_LastProduction;
      }
      WaterPumpingStationData data;
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetComponentWithUpgrades<WaterPumpingStationData>(this.selectedEntity, this.selectedPrefab, out data) && data.m_Capacity > 0 && data.m_Types != AllowedWaterTypes.None)
        this.tooltipKeys.Add("Pumping");
      if ((double) this.pollution <= 0.01)
        return;
      this.tooltipKeys.Add("Pollution");
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("pollution");
      writer.Write(this.pollution);
      writer.PropertyName("capacity");
      writer.Write(this.capacity);
      writer.PropertyName("lastProduction");
      writer.Write(this.lastProduction);
    }

    [Preserve]
    public WaterSection()
    {
    }
  }
}
