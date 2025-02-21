// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.SecondaryLane
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Net/", new Type[] {typeof (NetLanePrefab)})]
  public class SecondaryLane : ComponentBase
  {
    public SecondaryLaneInfo[] m_LeftLanes;
    public SecondaryLaneInfo[] m_RightLanes;
    public SecondaryLaneInfo2[] m_CrossingLanes;
    public bool m_CanFlipSides;
    public bool m_DuplicateSides;
    public bool m_RequireParallel;
    public bool m_RequireOpposite;
    public bool m_SkipSafePedestrianOverlap;
    public bool m_SkipSafeCarOverlap;
    public bool m_SkipUnsafeCarOverlap;
    public bool m_SkipTrackOverlap;
    public bool m_SkipMergeOverlap;
    public bool m_FitToParkingSpaces;
    public bool m_EvenSpacing;
    public float3 m_PositionOffset;
    public float2 m_LengthOffset;
    public float m_CutMargin;
    public float m_CutOffset;
    public float m_CutOverlap;
    public float m_Spacing;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (this.m_LeftLanes != null)
      {
        for (int index = 0; index < this.m_LeftLanes.Length; ++index)
          prefabs.Add((PrefabBase) this.m_LeftLanes[index].m_Lane);
      }
      if (this.m_RightLanes != null)
      {
        for (int index = 0; index < this.m_RightLanes.Length; ++index)
          prefabs.Add((PrefabBase) this.m_RightLanes[index].m_Lane);
      }
      if (this.m_CrossingLanes == null)
        return;
      for (int index = 0; index < this.m_CrossingLanes.Length; ++index)
        prefabs.Add((PrefabBase) this.m_CrossingLanes[index].m_Lane);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<SecondaryLaneData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Net.SecondaryLane>());
    }
  }
}
