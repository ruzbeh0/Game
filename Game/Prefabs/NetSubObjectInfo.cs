// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetSubObjectInfo
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [Serializable]
  public class NetSubObjectInfo
  {
    public ObjectPrefab m_Object;
    public float3 m_Position;
    public quaternion m_Rotation;
    public NetObjectPlacement m_Placement;
    public int m_FixedIndex;
    public bool m_AnchorTop;
    public bool m_AnchorCenter;
    public bool m_RequireElevated;
    public bool m_RequireOutsideConnection;
    public bool m_RequireDeadEnd;
    public bool m_RequireOrphan;
  }
}
