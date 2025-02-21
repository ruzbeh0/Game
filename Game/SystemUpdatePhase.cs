// Decompiled with JetBrains decompiler
// Type: Game.SystemUpdatePhase
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

#nullable disable
namespace Game
{
  public enum SystemUpdatePhase
  {
    Invalid = -1, // 0xFFFFFFFF
    MainLoop = 0,
    LateUpdate = 1,
    Modification1 = 2,
    Modification2 = 3,
    Modification2B = 4,
    Modification3 = 5,
    Modification4 = 6,
    Modification4B = 7,
    Modification5 = 8,
    ModificationEnd = 9,
    PreSimulation = 10, // 0x0000000A
    PostSimulation = 11, // 0x0000000B
    GameSimulation = 12, // 0x0000000C
    EditorSimulation = 13, // 0x0000000D
    Rendering = 14, // 0x0000000E
    PreTool = 15, // 0x0000000F
    PostTool = 16, // 0x00000010
    ToolUpdate = 17, // 0x00000011
    ClearTool = 18, // 0x00000012
    ApplyTool = 19, // 0x00000013
    Serialize = 20, // 0x00000014
    Deserialize = 21, // 0x00000015
    UIUpdate = 22, // 0x00000016
    UITooltip = 23, // 0x00000017
    PrefabUpdate = 24, // 0x00000018
    DebugGizmos = 25, // 0x00000019
    LoadSimulation = 26, // 0x0000001A
    PreCulling = 27, // 0x0000001B
    CompleteRendering = 28, // 0x0000001C
    Raycast = 29, // 0x0000001D
    PrefabReferences = 30, // 0x0000001E
    Cleanup = 31, // 0x0000001F
  }
}
