// Decompiled with JetBrains decompiler
// Type: Game.Common.EnumValueAttribute
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using UnityEngine;

#nullable disable
namespace Game.Common
{
  public class EnumValueAttribute : PropertyAttribute
  {
    public string[] names;

    public EnumValueAttribute(System.Type type) => this.names = Enum.GetNames(type);
  }
}
