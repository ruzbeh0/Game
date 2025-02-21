// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.LineSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using Game.Routes;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class LineSection : InfoSectionBase
  {
    protected override string group => nameof (LineSection);

    private float length { get; set; }

    private int stops { get; set; }

    private int cargo { get; set; }

    private float usage { get; set; }

    protected override void Reset()
    {
      this.length = 0.0f;
      this.stops = 0;
      this.cargo = 0;
      this.usage = 0.0f;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      int num;
      if (this.EntityManager.HasComponent<Route>(this.selectedEntity))
      {
        EntityManager entityManager = this.EntityManager;
        if (entityManager.HasComponent<TransportLine>(this.selectedEntity))
        {
          entityManager = this.EntityManager;
          num = entityManager.HasComponent<RouteWaypoint>(this.selectedEntity) ? 1 : 0;
          goto label_4;
        }
      }
      num = 0;
label_4:
      this.visible = num != 0;
    }

    protected override void OnProcess()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      this.m_InfoUISystem.SetRoutesVisible();
      int cargo = 0;
      int capacity = 0;
      TransportUIUtils.GetRouteVehiclesCount(this.EntityManager, this.selectedEntity, ref cargo, ref capacity);
      this.usage = capacity > 0 ? (float) cargo / (float) capacity : 0.0f;
      this.stops = TransportUIUtils.GetStopCount(this.EntityManager, this.selectedEntity);
      this.length = TransportUIUtils.GetRouteLength(this.EntityManager, this.selectedEntity);
      this.cargo = cargo;
      this.tooltipTags.Add(TooltipTags.CargoRoute.ToString());
      this.tooltipTags.Add(TooltipTags.TransportLine.ToString());
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("length");
      writer.Write(this.length);
      writer.PropertyName("stops");
      writer.Write(this.stops);
      writer.PropertyName("usage");
      writer.Write(this.usage);
      writer.PropertyName("cargo");
      writer.Write(this.cargo);
    }

    [Preserve]
    public LineSection()
    {
    }
  }
}
