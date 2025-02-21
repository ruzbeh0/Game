// Decompiled with JetBrains decompiler
// Type: Game.Tools.ToolMode
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

#nullable disable
namespace Game.Tools
{
  public readonly struct ToolMode
  {
    public string name { get; }

    public int index { get; }

    public ToolMode(string name, int index)
    {
      this.name = name;
      this.index = index;
    }
  }
}
