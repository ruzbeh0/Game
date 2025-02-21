// Decompiled with JetBrains decompiler
// Type: Game.UI.Menu.AssetUploadPanelUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.Annotations;
using Colossal.IO.AssetDatabase;
using Colossal.PSI.Common;
using Colossal.UI;
using Game.Prefabs;
using Game.PSI;
using Game.Reflection;
using Game.SceneFlow;
using Game.UI.Editor;
using Game.UI.Editor.Widgets;
using Game.UI.Localization;
using Game.UI.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Menu
{
  public class AssetUploadPanelUISystem : UISystemBase
  {
    private static readonly string kNotificationID = "AssetUpload";
    private static readonly LocalizedString kNone = LocalizedString.Id("Editor.NONE_VALUE");
    private static readonly string kFailureLabel = "Menu.ASSET_FAILURE";
    private static readonly string kSubmittingLabel = "Menu.ASSET_SUBMITTING";
    private static readonly string kCompleteLabel = "Menu.ASSET_COMPLETE";
    private static readonly string kSubmitLabel = "Common.SUBMIT";
    private static readonly string kNoInternetConnectionLabel = "Paradox.NO_INTERNET_CONNECTION";
    private static readonly string kNotLoggedInLabel = "Paradox.NOT_LOGGED_IN";
    private static readonly string kNoSocialProfile = "Paradox.NO_SOCIAL_PROFILE";
    private static readonly string kOpenProfilePage = "Paradox.OPEN_PROFILE_PAGE";
    private static readonly float kNotificationDelay = 4f;
    private NotificationUISystem m_NotificationUISystem;
    private AssetPickerAdapter m_PreviewPickerAdapter;
    private PdxAssetUploadHandle m_UploadHandle = new PdxAssetUploadHandle();
    private bool m_AllowManualFileCopy;
    private AssetUploadPanelUISystem.State m_State;
    private IWidget[] m_MainPanel;
    private LargeIconButton m_PreviewPickerButton;
    private ExternalLinkField m_ExternalLinkField;
    private LayoutContainer m_Screenshots;
    private LayoutContainer m_PlatformResult;
    private ItemPickerPopup<int> m_ExistingModPopup;
    private PopupValueField<int> m_ExistingModField;
    private Button m_SubmitButton;
    [CanBeNull]
    private ListField m_AssetList;
    [CanBeNull]
    private PopupValueField<PrefabBase> m_AssetListPopup;
    private ListField m_TagsList;
    private PopupValueField<string> m_TagsListPopup;
    private IWidget[] m_PreviewPickerPanel;
    private Button m_SelectPreviewButton;
    public Action<IList<IWidget>> onChildrenChange;
    private IList<IWidget> m_Children;

    private bool nameError => string.IsNullOrEmpty(this.m_UploadHandle.modInfo.m_DisplayName);

    private bool shortDescirptionError
    {
      get => string.IsNullOrEmpty(this.m_UploadHandle.modInfo.m_ShortDescription);
    }

    private bool longDescipriontError
    {
      get => string.IsNullOrEmpty(this.m_UploadHandle.modInfo.m_LongDescription);
    }

    private bool forumLinkError
    {
      get => !AssetUploadUtils.ValidateForumLink(this.m_UploadHandle.modInfo.m_ForumLink);
    }

    private bool externalLinkError
    {
      get
      {
        return !AssetUploadUtils.ValidateExternalLinks((IEnumerable<IModsUploadSupport.ExternalLinkData>) this.m_UploadHandle.modInfo.m_ExternalLinks);
      }
    }

    private bool changelogError
    {
      get
      {
        return this.m_UploadHandle.updateExisting && string.IsNullOrWhiteSpace(this.m_UploadHandle.modInfo.m_Changelog);
      }
    }

    private bool versionError
    {
      get
      {
        return this.m_UploadHandle.updateExisting && string.IsNullOrWhiteSpace(this.m_UploadHandle.modInfo.m_UserModVersion);
      }
    }

    private bool noInternetConnection => !PlatformManager.instance.hasConnectivity;

    private bool notLoggedIn => !this.m_UploadHandle.LoggedIn();

    private bool anyError
    {
      get
      {
        return this.nameError || this.shortDescirptionError || this.longDescipriontError || this.forumLinkError || this.externalLinkError || this.changelogError || this.versionError || this.noInternetConnection || this.notLoggedIn;
      }
    }

    private bool disableSubmit
    {
      get
      {
        return this.m_State == AssetUploadPanelUISystem.State.Processing || this.m_State == AssetUploadPanelUISystem.State.Success || this.m_State == AssetUploadPanelUISystem.State.Disabled || this.anyError;
      }
    }

    public IList<IWidget> children => this.m_Children;

    [Preserve]
    protected override void OnCreate()
    {
      base.OnCreate();
      this.m_NotificationUISystem = this.World.GetOrCreateSystemManaged<NotificationUISystem>();
      this.m_ExistingModPopup = new ItemPickerPopup<int>(false);
      IWidget[] widgetArray1 = new IWidget[1];
      Scrollable scrollable1 = new Scrollable();
      Scrollable scrollable2 = scrollable1;
      IWidget[] widgetArray2 = new IWidget[15];
      LargeIconButton largeIconButton1 = new LargeIconButton();
      largeIconButton1.action = (Action) (() =>
      {
        Action<Colossal.Hash128> callback = new Action<Colossal.Hash128>(this.SetPreview);
        AssetData preview = this.m_UploadHandle.preview;
        Colossal.Hash128 defaultSelection = (object) preview != null ? preview.guid : new Colossal.Hash128();
        this.OpenPreviewPickerPanel(callback, defaultSelection);
      });
      LargeIconButton largeIconButton2 = largeIconButton1;
      this.m_PreviewPickerButton = largeIconButton1;
      widgetArray2[0] = (IWidget) largeIconButton2;
      StringInputFieldWithError inputFieldWithError1 = new StringInputFieldWithError();
      inputFieldWithError1.displayName = (LocalizedString) "Menu.ASSET_NAME";
      inputFieldWithError1.errorMessage = (LocalizedString) "Menu.ASSET_ERROR_NAME";
      inputFieldWithError1.accessor = (ITypedValueAccessor<string>) new DelegateAccessor<string>((Func<string>) (() => this.m_UploadHandle.modInfo.m_DisplayName), (Action<string>) (value => this.m_UploadHandle.modInfo = this.m_UploadHandle.modInfo with
      {
        m_DisplayName = value
      }));
      inputFieldWithError1.error = (Func<bool>) (() => this.nameError);
      inputFieldWithError1.maxLength = 60;
      widgetArray2[1] = (IWidget) inputFieldWithError1;
      ToggleField toggleField = new ToggleField();
      toggleField.displayName = (LocalizedString) "Menu.ASSET_UPDATE_EXISTING";
      toggleField.accessor = (ITypedValueAccessor<bool>) new DelegateAccessor<bool>((Func<bool>) (() => this.m_UploadHandle.updateExisting), (Action<bool>) (value => this.m_UploadHandle.updateExisting = value));
      widgetArray2[2] = (IWidget) toggleField;
      PopupValueField<int> popupValueField1 = new PopupValueField<int>();
      popupValueField1.displayName = (LocalizedString) "Menu.ASSET_EXISTING";
      popupValueField1.accessor = (ITypedValueAccessor<int>) new DelegateAccessor<int>((Func<int>) (() => this.m_UploadHandle.modInfo.m_PublishedID), (Action<int>) (value =>
      {
        this.m_UploadHandle.modInfo = this.m_UploadHandle.modInfo with
        {
          m_PublishedID = value
        };
        this.SetInfoFromExisting();
      }));
      popupValueField1.hidden = (Func<bool>) (() => !this.m_UploadHandle.updateExisting || this.m_UploadHandle.authorMods.Count == 0);
      popupValueField1.popup = (IValueFieldPopup<int>) this.m_ExistingModPopup;
      PopupValueField<int> popupValueField2 = popupValueField1;
      this.m_ExistingModField = popupValueField1;
      widgetArray2[3] = (IWidget) popupValueField2;
      IntInputField intInputField = new IntInputField();
      intInputField.displayName = (LocalizedString) "Menu.ASSET_EXISTING_ID";
      intInputField.hidden = (Func<bool>) (() => !this.m_UploadHandle.updateExisting || this.m_UploadHandle.authorMods.Count > 0);
      intInputField.min = 0;
      intInputField.accessor = (ITypedValueAccessor<int>) new DelegateAccessor<int>((Func<int>) (() => this.m_UploadHandle.modInfo.m_PublishedID), (Action<int>) (value => this.m_UploadHandle.modInfo = this.m_UploadHandle.modInfo with
      {
        m_PublishedID = value
      }));
      widgetArray2[4] = (IWidget) intInputField;
      StringInputFieldWithError inputFieldWithError2 = new StringInputFieldWithError();
      inputFieldWithError2.displayName = (LocalizedString) "Menu.ASSET_VERSION";
      inputFieldWithError2.errorMessage = (LocalizedString) "Menu.ASSET_ERROR_VERSION";
      inputFieldWithError2.error = (Func<bool>) (() => this.versionError);
      inputFieldWithError2.hidden = (Func<bool>) (() => !this.m_UploadHandle.updateExisting);
      inputFieldWithError2.accessor = (ITypedValueAccessor<string>) new DelegateAccessor<string>((Func<string>) (() => this.m_UploadHandle.modInfo.m_UserModVersion), (Action<string>) (value =>
      {
        if (!value.All<char>((Func<char, bool>) (c => char.IsLetterOrDigit(c) || c == '.' || c == '-' || c == '_')))
          return;
        this.m_UploadHandle.modInfo = this.m_UploadHandle.modInfo with
        {
          m_UserModVersion = value
        };
      }));
      inputFieldWithError2.maxLength = 20;
      widgetArray2[5] = (IWidget) inputFieldWithError2;
      StringInputFieldWithError inputFieldWithError3 = new StringInputFieldWithError();
      inputFieldWithError3.displayName = (LocalizedString) "Menu.ASSET_CHANGELOG";
      inputFieldWithError3.errorMessage = (LocalizedString) "Menu.ASSET_ERROR_EMPTY_CHANGELOG";
      inputFieldWithError3.error = (Func<bool>) (() => this.changelogError);
      inputFieldWithError3.multiline = StringInputField.kDefaultMultilines;
      inputFieldWithError3.hidden = (Func<bool>) (() => !this.m_UploadHandle.updateExisting);
      inputFieldWithError3.accessor = (ITypedValueAccessor<string>) new DelegateAccessor<string>((Func<string>) (() => this.m_UploadHandle.modInfo.m_Changelog), (Action<string>) (value => this.m_UploadHandle.modInfo = this.m_UploadHandle.modInfo with
      {
        m_Changelog = value
      }));
      inputFieldWithError3.maxLength = 20000;
      widgetArray2[6] = (IWidget) inputFieldWithError3;
      StringInputFieldWithError inputFieldWithError4 = new StringInputFieldWithError();
      inputFieldWithError4.displayName = (LocalizedString) "Menu.ASSET_SHORT_DESCRIPTION";
      inputFieldWithError4.errorMessage = (LocalizedString) "Menu.ASSET_ERROR_DESCRIPTION";
      inputFieldWithError4.accessor = (ITypedValueAccessor<string>) new DelegateAccessor<string>((Func<string>) (() => this.m_UploadHandle.modInfo.m_ShortDescription), (Action<string>) (value => this.m_UploadHandle.modInfo = this.m_UploadHandle.modInfo with
      {
        m_ShortDescription = value
      }));
      inputFieldWithError4.error = (Func<bool>) (() => this.shortDescirptionError);
      inputFieldWithError4.maxLength = 200;
      widgetArray2[7] = (IWidget) inputFieldWithError4;
      StringInputFieldWithError inputFieldWithError5 = new StringInputFieldWithError();
      inputFieldWithError5.displayName = (LocalizedString) "Menu.ASSET_LONG_DESCRIPTION";
      inputFieldWithError5.errorMessage = (LocalizedString) "Menu.ASSET_ERROR_DESCRIPTION";
      inputFieldWithError5.multiline = StringInputField.kDefaultMultilines;
      inputFieldWithError5.accessor = (ITypedValueAccessor<string>) new DelegateAccessor<string>((Func<string>) (() => this.m_UploadHandle.modInfo.m_LongDescription), (Action<string>) (value => this.m_UploadHandle.modInfo = this.m_UploadHandle.modInfo with
      {
        m_LongDescription = value
      }));
      inputFieldWithError5.error = (Func<bool>) (() => this.longDescipriontError);
      inputFieldWithError5.maxLength = 20000;
      widgetArray2[8] = (IWidget) inputFieldWithError5;
      StringInputFieldWithError inputFieldWithError6 = new StringInputFieldWithError();
      inputFieldWithError6.displayName = (LocalizedString) "Menu.ASSET_FORUM_LINK_LABEL";
      inputFieldWithError6.errorMessage = (LocalizedString) "Menu.ASSET_ERROR_LINK";
      inputFieldWithError6.accessor = (ITypedValueAccessor<string>) new DelegateAccessor<string>((Func<string>) (() => this.m_UploadHandle.modInfo.m_ForumLink), (Action<string>) (value => this.m_UploadHandle.modInfo = this.m_UploadHandle.modInfo with
      {
        m_ForumLink = value
      }));
      inputFieldWithError6.error = (Func<bool>) (() => this.forumLinkError);
      widgetArray2[9] = (IWidget) inputFieldWithError6;
      Game.UI.Widgets.Group group1 = new Game.UI.Widgets.Group();
      group1.displayName = (LocalizedString) "Menu.ASSET_EXTERNAL_LINKS";
      group1.children = (IList<IWidget>) new IWidget[1]
      {
        (IWidget) (this.m_ExternalLinkField = new ExternalLinkField())
      };
      widgetArray2[10] = (IWidget) group1;
      Game.UI.Widgets.Group group2 = new Game.UI.Widgets.Group();
      group2.displayName = (LocalizedString) "Menu.ASSET_PREVIEW_SCREENSHOTS";
      Game.UI.Widgets.Group group3 = group2;
      IWidget[] widgetArray3 = new IWidget[1];
      Row row1 = new Row();
      row1.wrap = true;
      LayoutContainer layoutContainer1 = (LayoutContainer) row1;
      this.m_Screenshots = (LayoutContainer) row1;
      widgetArray3[0] = (IWidget) layoutContainer1;
      group3.children = (IList<IWidget>) widgetArray3;
      widgetArray2[11] = (IWidget) group2;
      EditorSection editorSection1 = new EditorSection();
      editorSection1.displayName = (LocalizedString) "Paradox.ADDITIONAL_TAGS";
      EditorSection editorSection2 = editorSection1;
      IWidget[] widgetArray4 = new IWidget[2]
      {
        (IWidget) (this.m_TagsList = new ListField()),
        null
      };
      PopupValueField<string> popupValueField3 = new PopupValueField<string>();
      popupValueField3.displayName = (LocalizedString) "Paradox.ADD_TAG";
      popupValueField3.popup = (IValueFieldPopup<string>) new ItemPickerPopup<string>(false, false);
      popupValueField3.accessor = (ITypedValueAccessor<string>) new DelegateAccessor<string>((Func<string>) (() => string.Empty), new Action<string>(this.OnAddTag));
      popupValueField3.disabled = (Func<bool>) (() => this.m_UploadHandle.tagCount >= ModTags.kMaxTags);
      PopupValueField<string> popupValueField4 = popupValueField3;
      this.m_TagsListPopup = popupValueField3;
      widgetArray4[1] = (IWidget) popupValueField4;
      editorSection2.children = (IList<IWidget>) widgetArray4;
      widgetArray2[12] = (IWidget) editorSection1;
      Row row2 = new Row();
      row2.flex = new FlexLayout(1f, 0.0f, -1);
      Row row3 = row2;
      IWidget[] widgetArray5 = new IWidget[2];
      Column column1 = new Column();
      column1.flex = new FlexLayout(1f, 1f, -1);
      LayoutContainer layoutContainer2 = (LayoutContainer) column1;
      this.m_PlatformResult = (LayoutContainer) column1;
      widgetArray5[0] = (IWidget) layoutContainer2;
      ProgressIndicator progressIndicator = new ProgressIndicator();
      progressIndicator.state = (Func<ProgressIndicator.State>) (() =>
      {
        if (this.m_State == AssetUploadPanelUISystem.State.Processing)
          return ProgressIndicator.State.Loading;
        return this.m_State == AssetUploadPanelUISystem.State.Failure || this.m_State == AssetUploadPanelUISystem.State.Disabled ? ProgressIndicator.State.Failure : ProgressIndicator.State.Success;
      });
      progressIndicator.hidden = (Func<bool>) (() => this.m_State == AssetUploadPanelUISystem.State.Ready);
      widgetArray5[1] = (IWidget) progressIndicator;
      row3.children = (IList<IWidget>) widgetArray5;
      widgetArray2[13] = (IWidget) row2;
      Button button1 = new Button();
      button1.displayName = (LocalizedString) AssetUploadPanelUISystem.kSubmitLabel;
      button1.action = new Action(this.Submit);
      button1.disabled = (Func<bool>) (() => this.disableSubmit);
      Button button2 = button1;
      this.m_SubmitButton = button1;
      widgetArray2[14] = (IWidget) button2;
      scrollable2.children = (IList<IWidget>) widgetArray2;
      widgetArray1[0] = (IWidget) scrollable1;
      this.m_MainPanel = widgetArray1;
      if (this.m_AssetList != null)
        this.m_AssetList.onItemRemoved += new Action<int>(this.OnRemoveAdditionalAsset);
      this.m_TagsList.onItemRemoved += new Action<int>(this.OnRemoveTag);
      this.m_PreviewPickerAdapter = new AssetPickerAdapter(this.GetPreviews(), 4);
      IWidget[] widgetArray6 = new IWidget[1];
      Column column2 = new Column();
      column2.flex = FlexLayout.Fill;
      Column column3 = column2;
      IWidget[] widgetArray7 = new IWidget[2]
      {
        (IWidget) new ItemPicker<AssetItem>()
        {
          adapter = (ItemPicker<AssetItem>.IAdapter) this.m_PreviewPickerAdapter
        },
        null
      };
      ButtonRow buttonRow1 = new ButtonRow();
      ButtonRow buttonRow2 = buttonRow1;
      Button[] buttonArray = new Button[2];
      Button button3 = new Button();
      button3.displayName = (LocalizedString) "Common.SELECT";
      Button button4 = button3;
      this.m_SelectPreviewButton = button3;
      buttonArray[0] = button4;
      Button button5 = new Button();
      button5.displayName = (LocalizedString) "Common.CANCEL";
      button5.action = (Action) (() => this.SetChildren(this.m_MainPanel));
      buttonArray[1] = button5;
      buttonRow2.children = buttonArray;
      widgetArray7[1] = (IWidget) buttonRow1;
      column3.children = (IList<IWidget>) widgetArray7;
      widgetArray6[0] = (IWidget) column2;
      this.m_PreviewPickerPanel = widgetArray6;
      this.SetChildren(this.m_MainPanel);
    }

    [Preserve]
    protected override void OnUpdate()
    {
      base.OnUpdate();
      this.RefreshSubmitButtonLabel();
    }

    private void RefreshSubmitButtonLabel()
    {
      string submitButtonLabel = this.GetSubmitButtonLabel();
      if (!(this.m_SubmitButton.displayName.id != submitButtonLabel))
        return;
      this.m_SubmitButton.displayName = (LocalizedString) submitButtonLabel;
      this.m_SubmitButton.SetPropertiesChanged();
    }

    private void ReportError(IModsUploadSupport.ModOperationResult result)
    {
      this.m_State = AssetUploadPanelUISystem.State.Failure;
      List<IWidget> widgetList1 = new List<IWidget>();
      ErrorLabel errorLabel1 = new ErrorLabel();
      errorLabel1.visible = true;
      errorLabel1.displayName = (LocalizedString) AssetUploadPanelUISystem.kFailureLabel;
      widgetList1.Add((IWidget) errorLabel1);
      List<IWidget> widgetList2 = widgetList1;
      foreach (string line in result.m_Error.GetLines())
      {
        List<IWidget> widgetList3 = widgetList2;
        ErrorLabel errorLabel2 = new ErrorLabel();
        errorLabel2.visible = true;
        errorLabel2.displayName = (LocalizedString) line;
        widgetList3.Add((IWidget) errorLabel2);
      }
      this.m_PlatformResult.children = (IList<IWidget>) widgetList2;
      // ISSUE: reference to a compiler-generated method
      this.m_NotificationUISystem.RemoveNotification(AssetUploadPanelUISystem.kNotificationID);
      // ISSUE: reference to a compiler-generated method
      this.m_NotificationUISystem.RemoveNotification(AssetUploadPanelUISystem.kNotificationID, AssetUploadPanelUISystem.kNotificationDelay, new LocalizedString?((LocalizedString) AssetUploadPanelUISystem.kFailureLabel), new LocalizedString?((LocalizedString) result.m_Error.m_Raw), progressState: new ProgressState?(ProgressState.Failed));
    }

    private void ReportSuccess()
    {
      this.m_State = AssetUploadPanelUISystem.State.Success;
      List<IWidget> widgetList1 = new List<IWidget>();
      List<IWidget> widgetList2 = widgetList1;
      Label label1 = new Label();
      label1.displayName = (LocalizedString) AssetUploadPanelUISystem.kCompleteLabel;
      widgetList2.Add((IWidget) label1);
      string id = this.m_UploadHandle.modInfo.m_PublishedID.ToString();
      LocalizedString localizedString = new LocalizedString("Menu.ASSET_UPLOAD_ID", id, (IReadOnlyDictionary<string, ILocElement>) new System.Collections.Generic.Dictionary<string, ILocElement>()
      {
        {
          "ID",
          (ILocElement) LocalizedString.Value(id)
        }
      });
      List<IWidget> widgetList3 = widgetList1;
      Label label2 = new Label();
      label2.displayName = localizedString;
      widgetList3.Add((IWidget) label2);
      List<IWidget> widgetList4 = widgetList1;
      Button button = new Button();
      button.displayName = (LocalizedString) "Menu.ASSET_COPY_ID";
      button.action = (Action) (() => GUIUtility.systemCopyBuffer = id);
      widgetList4.Add((IWidget) button);
      this.m_PlatformResult.children = (IList<IWidget>) widgetList1;
      this.RefreshSubmitButtonLabel();
      // ISSUE: reference to a compiler-generated method
      this.m_NotificationUISystem.RemoveNotification(AssetUploadPanelUISystem.kNotificationID);
      // ISSUE: reference to a compiler-generated method
      this.m_NotificationUISystem.RemoveNotification(AssetUploadPanelUISystem.kNotificationID, AssetUploadPanelUISystem.kNotificationDelay, new LocalizedString?((LocalizedString) AssetUploadPanelUISystem.kCompleteLabel), new LocalizedString?(localizedString), progressState: new ProgressState?(ProgressState.Complete), onClicked: (Action) (() => GUIUtility.systemCopyBuffer = id));
    }

    private void ReportLocalDataNotFound()
    {
      LayoutContainer platformResult = this.m_PlatformResult;
      IWidget[] widgetArray = new IWidget[1];
      Label label = new Label();
      label.displayName = (LocalizedString) "Paradox.EXISTING_PREVIEWS_ERROR";
      widgetArray[0] = (IWidget) label;
      platformResult.children = (IList<IWidget>) widgetArray;
    }

    private void ClearState()
    {
      this.m_State = AssetUploadPanelUISystem.State.Ready;
      this.m_PlatformResult.children = (IList<IWidget>) Array.Empty<IWidget>();
      this.RefreshSubmitButtonLabel();
    }

    private void ReportSubmitting()
    {
      this.m_State = AssetUploadPanelUISystem.State.Processing;
      LayoutContainer platformResult = this.m_PlatformResult;
      IWidget[] widgetArray = new IWidget[1];
      Label label = new Label();
      label.displayName = (LocalizedString) AssetUploadPanelUISystem.kSubmittingLabel;
      widgetArray[0] = (IWidget) label;
      platformResult.children = (IList<IWidget>) widgetArray;
      // ISSUE: reference to a compiler-generated method
      this.m_NotificationUISystem.AddOrUpdateNotification(AssetUploadPanelUISystem.kNotificationID, new LocalizedString?((LocalizedString) AssetUploadPanelUISystem.kSubmittingLabel), new LocalizedString?((LocalizedString) this.m_UploadHandle.modInfo.m_DisplayName), progressState: new ProgressState?(ProgressState.Indeterminate));
    }

    private void ReportSyncing()
    {
      this.m_State = AssetUploadPanelUISystem.State.Processing;
      LayoutContainer platformResult = this.m_PlatformResult;
      IWidget[] widgetArray = new IWidget[1];
      Label label = new Label();
      label.displayName = (LocalizedString) "Paradox.RETRIEVING_DATA";
      widgetArray[0] = (IWidget) label;
      platformResult.children = (IList<IWidget>) widgetArray;
    }

    private void ReportNoSocial()
    {
      this.m_State = AssetUploadPanelUISystem.State.Disabled;
      LayoutContainer platformResult = this.m_PlatformResult;
      IWidget[] widgetArray = new IWidget[2];
      Label label = new Label();
      label.displayName = (LocalizedString) AssetUploadPanelUISystem.kNoSocialProfile;
      widgetArray[0] = (IWidget) label;
      Button button = new Button();
      button.displayName = (LocalizedString) AssetUploadPanelUISystem.kOpenProfilePage;
      button.action = (Action) (() =>
      {
        this.m_UploadHandle.onSocialProfileSynced += new Action(this.RefreshSocialProfileStatus);
        this.m_UploadHandle.ShowModsUIProfilePage();
      });
      widgetArray[1] = (IWidget) button;
      platformResult.children = (IList<IWidget>) widgetArray;
    }

    private string GetSubmitButtonLabel()
    {
      if (this.m_State == AssetUploadPanelUISystem.State.Success)
        return AssetUploadPanelUISystem.kCompleteLabel;
      if (this.noInternetConnection)
        return AssetUploadPanelUISystem.kNoInternetConnectionLabel;
      return this.notLoggedIn ? AssetUploadPanelUISystem.kNotLoggedInLabel : AssetUploadPanelUISystem.kSubmitLabel;
    }

    private void SetChildren(IWidget[] newChildren)
    {
      if (newChildren == this.m_Children)
        return;
      this.m_Children = (IList<IWidget>) newChildren;
      Action<IList<IWidget>> onChildrenChange = this.onChildrenChange;
      if (onChildrenChange == null)
        return;
      onChildrenChange(this.m_Children);
    }

    public void Show(AssetData mainAsset, bool allowManualFileCopy = false)
    {
      this.m_AllowManualFileCopy = false;
      this.m_UploadHandle = new PdxAssetUploadHandle(mainAsset, Array.Empty<AssetData>());
      this.SetChildren(this.m_MainPanel);
      this.RefreshExternalLinks();
      this.RefreshScreenshots();
      this.RefreshPreview();
      this.RefreshAssetList();
      this.ClearState();
      this.SyncPlatformData();
    }

    private async void SyncPlatformData()
    {
      AssetUploadPanelUISystem uploadPanelUiSystem = this;
      uploadPanelUiSystem.ReportSyncing();
      await uploadPanelUiSystem.m_UploadHandle.SyncPlatformData();
      // ISSUE: reference to a compiler-generated method
      GameManager.instance.RunOnMainThread(new Action(uploadPanelUiSystem.\u003CSyncPlatformData\u003Eb__70_0));
    }

    public bool Close()
    {
      if (this.children == this.m_PreviewPickerPanel)
      {
        this.SetChildren(this.m_MainPanel);
        return false;
      }
      if (this.m_ExistingModField.expanded)
      {
        this.m_ExistingModField.expanded = false;
        return false;
      }
      if (this.m_AssetListPopup == null || !this.m_AssetListPopup.expanded)
        return true;
      this.m_AssetListPopup.expanded = false;
      return false;
    }

    private void RefreshSocialProfileStatus()
    {
      if (string.IsNullOrEmpty(this.m_UploadHandle.socialProfile.m_Name))
      {
        this.ReportNoSocial();
      }
      else
      {
        if (this.m_State != AssetUploadPanelUISystem.State.Disabled)
          return;
        this.ClearState();
      }
    }

    private void Submit()
    {
      this.ReportSubmitting();
      this.BeginSubmit();
    }

    private async void BeginSubmit()
    {
      IModsUploadSupport.ModOperationResult result = await this.m_UploadHandle.BeginSubmit();
      if (result.m_Success)
      {
        if (this.m_AllowManualFileCopy)
          GameManager.instance.RunOnMainThread((Action) (() =>
          {
            GUIUtility.systemCopyBuffer = this.m_UploadHandle.GetAbsoluteContentPath();
            GameManager.instance.userInterface.paradoxBindings.PushDialog((ParadoxBindings.ParadoxDialog) new ParadoxBindings.MultiOptionDialog("Ready to upload", "A work-in-progress folder has been created in " + this.m_UploadHandle.GetAbsoluteContentPath() + " where you may now copy any additional files you wish to share. Press Submit once you're ready to finalize the upload.", new ParadoxBindings.MultiOptionDialog.Option[2]
            {
              new ParadoxBindings.MultiOptionDialog.Option()
              {
                m_Id = "Submit",
                m_OnSelect = new Action(this.FinalizeSubmit)
              },
              new ParadoxBindings.MultiOptionDialog.Option()
              {
                m_Id = "Cancel",
                m_OnSelect = new Action(this.Cancel)
              }
            }));
          }));
        else
          this.FinalizeSubmit();
      }
      else
        GameManager.instance.RunOnMainThread((Action) (() => this.ReportError(result)));
    }

    private async void FinalizeSubmit()
    {
      IModsUploadSupport.ModOperationResult result = await this.m_UploadHandle.FinalizeSubmit();
      GameManager.instance.RunOnMainThread((Action) (() =>
      {
        if (!result.m_Success)
          this.ReportError(result);
        else
          this.ReportSuccess();
      }));
    }

    private async void Cancel()
    {
      // ISSUE: reference to a compiler-generated method
      this.m_NotificationUISystem.RemoveNotification(AssetUploadPanelUISystem.kNotificationID);
      await this.m_UploadHandle.Cleanup();
      this.ClearState();
    }

    private void RefreshExternalLinks()
    {
      this.m_ExternalLinkField.links = this.m_UploadHandle.modInfo.m_ExternalLinks;
      this.m_ExternalLinkField.SetPropertiesChanged();
    }

    private void RefreshScreenshots()
    {
      List<IWidget> widgetList = new List<IWidget>(this.m_UploadHandle.screenshots.Count + 1);
      for (int index1 = 0; index1 < this.m_UploadHandle.screenshots.Count; ++index1)
      {
        int index = index1;
        widgetList.Add((IWidget) new IconButton()
        {
          icon = AssetUploadUtils.GetImageURI(this.m_UploadHandle.screenshots[index]),
          action = (Action) (() => this.RemoveScreenshot(index))
        });
      }
      widgetList.Add((IWidget) new IconButton()
      {
        icon = "Media/Glyphs/Plus.svg",
        action = (Action) (() => this.OpenPreviewPickerPanel(new Action<Colossal.Hash128>(this.AddScreenshot), excludeScreenshots: true))
      });
      this.m_Screenshots.children = (IList<IWidget>) widgetList;
    }

    private void RefreshPreview()
    {
      this.m_PreviewPickerButton.icon = AssetUploadUtils.GetImageURI(this.m_UploadHandle.preview);
      this.m_PreviewPickerButton.SetPropertiesChanged();
    }

    private void RefreshAssetList()
    {
      if (this.m_AssetList == null)
        return;
      List<ListField.Item> objList = new List<ListField.Item>();
      foreach (AssetData asset in (IEnumerable<AssetData>) this.m_UploadHandle.assets)
        objList.Add(AssetUploadPanelUISystem.GetItem(asset, false, asset == (IAssetData) this.m_UploadHandle.mainAsset));
      foreach (AssetData additionalAsset in (IEnumerable<AssetData>) this.m_UploadHandle.additionalAssets)
        objList.Add(AssetUploadPanelUISystem.GetItem(additionalAsset, true, additionalAsset == (IAssetData) this.m_UploadHandle.mainAsset));
      this.m_AssetList.m_Items = objList;
      this.m_AssetList.SetPropertiesChanged();
    }

    private static ListField.Item GetItem(AssetData asset, bool removable, bool mainAsset)
    {
      ListField.Item obj = new ListField.Item()
      {
        m_Label = AssetUploadPanelUISystem.GetLabel(asset),
        m_Removable = removable,
        m_Data = (object) asset
      };
      if (asset is PrefabAsset prefabAsset)
      {
        HashSet<AssetData> assetDataSet = new HashSet<AssetData>();
        AssetUploadUtils.CollectPrefabAssetDependencies(prefabAsset, assetDataSet, mainAsset);
        obj.m_SubItems = assetDataSet.Where<AssetData>((Func<AssetData, bool>) (dep => dep != (IAssetData) asset && dep is PrefabAsset)).Select<AssetData, string>(new Func<AssetData, string>(AssetUploadPanelUISystem.GetLabel)).ToArray<string>();
      }
      return obj;
    }

    private static string GetLabel(AssetData asset)
    {
      SourceMeta meta = asset.GetMeta();
      if (meta.platformID > 0)
        return string.Format("{0} ({1})", (object) asset.name, (object) meta.platformID);
      return meta.isPackaged ? asset.name + " (" + meta.packageName + ")" : asset.name;
    }

    private void OnAddPrefab(PrefabBase prefab)
    {
      this.m_AssetListPopup.expanded = false;
      if ((UnityEngine.Object) prefab == (UnityEngine.Object) null)
        return;
      this.m_UploadHandle.AddAdditionalAsset((AssetData) prefab.asset);
      this.RefreshAssetList();
    }

    private void OnRemoveAdditionalAsset(int index)
    {
      AssetData data = this.m_AssetList.m_Items[index].m_Data as AssetData;
      if ((object) data == null)
        return;
      this.m_UploadHandle.RemoveAdditionalAsset(data);
      this.RefreshAssetList();
    }

    private bool ShouldShowInPrefabPicker(PrefabBase prefab)
    {
      return (AssetData) prefab.asset != (IAssetData) null && prefab.asset.database == Colossal.IO.AssetDatabase.AssetDatabase.user && !this.m_UploadHandle.cachedDependencies.Contains((AssetData) prefab.asset);
    }

    private void RefreshTags()
    {
      List<ListField.Item> objList1 = new List<ListField.Item>();
      foreach (string tag in this.m_UploadHandle.tags)
        objList1.Add(new ListField.Item()
        {
          m_Label = tag,
          m_Removable = false,
          m_Data = (object) tag
        });
      foreach (string additionalTag in this.m_UploadHandle.additionalTags)
        objList1.Add(new ListField.Item()
        {
          m_Label = additionalTag,
          m_Removable = true,
          m_Data = (object) additionalTag
        });
      this.m_TagsList.m_Items = objList1;
      this.m_TagsList.SetPropertiesChanged();
      List<ItemPickerPopup<string>.Item> items = new List<ItemPickerPopup<string>.Item>();
      foreach (IModsUploadSupport.ModTag availableTag in this.m_UploadHandle.availableTags)
      {
        if (!this.m_UploadHandle.tags.Contains(availableTag.m_Id) && !this.m_UploadHandle.additionalTags.Contains(availableTag.m_Id))
        {
          List<ItemPickerPopup<string>.Item> objList2 = items;
          ItemPickerPopup<string>.Item obj = new ItemPickerPopup<string>.Item();
          obj.m_Value = availableTag.m_Id;
          obj.displayName = (LocalizedString) availableTag.m_DisplayName;
          objList2.Add(obj);
        }
      }
      ((ItemPickerPopup<string>) this.m_TagsListPopup.popup).SetItems((IEnumerable<ItemPickerPopup<string>.Item>) items);
    }

    private void OnAddTag(string tag)
    {
      this.m_TagsListPopup.expanded = false;
      if (tag != null)
        this.m_UploadHandle.AddAdditionalTag(tag);
      this.RefreshTags();
    }

    private void OnRemoveTag(int index)
    {
      if (this.m_TagsList.m_Items[index].m_Data is string data)
        this.m_UploadHandle.RemoveAdditionalTag(data);
      this.RefreshTags();
    }

    private void OpenPreviewPickerPanel(
      Action<Colossal.Hash128> callback,
      Colossal.Hash128 defaultSelection = default (Colossal.Hash128),
      bool excludeScreenshots = false)
    {
      this.m_PreviewPickerAdapter.SetItems(this.GetPreviews(excludeScreenshots));
      this.m_PreviewPickerAdapter.SelectItemByGuid(defaultSelection);
      this.SetChildren(this.m_PreviewPickerPanel);
      this.m_SelectPreviewButton.action = (Action) (() => this.ClosePreviewPickerPanel(callback));
    }

    private void ClosePreviewPickerPanel(Action<Colossal.Hash128> callback)
    {
      this.SetChildren(this.m_MainPanel);
      if (callback == null)
        return;
      callback(this.m_PreviewPickerAdapter.selectedItem.guid);
    }

    private void SetPreview(Colossal.Hash128 guid)
    {
      AssetData assetData;
      if (Colossal.IO.AssetDatabase.AssetDatabase.global.TryGetAsset<AssetData>(guid, out assetData))
        this.m_UploadHandle.SetPreview(assetData);
      this.RefreshPreview();
    }

    private void AddScreenshot(Colossal.Hash128 guid)
    {
      AssetData assetData;
      if (Colossal.IO.AssetDatabase.AssetDatabase.global.TryGetAsset<AssetData>(guid, out assetData))
        this.m_UploadHandle.AddScreenshot(assetData);
      this.RefreshScreenshots();
    }

    private void RemoveScreenshot(int index)
    {
      this.m_UploadHandle.RemoveScreenshot(this.m_UploadHandle.screenshots[index]);
      this.RefreshScreenshots();
    }

    private IEnumerable<AssetItem> GetPreviews(bool excludeScreenshots = false)
    {
      HashSet<IAssetData> screenshots = excludeScreenshots ? new HashSet<IAssetData>((IEnumerable<IAssetData>) this.m_UploadHandle.screenshots) : (HashSet<IAssetData>) null;
      foreach (AssetData originalPreview in (IEnumerable<AssetData>) this.m_UploadHandle.originalPreviews)
      {
        if (!excludeScreenshots || !screenshots.Contains((IAssetData) originalPreview))
        {
          AssetItem preview = new AssetItem();
          preview.guid = originalPreview.guid;
          preview.image = AssetUploadUtils.GetImageURI(originalPreview);
          yield return preview;
        }
      }
      foreach (ImageAsset asset in Colossal.IO.AssetDatabase.AssetDatabase.global.GetAssets<ImageAsset>(SearchFilter<ImageAsset>.ByCondition((Func<ImageAsset, bool>) (a =>
      {
        string subPath = a.GetMeta().subPath;
        return subPath != null && subPath.StartsWith(ScreenUtility.kScreenshotDirectory);
      }))))
      {
        if (!excludeScreenshots || !screenshots.Contains((IAssetData) asset))
        {
          AssetItem preview = new AssetItem();
          preview.guid = asset.guid;
          preview.fileName = asset.name;
          preview.displayName = (LocalizedString) asset.name;
          preview.image = asset.ToUri();
          yield return preview;
        }
      }
    }

    private void RefreshAuthorMods() => this.m_ExistingModPopup.SetItems(this.GetExistingMods());

    private IEnumerable<ItemPickerPopup<int>.Item> GetExistingMods()
    {
      ItemPickerPopup<int>.Item existingMod1 = new ItemPickerPopup<int>.Item();
      existingMod1.m_Value = -1;
      existingMod1.displayName = AssetUploadPanelUISystem.kNone;
      yield return existingMod1;
      foreach (IModsUploadSupport.ModInfo authorMod in this.m_UploadHandle.authorMods)
      {
        ItemPickerPopup<int>.Item existingMod2 = new ItemPickerPopup<int>.Item();
        existingMod2.m_Value = authorMod.m_PublishedID;
        existingMod2.displayName = LocalizedString.Value(authorMod.m_DisplayName);
        existingMod2.m_SearchTerms = new string[2]
        {
          authorMod.m_PublishedID.ToString(),
          authorMod.m_DisplayName
        };
        yield return existingMod2;
      }
    }

    private async void SetInfoFromExisting()
    {
      this.m_State = AssetUploadPanelUISystem.State.Processing;
      IModsUploadSupport.ModInfo info = await this.m_UploadHandle.GetExistingInfo();
      (bool flag, IModsUploadSupport.ModLocalData localData) = await this.m_UploadHandle.GetLocalData(info.m_PublishedID);
      GameManager.instance.RunOnMainThread((Action) (() =>
      {
        this.m_State = AssetUploadPanelUISystem.State.Ready;
        this.m_UploadHandle.modInfo = info;
        this.RefreshExternalLinks();
        if (flag)
        {
          this.ClearState();
          this.m_UploadHandle.SetPreviewsFromExisting(localData);
          this.RefreshPreview();
          this.RefreshScreenshots();
        }
        else
          this.ReportLocalDataNotFound();
      }));
    }

    [Preserve]
    public AssetUploadPanelUISystem()
    {
    }

    private enum State
    {
      Ready,
      Processing,
      Success,
      Failure,
      Disabled,
    }
  }
}
