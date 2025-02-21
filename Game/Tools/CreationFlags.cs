// Decompiled with JetBrains decompiler
// Type: Game.Tools.CreationFlags
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Tools
{
  [Flags]
  public enum CreationFlags : uint
  {
    Permanent = 1,
    Select = 2,
    Delete = 4,
    Attach = 8,
    Upgrade = 16, // 0x00000010
    Relocate = 32, // 0x00000020
    Invert = 64, // 0x00000040
    Align = 128, // 0x00000080
    Hidden = 256, // 0x00000100
    Parent = 512, // 0x00000200
    Dragging = 1024, // 0x00000400
    Recreate = 2048, // 0x00000800
    Optional = 4096, // 0x00001000
    Lowered = 8192, // 0x00002000
    Native = 16384, // 0x00004000
    Construction = 32768, // 0x00008000
    SubElevation = 65536, // 0x00010000
    Duplicate = 131072, // 0x00020000
    Repair = 262144, // 0x00040000
  }
}
