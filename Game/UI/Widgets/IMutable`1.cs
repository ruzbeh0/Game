// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.IMutable`1
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;

#nullable disable
namespace Game.UI.Widgets
{
  public interface IMutable<out T> : IWidget, IJsonWritable where T : class
  {
    T GetValue();
  }
}
