// Decompiled with JetBrains decompiler
// Type: Game.Input.IDisableableProcessor
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

#nullable disable
namespace Game.Input
{
  public interface IDisableableProcessor
  {
    const bool kDefaultCanBeDisabled = true;

    bool canBeDisabled { get; }

    bool disabled { get; set; }
  }
}
