// Decompiled with JetBrains decompiler
// Type: Game.UI.Menu.ParadoxBindings
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.PSI.Common;
using Colossal.PSI.PdxSdk;
using Colossal.UI.Binding;
using PDX.SDK.Contracts.Enums;
using PDX.SDK.Contracts.Service.Mods.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

#nullable disable
namespace Game.UI.Menu
{
  public class ParadoxBindings : CompositeBinding
  {
    private const string kGroup = "paradox";
    private readonly ValueBinding<bool> m_RequestActiveBinding;
    private readonly ValueBinding<bool> m_LoggedInBinding;
    private readonly ValueBinding<AccountLinkProvider> m_AccountLinkProviderBinding;
    private readonly ValueBinding<int> m_AccountLinkStateBinding;
    private readonly ValueBinding<string> m_UserNameBinding;
    private readonly ValueBinding<string> m_EmailBinding;
    private readonly ValueBinding<string> m_AvatarBinding;
    private readonly ValueBinding<bool> m_HasInternetConnection;
    private readonly ValueBinding<bool> m_IsPDXSDKEnabled;
    private readonly StackBinding<ParadoxBindings.ParadoxDialog> m_ActiveDialogsBinding;
    private PdxSdkPlatform m_PdxPlatform;
    private static readonly string kTermsOfUse = "TERMS_OF_USE";
    private static readonly string kPrivacyPolicy = "PRIVACY_POLICY";

