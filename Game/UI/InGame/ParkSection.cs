// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.ParkSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class ParkSection : InfoSectionBase
  {
    protected override string group => nameof (ParkSection);

    private int maintenance { get; set; }

    protected override void Reset() => this.maintenance = 0;

    private bool Visible() => this.EntityManager.HasComponent<Game.Buildings.Park>(this.selectedEntity);

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      ParkData data;
      // ISSUE: reference to a compiler-generated method
      if (!this.TryGetComponentWithUpgrades<ParkData>(this.selectedEntity, this.selectedPrefab, out data))
        return;
      this.maintenance = Mathf.CeilToInt(math.select((float) this.EntityManager.GetComponentData<Game.Buildings.Park>(this.selectedEntity).m_Maintenance / (float) data.m_MaintenancePool, 0.0f, data.m_MaintenancePool == (short) 0) * 100f);
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("maintenance");
      writer.Write(this.maintenance);
    }

    [Preserve]
    public ParkSection()
    {
    }
  }
}
