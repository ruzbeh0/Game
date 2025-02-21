// Decompiled with JetBrains decompiler
// Type: Game.Input.ModifiersComparer
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#nullable disable
namespace Game.Input
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct ModifiersComparer : IComparer<float>
  {
    public int Compare(float x, float y)
    {
      if (float.IsNaN(x))
        return 1;
      if (float.IsNaN(y))
        return -1;
      float num1 = Math.Abs(x);
      float num2 = Math.Abs(y);
      if ((double) num1 > (double) num2)
        return -1;
      return (double) num1 >= (double) num2 ? 0 : 1;
    }
  }
}
