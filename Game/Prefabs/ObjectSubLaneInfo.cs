// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ObjectSubLaneInfo
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using System;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  [Serializable]
  public class ObjectSubLaneInfo
  {
    public NetLanePrefab m_LanePrefab;
    public Bezier4x3 m_BezierCurve;
    public int2 m_NodeIndex = new int2(-1, -1);
    public int2 m_ParentMesh = new int2(-1, -1);
  }
}
