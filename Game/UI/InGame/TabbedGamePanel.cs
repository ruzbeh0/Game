// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.TabbedGamePanel
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using System;

#nullable disable
namespace Game.UI.InGame
{
  public abstract class TabbedGamePanel : GamePanel, IEquatable<TabbedGamePanel>
  {
    public virtual int selectedTab { get; set; }

    protected override void BindProperties(IJsonWriter writer)
    {
      base.BindProperties(writer);
      writer.PropertyName("selectedTab");
      writer.Write(this.selectedTab);
    }

    public bool Equals(TabbedGamePanel other)
    {
      if (other == null)
        return false;
      return this == other || this.selectedTab.Equals(other.selectedTab);
    }
  }
}
