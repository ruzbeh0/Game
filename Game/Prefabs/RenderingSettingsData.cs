// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.RenderingSettingsData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  public struct RenderingSettingsData : IComponentData, IQueryTypeParameter
  {
    public Color m_HoveredColor;
    public Color m_OverrideColor;
    public Color m_WarningColor;
    public Color m_ErrorColor;
    public Color m_OwnerColor;
  }
}
