// Decompiled with JetBrains decompiler
// Type: Game.Rendering.InstancePropertyAttribute
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;

#nullable disable
namespace Game.Rendering
{
  [AttributeUsage(AttributeTargets.Field)]
  public class InstancePropertyAttribute : Attribute
  {
    public InstancePropertyAttribute(
      string shaderPropertyName,
      Type dataType,
      BatchFlags requiredFlags = (BatchFlags) 0,
      int dataIndex = 0,
      bool isBuiltin = false)
    {
      this.ShaderPropertyName = shaderPropertyName;
      this.DataType = dataType;
      this.RequiredFlags = requiredFlags;
      this.DataIndex = dataIndex;
      this.IsBuiltin = isBuiltin;
    }

    public string ShaderPropertyName { get; protected set; }

    public Type DataType { get; protected set; }

    public BatchFlags RequiredFlags { get; protected set; }

    public int DataIndex { get; protected set; }

    public bool IsBuiltin { get; protected set; }
  }
}
