// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.DensityAction
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Collections;

#nullable disable
namespace Game.Pathfind
{
  public struct DensityAction : IDisposable
  {
    public NativeQueue<DensityActionData> m_DensityData;

    public DensityAction(Allocator allocator)
    {
      this.m_DensityData = new NativeQueue<DensityActionData>((AllocatorManager.AllocatorHandle) allocator);
    }

    public void Dispose() => this.m_DensityData.Dispose();
  }
}
