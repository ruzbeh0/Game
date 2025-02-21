// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.DeleteAction
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Collections;

#nullable disable
namespace Game.Pathfind
{
  public struct DeleteAction : IDisposable
  {
    public NativeArray<DeleteActionData> m_DeleteData;

    public DeleteAction(int size, Allocator allocator)
    {
      this.m_DeleteData = new NativeArray<DeleteActionData>(size, allocator);
    }

    public void Dispose() => this.m_DeleteData.Dispose();
  }
}
