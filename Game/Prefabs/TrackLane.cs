// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TrackLane
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Net;
using Game.Simulation;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Net/", new System.Type[] {typeof (NetLanePrefab)})]
  public class TrackLane : ComponentBase
  {
    public NetLanePrefab m_FallbackLane;
    public ObjectPrefab m_EndObject;
    public TrackTypes m_TrackType = TrackTypes.Train;
    public float m_Width = 4f;
    public float m_MaxCurviness = 1.8f;
    public bool m_Twoway;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if ((UnityEngine.Object) this.m_FallbackLane != (UnityEngine.Object) null)
        prefabs.Add((PrefabBase) this.m_FallbackLane);
      if (!((UnityEngine.Object) this.m_EndObject != (UnityEngine.Object) null))
        return;
      prefabs.Add((PrefabBase) this.m_EndObject);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<TrackLaneData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Net.TrackLane>());
      if (components.Contains(ComponentType.ReadWrite<MasterLane>()))
        return;
      components.Add(ComponentType.ReadWrite<LaneObject>());
      components.Add(ComponentType.ReadWrite<Game.Net.LaneReservation>());
      components.Add(ComponentType.ReadWrite<LaneColor>());
      components.Add(ComponentType.ReadWrite<UpdateFrame>());
      if (components.Contains(ComponentType.ReadWrite<EdgeLane>()))
        return;
      components.Add(ComponentType.ReadWrite<LaneOverlap>());
    }
  }
}