    public ParadoxBindings()
    {
      this.AddBinding((IBinding) (this.m_RequestActiveBinding = new ValueBinding<bool>("paradox", "requestActive", false)));
      this.AddBinding((IBinding) (this.m_LoggedInBinding = new ValueBinding<bool>("paradox", "loggedIn", false)));
      this.AddBinding((IBinding) (this.m_AccountLinkProviderBinding = new ValueBinding<AccountLinkProvider>("paradox", "accountLinkProvider", AccountLinkProvider.Unknown, (IWriter<AccountLinkProvider>) new EnumNameWriter<AccountLinkProvider>())));
      this.AddBinding((IBinding) (this.m_AccountLinkStateBinding = new ValueBinding<int>("paradox", "accountLinkState", 0)));
      this.AddBinding((IBinding) (this.m_UserNameBinding = new ValueBinding<string>("paradox", "userName", (string) null, (IWriter<string>) new StringWriter().Nullable<string>())));
      this.AddBinding((IBinding) (this.m_EmailBinding = new ValueBinding<string>("paradox", "email", (string) null, (IWriter<string>) new StringWriter().Nullable<string>())));
      this.AddBinding((IBinding) (this.m_AvatarBinding = new ValueBinding<string>("paradox", "avatar", (string) null, (IWriter<string>) new StringWriter().Nullable<string>())));
      this.AddBinding((IBinding) new TriggerBinding("paradox", "linkAccount", new Action(this.LinkAccount)));
      this.AddBinding((IBinding) new TriggerBinding("paradox", "unlinkAccount", new Action(this.UnlinkAccount)));
      this.AddBinding((IBinding) new TriggerBinding("paradox", "logout", new Action(this.Logout)));
      this.AddBinding((IBinding) (this.m_ActiveDialogsBinding = new StackBinding<ParadoxBindings.ParadoxDialog>("paradox", "activeDialogs", (IWriter<ParadoxBindings.ParadoxDialog>) new ValueWriter<ParadoxBindings.ParadoxDialog>())));
      this.AddBinding((IBinding) new TriggerBinding("paradox", "closeActiveDialog", new Action(this.CloseActiveDialog)));
      this.AddBinding((IBinding) new TriggerBinding("paradox", "showLoginForm", new Action(this.ShowLoginForm)));
      this.AddBinding((IBinding) new TriggerBinding<string>("paradox", "submitPasswordReset", new Action<string>(this.SubmitPasswordReset)));
      this.AddBinding((IBinding) new TriggerBinding<ParadoxBindings.LoginFormData>("paradox", "submitLoginForm", new Action<ParadoxBindings.LoginFormData>(this.SubmitLoginForm), (IReader<ParadoxBindings.LoginFormData>) new ValueReader<ParadoxBindings.LoginFormData>()));
      this.AddBinding((IBinding) (this.m_HasInternetConnection = new ValueBinding<bool>("paradox", "hasInternetConnection", PlatformManager.instance.hasConnectivity)));
      this.AddBinding((IBinding) new GetterValueBinding<List<string>>("paradox", "countryCodes", new Func<List<string>>(this.GetCountryCodes), (IWriter<List<string>>) new ListWriter<string>((IWriter<string>) new StringWriter())));
      this.AddBinding((IBinding) new TriggerBinding("paradox", "showRegistrationForm", new Action(this.ShowRegistrationForm)));
      this.AddBinding((IBinding) new TriggerBinding<string>("paradox", "showLink", new Action<string>(this.ShowLink)));
      this.AddBinding((IBinding) new TriggerBinding<ParadoxBindings.RegistrationFormData>("paradox", "submitRegistrationForm", new Action<ParadoxBindings.RegistrationFormData>(this.SubmitRegistrationForm), (IReader<ParadoxBindings.RegistrationFormData>) new ValueReader<ParadoxBindings.RegistrationFormData>()));
      this.AddBinding((IBinding) new TriggerBinding("paradox", "confirmAccountLink", new Action(this.ConfirmAccountLink)));
      this.AddBinding((IBinding) new TriggerBinding("paradox", "confirmAccountLinkOverwrite", new Action(this.ConfirmAccountLinkOverwrite)));
      this.AddBinding((IBinding) new TriggerBinding("paradox", "markLegalDocumentAsViewed", new Action(this.MarkLegalDocumentAsViewed)));
      this.AddBinding((IBinding) new TriggerBinding("paradox", "showTermsOfUse", new Action(this.ShowTermsOfUse)));
      this.AddBinding((IBinding) new TriggerBinding("paradox", "showPrivacyPolicy", new Action(this.ShowPrivacyPolicy)));
      this.AddBinding((IBinding) new TriggerBinding<int>("paradox", "onOptionSelected", new Action<int>(this.OnOptionSelected)));
      this.AddBinding((IBinding) (this.m_IsPDXSDKEnabled = new ValueBinding<bool>("paradox", "pdxSDKEnabled", false)));
      this.m_PdxPlatform = PlatformManager.instance.GetPSI<PdxSdkPlatform>("PdxSdk");
      PlatformManager.instance.onPlatformRegistered += (PlatformRegisteredHandler) (psi =>
      {
        if (!(psi is PdxSdkPlatform pdxSdkPlatform2))
          return;
        this.m_PdxPlatform = pdxSdkPlatform2;
        this.m_PdxPlatform.onLoggedIn += new OnLoggedInEventHandler(this.OnUserLoggedIn);
        this.m_PdxPlatform.onLoggedOut += new OnLoggedOutEventHandler(this.OnUserLoggedOut);
        this.m_PdxPlatform.onAccountLinkChanged += new OnAccountLinkChangeEventHandler(this.OnAccountLinkChanged);
        this.m_PdxPlatform.onLegalDocumentStatusChanged += new OnLegalDocumentStatusChangedEventHandler(this.OnLegalDocumentStatusChanged);
        this.m_PdxPlatform.onStatusChanged += new OnStatusChangedEventHandler(this.OnStatusChanged);
      });
      PlatformManager.instance.onConnectivityStatusChanged += new OnConnectivityStatusChanged(this.OnInternetConnectionStatusChanged);
    }

    public void OnPSModsUIOpened(Action onContinue)
    {
      this.m_ActiveDialogsBinding.Push((ParadoxBindings.ParadoxDialog) new ParadoxBindings.MultiOptionDialog("Menu.PDX_MODS", "Paradox.PS_MODS_DISCLAIMER", new ParadoxBindings.MultiOptionDialog.Option[1]
      {
        new ParadoxBindings.MultiOptionDialog.Option()
        {
          m_Id = "Common.OK",
          m_OnSelect = onContinue
        }
      }));
    }

