// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.UITransportConfigurationPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new Type[] {})]
  public class UITransportConfigurationPrefab : PrefabBase
  {
    public InfoviewPrefab m_TransportInfoview;
    public InfomodePrefab m_RoutesInfomode;
    public PolicyPrefab m_TicketPricePolicy;
    public PolicyPrefab m_OutOfServicePolicy;
    public PolicyPrefab m_VehicleCountPolicy;
    public PolicyPrefab m_DayRoutePolicy;
    public PolicyPrefab m_NightRoutePolicy;
    public UITransportSummaryItem[] m_PassengerSummaryItems;
    public UITransportSummaryItem[] m_CargoSummaryItems;
    public UITransportItem[] m_PassengerLineTypes;
    public UITransportItem[] m_CargoLineTypes;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      prefabs.Add((PrefabBase) this.m_TransportInfoview);
      prefabs.Add((PrefabBase) this.m_TicketPricePolicy);
      prefabs.Add((PrefabBase) this.m_OutOfServicePolicy);
      prefabs.Add((PrefabBase) this.m_VehicleCountPolicy);
      prefabs.Add((PrefabBase) this.m_DayRoutePolicy);
      prefabs.Add((PrefabBase) this.m_NightRoutePolicy);
      for (int index = 0; index < this.m_PassengerLineTypes.Length; ++index)
        prefabs.Add(this.m_PassengerLineTypes[index].m_Unlockable);
      for (int index = 0; index < this.m_CargoLineTypes.Length; ++index)
        prefabs.Add(this.m_CargoLineTypes[index].m_Unlockable);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<UITransportConfigurationData>());
    }
  }
}
