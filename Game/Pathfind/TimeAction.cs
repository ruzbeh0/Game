// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.TimeAction
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Collections;

#nullable disable
namespace Game.Pathfind
{
  public struct TimeAction : IDisposable
  {
    public NativeQueue<TimeActionData> m_TimeData;

    public TimeAction(Allocator allocator)
    {
      this.m_TimeData = new NativeQueue<TimeActionData>((AllocatorManager.AllocatorHandle) allocator);
    }

    public void Dispose() => this.m_TimeData.Dispose();
  }
}
