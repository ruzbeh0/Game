// Decompiled with JetBrains decompiler
// Type: Game.UI.Menu.NotificationUISystem
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal.Annotations;
using Colossal.PSI.Common;
using Colossal.UI.Binding;
using Game.PSI;
using Game.UI.Localization;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Scripting;

#nullable disable
namespace Game.UI.Menu
{
  [CompilerGenerated]
  public class NotificationUISystem : UISystemBase
  {
    private const string kGroup = "notification";
    private const string kInstallation = "installation";
    private const string kDownloading = "downloading";
    private const float kDelay = 2f;
    private ValueBinding<List<NotificationUISystem.NotificationInfo>> m_NotificationsBinding;
    private Dictionary<string, NotificationUISystem.DelayedNotificationInfo> m_PendingRemoval;
    private Dictionary<string, NotificationUISystem.NotificationInfo> m_NotificationsMap;
    private Dictionary<int, Mod> m_ModInfoCache;
    private bool m_Dirty;

    [Preserve]
    protected override void OnCreate()
    {
      NotificationSystem.BindUI(this);
      base.OnCreate();
      // ISSUE: reference to a compiler-generated field
      this.m_NotificationsMap = new Dictionary<string, NotificationUISystem.NotificationInfo>();
      // ISSUE: reference to a compiler-generated field
      this.m_PendingRemoval = new Dictionary<string, NotificationUISystem.DelayedNotificationInfo>();
      // ISSUE: reference to a compiler-generated field
      this.m_ModInfoCache = new Dictionary<int, Mod>();
      // ISSUE: reference to a compiler-generated field
      this.AddBinding((IBinding) (this.m_NotificationsBinding = new ValueBinding<List<NotificationUISystem.NotificationInfo>>("notification", "notifications", new List<NotificationUISystem.NotificationInfo>(), (IWriter<List<NotificationUISystem.NotificationInfo>>) new ListWriter<NotificationUISystem.NotificationInfo>((IWriter<NotificationUISystem.NotificationInfo>) new ValueWriter<NotificationUISystem.NotificationInfo>()))));
      this.AddBinding((IBinding) new TriggerBinding<string>("notification", "selectNotification", (Action<string>) (notificationId =>
      {
        // ISSUE: variable of a compiler-generated type
        NotificationUISystem.NotificationInfo notificationInfo;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_NotificationsMap.TryGetValue(notificationId, out notificationInfo))
          return;
        // ISSUE: reference to a compiler-generated field
        Action onClicked = notificationInfo.onClicked;
        if (onClicked == null)
          return;
        onClicked();
      })));
      PlatformManager.instance.onModSubscriptionChanged += (ModSubscriptionEventHandler) ((psi, mod, status) =>
      {
        string str = status.ToString();
        // ISSUE: reference to a compiler-generated method
        string modNotificationId = this.GetModNotificationId(mod, str);
        LocalizedString? title = new LocalizedString?(LocalizedString.Value(mod.displayName));
        // ISSUE: reference to a compiler-generated method
        LocalizedString? text = new LocalizedString?((LocalizedString) NotificationUISystem.GetText(str));
        string thumbnail = string.Format("{0}?width={1})", (object) mod.thumbnailPath, (object) NotificationUISystem.width);
        Action onClick = mod.onClick;
        ProgressState? progressState = new ProgressState?();
        int? progress = new int?();
        Action onClicked = onClick;
        // ISSUE: reference to a compiler-generated method
        this.RemoveNotification(modNotificationId, 2f, title, text, thumbnail, progressState, progress, onClicked);
      });
      PlatformManager.instance.onModDownloadStarted += (ModEventHandler) ((psi, mod) =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ModInfoCache[mod.id] = mod;
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        this.AddModNotification(mod, this.GetModNotificationId(mod, "installation"));
        // ISSUE: reference to a compiler-generated method
        string modNotificationId = this.GetModNotificationId(mod, "installation");
        // ISSUE: reference to a compiler-generated method
        LocalizedString? nullable1 = new LocalizedString?((LocalizedString) NotificationUISystem.GetText("DownloadPending"));
        int? nullable2 = new int?(0);
        LocalizedString? title = new LocalizedString?();
        LocalizedString? text = nullable1;
        ProgressState? progressState = new ProgressState?();
        int? progress = nullable2;
        // ISSUE: reference to a compiler-generated method
        this.AddOrUpdateNotification(modNotificationId, title, text, progressState: progressState, progress: progress);
      });
      PlatformManager.instance.onModDownloadCompleted += (ModEventHandler) ((psi, mod) =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ModInfoCache.Remove(mod.id);
        // ISSUE: reference to a compiler-generated method
        string modNotificationId = this.GetModNotificationId(mod, "installation");
        // ISSUE: reference to a compiler-generated method
        LocalizedString? nullable3 = new LocalizedString?((LocalizedString) NotificationUISystem.GetText("InstallComplete"));
        ProgressState? nullable4 = new ProgressState?(ProgressState.Complete);
        int? nullable5 = new int?(100);
        LocalizedString? title = new LocalizedString?();
        LocalizedString? text = nullable3;
        ProgressState? progressState = nullable4;
        int? progress = nullable5;
        // ISSUE: reference to a compiler-generated method
        this.RemoveNotification(modNotificationId, 2f, title, text, progressState: progressState, progress: progress);
      });
      PlatformManager.instance.onModDownloadFailed += (ModEventHandler) ((psi, mod) =>
      {
        // ISSUE: reference to a compiler-generated field
        this.m_ModInfoCache.Remove(mod.id);
        // ISSUE: reference to a compiler-generated method
        string modNotificationId = this.GetModNotificationId(mod, "installation");
        // ISSUE: reference to a compiler-generated method
        LocalizedString? nullable6 = new LocalizedString?((LocalizedString) NotificationUISystem.GetText("InstallFailed"));
        ProgressState? nullable7 = new ProgressState?(ProgressState.Failed);
        int? nullable8 = new int?(100);
        LocalizedString? title = new LocalizedString?();
        LocalizedString? text = nullable6;
        ProgressState? progressState = nullable7;
        int? progress = nullable8;
        // ISSUE: reference to a compiler-generated method
        this.RemoveNotification(modNotificationId, 2f, title, text, progressState: progressState, progress: progress);
      });
      PlatformManager.instance.onModInstallProgress += (ModInstallProgressEventHandler) ((psi, modId, status) =>
      {
        ProgressState progressState1 = status.type == TransferType.Install ? status.progressState : ProgressState.Progressing;
        // ISSUE: reference to a compiler-generated method
        string modNotificationId1 = this.GetModNotificationId(status.id, "installation");
        // ISSUE: reference to a compiler-generated method
        LocalizedString? nullable9 = new LocalizedString?((LocalizedString) NotificationUISystem.GetText(string.Format("{0}{1}", (object) TransferType.Install, (object) progressState1)));
        ProgressState? nullable10 = new ProgressState?(progressState1);
        int? nullable11 = new int?(Mathf.CeilToInt(status.progress * 100f));
        LocalizedString? title1 = new LocalizedString?();
        LocalizedString? text1 = nullable9;
        ProgressState? progressState2 = nullable10;
        int? progress1 = nullable11;
        // ISSUE: reference to a compiler-generated method
        this.AddOrUpdateNotification(modNotificationId1, title1, text1, progressState: progressState2, progress: progress1);
        if (status.type != TransferType.Download)
          return;
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        if (status.progressState == ProgressState.Progressing && !this.NotificationExists(this.GetModNotificationId(status.id, "downloading")))
        {
          Mod mod;
          // ISSUE: reference to a compiler-generated field
          if (!this.m_ModInfoCache.TryGetValue(modId, out mod))
            mod = new Mod() { id = modId };
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          this.AddModNotification(mod, this.GetModNotificationId(mod, "downloading"));
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          // ISSUE: reference to a compiler-generated method
          if (status.progressState != ProgressState.Complete || !this.NotificationExists(this.GetModNotificationId(status.id, "downloading")))
            return;
          // ISSUE: reference to a compiler-generated method
          string modNotificationId2 = this.GetModNotificationId(status.id, "downloading");
          // ISSUE: reference to a compiler-generated method
          nullable9 = new LocalizedString?((LocalizedString) NotificationUISystem.GetText("DownloadComplete"));
          nullable10 = new ProgressState?(ProgressState.Complete);
          nullable11 = new int?(100);
          LocalizedString? title2 = new LocalizedString?();
          LocalizedString? text2 = nullable9;
          ProgressState? progressState3 = nullable10;
          int? progress2 = nullable11;
          // ISSUE: reference to a compiler-generated method
          this.RemoveNotification(modNotificationId2, 2f, title2, text2, progressState: progressState3, progress: progress2);
        }
      });
      PlatformManager.instance.onTransferOnGoing += (TransferEventHandler) ((psi, status) =>
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        if (!this.NotificationExists(this.GetModNotificationId(status.id, "downloading")))
          return;
        if (status.progressState == ProgressState.Complete)
        {
          // ISSUE: reference to a compiler-generated method
          string modNotificationId = this.GetModNotificationId(status.id, "downloading");
          // ISSUE: reference to a compiler-generated method
          LocalizedString? nullable12 = new LocalizedString?((LocalizedString) NotificationUISystem.GetText("DownloadComplete"));
          ProgressState? nullable13 = new ProgressState?(ProgressState.Complete);
          int? nullable14 = new int?(100);
          LocalizedString? title = new LocalizedString?();
          LocalizedString? text = nullable12;
          ProgressState? progressState = nullable13;
          int? progress = nullable14;
          // ISSUE: reference to a compiler-generated method
          this.RemoveNotification(modNotificationId, 2f, title, text, progressState: progressState, progress: progress);
        }
        else if (status.progressState == ProgressState.Failed)
        {
          // ISSUE: reference to a compiler-generated method
          string modNotificationId = this.GetModNotificationId(status.id, "downloading");
          // ISSUE: reference to a compiler-generated method
          LocalizedString? nullable15 = new LocalizedString?((LocalizedString) NotificationUISystem.GetText("DownloadFailed"));
          ProgressState? nullable16 = new ProgressState?(ProgressState.Failed);
          int? nullable17 = new int?(100);
          LocalizedString? title = new LocalizedString?();
          LocalizedString? text = nullable15;
          ProgressState? progressState = nullable16;
          int? progress = nullable17;
          // ISSUE: reference to a compiler-generated method
          this.RemoveNotification(modNotificationId, 2f, title, text, progressState: progressState, progress: progress);
        }
        else
        {
          // ISSUE: reference to a compiler-generated method
          string modNotificationId = this.GetModNotificationId(status.id, "downloading");
          // ISSUE: reference to a compiler-generated method
          LocalizedString? nullable18 = new LocalizedString?((LocalizedString) NotificationUISystem.GetText("DownloadProgressing"));
          ProgressState? nullable19 = new ProgressState?(status.progressState);
          int? nullable20 = new int?(Mathf.CeilToInt(status.progress * 100f));
          LocalizedString? title = new LocalizedString?();
          LocalizedString? text = nullable18;
          ProgressState? progressState = nullable19;
          int? progress = nullable20;
          // ISSUE: reference to a compiler-generated method
          this.AddOrUpdateNotification(modNotificationId, title, text, progressState: progressState, progress: progress);
        }
      });
    }

    [Preserve]
    protected override void OnDestroy()
    {
      base.OnDestroy();
      NotificationSystem.UnbindUI();
    }

    public static int width => Mathf.CeilToInt(48f * ((float) Screen.width / 1920f));

    public static string GetTitle(string titleId) => "Menu.NOTIFICATION_TITLE[" + titleId + "]";

    public static string GetText(string textId) => "Menu.NOTIFICATION_DESCRIPTION[" + textId + "]";

    private void AddModNotification(Mod mod, string notificationId = null)
    {
      // ISSUE: reference to a compiler-generated method
      string identifier = notificationId ?? this.GetModNotificationId(mod);
      LocalizedString? title = new LocalizedString?(LocalizedString.Value(mod.displayName));
      string str = string.Format("{0}?width={1})", (object) mod.thumbnailPath, (object) NotificationUISystem.width);
      Action onClick = mod.onClick;
      LocalizedString? text = new LocalizedString?();
      string thumbnail = str;
      ProgressState? progressState = new ProgressState?();
      int? progress = new int?();
      Action onClicked = onClick;
      // ISSUE: reference to a compiler-generated method
      this.AddOrUpdateNotification(identifier, title, text, thumbnail, progressState, progress, onClicked);
    }

    private string GetModNotificationId(Mod mod, string suffix = null)
    {
      // ISSUE: reference to a compiler-generated method
      return this.GetModNotificationId(mod.id.ToString(), suffix);
    }

    private string GetModNotificationId(string modId, string suffix = null)
    {
      return !string.IsNullOrEmpty(suffix) ? modId + "." + suffix : modId;
    }

    private void HandleModSubscription(IModSupport psi, Mod mod, ModSubscriptionStatus status)
    {
      string str = status.ToString();
      // ISSUE: reference to a compiler-generated method
      string modNotificationId = this.GetModNotificationId(mod, str);
      LocalizedString? title = new LocalizedString?(LocalizedString.Value(mod.displayName));
      // ISSUE: reference to a compiler-generated method
      LocalizedString? text = new LocalizedString?((LocalizedString) NotificationUISystem.GetText(str));
      string thumbnail = string.Format("{0}?width={1})", (object) mod.thumbnailPath, (object) NotificationUISystem.width);
      Action onClick = mod.onClick;
      ProgressState? progressState = new ProgressState?();
      int? progress = new int?();
      Action onClicked = onClick;
      // ISSUE: reference to a compiler-generated method
      this.RemoveNotification(modNotificationId, 2f, title, text, thumbnail, progressState, progress, onClicked);
    }

    private void HandleModDownloadStarted(IModSupport psi, Mod mod)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ModInfoCache[mod.id] = mod;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      this.AddModNotification(mod, this.GetModNotificationId(mod, "installation"));
      // ISSUE: reference to a compiler-generated method
      string modNotificationId = this.GetModNotificationId(mod, "installation");
      // ISSUE: reference to a compiler-generated method
      LocalizedString? nullable1 = new LocalizedString?((LocalizedString) NotificationUISystem.GetText("DownloadPending"));
      int? nullable2 = new int?(0);
      LocalizedString? title = new LocalizedString?();
      LocalizedString? text = nullable1;
      ProgressState? progressState = new ProgressState?();
      int? progress = nullable2;
      // ISSUE: reference to a compiler-generated method
      this.AddOrUpdateNotification(modNotificationId, title, text, progressState: progressState, progress: progress);
    }

    private void HandleModDownloadCompleted(IModSupport psi, Mod mod)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ModInfoCache.Remove(mod.id);
      // ISSUE: reference to a compiler-generated method
      string modNotificationId = this.GetModNotificationId(mod, "installation");
      // ISSUE: reference to a compiler-generated method
      LocalizedString? nullable1 = new LocalizedString?((LocalizedString) NotificationUISystem.GetText("InstallComplete"));
      ProgressState? nullable2 = new ProgressState?(ProgressState.Complete);
      int? nullable3 = new int?(100);
      LocalizedString? title = new LocalizedString?();
      LocalizedString? text = nullable1;
      ProgressState? progressState = nullable2;
      int? progress = nullable3;
      // ISSUE: reference to a compiler-generated method
      this.RemoveNotification(modNotificationId, 2f, title, text, progressState: progressState, progress: progress);
    }

    private void HandleModDownloadFailed(IModSupport psi, Mod mod)
    {
      // ISSUE: reference to a compiler-generated field
      this.m_ModInfoCache.Remove(mod.id);
      // ISSUE: reference to a compiler-generated method
      string modNotificationId = this.GetModNotificationId(mod, "installation");
      // ISSUE: reference to a compiler-generated method
      LocalizedString? nullable1 = new LocalizedString?((LocalizedString) NotificationUISystem.GetText("InstallFailed"));
      ProgressState? nullable2 = new ProgressState?(ProgressState.Failed);
      int? nullable3 = new int?(100);
      LocalizedString? title = new LocalizedString?();
      LocalizedString? text = nullable1;
      ProgressState? progressState = nullable2;
      int? progress = nullable3;
      // ISSUE: reference to a compiler-generated method
      this.RemoveNotification(modNotificationId, 2f, title, text, progressState: progressState, progress: progress);
    }

    private void HandleModInstallProgress(IModSupport psi, int modId, TransferStatus status)
    {
      ProgressState progressState1 = status.type == TransferType.Install ? status.progressState : ProgressState.Progressing;
      // ISSUE: reference to a compiler-generated method
      string modNotificationId1 = this.GetModNotificationId(status.id, "installation");
      // ISSUE: reference to a compiler-generated method
      LocalizedString? nullable1 = new LocalizedString?((LocalizedString) NotificationUISystem.GetText(string.Format("{0}{1}", (object) TransferType.Install, (object) progressState1)));
      ProgressState? nullable2 = new ProgressState?(progressState1);
      int? nullable3 = new int?(Mathf.CeilToInt(status.progress * 100f));
      LocalizedString? title1 = new LocalizedString?();
      LocalizedString? text1 = nullable1;
      ProgressState? progressState2 = nullable2;
      int? progress1 = nullable3;
      // ISSUE: reference to a compiler-generated method
      this.AddOrUpdateNotification(modNotificationId1, title1, text1, progressState: progressState2, progress: progress1);
      if (status.type != TransferType.Download)
        return;
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      if (status.progressState == ProgressState.Progressing && !this.NotificationExists(this.GetModNotificationId(status.id, "downloading")))
      {
        Mod mod;
        // ISSUE: reference to a compiler-generated field
        if (!this.m_ModInfoCache.TryGetValue(modId, out mod))
          mod = new Mod() { id = modId };
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        this.AddModNotification(mod, this.GetModNotificationId(mod, "downloading"));
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        // ISSUE: reference to a compiler-generated method
        if (status.progressState != ProgressState.Complete || !this.NotificationExists(this.GetModNotificationId(status.id, "downloading")))
          return;
        // ISSUE: reference to a compiler-generated method
        string modNotificationId2 = this.GetModNotificationId(status.id, "downloading");
        // ISSUE: reference to a compiler-generated method
        nullable1 = new LocalizedString?((LocalizedString) NotificationUISystem.GetText("DownloadComplete"));
        nullable2 = new ProgressState?(ProgressState.Complete);
        nullable3 = new int?(100);
        LocalizedString? title2 = new LocalizedString?();
        LocalizedString? text2 = nullable1;
        ProgressState? progressState3 = nullable2;
        int? progress2 = nullable3;
        // ISSUE: reference to a compiler-generated method
        this.RemoveNotification(modNotificationId2, 2f, title2, text2, progressState: progressState3, progress: progress2);
      }
    }

    private void HandleTransferOnGoing(ITransferSupport psi, TransferStatus status)
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: reference to a compiler-generated method
      if (!this.NotificationExists(this.GetModNotificationId(status.id, "downloading")))
        return;
      if (status.progressState == ProgressState.Complete)
      {
        // ISSUE: reference to a compiler-generated method
        string modNotificationId = this.GetModNotificationId(status.id, "downloading");
        // ISSUE: reference to a compiler-generated method
        LocalizedString? nullable1 = new LocalizedString?((LocalizedString) NotificationUISystem.GetText("DownloadComplete"));
        ProgressState? nullable2 = new ProgressState?(ProgressState.Complete);
        int? nullable3 = new int?(100);
        LocalizedString? title = new LocalizedString?();
        LocalizedString? text = nullable1;
        ProgressState? progressState = nullable2;
        int? progress = nullable3;
        // ISSUE: reference to a compiler-generated method
        this.RemoveNotification(modNotificationId, 2f, title, text, progressState: progressState, progress: progress);
      }
      else if (status.progressState == ProgressState.Failed)
      {
        // ISSUE: reference to a compiler-generated method
        string modNotificationId = this.GetModNotificationId(status.id, "downloading");
        // ISSUE: reference to a compiler-generated method
        LocalizedString? nullable4 = new LocalizedString?((LocalizedString) NotificationUISystem.GetText("DownloadFailed"));
        ProgressState? nullable5 = new ProgressState?(ProgressState.Failed);
        int? nullable6 = new int?(100);
        LocalizedString? title = new LocalizedString?();
        LocalizedString? text = nullable4;
        ProgressState? progressState = nullable5;
        int? progress = nullable6;
        // ISSUE: reference to a compiler-generated method
        this.RemoveNotification(modNotificationId, 2f, title, text, progressState: progressState, progress: progress);
      }
      else
      {
        // ISSUE: reference to a compiler-generated method
        string modNotificationId = this.GetModNotificationId(status.id, "downloading");
        // ISSUE: reference to a compiler-generated method
        LocalizedString? nullable7 = new LocalizedString?((LocalizedString) NotificationUISystem.GetText("DownloadProgressing"));
        ProgressState? nullable8 = new ProgressState?(status.progressState);
        int? nullable9 = new int?(Mathf.CeilToInt(status.progress * 100f));
        LocalizedString? title = new LocalizedString?();
        LocalizedString? text = nullable7;
        ProgressState? progressState = nullable8;
        int? progress = nullable9;
        // ISSUE: reference to a compiler-generated method
        this.AddOrUpdateNotification(modNotificationId, title, text, progressState: progressState, progress: progress);
      }
    }

    [Preserve]
    protected override void OnUpdate()
    {
      base.OnUpdate();
      // ISSUE: reference to a compiler-generated method
      this.ProcessPendingRemovals(this.CheckedStateRef.WorldUnmanaged.Time.DeltaTime);
      // ISSUE: reference to a compiler-generated field
      if (!this.m_Dirty)
        return;
      // ISSUE: reference to a compiler-generated field
      this.m_Dirty = false;
      // ISSUE: reference to a compiler-generated field
      this.m_NotificationsBinding.TriggerUpdate();
    }

    private void SelectNotification(string notificationId)
    {
      // ISSUE: variable of a compiler-generated type
      NotificationUISystem.NotificationInfo notificationInfo;
      // ISSUE: reference to a compiler-generated field
      if (!this.m_NotificationsMap.TryGetValue(notificationId, out notificationInfo))
        return;
      // ISSUE: reference to a compiler-generated field
      Action onClicked = notificationInfo.onClicked;
      if (onClicked == null)
        return;
      onClicked();
    }

    private void UpdateNotification(
      NotificationUISystem.NotificationInfo notificationInfo,
      LocalizedString? title,
      LocalizedString? text,
      string thumbnail,
      ProgressState? progressState,
      int? progress,
      Action onClicked)
    {
      // ISSUE: reference to a compiler-generated field
      if (title.HasValue && !notificationInfo.title.HasValue)
      {
        // ISSUE: reference to a compiler-generated field
        notificationInfo.title = title;
      }
      if (text.HasValue)
      {
        // ISSUE: reference to a compiler-generated field
        notificationInfo.text = text;
      }
      // ISSUE: reference to a compiler-generated field
      if (thumbnail != null && notificationInfo.thumbnail == null)
      {
        // ISSUE: reference to a compiler-generated field
        notificationInfo.thumbnail = thumbnail;
      }
      if (progressState.HasValue)
      {
        // ISSUE: reference to a compiler-generated field
        notificationInfo.progressState = progressState.Value;
      }
      if (progress.HasValue)
      {
        // ISSUE: reference to a compiler-generated field
        notificationInfo.progress = progress.Value;
      }
      // ISSUE: reference to a compiler-generated field
      if (onClicked == null || notificationInfo.onClicked != null)
        return;
      // ISSUE: reference to a compiler-generated field
      notificationInfo.onClicked = onClicked;
    }

    public NotificationUISystem.NotificationInfo AddOrUpdateNotification(
      string identifier,
      LocalizedString? title = null,
      LocalizedString? text = null,
      string thumbnail = null,
      ProgressState? progressState = null,
      int? progress = null,
      Action onClicked = null)
    {
      // ISSUE: variable of a compiler-generated type
      NotificationUISystem.NotificationInfo notificationInfo;
      // ISSUE: reference to a compiler-generated field
      if (this.m_NotificationsMap.TryGetValue(identifier, out notificationInfo))
      {
        // ISSUE: reference to a compiler-generated method
        this.UpdateNotification(notificationInfo, title, text, thumbnail, progressState, progress, onClicked);
      }
      else
      {
        // ISSUE: object of a compiler-generated type is created
        notificationInfo = new NotificationUISystem.NotificationInfo(identifier);
        // ISSUE: reference to a compiler-generated method
        this.UpdateNotification(notificationInfo, title, text, thumbnail, progressState, progress, onClicked);
        // ISSUE: reference to a compiler-generated field
        this.m_NotificationsMap.Add(identifier, notificationInfo);
        // ISSUE: reference to a compiler-generated field
        this.m_NotificationsBinding.value.Add(notificationInfo);
      }
      // ISSUE: reference to a compiler-generated field
      this.m_Dirty = true;
      return notificationInfo;
    }

    public void RemoveNotification(
      string identifier,
      float delay = 0.0f,
      LocalizedString? title = null,
      LocalizedString? text = null,
      string thumbnail = null,
      ProgressState? progressState = null,
      int? progress = null,
      Action onClicked = null)
    {
      // ISSUE: reference to a compiler-generated method
      // ISSUE: variable of a compiler-generated type
      NotificationUISystem.NotificationInfo notification = this.AddOrUpdateNotification(identifier, title, text, thumbnail, progressState, progress, onClicked);
      if ((double) delay == 0.0)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_NotificationsBinding.value.Remove(notification);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_NotificationsMap.Remove(notification.id);
      }
      else
      {
        // ISSUE: variable of a compiler-generated type
        NotificationUISystem.DelayedNotificationInfo notificationInfo;
        // ISSUE: reference to a compiler-generated field
        if (this.m_PendingRemoval.TryGetValue(identifier, out notificationInfo))
        {
          // ISSUE: reference to a compiler-generated method
          notificationInfo.Reset(delay);
        }
        else
        {
          // ISSUE: reference to a compiler-generated field
          // ISSUE: object of a compiler-generated type is created
          this.m_PendingRemoval.Add(identifier, new NotificationUISystem.DelayedNotificationInfo(notification, delay));
        }
      }
      // ISSUE: reference to a compiler-generated field
      this.m_Dirty = true;
    }

    public bool NotificationExists(string identifier)
    {
      // ISSUE: reference to a compiler-generated field
      return this.m_NotificationsMap.ContainsKey(identifier);
    }

    private void ProcessPendingRemovals(float deltaTime)
    {
      List<NotificationUISystem.NotificationInfo> notificationInfoList = (List<NotificationUISystem.NotificationInfo>) null;
      // ISSUE: reference to a compiler-generated field
      foreach (KeyValuePair<string, NotificationUISystem.DelayedNotificationInfo> keyValuePair in this.m_PendingRemoval)
      {
        // ISSUE: variable of a compiler-generated type
        NotificationUISystem.NotificationInfo notification;
        // ISSUE: reference to a compiler-generated method
        if (keyValuePair.Value.Update(deltaTime, out notification))
        {
          notificationInfoList = new List<NotificationUISystem.NotificationInfo>();
          notificationInfoList.Add(notification);
          // ISSUE: reference to a compiler-generated field
          this.m_Dirty = true;
        }
      }
      if (notificationInfoList == null)
        return;
      foreach (NotificationUISystem.NotificationInfo notificationInfo in notificationInfoList)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_NotificationsBinding.value.Remove(notificationInfo);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_NotificationsMap.Remove(notificationInfo.id);
        // ISSUE: reference to a compiler-generated field
        // ISSUE: reference to a compiler-generated field
        this.m_PendingRemoval.Remove(notificationInfo.id);
      }
    }

    [Preserve]
    public NotificationUISystem()
    {
    }

    private class DelayedNotificationInfo
    {
      private NotificationUISystem.NotificationInfo m_Notification;
      private float m_Delay;

      public DelayedNotificationInfo(
        NotificationUISystem.NotificationInfo notification,
        float delay)
      {
        // ISSUE: reference to a compiler-generated field
        this.m_Notification = notification;
        // ISSUE: reference to a compiler-generated field
        this.m_Delay = delay;
      }

      public void Reset(float delay) => this.m_Delay = delay;

      public bool Update(
        float deltaTime,
        out NotificationUISystem.NotificationInfo notification)
      {
        notification = (NotificationUISystem.NotificationInfo) null;
        // ISSUE: reference to a compiler-generated field
        this.m_Delay -= deltaTime;
        // ISSUE: reference to a compiler-generated field
        if ((double) this.m_Delay > 0.0)
          return false;
        // ISSUE: reference to a compiler-generated field
        notification = this.m_Notification;
        return true;
      }
    }

    public class NotificationInfo : IJsonWritable
    {
      public readonly string id;
      [CanBeNull]
      public string thumbnail;
      [CanBeNull]
      public LocalizedString? title;
      [CanBeNull]
      public LocalizedString? text;
      public ProgressState progressState;
      public int progress;
      public Action onClicked;

      public NotificationInfo(string id) => this.id = id;

      public void Write(IJsonWriter writer)
      {
        writer.TypeBegin(this.GetType().FullName);
        writer.PropertyName("id");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.id);
        writer.PropertyName("thumbnail");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.thumbnail);
        writer.PropertyName("title");
        // ISSUE: reference to a compiler-generated field
        writer.Write<LocalizedString>(this.title);
        writer.PropertyName("text");
        // ISSUE: reference to a compiler-generated field
        writer.Write<LocalizedString>(this.text);
        writer.PropertyName("progressState");
        // ISSUE: reference to a compiler-generated field
        writer.Write((int) this.progressState);
        writer.PropertyName("progress");
        // ISSUE: reference to a compiler-generated field
        writer.Write(this.progress);
        writer.TypeEnd();
      }
    }
  }
}
