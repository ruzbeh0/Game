// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetLaneMeshInfo
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Prefabs
{
  [Serializable]
  public class NetLaneMeshInfo
  {
    public RenderPrefab m_Mesh;
    public bool m_RequireSafe;
    public bool m_RequireLevelCrossing;
    public bool m_RequireEditor;
    public bool m_RequireTrackCrossing;
    public bool m_RequireClear;
    public bool m_RequireLeftHandTraffic;
    public bool m_RequireRightHandTraffic;
  }
}
