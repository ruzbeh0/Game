// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.CoverageAction
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Collections;
using System;
using Unity.Collections;

#nullable disable
namespace Game.Pathfind
{
  public struct CoverageAction : IDisposable
  {
    public NativeReference<CoverageActionData> m_Data;

    public CoverageAction(Allocator allocator)
    {
      this.m_Data = new NativeReference<CoverageActionData>(new CoverageActionData(allocator), (AllocatorManager.AllocatorHandle) allocator);
    }

    public ref CoverageActionData data => ref this.m_Data.ValueAsRef<CoverageActionData>();

    public void Dispose()
    {
      this.data.Dispose();
      this.m_Data.Dispose();
    }
  }
}
