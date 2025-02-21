// Decompiled with JetBrains decompiler
// Type: Game.Objects.NetObjectFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Objects
{
  [Flags]
  public enum NetObjectFlags : byte
  {
    IsClear = 1,
    TrackPassThrough = 2,
    Backward = 4,
  }
}
