// Decompiled with JetBrains decompiler
// Type: Game.Net.UtilityLaneFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Net
{
  [Flags]
  public enum UtilityLaneFlags
  {
    SecondaryStartAnchor = 1,
    SecondaryEndAnchor = 2,
    PipelineConnection = 4,
    CutForTraffic = 8,
  }
}
