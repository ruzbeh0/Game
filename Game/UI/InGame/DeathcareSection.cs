// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.DeathcareSection
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
  public class DeathcareSection : InfoSectionBase
  {
    protected override string group => nameof (DeathcareSection);

    private int bodyCount { get; set; }

    private int bodyCapacity { get; set; }

    private float processingSpeed { get; set; }

    private float processingCapacity { get; set; }

    protected override void Reset()
    {
      this.bodyCount = 0;
      this.bodyCapacity = 0;
      this.processingSpeed = 0.0f;
      this.processingCapacity = 0.0f;
    }

    private bool Visible()
    {
      return this.EntityManager.HasComponent<Game.Buildings.DeathcareFacility>(this.selectedEntity);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      DeathcareFacilityData data;
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetComponentWithUpgrades<DeathcareFacilityData>(this.selectedEntity, this.selectedPrefab, out data))
      {
        this.bodyCapacity = data.m_StorageCapacity;
        this.tooltipKeys.Add(data.m_LongTermStorage ? "Cemetery" : "Crematorium");
      }
      DynamicBuffer<Efficiency> buffer1;
      if (this.EntityManager.TryGetBuffer<Efficiency>(this.selectedEntity, true, out buffer1))
        this.processingSpeed = data.m_ProcessingRate * BuildingUtils.GetEfficiency(buffer1);
      this.bodyCount = this.EntityManager.GetComponentData<Game.Buildings.DeathcareFacility>(this.selectedEntity).m_LongTermStoredCount;
      this.processingCapacity = data.m_ProcessingRate;
      DynamicBuffer<Patient> buffer2;
      if (this.EntityManager.TryGetBuffer<Patient>(this.selectedEntity, true, out buffer2))
        this.bodyCount += buffer2.Length;
      if (this.bodyCount > 0)
        return;
      this.processingSpeed = 0.0f;
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("bodyCount");
      writer.Write(this.bodyCount);
      writer.PropertyName("bodyCapacity");
      writer.Write(this.bodyCapacity);
      writer.PropertyName("processingSpeed");
      writer.Write(this.processingSpeed);
      writer.PropertyName("processingCapacity");
      writer.Write(this.processingCapacity);
    }

    [Preserve]
    public DeathcareSection()
    {
    }
  }
}
