// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.PoliceSection
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
  public class PoliceSection : InfoSectionBase
  {
    protected override string group => nameof (PoliceSection);

    private int prisonerCount { get; set; }

    private int prisonerCapacity { get; set; }

    protected override void Reset()
    {
      this.prisonerCount = 0;
      this.prisonerCapacity = 0;
    }

    private bool Visible() => this.EntityManager.HasComponent<Game.Buildings.PoliceStation>(this.selectedEntity);

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      PoliceStationData data;
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetComponentWithUpgrades<PoliceStationData>(this.selectedEntity, this.selectedPrefab, out data))
        this.prisonerCapacity = data.m_JailCapacity;
      DynamicBuffer<Occupant> buffer;
      if (!this.EntityManager.TryGetBuffer<Occupant>(this.selectedEntity, true, out buffer))
        return;
      this.prisonerCount = buffer.Length;
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("prisonerCount");
      writer.Write(this.prisonerCount);
      writer.PropertyName("prisonerCapacity");
      writer.Write(this.prisonerCapacity);
    }

    [Preserve]
    public PoliceSection()
    {
    }
  }
}
