// Decompiled with JetBrains decompiler
// Type: Game.Tools.Temp
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Tools
{
  public struct Temp : IComponentData, IQueryTypeParameter
  {
    public Entity m_Original;
    public float m_CurvePosition;
    public int m_Value;
    public int m_Cost;
    public TempFlags m_Flags;

    public Temp(Entity original, TempFlags flags)
    {
      this.m_Original = original;
      this.m_CurvePosition = 0.0f;
      this.m_Value = 0;
      this.m_Cost = 0;
      this.m_Flags = flags;
    }
  }
}
