// Decompiled with JetBrains decompiler
// Type: Game.Objects.SubObject
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;
using Unity.Entities;

#nullable disable
namespace Game.Objects
{
  [InternalBufferCapacity(0)]
  public struct SubObject : IBufferElementData, IEquatable<SubObject>, IEmptySerializable
  {
    public Entity m_SubObject;

    public SubObject(Entity subObject) => this.m_SubObject = subObject;

    public bool Equals(SubObject other) => this.m_SubObject.Equals(other.m_SubObject);

    public override int GetHashCode() => this.m_SubObject.GetHashCode();
  }
}
