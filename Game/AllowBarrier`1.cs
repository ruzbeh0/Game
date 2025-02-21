// Decompiled with JetBrains decompiler
// Type: Game.AllowBarrier`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using UnityEngine.Scripting;

#nullable disable
namespace Game
{
  public class AllowBarrier<T> : GameSystemBase where T : SafeCommandBufferSystem
  {
    private T m_Barrier;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_Barrier = this.World.GetOrCreateSystemManaged<T>();
    }

    [Preserve]
    protected override void OnUpdate() => this.m_Barrier.AllowUsage();

    [Preserve]
    public AllowBarrier()
    {
    }
  }
}
