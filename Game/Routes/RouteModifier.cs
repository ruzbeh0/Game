// Decompiled with JetBrains decompiler
// Type: Game.Routes.RouteModifier
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Routes
{
  [InternalBufferCapacity(0)]
  public struct RouteModifier : IBufferElementData, ISerializable
  {
    public float2 m_Delta;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Delta);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version >= Version.modifierRefactoring)
        reader.Read(out this.m_Delta);
      else
        reader.Read(out this.m_Delta.y);
    }
  }
}
