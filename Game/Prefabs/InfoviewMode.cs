// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.InfoviewMode
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [InternalBufferCapacity(0)]
  public struct InfoviewMode : IBufferElementData
  {
    public Entity m_Mode;
    public int m_Priority;
    public bool m_Supplemental;
    public bool m_Optional;

    public InfoviewMode(Entity mode, int priority, bool supplemental, bool optional)
    {
      this.m_Mode = mode;
      this.m_Priority = priority;
      this.m_Supplemental = supplemental;
      this.m_Optional = optional;
    }
  }
}
