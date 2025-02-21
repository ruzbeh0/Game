// Decompiled with JetBrains decompiler
// Type: Game.PSI.PlatformSupport
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.PSI.Common;
using Colossal.PSI.Discord;
using Colossal.PSI.Steamworks;
using System;

#nullable disable
namespace Game.PSI
{
  public static class PlatformSupport
  {
    private const uint kSteamAppId = 949230;
    public static readonly Func<IPlatformServiceIntegration> kCreateSteamPlatform = (Func<IPlatformServiceIntegration>) (() => (IPlatformServiceIntegration) new SteamworksPlatform(949230U));
    private const long kDiscordClientId = 1125009418476605441;
    public static readonly Func<IPlatformServiceIntegration> kCreateDiscordRichPresence = (Func<IPlatformServiceIntegration>) (() => (IPlatformServiceIntegration) new DiscordRichPresence(1125009418476605441L));
  }
}
