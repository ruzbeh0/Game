// Decompiled with JetBrains decompiler
// Type: Game.Tutorials.TutorialFireActivationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Buildings;
using Game.Common;
using Game.Events;
using Game.Objects;
using Game.Tools;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Tutorials
{
  public class TutorialFireActivationSystem : GameSystemBase
  {
    protected EntityCommandBufferSystem m_BarrierSystem;
    private EntityQuery m_BuildingFireQuery;
    private EntityQuery m_ForestFireQuery;
    private EntityQuery m_BuildingFireTutorialQuery;
    private EntityQuery m_ForestFireTutorialQuery;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_BarrierSystem = (EntityCommandBufferSystem) this.World.GetOrCreateSystemManaged<ModificationBarrier4>();
      this.m_BuildingFireQuery = this.GetEntityQuery(ComponentType.ReadOnly<OnFire>(), ComponentType.ReadOnly<Building>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      this.m_ForestFireQuery = this.GetEntityQuery(ComponentType.ReadOnly<OnFire>(), ComponentType.ReadOnly<Tree>(), ComponentType.Exclude<Temp>(), ComponentType.Exclude<Deleted>());
      this.m_BuildingFireTutorialQuery = this.GetEntityQuery(ComponentType.ReadOnly<BuildingFireActivationData>(), ComponentType.Exclude<TutorialActivated>(), ComponentType.Exclude<TutorialCompleted>());
      this.m_ForestFireTutorialQuery = this.GetEntityQuery(ComponentType.ReadOnly<ForestFireActivationData>(), ComponentType.Exclude<TutorialActivated>(), ComponentType.Exclude<TutorialCompleted>());
    }

    [Preserve]
    protected override void OnUpdate()
    {
      bool flag1 = !this.m_ForestFireQuery.IsEmptyIgnoreFilter && !this.m_ForestFireTutorialQuery.IsEmptyIgnoreFilter;
      bool flag2 = !this.m_BuildingFireQuery.IsEmptyIgnoreFilter && !this.m_BuildingFireTutorialQuery.IsEmptyIgnoreFilter;
      if (!(flag1 | flag2))
        return;
      EntityCommandBuffer commandBuffer = this.m_BarrierSystem.CreateCommandBuffer();
      if (flag1)
        commandBuffer.AddComponent<TutorialActivated>(this.m_ForestFireTutorialQuery, EntityQueryCaptureMode.AtPlayback);
      if (!flag2)
        return;
      commandBuffer.AddComponent<TutorialActivated>(this.m_BuildingFireTutorialQuery, EntityQueryCaptureMode.AtPlayback);
    }

    [Preserve]
    public TutorialFireActivationSystem()
    {
    }
  }
}
