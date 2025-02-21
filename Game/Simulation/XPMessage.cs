// Decompiled with JetBrains decompiler
// Type: Game.Simulation.XPMessage
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

#nullable disable
namespace Game.Simulation
{
  public struct XPMessage
  {
    public XPMessage(uint createdSimFrame, int amount, XPReason reason)
    {
      this.createdSimFrame = createdSimFrame;
      this.amount = amount;
      this.reason = reason;
    }

    public uint createdSimFrame { get; private set; }

    public XPReason reason { get; private set; }

    public int amount { get; private set; }
  }
}
