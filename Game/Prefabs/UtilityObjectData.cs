// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.UtilityObjectData
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Net;
using Unity.Entities;
using Unity.Mathematics;

#nullable disable
namespace Game.Prefabs
{
  public struct UtilityObjectData : IComponentData, IQueryTypeParameter
  {
    public UtilityTypes m_UtilityTypes;
    public float3 m_UtilityPosition;
  }
}
