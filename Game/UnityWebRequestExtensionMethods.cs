// Decompiled with JetBrains decompiler
// Type: Game.UnityWebRequestExtensionMethods
// Assembly: Game, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6E65E546-90EB-41EE-A5F5-E22CC56BB1AC
// Assembly location: C:\Program Files (x86)\Steam\steamapps\common\Cities Skylines II\Cities2_Data\Managed\Game.dll

using Game.SceneFlow;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.Networking;

#nullable disable
namespace Game
{
  public static class UnityWebRequestExtensionMethods
  {
    public static UnityWebRequestExtensionMethods.UnityWebRequestAwaiter GetAwaiter(
      this UnityWebRequestAsyncOperation asyncOp)
    {
      return new UnityWebRequestExtensionMethods.UnityWebRequestAwaiter(asyncOp);
    }

    public static UnityWebRequestExtensionMethods.UnityWebRequestAwaiter ConfigureAwait(
      this UnityWebRequestAsyncOperation asyncOperation,
      Func<UnityWebRequestAsyncOperation, bool> updaterMethod,
      CancellationToken token,
      float connectionTimeout = 0.0f)
    {
      float progress = 0.0f;
      float time = Time.realtimeSinceStartup;
      GameManager.instance.RegisterUpdater((Func<bool>) (() =>
      {
        if (token.IsCancellationRequested)
        {
          asyncOperation.webRequest.Abort();
          return true;
        }
        if ((double) connectionTimeout > 0.0)
        {
          if ((double) asyncOperation.progress > (double) progress)
          {
            progress = asyncOperation.progress;
            time = Time.realtimeSinceStartup;
          }
          else if ((double) Time.realtimeSinceStartup - (double) time > (double) connectionTimeout)
          {
            asyncOperation.webRequest.Abort();
            return true;
          }
        }
        return updaterMethod(asyncOperation);
      }));
      return new UnityWebRequestExtensionMethods.UnityWebRequestAwaiter(asyncOperation);
    }

    public class UnityWebRequestAwaiter : INotifyCompletion
    {
      private UnityWebRequestAsyncOperation asyncOp;
      private Action continuation;

      public UnityWebRequestAwaiter(UnityWebRequestAsyncOperation asyncOp)
      {
        this.asyncOp = asyncOp;
        this.asyncOp.completed += new Action<AsyncOperation>(this.OnRequestCompleted);
      }

      public bool IsCompleted => this.asyncOp.isDone;

      public UnityWebRequest.Result GetResult() => this.asyncOp.webRequest.result;

      public void OnCompleted(Action continuation)
      {
        this.continuation = continuation;
        if (!this.IsCompleted)
          return;
        this.OnRequestCompleted((AsyncOperation) this.asyncOp);
      }

      public void OnRequestCompleted(AsyncOperation obj)
      {
        Action continuation = this.continuation;
        if (continuation == null)
          return;
        continuation();
      }

      public UnityWebRequestExtensionMethods.UnityWebRequestAwaiter GetAwaiter() => this;
    }
  }
}
