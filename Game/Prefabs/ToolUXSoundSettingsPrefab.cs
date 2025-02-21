// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ToolUXSoundSettingsPrefab
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  [ComponentMenu("Settings/", new Type[] {})]
  public class ToolUXSoundSettingsPrefab : PrefabBase
  {
    public PrefabBase m_PolygonToolSelectPointSound;
    public PrefabBase m_PolygonToolDropPointSound;
    public PrefabBase m_PolygonToolRemovePointSound;
    public PrefabBase m_PolygonToolDeleteAreaSound;
    public PrefabBase m_PolygonToolFinishAreaSound;
    public PrefabBase m_BulldozeSound;
    public PrefabBase m_PropPlantBulldozeSound;
    public PrefabBase m_TerraformSound;
    public PrefabBase m_PlaceBuildingSound;
    public PrefabBase m_RelocateBuildingSound;
    public PrefabBase m_PlaceUpgradeSound;
    public PrefabBase m_PlacePropSound;
    public PrefabBase m_PlaceBuildingFailSound;
    public PrefabBase m_ZoningFillSound;
    public PrefabBase m_ZoningRemoveFillSound;
    public PrefabBase m_ZoningStartPaintSound;
    public PrefabBase m_ZoningEndPaintSound;
    public PrefabBase m_ZoningStartRemovePaintSound;
    public PrefabBase m_ZoningEndRemovePaintSound;
    public PrefabBase m_ZoningMarqueeStartSound;
    public PrefabBase m_ZoningMarqueeEndSound;
    public PrefabBase m_ZoningMarqueeClearStartSound;
    public PrefabBase m_ZoningMarqueeClearEndSound;
    public PrefabBase m_SelectEntitySound;
    public PrefabBase m_SnapSound;
    public PrefabBase m_NetExpandSound;
    public PrefabBase m_NetStartSound;
    public PrefabBase m_NetNodeSound;
    public PrefabBase m_NetBuildSound;
    public PrefabBase m_NetCancelSound;
    public PrefabBase m_NetElevationUpSound;
    public PrefabBase m_NetElevationDownSound;
    public PrefabBase m_TransportLineCompleteSound;
    public PrefabBase m_TransportLineStartSound;
    public PrefabBase m_TransportLineBuildSound;
    public PrefabBase m_TransportLineRemoveSound;
    public PrefabBase m_AreaMarqueeStartSound;
    public PrefabBase m_AreaMarqueeEndSound;
    public PrefabBase m_AreaMarqueeClearStartSound;
    public PrefabBase m_AreaMarqueeClearEndSound;
    public PrefabBase m_TutorialStartedSound;
    public PrefabBase m_TutorialCompletedSound;
    public PrefabBase m_CameraZoomInSound;
    public PrefabBase m_CameraZoomOutSound;
    public PrefabBase m_DeletetEntitySound;

    public override void GetPrefabComponents(HashSet<ComponentType> components)
    {
      base.GetPrefabComponents(components);
      components.Add(ComponentType.ReadWrite<ToolUXSoundSettingsData>());
    }

    public override void LateInitialize(EntityManager entityManager, Entity entity)
    {
      base.LateInitialize(entityManager, entity);
      // ISSUE: variable of a compiler-generated type
      PrefabSystem systemManaged = entityManager.World.GetOrCreateSystemManaged<PrefabSystem>();
      ToolUXSoundSettingsData componentData;
      // ISSUE: reference to a compiler-generated method
      componentData.m_PolygonToolSelectPointSound = systemManaged.GetEntity(this.m_PolygonToolSelectPointSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_PolygonToolDropPointSound = systemManaged.GetEntity(this.m_PolygonToolDropPointSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_PolygonToolRemovePointSound = systemManaged.GetEntity(this.m_PolygonToolRemovePointSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_PolygonToolDeleteAreaSound = systemManaged.GetEntity(this.m_PolygonToolDeleteAreaSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_PolygonToolFinishAreaSound = systemManaged.GetEntity(this.m_PolygonToolFinishAreaSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_BulldozeSound = systemManaged.GetEntity(this.m_BulldozeSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_PropPlantBulldozeSound = systemManaged.GetEntity(this.m_PropPlantBulldozeSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_TerraformSound = systemManaged.GetEntity(this.m_TerraformSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_PlaceBuildingSound = systemManaged.GetEntity(this.m_PlaceBuildingSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_RelocateBuildingSound = systemManaged.GetEntity(this.m_RelocateBuildingSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_PlaceUpgradeSound = systemManaged.GetEntity(this.m_PlaceUpgradeSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_PlaceBuildingFailSound = systemManaged.GetEntity(this.m_PlaceBuildingFailSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_ZoningFillSound = systemManaged.GetEntity(this.m_ZoningFillSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_ZoningRemoveFillSound = systemManaged.GetEntity(this.m_ZoningRemoveFillSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_ZoningStartPaintSound = systemManaged.GetEntity(this.m_ZoningStartPaintSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_ZoningEndPaintSound = systemManaged.GetEntity(this.m_ZoningEndPaintSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_ZoningStartRemovePaintSound = systemManaged.GetEntity(this.m_ZoningStartRemovePaintSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_ZoningEndRemovePaintSound = systemManaged.GetEntity(this.m_ZoningEndRemovePaintSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_ZoningMarqueeStartSound = systemManaged.GetEntity(this.m_ZoningMarqueeStartSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_ZoningMarqueeEndSound = systemManaged.GetEntity(this.m_ZoningMarqueeEndSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_ZoningMarqueeClearStartSound = systemManaged.GetEntity(this.m_ZoningMarqueeClearStartSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_ZoningMarqueeClearEndSound = systemManaged.GetEntity(this.m_ZoningMarqueeClearEndSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_SelectEntitySound = systemManaged.GetEntity(this.m_SelectEntitySound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_SnapSound = systemManaged.GetEntity(this.m_SnapSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_PlacePropSound = systemManaged.GetEntity(this.m_PlacePropSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_NetExpandSound = systemManaged.GetEntity(this.m_NetExpandSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_NetStartSound = systemManaged.GetEntity(this.m_NetStartSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_NetNodeSound = systemManaged.GetEntity(this.m_NetNodeSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_NetBuildSound = systemManaged.GetEntity(this.m_NetBuildSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_NetCancelSound = systemManaged.GetEntity(this.m_NetCancelSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_NetElevationUpSound = systemManaged.GetEntity(this.m_NetElevationUpSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_NetElevationDownSound = systemManaged.GetEntity(this.m_NetElevationDownSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_TransportLineCompleteSound = systemManaged.GetEntity(this.m_TransportLineCompleteSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_TransportLineStartSound = systemManaged.GetEntity(this.m_TransportLineStartSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_TransportLineBuildSound = systemManaged.GetEntity(this.m_TransportLineBuildSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_TransportLineRemoveSound = systemManaged.GetEntity(this.m_TransportLineRemoveSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_AreaMarqueeStartSound = systemManaged.GetEntity(this.m_AreaMarqueeStartSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_AreaMarqueeEndSound = systemManaged.GetEntity(this.m_AreaMarqueeEndSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_AreaMarqueeClearStartSound = systemManaged.GetEntity(this.m_AreaMarqueeClearStartSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_AreaMarqueeClearEndSound = systemManaged.GetEntity(this.m_AreaMarqueeClearEndSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_TutorialStartedSound = systemManaged.GetEntity(this.m_TutorialStartedSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_TutorialCompletedSound = systemManaged.GetEntity(this.m_TutorialCompletedSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_CameraZoomInSound = systemManaged.GetEntity(this.m_CameraZoomInSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_CameraZoomOutSound = systemManaged.GetEntity(this.m_CameraZoomOutSound);
      // ISSUE: reference to a compiler-generated method
      componentData.m_DeletetEntitySound = systemManaged.GetEntity(this.m_DeletetEntitySound);
      entityManager.SetComponentData<ToolUXSoundSettingsData>(entity, componentData);
    }
  }
}
