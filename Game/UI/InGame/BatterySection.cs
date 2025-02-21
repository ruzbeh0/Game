// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.BatterySection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class BatterySection : InfoSectionBase
  {
    protected override string group => nameof (BatterySection);

    private int batteryCharge { get; set; }

    private int batteryCapacity { get; set; }

    private int flow { get; set; }

    private float remainingTime { get; set; }

    protected override void Reset()
    {
      this.batteryCharge = 0;
      this.batteryCapacity = 0;
      this.flow = 0;
      this.remainingTime = 0.0f;
    }

    private bool Visible() => this.EntityManager.HasComponent<Game.Buildings.Battery>(this.selectedEntity);

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      BatteryData data;
      // ISSUE: reference to a compiler-generated method
      if (!this.TryGetComponentWithUpgrades<BatteryData>(this.selectedEntity, this.selectedPrefab, out data))
        return;
      Game.Buildings.Battery componentData = this.EntityManager.GetComponentData<Game.Buildings.Battery>(this.selectedEntity);
      this.batteryCharge = componentData.storedEnergyHours;
      this.batteryCapacity = data.m_Capacity;
      this.flow = componentData.m_LastFlow;
      if (this.flow > 0)
        this.remainingTime = math.min((float) ((data.capacityTicks - componentData.m_StoredEnergy) / (long) this.flow) / 2048f, 12f);
      else if (this.flow < 0)
        this.remainingTime = math.min((float) (componentData.m_StoredEnergy / (long) -this.flow) / 2048f, 12f);
      else
        this.remainingTime = 0.0f;
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("batteryCharge");
      writer.Write(this.batteryCharge);
      writer.PropertyName("batteryCapacity");
      writer.Write(this.batteryCapacity);
      writer.PropertyName("flow");
      writer.Write(this.flow);
      writer.PropertyName("remainingTime");
      writer.Write(this.remainingTime);
    }

    [Preserve]
    public BatterySection()
    {
    }
  }
}
