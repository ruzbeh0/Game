// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.ColorGradient
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.UI.Binding;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
namespace Game.UI.Widgets
{
  public struct ColorGradient : IJsonWritable
  {
    public GradientStop[] stops;

    public ColorGradient(GradientStop[] stops) => this.stops = stops;

    public static explicit operator ColorGradient(Gradient gradient)
    {
      List<GradientStop> gradientStopList = new List<GradientStop>();
      foreach (GradientColorKey colorKey in gradient.colorKeys)
        gradientStopList.Add(new GradientStop(colorKey.time, (Color32) colorKey.color));
      return new ColorGradient(gradientStopList.ToArray());
    }

    public void Write(IJsonWriter writer)
    {
      writer.TypeBegin(this.GetType().FullName);
      writer.PropertyName("stops");
      int length = this.stops != null ? this.stops.Length : 0;
      writer.ArrayBegin(length);
      for (int index = 0; index < length; ++index)
        writer.Write<GradientStop>(this.stops[index]);
      writer.ArrayEnd();
      writer.TypeEnd();
    }
  }
}
