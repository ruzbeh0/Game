// Decompiled with JetBrains decompiler
// Type: Game.UI.Widgets.NumberStepAttribute
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using UnityEngine;

#nullable disable
namespace Game.UI.Widgets
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
  public class NumberStepAttribute : PropertyAttribute
  {
    public float Step { get; set; }

    public NumberStepAttribute(float step) => this.Step = step;
  }
}
