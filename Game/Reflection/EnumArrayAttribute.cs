// Decompiled with JetBrains decompiler
// Type: Game.Reflection.EnumArrayAttribute
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Reflection
{
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field)]
  public class EnumArrayAttribute : Attribute
  {
    public Type type { get; set; }

    public EnumArrayAttribute(Type type) => this.type = type;
  }
}
