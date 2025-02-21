// Decompiled with JetBrains decompiler
// Type: Game.Simulation.Flow.CutElement
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

#nullable disable
namespace Game.Simulation.Flow
{
  public struct CutElement
  {
    public CutElementFlags m_Flags;
    public int m_StartNode;
    public int m_EndNode;
    public int m_Edge;
    public int m_Group;
    public int m_Version;
    public int m_LinkedElements;
    public int m_NextIndex;

    public bool isCreated
    {
      get => this.GetFlag(CutElementFlags.Created);
      set => this.SetFlag(CutElementFlags.Created, value);
    }

    public bool isAdmissible
    {
      get => this.GetFlag(CutElementFlags.Admissible);
      set => this.SetFlag(CutElementFlags.Admissible, value);
    }

    public bool isChanged
    {
      get => this.GetFlag(CutElementFlags.Changed);
      set => this.SetFlag(CutElementFlags.Changed, value);
    }

    public bool isDeleted
    {
      get => this.GetFlag(CutElementFlags.Deleted);
      set => this.SetFlag(CutElementFlags.Deleted, value);
    }

    private bool GetFlag(CutElementFlags flag) => (this.m_Flags & flag) != 0;

    private void SetFlag(CutElementFlags flag, bool value)
    {
      if (value)
        this.m_Flags |= flag;
      else
        this.m_Flags &= ~flag;
    }
  }
}
