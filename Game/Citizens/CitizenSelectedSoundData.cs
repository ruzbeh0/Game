// Decompiled with JetBrains decompiler
// Type: Game.Citizens.CitizenSelectedSoundData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using Unity.Entities;

#nullable disable
namespace Game.Citizens
{
  [InternalBufferCapacity(0)]
  public struct CitizenSelectedSoundData : IBufferElementData, IEquatable<CitizenSelectedSoundData>
  {
    public bool m_IsSickOrInjured;
    public CitizenAge m_Age;
    public CitizenHappiness m_Happiness;
    public Entity m_SelectedSound;

    public CitizenSelectedSoundData(
      bool isSickOrInjured,
      CitizenAge age,
      CitizenHappiness happiness,
      Entity selectedSound)
    {
      this.m_IsSickOrInjured = isSickOrInjured;
      this.m_Age = age;
      this.m_Happiness = happiness;
      this.m_SelectedSound = selectedSound;
    }

    public bool Equals(CitizenSelectedSoundData other)
    {
      if (!this.m_IsSickOrInjured.Equals(other.m_IsSickOrInjured) || !this.m_Age.Equals((object) other.m_Age))
        return false;
      return this.m_IsSickOrInjured || this.m_Happiness.Equals((object) other.m_Happiness);
    }

    public override int GetHashCode()
    {
      return (this.m_IsSickOrInjured, this.m_Age, this.m_Happiness).GetHashCode();
    }
  }
}
