// Decompiled with JetBrains decompiler
// Type: Game.Input.BuiltInUsages
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Input
{
  [Flags]
  public enum BuiltInUsages
  {
    Menu = 1,
    DefaultTool = 2,
    Overlay = 4,
    Tool = 8,
    CancelableTool = 16, // 0x00000010
    Debug = 32, // 0x00000020
    Editor = 64, // 0x00000040
    PhotoMode = 128, // 0x00000080
    Options = 256, // 0x00000100
    Tutorial = 512, // 0x00000200
    DiscardableTool = 1024, // 0x00000400
    All = DiscardableTool | Tutorial | Options | PhotoMode | Editor | Debug | CancelableTool | Tool | Overlay | DefaultTool | Menu, // 0x000007FF
    DefaultSet = DiscardableTool | CancelableTool | Tool | Overlay | DefaultTool, // 0x0000041E
  }
}
