// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.SewageSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Prefabs;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class SewageSection : InfoSectionBase
  {
    protected override string group => nameof (SewageSection);

    private float capacity { get; set; }

    private float lastProcessed { get; set; }

    private float lastPurified { get; set; }

    private float purification { get; set; }

    protected override void Reset()
    {
      this.capacity = 0.0f;
      this.lastProcessed = 0.0f;
      this.lastPurified = 0.0f;
      this.purification = 0.0f;
    }

    private bool Visible() => this.EntityManager.HasComponent<Game.Buildings.SewageOutlet>(this.selectedEntity);

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      Game.Buildings.SewageOutlet componentData = this.EntityManager.GetComponentData<Game.Buildings.SewageOutlet>(this.selectedEntity);
      this.capacity = (float) componentData.m_Capacity;
      this.lastProcessed = (float) componentData.m_LastProcessed;
      this.lastPurified = (float) componentData.m_LastPurified;
      SewageOutletData data;
      // ISSUE: reference to a compiler-generated method
      if (this.TryGetComponentWithUpgrades<SewageOutletData>(this.selectedEntity, this.selectedPrefab, out data))
        this.purification = data.m_Purification;
      // ISSUE: reference to a compiler-generated method
      this.tooltipKeys.Add(this.HasWaterSource() ? "Outlet" : "Treatment");
      if ((double) this.purification <= 0.0)
        return;
      if (this.EntityManager.HasComponent<Game.Buildings.WaterPumpingStation>(this.selectedEntity))
        this.tooltipKeys.Add("TreatmentPurification");
      else
        this.tooltipKeys.Add("OutletPurification");
    }

    private bool HasWaterSource()
    {
      DynamicBuffer<Game.Objects.SubObject> buffer;
      if (this.EntityManager.TryGetBuffer<Game.Objects.SubObject>(this.selectedEntity, true, out buffer))
      {
        for (int index = 0; index < buffer.Length; ++index)
        {
          if (this.EntityManager.HasComponent<Game.Simulation.WaterSourceData>(buffer[index].m_SubObject))
            return true;
        }
      }
      return false;
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("capacity");
      writer.Write(this.capacity);
      writer.PropertyName("lastProcessed");
      writer.Write(this.lastProcessed);
      writer.PropertyName("lastPurified");
      writer.Write(this.lastPurified);
      writer.PropertyName("purification");
      writer.Write(this.purification);
    }

    [Preserve]
    public SewageSection()
    {
    }
  }
}
