// Decompiled with JetBrains decompiler
// Type: Game.Tools.IconDefinition
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Notifications;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  public struct IconDefinition : IComponentData, IQueryTypeParameter
  {
    public float3 m_Location;
    public IconPriority m_Priority;
    public IconClusterLayer m_ClusterLayer;
    public IconFlags m_Flags;

    public IconDefinition(Icon icon)
    {
      this.m_Location = icon.m_Location;
      this.m_Priority = icon.m_Priority;
      this.m_ClusterLayer = icon.m_ClusterLayer;
      this.m_Flags = icon.m_Flags;
    }
  }
}
