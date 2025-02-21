// Decompiled with JetBrains decompiler
// Type: Game.Tools.CreationDefinition
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Tools
{
  public struct CreationDefinition : IComponentData, IQueryTypeParameter
  {
    public Entity m_Prefab;
    public Entity m_SubPrefab;
    public Entity m_Original;
    public Entity m_Owner;
    public Entity m_Attached;
    public CreationFlags m_Flags;
    public int m_RandomSeed;
  }
}
