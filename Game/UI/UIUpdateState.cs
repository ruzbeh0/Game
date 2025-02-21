// Decompiled with JetBrains decompiler
// Type: Game.UI.UIUpdateState
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Simulation;
using Unity.Entities;

#nullable disable
namespace Game.UI
{
  public class UIUpdateState
  {
    private readonly SimulationSystem m_SimulationSystem;
    private readonly uint m_UpdateInterval;
    private bool m_ForceUpdate;
    private uint m_LastTickIndex;

    private UIUpdateState(World world, int updateInterval)
    {
      this.m_SimulationSystem = world.GetOrCreateSystemManaged<SimulationSystem>();
      this.m_UpdateInterval = (uint) updateInterval;
      this.m_ForceUpdate = true;
    }

    public static UIUpdateState Create(World world, int updateInterval)
    {
      return new UIUpdateState(world, updateInterval);
    }

    public bool Advance()
    {
      uint num = this.m_SimulationSystem.frameIndex - this.m_LastTickIndex;
      if (!this.m_ForceUpdate && num < this.m_UpdateInterval)
        return false;
      this.m_LastTickIndex = this.m_SimulationSystem.frameIndex;
      this.m_ForceUpdate = false;
      return true;
    }

    public void ForceUpdate() => this.m_ForceUpdate = true;
  }
}
