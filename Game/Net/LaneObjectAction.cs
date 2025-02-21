// Decompiled with JetBrains decompiler
// Type: Game.Net.LaneObjectAction
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Net
{
  public struct LaneObjectAction : IComparable<LaneObjectAction>
  {
    public Entity m_Lane;
    public Entity m_Remove;
    public Entity m_Add;
    public float2 m_CurvePosition;

    public LaneObjectAction(Entity lane, Entity remove)
    {
      this.m_Lane = lane;
      this.m_Remove = remove;
      this.m_Add = Entity.Null;
      this.m_CurvePosition = new float2();
    }

    public LaneObjectAction(Entity lane, Entity add, float2 curvePosition)
    {
      this.m_Lane = lane;
      this.m_Remove = Entity.Null;
      this.m_Add = add;
      this.m_CurvePosition = curvePosition;
    }

    public LaneObjectAction(Entity lane, Entity remove, Entity add, float2 curvePosition)
    {
      this.m_Lane = lane;
      this.m_Remove = remove;
      this.m_Add = add;
      this.m_CurvePosition = curvePosition;
    }

    public int CompareTo(LaneObjectAction other) => this.m_Lane.Index - other.m_Lane.Index;

    public override int GetHashCode() => this.m_Lane.GetHashCode();
  }
}
