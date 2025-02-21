// Decompiled with JetBrains decompiler
// Type: Game.Net.LaneColor
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Net
{
  public struct LaneColor : IComponentData, IQueryTypeParameter, IEmptySerializable
  {
    public byte m_Index;
    public byte m_Value0;
    public byte m_Value1;

    public LaneColor(byte index, byte value0, byte value1)
    {
      this.m_Index = index;
      this.m_Value0 = value0;
      this.m_Value1 = value1;
    }
  }
}
