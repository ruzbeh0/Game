// Decompiled with JetBrains decompiler
// Type: Game.Pathfind.CreateAction
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Collections;

#nullable disable
namespace Game.Pathfind
{
  public struct CreateAction : IDisposable
  {
    public NativeArray<CreateActionData> m_CreateData;

    public CreateAction(int size, Allocator allocator)
    {
      this.m_CreateData = new NativeArray<CreateActionData>(size, allocator);
    }

    public void Dispose() => this.m_CreateData.Dispose();
  }
}
