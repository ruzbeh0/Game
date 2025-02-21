// Decompiled with JetBrains decompiler
// Type: Game.UI.UIUpdateSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using UnityEngine.Scripting;

#nullable disable
namespace Game.UI
{
  public class UIUpdateSystem : GameSystemBase
  {
    private UpdateSystem m_UpdateSystem;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_UpdateSystem = this.World.GetOrCreateSystemManaged<UpdateSystem>();
    }

    [Preserve]
    protected override void OnUpdate() => this.m_UpdateSystem.Update(SystemUpdatePhase.UIUpdate);

    [Preserve]
    public UIUpdateSystem()
    {
    }
  }
}
