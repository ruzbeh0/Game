// Decompiled with JetBrains decompiler
// Type: Game.Rendering.BatchRenderFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Rendering
{
  [Flags]
  public enum BatchRenderFlags : byte
  {
    MotionVectors = 1,
    ReceiveShadows = 2,
    CastShadows = 4,
    IsEnabled = 8,
    All = 255, // 0xFF
  }
}
