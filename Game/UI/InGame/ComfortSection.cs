// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.ComfortSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.UI.Binding;
using Game.Buildings;
using Game.Routes;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class ComfortSection : InfoSectionBase
  {
    protected override string group => nameof (ComfortSection);

    private int comfort { get; set; }

    protected override void Reset() => this.comfort = 0;

    private bool Visible()
    {
      return !this.EntityManager.HasComponent<MailBox>(this.selectedEntity) && this.EntityManager.HasComponent<TransportStop>(this.selectedEntity) || this.EntityManager.HasComponent<TransportStation>(this.selectedEntity) && this.EntityManager.HasComponent<PublicTransportStation>(this.selectedEntity) || this.EntityManager.HasComponent<ParkingFacility>(this.selectedEntity);
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      float num = 0.0f;
      TransportStop component1;
      if (this.EntityManager.TryGetComponent<TransportStop>(this.selectedEntity, out component1))
      {
        num = component1.m_ComfortFactor;
        this.tooltipKeys.Add("TransportStop");
      }
      else
      {
        TransportStation component2;
        if (this.EntityManager.TryGetComponent<TransportStation>(this.selectedEntity, out component2))
        {
          num = component2.m_ComfortFactor;
          this.tooltipKeys.Add("TransportStation");
        }
        else
        {
          ParkingFacility component3;
          if (this.EntityManager.TryGetComponent<ParkingFacility>(this.selectedEntity, out component3))
          {
            num = component3.m_ComfortFactor;
            this.tooltipKeys.Add("Parking");
          }
        }
      }
      this.comfort = (int) math.round(100f * num);
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("comfort");
      writer.Write(this.comfort);
    }

    [Preserve]
    public ComfortSection()
    {
    }
  }
}
