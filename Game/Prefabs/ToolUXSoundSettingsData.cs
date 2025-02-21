// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ToolUXSoundSettingsData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct ToolUXSoundSettingsData : IComponentData, IQueryTypeParameter
  {
    public Entity m_PolygonToolSelectPointSound;
    public Entity m_PolygonToolDropPointSound;
    public Entity m_PolygonToolRemovePointSound;
    public Entity m_PolygonToolDeleteAreaSound;
    public Entity m_PolygonToolFinishAreaSound;
    public Entity m_BulldozeSound;
    public Entity m_PropPlantBulldozeSound;
    public Entity m_TerraformSound;
    public Entity m_PlaceBuildingSound;
    public Entity m_RelocateBuildingSound;
    public Entity m_PlaceUpgradeSound;
    public Entity m_PlacePropSound;
    public Entity m_PlaceBuildingFailSound;
    public Entity m_ZoningFillSound;
    public Entity m_ZoningRemoveFillSound;
    public Entity m_ZoningStartPaintSound;
    public Entity m_ZoningEndPaintSound;
    public Entity m_ZoningStartRemovePaintSound;
    public Entity m_ZoningEndRemovePaintSound;
    public Entity m_ZoningMarqueeStartSound;
    public Entity m_ZoningMarqueeEndSound;
    public Entity m_ZoningMarqueeClearStartSound;
    public Entity m_ZoningMarqueeClearEndSound;
    public Entity m_SelectEntitySound;
    public Entity m_SnapSound;
    public Entity m_NetExpandSound;
    public Entity m_NetStartSound;
    public Entity m_NetNodeSound;
    public Entity m_NetBuildSound;
    public Entity m_NetCancelSound;
    public Entity m_NetElevationUpSound;
    public Entity m_NetElevationDownSound;
    public Entity m_TransportLineCompleteSound;
    public Entity m_TransportLineStartSound;
    public Entity m_TransportLineBuildSound;
    public Entity m_TransportLineRemoveSound;
    public Entity m_AreaMarqueeStartSound;
    public Entity m_AreaMarqueeEndSound;
    public Entity m_AreaMarqueeClearStartSound;
    public Entity m_AreaMarqueeClearEndSound;
    public Entity m_TutorialStartedSound;
    public Entity m_TutorialCompletedSound;
    public Entity m_CameraZoomInSound;
    public Entity m_CameraZoomOutSound;
    public Entity m_DeletetEntitySound;
  }
}