    public void OnPSModsUIClosed(Action onKeepMods, Action onDisableMods, Action onBack)
    {
      this.m_ActiveDialogsBinding.Push((ParadoxBindings.ParadoxDialog) new ParadoxBindings.MultiOptionDialog("Menu.PDX_MODS", "Paradox.PS_MODS_EXIT_DISCLAIMER", new ParadoxBindings.MultiOptionDialog.Option[3]
      {
        new ParadoxBindings.MultiOptionDialog.Option()
        {
          m_Id = "Paradox.PS_MODS_EXIT_KEEP_MODS",
          m_OnSelect = onKeepMods
        },
        new ParadoxBindings.MultiOptionDialog.Option()
        {
          m_Id = "Paradox.PS_MODS_EXIT_DISABLE_MODS",
          m_OnSelect = onDisableMods
        },
        new ParadoxBindings.MultiOptionDialog.Option()
        {
          m_Id = "Paradox.PS_MODS_EXIT_GO_BACK",
          m_OnSelect = onBack
        }
      }));
    }

    public void PushDialog(ParadoxBindings.ParadoxDialog dialog)
    {
      this.m_ActiveDialogsBinding.Push(dialog);
    }

    private void OnOptionSelected(int index)
    {
      if (!(this.m_ActiveDialogsBinding.Peek() is ParadoxBindings.MultiOptionDialog multiOptionDialog))
        return;
      this.m_ActiveDialogsBinding.Pop();
      Action onSelect = multiOptionDialog.m_Options[index].m_OnSelect;
      if (onSelect == null)
        return;
      onSelect();
    }

    private void OnAccountLinkChanged(AccountLinkState state, AccountLinkProvider provider)
    {
      this.m_AccountLinkProviderBinding.Update(provider);
      this.m_AccountLinkStateBinding.Update((int) state);
    }

    private async void Logout() => await this.m_PdxPlatform.Logout();

    private List<string> GetCountryCodes()
    {
      List<string> countryCodes = new List<string>((IEnumerable<string>) Enum.GetNames(typeof (Country)));
      countryCodes.Remove(Country.Undefined.ToString());
      return countryCodes;
    }

    private void OnInternetConnectionStatusChanged(bool connected)
    {
      this.m_HasInternetConnection.Update(connected);
    }

    private void OnStatusChanged(IPlatformServiceIntegration psi)
    {
      if (psi != this.m_PdxPlatform)
        return;
      this.m_IsPDXSDKEnabled.Update(this.m_PdxPlatform.isInitialized);
      this.m_AccountLinkProviderBinding.Update(this.m_PdxPlatform.accountLinkProvider);
      this.m_AccountLinkStateBinding.Update((int) this.m_PdxPlatform.accountLinkState);
    }

    private async void OnUserLoggedIn(
      string firstName,
      string lastName,
      string email,
      AccountLinkState accountLinkState,
      bool firstTime)
    {
      this.m_LoggedInBinding.Update(true);
      this.m_AccountLinkStateBinding.Update((int) accountLinkState);
      this.m_EmailBinding.Update(email);
      ModCreator creatorProfile = await this.m_PdxPlatform.GetCreatorProfile();
      if (creatorProfile == null)
        return;
      this.m_UserNameBinding.Update(creatorProfile.Username);
      this.m_AvatarBinding.Update(creatorProfile.Avatar.Url);
    }

    private void OnUserLoggedOut(string id)
    {
      this.m_UserNameBinding.Update((string) null);
      this.m_EmailBinding.Update((string) null);
      this.m_AvatarBinding.Update((string) null);
      this.m_LoggedInBinding.Update(false);
    }

    private void OnLegalDocumentStatusChanged(LegalDocument doc, int remainingCount)
    {
      if (this.m_ActiveDialogsBinding.Peek() is ParadoxBindings.LegalDocumentDialog)
        this.m_ActiveDialogsBinding.Pop();
      if (doc == null)
        return;
      this.m_ActiveDialogsBinding.Push((ParadoxBindings.ParadoxDialog) new ParadoxBindings.LegalDocumentDialog(doc));
    }

