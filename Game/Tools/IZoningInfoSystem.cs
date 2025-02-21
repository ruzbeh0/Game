// Decompiled with JetBrains decompiler
// Type: Game.Tools.IZoningInfoSystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Simulation;
using Unity.Collections;

#nullable disable
namespace Game.Tools
{
  public interface IZoningInfoSystem
  {
    NativeList<ZoneEvaluationUtils.ZoningEvaluationResult> evaluationResults { get; }
  }
}
