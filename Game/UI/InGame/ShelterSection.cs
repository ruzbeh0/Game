// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.ShelterSection
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
  public class ShelterSection : InfoSectionBase
  {
    protected override string group => nameof (ShelterSection);

    private int sheltered { get; set; }

    private int shelterCapacity { get; set; }

    private int consumables { get; set; }

    private int consumableCapacity { get; set; }

    protected override void Reset()
    {
      this.sheltered = 0;
      this.shelterCapacity = 0;
      this.consumables = 0;
    }

    private bool Visible()
    {
      return this.EntityManager.HasComponent<Game.Buildings.EmergencyShelter>(this.selectedEntity);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      EmergencyShelterData data;
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetComponentWithUpgrades<EmergencyShelterData>(this.selectedEntity, this.selectedPrefab, out data))
        this.shelterCapacity = data.m_ShelterCapacity;
      DynamicBuffer<Occupant> buffer;
      if (!this.EntityManager.TryGetBuffer<Occupant>(this.selectedEntity, true, out buffer))
        return;
      this.sheltered = buffer.Length;
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("sheltered");
      writer.Write(this.sheltered);
      writer.PropertyName("shelterCapacity");
      writer.Write(this.shelterCapacity);
    }

    [Preserve]
    public ShelterSection()
    {
    }
  }
}