    private void CloseActiveDialog()
    {
      if (this.m_RequestActiveBinding.value || this.m_ActiveDialogsBinding.Peek() is ParadoxBindings.LegalDocumentDialog legalDocumentDialog && legalDocumentDialog.agreementRequired)
        return;
      this.m_ActiveDialogsBinding.Pop();
      if (this.m_ActiveDialogsBinding.count != 0)
        return;
      PlatformManager.instance.EnableSharing();
    }

    public void ShowLoginForm()
    {
      if (Connectivity.hasConnectivity)
      {
        PlatformManager.instance.DisableSharing();
        this.m_ActiveDialogsBinding.ClearAndPush((ParadoxBindings.ParadoxDialog) new ParadoxBindings.LoginDialog());
      }
      else
        this.m_ActiveDialogsBinding.Push((ParadoxBindings.ParadoxDialog) new ParadoxBindings.ErrorDialog("Failed to connect", "Please check your internet connection"));
    }

    private async void SubmitLoginForm(ParadoxBindings.LoginFormData data)
    {
      if (this.m_RequestActiveBinding.value)
        return;
      PdxSdkPlatform.RequestReport requestReport = await this.RunForegroundRequest<PdxSdkPlatform.RequestReport>(this.m_PdxPlatform.Login(data.email, data.password, CancellationToken.None));
      if (requestReport == null)
      {
        this.m_ActiveDialogsBinding.Clear();
        if (this.m_PdxPlatform.accountLinkProvider == AccountLinkProvider.Unknown || this.m_PdxPlatform.accountLinkState != AccountLinkState.Unlinked)
          return;
        this.LinkAccount();
      }
      else
        this.m_ActiveDialogsBinding.Push((ParadoxBindings.ParadoxDialog) new ParadoxBindings.ErrorDialog(requestReport.messageId, requestReport.message));
    }

    private async void SubmitPasswordReset(string email)
    {
      if (this.m_RequestActiveBinding.value)
        return;
      PdxSdkPlatform.RequestReport requestReport = await this.RunForegroundRequest<PdxSdkPlatform.RequestReport>(this.m_PdxPlatform.ResetPassword(email));
      if (requestReport == null)
        this.m_ActiveDialogsBinding.Push((ParadoxBindings.ParadoxDialog) new ParadoxBindings.ConfirmationDialog((string) null, (string) null, "Paradox.PASSWORD_RESET_CONFIRMATION_TEXT", new Dictionary<string, string>()
        {
          {
            "EMAIL",
            email
          }
        }));
      else
        this.m_ActiveDialogsBinding.Push((ParadoxBindings.ParadoxDialog) new ParadoxBindings.ErrorDialog(requestReport.messageId, requestReport.message));
    }

    private void ShowRegistrationForm()
    {
      this.m_ActiveDialogsBinding.ClearAndPush((ParadoxBindings.ParadoxDialog) new ParadoxBindings.RegistrationDialog());
    }

    private async void ShowLink(string link)
    {
      if (link == ParadoxBindings.kTermsOfUse)
      {
        LegalDocument document = await this.RunForegroundRequest<LegalDocument>(this.m_PdxPlatform.ShowTermsOfUse());
        if (document == null)
          return;
        this.m_ActiveDialogsBinding.Push((ParadoxBindings.ParadoxDialog) new ParadoxBindings.LegalDocumentDialog(document, false));
      }
      else if (link == ParadoxBindings.kPrivacyPolicy)
      {
        LegalDocument document = await this.RunForegroundRequest<LegalDocument>(this.m_PdxPlatform.ShowPrivacyPolicy());
        if (document == null)
          return;
        this.m_ActiveDialogsBinding.Push((ParadoxBindings.ParadoxDialog) new ParadoxBindings.LegalDocumentDialog(document, false));
      }
      else
        Application.OpenURL(link);
    }

