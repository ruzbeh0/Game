// Decompiled with JetBrains decompiler
// Type: $BurstDirectCallInitializer
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Simulation;
using UnityEngine;

#nullable disable
internal static class \u0024BurstDirectCallInitializer
{
  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
  private static void Initialize()
  {
    SurfaceDataReader.CopyWaterValues_00005969\u0024BurstDirectCall.Initialize();
  }
}
