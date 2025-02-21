// Decompiled with JetBrains decompiler
// Type: Game.Common.BufferedEntity
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Common
{
  public struct BufferedEntity
  {
    public Entity m_Value;
    public bool m_Stored;

    public BufferedEntity(Entity value, bool stored)
    {
      this.m_Value = value;
      this.m_Stored = stored;
    }

    public override string ToString()
    {
      return string.Format("{0}: {1}, {2}: {3}", (object) "m_Value", (object) this.m_Value, (object) "m_Stored", (object) this.m_Stored);
    }
  }
}
