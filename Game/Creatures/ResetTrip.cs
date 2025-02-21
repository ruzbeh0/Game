// Decompiled with JetBrains decompiler
// Type: Game.Creatures.ResetTrip
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Citizens;
using Game.Economy;
using Unity.Entities;

#nullable disable
namespace Game.Creatures
{
  public struct ResetTrip : IComponentData, IQueryTypeParameter
  {
    public Entity m_Creature;
    public Entity m_Source;
    public Entity m_Target;
    public Entity m_DivertTarget;
    public Entity m_NextTarget;
    public Entity m_Arrived;
    public Resource m_TravelResource;
    public Resource m_DivertResource;
    public Resource m_NextResource;
    public ResidentFlags m_ResidentFlags;
    public int m_TravelData;
    public int m_DivertData;
    public int m_NextData;
    public uint m_Delay;
    public Purpose m_TravelPurpose;
    public Purpose m_DivertPurpose;
    public Purpose m_NextPurpose;
    public bool m_HasDivertPath;
  }
}
