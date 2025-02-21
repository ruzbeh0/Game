// Decompiled with JetBrains decompiler
// Type: Game.Tools.ToolOutputSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using UnityEngine.Scripting;

#nullable disable
namespace Game.Tools
{
  public class ToolOutputSystem : GameSystemBase
  {
    private ToolSystem m_ToolSystem;
    private UpdateSystem m_UpdateSystem;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_ToolSystem = this.World.GetOrCreateSystemManaged<ToolSystem>();
      this.m_UpdateSystem = this.World.GetOrCreateSystemManaged<UpdateSystem>();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      switch (this.m_ToolSystem.applyMode)
      {
        case ApplyMode.Apply:
          this.m_UpdateSystem.Update(SystemUpdatePhase.ApplyTool);
          break;
        case ApplyMode.Clear:
          this.m_UpdateSystem.Update(SystemUpdatePhase.ClearTool);
          break;
      }
    }

    [Preserve]
    public ToolOutputSystem()
    {
    }
  }
}
