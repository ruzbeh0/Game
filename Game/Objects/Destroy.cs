// Decompiled with JetBrains decompiler
// Type: Game.Objects.Destroy
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Objects
{
  public struct Destroy : IComponentData, IQueryTypeParameter
  {
    public Entity m_Object;
    public Entity m_Event;

    public Destroy(Entity _object, Entity _event)
    {
      this.m_Object = _object;
      this.m_Event = _event;
    }
  }
}
