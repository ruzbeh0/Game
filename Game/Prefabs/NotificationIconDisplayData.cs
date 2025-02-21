// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.NotificationIconDisplayData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Serialization.Entities;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct NotificationIconDisplayData : 
    IComponentData,
    IQueryTypeParameter,
    IEnableableComponent,
    ISerializable,
    ISerializeAsEnabled
  {
    public float2 m_MinParams;
    public float2 m_MaxParams;
    public int m_IconIndex;
    public uint m_CategoryMask;

    public void Serialize<TWriter>(TWriter writer) where TWriter : IWriter
    {
      writer.Write((this.m_MinParams + this.m_MaxParams) * 0.5f);
    }

    public void Deserialize<TReader>(TReader reader) where TReader : IReader
    {
      reader.Read(out this.m_MinParams);
      this.m_MaxParams = this.m_MinParams;
      this.m_IconIndex = 0;
      this.m_CategoryMask = 2147483648U;
    }
  }
}
