// Decompiled with JetBrains decompiler
// Type: Game.UI.InGame.ElectricityUIUtils
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Entities;
using Game.Net;
using Game.Prefabs;
using Unity.Entities;

#nullable disable
namespace Game.UI.InGame
{
  public static class ElectricityUIUtils
  {
    public static bool HasVoltageLayers(Layer layers)
    {
      return (layers & (Layer.PowerlineLow | Layer.PowerlineHigh)) > Layer.None;
    }

    public static VoltageLocaleKey GetVoltage(Layer layers)
    {
      switch (layers & (Layer.PowerlineLow | Layer.PowerlineHigh))
      {
        case Layer.PowerlineLow:
          return VoltageLocaleKey.Low;
        case Layer.PowerlineHigh:
          return VoltageLocaleKey.High;
        default:
          return VoltageLocaleKey.Both;
      }
    }

    public static Layer GetPowerLineLayers(EntityManager entityManager, Entity prefabEntity)
    {
      Layer layer = Layer.None;
      if (entityManager.HasComponent<TransformerData>(prefabEntity))
        layer |= Layer.PowerlineLow;
      DynamicBuffer<Game.Prefabs.SubNet> buffer;
      if (entityManager.TryGetBuffer<Game.Prefabs.SubNet>(prefabEntity, true, out buffer))
      {
        for (int index = 0; index < buffer.Length; ++index)
        {
          Entity prefab = buffer[index].m_Prefab;
          NetData component;
          if (entityManager.HasComponent<ElectricityConnectionData>(prefab) && entityManager.TryGetComponent<NetData>(prefab, out component))
            layer |= component.m_LocalConnectLayers;
        }
      }
      return layer & (Layer.PowerlineLow | Layer.PowerlineHigh);
    }
  }
}
