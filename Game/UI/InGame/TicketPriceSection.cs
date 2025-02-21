// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.TicketPriceSection
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Colossal.Serialization.Entities;
using Colossal.UI.Binding;
using Game.Policies;
using Game.Prefabs;
using Game.Routes;
using System;
using System.Runtime.CompilerServices;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.InGame
{
  [CompilerGenerated]
  public class TicketPriceSection : InfoSectionBase
  {
    private PoliciesUISystem m_PoliciesUISystem;
    private Entity m_TicketPricePolicy;
    private EntityQuery m_ConfigQuery;

    protected override string group => nameof (TicketPriceSection);

    private UIPolicySlider sliderData { get; set; }

    protected override void Reset()
    {
    }

    [Preserve]
    protected override void OnCreate()
    {
      // ISSUE: reference to a compiler-generated method
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_PoliciesUISystem = this.World.GetOrCreateSystemManaged<PoliciesUISystem>();
      // ISSUE: reference to a compiler-generated field
      this.m_ConfigQuery = this.GetEntityQuery(ComponentType.ReadOnly<UITransportConfigurationData>());
      this.AddBinding((IBinding) new TriggerBinding<int>(this.group, "setTicketPrice", (Action<int>) (newPrice =>
      {
        // ISSUE: reference to a compiler-generated field
        // ISSUE: variable of a compiler-generated type
        PoliciesUISystem policiesUiSystem = this.m_PoliciesUISystem;
        Entity selectedEntity = this.selectedEntity;
        // ISSUE: reference to a compiler-generated field
        Entity ticketPricePolicy = this.m_TicketPricePolicy;
        int num1 = newPrice > 0 ? 1 : 0;
        double num2 = (double) newPrice;
        UIPolicySlider sliderData = this.sliderData;
        double min = (double) sliderData.range.min;
        sliderData = this.sliderData;
        double max = (double) sliderData.range.max;
        double adjustment = (double) Mathf.Clamp((float) num2, (float) min, (float) max);
        // ISSUE: reference to a compiler-generated method
        policiesUiSystem.SetPolicy(selectedEntity, ticketPricePolicy, num1 != 0, (float) adjustment);
      })));
    }

    protected override void OnGameLoaded(Context serializationContext)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.m_ConfigQuery.IsEmptyIgnoreFilter)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.m_TicketPricePolicy = this.m_PrefabSystem.GetEntity((PrefabBase) this.m_PrefabSystem.GetSingletonPrefab<UITransportConfigurationPrefab>(this.m_ConfigQuery).m_TicketPricePolicy);
    }

    private void OnSetTicketPrice(int newPrice)
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: variable of a compiler-generated type
      PoliciesUISystem policiesUiSystem = this.m_PoliciesUISystem;
      Entity selectedEntity = this.selectedEntity;
      // ISSUE: reference to a compiler-generated field
      Entity ticketPricePolicy = this.m_TicketPricePolicy;
      int num1 = newPrice > 0 ? 1 : 0;
      double num2 = (double) newPrice;
      UIPolicySlider sliderData = this.sliderData;
      double min = (double) sliderData.range.min;
      sliderData = this.sliderData;
      double max = (double) sliderData.range.max;
      double adjustment = (double) Mathf.Clamp((float) num2, (float) min, (float) max);
      // ISSUE: reference to a compiler-generated method
      policiesUiSystem.SetPolicy(selectedEntity, ticketPricePolicy, num1 != 0, (float) adjustment);
    }

    private bool Visible()
    {
      TransportLineData component;
      return this.EntityManager.HasComponent<Route>(this.selectedEntity) && this.EntityManager.HasComponent<TransportLine>(this.selectedEntity) && this.EntityManager.HasComponent<RouteWaypoint>(this.selectedEntity) && this.EntityManager.HasComponent<Policy>(this.selectedEntity) && this.EntityManager.TryGetComponent<TransportLineData>(this.selectedPrefab, out component) && !component.m_CargoTransport;
    }

    [Preserve]
    protected override void OnUpdate() => this.visible = this.Visible();

    protected override void OnProcess()
    {
      DynamicBuffer<Policy> buffer = this.EntityManager.GetBuffer<Policy>(this.selectedEntity, true);
      // ISSUE: reference to a compiler-generated field
      PolicySliderData componentData = this.EntityManager.GetComponentData<PolicySliderData>(this.m_TicketPricePolicy);
      for (int index = 0; index < buffer.Length; ++index)
      {
        // ISSUE: reference to a compiler-generated field
        if (!(buffer[index].m_Policy != this.m_TicketPricePolicy))
        {
          this.sliderData = new UIPolicySlider((buffer[index].m_Flags & PolicyFlags.Active) != (PolicyFlags) 0 ? buffer[index].m_Adjustment : 0.0f, componentData);
          return;
        }
      }
      this.sliderData = new UIPolicySlider(0.0f, componentData);
    }

    public override void OnWriteProperties(IJsonWriter writer)
    {
      writer.PropertyName("sliderData");
      writer.Write<UIPolicySlider>(this.sliderData);
    }

    [Preserve]
    public TicketPriceSection()
    {
    }
  }
}
