// Decompiled with JetBrains decompiler
// Type: Game.Simulation.Flow.Node
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System.Runtime.InteropServices;

#nullable disable
namespace Game.Simulation.Flow
{
  [StructLayout(LayoutKind.Explicit)]
  public struct Node
  {
    [FieldOffset(0)]
    public int m_FirstConnection;
    [FieldOffset(4)]
    public int m_LastConnection;
    [FieldOffset(8)]
    public int m_Height;
    [FieldOffset(12)]
    public int m_Excess;
    [FieldOffset(16)]
    public int m_Version;
    [FieldOffset(20)]
    public Identifier m_CutElementId;
    [FieldOffset(28)]
    public bool m_Retreat;
    [FieldOffset(20)]
    public int m_Distance;
    [FieldOffset(24)]
    public int m_Predecessor;
    [FieldOffset(28)]
    public bool m_Enqueued;

    public int connectionCount => this.m_LastConnection - this.m_FirstConnection;
  }
}
