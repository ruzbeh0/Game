// Decompiled with JetBrains decompiler
// Type: Game.Events.AddCriminal
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Citizens;
using Unity.Entities;

#nullable disable
namespace Game.Events
{
  public struct AddCriminal : IComponentData, IQueryTypeParameter
  {
    public Entity m_Event;
    public Entity m_Target;
    public CriminalFlags m_Flags;
  }
}
