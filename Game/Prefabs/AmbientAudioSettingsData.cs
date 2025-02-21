// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.AmbientAudioSettingsData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct AmbientAudioSettingsData : IComponentData, IQueryTypeParameter
  {
    public float m_MinHeight;
    public float m_MaxHeight;
    public float m_OverlapRatio;
    public float m_MinDistanceRatio;
  }
}
