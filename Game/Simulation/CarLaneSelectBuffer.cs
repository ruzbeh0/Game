// Decompiled with JetBrains decompiler
// Type: Game.Simulation.CarLaneSelectBuffer
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Collections;

#nullable disable
namespace Game.Simulation
{
  public struct CarLaneSelectBuffer
  {
    private NativeArray<float> m_Buffer;

    public NativeArray<float> Ensure()
    {
      if (!this.m_Buffer.IsCreated)
        this.m_Buffer = new NativeArray<float>(64, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
      return this.m_Buffer;
    }

    public void Dispose()
    {
    }
  }
}
