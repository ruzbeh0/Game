// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PrefabRef
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;

#nullable disable
namespace Game.Prefabs
{
  public struct PrefabRef : IComponentData, IQueryTypeParameter, IStrideSerializable, ISerializable
  {
    public Entity m_Prefab;

    public PrefabRef(Entity prefab) => this.m_Prefab = prefab;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Prefab);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Prefab);
    }

    public int GetStride(Context context) => 4;

    public static implicit operator Entity(PrefabRef prefabRef) => prefabRef.m_Prefab;
  }
}
