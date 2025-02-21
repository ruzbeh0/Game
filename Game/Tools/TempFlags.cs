// Decompiled with JetBrains decompiler
// Type: Game.Tools.TempFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Tools
{
  [Flags]
  public enum TempFlags : uint
  {
    Create = 1,
    Delete = 2,
    IsLast = 4,
    Essential = 8,
    Dragging = 16, // 0x00000010
    Select = 32, // 0x00000020
    Modify = 64, // 0x00000040
    Regenerate = 128, // 0x00000080
    Replace = 256, // 0x00000100
    Upgrade = 512, // 0x00000200
    Hidden = 1024, // 0x00000400
    Parent = 2048, // 0x00000800
    Combine = 4096, // 0x00001000
    RemoveCost = 8192, // 0x00002000
    Optional = 16384, // 0x00004000
    Cancel = 32768, // 0x00008000
    SubDetail = 65536, // 0x00010000
    Duplicate = 131072, // 0x00020000
  }
}
