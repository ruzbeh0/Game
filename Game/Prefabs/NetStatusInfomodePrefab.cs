// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetStatusInfomodePrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Tools/Infomode/", new System.Type[] {})]
  public class NetStatusInfomodePrefab : GradientInfomodeBasePrefab
  {
    public NetStatusType m_Type;
    public Bounds1 m_Range;
    public float m_FlowSpeed;
    public float m_FlowTiling;
    public float m_MinFlow;

    public override string infomodeTypeLocaleKey => "NetworkColor";

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<InfoviewNetStatusData>());
    }

    public override void Initialize(EntityManager entityManager, Entity entity)
    {
      base.Initialize(entityManager, entity);
      InfoviewNetStatusData componentData = new InfoviewNetStatusData();
      componentData.m_Type = this.m_Type;
      componentData.m_Range = this.m_Range;
      if ((double) this.m_FlowTiling != 0.0)
        componentData.m_Tiling = 1f / this.m_FlowTiling;
      entityManager.SetComponentData<InfoviewNetStatusData>(entity, componentData);
    }

    public override bool CanActivateBoth(InfomodePrefab other)
    {
      return (!(other is NetStatusInfomodePrefab statusInfomodePrefab) || !this.VisibleOnRoadSurface() || !statusInfomodePrefab.VisibleOnRoadSurface()) && base.CanActivateBoth(other);
    }

    public override void GetColors(
      out Color color0,
      out Color color1,
      out Color color2,
      out float steps,
      out float speed,
      out float tiling,
      out float fill)
    {
      base.GetColors(out color0, out color1, out color2, out steps, out speed, out tiling, out fill);
      speed = this.m_FlowSpeed;
      if ((double) this.m_FlowTiling != 0.0)
        tiling = 1f / this.m_FlowTiling;
      if ((double) this.m_MinFlow == 0.0)
        return;
      fill = 1f / this.m_MinFlow;
    }

    private bool VisibleOnRoadSurface()
    {
      switch (this.m_Type)
      {
        case NetStatusType.Wear:
        case NetStatusType.TrafficFlow:
        case NetStatusType.NoisePollutionSource:
        case NetStatusType.AirPollutionSource:
        case NetStatusType.TrafficVolume:
          return true;
        default:
          return false;
      }
    }
  }
}
