// Decompiled with JetBrains decompiler
// Type: Game.Rendering.MaterialPropertyAttribute
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Rendering
{
  [AttributeUsage(AttributeTargets.Field)]
  public class MaterialPropertyAttribute : Attribute
  {
    public MaterialPropertyAttribute(string shaderPropertyName, Type dataType, bool isBuiltin = false)
    {
      this.ShaderPropertyName = shaderPropertyName;
      this.DataType = dataType;
      this.IsBuiltin = isBuiltin;
    }

    public string ShaderPropertyName { get; protected set; }

    public Type DataType { get; protected set; }

    public bool IsBuiltin { get; protected set; }
  }
}
