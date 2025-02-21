// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.ZoneBuiltDataValue
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

#nullable disable
namespace Game.Prefabs
{
  public struct ZoneBuiltDataValue
  {
    public int m_Squares;
    public int m_Count;

    public ZoneBuiltDataValue(int count, int squares)
    {
      this.m_Count = count;
      this.m_Squares = squares;
    }
  }
}
