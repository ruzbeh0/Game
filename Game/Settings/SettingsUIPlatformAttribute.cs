// Decompiled with JetBrains decompiler
// Type: Game.Settings.SettingsUIPlatformAttribute
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using System;
using UnityEngine;

#nullable disable
namespace Game.Settings
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Property, Inherited = true)]
  public class SettingsUIPlatformAttribute : Attribute
  {
    private readonly Platform m_Platforms;
    private readonly bool m_DebugConditional;

    public bool IsPlatformSet(RuntimePlatform platform)
    {
      return this.m_Platforms.IsPlatformSet(platform, this.m_DebugConditional);
    }

    public SettingsUIPlatformAttribute(Platform platforms, bool debugConditional = false)
    {
      this.m_Platforms = platforms;
      this.m_DebugConditional = debugConditional;
    }
  }
}
