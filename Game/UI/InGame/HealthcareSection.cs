// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.HealthcareSection
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
  public class HealthcareSection : InfoSectionBase
  {
    protected override string group => nameof (HealthcareSection);

    private int patientCount { get; set; }

    private int patientCapacity { get; set; }

    protected override void Reset()
    {
      this.patientCount = 0;
      this.patientCapacity = 0;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      HospitalData data;
      // ISSUE: reference to a compiler-generated method
      if (this.EntityManager.HasComponent<Game.Buildings.Hospital>(this.selectedEntity) && this.TryGetComponentWithUpgrades<HospitalData>(this.selectedEntity, this.selectedPrefab, out data))
        this.patientCapacity = data.m_PatientCapacity;
      this.visible = this.patientCapacity > 0;
    }

    protected override void OnProcess()
    {
      DynamicBuffer<Patient> buffer;
      if (!this.EntityManager.TryGetBuffer<Patient>(this.selectedEntity, true, out buffer))
        return;
      this.patientCount = buffer.Length;
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("patientCount");
      writer.Write(this.patientCount);
      writer.PropertyName("patientCapacity");
      writer.Write(this.patientCapacity);
    }

    [Preserve]
    public HealthcareSection()
    {
    }
  }
}
