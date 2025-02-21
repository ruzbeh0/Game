// Decompiled with JetBrains decompiler
// Type: Game.Buildings.BuildingEfficiency
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using System.Runtime.InteropServices;
using Unity.Entities;

#nullable disable
namespace Game.Buildings
{
  [StructLayout(LayoutKind.Sequential, Size = 1)]
  public struct BuildingEfficiency : IComponentData, IQueryTypeParameter, ISerializable
  {
    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((byte) 0);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out byte _);
    }
  }
}
