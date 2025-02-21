// Decompiled with JetBrains decompiler
// Type: Game.Notifications.Icon
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Notifications
{
  public struct Icon : IComponentData, IQueryTypeParameter, IEquatable<Icon>, ISerializable
  {
    public float3 m_Location;
    public IconPriority m_Priority;
    public IconClusterLayer m_ClusterLayer;
    public IconFlags m_Flags;
    public int m_ClusterIndex;

    public bool Equals(Icon other)
    {
      return this.m_Location.Equals(other.m_Location) & this.m_Priority == other.m_Priority & this.m_ClusterLayer == other.m_ClusterLayer & this.m_Flags == other.m_Flags;
    }

    public override int GetHashCode() => this.m_Location.GetHashCode();

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write(this.m_Location);
      writer.Write((byte) this.m_Priority);
      writer.Write((byte) this.m_ClusterLayer);
      writer.Write((byte) this.m_Flags);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_Location);
      byte num1;
      reader.Read(out num1);
      this.m_Priority = (IconPriority) num1;
      if (!(reader.context.version >= Game.Version.iconClusteringData))
        return;
      byte num2;
      reader.Read(out num2);
      byte num3;
      reader.Read(out num3);
      this.m_ClusterLayer = (IconClusterLayer) num2;
      this.m_Flags = (IconFlags) num3;
    }
  }
}
