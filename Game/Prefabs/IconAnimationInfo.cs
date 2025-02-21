// Decompiled with JetBrains decompiler
// Type: Game.Prefabs.IconAnimationInfo
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using System;
using UnityEngine;

#nullable disable
namespace Game.Prefabs
{
  [Serializable]
  public class IconAnimationInfo
  {
    public Game.Notifications.AnimationType m_Type;
    public float m_Duration;
    public AnimationCurve m_Scale;
    public AnimationCurve m_Alpha;
    public AnimationCurve m_ScreenY;
  }
}
