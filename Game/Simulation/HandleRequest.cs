// Decompiled with JetBrains decompiler
// Type: Game.Simulation.HandleRequest
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Unity.Entities;

#nullable disable
namespace Game.Simulation
{
  public struct HandleRequest : IComponentData, IQueryTypeParameter
  {
    public Entity m_Request;
    public Entity m_Handler;
    public bool m_Completed;
    public bool m_PathConsumed;

    public HandleRequest(Entity request, Entity handler, bool completed, bool pathConsumed = false)
    {
      this.m_Request = request;
      this.m_Handler = handler;
      this.m_Completed = completed;
      this.m_PathConsumed = pathConsumed;
    }
  }
}
