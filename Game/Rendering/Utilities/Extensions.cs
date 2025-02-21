// Decompiled with JetBrains decompiler
// Type: Game.Rendering.Utilities.Extensions
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Rendering.Utilities
{
  public static class Extensions
  {
    public static void Fire(this Action action)
    {
      if (action == null)
        return;
      action();
    }

    public static void Fire<T>(this Action<T> action, T arg1)
    {
      if (action == null)
        return;
      action(arg1);
    }

    public static void Fire<T, U>(this Action<T, U> action, T arg1, U arg2)
    {
      if (action == null)
        return;
      action(arg1, arg2);
    }
  }
}
