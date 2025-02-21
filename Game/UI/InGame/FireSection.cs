// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.FireSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Buildings;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class FireSection : InfoSectionBase
  {
    protected override string group => nameof (FireSection);

    private float vehicleEfficiency { get; set; }

    private bool disasterResponder { get; set; }

    protected override void Reset()
    {
      this.vehicleEfficiency = 0.0f;
      this.disasterResponder = false;
    }

    private bool Visible() => this.EntityManager.HasComponent<Game.Buildings.FireStation>(this.selectedEntity);

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      FireStationData data;
      DynamicBuffer<Efficiency> buffer;
      // ISSUE: reference to a compiler-generated method
      if (!this.TryGetComponentWithUpgrades<FireStationData>(this.selectedEntity, this.selectedPrefab, out data) || !this.EntityManager.TryGetBuffer<Efficiency>(this.selectedEntity, true, out buffer))
        return;
      this.disasterResponder = data.m_DisasterResponseCapacity > 0;
      float efficiency = BuildingUtils.GetEfficiency(buffer);
      this.vehicleEfficiency = (float) ((double) data.m_VehicleEfficiency * (0.5 + (double) efficiency * 0.5) * 100.0);
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("vehicleEfficiency");
      writer.Write(this.vehicleEfficiency);
      writer.PropertyName("disasterResponder");
      writer.Write(this.disasterResponder);
    }

    [Preserve]
    public FireSection()
    {
    }
  }
}
