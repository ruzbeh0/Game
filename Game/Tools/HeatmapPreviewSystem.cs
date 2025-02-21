// Decompiled with JetBrains decompiler
// Type: Game.Tools.HeatmapPreviewSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Prefabs;
using Game.Rendering;
using Unity.Collections;
using Unity.Entities;
using UnityEngine.Scripting;

#nullable disable
namespace Game.Tools
{
  public class HeatmapPreviewSystem : GameSystemBase
  {
    private TelecomPreviewSystem m_TelecomPreviewSystem;
    private EntityQuery m_InfomodeQuery;
    private ComponentSystemBase m_LastPreviewSystem;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_TelecomPreviewSystem = this.World.GetOrCreateSystemManaged<TelecomPreviewSystem>();
      this.m_InfomodeQuery = this.GetEntityQuery(ComponentType.ReadOnly<InfomodeActive>(), ComponentType.ReadOnly<InfoviewHeatmapData>());
      this.RequireForUpdate(this.m_InfomodeQuery);
    }

    [Preserve]
    protected override void OnStopRunning()
    {
      if (this.m_LastPreviewSystem != null)
      {
        this.m_LastPreviewSystem.Enabled = false;
        this.m_LastPreviewSystem.Update();
        this.m_LastPreviewSystem = (ComponentSystemBase) null;
      }
      base.OnStopRunning();
    }

    private ComponentSystemBase GetPreviewSystem()
    {
      if (this.m_InfomodeQuery.IsEmptyIgnoreFilter)
        return (ComponentSystemBase) null;
      using (NativeArray<InfoviewHeatmapData> componentDataArray = this.m_InfomodeQuery.ToComponentDataArray<InfoviewHeatmapData>((AllocatorManager.AllocatorHandle) Allocator.TempJob))
      {
        for (int index = 0; index < componentDataArray.Length; ++index)
        {
          if (componentDataArray[index].m_Type == HeatmapData.TelecomCoverage)
            return (ComponentSystemBase) this.m_TelecomPreviewSystem;
        }
      }
      return (ComponentSystemBase) null;
    }

    [Preserve]
    protected override void OnUpdate()
    {
      ComponentSystemBase previewSystem = this.GetPreviewSystem();
      if (previewSystem != this.m_LastPreviewSystem)
      {
        if (this.m_LastPreviewSystem != null)
        {
          this.m_LastPreviewSystem.Enabled = false;
          this.m_LastPreviewSystem.Update();
        }
        this.m_LastPreviewSystem = previewSystem;
        if (this.m_LastPreviewSystem != null)
          this.m_LastPreviewSystem.Enabled = true;
      }
      previewSystem?.Update();
    }

    [Preserve]
    public HeatmapPreviewSystem()
    {
    }
  }
}
