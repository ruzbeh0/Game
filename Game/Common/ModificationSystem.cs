// Decompiled with JetBrains decompiler
// Type: Game.Common.ModificationSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using UnityEngine.Scripting;

#nullable disable
namespace Game.Common
{
  public class ModificationSystem : GameSystemBase
  {
    private UpdateSystem m_UpdateSystem;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_UpdateSystem = this.World.GetOrCreateSystemManaged<UpdateSystem>();
    }

    [Preserve]
    protected override void OnUpdate()
    {
      this.m_UpdateSystem.Update(SystemUpdatePhase.Modification1);
      this.m_UpdateSystem.Update(SystemUpdatePhase.Modification2);
      this.m_UpdateSystem.Update(SystemUpdatePhase.Modification2B);
      this.m_UpdateSystem.Update(SystemUpdatePhase.Modification3);
      this.m_UpdateSystem.Update(SystemUpdatePhase.Modification4);
      this.m_UpdateSystem.Update(SystemUpdatePhase.Modification4B);
      this.m_UpdateSystem.Update(SystemUpdatePhase.Modification5);
      this.m_UpdateSystem.Update(SystemUpdatePhase.ModificationEnd);
    }

    [Preserve]
    public ModificationSystem()
    {
    }
  }
}