    private async void SubmitRegistrationForm(ParadoxBindings.RegistrationFormData data)
    {
      if (this.m_RequestActiveBinding.value)
        return;
      Country result1;
      DateTime result2;
      if (Enum.TryParse<Country>(data.country, out result1) && DateTime.TryParseExact(data.dateOfBirth, "yyyy-MM-dd", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result2))
      {
        PdxSdkPlatform.RequestReport requestReport = await this.RunForegroundRequest<PdxSdkPlatform.RequestReport>(this.m_PdxPlatform.CreateParadoxAccount(data.email, data.password, Language.en, result1, result2, data.marketingPermission));
        if (requestReport == null)
        {
          this.m_ActiveDialogsBinding.Clear();
          if (this.m_PdxPlatform.accountLinkProvider != AccountLinkProvider.Unknown && this.m_PdxPlatform.accountLinkState == AccountLinkState.Unlinked)
            this.LinkAccount();
          this.m_ActiveDialogsBinding.Push((ParadoxBindings.ParadoxDialog) new ParadoxBindings.ConfirmationDialog((string) null, "Paradox.REGISTRATION_CONFIRMATION_TITLE", "Paradox.REGISTRATION_CONFIRMATION_TEXT", (Dictionary<string, string>) null));
        }
        else
          this.m_ActiveDialogsBinding.Push((ParadoxBindings.ParadoxDialog) new ParadoxBindings.ErrorDialog(requestReport.messageId, requestReport.message));
      }
      else
        this.m_ActiveDialogsBinding.Push((ParadoxBindings.ParadoxDialog) new ParadoxBindings.ErrorDialog((string) null, "Internal error: Invalid Country Code string or Invalid date string"));
    }

    private async void ConfirmAccountLink()
    {
      if (this.m_RequestActiveBinding.value)
        return;
      if (this.m_PdxPlatform.AccountLinkMismatch == AccountLinkMismatch.None)
      {
        PdxSdkPlatform.RequestReport requestReport = await this.RunForegroundRequest<PdxSdkPlatform.RequestReport>(this.m_PdxPlatform.LinkAccount());
        if (requestReport == null)
        {
          this.m_AccountLinkStateBinding.Update(2);
          this.m_ActiveDialogsBinding.ClearAndPush((ParadoxBindings.ParadoxDialog) new ParadoxBindings.ConfirmationDialog(this.GetAccountLinkProviderIcon(), "Paradox.ACCOUNT_LINK_PROMPT_TITLE", string.Format("Paradox.ACCOUNT_LINK_CONFIRMATION_TEXT[{0:G}]", (object) this.m_PdxPlatform.accountLinkProvider), (Dictionary<string, string>) null));
        }
        else
          this.m_ActiveDialogsBinding.Push((ParadoxBindings.ParadoxDialog) new ParadoxBindings.ErrorDialog(requestReport.messageId, requestReport.message));
      }
      else
      {
        string str;
        switch (this.m_PdxPlatform.AccountLinkMismatch)
        {
          case AccountLinkMismatch.Paradox:
            str = string.Format("Paradox.PDX_ACCOUNT_LINK_OVERWRITE_PROMPT_TEXT[{0:G}]", (object) this.m_PdxPlatform.accountLinkProvider);
            break;
          case AccountLinkMismatch.ThirdParty:
            str = string.Format("Paradox.PLATFORM_ACCOUNT_LINK_OVERWRITE_PROMPT_TEXT[{0:G}]", (object) this.m_PdxPlatform.accountLinkProvider);
            break;
          case AccountLinkMismatch.Both:
            str = string.Format("Paradox.PDX_PLATFORM_ACCOUNT_LINK_OVERWRITE_PROMPT_TEXT[{0:G}]", (object) this.m_PdxPlatform.accountLinkProvider);
            break;
          default:
            str = (string) null;
            break;
        }
        string messageId = str;
        this.m_ActiveDialogsBinding.Push((ParadoxBindings.ParadoxDialog) new ParadoxBindings.AccountLinkOverwriteDialog(this.GetAccountLinkProviderIcon(), messageId));
      }
    }

