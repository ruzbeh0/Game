// Decompiled with JetBrains decompiler
// Type: Game.Tools.SelectionInfo
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Areas;
using Unity.Entities;

#nullable disable
namespace Game.Tools
{
  public struct SelectionInfo : IComponentData, IQueryTypeParameter
  {
    public SelectionType m_SelectionType;
    public AreaType m_AreaType;
  }
}
