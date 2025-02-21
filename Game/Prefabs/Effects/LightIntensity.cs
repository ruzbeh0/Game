// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.Effects.LightIntensity
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.Rendering;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Linq;
using UnityEngine.Serialization;

#nullable disable
namespace Game.Prefabs.Effects
{
  [Serializable]
  public class LightIntensity
  {
    [FormerlySerializedAs("m_LuxIntensity, m_Intensity")]
    public float m_Intensity = 10f;
    public LightUnit m_LightUnit = LightUnit.Lux;

    public static EnumMember[] ConvertToEnumMembers<TEnum>() where TEnum : Enum
    {
      return Enum.GetValues(typeof (TEnum)).Cast<TEnum>().Select<TEnum, EnumMember>((Func<TEnum, EnumMember>) (e => new EnumMember(Convert.ToUInt64((object) e), (LocalizedString) e.ToString()))).ToArray<EnumMember>();
    }
  }
}