    private async void ConfirmAccountLinkOverwrite()
    {
      if (this.m_RequestActiveBinding.value)
        return;
      PdxSdkPlatform.RequestReport requestReport = await this.RunForegroundRequest<PdxSdkPlatform.RequestReport>(this.m_PdxPlatform.OverwriteAccountLinks());
      if (requestReport == null)
      {
        this.m_AccountLinkStateBinding.Update(2);
        this.m_ActiveDialogsBinding.ClearAndPush((ParadoxBindings.ParadoxDialog) new ParadoxBindings.ConfirmationDialog(this.GetAccountLinkProviderIcon(), "Paradox.ACCOUNT_LINK_PROMPT_TITLE", string.Format("Paradox.ACCOUNT_LINK_CONFIRMATION_TEXT[{0:G}]", (object) this.m_PdxPlatform.accountLinkProvider), (Dictionary<string, string>) null));
      }
      else
        this.m_ActiveDialogsBinding.Push((ParadoxBindings.ParadoxDialog) new ParadoxBindings.ErrorDialog(requestReport.messageId, requestReport.message));
    }

    private void LinkAccount()
    {
      this.m_ActiveDialogsBinding.Push((ParadoxBindings.ParadoxDialog) new ParadoxBindings.AccountLinkDialog(this.GetAccountLinkProviderIcon(), string.Format("Paradox.ACCOUNT_LINK_PROMPT_TEXT[{0:G}]", (object) this.m_PdxPlatform.accountLinkProvider)));
    }

    private async void UnlinkAccount()
    {
      if (this.m_RequestActiveBinding.value)
        return;
      PdxSdkPlatform.RequestReport requestReport = await this.RunForegroundRequest<PdxSdkPlatform.RequestReport>(this.m_PdxPlatform.UnlinkThirdPartyAccount());
      if (requestReport == null)
        this.m_AccountLinkStateBinding.Update(1);
      else
        this.m_ActiveDialogsBinding.Push((ParadoxBindings.ParadoxDialog) new ParadoxBindings.ErrorDialog(requestReport.messageId, requestReport.message));
    }

    private void ShowTermsOfUse() => this.ShowLink(ParadoxBindings.kTermsOfUse);

    private void ShowPrivacyPolicy() => this.ShowLink(ParadoxBindings.kPrivacyPolicy);

    private string GetAccountLinkProviderIcon()
    {
      return string.Format("Media/Menu/Platforms/{0:G}.svg", (object) this.m_PdxPlatform.accountLinkProvider);
    }

    private async void MarkLegalDocumentAsViewed()
    {
      if (!(this.m_ActiveDialogsBinding.Peek() is ParadoxBindings.LegalDocumentDialog legalDocumentDialog))
        return;
      int num = await this.RunForegroundRequest<bool>(this.m_PdxPlatform.MarkLegalDocumentAsViewed(legalDocumentDialog.document)) ? 1 : 0;
    }

    private async Task<T> RunForegroundRequest<T>(Task<T> task)
    {
      this.m_RequestActiveBinding.Update(true);
      T obj;
      try
      {
        obj = await task;
      }
      finally
      {
        this.m_RequestActiveBinding.Update(false);
      }
      return obj;
    }

    public abstract class ParadoxDialog : IJsonWritable
    {
      public virtual void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.TypeEnd();
      }
    }

    public class LoginFormData : IJsonReadable
    {
      public string email;
      public string password;

      public void Read(IJsonReader reader)
      {
        long num = (long) reader.ReadMapBegin();
        reader.ReadProperty("email");
        reader.Read(out this.email);
        reader.ReadProperty("password");
        reader.Read(out this.password);
        reader.ReadMapEnd();
      }
    }

    public class RegistrationFormData : IJsonReadable
    {
      public string email;
      public string password;
      public string country;
      public string dateOfBirth;
      public bool marketingPermission;

      public void Read(IJsonReader reader)
      {
        long num = (long) reader.ReadMapBegin();
        reader.ReadProperty("email");
        reader.Read(out this.email);
        reader.ReadProperty("password");
        reader.Read(out this.password);
        reader.ReadProperty("country");
        reader.Read(out this.country);
        reader.ReadProperty("dateOfBirth");
        reader.Read(out this.dateOfBirth);
        reader.ReadProperty("marketingPermission");
        reader.Read(out this.marketingPermission);
        reader.ReadMapEnd();
      }
    }

    public abstract class MessageDialog : ParadoxBindings.ParadoxDialog
    {
      [CanBeNull]
      public readonly string icon;
      [CanBeNull]
      public readonly string titleId;
      [NotNull]
      public readonly string messageId;
      [CanBeNull]
      public readonly Dictionary<string, string> messageArgs;

