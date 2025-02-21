// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.TrackPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Net;
using Game.PSI;
using Game.Simulation;
using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Net/Prefab/", new Type[] {})]
  public class TrackPrefab : NetGeometryPrefab
  {
    public TrackTypes m_TrackType = TrackTypes.Train;
    public float m_SpeedLimit = 100f;

    public override void GetDependencies(List<PrefabBase> prefabs) => base.GetDependencies(prefabs);

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<TrackData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      base.GetArchetypeComponents(components);
      if (components.Contains(ComponentType.ReadWrite<Edge>()))
      {
        components.Add(ComponentType.ReadWrite<UpdateFrame>());
        components.Add(ComponentType.ReadWrite<EdgeColor>());
        this.AddTrackType(components);
      }
      else if (components.Contains(ComponentType.ReadWrite<Game.Net.Node>()))
      {
        components.Add(ComponentType.ReadWrite<UpdateFrame>());
        components.Add(ComponentType.ReadWrite<NodeColor>());
        this.AddTrackType(components);
      }
      else
      {
        if (!components.Contains(ComponentType.ReadWrite<NetCompositionData>()))
          return;
        components.Add(ComponentType.ReadWrite<TrackComposition>());
      }
    }

    private void AddTrackType(HashSet<ComponentType> components)
    {
      switch (this.m_TrackType)
      {
        case TrackTypes.Train:
          components.Add(ComponentType.ReadWrite<TrainTrack>());
          break;
        case TrackTypes.Tram:
          components.Add(ComponentType.ReadWrite<TramTrack>());
          break;
        case TrackTypes.Subway:
          components.Add(ComponentType.ReadWrite<SubwayTrack>());
          break;
      }
    }

    public override IEnumerable<string> modTags
    {
      get
      {
        foreach (string modTag in base.modTags)
          yield return modTag;
        foreach (string enumFlagTag in ModTags.GetEnumFlagTags<TrackTypes>(this.m_TrackType, TrackTypes.Train))
          yield return enumFlagTag + "Track";
      }
    }
  }
}
