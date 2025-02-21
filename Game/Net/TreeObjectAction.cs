// Decompiled with JetBrains decompiler
// Type: Game.Net.TreeObjectAction
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Mathematics;
using Unity.Entities;

#nullable disable
namespace Game.Net
{
  public struct TreeObjectAction
  {
    public Entity m_Remove;
    public Entity m_Add;
    public Bounds3 m_Bounds;

    public TreeObjectAction(Entity remove)
    {
      this.m_Remove = remove;
      this.m_Add = Entity.Null;
      this.m_Bounds = new Bounds3();
    }

    public TreeObjectAction(Entity add, Bounds3 bounds)
    {
      this.m_Remove = Entity.Null;
      this.m_Add = add;
      this.m_Bounds = bounds;
    }

    public TreeObjectAction(Entity remove, Entity add, Bounds3 bounds)
    {
      this.m_Remove = remove;
      this.m_Add = add;
      this.m_Bounds = bounds;
    }
  }
}
