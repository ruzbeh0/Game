// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.PrefabID
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;

#nullable disable
namespace Game.Prefabs
{
  public struct PrefabID : IEquatable<PrefabID>, ISerializable
  {
    private string m_Type;
    private string m_Name;

    public PrefabID(PrefabBase prefab)
    {
      this.m_Type = prefab.GetType().Name;
      this.m_Name = prefab.name;
    }

    public PrefabID(string type, string name)
    {
      this.m_Type = type;
      this.m_Name = name;
    }

    public bool Equals(PrefabID other)
    {
      return this.m_Type.Equals(other.m_Type) && this.m_Name.Equals(other.m_Name);
    }

    public override int GetHashCode() => this.m_Name.GetHashCode();

    public override string ToString()
    {
      return string.Format("{0}:{1}", (object) this.m_Type, (object) this.m_Name);
    }

    public string GetName() => this.m_Name;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Type);
      writer.Write(this.m_Name);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      if (reader.context.version < Game.Version.newPrefabID)
      {
        string str;
        reader.Read(out str);
        string[] strArray = str.Split(':', StringSplitOptions.None);
        this.m_Type = strArray[0];
        this.m_Name = strArray[1];
      }
      else
      {
        reader.Read(out this.m_Type);
        reader.Read(out this.m_Name);
      }
      if (!(reader.context.version < Game.Version.staticObjectPrefab) || !(this.m_Type == "ObjectGeometryPrefab"))
        return;
      this.m_Type = "StaticObjectPrefab";
    }
  }
}
