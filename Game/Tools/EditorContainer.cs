// Decompiled with JetBrains decompiler
// Type: Game.Tools.EditorContainer
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Tools
{
  public struct EditorContainer : IComponentData, IQueryTypeParameter, ISerializable
  {
    public Entity m_Prefab;
    public float3 m_Scale;
    public float m_Intensity;
    public int m_GroupIndex;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Prefab);
      writer.Write(this.m_Scale);
      writer.Write(this.m_Intensity);
      writer.Write(this.m_GroupIndex);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Prefab);
      reader.Read(out this.m_Scale);
      reader.Read(out this.m_Intensity);
      if (reader.context.version >= Version.editorContainerGroupIndex)
        reader.Read(out this.m_GroupIndex);
      else
        this.m_GroupIndex = -1;
    }
  }
}
