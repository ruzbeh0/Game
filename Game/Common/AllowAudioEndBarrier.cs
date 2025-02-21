// Decompiled with JetBrains decompiler
// Type: Game.Common.AllowAudioEndBarrier
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using UnityEngine.Scripting;

#nullable disable
namespace Game.Common
{
  public class AllowAudioEndBarrier : GameSystemBase
  {
    private AudioEndBarrier m_AudioEndBarrier;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_AudioEndBarrier = this.World.GetOrCreateSystemManaged<AudioEndBarrier>();
    }

    [Preserve]
    protected override void OnUpdate() => this.m_AudioEndBarrier.AllowUsage();

    [Preserve]
    public AllowAudioEndBarrier()
    {
    }
  }
}
