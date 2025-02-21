// Decompiled with JetBrains decompiler
// Type: Game.Modding.Toolchain.DeploymentState
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

#nullable disable
namespace Game.Modding.Toolchain
{
  public enum DeploymentState
  {
    Unknown = -1, // 0xFFFFFFFF
    Installed = 0,
    NotInstalled = 1,
    Outdated = 2,
    Invalid = 3,
  }
}
