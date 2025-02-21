// Decompiled with JetBrains decompiler
// Type: Game.Routes.PathTargets
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Routes
{
  public struct PathTargets : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_StartLane;
    public Entity m_EndLane;
    public float2 m_CurvePositions;
    public float3 m_ReadyStartPosition;
    public float3 m_ReadyEndPosition;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_StartLane);
      writer.Write(this.m_EndLane);
      writer.Write(this.m_CurvePositions);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_StartLane);
      reader.Read(out this.m_EndLane);
      reader.Read(out this.m_CurvePositions);
    }
  }
}
