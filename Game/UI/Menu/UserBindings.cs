// Decompiled with JetBrains decompiler
// Type: Game.UI.Menu.UserBindings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.PSI.Common;
using Colossal.Serialization.Entities;
using Colossal.UI.Binding;
using Game.SceneFlow;
using System;

#nullable disable
namespace Game.UI.Menu
{
  public class UserBindings : CompositeBinding, IDisposable
  {
    private const string kGroup = "user";
    private ValueBinding<bool> m_SwitchPromptVisible;
    private ValueBinding<string> m_AvatarBinding;
    private ValueBinding<string> m_UserIDBinding;
    private ValueBinding<string> m_SwitchUserHintOverload;
    private static int s_AvatarVersion;

    public UserBindings()
    {
      GameManager.instance.onGameLoadingComplete += new GameManager.EventGamePreload(this.OnMainMenuReached);
      this.AddBinding((IBinding) (this.m_SwitchPromptVisible = new ValueBinding<bool>("user", "switchPromptVisible", !GameManager.instance.configuration.disableUserSection && (PlatformManager.instance.supportsUserSwitching || PlatformManager.instance.supportsUserSection))));
      this.AddBinding((IBinding) (this.m_AvatarBinding = new ValueBinding<string>("user", "avatar", string.Format("{0}/UserAvatar#{1}?size={2}", (object) "useravatar://", (object) UserBindings.s_AvatarVersion++, (object) AvatarSize.Auto), (IWriter<string>) new StringWriter().Nullable<string>())));
      this.AddBinding((IBinding) (this.m_UserIDBinding = new ValueBinding<string>("user", "userID", PlatformManager.instance.userName, (IWriter<string>) new StringWriter().Nullable<string>())));
      this.AddBinding((IBinding) (this.m_SwitchUserHintOverload = new ValueBinding<string>("user", "switchUserHintOverload", this.getSwitchUserHintOverload(), (IWriter<string>) new StringWriter().Nullable<string>())));
      this.AddBinding((IBinding) new TriggerBinding("user", "switchUser", new Action(this.SwitchUser)));
      PlatformManager.instance.onStatusChanged += (OnStatusChangedEventHandler) (psi =>
      {
        if (!PlatformManager.instance.IsPrincipalOverlayIntegration(psi))
          return;
        this.m_SwitchPromptVisible.Update(!GameManager.instance.configuration.disableUserSection && (PlatformManager.instance.supportsUserSwitching || PlatformManager.instance.supportsUserSection));
      });
      PlatformManager.instance.onUserUpdated += (OnUserUpdatedEventHandler) ((psi, flags) =>
      {
        if (!PlatformManager.instance.IsPrincipalUserIntegration((IPlatformServiceIntegration) psi))
          return;
        if (flags.HasChanged(UserChangedFlags.Name))
          this.m_UserIDBinding.Update(PlatformManager.instance.userName);
        if (!flags.HasChanged(UserChangedFlags.Avatar))
          return;
        this.m_AvatarBinding.Update(string.Format("{0}/UserAvatar#{1}?size={2}", (object) "useravatar://", (object) UserBindings.s_AvatarVersion++, (object) AvatarSize.Auto));
      });
    }

    private void OnMainMenuReached(Purpose purpose, GameMode mode)
    {
      if (mode != GameMode.MainMenu)
        return;
      this.m_SwitchPromptVisible.Update(!GameManager.instance.configuration.disableUserSection && (PlatformManager.instance.supportsUserSwitching || PlatformManager.instance.supportsUserSection));
    }

    public void Dispose()
    {
      GameManager.instance.onGameLoadingComplete -= new GameManager.EventGamePreload(this.OnMainMenuReached);
    }

    public string getSwitchUserHintOverload()
    {
      return PlatformManager.instance.supportsUserSwitching ? (string) null : "Steam Overlay";
    }

    private void SwitchUser()
    {
      if (!this.m_SwitchPromptVisible.value)
        return;
      PlatformManager instance = PlatformManager.instance;
      if (instance.supportsUserSwitching)
      {
        GameManager.instance.SetScreenActive<SwitchUserScreen>();
      }
      else
      {
        if (!instance.supportsUserSection)
          return;
        instance.ShowOverlay(Colossal.PSI.Common.Page.Community, (string) null);
      }
    }
  }
}
