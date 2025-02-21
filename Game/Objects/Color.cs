// Decompiled with JetBrains decompiler
// Type: Game.Objects.Color
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Objects
{
  public struct Color : IComponentData, IQueryTypeParameter, IEmptySerializable
  {
    public byte m_Index;
    public byte m_Value;
    public bool m_SubColor;

    public Color(byte index, byte value, bool subColor = false)
    {
      this.m_Index = index;
      this.m_Value = value;
      this.m_SubColor = subColor;
    }
  }
}
