// Decompiled with JetBrains decompiler
// Type: Game.UI.ErrorDialogManager
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Colossal;
using Colossal.Annotations;
using Colossal.Logging;
using Colossal.Logging.Diagnostics;
using Game.SceneFlow;
using Game.Simulation;
using Game.UI.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Entities;
using UnityEngine;

#nullable disable
namespace Game.UI
{
  public static class ErrorDialogManager
  {
    private static bool m_Initialized;
    private static bool m_Enabled = true;
    private static Queue<ErrorDialog> m_ErrorDialogs = new Queue<ErrorDialog>();
    private static float m_SimulationSpeed;

    public static bool enabled
    {
      get => ErrorDialogManager.m_Enabled;
      set
      {
        if (ErrorDialogManager.m_Enabled == value)
          return;
        ErrorDialogManager.m_Enabled = value;
        if (ErrorDialogManager.m_Enabled)
          return;
        ErrorDialogManager.Clear();
      }
    }

    public static bool TryGetFirstError(out string error)
    {
      if (ErrorDialogManager.m_ErrorDialogs.Count > 0)
      {
        ErrorDialog errorDialog = ErrorDialogManager.m_ErrorDialogs.First<ErrorDialog>();
        error = string.Format("{0}\n{1}", (object) errorDialog.localizedMessage, (object) errorDialog.errorDetails);
        return true;
      }
      error = (string) null;
      return false;
    }

    public static void DismissAllErrors()
    {
      while (ErrorDialogManager.m_ErrorDialogs.Count > 0)
        ErrorDialogManager.DismissCurrentErrorDialog();
    }

    public static void Initialize()
    {
      if (ErrorDialogManager.m_Initialized)
        return;
      UnityLogger.OnException += new Action<Exception, UnityEngine.Object>(ErrorDialogManager.OnException);
      UnityLogger.OnWarnOrHigher += new Action<ILog, Level, string, Exception, UnityEngine.Object>(ErrorDialogManager.OnWarnOrHigher);
      ErrorDialogManager.m_Initialized = true;
    }

    public static void Clear()
    {
      ErrorDialogManager.m_ErrorDialogs.Clear();
      ErrorDialogManager.m_SimulationSpeed = 0.0f;
    }

    public static void Dispose()
    {
      if (!ErrorDialogManager.m_Initialized)
        return;
      ErrorDialogManager.Clear();
      UnityLogger.OnException -= new Action<Exception, UnityEngine.Object>(ErrorDialogManager.OnException);
      UnityLogger.OnWarnOrHigher -= new Action<ILog, Level, string, Exception, UnityEngine.Object>(ErrorDialogManager.OnWarnOrHigher);
      ErrorDialogManager.m_Initialized = false;
    }

    private static void OnException(Exception e, UnityEngine.Object context)
    {
      ErrorDialogManager.ShowErrorDialog(new ErrorDialog()
      {
        severity = ErrorDialog.Severity.Error,
        localizedMessage = LocalizedString.Value(e.Message),
        errorDetails = ErrorDialogManager.GetErrorDetail(e, context),
        actions = ErrorDialogManager.GetActions()
      });
    }

    private static string GetErrorDetail(Exception e, UnityEngine.Object context)
    {
      StringBuilder sb = new StringBuilder();
      if (context != (UnityEngine.Object) null)
      {
        string str1 = string.Format("{0} ({1})", (object) context.name, (object) context.GetType());
        string str2 = context.ToString();
        sb.AppendFormat("With object {0}", (object) str1);
        sb.AppendLine();
        if (str1 != str2)
        {
          sb.AppendFormat("Additional info: {0}", (object) str2);
          sb.AppendLine();
        }
        sb.AppendLine();
      }
      if (e == null)
        return StackTraceHelper.ExtractStackTrace(3, sb: sb);
      StackTraceHelper.ExtractStackTraceFromException(e, sb);
      return StackTraceHelper.ExtractStackTrace(3, sb: sb);
    }

    private static void OnWarnOrHigher(
      ILog log,
      Level level,
      string message,
      Exception e,
      UnityEngine.Object context)
    {
      if (log != null && !log.showsErrorsInUI || !(level >= Level.Error))
        return;
      ErrorDialogManager.ShowErrorDialog(new ErrorDialog()
      {
        severity = level == Level.Warn ? ErrorDialog.Severity.Warning : ErrorDialog.Severity.Error,
        localizedMessage = LocalizedString.Value(message),
        errorDetails = ErrorDialogManager.GetErrorDetail(e, context),
        actions = ErrorDialogManager.GetActions()
      });
    }

    private static ErrorDialog.Actions GetActions()
    {
      if ((UnityEngine.Object) GameManager.instance == (UnityEngine.Object) null)
        return ErrorDialog.Actions.Quit;
      return Platform.PlayStation.IsPlatformSet(Application.platform) ? (!GameManager.instance.gameMode.IsGameOrEditor() ? ErrorDialog.Actions.None : ErrorDialog.Actions.SaveAndContinue) : (!GameManager.instance.gameMode.IsGameOrEditor() ? ErrorDialog.Actions.Quit : ErrorDialog.Actions.Default);
    }

    [CanBeNull]
    public static ErrorDialog currentErrorDialog
    {
      get
      {
        return ErrorDialogManager.m_ErrorDialogs.Count == 0 ? (ErrorDialog) null : ErrorDialogManager.m_ErrorDialogs.Peek();
      }
    }

    public static void DismissCurrentErrorDialog()
    {
      if (ErrorDialogManager.m_ErrorDialogs.Count > 0)
        ErrorDialogManager.m_ErrorDialogs.Dequeue();
      ErrorDialogManager.RestorePause();
    }

    private static void HandlePause()
    {
      // ISSUE: variable of a compiler-generated type
      SimulationSystem existingSystemManaged = World.DefaultGameObjectInjectionWorld?.GetExistingSystemManaged<SimulationSystem>();
      if (existingSystemManaged == null)
        return;
      if ((double) ErrorDialogManager.m_SimulationSpeed == 0.0)
        ErrorDialogManager.m_SimulationSpeed = existingSystemManaged.selectedSpeed;
      existingSystemManaged.selectedSpeed = 0.0f;
    }

    private static void RestorePause()
    {
      if (ErrorDialogManager.m_ErrorDialogs.Count != 0)
        return;
      // ISSUE: variable of a compiler-generated type
      SimulationSystem existingSystemManaged = World.DefaultGameObjectInjectionWorld?.GetExistingSystemManaged<SimulationSystem>();
      if (existingSystemManaged == null)
        return;
      existingSystemManaged.selectedSpeed = ErrorDialogManager.m_SimulationSpeed;
    }

    public static void DisplayDebugErrorDialog()
    {
      ErrorDialogManager.ShowErrorDialog(new ErrorDialog()
      {
        severity = ErrorDialog.Severity.Error,
        localizedMessage = (LocalizedString) "Debug Error",
        errorDetails = "Debug details",
        actions = ErrorDialogManager.GetActions()
      });
    }

    public static void ShowErrorDialog(ErrorDialog e)
    {
      if (!ErrorDialogManager.m_Enabled)
        return;
      ErrorDialogManager.HandlePause();
      ErrorDialogManager.m_ErrorDialogs.Enqueue(e);
    }
  }
}