      protected MessageDialog(
        string icon,
        string titleId,
        string messageId,
        Dictionary<string, string> messageArgs)
      {
        this.icon = icon;
        this.titleId = titleId;
        this.messageId = messageId;
        this.messageArgs = messageArgs;
      }

      public override void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("icon");
        writer.Write(this.icon);
        writer.PropertyName("titleId");
        writer.Write(this.titleId);
        writer.PropertyName("messageId");
        writer.Write(this.messageId);
        writer.PropertyName("messageArgs");
        writer.Write((IReadOnlyDictionary<string, string>) this.messageArgs);
        writer.TypeEnd();
      }
    }

    public class LoginDialog : ParadoxBindings.ParadoxDialog
    {
    }

    public class RegistrationDialog : ParadoxBindings.ParadoxDialog
    {
    }

    public class AccountLinkDialog : ParadoxBindings.MessageDialog
    {
      public AccountLinkDialog(string icon, string messageId)
        : base(icon, "Paradox.ACCOUNT_LINK_PROMPT_TITLE", messageId, (Dictionary<string, string>) null)
      {
      }
    }

    public class AccountLinkOverwriteDialog : ParadoxBindings.MessageDialog
    {
      public AccountLinkOverwriteDialog(string icon, string messageId)
        : base(icon, "Paradox.ACCOUNT_LINK_OVERWRITE_TITLE", messageId, (Dictionary<string, string>) null)
      {
      }
    }

    public class LegalDocumentDialog : ParadoxBindings.ParadoxDialog
    {
      [NotNull]
      public readonly LegalDocument document;
      public readonly bool agreementRequired;
      public readonly string confirmLabel;

      public LegalDocumentDialog(LegalDocument document, bool agreementRequired = true)
      {
        this.document = document;
        this.agreementRequired = agreementRequired;
        this.confirmLabel = document.confirmLabel;
      }

      public override void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("text");
        writer.Write(this.document.content);
        writer.PropertyName("agreementRequired");
        writer.Write(this.agreementRequired);
        writer.PropertyName("confirmLabel");
        writer.Write(this.confirmLabel);
        writer.TypeEnd();
      }
    }

    public class ConfirmationDialog : ParadoxBindings.MessageDialog
    {
      public ConfirmationDialog(
        string icon,
        string titleId,
        string messageId,
        Dictionary<string, string> messageArgs)
        : base(icon, titleId, messageId, messageArgs)
      {
      }
    }

    public class ErrorDialog : ParadoxBindings.ParadoxDialog
    {
      [CanBeNull]
      public readonly string messageId;
      [CanBeNull]
      public readonly string message;

      public ErrorDialog(string messageId, string message)
      {
        this.messageId = messageId;
        this.message = message;
      }

      public override void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("messageId");
        writer.Write(this.messageId);
        writer.PropertyName("message");
        writer.Write(this.message);
        writer.TypeEnd();
      }
    }

    public class MultiOptionDialog : ParadoxBindings.ParadoxDialog
    {
      public string m_TitleId;
      public string m_MessageId;
      public ParadoxBindings.MultiOptionDialog.Option[] m_Options;

      public MultiOptionDialog(
        string titleId,
        string messageId,
        params ParadoxBindings.MultiOptionDialog.Option[] options)
      {
        this.m_TitleId = titleId;
        this.m_MessageId = messageId;
        this.m_Options = options;
      }

      public override void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("titleId");
        writer.Write(this.m_TitleId);
        writer.PropertyName("messageId");
        writer.Write(this.m_MessageId);
        writer.PropertyName("options");
        writer.Write<ParadoxBindings.MultiOptionDialog.Option>((IList<ParadoxBindings.MultiOptionDialog.Option>) this.m_Options);
        writer.TypeEnd();
      }

      public struct Option : IJsonWritable
      {
        public string m_Id;
        public Action m_OnSelect;

        public void Write(IJsonWriter writer)
        {
          writer.TypeBegin(this.GetType().Name);
          writer.PropertyName("id");
          writer.Write(this.m_Id);
          writer.TypeEnd();
        }
      }
    }
  }
}
