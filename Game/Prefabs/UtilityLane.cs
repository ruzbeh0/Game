// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.UtilityLane
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Net;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Net/", new System.Type[] {typeof (NetLanePrefab)})]
  public class UtilityLane : ComponentBase
  {
    public UtilityTypes m_UtilityType = UtilityTypes.WaterPipe;
    public NetLanePrefab m_LocalConnectionLane;
    public NetLanePrefab m_LocalConnectionLane2;
    public ObjectPrefab m_NodeObject;
    public float m_Width;
    public float m_VisualCapacity;
    public float m_Hanging;
    public bool m_Underground;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if ((UnityEngine.Object) this.m_LocalConnectionLane != (UnityEngine.Object) null)
        prefabs.Add((PrefabBase) this.m_LocalConnectionLane);
      if ((UnityEngine.Object) this.m_LocalConnectionLane2 != (UnityEngine.Object) null)
        prefabs.Add((PrefabBase) this.m_LocalConnectionLane2);
      if (!((UnityEngine.Object) this.m_NodeObject != (UnityEngine.Object) null))
        return;
      prefabs.Add((PrefabBase) this.m_NodeObject);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<UtilityLaneData>());
    }

    public override void GetArchetypeComponents(HashSet<ComponentType> components)
    {
      components.Add(ComponentType.ReadWrite<Game.Net.UtilityLane>());
      components.Add(ComponentType.ReadWrite<LaneColor>());
      if ((this.m_UtilityType & (UtilityTypes.WaterPipe | UtilityTypes.SewagePipe | UtilityTypes.LowVoltageLine | UtilityTypes.HighVoltageLine)) != UtilityTypes.None)
      {
        components.Add(ComponentType.ReadWrite<EdgeMapping>());
        components.Add(ComponentType.ReadWrite<SubFlow>());
      }
      if ((double) this.m_Hanging == 0.0)
        return;
      components.Add(ComponentType.ReadWrite<HangingLane>());
    }
  }
}
