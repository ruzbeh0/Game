// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.SecondaryLaneInfo
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Prefabs
{
  [Serializable]
  public class SecondaryLaneInfo
  {
    public NetLanePrefab m_Lane;
    public bool m_RequireSafe;
    public bool m_RequireUnsafe;
    public bool m_RequireSingle;
    public bool m_RequireMultiple;
    public bool m_RequireAllowPassing;
    public bool m_RequireForbidPassing;
    public bool m_RequireMerge;
    public bool m_RequireContinue;
    public bool m_RequireSafeMaster;

    public SecondaryNetLaneFlags GetFlags()
    {
      SecondaryNetLaneFlags flags = (SecondaryNetLaneFlags) 0;
      if (this.m_RequireSafe)
        flags |= SecondaryNetLaneFlags.RequireSafe;
      if (this.m_RequireUnsafe)
        flags |= SecondaryNetLaneFlags.RequireUnsafe;
      if (this.m_RequireSingle)
        flags |= SecondaryNetLaneFlags.RequireSingle;
      if (this.m_RequireMultiple)
        flags |= SecondaryNetLaneFlags.RequireMultiple;
      if (this.m_RequireAllowPassing)
        flags |= SecondaryNetLaneFlags.RequireAllowPassing;
      if (this.m_RequireForbidPassing)
        flags |= SecondaryNetLaneFlags.RequireForbidPassing;
      if (this.m_RequireMerge)
        flags |= SecondaryNetLaneFlags.RequireMerge;
      if (this.m_RequireContinue)
        flags |= SecondaryNetLaneFlags.RequireContinue;
      if (this.m_RequireSafeMaster)
        flags |= SecondaryNetLaneFlags.RequireSafeMaster;
      return flags;
    }
  }
}
