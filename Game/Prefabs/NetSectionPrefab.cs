// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NetSectionPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Net/Section/", new Type[] {})]
  public class NetSectionPrefab : PrefabBase
  {
    public NetSubSectionInfo[] m_SubSections;
    public NetPieceInfo[] m_Pieces;

    public override void GetDependencies(List<PrefabBase> prefabs)
    {
      base.GetDependencies(prefabs);
      if (this.m_SubSections != null)
      {
        for (int index = 0; index < this.m_SubSections.Length; ++index)
          prefabs.Add((PrefabBase) this.m_SubSections[index].m_Section);
      }
      if (this.m_Pieces == null)
        return;
      for (int index = 0; index < this.m_Pieces.Length; ++index)
        prefabs.Add((PrefabBase) this.m_Pieces[index].m_Piece);
    }

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<NetSectionData>());
      components.Add(ComponentType.ReadWrite<NetSubSection>());
      components.Add(ComponentType.ReadWrite<NetSectionPiece>());
    }
  }
}
