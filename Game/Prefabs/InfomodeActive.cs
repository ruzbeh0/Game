// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.InfomodeActive
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct InfomodeActive : IComponentData, IQueryTypeParameter
  {
    public int m_Priority;
    public int m_Index;

    public InfomodeActive(int priority, int index)
    {
      this.m_Priority = priority;
      this.m_Index = index;
    }
  }
}
