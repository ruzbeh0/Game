// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.UpdateAction
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Collections;

#nullable disable
namespace Game.Pathfind
{
  public struct UpdateAction : IDisposable
  {
    public NativeArray<UpdateActionData> m_UpdateData;

    public UpdateAction(int size, Allocator allocator)
    {
      this.m_UpdateData = new NativeArray<UpdateActionData>(size, allocator);
    }

    public void Dispose() => this.m_UpdateData.Dispose();
  }
}
